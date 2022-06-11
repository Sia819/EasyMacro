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

        public ObservableDictionary<string, ImageList> RegisterdImages { get; }
        public ReactiveCommand<Unit, Unit> ImageAddCommand { get; }
        public ReactiveCommand<string, Unit> ImageDeleteCommand { get; }

        public class ImageList : INotifyPropertyChanged
        {
            #region PropertyChanged
            public event PropertyChangedEventHandler PropertyChanged;
            #endregion

            public string Name { get; set; }
            public string FilePath { get; set; }
            public Bitmap PreviewImage { get; set; }
            public Bitmap ImageClone()
            {
                // 이미지를 클론하는 중, 멀티스레드 다중참조를 우려
                return Application.Current.MainWindow.Dispatcher.Invoke(
                    () => PreviewImage.Clone(new Rectangle(0, 0, PreviewImage.Width, PreviewImage.Height), PreviewImage.PixelFormat));
            }
            ~ImageList()
            {
                Name = null;
                FilePath = null;
                PreviewImage.Dispose();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private ImageManagerViewModel()
        {
            // Property Initialize
            this.RegisterdImages = new();

            this.ImageAddCommand = ReactiveCommand.CreateFromObservable(ImageAdd_ExcuteCommand);

            // TODO : ImageManager Delete 구현
            //this.ImageDeleteCommand = ReactiveCommand.CreateFromObservable<Unit, string>(ImageDelCommand(path));  //ReactiveCommand.Create<string>(path => ImageDelCommand(path));
        }

        private IObservable<Unit> ImageAdd_ExcuteCommand()
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
                        return Observable.Return(Unit.Default);
                    }
                    else
                    {
                        var temp = new ImageList()
                        {
                            Name = dictDupeRename(Path.GetFileNameWithoutExtension(ImageFilePath)),
                            FilePath = ImageFilePath,
                            PreviewImage = PathToBitmap(ImageFilePath)
                        };
                        RegisterdImages.Add(temp.Name, temp);
                        return Observable.Return(Unit.Default);
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
                    PreviewImage = PathToBitmap(dlg.FileName)
                };
                RegisterdImages.Add(temp.Name, temp);
            }
            return Observable.Return(Unit.Default);
        }

        private Bitmap PathToBitmap(string path)
        {
            Bitmap targetBmp;
            using (Bitmap oldBmp = new Bitmap(path))
            {
                using (Bitmap newBmp = new Bitmap(oldBmp))
                {
                    targetBmp = newBmp.Clone(new Rectangle(0, 0, newBmp.Width, newBmp.Height), PixelFormat.Format32bppRgb);
                }
            }
            return targetBmp;
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
                    if (this.RegisterdImages.ContainsKey(String.Format("{0} ({1})", key, i)) == false)
                    {
                        return String.Format("{0} ({1})", key, i);
                    }
                }
            }
            return key;
        }
    }
}

