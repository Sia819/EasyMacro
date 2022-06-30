using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using EasyMacro.ViewModel;
using ReactiveUI;

namespace EasyMacro.View.UC
{
    /// <summary>
    /// ImageManager.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ImageManagerView : UserControl, INotifyPropertyChanged, IViewFor<ImageManagerViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(ImageManagerViewModel), typeof(ImageManagerView), new PropertyMetadata(null));

        public ImageManagerViewModel ViewModel 
        {
            get => (ImageManagerViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ImageManagerViewModel)value;
        }

        public ImageManagerView()
        {
            InitializeComponent();
            this.ViewModel = ImageManagerViewModel.Instance;

            // 디자이너 미리보기
            // Bitmap unknownImage = UnknownQuestionImage();
            // PreviewImages = new ObservableCollection<ImageManagerViewModel.ImageList>();
            // PreviewImages.Add(new ImageManagerViewModel.ImageList { FilePath = "preview/myFilePath/1.png", Name = "ImageName1", PreviewImage = unknownImage });
            // PreviewImages.Add(new ImageManagerViewModel.ImageList { FilePath = "preview/myFilePath/2.png", Name = "ImageName2", PreviewImage = unknownImage });
            // PreviewImages.Add(new ImageManagerViewModel.ImageList { FilePath = "preview/myFilePath/3.png", Name = "ImageName3", PreviewImage = unknownImage });

            this.WhenActivated(d =>
            {
                this.Bind(this.ViewModel, vm => vm.ImageFilePath, v => v.txtEditor.Text)
                    .DisposeWith(d);
                this.OneWayBind(this.ViewModel, vm => vm.RegisterdImages, v => v.imageList.ItemsSource)
                    .DisposeWith(d);
                this.BindCommand(this.ViewModel, vm => vm.ImageAddCommand, view => view.addButton)
                    .DisposeWith(d);
            });
        }

        public ObservableCollection<Model.ImageList> PreviewImages { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        // 물음표 이미지 만들기
        public Bitmap UnknownQuestionImage()
        {
            System.Drawing.Image unknownImage = new Bitmap(100, 100);
            Graphics a = Graphics.FromImage(unknownImage);
            a.FillRectangle(new SolidBrush(System.Drawing.Color.Pink), new RectangleF(0, 0, 100, 100));
            a.DrawString("?", new Font("Microsoft JhengHei UI", 60), new SolidBrush(System.Drawing.Color.Red), new PointF(17, 0));
            return (Bitmap)unknownImage;
        }

        public static ImageSource BitmapToImageSource(Bitmap bitmap)
        {
            BitmapImage bitmapImage;
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
            }
            return bitmapImage;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // 삭제버튼 
            Button btn = sender as Button;
            Model.ImageList item = btn.DataContext as Model.ImageList;
            ViewModel.RegisterdImages.Remove(item);
            if (Directory.Exists("images"))
            {
                File.Delete("images/" + item.Name + ".png");
            }
        }
    }
}