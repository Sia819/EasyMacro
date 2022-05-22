using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static EasyMacro.ViewModel.ImageManagerViewModel;

namespace EasyMacro.View.UC
{
    /// <summary>
    /// ImageManager.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ImageManager : UserControl
    {
        public ImageManager()
        {
            InitializeComponent();

            //ImageSource a = new ImageSource();

            PreviewImages = new ObservableCollection<ImageList>();
            PreviewImages.Add(new ImageList { FilePath = "preview/myFilePath/1.png", Name = "ImageName1" });
            PreviewImages.Add(new ImageList { FilePath = "preview/myFilePath/2.png", Name = "ImageName2" });
            PreviewImages.Add(new ImageList { FilePath = "preview/myFilePath/3.png", Name = "ImageName3" });
        }

        public ObservableCollection<ImageList> PreviewImages { get; set; }

        
        
    }
}