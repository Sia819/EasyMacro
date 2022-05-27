using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media;
using ReactiveUI;
using System.Reactive;
using System.Collections.ObjectModel;
using EasyMacro.Common;
using System.Windows.Media.Imaging;

namespace EasyMacro.ViewModel
{
    public partial class ImageManagerViewModel
    {
        public string ImageFilePath { get; set; }
        public ObservableDictionary<string, ImageList> RegisterdImages { get; }
        public ReactiveCommand<Unit, Unit> ImageAddCommand { get; }
        public ReactiveCommand<string, Unit> ImageDeleteCommand { get; }

        public class ImageList
        {
            public string Name { get; set; }
            public string FilePath { get; set; }
            public Bitmap PreviewImage { get; set; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ImageManagerViewModel()
        {
            this.RegisterdImages = new();
            this.ImageAddCommand = ReactiveUI.ReactiveCommand.Create(ImageAdd);
            this.ImageDeleteCommand = ReactiveUI.ReactiveCommand.Create<string>(path => ImageDelCommand(path));
        }
    }

    public partial class ImageManagerViewModel
    {
        private void ImageAdd()
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
                        var temp = new ImageList()
                        {
                            Name = (new FileInfo(ImageFilePath)).Name,
                            FilePath = ImageFilePath,
                            PreviewImage = new Bitmap(ImageFilePath)
                        };
                        RegisterdImages.Add(ImageFilePath, temp);
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
                // Do something with fileName
                var temp = new ImageList()
                {
                    Name = (new FileInfo(ImageFilePath)).Name,
                    FilePath = dlg.FileName,
                    PreviewImage = new Bitmap(dlg.FileName)
                };
                RegisterdImages.Add(dlg.FileName, temp);
            }
        }

        private void ImageDelCommand(string path)
        {
            RegisterdImages.Remove(path);
        }

        public Bitmap CopyImg(string path)
        {
            Bitmap cloneBitmap = (Bitmap)RegisterdImages[path].PreviewImage.Clone();
            return cloneBitmap;
        }
    }
}
