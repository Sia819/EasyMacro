using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using EasyMacro.ViewModel.Node.Editors;
using ReactiveUI;

namespace EasyMacro.View.Node.Editors
{
    /// <summary>
    /// PointRecordEditorView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PointRecordEditorView : UserControl, IViewFor<PointRecordEditorViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel),
                                        typeof(PointRecordEditorViewModel),
                                        typeof(PointRecordEditorView),
                                        new PropertyMetadata(null));

        public PointRecordEditorViewModel ViewModel
        {
            get => (PointRecordEditorViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (PointRecordEditorViewModel)value;
        }
    
        public PointRecordEditorView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.ReactiveObject.GetMousePos_Command, v => v.recordButton.Command)
                    .DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.ReactiveObject.ButtonEnable, v => v.recordButton.Visibility)
                    .DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.ReactiveObject.MyPoint, v => v.X_UpDown.Value, (point) => point.X)
                    .DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.ReactiveObject.Editable, v => v.X_UpDown.IsEnabled)
                    .DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.ReactiveObject.MyPoint, v => v.Y_UpDown.Value, (point) => point.Y)
                    .DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.ReactiveObject.Editable, v => v.Y_UpDown.IsEnabled)
                    .DisposeWith(d);
            });
        }
    }
}
