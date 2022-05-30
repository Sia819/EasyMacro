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
using System.Reactive.Linq;
using ReactiveUI.Fody.Helpers;

namespace EasyMacro.ViewModel
{
    // 보통 UserControl의 ViewModel을 상속받지 않는 케이스
    public partial class ImageManagerViewModel
    {
        [Reactive] public string ImageFilePath { get; set; }
        public ObservableDictionary<string, ImageList> RegisterdImages { get; }
        public ReactiveCommand<Unit, Unit> ImageAddCommand { get; }
        public ReactiveCommand<string, Unit> ImageDeleteCommand { get; }

        public class ImageList
        {
            [Reactive] public string Name { get; set; }
            [Reactive] public string FilePath { get; set; }
            [Reactive] public Bitmap PreviewImage { get; set; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ImageManagerViewModel()
        {
            this.RegisterdImages = new();
            this.ImageAddCommand = ReactiveCommand.CreateFromObservable(ImageAdd_ExcuteCommand);
            this.ImageDeleteCommand = ReactiveUI.ReactiveCommand.Create<string>(path => ImageDelCommand(path));
        }
    }

    public partial class ImageManagerViewModel
    {
        private IObservable<Unit> ImageAdd_ExcuteCommand()
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
                    if (Path.GetExtension(ImageFilePath) == ".jpg" || Path.GetExtension(ImageFilePath) == ".png")
                    {
                        var temp = new ImageList()
                        {
                            Name = dictDupeRename(Path.GetFileNameWithoutExtension(dlg.FileName)),
                            FilePath = ImageFilePath,
                            PreviewImage = new Bitmap(ImageFilePath)
                        };
                        RegisterdImages.Add(temp.Name, temp);
                        return Observable.Return(Unit.Default);
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
                // Make ImageList Object
                var temp = new ImageList()
                {
                    Name = dictDupeRename(Path.GetFileNameWithoutExtension(dlg.FileName)),
                    FilePath = dlg.FileName,
                    PreviewImage = new Bitmap(dlg.FileName)
                };
                RegisterdImages.Add(temp.Name, temp);
            }
            return Observable.Return(Unit.Default);
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

        // 이름 중복시, "너구리 (2)" 이런식으로 바꿔주는 메서드
        private string dictDupeRename(string key)
        {
            if (this.RegisterdImages.ContainsKey(key))
            {
                for (uint i = 2; i < uint.MaxValue; i++)
                {
                    if(this.RegisterdImages.ContainsKey(String.Format("{0} ({1})", key, i)) == false)
                    {
                        return String.Format("{0} ({1})", key, i);
                    }
                }
            }
            return key;
        }
    }
}

