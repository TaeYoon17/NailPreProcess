using OpenCvSharp;
using CropFiles;
using System.Collections.Generic;
using System;
namespace NailPreProcess
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string[] labels = { "sixthDay", "seventhDay" };
                
                foreach (string label in labels)
                {
                    Console.WriteLine(label);
                    string Path = @"C:\Users\GVR_LAB\Desktop\NailCrawling\RawDataSet\" + label;
                    string newPath = @"C:\Users\GVR_LAB\Desktop\NailCrawling\CropDataSet\" + label;
                    CropDirectories directories = new(label);
                    directories.Add_SubDirectory(Path, newPath);
                    directories.Run();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine("종료하려면 아무 키나 누르세요...");
            Console.ReadKey();

        }
    }
}
