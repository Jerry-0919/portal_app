using diga.bl.Constants;
using diga.web.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Helpers
{
    public class MangoPayHelper
    {
        private readonly ConstantOptions _constantOptions;

        public MangoPayHelper(IOptions<ConstantOptions> constantOptions)
        {
            _constantOptions = constantOptions.Value;
        }

        public long CalculateFee(double sum, string method)
        {
            var ourFee = sum * (_constantOptions.Fee / 100);
            var mangoFee = 0.0;
            switch(method)
            {
                case PlatformPayInMethods.Card:
                    mangoFee = sum * (_constantOptions.MangoPayCardFeePercent / 100);
                    mangoFee += _constantOptions.MangoPayCardFee;
                    break;
                case PlatformPayInMethods.BankWire:
                    mangoFee = sum * (_constantOptions.MangoPayBankWireFeePercent / 100);
                    break;
                default:
                    break;
            }
            ourFee -= mangoFee;

            return (long) ourFee;
        }

        public long CalculateTransfer(double sum, string method)
        {
            var fee = CalculateFee(sum, method);

            return (long)(sum - fee);
        }
    }
}
