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
using diga.web.ViewModels;
using Microsoft.EntityFrameworkCore;
using diga.dal;

namespace diga.web.Helpers
{
    public interface IEupagoHelper
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
        Task CheckPayments();
    }
    public class EupagoHelper: IEupagoHelper
    {
        IConfiguration _config;
        private HttpClientHandler Handler { get; set; }
        private HttpClient HttpClient { get; set; }
        private readonly DigaContext _context;
        IPaymentHelper paymentHelper;
        public EupagoHelper(IConfiguration config, DigaContext context, IPaymentHelper paymentHelper)
        {
            _config = config;
            Handler = new HttpClientHandler();
            HttpClient = new HttpClient(Handler);
            _context = context;
            this.paymentHelper = paymentHelper;
        }
        public async Task<EupagoResponseModel> CreditCard(
            double sum,
            string cartId,
            string cardNumber,
            string cvv,
            string validTo
        ) 
        {
            HttpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            string payload = JsonConvert.SerializeObject(new
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
            if (!resultJson.ContainsKey("sucesso") || !resultJson.ContainsKey("estado")){
                throw new Exception("Eupago success error");
            }
            var estado = int.Parse(resultJson["estado"].ToString());
            var sucesso = bool.Parse(resultJson["sucesso"].ToString());
            var resposta = resultJson["resposta"].ToString();

            if (estado != 0 || sucesso != true){
                throw new Exception("Eupago error " + resposta);
            }

            return new EupagoResponseModel{
                Reference = resultJson["referencia"].ToString(),
                RedirectUrl = resultJson["tds_url"].ToString()
            };
        }

        public async Task<EupagoResponseModel> Multibanco(
            double sum,
            string cartId
        ) 
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
            if (!resultJson.ContainsKey("sucesso") || !resultJson.ContainsKey("estado")){
                throw new Exception("Eupago success error");
            }
            var estado = int.Parse(resultJson["estado"].ToString());
            var sucesso = bool.Parse(resultJson["sucesso"].ToString());
            var resposta = resultJson["resposta"].ToString();

            if (estado != 0 || sucesso != true){
                throw new Exception("Eupago error " + resposta);
            }

            return new EupagoResponseModel{
                Reference = resultJson["referencia"].ToString(),
                Entity = resultJson["entidade"].ToString()
            };
        }

        public async Task<EupagoResponseModel> Mbway(
            double sum,
            string phone,
            string cartId
        ) 
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
            if (!resultJson.ContainsKey("sucesso") || !resultJson.ContainsKey("estado")){
                throw new Exception("Eupago success error");
            }
            var estado = int.Parse(resultJson["estado"].ToString());
            var sucesso = bool.Parse(resultJson["sucesso"].ToString());
            var resposta = resultJson["resposta"].ToString();

            if (estado != 0 || sucesso != true){
                throw new Exception("Eupago error " + resposta);
            }

            return new EupagoResponseModel{
                Reference = resultJson["referencia"].ToString(),
            };
        }
        
        public async Task CheckPayments()
        {
            var carts = await _context.Carts.Include(c => c.User).Where(c => c.IsPaid != true && c.Provider == "eupago_multibanco").ToListAsync();
            if (carts.Count > 0)
            {
                foreach (var cart in carts)
                {
                    HttpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                    string payload = JsonConvert.SerializeObject(new
                    {
                        chave = _config.GetSection("Constants:eupago_key").Value,
                        referencia = cart.Reference
                    });
                    var content = new StringContent(payload, Encoding.UTF8, "application/json");

                    var result = await HttpClient.PostAsync($"{_config.GetSection("Constants:eupago_endpoint").Value}/multibanco/info", content);
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

                    var pagamentos = resultJson["pagamentos"];
                    var paymentsArray = (JArray)pagamentos;

                    if (paymentsArray == null)
                    {
                        continue;
                    }

                    foreach (JObject payment in paymentsArray.Children<JObject>())
                    {
                        var estadoP = payment["estado"].ToString();
                        if (estadoP != "paga")
                        {
                            continue;
                        }

                        var valor = double.Parse(payment["valor"].ToString());
                        if (valor >= cart.TotalPriceWithDiscount)
                        {
                            cart.IsPaid = true;                            
                        }
                    }

                    if (cart.IsPaid == true)
                    {
                        await paymentHelper.SuccessfullPayment(cart);                        
                    }
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}
