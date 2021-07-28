using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.Extensions
{
    public static class MyLinq
    {
        public static int CountTotal<T>(this IEnumerable<T> sequence)
        {
            var count = 0;
            foreach(var item in sequence)
            {
                count += 1;
            }
            return count;
        }
    }
}
