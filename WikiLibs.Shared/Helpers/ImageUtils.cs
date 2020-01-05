using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace WikiLibs.Shared.Helpers
{
    public class ImageUtils
    {
        /// <summary>
        /// Resize an image with respect of aspect ratio
        /// </summary>
        /// <param name="source">Source size</param>
        /// <param name="target">Target size</param>
        /// <returns>The size and position of the image to draw</returns>
        public static Rectangle GetImageSize(Size source, Size target)
        {
            double ratioX = (double)target.Width / source.Width;
            double ratioY = (double)target.Height / source.Height;
            double ratio = ratioX > ratioY ? ratioY : ratioX;
            var res = new Rectangle();
            res.Width = (int)(source.Width * ratio);
            res.Height = (int)(source.Height * ratio);
            res.X = (int)((target.Width - source.Width * ratio) / 2);
            res.Y = (int)((target.Height - source.Height * ratio) / 2);
            return (res);
        }

        public static Image ResizeImage(Image source, Size target)
        {
            if (source.Width > target.Width || source.Height > target.Height)
            {
                var image = new Bitmap(target.Width, target.Height);
                using (var graphics = Graphics.FromImage(image))
                {
                    graphics.CompositingQuality = CompositingQuality.HighSpeed;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    var rect = GetImageSize(source.Size, target);
                    graphics.Clear(Color.White);
                    graphics.DrawImage(source, rect.X, rect.Y, rect.Width, rect.Height);
                }
                return (image);
            }
            else
                return (source);
        }

        public static byte[] ResizeImage(Stream source, Size target, ImageFormat format)
        {
            var stream = new MemoryStream();

            using (var img = new Bitmap(source))
            {
                var res = ResizeImage(img, target);
                res.Save(stream, format);
            }
            return (stream.ToArray());
        }
    }
}
