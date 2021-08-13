using Diga.Parsing.Models.Estimates;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Diga.Parsing.Parsers
{
    public class EstimateParser
    {
        private readonly FileStream _stream;

        public EstimateParser(FileStream stream)
        {
            _stream = stream;
        }

        private string GetFullNumber(string sectionNumber, string subSectionNumber)
        {
            return $"{sectionNumber}.{subSectionNumber}";
        }

        private string GetParentNumber(string number)
        {
            List<string> temp = number.Split(".").ToList();
            temp.RemoveAt(temp.Count - 1);
            return string.Join('.', temp);
        }

        private EstimateSectionDto GetParentSection(EstimateDto estimate, string number)
        {
            return estimate.Sections.FirstOrDefault(s => s.Number == GetParentNumber(number));
        }

        private string IncrementLast(string number)
        {
            List<string> temp = number.Split(".").ToList();
            temp[^1] = (int.Parse(temp[^1]) + 1).ToString();
            return string.Join('.', temp);
        }

        private EstimateDto SectionRecalculate(EstimateDto estimate, EstimateSectionDto section)
        {
            int counter = 1;
            List<EstimateSectionDto> sections = estimate.Sections.Where(s => s.ParentSection == section).ToList();
            foreach (EstimateSectionDto innerSection in sections)
            {
                innerSection.Number = $"{section.Number}.{counter}";
                estimate = SectionRecalculate(estimate, innerSection);

                counter++;
            }

            return estimate;
        }

        private EstimateDto NumberRecalculate(EstimateDto estimate)
        {
            List<EstimateSectionDto> sections = estimate.Sections.Where(s => s.ParentSection == null).ToList();
            foreach (EstimateSectionDto section in sections)
            {
                estimate = SectionRecalculate(estimate, section);
            }

            return estimate;
        }

        private int GetLastChildNumber(EstimateDto estimate, string sectionNumber)
        {
            string[] temp = sectionNumber.Split(".");
            List<EstimateSectionDto> childSections = estimate.Sections.Where(s => s.Number.Count(n => n == '.') == temp.Length).ToList();

            return childSections.Count == 0 ? 0 : childSections.Max(s => int.Parse(s.Number.Split(".").Last()));
        }

        private EstimateDto PositionNumberRecalculate(EstimateDto estimate)
        {
            foreach (EstimateSectionDto section in estimate.Sections)
            {
                int lastChildNumber = GetLastChildNumber(estimate, section.Number);
                foreach (EstimatePositionDto position in estimate.Positions.Where(p => p.Section == section))
                {
                    position.Number = $"{section.Number}.{++lastChildNumber}";
                }
            }

            return estimate;
        }

        public EstimateDto ParseEstimate()
        {
            EstimateDto estimate = new EstimateDto { };

            try
            {
                List<List<string>> rows;

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                // Excel Reader
                using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(_stream))
                {
                    DataSet dataSet = reader.AsDataSet();
                    rows = dataSet.Tables[0].AsEnumerable().ToList()
                        .Select(r => r.ItemArray.Select(i => Convert.ToString(i, new CultureInfo("en-US"))).ToList()).ToList();
                }

                rows.RemoveAll(r => r.All(i => string.IsNullOrEmpty(i)));
                rows = rows.Skip(1).ToList();

                EstimateSectionDto currentSection = null;

                foreach (List<string> cols in rows)
                {
                    string sectionNumber = cols[0].Trim('.');
                    string subSectionNumber = cols[1].Trim('.');

                    if (subSectionNumber.Any(x => char.IsLetter(x)))
                    {
                        currentSection = new EstimateSectionDto { Name = cols[1], Number = sectionNumber };
                        estimate.Sections.Add(currentSection);
                    }
                    else if (!string.IsNullOrEmpty(sectionNumber) && !string.IsNullOrEmpty(subSectionNumber)
                        && string.IsNullOrEmpty(cols[3]) && string.IsNullOrEmpty(cols[4]))
                    {
                        string fullNumber = GetFullNumber(sectionNumber, subSectionNumber);
                        EstimateSectionDto parentSection = GetParentSection(estimate, fullNumber);

                        if (parentSection == null)
                        {
                            throw new Exception($"Cannot find parent category {fullNumber}!");
                        }

                        EstimateSectionDto subSection = new EstimateSectionDto
                        {
                            Name = cols[2],
                            Number = fullNumber,
                            ParentSection = parentSection
                        };

                        estimate.Sections.Add(subSection);
                        currentSection = subSection;
                    }
                    else
                    {
                        string fullNumber = GetFullNumber(sectionNumber, subSectionNumber);
                        EstimatePositionDto position = new EstimatePositionDto
                        {
                            Description = cols[2],
                            Measurement = cols[3],
                            Section = currentSection
                        };

                        if (double.TryParse(cols[4].Contains('.') ? cols[4].Replace(",", "") : cols[4].Replace(",", "."),
                                NumberStyles.Any, new CultureInfo("en-US"), out double quantity))
                            position.Quantity = quantity;
                        else
                            position.Quantity = 0;

                        estimate.Positions.Add(position);
                    }
                }

                estimate = NumberRecalculate(estimate);
                estimate = PositionNumberRecalculate(estimate);
            }
            catch
            {
                estimate.FileName = "Failed";
            }

            return estimate;
        }
    }
}
