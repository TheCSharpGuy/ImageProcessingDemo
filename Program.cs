using ImageServices;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ImageManipulationDemo
{
    static class Program
    {
        public const string spath = @"C:\source\Demos\ImageProcessingDemo";
        static void Main(string[] args)
        {
            Directory.CreateDirectory($"{spath}\\output");
            string[] filePaths = Directory.GetFiles($"{spath}\\input");
            ImageService resizeImage = new();
            Parallel.ForEach(filePaths, 
                new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount } 
                , file =>
            {
                var id = Guid.NewGuid().ToString();
                Console.WriteLine($"Converting file:{file}");
                resizeImage.ConvertToWebpAsync(file, $"{spath}//output//_{id}.webp");
                Console.WriteLine($"Creating thumbnail & converting file:{file}");
                resizeImage.ResizeImageWebpAsync(file, $"{spath}//output//thumb_{id}.webp",250, 250);               
            });
        }
    }
}