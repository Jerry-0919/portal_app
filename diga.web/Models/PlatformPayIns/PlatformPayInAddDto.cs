using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Models.PlatformPayIns
{
    public class PlatformPayInAddDto
    {
        public string CardId { get; set; }
        public string Method { get; set; }
        public string Type { get; set; }
        public int ContractId { get; set; }
        public int? MeasurementReportId { get; set; }
        public int? PaymentPartId { get; set; }
    }
}
