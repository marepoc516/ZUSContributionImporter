using System;

namespace ZUSContributionImporter.Common
{
    public static class ConvertExtensions
    {
        public static decimal ToDecimalWithTrim(string value)
        {
            return Convert.ToDecimal(value.Replace("&nbsp;", "").Replace("%", "").Trim());
        }
        public static int ToInt32WithTrim(string value)
        {
            return Convert.ToInt32(value.Replace("&nbsp;", "").Replace("%", "").Trim());
        }
    }
}