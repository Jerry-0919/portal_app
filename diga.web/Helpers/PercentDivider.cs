using System.Collections.Generic;

namespace diga.web.Helpers
{
    public static class PercentDivider
    {
        public static Dictionary<string, double> Divide(double value, Dictionary<string, double> percents)
        {
            Dictionary<string, double> result = new Dictionary<string, double> { };

            foreach(KeyValuePair<string, double> percent in percents)
            {
                result.Add(percent.Key, percent.Value * value);
            }

            return result;
        }
    }
}
