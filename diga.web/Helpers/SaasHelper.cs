using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Text;
using diga.web.Options;
using Microsoft.Extensions.Options;

namespace diga.web.Helpers
{  

    public class SaasHelper
    {
        private readonly SaasOptions _saasOptions;
        private HttpClientHandler Handler { get; set; }
        private HttpClient HttpClient { get; set; }
        
        public SaasHelper(IOptions<SaasOptions> saasOptions)
        {
            _saasOptions = saasOptions.Value;

            Handler = new HttpClientHandler();
            HttpClient = new HttpClient(Handler);
        }
        public async Task<bool> CreateInstance(
            string name,
            string email,
            string password,
            string telephone,
            string domain,
            string language,
            string promocode,
            int numberOfUsers,
            string tariff,
            int days,
            int trialDays,
            string modules,
            string authToken
        ) 
        {
            string payload = JsonConvert.SerializeObject(new
            {
                grant_type = "client_credentials",
                client_id = $"{_saasOptions.ClientId}",
                client_secret = $"{_saasOptions.ClientSecret}",
            });
            var oauthJson = new StringContent(payload, Encoding.UTF8, "application/json");
            var oauthResult = await HttpClient.PostAsync($"{_saasOptions.Url}/oauth/token", oauthJson);
            var oauthResultJson = JObject.Parse(await oauthResult.Content.ReadAsStringAsync());
            if (!oauthResultJson.ContainsKey("access_token")){
                throw new Exception("Saas auth exception");
            }
            var bearerToken = oauthResultJson["access_token"];

            HttpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + bearerToken.ToString());
            payload = JsonConvert.SerializeObject(new
            {
                client_name = name, 
                email = email, 
                password = password,
                phone = telephone, 
                url = domain, 
                language = language, 
                site_language = language, 
                promocode = promocode, 
                numberOfUsers = numberOfUsers,
                tariff = tariff,
                days =  days.ToString(),
                trialDays =  trialDays.ToString(),
                modules = modules,
                token = authToken
            });
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var result = await HttpClient.PostAsync($"{_saasOptions.Url}/api/new_instance", content);

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> ChangeModules(string authToken, string modules, int numberOfUsers){
            string payload = JsonConvert.SerializeObject(new
            {
                grant_type = "client_credentials",
                client_id = $"{_saasOptions.ClientId}",
                client_secret = $"{_saasOptions.ClientSecret}",
            });
            var oauthJson = new StringContent(payload, Encoding.UTF8, "application/json");
            var oauthResult = await HttpClient.PostAsync($"{_saasOptions.Url}/oauth/token", oauthJson);
            var oauthResultJson = JObject.Parse(await oauthResult.Content.ReadAsStringAsync());
            if (!oauthResultJson.ContainsKey("access_token")){
                throw new Exception("Saas auth exception");
            }
            var bearerToken = oauthResultJson["access_token"];

            HttpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + bearerToken.ToString());
            payload = JsonConvert.SerializeObject(new
            {
                auth_token = authToken,
                modules = modules, 
                number_of_users = numberOfUsers, 
            });
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var result = await HttpClient.PostAsync($"{_saasOptions.Url}/api/change_modules", content);

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return false;
            }

            var responseJson = JObject.Parse(await result.Content.ReadAsStringAsync());
            if ((int)responseJson["errcode"] != 0){
                return false;
            }

            return true;            
        }
    }
}
