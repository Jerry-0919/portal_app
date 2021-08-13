using diga.dal;
using diga.web.Helpers;
using diga.web.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : BaseController
    {
        public ProfileController(DigaContext db, IConfiguration config) : base(db, config)
        {
        }

        [HttpGet("balance")]
        [Authorize(AuthenticationSchemes =
    JwtBearerDefaults.AuthenticationScheme, Roles = "Erp")]
        public async Task<IActionResult> Balance()
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == UserId);
            return Ok(new
            {
                balance = user.Balance,
                token = user.AuthToken
            });
        }

        [HttpGet("subscriptions")]
        [Authorize(AuthenticationSchemes =
    JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Subscriptions()
        {
            var userTariffs = await _db.UserTariffs.
                Include(ut => ut.Tariff).ThenInclude(t => t.TariffModules).ThenInclude(tm => tm.Module).
                Where(ut => ut.UserId == UserId).ToListAsync();
            var userModules = await _db.UserModules.Include(um => um.Module).Where(um => um.UserId == UserId).ToListAsync();

            return Ok(new
            {
                userTariffs = userTariffs,
                userModules = userModules,
            });
        }

        [HttpPost("me")]
        [Authorize(AuthenticationSchemes =
    JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostUser(UserUpdateModel model)
        {
            var user = await _db.Users
                .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == UserId);

            var passwordHelper = new PasswordHelper();

            if (!string.IsNullOrEmpty(model.OldPassword)
                && !string.IsNullOrEmpty(model.NewPassword)
                && !string.IsNullOrEmpty(model.ConfirmNewPassword))
            {
                if (passwordHelper.Compare(model.OldPassword, user.PasswordHash) == false)
                {
                    return BadRequest();
                }

                if (model.NewPassword != model.ConfirmNewPassword)
                {
                    return BadRequest();
                }

                user.PasswordHash = passwordHelper.Generate(model.NewPassword);
            }
            user.Name = model.Name;
            user.PhoneNumber = model.Phone;
            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("me")]
        [Authorize(AuthenticationSchemes =
    JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Me()
        {
            var user = await _db.Users
                .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == UserId);

            return Ok(new
            {
                username = user.Email,
                id = user.Id,
                roles = user.UserRoles.Select(ur => ur.Role.Name).ToList(),
                name = user.Name,
                phone = user.PhoneNumber,
                balance = user.Balance,
                domain = user.Domain
            });
        }
    }
}