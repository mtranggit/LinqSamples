using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Cars
{
    class Program
    {
        static void Main(string[] args)
        {

            // dangerous but just for demo purpose
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<CarDb>());

            //InsertData();
            QueryData();

            //createXmlDocument();
            //queryXmlDocument();

            //var cars = ProcessFile("fuel.csv");
            //var manufacturers = ProcessManufacturers("manufacturers.csv");


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
            //var query =
            //    cars.GroupBy(c => c.Manufacturer)
            //    .Select(g =>

            //    {
            //        // compute the statistic with one loop using a helper class
            //        var results = g.Aggregate(new CarStatistics(),
            //            (acc, c) => acc.Accumulate(c),
            //            acc => acc.Compute());

            //        return new
            //        {
            //            Name = g.Key,
            //            Avg = results.Average,
            //            Min = results.Min,
            //            Max = results.Max
            //        };
            //    })
            //    .OrderByDescending(r => r.Max);

            //foreach (var result in query)
            //{
            //    Console.WriteLine($"{result.Name}");
            //    Console.WriteLine($"\tMax: {result.Max}");
            //    Console.WriteLine($"\tMin: {result.Min}");
            //    Console.WriteLine($"\tAvg: {result.Avg:N2}");
            //}


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

        private static void QueryData()
        {
            var db = new CarDb();
            db.Database.Log = Console.WriteLine;

            // query syntax
            var query =
                from car in db.Cars
                group car by car.Manufacturer into manufacturer
                select new
                {
                    Name = manufacturer.Key,
                    Cars = (from car in manufacturer
                            orderby car.Combined descending
                            select car).Take(2)
                };

            // extension method syntax
            //var query = db.Cars
            //    .GroupBy(c => c.Manufacturer)
            //    .Select(g => new
            //    {
            //        Name = g.Key,
            //        Cars = g.OrderByDescending(c => c.Combined).Take(2)
            //    });

            foreach (var group in query)
            {
                Console.WriteLine($"{group.Name}");
                foreach(var car in group.Cars)
                {
                    Console.WriteLine($"\t{car.Name}: {car.Combined}");
                }
            }
            //{
            //    Console.WriteLine($"{car.Name}: {car.Combined}");
            //}
        }

        private static void InsertData()
        {
            var cars = ProcessFile("fuel.csv");
            var db = new CarDb();
            db.Database.Log = Console.WriteLine;

            if (!db.Cars.Any())
            {
                foreach (var car in cars)
                {
                    db.Cars.Add(car);
                }
                db.SaveChanges();
            }
        }

        private static void queryXmlDocument()
        {
            var document = XDocument.Load("fuel.xml");
            var ns = (XNamespace)"http://pluralsight.com/cars/2016";
            var ex = (XNamespace)"http://pluralsight.com/cars/2016/ex";
            var query =
                    // more explicit way
                    from element in document.Element(ns + "Cars")?.Elements(ex + "Car") ?? Enumerable.Empty<XElement>( )
                    //from element in document.Descendants("Car")
                    where element.Attribute("Manufacturer")?.Value == "BMW"
                    select element.Attribute("Name").Value;

            foreach(var name in query)
            {
                Console.WriteLine($"{name}");
            }
        }

        private static void createXmlDocument()
        {
            var records = ProcessFile("fuel.csv");

            // create xml namespace
            var ns = (XNamespace)"http://pluralsight.com/cars/2016";
            var ex = (XNamespace)"http://pluralsight.com/cars/2016/ex";

           // 1. Foreach looping approach
           /* 
           var document = new XDocument();
           var cars = new XElement("Cars");
           foreach (var record in records)
           {
               // 1a. Normal long approach
               //var car = new XElement("Car");
               //// element oriented
               ////var name = new XElement("Name", record.Name);
               ////var combined = new XElement("Combined", record.Combined);

               //// attribute oriented
               //var name = new XAttribute("Name", record.Name);
               //var combined = new XAttribute("Combined", record.Combined);

               //car.Add(name);
               //car.Add(combined);
               //cars.Add(car);
               // 1b. Functional Construction approach
               var car = new XElement("Car", 
                   new XAttribute("Name", record.Name),  
                   new XAttribute("Combined", record.Combined),  
                   new XAttribute("Manufacturer", record.Manufacturer)
               );

               cars.Add(car);
           }
           */

           // 2. Linq approach
           // 2a. query syntax
           var document = new XDocument();
            var cars = new XElement(ns + "Cars");
            //var cars = new XElement("Cars");
            var elements =
                    from record in records
                    select new XElement(ex + "Car",
                    //select new XElement(ns + "Car",
                    //select new XElement("Car",
                        new XAttribute("Name", record.Name),
                        new XAttribute("Combined", record.Combined),
                        new XAttribute("Manufacturer", record.Manufacturer)
                    );

            cars.Add(new XAttribute(XNamespace.Xmlns + "ex", ex));
            cars.Add(elements);

            document.Add(cars);
            document.Save("fuel.xml");
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

