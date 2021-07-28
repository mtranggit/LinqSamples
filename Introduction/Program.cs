using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Introduction
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\windows";
            Console.WriteLine("******Show Large Files Without Linq*********");
            ShowLargeFilesWithoutLinq(path);
            Console.WriteLine("******Show Large Files With Linq*********");
            ShowLargeFilesWithLinq(path);
        }

        private static void ShowLargeFilesWithLinq(string path)
        {
            // Extension Method Syntax
            //var query = new DirectoryInfo(path).GetFiles()
            //                .OrderByDescending(f => f.Length)

            // Query Syntax
            var query = from file in new DirectoryInfo(path).GetFiles()
                         orderby file.Length descending
                         select file;

            foreach(var file in query.Take(5))
            {
                Console.WriteLine($"{file.Name, -20}: {file.Length, 10:N0}");
            }
        }

        private static void ShowLargeFilesWithoutLinq(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            FileInfo[] files = directory.GetFiles();
            Array.Sort(files, new FileInfoComparer());

            for(var i= 0; i < 5; i++)
            //foreach(var file in files)
            {
                var file = files[i];
                // left justify name and right justify size
                Console.WriteLine($"{file.Name, -20}: {file.Length, 10:N0}");
            }
        }

   }
    public class FileInfoComparer : IComparer<FileInfo>
    {
        public int Compare(FileInfo x, FileInfo y)
        {
            return y.Length.CompareTo(x.Length);
        }
    }
 
}
