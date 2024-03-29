﻿using System.Windows;
using System.Windows.Controls;
using System.Reactive.Disposables;
using EasyMacro.ViewModel.Node;
using NodeNetwork.Views;
using ReactiveUI;
using EasyMacro.Model.Node;
using System;
using System.Windows.Media;

namespace EasyMacro.View.Node
{
    /// <summary>
    /// CodeGenPortView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CodeGenPortView : UserControl, IViewFor<CodeGenPortViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(CodeGenPortViewModel), typeof(CodeGenPortView), new PropertyMetadata(null));

        public CodeGenPortViewModel ViewModel
        {
            get => (CodeGenPortViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (CodeGenPortViewModel)value;
        }
        #endregion

        #region Template Resource Keys
        public const String ExecutionPortTemplateKey = "ExecutionPortTemplate";
        public const String IntegerPortTemplateKey = "IntegerPortTemplate";
        public const String StringPortTemplateKey = "StringPortTemplate";
        public const String PointPortTemplateKey = "PointPortTemplate";
        #endregion

        public CodeGenPortView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.WhenAnyValue(v => v.ViewModel).BindTo(this, v => v.PortView.ViewModel).DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.PortType, v => v.PortView.Template, GetTemplateFromPortType)
                    .DisposeWith(d);

                this.OneWayBind(ViewModel, vm => vm.IsMirrored, v => v.PortView.RenderTransform,
                    isMirrored => new ScaleTransform(isMirrored ? -1.0 : 1.0, 1.0))
                    .DisposeWith(d);
            });
        }

        public ControlTemplate GetTemplateFromPortType(PortType type)
        {
            switch (type)
            {
                case PortType.Execution: return (ControlTemplate)Resources[ExecutionPortTemplateKey];
                case PortType.Integer: return (ControlTemplate)Resources[IntegerPortTemplateKey];
                case PortType.String: return (ControlTemplate)Resources[StringPortTemplateKey];
                case PortType.Point: return (ControlTemplate)Resources[PointPortTemplateKey];
                default: throw new Exception("Unsupported port type");
            }
        }
    }
}
