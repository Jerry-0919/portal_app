using diga.web.Models.Auth0;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Helpers
{
    public class Auth0Helper
    {
        IConfiguration _config;
        private ManagementAccessDto ManagementAccess { get; set; }
        public Auth0Helper(IConfiguration config)
        {
            _config = config;
            ManagementToken();
        }

        private void ManagementToken()
        {
            var client = new RestClient($"https://{_config.GetSection("Constants:AUTH0_DOMAIN").Value}/oauth/token");
            var request = new RestRequest(Method.POST)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddJsonBody(new {
                client_id = _config.GetSection("Constants:AUTH0_CLIENT_ID").Value,
                client_secret = _config.GetSection("Constants:AUTH0_CLIENT_SECRET").Value,
                audience = "https://diga.eu.auth0.com/api/v2/",
                grant_type = "client_credentials"
            });
            //request.AddParameter("application/x-www-form-urlencoded", $"grant_type=client_credentials&client_id=%24%7B{_config.GetSection("Constants:AUTH0_CLIENT_ID").Value}%7D&client_secret={_config.GetSection("Constants:AUTH0_CLIENT_SECRET").Value}&audience=https%3A%2F%2F%24%7Bdiga.eu.auth0.com%7D%2Fapi%2Fv2%2F", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            if (response.ResponseStatus == ResponseStatus.Error)
            {
                throw new Exception("Management token is not obtained");
            }
            this.ManagementAccess = JsonConvert.DeserializeObject<ManagementAccessDto>(response.Content);
        }

        public List<UserAuth0RoleDto> UserRoles(string sub)
        {
            if (this.ManagementAccess == null || string.IsNullOrEmpty(this.ManagementAccess.Access_token))
            {
                throw new Exception("Management token is empty");
            }
            var client = new RestClient($"https://{_config.GetSection("Constants:AUTH0_DOMAIN").Value}/api/v2/users/{sub}/roles");
            var request = new RestRequest(Method.GET);
            request.AddHeader("authorization", $"Bearer {ManagementAccess.Access_token}");
            IRestResponse response = client.Execute(request);
            if (response.ResponseStatus == ResponseStatus.Error)
            {
                throw new Exception("Roles failed");
            }
            return JsonConvert.DeserializeObject<List<UserAuth0RoleDto>>(response.Content);
        }
    }
}
