using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    class Program
    {
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
                Console.WriteLine(display);
            }
        }

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
