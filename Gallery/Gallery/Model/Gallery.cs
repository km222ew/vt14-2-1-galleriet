using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.IO;
using System.Drawing;

namespace Gallery.Model
{
    public class Gallery
    {
        private static readonly Regex ApprovedExtensions;
        private static string PhysicalUploadedImagesPath;
        private static readonly Regex SantizePath;

        static Gallery()
        {
            ApprovedExtensions = new Regex("^.*.(gif|jpg|png)$", RegexOptions.IgnoreCase);
            PhysicalUploadedImagesPath = Path.Combine(AppDomain.CurrentDomain.GetData("APPBASE").ToString(), "Content", "Images");

            var invalidChars = new string(Path.GetInvalidFileNameChars());
            SantizePath = new Regex(string.Format("[{0}]", Regex.Escape(invalidChars)));
        }

        public IEnumerable<string> GetImagesNames()
        {
            var dirInf = new DirectoryInfo(PhysicalUploadedImagesPath);
            var images = dirInf.GetFiles();

            List<string> imageNameList = new List<string>();

            foreach (var image in images)
            {
                if (ApprovedExtensions.IsMatch(image.ToString()))
                {
                    imageNameList.Add(image.ToString());
                }
            }

            imageNameList.Sort();
            return imageNameList;
        }

        public bool ImageExists(string name)
        {
            return File.Exists(Path.Combine(PhysicalUploadedImagesPath, name));
        }

        private static bool IsValidImage(Image image)
        {
            if (image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Gif.Guid ||
                image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Jpeg.Guid ||
                image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Png.Guid)
            {
                return true;
            }
            return false;
        }

        public string SaveImage(Stream stream, string fileName)
        {
            var image = System.Drawing.Image.FromStream(stream);
            fileName = SantizePath.Replace(fileName, "");

            if (IsValidImage(image))
            {
                throw new ArgumentException();
            }

            if (ImageExists(fileName))
            {
                var fileNameWOExtension = Path.GetFileNameWithoutExtension(fileName);
                var extension = Path.GetExtension(fileName);
                int copyCount = 1;

                while (ImageExists(fileName))
                {
                    fileName = String.Format("{0}({1}){2}", fileNameWOExtension, copyCount, extension);
                    copyCount++;
                }
            }

            image.Save(Path.Combine(PhysicalUploadedImagesPath, fileName));

            var thumbnail = image.GetThumbnailImage(60, 45, null, System.IntPtr.Zero);
            thumbnail.Save(Path.Combine(PhysicalUploadedImagesPath, "Thumbnails", fileName));

            return fileName;
        }
    }
}