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
        private readonly object BitmapLock = new object();

        private Bitmap bitmap;

        public int Width { get; set; }
        public int Height { get; set; }

        public Bitmap DangerousGetBitmap() => bitmap;

        public Bitmap Snapshot
        {
            get
            {
                lock (BitmapLock)
                    return new Bitmap(bitmap);
            }
            set
            {
                Bitmap oldSnapshot;
                Bitmap newSnapshot = new Bitmap(value);
                lock (BitmapLock)
                {
                    oldSnapshot = bitmap;
                    bitmap = newSnapshot;
                }
                if (oldSnapshot != null)
                    oldSnapshot.Dispose();
            }
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

        public void Dispose()
        {
            bitmap.Dispose();
        }
    }
}
