using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Drawing;

namespace FileManager
{
    class Program
    {
        public static void ConsoleWriteImage(Bitmap bmpSrc)
        {
            int sMax = 39;
            decimal percent = Math.Min(decimal.Divide(sMax, bmpSrc.Width), decimal.Divide(sMax, bmpSrc.Height));
            Size resSize = new Size((int)(bmpSrc.Width * percent), (int)(bmpSrc.Height * percent));
            Func<System.Drawing.Color, int> ToConsoleColor = c =>
            {
                int index = (c.R > 128 | c.G > 128 | c.B > 128) ? 8 : 0;
                index |= (c.R > 64) ? 4 : 0;
                index |= (c.G > 64) ? 2 : 0;
                index |= (c.B > 64) ? 1 : 0;
                return index;
            };
            Bitmap bmpMin = new Bitmap(bmpSrc, resSize.Width, resSize.Height);
            Bitmap bmpMax = new Bitmap(bmpSrc, resSize.Width * 2, resSize.Height * 2);
            for (int i = 0; i < resSize.Height; i++)
            {
                for (int j = 0; j < resSize.Width; j++)
                {
                    Console.ForegroundColor = (ConsoleColor)ToConsoleColor(bmpMin.GetPixel(j, i));
                    Console.Write("██");
                }

                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write("    ");

                for (int j = 0; j < resSize.Width; j++)
                {
                    Console.ForegroundColor = (ConsoleColor)ToConsoleColor(bmpMax.GetPixel(j * 2, i * 2));
                    Console.BackgroundColor = (ConsoleColor)ToConsoleColor(bmpMax.GetPixel(j * 2, i * 2 + 1));
                    Console.Write("▀");

                    Console.ForegroundColor = (ConsoleColor)ToConsoleColor(bmpMax.GetPixel(j * 2 + 1, i * 2));
                    Console.BackgroundColor = (ConsoleColor)ToConsoleColor(bmpMax.GetPixel(j * 2 + 1, i * 2 + 1));
                    Console.Write("▀");
                }
                System.Console.WriteLine();
            }
        }

        public static bool run = true;
        public static Stack<string> stk;
        public static Stack<int> cursorBefore;
        public static DirectoryInfo dir;
        public static List<FileSystemInfo> sys_dir = new List<FileSystemInfo>();
        public static int position = 0, before = 0;

        static void DrawPosition(int before, int position, List<FileSystemInfo> dirs)
        {
            string display;
            Console.SetCursorPosition(0, before);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            display = (dirs[before].Name.Length < 20) ? dirs[before].Name : dirs[before].Name.Substring(0, 20);
            Console.WriteLine(display);
            Console.SetCursorPosition(0, position);
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.Yellow;
            display = (dirs[position].Name.Length < 20) ? dirs[position].Name : dirs[position].Name.Substring(0, 20);
            Console.WriteLine(display);
            Console.ResetColor();
        }
        static void DrawDirectory(string path)
        {
            Console.Clear();
            dir = new DirectoryInfo(path);
            sys_dir.Clear();
            foreach (var n in dir.GetDirectories())
            {
                sys_dir.Add(n);
            }
            foreach (var n in dir.GetFiles())
            {
                sys_dir.Add(n);
            }
            string display;
            for (int i = 0; i < sys_dir.Count; i++)
            {
                display = (sys_dir[i].Name.Length < 20) ? sys_dir[i].Name : sys_dir[i].Name.Substring(0, 20);
                //if (sys_dir[i].GetType() == typeof(DirectoryInfo))
                //{
                //    Console.BackgroundColor = ConsoleColor.Magenta;
                //    Console.ForegroundColor = ConsoleColor.White;
                //}
                //else if (sys_dir[i].GetType() == typeof(FileInfo))
                //{
                //    Console.BackgroundColor = ConsoleColor.DarkYellow;
                //    Console.ForegroundColor = ConsoleColor.DarkBlue;
                //}
                Console.WriteLine(display);
                //Console.ResetColor();
            }
        }

        public static readonly List<string> ImageExtensions = new List<string> { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };

        static void Main(string[] args)
        {
            stk = new Stack<string>();
            cursorBefore = new Stack<int>();
            string path = @"C:\";
            DrawDirectory(path);

            while (run == true)
            {
                ConsoleKey pressedKey = Console.ReadKey(true).Key;
                if (pressedKey == ConsoleKey.UpArrow)
                {
                    if (position == 0)
                    {
                        before = position;
                        position = sys_dir.Count - 1;
                        DrawPosition(before, position, sys_dir);
                    }
                    else
                    {
                        before = position;
                        position--;
                        DrawPosition(before, position, sys_dir);
                    }
                }
                else if (pressedKey == ConsoleKey.DownArrow)
                {
                    if (position == sys_dir.Count - 1)
                    {
                        before = position;
                        position = 0;
                        DrawPosition(before, position, sys_dir);
                    }
                    else
                    {
                        before = position;
                        position++;
                        DrawPosition(before, position, sys_dir);
                    }
                }
                if (pressedKey == ConsoleKey.Enter)
                {
                    if (sys_dir[position] is DirectoryInfo)
                    {
                        FileIOPermission f = new FileIOPermission(FileIOPermissionAccess.Read, sys_dir[position].FullName);
                        if (SecurityManager.IsGranted(f))
                        {
                            cursorBefore.Push(position);
                            stk.Push(path);
                            path = sys_dir[position].FullName;
                            position = 0;
                            DrawDirectory(path);
                        }

                    }
                    else if (ImageExtensions.Contains(Path.GetExtension(sys_dir[position].FullName).ToUpper()))
                    {
                        Console.Clear();
                        Bitmap btm = new Bitmap(sys_dir[position].FullName, true);
                        ConsoleWriteImage(btm);
                    }
                    else if (sys_dir[position] is FileInfo) 
                    {
                        System.Diagnostics.Process.Start(sys_dir[position].FullName);
                    }
                }
                if (pressedKey == ConsoleKey.Escape)
                {
                    if (stk.Count != 0)
                    {
                        position = cursorBefore.Pop();
                        path = stk.Pop();
                        DrawDirectory(path);
                        DrawPosition(before, position, sys_dir);
                    }
                }
            }
        }
    }
}
