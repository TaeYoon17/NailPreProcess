using OpenCvSharp;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
namespace ImageManipulateion
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
    delegate void Perform(PathStruct path,int number,string labelName);
    internal class ManipulationMethod
    {
        public static void Crop(PathStruct path,int number,string labelName)
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
                    Console.WriteLine("IndexNumber"+index);
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
        public static void Reverse(PathStruct path, int number, string labelName)
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
                List<string> files =new( Directory.GetFiles(rawPath, "*.jpg"));
                foreach ((string value, int index) fileItem in files.Select((value, index) => (value, index)))
                {
                    try
                    {
                        string file = fileItem.value;
                        int index = fileItem.index;
                        Mat src = Cv2.ImRead(file, ImreadModes.Unchanged);
                        Mat dst = new(new OpenCvSharp.Size(1, 1), MatType.CV_8UC3);
                        Cv2.Resize(src, dst, new OpenCvSharp.Size(416, 416), 0, 0);
                        Cv2.ImWrite($"{newPath}\\{labelName}_{number}_{index}.jpg", dst);
                        Console.WriteLine("자르기 완료");
                    }
                    catch
                    { }
                }
                Cv2.WaitKey(0);
            }
            else
            {
                Console.WriteLine("Raw 경로가 이상합니다!!");
            }
        }

    }
    public class Directories
    {
        public List<PathStruct> directories= new List<PathStruct>();
        private Perform perform;
        private readonly string label;
        public Directories(string labelName)
        {
            perform = null;
            this.label = labelName;
        }
        public void Add(PathStruct directory) => directories.Add(directory);
        public void Add_SubDirectory(PathStruct path)=>Add_SubDirectory(path.rawPath,path.rawPath);
        public Directories Add_SubDirectory(string rawPath,string newPath)
        {
            Queue<string> quPath = new Queue<string>(new string[] { rawPath });
            Queue<string> acc = getSubDirectoriesName(quPath, new Queue<string>(quPath));
            acc.ToList().ForEach(SubRawPath => Add(new PathStruct(SubRawPath, newPath)));
            return this;
        }
        private void Run()
        {
            if (perform == null) Console.WriteLine("실행할 delegate가 설정 안 됨");
            List<Task> taskList=new List<Task>();
            foreach ((PathStruct value, int index) directory in directories.Select((value, index) => (value, index)))
            {
                Task task = Task.Factory.StartNew(() =>perform(directory.value, directory.index, label));
                taskList.Add(task);
            }
            Task.WaitAll(taskList.ToArray());
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
        public void Reverse() { perform=new Perform(ManipulationMethod.Reverse);Run(); }
        public void Crop() { perform=new Perform(ManipulationMethod.Crop);Run(); }
    }
}
