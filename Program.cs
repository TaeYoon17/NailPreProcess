using OpenCvSharp;
using ImageManipulateion;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
namespace NailPreProcess
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 라벨 배열 안 넣어주면 자동으로 모든 Day 폴더 처리 (FirstDay,SecondDay...)
            string[] labels = { };
            // 끝에 \ 붙여주기 -> 디렉토리 하위 경로 표시
            string inputPath = @"C:\Users\MY_PC\Desktop\NailCrawling\DataSet\";
            string inputNewPath = @"C:\Users\MY_PC\Desktop\NailCrawling\cropDataSet\";
            try
            {
                List<string> Labels = labels.Length!=0 ? new(labels) : (new List<DirectoryInfo>(
                    new DirectoryInfo(inputPath).GetDirectories())).Select(d => d.Name).ToList();
                Labels.ForEach(label =>
                {
                    string Path=inputPath+label;
                    string newPath=inputNewPath+label;
                    Directories directories = new(label);
                    //Add_SubDirectory(기존 이미지 폴더, 새로운 이미지 폴더).나머지 전치리 함수 실행()
                    directories.Add_SubDirectory(Path, newPath).Crop();
                    //directories.Add_SubDirectory(Path, newPath).Reverse();
                });
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
