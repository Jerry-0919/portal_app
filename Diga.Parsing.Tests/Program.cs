using Diga.Parsing.Builders;
using Diga.Parsing.Models.Estimates;
using Diga.Parsing.Parsers;
using System;
using System.Collections.Generic;
using System.IO;

namespace Diga.Parsing.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            // Parsing
            //EstimateParser parser = new EstimateParser(new FileStream("298PE01_MT02_out_2020.xls", FileMode.Open, FileAccess.Read));
            EstimateParser parser = new EstimateParser(new FileStream(@"C:\Users\VodemSharp\Desktop\estimate short.xlsx", FileMode.Open, FileAccess.Read));
            EstimateDto result = parser.ParseEstimate();

            // Building
            EstimateBuilder builder = new EstimateBuilder(
                new EstimateBuildDto
                {
                    Sections = new List<EstimateSectionBuildDto>
                    {
                        new EstimateSectionBuildDto { Id = 1, Name = "Section 1", Number = 1, ParentSectionId = null },
                        new EstimateSectionBuildDto { Id = 2, Name = "Section 1.3", Number = 3, ParentSectionId = 1 },
                        new EstimateSectionBuildDto { Id = 3, Name = "Section 1.3.1", Number = 1, ParentSectionId = 2 },
                        new EstimateSectionBuildDto { Id = 4, Name = "Section 1.3.2", Number = 2, ParentSectionId = 2 },
                        new EstimateSectionBuildDto { Id = 5, Name = "Section 1.4", Number = 4, ParentSectionId = 1 },
                    },
                    Positions = new List<EstimatePositionBuildDto>
                    {
                        new EstimatePositionBuildDto { Id = 1, Description = "Position 1.1", Measurement = "kg", Quantity = 1, Number = 1, SectionId = 1 },
                        new EstimatePositionBuildDto { Id = 1, Description = "Position 1.2", Measurement = "kg", Quantity = 1, Number = 2, SectionId = 1 },
                        new EstimatePositionBuildDto { Id = 1, Description = "Position 1.3.1.1", Measurement = "kg", Quantity = 2, Number = 1, SectionId = 3 },
                        new EstimatePositionBuildDto { Id = 1, Description = "Position 1.3.1.2", Measurement = "kg", Quantity = 3, Number = 2, SectionId = 3 },
                    }
                },
                $"{Guid.NewGuid()}.xlsx");

            builder.BuildEstimate();
        }
    }
}
