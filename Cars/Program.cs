using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cars
{
    class Program
    {
        static void Main(string[] args)
        {
            var cars = ProcessFile("fuel.csv");

            var query = from car in cars
                        where car.Manufacturer == "BMW" && car.Year == 2016
                        orderby car.Combined descending, car.Name ascending
                        select new
                        {
                            car.Manufacturer,
                            car.Name,
                            car.Combined
                        };

            //var query = cars.Where(c => c.Manufacturer == "BMW" && c.Year == 2016)
            //        .OrderByDescending(c => c.Combined)
            //        .ThenBy(c => c.Name)
            //        .Select(c => c);

            foreach (var car in query)
            {
                Console.WriteLine($"{car.Manufacturer} {car.Name} : {car.Combined}");
            }

            //var result = cars.SelectMany(c => c.Name)
            //                 .OrderBy(c => c);
            //foreach (var name in result)
            //{
            //    Console.WriteLine($"{name}");
            //}

        }

        private static List<Car> ProcessFile(string path)
        {
            //var query = from line in File.ReadAllLines(path).Skip(1)
            //            where line.Length > 1
            //            select Car.ParseFromCsv(line);

            var query = File.ReadAllLines(path)
                .Skip(1)
                .Where(l => l.Length > 1)
                //.Select(l => Car.ParseFromCsv(l));
                .ToCar();

            return query.ToList();
        }


    }

}

