using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Efika.Crm.Negocio.Common
{
    public static class Utility
    {

        public static string ReplaceDotToComma(string str)
        {
            return str.Replace(".", ",");
        }

        public static string ReplaceCommaToDot(string str)
        {
            return str.Replace(",", ".");
        }

        public static bool IsNumeric(object stringToTest)
        {
            int i; float f; decimal d;
            return stringToTest == null ? false : int.TryParse(stringToTest.ToString(), out i) ||
            float.TryParse(stringToTest.ToString(), out f) ||
            decimal.TryParse(stringToTest.ToString(), out d);
        }

        public static decimal ConvertStringToDecimal(string value, decimal defaultValue = 0)
        {
            decimal rvalue = defaultValue;

            try
            {
                if (! string.IsNullOrEmpty (value))
                    rvalue = Convert.ToDecimal(value);
            }
            catch
            {
                rvalue = defaultValue;
            }
            return rvalue;
        }

        public static int ConvertStringToInt32(string value, int defaultValue = 0)
        {
            int rvalue = defaultValue;

            try
            {
                rvalue = Convert.ToInt32(value);
            }
            catch (Exception)
            {
                rvalue = defaultValue;
            }
            return rvalue;
        }

        public static int EmptyInt(object value, int def = 0)
        {
            int returnValue = def;
            try
            {
                returnValue = Convert.ToInt32(value);
            }
            catch (Exception)
            {
                returnValue = def;
            }
            return returnValue;
        }

        public static decimal EmptyDecimal(object value, decimal def = 0)
        {
            decimal returnValue = def;
            try
            {
                returnValue = Convert.ToDecimal(value);
            }
            catch (Exception)
            {
                returnValue = def;
            }
            return returnValue;
        }

        /// <summary>
        /// Devuelve un valor Long que especifica el número de
        /// intervalos de tiempo entre dos valores Date.
        /// </summary>
        /// <param name="interval">Obligatorio. Valor de enumeración
        /// DateInterval o expresión String que representa el intervalo
        /// de tiempo que se desea utilizar como unidad de diferencia
        /// entre Date1 y Date2.</param>
        /// <param name="date1">Obligatorio. Date. Primer valor de
        /// fecha u hora que se desea utilizar en el cálculo.</param>
        /// <param name="date2">Obligatorio. Date. Segundo valor de
        /// fecha u hora que se desea utilizar en el cálculo.</param>
        /// <returns></returns>
        public static long DateDiff(DateInterval interval, DateTime date1, DateTime date2)
        {
            long rs = 0;
            TimeSpan diff = date2.Subtract(date1);
            switch (interval)
            {
                case DateInterval.Day:
                case DateInterval.DayOfYear:
                    rs = (long)diff.TotalDays;
                    break;
                case DateInterval.Hour:
                    rs = (long)diff.TotalHours;
                    break;
                case DateInterval.Minute:
                    rs = (long)diff.TotalMinutes;
                    break;
                case DateInterval.Month:
                    rs = (date2.Month - date1.Month) + (12 * Utility.DateDiff(DateInterval.Year, date1, date2));
                    break;
                case DateInterval.Quarter:
                    rs = (long)Math.Ceiling((double)(Utility.DateDiff(DateInterval.Month, date1, date2) / 3.0));
                    break;
                case DateInterval.Second:
                    rs = (long)diff.TotalSeconds;
                    break;
                case DateInterval.Weekday:
                case DateInterval.WeekOfYear:
                    rs = (long)(diff.TotalDays / 7);
                    break;
                case DateInterval.Year:
                default:
                    rs = date2.Year - date1.Year;
                    break;
            }//end of switch
            return rs;
        }//DateDiff
    }

    /// <summary>
    /// Enumerados que definen los tipos de
    /// intervalos de tiempo posibles.
    /// </summary>
    public enum DateInterval
    {
        Day,
        DayOfYear,
        Hour,
        Minute,
        Month,
        Quarter,
        Second,
        Weekday,
        WeekOfYear,
        Year
    }



}

