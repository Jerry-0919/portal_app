using diga.bl.Models.Platform.Estimates;
using System.Collections.Generic;
using System.Linq;

namespace diga.web.Models.PlatformEstimates
{
    public class PlatformEstimateEditDto
    {
        // public string Name { get; set; }
        //public int? PlatformVATId { get; set; }
        public double AnotherPercent { get; set; }
        public string VatType { get; set; }

        public List<PlatformEstimateSectionEditDto> SectionsToAdd { get; set; }
        public List<PlatformEstimateSectionEditDto> SectionsToUpdate { get; set; }
        public List<int> SectionIdsToRemove { get; set; }
        public List<PlatformEstimateVatDto> PlatformEstimateVats { get; set; }
        public List<PlatformEstimatePositionEditDto> PositionsToAdd { get; set; }
        public List<PlatformEstimatePositionEditDto> PositionsToUpdate { get; set; }
        public List<int> PositionIdsToRemove { get; set; }

        public void MapNewIds(PlatformEstimate oldEstimate, PlatformEstimate newEstimate)
        {
            // Sections
            foreach (PlatformEstimateSectionEditDto section in SectionsToUpdate)
            {
                PlatformEstimateSection newSection = newEstimate.Sections.First(s =>
                    s.GetFullNumber() == oldEstimate.Sections.First(s => s.Id == section.Id).GetFullNumber());

                section.Id = newSection.Id;
                section.ParentSectionId = newSection.ParentSectionId;
            }

            List<int> newSectionsForRemove = new List<int> { };
            foreach (int sectionId in SectionIdsToRemove)
            {
                PlatformEstimateSection newSection = newEstimate.Sections.First(s =>
                    s.GetFullNumber() == oldEstimate.Sections.First(s => s.Id == sectionId).GetFullNumber());
                newSectionsForRemove.Add(newSection.Id);
            }
            SectionIdsToRemove = newSectionsForRemove;

            // Positions
            foreach (PlatformEstimatePositionEditDto position in PositionsToUpdate)
            {
                PlatformEstimatePosition newPosition = newEstimate.Sections.SelectMany(s => s.Positions).First(s =>
                      s.GetFullNumber() == oldEstimate.Sections.SelectMany(s => s.Positions).First(s => s.Id == position.Id).GetFullNumber());

                position.Id = newPosition.Id;
                position.SectionId = newPosition.EstimateSectionId;
            }

            List<int> newPositionsForRemove = new List<int> { };
            foreach (int positionId in PositionIdsToRemove)
            {
                PlatformEstimatePosition newPosition = newEstimate.Sections.SelectMany(s => s.Positions).First(s =>
                      s.GetFullNumber() == oldEstimate.Sections.SelectMany(s => s.Positions).First(s => s.Id == positionId).GetFullNumber());
                newSectionsForRemove.Add(newPosition.Id);
            }
            PositionIdsToRemove = newPositionsForRemove;
        }
    }
}
