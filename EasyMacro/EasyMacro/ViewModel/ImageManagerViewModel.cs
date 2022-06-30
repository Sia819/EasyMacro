using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using Microsoft.Win32;
using EasyMacro.Common;
using ReactiveUI;
using System.ComponentModel;
using System.Collections.ObjectModel;
using EasyMacro.Model;
using System.Windows.Input;

namespace EasyMacro.ViewModel
{
    // 보통 UserControl의 ViewModel을 상속받지 않는 케이스
    public class ImageManagerViewModel : INotifyPropertyChanged
    {
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        private static ImageManagerViewModel _instance;
        public static ImageManagerViewModel Instance => _instance ??= new ImageManagerViewModel();

        public string ImageFilePath { get; set; }

        public ObservableCollection<ImageList> RegisterdImages { get; }
        public ReactiveCommand<Unit, Unit> ImageAddCommand { get; }
        public ReactiveCommand<string, Unit> ImageDeleteCommand { get; }

        /// <summary> Constructor </summary>
        private ImageManagerViewModel()
        {
            // Property Initialize
            this.RegisterdImages = new();

            this.ImageAddCommand = ReactiveCommand.Create(ImageAdd_ExcuteCommand);
            this.ImageDeleteCommand = ReactiveCommand.Create<string>((itemName) => DeleteImage_ExcuteCommand(itemName));

            if (Directory.Exists("images"))
            {
                var files =  Directory.GetFiles("images");
                foreach (var file in files)
                {
                    var temp = new ImageList()
                    {
                        Name = dictDupeRename(Path.GetFileNameWithoutExtension(file)),
                        FilePath = file,
                        PreviewImage = new SafeBitmap(PathToBitmap(file))
                    };
                    RegisterdImages.Add(temp);
                }
            }
        }

        private void ImageAdd_ExcuteCommand()
        {
            // Configure open file dialog box 
            OpenFileDialog dlg = new OpenFileDialog();

            if (!String.IsNullOrEmpty(ImageFilePath))
            {
                if (Directory.Exists(ImageFilePath)) // TextBox에 있는것이 존재하는 폴더
                {
                    dlg.InitialDirectory = ImageFilePath;
                    // goes ShowDialog logic
                }
                else if (File.Exists(ImageFilePath)) // TextBox에 있는것이 파일
                {
                    // 이미지 파일인지 체크
                    string imageCodec = Path.GetExtension(ImageFilePath);
                    string[] imageCodecs = { ".bmp", ".dib", ".rle",
                                            ".jpg", ".jpeg", ".jpe", ".jfif",
                                            ".gif",
                                            ".tif", ".tiff",
                                            ".png" };
                    bool isImage = false;
                    foreach (var c in imageCodecs)
                    {
                        if (imageCodec == c)
                        {
                            isImage = true;
                        }
                    }

                    if (!isImage)
                    {
                        MessageBox.Show("이미지 형식 파일이 아닙니다!!");
                        return;
                    }
                    else
                    {
                        var temp = new ImageList()
                        {
                            Name = dictDupeRename(Path.GetFileNameWithoutExtension(ImageFilePath)),
                            FilePath = ImageFilePath,
                            PreviewImage = new SafeBitmap(PathToBitmap(ImageFilePath))
                        };
                        RegisterdImages.Add(temp);
                        temp.PreviewImage.Save("images\\" + temp.Name, ImageFormat.Png);
                        return;
                    }
                }
                else // 올바르지 않은 문자열
                {
                    dlg.InitialDirectory = "";
                    this.ImageFilePath = "";
                }
            }

            // 다이얼로그 기본 경로는 바탕화면
            if (String.IsNullOrEmpty(dlg.InitialDirectory))
                dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // 다이얼로그 필터 만들기 (bmp, jpg, ...)
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            string codec = "";
            for (int i = 0; i < codecs.Length; i++)//foreach (var c in codecs)
            {
                string codecName = codecs[i].CodecName.Substring(8).Replace("Codec", "Files").Trim();
                codec += String.Format("{0} ({1})|{1}|", codecName, codecs[i].FilenameExtension);
            }
            codec += String.Format("{0} ({1})|{1}", "All Files", "*.*");
            dlg.Filter = codec;
            dlg.DefaultExt = ".png"; // Default file extension is png

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
                    PreviewImage = new SafeBitmap(PathToBitmap(dlg.FileName))
                };
                RegisterdImages.Add(temp);
                if (System.IO.Directory.Exists("images") == false) Directory.CreateDirectory("images");
                temp.PreviewImage.Save("images/" + temp.Name + ".png", ImageFormat.Png);
            }
            return;
        }

        public void DeleteImage_ExcuteCommand(string imageName)
        {
            RegisterdImages.Remove(RegisterdImages.Find(imageName));
        }

        private Bitmap PathToBitmap(string path)
        {
            Bitmap targetBmp;
            using (Bitmap oldBmp = new Bitmap(path))
            {
                using (Bitmap newBmp = new Bitmap(oldBmp))
                {
                    targetBmp = newBmp.Clone(new Rectangle(0, 0, newBmp.Width, newBmp.Height), PixelFormat.Format32bppArgb);
                }
            }
            return targetBmp;
        }

        public Bitmap CopyImg(string imageName)
        {
            Bitmap cloneBitmap = (Bitmap)RegisterdImages.Find(imageName).PreviewImage.Snapshot;
            return cloneBitmap;
        }

        // 이름 중복시, "너구리 (2)" 이런식으로 바꿔주는 메서드
        private string dictDupeRename(string key)
        {
            if (this.RegisterdImages.ContainsKey(key))
            {
                for (uint i = 2; i < uint.MaxValue; i++)
                {
                    if (this.RegisterdImages.ContainsKey(String.Format("{0} ({1})", key, i)) == false)
                    {
                        return String.Format("{0} ({1})", key, i);
                    }
                }
            }
            return key;
        }
    }

    public static class ObservableCollectionExtension
    {
        public static bool ContainsKey<T>(this ObservableCollection<T> obs, string obj)
        {
            var ob = obs;
            foreach (T i in ob)
            {
                if (i is Tuple<string, ImageList> result)
                {
                    if (result.Item1 == obj)
                        return true;
                }
            }

            return false;
        }

        public static ImageList Find<T>(this ObservableCollection<T> obs, string obj)
        {
            foreach (T i in obs)
            {
                if (i is ImageList result)
                {
                    if (result.Name == obj)
                        return result;
                }
            }
            return null;
        }
    }
}

