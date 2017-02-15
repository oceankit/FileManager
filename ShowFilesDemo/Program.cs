using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ShowFilesDemo
{
    class Program
    {
        public static List<FileSystemInfo> allfiles = new List<FileSystemInfo>();

        public static void ShowDirectory(DirectoryInfo dir)
        {
            foreach (var s in dir.GetFiles())
                allfiles.Add(s);
            foreach (var s in dir.GetDirectories())
                ShowDirectory(new DirectoryInfo(s.FullName));
        }

        static void Main(string[] args)
        {
            //Task 1
            string path = @"C:/Users/Sattar/Downloads/";
            DirectoryInfo dir = new DirectoryInfo(path);
            ShowDirectory(dir);
            foreach (var n in allfiles)
                Console.WriteLine(n.Name);
        }
    }
}
