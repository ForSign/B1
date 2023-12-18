using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.B1.Library
{
    public static class Extensions
    {

        public static bool IntCompatible(this string s)
        {
            if (s != null && Int32.TryParse(s, out _))
                return true;

            return false;
        }

        public static bool DateCompatible(this string s)
        {
            if (s != null && DateTime.TryParse(s, out _))
                return true;

            return false;
        }
    }
}
