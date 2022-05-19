using EasyMacro.ViewModel.Node.Editors;
using ReactiveUI;
using System;
using System.Collections.Generic;
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

namespace EasyMacro.View.Node.Editors
{
    /// <summary>
    /// RadioButtonEditorView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RadioButtonEditorView : UserControl, IViewFor<RadioButtonEditorViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(RadioButtonEditorViewModel), typeof(RadioButtonEditorView), new PropertyMetadata(null));

        public RadioButtonEditorViewModel ViewModel
        {
            get => (RadioButtonEditorViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (RadioButtonEditorViewModel)value;
        }
        #endregion ViewModel

        public RadioButtonEditorView()
        {
            InitializeComponent();

           

            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.MyList, v => v.radioList.ItemsSource);//.DisposeWith(d);
            });
        }

        
    }
}
