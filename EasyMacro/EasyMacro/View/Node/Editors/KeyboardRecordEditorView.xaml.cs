using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using EasyMacro.ViewModel.Node.Editors;
using ReactiveUI;

namespace EasyMacro.View.Node.Editors
{
    /// <summary>
    /// KeyboardRecordEditorView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class KeyboardRecordEditorView : UserControl, IViewFor<KeyboardRecordEditorViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel),
                                        typeof(KeyboardRecordEditorViewModel),
                                        typeof(KeyboardRecordEditorView),
                                        new PropertyMetadata(null));

        public KeyboardRecordEditorViewModel ViewModel
        {
            get => (KeyboardRecordEditorViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (KeyboardRecordEditorViewModel)value;
        }
    
        public KeyboardRecordEditorView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.GetKeyFormHook_Command, v => v.recordButton.Command)
                    .DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.ReactiveObject.MyKey, v => v.TextBox_TextBlock.Text)
                    .DisposeWith(d);
            });
        }
        
    }
}
