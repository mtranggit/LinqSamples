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
            var manufacturers = ProcessManufacturers("manufacturers.csv");

            // A summary of the most, the least and average fuel efficiency of cars by manufacturer
            // and show the most fuel efficiency car's manufacturer first
            // query syntax
            //var query =
            //    from car in cars
            //    group car by car.Manufacturer into carGroup
            //    select new
            //    {
            //        Name = carGroup.Key,
            //        // 3 loops
            //        Max = carGroup.Max(c => c.Combined),
            //        Min = carGroup.Min(c => c.Combined),
            //        Avg = carGroup.Average(c => c.Combined)
            //    } into result
            //    orderby result.Max descending // show the manufacturer with the most fuel efficiency car first
            //    select result;

            // extension method syntax
            var query =
                cars.GroupBy(c => c.Manufacturer)
                .Select(g =>
                {
                    // compute the statistic with one loop using a helper class
                    var results = g.Aggregate(new CarStatistics(),
                        (acc, c) => acc.Accumulate(c),
                        acc => acc.Compute());

                    return new
                    {
                        Name = g.Key,
                        Avg = results.Average,
                        Min = results.Min,
                        Max = results.Max
                    };
                })
                .OrderByDescending(r => r.Max);

            foreach (var result in query)
            {
                Console.WriteLine($"{result.Name}");
                Console.WriteLine($"\tMax: {result.Max}");
                Console.WriteLine($"\tMin: {result.Min}");
                Console.WriteLine($"\tAvg: {result.Avg:N2}");
            }


        // Get the top 3 most fuel efficient cars by country
        // GroupJoin
        // query syntax
        //var query =
        //    from manufacturer in manufacturers
        //    join car in cars on manufacturer.Name equals car.Manufacturer into carGroup
        //    select new
        //    {
        //        Manufacturer = manufacturer,
        //        Cars = carGroup
        //    } into result
        //    group result by result.Manufacturer.Headquarters;

        // extension method syntax
        //var query =
        //    manufacturers.GroupJoin(cars, m => m.Name, c => c.Manufacturer, (m, g) => new
        //    {
        //        Manufacturer = m,
        //        Cars = g
        //    })
        //    .GroupBy(m => m.Manufacturer.Headquarters);

        //foreach (var group in query)
        //{
        //    Console.WriteLine($"{group.Key}"); // key is the group by 'Manufacturer' value
        //    foreach (var car in group.SelectMany(g => g.Cars).OrderByDescending(c => c.Combined).Take(3))
        //    {
        //        Console.WriteLine($"\t{car.Name}: {car.Combined}");
        //    }
        //}

        // get the 2 most fuel efficient cars by manufacturer with headquarters order by ascending manufacturer name
        // query syntax, group joining
        //var query =
        //    from manufacturer in manufacturers
        //    join car in cars on manufacturer.Name equals car.Manufacturer into carGroup
        //    orderby manufacturer.Name
        //    select new
        //    {
        //        Manufacturer = manufacturer,
        //        Cars = carGroup
        //    };

        // extension method syntax
        //var query =
        //    manufacturers.GroupJoin(cars, m => m.Name, c => c.Manufacturer, (m, g) =>
        //    new
        //    {
        //        Manufacturer = m,
        //        Cars = g
        //    })
        //    .OrderBy(m => m.Manufacturer.Name);


        //foreach (var group in query)
        //{
        //    Console.WriteLine($"{group.Manufacturer.Name} - {group.Manufacturer.Headquarters}:"); // key is the group by 'Manufacturer' value
        //    foreach (var car in group.Cars.OrderByDescending(c => c.Combined).Take(2))
        //    {
        //        Console.WriteLine($"\t{car.Name}: {car.Combined}");
        //    }
        //}

        // get the 2 most fuel efficient cars by manufacturer order by ascending manufacturer name
        // query syntax
        //var query =
        //    from car in cars
        //    group car by car.Manufacturer.ToUpper() into manufacturer
        //    orderby manufacturer.Key
        //    select manufacturer;

        // extension method syntax, it's simpler in this case
        //var query = cars.GroupBy(c => c.Manufacturer.ToUpper())
        //                .OrderBy(g => g.Key);

        // get the 2 most fuel efficient cars by manufacturer
        //var query =
        //    from car in cars
        //    group car by car.Manufacturer;

        //foreach(var group in query)
        //{
        //    Console.WriteLine(group.Key); // key is the group by 'Manufacturer' value
        //    foreach (var car in group.OrderByDescending(c => c.Combined).Take(2))
        //    {
        //        Console.WriteLine($"\t{car.Name}: {car.Combined}");
        //    }
        //}

        // get the most fuel efficient car's manufacturer headquarters, the car name and the combined efficiency
        // query syntax joining on 1 field

        //var query =
        //    from car in cars
        //    join manufacturer in manufacturers
        //        on car.Manufacturer equals manufacturer.Name
        //    orderby car.Combined descending, car.Name ascending
        //    select new
        //    {
        //        manufacturer.Headquarters,
        //        car.Name,
        //        car.Combined
        //    };


        // joining on manufacturer name and year
        //var query =
        //    from car in cars
        //    join manufacturer in manufacturers
        //        on new { car.Manufacturer, car.Year } equals new { Manufacturer = manufacturer.Name, manufacturer.Year }
        //    orderby car.Combined descending, car.Name ascending
        //    select new
        //    {
        //        manufacturer.Headquarters,
        //        car.Name,
        //        car.Combined
        //    };

        // extensions method syntax joining on 2 fields
        // 1. Efficient
        //var query =
        //    cars.Join(manufacturers,
        //                c => new { c.Manufacturer, c.Year },
        //                m => new { Manufacturer = m.Name, m.Year },
        //                (c, m) => new
        //                {
        //                    m.Headquarters,
        //                    c.Name,
        //                    c.Combined
        //                })
        //        .OrderByDescending(r => r.Combined)
        //        .ThenBy(r => r.Name);

        // 1. Efficient
        //var query =
        //    cars.Join(manufacturers,
        //                c => c.Manufacturer,
        //                m => m.Name,
        //                (c, m) => new
        //                {
        //                    m.Headquarters,
        //                    c.Name,
        //                    c.Combined
        //                })
        //        .OrderByDescending(r => r.Combined)
        //        .ThenBy(r => r.Name);

        // 2. Not as efficient
        //var query =
        //    cars.Join(manufacturers,
        //                c => c.Manufacturer,
        //                m => m.Name,
        //                (c, m) => new
        //                // projecting
        //                {
        //                    Car = c,
        //                    Manufacturer = m
        //                })
        //        .OrderByDescending(c => c.Car.Combined)
        //        .ThenBy(c => c.Car.Name)
        //        .Select(c => new
        //        {
        //            c.Manufacturer.Headquarters,
        //            c.Car.Name,
        //            c.Car.Combined
        //        });

        //foreach (var car in query.Take(10))
        //{
        //    Console.WriteLine($"{car.Headquarters} - {car.Name}: {car.Combined}");
        //}



        //var query = from car in cars
        //            where car.Manufacturer == "BMW" && car.Year == 2016
        //            orderby car.Combined descending, car.Name ascending
        //            select new
        //            {
        //                car.Manufacturer,
        //                car.Name,
        //                car.Combined
        //            };

        //var query = cars.Where(c => c.Manufacturer == "BMW" && c.Year == 2016)
        //        .OrderByDescending(c => c.Combined)
        //        .ThenBy(c => c.Name)
        //        .Select(c => c);

        //foreach (var car in query)
        //{
        //    Console.WriteLine($"{car.Manufacturer} {car.Name} : {car.Combined}");
        //}

        //var result = cars.SelectMany(c => c.Name)
        //                 .OrderBy(c => c);
        //foreach (var name in result)
        //{
        //    Console.WriteLine($"{name}");
        //}

    }

        private static List<Manufacturer> ProcessManufacturers(string path)
        {
            var query = File.ReadAllLines(path)
                .Where(l => l.Length > 1)
                .Select(l =>
                {
                    var columns = l.Split(",");
                    return new Manufacturer
                    {
                        Name = columns[0],
                        Headquarters = columns[1],
                        Year = int.Parse(columns[2])
                    };
                });

            return query.ToList();
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

