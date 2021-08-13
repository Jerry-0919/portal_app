using System;
using System.Collections.Generic;
using System.Text;

namespace diga.bl.Constants
{
    public static class MangoPayKYCValidationStatuses
    {
        public const string NotSpecified = "NotSpecified";
        public const string CREATED = "CREATED";
        public const string VALIDATION_ASKED = "VALIDATION_ASKED";
        public const string VALIDATED = "VALIDATED";
        public const string REFUSED = "REFUSED";
        public const string OUT_OF_DATE = "OUT_OF_DATE";
    }
}
