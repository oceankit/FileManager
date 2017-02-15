using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadWriteFiles
{
    class Program
    {

        public static void Save(List<string> ls)
        {
            if (File.Exists("data.txt") != true)
            {
                using (FileStream fs = File.Create("data.txt"))
                {
                    StreamWriter sw = new StreamWriter(fs);
                    foreach (string s in ls)
                    {
                        sw.WriteLine(s + ".");
                    }
                    sw.Close();
                }
            }
            else
            {
                using (FileStream fs = File.OpenWrite("data.txt"))
                {
                    StreamWriter sw = new StreamWriter(fs);
                    foreach (string s in ls)
                    {
                        sw.WriteLine(s + ".");
                    }
                    sw.Close();
                }
            }
        }

        public static void Show()
        {
            if (File.Exists("data.txt"))
            {
                string s = "";
                using (StreamReader sr = File.OpenText("data.txt"))
                {
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine("-" + s.Substring(0, s.Length - 1));
                    }
                    sr.Close();
                }
            }
        }

        static void Main(string[] args)
        {
            List<string> MyFriends = new List<string>();
            MyFriends.Add("Aliya");
            MyFriends.Add("Madina");
            MyFriends.Add("Anna");

            Save(MyFriends);
            Show();

            List<string> NewFriends = new List<string>();
            NewFriends.Add("Askar");
            NewFriends.Add("Askhat");
            NewFriends.Add("Sanzhar");
            Save(NewFriends);
            Show();
            
        }
    }
}
