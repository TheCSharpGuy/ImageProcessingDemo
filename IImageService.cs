using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using System;

namespace ImageServices
{
    public interface IImageService
    {
        public void ResizeImageWebpAsync(string sourceImageFileName, string targetImageFileName, int maxWidth, int maxHeight, bool compress);
        public void ConvertToWebpAsync(string sourceImageFileName, string targetImageFileName);
    }
}

namespace ImageServices
{
    public class ImageService : IImageService
    {
        /// <summary>
        /// ConvertToWebpAsync
        /// </summary>
        /// <param name="sourceImageFileName">Provide full path along with file name and extension of the source file</param>
        /// <param name="targetImageFileName">Provide full path and file name with '.webp' extension of the target file</param>
        public async void ConvertToWebpAsync(string sourceImageFileName, string targetImageFileName)
        {
            targetImageFileName = targetImageFileName.ToLower();

            Image image = Image.Load(sourceImageFileName);
            var enc = new WebpEncoder
            {
                Method = WebpEncodingMethod.Level2
            };
            await image.SaveAsync(targetImageFileName, enc);
        }
        /// <summary>
        /// ResizeImageWebpAsync
        /// </summary>
        /// <param name="sourceImageFileName">Provide full path along with file name and extension of the source file</param>
        /// <param name="targetImageFileName">Provide full path and file name with '.webp' extension of the target file</param>
        /// <param name="maxWidth">Provide max width in pixels for the new file</param>
        /// <param name="maxHeight">Provide max height in pixels for the new file</param>
        /// <param name="compress"></param>
        public async void ResizeImageWebpAsync(string sourceImageFileName, string targetImageFileName, int maxWidth=0, int maxHeight=0, bool compress=false)
        {
            int newWidth;
            int newHeight;
            Image image = Image.Load(sourceImageFileName);
            //check if the with or height of the image exceeds the maximum specified, if so calculate the new dimensions
            if (image.Width > maxWidth || image.Height > maxHeight)
            {
                double ratioX = (double)maxWidth / image.Width;
                double ratioY = (double)maxHeight / image.Height;
                double ratio = Math.Min(ratioX, ratioY);

                newWidth = (int)(image.Width * ratio);
                newHeight = (int)(image.Height * ratio);
            }
            else
            {
                newWidth = image.Width;
                newHeight = image.Height;
            }

            //start the resize with a new image
            ResizeOptions resizeOptions = new()
            {
                Compand = compress,
                Size = new(newWidth, newHeight)
            };
            var enc = new WebpEncoder
            {
                Method = WebpEncodingMethod.Level2
            };
            image.Mutate(x => x.Resize(resizeOptions));
            await image.SaveAsync(targetImageFileName, enc);
        }
    }
}
