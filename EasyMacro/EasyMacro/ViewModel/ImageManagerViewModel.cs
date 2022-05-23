using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace EasyMacro.ViewModel
{
    public partial class ImageManagerViewModel
    {
        public string ImageFilePath { get; set; }


        public class ImageList
        {
            public string Name { get; set; }
            public string FilePath { get; set; }
            public ImageSource PreviewImage { get; set; }
        }
    }

    public partial class ImageManagerViewModel
    {
        Dictionary<string, Bitmap> imgDict = new Dictionary<string, Bitmap>();

        private void ImageAddCommand()
        {
            // Configure open file dialog box 
            OpenFileDialog dlg = new OpenFileDialog();
            if (!String.IsNullOrEmpty(ImageFilePath))
            {
                if (Directory.Exists(ImageFilePath)) // TextBox에 있는것이 존재하는 폴더
                {
                    dlg.InitialDirectory = ImageFilePath;
                }
                else if (File.Exists(ImageFilePath)) // TextBox에 있는것이 파일
                {
                    // 이미지 파일인지 체크
                    if(Path.GetExtension(ImageFilePath) == ".jpg" || Path.GetExtension(ImageFilePath) == ".png")
                    {
                        imgDict.Add(ImageFilePath, new Bitmap(ImageFilePath));
                        return;
                    }
                }
            }

            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            dlg.Filter = "";

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            string sep = string.Empty;

            foreach (var c in codecs)
            {
                string codecName = c.CodecName.Substring(8).Replace("Codec", "Files").Trim();
                dlg.Filter = String.Format("{0}{1}{2} ({3})|{3}", dlg.Filter, sep, codecName, c.FilenameExtension);
                sep = "|";
            }

            dlg.Filter = String.Format("{0}{1}{2} ({3})|{3}", dlg.Filter, sep, "All Files", "*.*");

            dlg.DefaultExt = ".png"; // Default file extension 

            // Show open file dialog box 
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                // Open document 
                ImageFilePath = dlg.FileName;
                // Do something with fileName
                imgDict.Add(ImageFilePath, new Bitmap(ImageFilePath));
            }
        }

        public void ImageDelCommand(string path)
        {
            imgDict.Remove(path);
        }

        public Bitmap CopyImg(string path)
        {
            Bitmap cloneBitmap = (Bitmap)imgDict[path].Clone();
            return cloneBitmap;
        }
    }
}
