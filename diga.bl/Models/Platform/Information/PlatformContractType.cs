using diga.bl.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace diga.bl.Models.Platform.Information
{
    public class PlatformContractType : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
