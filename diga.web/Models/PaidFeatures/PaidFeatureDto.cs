using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Models.PaidFeatures
{
    public class PaidFeatureDto
    {
        public int NameId { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public string Currency { get; set; }

    }
}
