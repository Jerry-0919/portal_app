using diga.dal;
using diga.web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace diga.web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly DigaContext _db;
        protected readonly IConfiguration _config;
        protected readonly IPaymentHelper _paymentHelper;
        protected readonly IEupagoHelper _eupagoHelper;
        protected readonly SaasHelper _saasHelper;

        public BaseController(DigaContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        public BaseController(DigaContext db, IConfiguration config, IPaymentHelper paymentHelper)
        {
            _db = db;
            _config = config;
            _paymentHelper = paymentHelper;
        }

        public BaseController(DigaContext db, IConfiguration config, IPaymentHelper paymentHelper, IEupagoHelper eupagoHelper, SaasHelper saasHelper)
        {
            _db = db;
            _config = config;
            _paymentHelper = paymentHelper;
            _eupagoHelper = eupagoHelper;
            _saasHelper = saasHelper;
        }

        protected int UserId
        {
            get
            {
                return int.Parse(User.FindFirst("Id").Value);
            }
        }
    }
}