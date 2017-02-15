using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileWatchingDemo
{
    class Program
    {

        static void Main(string[] args)
        {
            string path = @"C:/Users/Sattar/Downloads/Stuff/";
            FileSystemWatcher fsw = new FileSystemWatcher(path, "*.docx");
            fsw.Changed += new FileSystemEventHandler(OnChanged);
            fsw.EnableRaisingEvents = true;
            fsw.NotifyFilter = NotifyFilters.Size | NotifyFilters.Attributes;
            Console.ReadKey();
        }

        public static void OnChanged(object source, FileSystemEventArgs e)
        {
            Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
        }
    }
}
