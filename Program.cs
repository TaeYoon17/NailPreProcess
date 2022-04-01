using OpenCvSharp;
using ImageManipulateion;
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
                string[] labels = { "" };
                
                foreach (string label in labels)
                {
                    Console.WriteLine(label);
                    string Path = @"C:\Users\kim05\OneDrive - Sejong University\연구실\랩미팅\03_31\" + label;
                    string newPath = @"C:\Users\kim05\Desktop\실험 폴더\" + label;
                    Directories directories = new(label);
                    directories.Add_SubDirectory(Path, newPath);
                    directories.Crop();
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
