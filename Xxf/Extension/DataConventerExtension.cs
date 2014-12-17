using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xxf.Extension
{
    public static class DataConventerExtension
    {
        public static decimal? TryParseDecimal(this string val)
        {
            if (string.IsNullOrEmpty(val))
                return null;
            decimal result;
            if (decimal.TryParse(val, out result))
                return result;
            else
                return null;
        }

        public static int? TryParseInt(this string val)
        {
            if (string.IsNullOrEmpty(val))
                return null;
            int result;
            if (int.TryParse(val, out result))
                return result;
            else
                return null;
        }

        public static bool? TryParseBoolean(this string val)
        {
            if (string.IsNullOrEmpty(val))
                return null;
            bool result;
            if (bool.TryParse(val, out result))
                return result;
            else return null;
        }

        public static DateTime? TryParseDatetime(this string val)
        {
            if (string.IsNullOrEmpty(val))
                return null;
            DateTime result;
            if (DateTime.TryParse(val, out result))
                return result;
            else return null;
        }

        public static T? TryParseEnum<T>(this string val)
           where T : struct
        {
            if (string.IsNullOrEmpty(val))
                return null;

            try
            {
                return (T)Enum.Parse(typeof(T), val);
            }
            catch
            {
                return null;
            }

        }



        public static string TryToString(this DateTime? dt,string format)
        {
            return dt.HasValue ? dt.Value.ToString(format) : "";
        }
    }
}
