using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using Identity.Interfaces;

namespace Identity.Services
{
    public class ImageService : IImageService
    {
        /// <summary>
        /// Get the default image of AboutUser object as an array of bytes.
        /// </summary>
        public byte[] DefaultImage
        {
            get
            {
                var defaultPath = AppContext.BaseDirectory + "App_Data//Upload//Default.png";
                var bitmap = new Bitmap(defaultPath, true);
                var image = (byte[])new ImageConverter().ConvertTo(bitmap, typeof(byte[]));
                bitmap.Dispose();
                return image;
            }
        }

        /// <summary>
        /// Create an image for AboutUser object. If there are not files to create a new one, will be created the default image.
        /// </summary>
        /// <param name="id">AboutUser's id value.</param>
        /// <param name="files">List of files that were uploaded.</param>
        public void CreateImage(int id, HttpFileCollectionBase files)
        {
            if (files.Count > 0)
            {
                var file = files[0];
                if (file.ContentLength > 0)
                {
                    var fileName = id + "." + file.FileName.Split('.').Last();
                    var app = AppContext.BaseDirectory + "App_Data//Upload//";
                    var path = Path.Combine(app, fileName);
                    file.SaveAs(path);
                }                
            }
        }

        /// <summary>
        /// Update an image for AboutUser object. If there are no files to update AboutUser's image, will be no changes.
        /// </summary>
        /// <param name="id">AboutUser's id value.</param>
        /// <param name="files">List of files that were uploaded.</param>
        public void UpdateImage(int id, HttpFileCollectionBase files)
        {
            if (files.Count > 0)
            {
                var file = files[0];
                if (file.ContentLength > 0)
                {
                    var fileName = id + "." + file.FileName.Split('.').Last();
                    var app = AppContext.BaseDirectory + "App_Data//Upload//";
                    var path = Path.Combine(app, fileName);
                    DeleteImage(id);
                    file.SaveAs(path);
                }
            }
        }

        /// <summary>
        /// Get the image for AboutUser with the received id.
        /// </summary>
        /// <param name="id">AboutUser's id value.</param>
        /// <returns>The AboutUser's image as an array of bytes.</returns>
        public byte[] GetImage(int id)
        {
            var path = Directory.GetFiles(AppContext.BaseDirectory + "App_Data//Upload//", $"{id}.*");
            var defaultPath = AppContext.BaseDirectory + "App_Data//Upload//Default.png";

            var image = new Bitmap(defaultPath, true);
            if (path.Length > 0)  image = new Bitmap(path[0], true);

            var result = (byte[])new ImageConverter().ConvertTo(image, typeof(byte[]));
            image.Dispose();
            return result;
        }

        /// <summary>
        /// Delete the image for AboutUser with the received id.
        /// </summary>
        /// <param name="id">AboutUser's id value.</param>
        public void DeleteImage(int id)
        {
            var pathDelete = Directory.GetFiles(AppContext.BaseDirectory + "App_Data//Upload//", $"{id}.*");
            if (pathDelete.Length > 0)
                File.Delete(pathDelete[0]);
        }
    }
}
