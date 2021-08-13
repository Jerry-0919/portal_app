using diga.bl.Models.Platform.Contracts;
using System;
using System.Collections.Generic;

namespace diga.web.Helpers
{
    public static class PlatformContractChangeHelper
    {
        public static List<PlatformContractChange> CompareWrite(this List<PlatformContractChange> changes,
             string from, string to, int userId, int contractId, string caseValue, DateTime dateTime)
        {
            if (from != to)
                changes.Add(new PlatformContractChange(userId, contractId, caseValue, from, to, dateTime));

            return changes;
        }
    }
}
