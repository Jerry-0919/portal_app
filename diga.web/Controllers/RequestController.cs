using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CloudFlare.Client;
using diga.web.Helpers;
using diga.bl.Models;
using diga.dal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace diga.web.Controllers
{
    public class RequestController : ControllerBase
    {
        private DigaContext DigaContext { get; set; }
        private HttpClientHandler Handler { get; set; }
        private HttpClient HttpClient { get; set; }
        public ILanguageStorage LanguageStorage { get; set; }
        private readonly SaasHelper _saasHelper;

        public RequestController(DigaContext context, ILanguageStorage languageStorage, SaasHelper saasHelper)
        {
            this.DigaContext = context;
            Handler = new HttpClientHandler();
            HttpClient = new HttpClient(Handler);
            this.LanguageStorage = languageStorage;
            _saasHelper = saasHelper;
        }
        [HttpPost]
        public IActionResult Post(Request model)
        {
            model.DateCreated = DateTime.Now;
            DigaContext.Requests.Add(model);
            DigaContext.SaveChanges();
            return RedirectToAction("FormSubmitSuccess", "Message");
        }

        [HttpPost("external_trial")]
        public async Task<IActionResult> PostExternalTrial([FromBody] TrialRequest model)
        {
            if (DigaContext.Users.Any(u => u.Email == model.Email))
            {
                return BadRequest("Email is already in use");
            }
            var toSave = new Request
            {
                DateCreated = DateTime.Now,
                Name = model.Name,
                Email = model.Email,
                Telephone = model.Telephone,
                Type = $"Trial {model.Domain} {model.Language}"
            };
            DigaContext.Requests.Add(toSave);
            await DigaContext.SaveChangesAsync();

            var newUser = new ApplicationUser
            {
                Email = model.Email,
                Name = model.Name,
                PhoneNumber = model.Telephone,
                Balance = 0,
                Language = model.Language,
                Domain = model.Domain,
                AuthToken = Guid.NewGuid().ToString(),
            };

            var passwordHelper = new PasswordHelper();
            newUser.PasswordHash = passwordHelper.Generate(model.Password);

            ApplicationRole userRole = await DigaContext.Roles.FirstOrDefaultAsync(r => r.Name == "Erp");
            if (userRole != null)
            {
                newUser.UserRoles.Add(new ApplicationUserRole { RoleId = userRole.Id });
            }
            DigaContext.Users.Add(newUser);
            await DigaContext.SaveChangesAsync();

            Packets packet = await DigaContext.Packets.FirstOrDefaultAsync(p => p.Token == model.PacketToken);
            Tariffs tariff = null;
            int numberOfUsers = 0;
            string moduleNames = "";
            int days = 0;

            if (packet != null)
            {
                tariff = await DigaContext.Tariffs.Include(t => t.TariffModules).ThenInclude(tm => tm.Module).FirstOrDefaultAsync(t => t.Name == packet.Tariff);
                var newUserTariff = new UserTariffs
                {
                    UserId = newUser.Id,
                    TariffId = tariff.Id,
                    Start = DateTime.Now,
                    End = packet.Days > 0 ? DateTime.Now.AddDays(packet.Days.Value) : DateTime.Now.AddDays(packet.TrialDays.Value),
                    IsTrial = packet.TrialDays > 0,
                    CurrentPrice = packet.TariffPrice,
                    PriceForNextUser = packet.PriceForNextUser,
                    NumberOfUsers = packet.NumberOfUsers
                };
                DigaContext.UserTariffs.Add(newUserTariff);
                numberOfUsers = newUserTariff.NumberOfUsers.Value;
                moduleNames = string.Join(",", tariff.TariffModules.Select(tm => tm.Module.Name).ToList());
                days = packet.Days.Value;

                if (packet.PacketModules != null && packet.PacketModules.Count > 0)
                {
                    foreach (var pm in packet.PacketModules)
                    {
                        var userModule = new UserModules
                        {
                            UserId = newUser.Id,
                            ModuleId = pm.ModuleId,
                            Start = DateTime.Now,
                            End = packet.Days > 0 ? DateTime.Now.AddDays(packet.Days.Value) : DateTime.Now.AddDays(packet.TrialDays.Value),
                            IsTrial = packet.TrialDays > 0,
                            CurrentPrice = pm.Price
                        };
                        DigaContext.UserModules.Add(userModule);
                    }
                    moduleNames += "," + string.Join(",", packet.PacketModules.Select(pm => pm.Module.Name).ToList());
                }
            }
            else
            {
                tariff = await DigaContext.Tariffs.Include(t => t.TariffModules).ThenInclude(tm => tm.Module).FirstOrDefaultAsync(t => t.Name == "base");
                var newUserTariff = new UserTariffs
                {
                    UserId = newUser.Id,
                    TariffId = tariff.Id,
                    Start = DateTime.Now,
                    End = DateTime.Now.AddYears(5),
                    IsTrial = false,
                    CurrentPrice = tariff.Price,
                    PriceForNextUser = tariff.PriceForNextUser,
                    NumberOfUsers = tariff.NumberOfUsers
                };
                DigaContext.UserTariffs.Add(newUserTariff);
                numberOfUsers = newUserTariff.NumberOfUsers.Value;
                moduleNames = string.Join(",", tariff.TariffModules.Select(tm => tm.Module.Name).ToList());
                days = tariff.Days.Value;
            }

            await DigaContext.SaveChangesAsync();

            if (await _saasHelper.CreateInstance(
                model.Name,
                model.Email,
                model.Password,
                model.Telephone.Replace(" ", "").Replace("(", "").Replace(")", "").Replace("_", ""),
                model.Domain,
                model.Language,
                model.Promocode,
                numberOfUsers,
                packet != null ? packet.Tariff : "base",
                days,
                packet != null ? packet.TrialDays.Value : 0,
                moduleNames,
                newUser.AuthToken
            ) == false)
            {
                return BadRequest("Erp generating problem");
            }

            return Ok("success");
        }

        [HttpGet]
        public async Task<bool> CheckDomain(string domain)
        {
            using (var client = new CloudFlareClient("it.rkesa@gmail.com", "79f048c56535996fa97c9e6ac1fea7f85aaff"))
            {
                var zones = await client.GetZonesAsync();
                foreach (var zone in zones.Result)
                {
                    if (zone.Name == "diga.pt")
                    {
                        var dnsRecordQueryResult = await client.GetDnsRecordsAsync(zone.Id, null, "", "", 1, 1000);
                        foreach (var dnsRecord in dnsRecordQueryResult.Result)
                        {
                            if (dnsRecord.Name == $"{domain.ToLower().Trim()}.diga.pt")
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }
    }
}
