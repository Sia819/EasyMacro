 using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EasyMacro.ViewModel.Node;
using EasyMacro.ViewModel.Node.Editors;
using ReactiveUI;

namespace EasyMacro.View.Node
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
                this.OneWayBind(ViewModel, vm => (double)vm.Value, v => v.valueUpDown.Value);
                this.OneWayBind(ViewModel, vm => vm.MinValue, v => v.valueUpDown.Minimum);
                this.OneWayBind(ViewModel, vm => vm.MaxValue, v => v.valueUpDown.Maximum);
            });

        }
    }
}
