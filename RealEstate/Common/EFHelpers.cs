using System;
using System.Text.RegularExpressions;

namespace RealEstate.Common
{
    public static class EFHelpers
    {
        public static bool IsEqual(this string source, string param)
        {
            if (string.IsNullOrEmpty(source))
                return false;
            return (Regex.Replace(source, @"[^\w\d]", "")).Equals((Regex.Replace(param ?? "", @"[^\w\d]", "")), StringComparison.CurrentCultureIgnoreCase);
        }
    }
}