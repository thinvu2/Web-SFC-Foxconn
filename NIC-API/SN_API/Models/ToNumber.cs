using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models
{
    public static class ToNumber
    {
        public static int ToNullableInt(this string s)
        {
            int i;
            if (int.TryParse(s, out i)) return i;
            return 0;
        }
    }
}