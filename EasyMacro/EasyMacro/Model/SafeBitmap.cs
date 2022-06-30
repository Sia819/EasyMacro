using System;
using System.Collections.Generic;
using System.Drawing;
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

        public static implicit operator System.Drawing.Bitmap(SafeBitmap safeBitmap) => safeBitmap.Snapshot;
        public static implicit operator SafeBitmap(System.Drawing.Bitmap safeBitmap) => new SafeBitmap(safeBitmap);

        public void Dispose()
        {
            bitmap.Dispose();
        }
    }
}
