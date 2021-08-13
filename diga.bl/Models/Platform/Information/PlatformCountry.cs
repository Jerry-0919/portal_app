using diga.bl.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace diga.bl.Models.Platform.Information
{
    public class PlatformCountry : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
