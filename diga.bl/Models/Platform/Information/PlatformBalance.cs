using diga.bl.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace diga.bl.Models.Platform.Information
{
    public class PlatformBalance : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }

        public double Value { get; set; }
    }
}
