using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Options
{
    public class ConstantOptions
    {
        public string eupago_endpoint { get; set; }
        public string eupago_key { get; set; }
        public string host { get; set; }
        public string AUTH0_DOMAIN { get; set; }
        public string API_IDENTIFIER { get; set; }
        public string AUTH0_CLIENT_ID { get; set; }
        public string AUTH0_CLIENT_SECRET { get; set; }
        public double Fee { get; set; }
        public double MangoPayCardFee { get; set; }
        public double MangoPayCardFeePercent { get; set; }
        public double MangoPayBankWireFeePercent { get; set; }
    }
}
