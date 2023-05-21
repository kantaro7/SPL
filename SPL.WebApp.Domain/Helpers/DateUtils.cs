using System.Globalization;
using System;

namespace SPL.WebApp.Domain.Helpers
{
    public class DateUtils
    {
        public static DateTime getDateFromSheet(string fecha)
        {
            double fechaD = returnDouble(fecha);
            if (fechaD > 0)
            {
                return DateTime.FromOADate(fechaD);
            }
            DateTime date = DateTime.ParseExact(fecha, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            return date;
        }
        public static double returnDouble(string value)
        {
            double valueD = 0;
            try
            {
                valueD = Double.Parse(value);
            }
            catch (Exception e)
            {

            }
            return valueD;
        }
    }
}
