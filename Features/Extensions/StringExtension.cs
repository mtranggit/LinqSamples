using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.Extensions
{

    // Extension methods can be used to extend any type
    // only extends non existing method of a type
    public static class StringExtension
    {
        public static double ToDouble(this string data)
        {
            double result = double.Parse(data);
            return result;
        }
    }
}
