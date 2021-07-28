using System;
using System.Collections.Generic;
using System.Linq;
using Features.Extensions;

namespace Features
{
    class Program
    {
        static void Main(string[] args)
        {
            Func<int, int> square = x => x * x;
            Func<int, int, int> add = (x, y) => x + y;

            Action<int> write = x => Console.WriteLine(x);

            write(square(add(2, 3)));

            Employee[] developers = new Employee[]
            {
                new Employee {  Id = 1, Name = "Scott"},
                new Employee {  Id = 2, Name = "Chris"},
                new Employee {  Id = 3, Name = "Craig"}
            };


            List<Employee> sales = new List<Employee>()
            {
                new Employee { Id = 4, Name = "Alex"},
                new Employee { Id = 5, Name = "Dan"}
            };

            foreach(var person in developers)
            {
                Console.WriteLine($"{person.Id}: {person.Name}");
            }
            Console.WriteLine($"Number of developers: {developers.Count()}");

            foreach(var person in sales)
            {
                Console.WriteLine($"{person.Id}: {person.Name}");
            }
            Console.WriteLine($"Number of sales: {sales.Count()}");

            // extension method syntax
            var query = developers.Where(e => e.Name.Length == 5)
                                  .OrderBy(e => e.Name);

            // query syntax always start with 'from' and end with 'select'
            var query2 = from developer in developers
                         where developer.Name.Length == 5
                         orderby developer.Name
                         select developer;

            foreach(var employee in query2)
            //foreach(var employee in query)
            {
                Console.WriteLine(employee.Name);
            }

            IEnumerable < Employee > marketing = new List<Employee>()
            {
                new Employee { Id = 6, Name = "John"},
                new Employee { Id = 7, Name = "Sean"}
            };


            IEnumerator<Employee> enumerator = marketing.GetEnumerator();
            while(enumerator.MoveNext())
            {
                Console.WriteLine($"{enumerator.Current.Id}: {enumerator.Current.Name}");
            }

            Console.WriteLine($"Number of marketing: {marketing.Count()}");

            var text = "43.35";
            Console.WriteLine($"{text}: ${text.ToDouble():N4}");

        }

    }
}
