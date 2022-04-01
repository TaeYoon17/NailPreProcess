using OpenCvSharp;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
namespace CropFiles
{
    public struct PathStruct
    {
        public string rawPath, newPath;
        public PathStruct(string rawPath, string newPath)
        {
            this.newPath = newPath;
            this.rawPath = rawPath;
        }
    }
    internal class CropDirectory
    {
        public static void Run(PathStruct path,int number,string labelName)
        {
            var rawPath = path.rawPath;
            var newPath = path.newPath;
            if (Directory.Exists(rawPath))
            {
                if (!Directory.Exists(newPath))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(newPath);
                    directoryInfo.Create();
                }
                string[] files = Directory.GetFiles(rawPath, "*.jpg");
                foreach ((string value, int index) fileItem in files.Select((value, index) => (value, index)))
                {
                    try{
                    string file = fileItem.value;
                    int index = fileItem.index;
                    Mat src = Cv2.ImRead(file, ImreadModes.Unchanged);
                    Mat dst = new(new OpenCvSharp.Size(1, 1), MatType.CV_8UC3);
                    Cv2.Resize(src, dst, new OpenCvSharp.Size(416, 416), 0, 0);

                    Cv2.ImWrite($"{newPath}\\{labelName}_{number}_{index}.jpg", dst);
                    }
                    catch
                    {                    }
                }
                Cv2.WaitKey(0);
            }
            else
            {
                Console.WriteLine("Raw 경로가 이상합니다!!");
            }
        }
    }
    delegate void Perform(PathStruct path,int number,string labelName);
    public class CropDirectories
    {
        public List<PathStruct> directories;
        private Perform perform;
        private readonly string label;
        public CropDirectories(string labelName)
        {
            directories = new List<PathStruct>();
            perform = new Perform(CropDirectory.Run);
            this.label = labelName;
        }
        public void Add(PathStruct directory) => directories.Add(directory);
        public void Add_SubDirectory(PathStruct path)=>Add_SubDirectory(path.rawPath,path.rawPath);
        public void Add_SubDirectory(string rawPath,string newPath)
        {
            Queue<string> quPath = new Queue<string>(new string[] { rawPath });
            Queue<string> acc = getSubDirectoriesName(quPath, new Queue<string>(quPath));
            foreach (var s in acc)
            {
                Add(new PathStruct(s, newPath));
            }
        }
        public void Run()
        {
            foreach (var directory in directories.Select((value, index) => (value, index)))
                perform(directory.value, directory.index,this.label);
        }
        private Queue<string> getSubDirectoriesName(Queue<string> Paths, Queue<string> acc)
        {
            if (Paths.Count == 0) return acc;
            DirectoryInfo Info = new DirectoryInfo(Paths.Dequeue());
            if (Info.Exists)
            {
                DirectoryInfo[] CInfo = Info.GetDirectories("*", System.IO.SearchOption.AllDirectories);
                foreach (DirectoryInfo info in CInfo)
                {
                    Paths.Enqueue(info.FullName);
                    acc.Enqueue(info.FullName);
                }
            }
            return getSubDirectoriesName(Paths, acc);
        }
    }
}
