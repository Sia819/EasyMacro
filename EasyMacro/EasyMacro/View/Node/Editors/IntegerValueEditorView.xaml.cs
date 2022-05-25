using System;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using EasyMacro.ViewModel.Node.Editors;
using ReactiveUI;

namespace EasyMacro.View.Node.Editors
{
    /// <summary>
    /// IntegerValueEditorView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class IntegerValueEditorView : UserControl, IViewFor<IntegerValueEditorViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = 
            DependencyProperty.Register(nameof(ViewModel), 
                                        typeof(IntegerValueEditorViewModel), 
                                        typeof(IntegerValueEditorView), 
                                        new PropertyMetadata(null));

        public IntegerValueEditorViewModel ViewModel
        {
            get => (IntegerValueEditorViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (IntegerValueEditorViewModel)value;
        }
        #endregion

        public IntegerValueEditorView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                // int? 를 double? 로 TwoWayBind하기위해 Converter를 사용
                this.Bind(ViewModel, 
                          vm => vm.Value, 
                          v => v.valueUpDown.Value,
                          this.ViewModelToViewConverterFunc,
                          this.ViewToViewModelConverterFunc).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.MinValue, v => v.valueUpDown.Minimum).DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.MaxValue, v => v.valueUpDown.Maximum).DisposeWith(d);
            });

        }

        private double? ViewModelToViewConverterFunc(int? viewModelValue)
        {
            return Convert.ToDouble(viewModelValue);
        }

        private int? ViewToViewModelConverterFunc(double? viewValue)
        {
            return Convert.ToInt32(viewValue);
        }
    }
}
