using System;
using System.Collections.Generic;
using System.Linq;
//using System.Linq;

namespace Queries
{
    class Program
    {
        static void Main(string[] args)
        {
            var numbers = MyLinq.Random().Where(n => n > 0.5)
                .Take(10)
                .OrderBy(n => n);

            foreach (var num in numbers)
            {
                Console.WriteLine(num);
            }

            var movies = new List<Movie>
            {
                new Movie { Title = "The Dark Knight", Rating = 8.9f, Year = 2008},
                new Movie { Title = "The King's Speech", Rating = 8.0f, Year = 2010},
                new Movie { Title = "Casablanca", Rating = 8.5f, Year = 1942},
                new Movie { Title = "Star Wars V", Rating = 8.7f, Year = 1980},
            };

            //var query = movies.Where(m => m.Year > 2000);

            // pit fall of deferred execution
            //var query = Enumerable.Empty<Movie>();

            /* 
            try
            {
                //query = movies.Where(m => m.Year > 2000);
                query = movies.Where(m => m.Year > 2000).ToList(); // execute the query immediately
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            */

            //Console.WriteLine(query.Count());
            //var enumerator = query.GetEnumerator();
            //while (enumerator.MoveNext())
            //{
            //    Console.WriteLine(enumerator.Current.Title);
            //}

            //var query = movies.Where(m => m.Year > 2000)
            //            .OrderByDescending(m => m.Rating);

            //foreach (var movie in query)
            //{
            //    Console.WriteLine(movie.Title);
            //}

            var query = from movie in movies
                        where movie.Year > 2000
                        orderby movie.Rating descending
                        select movie;


            /* 
            var enumerator = query.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current.Title);
            }
            */
        }
    }
}
