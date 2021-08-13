using diga.bl.Models;
using diga.bl.Models.Platform.Information;
using System.Collections.Generic;
using System.Linq;

namespace diga.dal.Seeders
{
    public class DigaContextSeeder
    {
        private readonly DigaContext _db;

        public DigaContextSeeder(DigaContext db)
        {
            _db = db;
        }

        public void Seed()
        {
            AddRoles();
            AddCountries();
            AddCities();
            AddPriorities();
            AddVATs();
        }

        private void AddRoles()
        {
            List<string> roles = new List<string> {
                "Admin",
                "Partner",
                "Erp",
                "Platform",
                "PlatformCustomer",
                "PlatformExecutor",
                "PlatformCustomerIndividual",
                "PlatformCustomerTeam",
                "PlatformCustomerCompany",
                "PlatformExecutorIndividual",
                "PlatformExecutorMaster",
                "PlatformExecutorTeam",
                "PlatformExecutorCompany",
                "Sales",
                "Support",
                "SupervisorManager"
            };

            foreach (string role in roles)
            {
                if (!_db.Roles.Any(r => r.Name == role))
                {
                    _db.Roles.Add(new ApplicationRole { Name = role });
                    _db.SaveChanges();
                }
            }
        }

        private void AddCountries()
        {
            if (!_db.PlatformCountries.Any(r => r.Name == "Portugal"))
            {
                _db.PlatformCountries.Add(new PlatformCountry
                {
                    Name = "Portugal"
                });

                _db.SaveChanges();
            }
        }

        private void AddCities()
        {
            if (!_db.PlatformCities.Any(r => r.Name == "Lisbon"))
            {
                _db.PlatformCities.Add(new PlatformCity
                {
                    Name = "Lisbon",
                    CountryId = _db.PlatformCountries.First(c => c.Name == "Portugal").Id
                });

                _db.SaveChanges();
            }
        }

        private void AddPriorities()
        {
            List<string> priorities = new List<string> { "Normal", "Urgently", "Burning", "Considering offers" };

            foreach (string priority in priorities)
            {
                if (!_db.PlatformContractPriorities.Any(p => p.Value == priority))
                {
                    _db.PlatformContractPriorities.Add(new PlatformContractPriority
                    {
                        Value = priority
                    });

                    _db.SaveChanges();
                }
            }
        }

        private void AddVATs()
        {
            List<PlatformVAT> vats = new List<PlatformVAT> {
                new PlatformVAT { Code = "NOR", Name = "Taxa normal Continente", Percent = 23, RegionCode = "PT" },
                new PlatformVAT { Code = "NOR", Name = "Taxa normal Açores", Percent = 18, RegionCode = "PT-AC" },
                new PlatformVAT { Code = "NOR", Name = "Taxa normal Madeira", Percent = 22, RegionCode = "PT-MA" },
                new PlatformVAT { Code = "INT", Name = "Taxa intermédia Continente", Percent = 13, RegionCode = "PT" },
                new PlatformVAT { Code = "INT", Name = "Taxa intermédia Açores", Percent = 9, RegionCode = "PT-AC" },
                new PlatformVAT { Code = "INT", Name = "Taxa intermédia Madeira", Percent = 12, RegionCode = "PT-MA" },
                new PlatformVAT { Code = "RED", Name = "Taxa reduzida Continente", Percent = 6, RegionCode = "PT" },
                new PlatformVAT { Code = "RED", Name = "Taxa reduzida Açores", Percent = 4, RegionCode = "PT-AC" },
                new PlatformVAT { Code = "RED", Name = "Taxa reduzida Madeira", Percent = 5, RegionCode = "PT-MA" },
                new PlatformVAT { Code = "ISE", Name = "Regime de isenção de IVA", Percent = 0, RegionCode = "PT" },
                new PlatformVAT { Code = "ISE", Name = "Regime de isenção de IVA Açores", Percent = 0, RegionCode = "PT-AC" },
                new PlatformVAT { Code = "ISE", Name = "Regime de isenção de IVA Madeira", Percent = 0, RegionCode = "PT-MA" },
            };

            foreach (PlatformVAT vat in vats)
            {
                if (!_db.PlatformVATs.Any(v => v.Name == vat.Name))
                {
                    _db.PlatformVATs.Add(vat);
                    _db.SaveChanges();
                }
            }
        }
    }
}
