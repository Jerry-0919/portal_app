using Diga.Parsing.Models.Estimates;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Diga.Parsing.Builders
{
    public class EstimateBuilder
    {
        private readonly EstimateBuildDto _estimate;
        private readonly string _savePath;
        private List<List<object>> rows;

        public EstimateBuilder(EstimateBuildDto estimate, string savePath)
        {
            _estimate = estimate;
            _savePath = savePath;
        }

        private string GetFullNumber(EstimateSectionBuildDto section)
        {
            if (section == null)
                return null;

            string result = section.Number.ToString();
            string innerResult = GetFullNumber(_estimate.Sections.FirstOrDefault(s => s.Id == section.ParentSectionId));

            if (string.IsNullOrEmpty(innerResult))
                return result;

            return $"{innerResult}.{result}";
        }

        private void AppendSection(EstimateSectionBuildDto section)
        {
            string sectionFullNumber = GetFullNumber(section);
            rows.Add(new List<object> { sectionFullNumber, section.Name });

            foreach (EstimatePositionBuildDto innerPosition in _estimate.Positions.Where(p => p.SectionId == section.Id))
            {
                rows.Add(new List<object>
                {
                    sectionFullNumber,
                    innerPosition.Number,
                    innerPosition.Description,
                    innerPosition.Measurement,
                    innerPosition.Quantity
                });
            }

            foreach (EstimateSectionBuildDto innerSection in _estimate.Sections.Where(s => s.ParentSectionId == section.Id))
            {
                AppendSection(innerSection);
            }
        }

        public void BuildEstimate()
        {
            rows = new List<List<object>> { };

            using (SpreadsheetDocument document = SpreadsheetDocument.Create(_savePath, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                SheetData sheetData = new SheetData();
                worksheetPart.Worksheet = new Worksheet(sheetData);

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Estimate" };

                // Add headers
                Row newRow = new Row();
                foreach (string col in new string[] { "Especialidade", "Art.º", "Designação", "Un", "Quant.", "Valor Unitário", "Valor Parcial", "Valor Total Capitulo" })
                {
                    newRow.AppendChild(new Cell
                    {
                        DataType = CellValues.String,
                        CellValue = new CellValue(col)
                    });
                }

                sheetData.AppendChild(newRow);

                // Add rows
                foreach (EstimateSectionBuildDto section in _estimate.Sections.Where(s => !s.ParentSectionId.HasValue))
                    AppendSection(section);

                foreach (List<object> row in rows)
                {
                    newRow = new Row();
                    foreach (object col in row)
                    {
                        Type type = col.GetType();
                        Cell cell;

                        if (type == typeof(double))
                        {
                            cell = new Cell
                            {
                                DataType = CellValues.Number,
                                CellValue = new CellValue((double)col)
                            };
                        }
                        else
                        {
                            cell = new Cell
                            {
                                DataType = CellValues.String,
                                CellValue = new CellValue(col.ToString())
                            };
                        }

                        newRow.AppendChild(cell);
                    }

                    sheetData.AppendChild(newRow);
                }

                sheets.Append(sheet);

                workbookPart.Workbook.Save();
            }
        }
    }
}
