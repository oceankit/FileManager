using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace CompressUncompress
{
    class Program
    {
        public static string path = @"C:/Users/Sattar/Downloads/Stuff/";
        public static string savePath = @"C:/Users/Sattar/Downloads/Compressed/";
        public static string destPath = @"C:/Users/Sattar/Downloads/Uncompressed/";

        public static void Compress(DirectoryInfo directorySelected)
        {
            foreach (FileInfo fileToCompress in directorySelected.GetFiles())
            {
                using (FileStream originalFileStream = fileToCompress.OpenRead())
                {
                    if ((File.GetAttributes(fileToCompress.FullName) &
                       FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".gz")
                    {
                        using (FileStream compressedFileStream = File.Create(fileToCompress.FullName + ".gz"))
                        {
                            using (GZipStream compressionStream = new GZipStream(compressedFileStream,
                               CompressionMode.Compress))
                            {
                                originalFileStream.CopyTo(compressionStream);

                            }
                        }
                        FileInfo info = new FileInfo(savePath + "\\" + fileToCompress.Name + ".gz");
                        Console.WriteLine("Compressed {0} from {1} to {2} bytes.",
                        fileToCompress.Name, fileToCompress.Length.ToString(), info.Length.ToString());
                    }

                }
            }
        }

        public static void Decompress(FileInfo fileToDecompress)
        {
            using (FileStream originalFileStream = fileToDecompress.OpenRead())
            {
                string currentFileName = fileToDecompress.FullName;
                string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                using (FileStream decompressedFileStream = File.Create(newFileName))
                {
                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                        Console.WriteLine("Decompressed: {0}", fileToDecompress.Name);
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            

            List<FileInfo> paths = new List<FileInfo>();
            DirectoryInfo dir = new DirectoryInfo(path);
            foreach (var n in dir.GetFiles())
            {
                paths.Add(n);
            }
            Compress(dir);

            List<FileInfo> uncom = new List<FileInfo>();
            DirectoryInfo newdir = new DirectoryInfo(savePath);
            foreach (var d in newdir.GetFiles())
            {
                uncom.Add(d);
            }

            for(int i=0; i<uncom.Count; i++)
            {
                Decompress(uncom[i]);
            }

            


        }
    }
}
