using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EasyMacro.Model
{
    public class ImageList : INotifyPropertyChanged
    {
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        public string Name { get; set; }
        public string FilePath { get; set; }
        public SafeBitmap PreviewImage { get; set; }

        /// <summary> Image to deep copy </summary>
        public Bitmap CloneImage() => PreviewImage.Snapshot;

        ~ImageList()
        {
            Name = null;
            FilePath = null;
            PreviewImage.Dispose();
        }
    }
}
