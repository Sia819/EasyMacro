using DynamicData;
using EasyMacro.Model.Node;
using EasyMacro.Model.Node.Compiler;
using EasyMacro.View.Node;
using EasyMacro.ViewModel.Node.Editors;
using EasyMacroAPI.Command;
using ExtendedXmlSerializer;
using ExtendedXmlSerializer.ExtensionModel.Xml;
using NodeNetwork.Toolkit.ValueNode;
using NodeNetwork.ViewModels;
using NodeNetwork.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reactive.Linq;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

namespace EasyMacro.ViewModel.Node.NodeObject
{
    public class RelativeMouseMoveNodeViewModel : CodeGenNodeViewModel, INodeSerializable, IExtendedXmlCustomSerializer
    {
        static RelativeMouseMoveNodeViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new CodeGenNodeView(), typeof(IViewFor<RelativeMouseMoveNodeViewModel>));
        }

        private static RelativeMouseMove _relativeMouseMove;
        private RelativeMouseMove relativeMouseMove
        {
            get
            {
                if (_relativeMouseMove is null)
                    _relativeMouseMove = new RelativeMouseMove("", 0, 0);
                return _relativeMouseMove;
            }
        }

        /// <summary>
        /// Delay Time
        /// </summary>
        /// 
        public ValueNodeInputViewModel<IntPtr> hWnd { get; }

        public ValueNodeInputViewModel<Point> MyPoint { get; }

        public ValueNodeOutputViewModel<IStatement> FlowIn { get; }

        public ValueListNodeInputViewModel<IStatement> FlowOut { get; }

        public ValueNodeInputViewModel<int?> RunButton { get; }

        public bool IsCanExcute { get; set; } = true;

        Action Func()
        {
            Action action = () =>
            {
                if (CodeSimViewModel.Instance.IsRunning || Thread.CurrentThread.IsBackground is false)
                {
                    CodeSimViewModel.Instance.Print((FlowIn.CurrentValue as NodeCompile).CurrentValue);

                    relativeMouseMove.ChangeWindow(hWnd.Value);
                    relativeMouseMove.X = (int)MyPoint.Value.X;
                    relativeMouseMove.Y = (int)MyPoint.Value.Y;

                    relativeMouseMove.Do();

                    foreach (var a in FlowOut.Values.Items)
                    {
                        a.Compile(new CompilerContext());
                    }
                }
            };
            return action;
        }

        public override void Serializer(XmlWriter xmlWriter, object obj)
        {
            NodeSerializer.SerializerOfNodeViewModel(ref xmlWriter, ref obj);

            RelativeMouseMoveNodeViewModel instance = obj as RelativeMouseMoveNodeViewModel;
            FindWindowEditorViewModel findWindowEditor = (instance.hWnd.Editor as FindWindowEditorViewModel);
            xmlWriter.WriteElementString(nameof(findWindowEditor.TargetWindowTitle), findWindowEditor.TargetWindowTitle);
            xmlWriter.WriteElementString(nameof(findWindowEditor.TargetWindowClass), findWindowEditor.TargetWindowClass);
            xmlWriter.WriteElementString("PointX", instance.MyPoint.Value.X.ToString());
            xmlWriter.WriteElementString("PointY", instance.MyPoint.Value.Y.ToString());
        }

        public override object Deserialize(XElement xElement)
        {
            RelativeMouseMoveNodeViewModel instance = (RelativeMouseMoveNodeViewModel)NodeSerializer.DeserializeOfNoveViewModel(ref xElement, new RelativeMouseMoveNodeViewModel());
            Dictionary<string, XElement> dictionary = NodeSerializer.XElementToDictionary(xElement);

            //FindWindowEditorViewModel findWindowEditor = (instance.hWnd.Editor as FindWindowEditorViewModel);
            (instance.hWnd.Editor as FindWindowEditorViewModel).TargetWindowTitle = dictionary["TargetWindowTitle"].Value;
            (instance.hWnd.Editor as FindWindowEditorViewModel).TargetWindowClass = dictionary["TargetWindowClass"].Value;
            (instance.MyPoint.Editor as PointRecordEditorViewModel).Value = new Point(int.TryParse(dictionary["PointX"].Value, out int x) ? x : 0,
                                                                                      int.TryParse(dictionary["PointY"].Value, out int y) ? y : 0);
            return instance;
        }

        public RelativeMouseMoveNodeViewModel() : base(NodeType.Function)
        {
            base.Name = "마우스 상대좌표 이동";

            hWnd = new ValueNodeInputViewModel<IntPtr>()
            {
                Port = null,
                Name = "프로세스 이름",
                Editor = new FindWindowEditorViewModel()
            };
            this.Inputs.Add(hWnd);

            MyPoint = new ValueNodeInputViewModel<Point>()
            {
                Name = "Point",
                Editor = new PointRecordEditorViewModel(hWnd.Value),
                Port = null
            };
            this.Inputs.Add(MyPoint);

            this.RunButton = new ValueNodeInputViewModel<int?>()
            {
                Port = null,

                Editor = new RunButtonViewModel()
                {
                    RunScript = ReactiveCommand.Create
                    (
                        Func(),
                        this.WhenAnyValue(vm => vm.IsCanExcute)
                    )
                }
            };
            this.Inputs.Add(this.RunButton);

            FlowIn = new CodeGenOutputViewModel<IStatement>(PortType.Execution)
            {
                Name = "",
                Value = this.RunButton.ValueChanged.Select(_ => new NodeCompile(this.Func())
                {
                    Log = Observable.Merge(hWnd.ValueChanged.Select(windowName => $"InputMouse - ({windowName}, {this.MyPoint.Value.X}, {this.MyPoint.Value.Y})"),
                                            MyPoint.ValueChanged.Select(point => $"InputMouse - ({hWnd.Value}, {point.X}, {point.Y}"))
                })
            };
            this.Outputs.Add(FlowIn);


            FlowOut = new CodeGenListInputViewModel<IStatement>(PortType.Execution)
            {
                Name = "",
                MaxConnections = 1
            };
            this.Inputs.Add(FlowOut);
        }
    }
}
