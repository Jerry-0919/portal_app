using diga.web.Helpers;
using diga.web.Models.Auth0;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class Auth0Controller : ControllerBase
    {
        IConfiguration _config;
        public Auth0Controller(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("{sub}/roles")]
        public List<UserAuth0RoleDto> Post(string sub)
        {
            var auth0Helper = new Auth0Helper(_config);
            return auth0Helper.UserRoles(sub);
        }
    }
}
