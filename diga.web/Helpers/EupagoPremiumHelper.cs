using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Text;
using diga.web.ViewModels;
using diga.dal;
using Newtonsoft.Json;

namespace diga.web.Helpers
{
    public interface IEupagoPremiumHelper
    {
        Task<EupagoResponseModel> CreditCard(double sum,
            string cartId,
            string cardNumber,
            string cvv,
            string validTo);
        Task<EupagoResponseModel> Multibanco(double sum,
            string cartId);
        Task<EupagoResponseModel> Mbway(double sum,
            string phone,
            string cartId);
    }
    public class EupagoPremiumHelper : IEupagoPremiumHelper
    {
        IConfiguration _config;
        private HttpClientHandler Handler { get; set; }
        private HttpClient HttpClient { get; set; }
        private readonly DigaContext _context;
        IPaymentHelper paymentHelper;
        public EupagoPremiumHelper(IConfiguration config, DigaContext context, IPaymentHelper paymentHelper)
        {
            _config = config;
            Handler = new HttpClientHandler();
            HttpClient = new HttpClient(Handler);
            _context = context;
            this.paymentHelper = paymentHelper;
        }

        public async Task<EupagoResponseModel> CreditCard(double sum, string cartId, string cardNumber, string cvv, string validTo)
        {
            HttpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            string payload = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                chave = _config.GetSection("Constants:eupago_key").Value,
                valor = sum,
                id = cartId,
                numero = cardNumber,
                cripto = cvv,
                validade = validTo,
                url_retorno = $"{_config.GetSection("Constants:host").Value}/message/SuccessCreditCardPayment/" + cartId
            });
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var result = await HttpClient.PostAsync($"{_config.GetSection("Constants:eupago_endpoint").Value}/cc/purchase_tds", content);
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("Eupago request error");
            }
            var resultJson = JObject.Parse(await result.Content.ReadAsStringAsync());
            if (!resultJson.ContainsKey("sucesso") || !resultJson.ContainsKey("estado"))
            {
                throw new Exception("Eupago success error");
            }
            var estado = int.Parse(resultJson["estado"].ToString());
            var sucesso = bool.Parse(resultJson["sucesso"].ToString());
            var resposta = resultJson["resposta"].ToString();

            if (estado != 0 || sucesso != true)
            {
                throw new Exception("Eupago error " + resposta);
            }

            return new EupagoResponseModel
            {
                Reference = resultJson["referencia"].ToString(),
                RedirectUrl = resultJson["tds_url"].ToString()
            };
        }

        public async Task<EupagoResponseModel> Mbway(double sum, string phone, string cartId)
        {
            HttpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            string payload = JsonConvert.SerializeObject(new
            {
                chave = _config.GetSection("Constants:eupago_key").Value,
                valor = sum,
                id = cartId,
                alias = phone,
                descricao = "ODIGA LDA - Pagamento pela utilização do sistema DIGA"
            });
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var result = await HttpClient.PostAsync($"{_config.GetSection("Constants:eupago_endpoint").Value}/mbway/create", content);
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("Eupago request error");
            }
            var resultJson = JObject.Parse(await result.Content.ReadAsStringAsync());
            if (!resultJson.ContainsKey("sucesso") || !resultJson.ContainsKey("estado"))
            {
                throw new Exception("Eupago success error");
            }
            var estado = int.Parse(resultJson["estado"].ToString());
            var sucesso = bool.Parse(resultJson["sucesso"].ToString());
            var resposta = resultJson["resposta"].ToString();

            if (estado != 0 || sucesso != true)
            {
                throw new Exception("Eupago error " + resposta);
            }

            return new EupagoResponseModel
            {
                Reference = resultJson["referencia"].ToString(),
            };
        }

        public async Task<EupagoResponseModel> Multibanco(double sum, string cartId)
        {
            HttpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            string payload = JsonConvert.SerializeObject(new
            {
                chave = _config.GetSection("Constants:eupago_key").Value,
                valor = sum,
                id = cartId,
                data_inicio = DateTime.Now.ToString("yyyy-MM-dd"),
                data_fim = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd"),
                valor_maximo = sum,
                valor_minimo = sum,
                per_dup = "0"
            });
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var result = await HttpClient.PostAsync($"{_config.GetSection("Constants:eupago_endpoint").Value}/multibanco/create", content);
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("Eupago request error");
            }
            var resultJson = JObject.Parse(await result.Content.ReadAsStringAsync());
            if (!resultJson.ContainsKey("sucesso") || !resultJson.ContainsKey("estado"))
            {
                throw new Exception("Eupago success error");
            }
            var estado = int.Parse(resultJson["estado"].ToString());
            var sucesso = bool.Parse(resultJson["sucesso"].ToString());
            var resposta = resultJson["resposta"].ToString();

            if (estado != 0 || sucesso != true)
            {
                throw new Exception("Eupago error " + resposta);
            }

            return new EupagoResponseModel
            {
                Reference = resultJson["referencia"].ToString(),
                Entity = resultJson["entidade"].ToString()
            };
        }
    }
}
