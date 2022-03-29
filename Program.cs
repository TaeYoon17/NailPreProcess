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
                Console.Write("lable 이름: ");
                string label = Console.ReadLine();
                if (label == "" || label == null) throw new Exception("isNull!");
                string Path = @"C:\Users\MY_PC\Desktop\DataSet\RawDataSet\" + label;
                string newPath = @"C:\Users\MY_PC\Desktop\DataSet\CropDataSet\" + label;
                CropDirectories directories = new(label);
                directories.Add_SubDirectory(Path, newPath);
                directories.Run();
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
