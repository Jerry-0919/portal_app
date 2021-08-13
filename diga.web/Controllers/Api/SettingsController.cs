using diga.dal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace diga.web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : BaseController
    {
        public SettingsController(DigaContext db, IConfiguration config) : base(db, config)
        {
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var setting = _db.Settings.FirstOrDefault(s => s.Name == name);
            return Ok(setting.Value);
        }
    }
}
