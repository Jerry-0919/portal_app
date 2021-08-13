using diga.bl.Models.Platform.Estimates;
using diga.web.Models.PlatformEstimates;
using System;
using System.Collections.Generic;
using System.Linq;

namespace diga.web.Helpers
{
    public static class PlatformEstimateHelper
    {
        private static List<PlatformEstimateSectionDto> ParseSections(List<PlatformEstimateSection> sections,
            List<PlatformEstimatePosition> positions, bool isMeasurementReport, int measurementReportId, int? parentSectionId = null)
        {
            List<PlatformEstimateSectionDto> result = new List<PlatformEstimateSectionDto> { };
            foreach (PlatformEstimateSection section in sections.OrderBy(s => s.Number).Where(s => s.ParentSectionId == parentSectionId))
            {
                result.Add(new PlatformEstimateSectionDto
                {
                    Id = section.Id,
                    Name = section.Name,
                    Number = section.Number,
                    Notes = section.Notes,
                    Sections = ParseSections(sections, positions, isMeasurementReport, measurementReportId, section.Id),
                    Positions = positions.OrderBy(p => p.Number).Where(p => p.EstimateSectionId == section.Id)
                        .Select(p => new PlatformEstimatePositionDto
                        {
                            Id = p.Id,
                            Description = p.Description,
                            Measurement = p.Measurement,
                            Number = p.Number,
                            Quantity = p.Quantity == 0 ? (double?)null : p.Quantity,
                            Price = p.Price == 0 ? (double?)null : p.Price,
                            Notes = p.Notes,
                            MeasurementReportPositions = p.PlatformMeasurementReportPositions.Select(mp => new Models.PlatformMeasurementReportPositions.PlatformMeasurementReportPositionDto
                            {
                                Id = mp.Id,
                                ReportId = mp.ReportId,
                                PercentCompleted = mp.PercentCompleted,
                                QuantityCompleted = mp.QuantityCompleted,
                                Status = mp.Status,
                                RejectionReason = mp.RejectionReason
                            }).ToList()
                        }).ToList()
                });
            }

            return result;
        }

        public static PlatformEstimateFullDto Parse(PlatformEstimate estimate,
            List<PlatformEstimateSection> sections, List<PlatformEstimatePosition> positions, bool isMeasurementReport = false, int measurementReportId = 0)
        {
            return new PlatformEstimateFullDto
            {
                Id = estimate.Id,
                AnotherPercent = estimate.AnotherPercent,
                Name = $"№ {estimate.PlatformContractId}-{estimate.Id}", //estimate.Name,
                VatType = estimate.VatType,
                Sections = ParseSections(sections, positions, isMeasurementReport, measurementReportId),
                PlatformEstimateVats = estimate.PlatformEstimateVATs?.OrderBy(v => v.PlatformVAT?.Percent).Select(v => new PlatformEstimateVatDto
                {
                    Percent = v.Percent,
                    PlatformEstimateId = v.PlatformEstimateId,
                    PlatformVATId = v.PlatformVATId
                }).ToList()
            };
        }

        private static string CreateNumber(string first, int second)
        {
            if (string.IsNullOrEmpty(first))
                return second.ToString();

            return $"{first}.{second}";
        }

        private static Tuple<List<PlatformEstimateSection>, List<PlatformEstimatePosition>> BuildSections(
            List<PlatformEstimateSectionDto> sections, int estimateId, string sectionNumber = "")
        {
            Tuple<List<PlatformEstimateSection>, List<PlatformEstimatePosition>> result =
                new Tuple<List<PlatformEstimateSection>, List<PlatformEstimatePosition>>(new List<PlatformEstimateSection> { }, new List<PlatformEstimatePosition> { });

            foreach (PlatformEstimateSectionDto section in sections)
            {
                PlatformEstimateSection estimateSection = new PlatformEstimateSection
                {
                    Name = section.Name,
                    Number = section.Number,
                    Notes = section.Notes,
                    EstimateId = estimateId,
                    FullNumber = CreateNumber(sectionNumber, section.Number)
                };

                result.Item1.Add(estimateSection);

                result.Item2.AddRange(section.Positions.Select(p => new PlatformEstimatePosition
                {
                    Description = p.Description,
                    Measurement = p.Measurement,
                    Number = p.Number,
                    Quantity = p.Quantity ?? 0,
                    Notes = p.Notes,
                    Price = p.Price ?? 0,
                    EstimateSection = estimateSection,
                    FullNumber = CreateNumber(estimateSection.FullNumber, p.Number)
                }).ToList());

                Tuple<List<PlatformEstimateSection>, List<PlatformEstimatePosition>> innerResult =
                    BuildSections(section.Sections, estimateId, CreateNumber(sectionNumber, section.Number));

                result.Item1.AddRange(innerResult.Item1);
                result.Item2.AddRange(innerResult.Item2);
            }

            return result;
        }

        public static Tuple<List<PlatformEstimateSection>, List<PlatformEstimatePosition>> Build(List<PlatformEstimateSectionDto> sections, int id)
        {
            return BuildSections(sections, id);
        }
    }
}
