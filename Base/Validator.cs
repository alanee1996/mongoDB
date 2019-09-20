using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Base
{
    public static class Validator
    {
        private static readonly string dateTimeFormat = "dd-MM-yyyy hh:mm tt";
        public static bool isNullOrEmpty(object obj)
        {
            if (obj == null) return true;
            if (obj.GetType() == typeof(string))
            {
                return ((string)obj) == "";
            }
            return false;
        }

        public static bool isBetween(int start, int end, int value)
        {
            return value > start && value < end;
        }

        public static bool isBetween(DateTime start, DateTime end, DateTime value)
        {
            return value > start && value < end;
        }

        public static bool matchPattern(string data, string pattern)
        {
            Regex regex = new Regex(pattern);
            return regex.IsMatch(data);
        }
    }
}
