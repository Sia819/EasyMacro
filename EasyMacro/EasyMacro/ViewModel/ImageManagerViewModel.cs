using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EasyMacro.ViewModel
{
    public partial class ImageManagerViewModel
    {
        public string ImageFilePath { get; set; }

    }

    public partial class ImageManagerViewModel
    {



        private void ImageAddCommand()
        {
            // Configure open file dialog box 
            OpenFileDialog dlg = new OpenFileDialog();
            if (!String.IsNullOrEmpty(ImageFilePath))
            {
                if (Directory.Exists(ImageFilePath)) // TextBox에 있는것이 존재하는 폴더
                {

                }
                else if (File.Exists(ImageFilePath)) // TextBox에 있는것이 파일
                {
                    // 이미지 파일인지 체크
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
                string fileName = dlg.FileName;
                // Do something with fileName  
            }
        }





    }
}
