using Microsoft.AspNetCore.Http;
using System.Linq;

namespace SPL.WebApp.Helpers
{
    public static class ParserHelper
    {
        public static decimal? StringToDecimalNullable(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            else
            {
                decimal.TryParse(value, out var result);
                return result;
            }
        }

        public static decimal StringToDecimal(string value)
        {
            if (string.IsNullOrEmpty(value))
                return 0;
            else
            {
                decimal.TryParse(value, out var result);
                return result;
            }
        }

        public static decimal? StringToIntNullable(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            else
            {
                int.TryParse(value, out var result);
                return result;
            }
        }

        public static decimal StringToInt(string value)
        {
            if (string.IsNullOrEmpty(value))
                return 0;
            else
            {
                int.TryParse(value, out var result);
                return result;
            }
        }

        public static string DecimalToString(decimal? value)
        {
            if (value.HasValue)
                return value.ToString();
            else
            {
                return string.Empty;
            }
        }

        public static string IntToString(int? value)
        {
            if (value.HasValue)
                return value.ToString();
            else
            {
                return string.Empty;
            }
        }

        public static bool? DecimalToBool(decimal? value)
        {
            if (value.HasValue)
                if (value.Value.Equals(1))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            else
            {
                return null;
            }
        }

        public static decimal? BoolToDecimal(bool? value)
        {
            if (value.HasValue)
                if (value.Value)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Obtiene el numero de serie enviado a traves de la url
        /// </summary>
        /// <param name="query">Url query parameters</param>
        /// <returns>NoSerie</returns>
        public static string GetNoSerieFromUrlQuery(IQueryCollection query)
        {
            if (query.Any())
                return query.FirstOrDefault(x => x.Key == "noSerie").Value;

            return string.Empty;
        }
    }
}