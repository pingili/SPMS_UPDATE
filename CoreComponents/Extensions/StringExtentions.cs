using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreComponents.Security;
using System.Globalization;

namespace CoreComponents
{
    public static class StringExtentions
    {
        public static string EncryptString(this object input)
        {
            string str = Convert.ToString(input);

            try
            {
                return EncryptionMethods.EncryptString(str);
            }
            catch
            {
                return null;
            }
        }

        public static string DecryptString(this object input)
        {
            string str = Convert.ToString(input);
            try
            {
                string decryptedString = EncryptionMethods.DecryptString(str);
                return decryptedString;
            }
            catch
            {
                return null;
            }
        }
        public static string ToDisplayDateFormat(this DateTime dt)
        {
            if (dt == DateTime.MinValue)
                return string.Empty;
            else
                return dt.ToString("dd'/'MMM'/'yyyy");
            //return dt.ToString("dd/MMM/yyyy");
        }

        public static DateTime ConvertToDateTime(this string input)
        {
            DateTime dt = default(DateTime);

            try
            {
                dt = DateTime.ParseExact(input, "dd/MMM/yyyy", CultureInfo.InvariantCulture);
            }
            catch { }

            return dt;
        }

        public static DateTime ParseExactCustom(this string input, string format = "MM/DD/YYYY")
        {
            DateTime dt = default(DateTime);

            try
            {
                dt = DateTime.ParseExact(input, format, CultureInfo.InvariantCulture);
            }
            catch { }

            return dt;
        }

        public static long ToLong(this decimal input)
        {
            return ((long)input);
        }


        public static string ToDisplayCurrency(this decimal input)
        {
            return ((long)input).ToString();
        }

        public static string ToDisplayCurrencyInRupeesWithComma(this decimal input)
        {
            return String.Format("{0:n0}", input);
        }
        
        public static string ToDisplayCurrencyInRupees(this decimal input)
        {
            return ((long)input).ToString();
        }

        public static string ToDisplayCurrencyRPT(this decimal input)
        {
            return String.Format("{0:n}", input);
        }
    }
}
