using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace diga.web.Models.Status
{
    public class ResponseData<T> : ResponseData
        where T : class
    {
        public T Data { get; set; }

        public static new ResponseData<T> Fail(string key, string value)
        {
            ResponseData<T> response = new ResponseData<T>();
            response.AddError(key, value);

            return response;
        }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.ContentType = "application/json";

            if (Errors.Count != 0)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(Errors, SerializerSettings));
            }
            else
            {
                await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(Data, SerializerSettings));
            }
        }
    }

    public class ResponseData : IActionResult
    {
        //public Dictionary<string, List<string>> Errors { get; set; } = new Dictionary<string, List<string>> { };
        protected JsonSerializerSettings SerializerSettings =
            new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };

        public Dictionary<string, List<string>> Errors { get; set; } = new Dictionary<string, List<string>> { };
        public object Data { get; set; }

        public void AddError(string key, string value)
        {
            if (Errors.ContainsKey(key))
                Errors[key].Add(value);
            else
                Errors.Add(key, new List<string> { value });
        }

        public static ResponseData Fail(string key, string value)
        {
            ResponseData response = new ResponseData();
            response.AddError(key, value);

            return response;
        }

        public static ResponseData Ok()
        {
            return new ResponseData();
        }

        public static ResponseData OkWithData(object data)
        {
            ResponseData response = new ResponseData();
            response.Data = data;

            return response;
        }

        public virtual async Task ExecuteResultAsync(ActionContext context)
        {
            if (Errors.Count != 0 || Data != null)
            {
                context.HttpContext.Response.ContentType = "application/json";
                
                if (Errors.Count != 0){
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(Errors, SerializerSettings));
                }
                if (Data != null){
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                    await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(Data, SerializerSettings));
                }
            }
        }
    }
}
