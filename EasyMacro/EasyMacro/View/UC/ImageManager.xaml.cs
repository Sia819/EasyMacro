using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static EasyMacro.ViewModel.ImageManagerViewModel;

namespace EasyMacro.View.UC
{
    /// <summary>
    /// ImageManager.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ImageManager : UserControl, INotifyPropertyChanged
    {
        public ImageManager()
        {
            InitializeComponent();

            //ImageSource a = new ImageSource();

            ImageSource unknownImage = UnknownQuestionImage();

            PreviewImages = new ObservableCollection<ImageList>();
            PreviewImages.Add(new ImageList { FilePath = "preview/myFilePath/1.png", Name = "ImageName1", PreviewImage = unknownImage });
            PreviewImages.Add(new ImageList { FilePath = "preview/myFilePath/2.png", Name = "ImageName2", PreviewImage = unknownImage });
            PreviewImages.Add(new ImageList { FilePath = "preview/myFilePath/3.png", Name = "ImageName3", PreviewImage = unknownImage });
        }

        public ObservableCollection<ImageList> PreviewImages { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public BitmapImage UnknownQuestionImage()
        {
            System.Drawing.Image unknownImage = new Bitmap(100, 100);
            Graphics a = Graphics.FromImage(unknownImage);
            a.FillRectangle(new SolidBrush(System.Drawing.Color.Pink), new RectangleF(0, 0, 100, 100));
            a.DrawString("?", new Font("Microsoft JhengHei UI", 60), new SolidBrush(System.Drawing.Color.Red), new PointF(17, 0));

            BitmapImage bitmapImage;
            using (MemoryStream memory = new MemoryStream())
            {
                unknownImage.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
            }
            return bitmapImage;
        }
    }
}