using diga.web.Helpers;
using Hangfire;
using Hangfire.Annotations;
using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Jobs
{
    public class HangFireCheckPayment 
    {
        private readonly IEupagoHelper _eupagoHelper;
        public HangFireCheckPayment(IEupagoHelper eupagoHelper)
        {
            _eupagoHelper = eupagoHelper;
        }

        public void Check()
        {
           BackgroundJob.Enqueue(() => _eupagoHelper.CheckPayments());
        }
    }
}
