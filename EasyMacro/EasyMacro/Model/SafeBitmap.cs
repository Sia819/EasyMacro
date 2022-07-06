using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMacro.Model
{
    public class SafeBitmap : IDisposable
    {
        private readonly object _bitmapLock = new object();

        private Bitmap bitmap;

        public int Width { get; set; }
        public int Height { get; set; }

        public Bitmap Snapshot100px
        {
            get
            {
                lock (_bitmapLock)
                {
                    return ResizeImage(bitmap, 100, 100);
                }
            }
        }

        public Bitmap DangerousGetBitmap() => bitmap;

        /// <summary> Constructor </summary>
        public Bitmap Snapshot
        {
            get
            {
                lock (_bitmapLock)
                    return new Bitmap(bitmap);
            }
            set
            {
                Bitmap oldSnapshot;
                Bitmap newSnapshot = new Bitmap(value);
                lock (_bitmapLock)
                {
                    oldSnapshot = bitmap;
                    bitmap = newSnapshot;
                }
                if (oldSnapshot != null)
                    oldSnapshot.Dispose();
            }
        }

        public void Save(string filename)
        {
            lock (_bitmapLock)
                bitmap.Save(filename);
        }

        public void Save(string filename, System.Drawing.Imaging.ImageFormat format)
        {
            lock (_bitmapLock)
                bitmap.Save(filename, format);
        }

        /// <summary> Create size of empty bitmap </summary>
        public SafeBitmap(int width, int height)
        {
            bitmap = new Bitmap(width, height);
        }

        /// <summary> Create from bitmap to deep copy </summary>
        public SafeBitmap(Bitmap bitmap)
        {
            this.bitmap = new Bitmap(bitmap);
        }

        public static explicit operator System.Drawing.Bitmap(SafeBitmap safeBitmap) => safeBitmap.Snapshot;
        public static explicit operator SafeBitmap(System.Drawing.Bitmap safeBitmap) => new SafeBitmap(safeBitmap);

        public void Dispose()
        {
            bitmap.Dispose();
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}
