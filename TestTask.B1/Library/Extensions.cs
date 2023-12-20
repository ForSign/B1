using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.B1.Library
{
    public static class Extensions
    {
        /// <summary>
        ///     Check if int convertable
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IntCompatible(this string s)
        {
            if (s != null && Int32.TryParse(s, out _))
                return true;

            return false;
        }

        /// <summary>
        ///     Check if date convertable
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool DateCompatible(this string s)
        {
            if (s != null && DateTime.TryParse(s, out _))
                return true;

            return false;
        }

        /// <summary>
        ///     Enumerate with index
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ie"></param>
        /// <param name="action"></param>
        public static void Each<T>(this IEnumerable<T> ie, Action<T, int> action)
        {
            var i = 0;
            foreach (var e in ie) action(e, i++);
        }
    }
}
