using VNPTNET.NOM.Common;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using VNPTNET.NOM.Common.Enums;

namespace VNPTNET.NOM.Lib
{
    public class Validate
    {
        public static Regex RgxInterger = new Regex(RegexDefine.Interger, RegexOptions.None);
        public static Regex RgxNumber = new Regex(RegexDefine.Numeric, RegexOptions.None);
        public static Regex RgxDateVn = new Regex(RegexDefine.DateVN, RegexOptions.None);
        public static Regex RgxDateTimeVn = new Regex(RegexDefine.DateTimeVN, RegexOptions.None);
        public static Regex RgxDateIso = new Regex(RegexDefine.DateIso, RegexOptions.None);
        public static Regex RgxGuid = new Regex(RegexDefine.Guid, RegexOptions.IgnoreCase);

        public static bool IsInterger(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;
            return RgxInterger.IsMatch(value);
        }

        public static int ConvertInt(string value, int defaultValue = 0)
        {
            if (IsInterger(value))
                return Convert.ToInt32(value);
            else
                return defaultValue;
        }

        public static int? ConvertIntAlowNull(string value, int? defaultValue = null)
        {
            if (IsInterger(value))
                return Convert.ToInt32(value);
            else
                return defaultValue;
        }

        public static byte ConvertByte(string value, byte defaultValue = 0)
        {
            if (IsInterger(value))
                return Convert.ToByte(value);
            else
                return defaultValue;
        }

        public static byte? ConvertByteAlowNull(string value, byte? defaultValue = null)
        {
            if (IsInterger(value))
                return Convert.ToByte(value);
            else
                return defaultValue;
        }

        public static short ConvertShort(string value, short defaultValue = 0)
        {
            if (IsInterger(value))
                return Convert.ToInt16(value);
            else
                return defaultValue;
        }

        public static short? ConvertShortAlowNull(string value, short? defaultValue = null)
        {
            if (IsInterger(value))
                return Convert.ToInt16(value);
            else
                return defaultValue;
        }

        public static long ConvertLong(string value, long defaultValue = 0)
        {
            if (IsInterger(value))
                return Convert.ToInt64(value);
            else
                return defaultValue;
        }

        public static long? ConvertLongAlowNull(string value, long? defaultValue = null)
        {
            if (IsInterger(value))
                return Convert.ToInt64(value);
            else
                return defaultValue;
        }

        public static bool IsNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;
            return RgxNumber.IsMatch(value);
        }

        public static bool IsDecimal(string value, string dot = ".")
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;
            return Regex.IsMatch(value, @"^-?\d+(\" + dot + @"\d+)?$");
        }

        public static decimal ConvertDecimal(string value, decimal defaultValue = 0)
        {
            if (IsDecimal(value))
                return Convert.ToDecimal(value);
            else
                return defaultValue;
        }

        public static decimal? ConvertDecimalAlowNull(string value, decimal? defaultValue = null)
        {
            if (IsDecimal(value))
                return Convert.ToDecimal(value);
            else
                return defaultValue;
        }

        public static double ConvertDouble(string value, double defaultValue = 0)
        {
            if (IsNumber(value))
                return Convert.ToDouble(value);
            else
                return defaultValue;
        }

        public static double? ConvertDoubleAlowNull(string value, double? defaultValue = null)
        {
            if (IsNumber(value))
                return Convert.ToDouble(value);
            else
                return defaultValue;
        }

        public static bool IsDateVn(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;
            return RgxDateVn.IsMatch(value);
        }

        public static bool IsDateTimeVn(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;
            return RgxDateTimeVn.IsMatch(value);
        }

        public static bool IsGuid(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;
            return RgxGuid.IsMatch(value);
        }

        public static DateTime ConvertDateVN(string value, DateTime? defaultValue = null)
        {
            if (value=="__/__/____")
            {
                value = string.Empty;
            }
            if (IsDateVn(value))
                return DateTime.ParseExact(value, "d/M/yyyy", CultureInfo.InvariantCulture);
            else
                return defaultValue.HasValue ? defaultValue.Value : DateTime.MinValue;
        }

        public static DateTime ConvertDateMonthVN(string value, DateTime? defaultValue = null)
        {
            value = "01/" + value;
            if (IsDateVn(value))
                return DateTime.ParseExact(value, "d/M/yyyy", CultureInfo.InvariantCulture);
            else
                return defaultValue.HasValue ? defaultValue.Value : DateTime.MinValue;
        }

        public static DateTime ConvertDateTimeVN(string value, DateTime? defaultValue = null)
        {
            if (IsDateTimeVn(value))
                return DateTime.ParseExact(value, "d/M/yyyy HH:mm", CultureInfo.InvariantCulture);
            else
                return ConvertDateVN(value, defaultValue);
        }

        public static DateTime? ConvertDateVNAlowNull(string value, DateTime? defaultValue = null)
        {
            if (IsDateVn(value))
                return DateTime.ParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            else
                return defaultValue;
        }

        public static DateTime ConvertDateVNShortAlowNull(string value, DateTime defaultValue)
        {
            if (IsDateVn(value))
                return DateTime.ParseExact(value, "d/M/yyyy", CultureInfo.InvariantCulture);
            else
                return defaultValue;
        }

        public static DateTime? ConvertDateTimeVNAlowNull(string value, DateTime? defaultValue = null)
        {
            if (IsDateTimeVn(value))
                return DateTime.ParseExact(value, "d/M/yyyy HH:mm", CultureInfo.InvariantCulture);
            else
                return ConvertDateVNAlowNull(value, defaultValue);
        }

        public static bool IsDateIso(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;
            return RgxInterger.IsMatch(value);
        }

        public static DateTime ConvertDateIso(string value, DateTime? defaultValue)
        {
            if (IsDateIso(value))
                return Convert.ToDateTime(value);
            else
                return defaultValue.HasValue ? defaultValue.Value : DateTime.Now;
        }

        public static bool CheckCustom(string pattern, string value)
        {
            return Regex.IsMatch(value, pattern);
        }

        public static DateTime ConvertDate(string value, string format = "d/M/yyyy", string DateSeparator = "/")
        {
            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.ShortDatePattern = format;
            dtfi.DateSeparator = DateSeparator;
            return Convert.ToDateTime(value, dtfi);
        }

        public static DateTime? MaxDate(DateTime? date1, DateTime? date2)
        {
            if(date1.HasValue && date2.HasValue)
            {
                if (date1.Value > date2.Value)
                    return date1;
                return date2;
            }
            if (date1.HasValue)
                return date1;
            if (date2.HasValue)
                return date2;
            return null;
        }

        public static DateTime? MinDate(DateTime? date1, DateTime? date2)
        {

            if (date1.HasValue && date2.HasValue)
            {
                if (date1.Value > date2.Value)
                    return date2;
                return date1;
            }
            if (date1.HasValue)
                return date1;
            if (date2.HasValue)
                return date2;
            return null;
        }
    }
}