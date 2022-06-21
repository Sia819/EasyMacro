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
using System.Drawing;
using System.Reactive.Linq;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using static EasyMacro.ViewModel.Node.Editors.RadioButtonEditorViewModel;

namespace EasyMacro.ViewModel.Node.NodeObject
{
    public class RelativeMouseMoveNodeViewModel : CodeGenNodeViewModel, IExtendedXmlCustomSerializer
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

        public void Serializer(XmlWriter xmlWriter, object obj)
        {
            NodeSerializer.Serializer(ref xmlWriter, ref obj);

            RelativeMouseMoveNodeViewModel instance = obj as RelativeMouseMoveNodeViewModel;

            xmlWriter.WriteElementString(nameof(hWnd), instance.hWnd.Value.ToString());
        }

        public object Deserialize(XElement xElement)
        {
            RelativeMouseMoveNodeViewModel instance = (RelativeMouseMoveNodeViewModel)NodeSerializer.Deserialize(ref xElement, new RelativeMouseMoveNodeViewModel());
            (instance.hWnd.Editor as RadioButtonEditorViewModel).MyList[(int)xElement.Member(nameof(hWnd))].IsChecked = true;
            return instance;
        }

        public RelativeMouseMoveNodeViewModel() : base(NodeType.Function)
        {
            base.Name = "마우스 상대좌표 이동";

            hWnd = new ValueNodeInputViewModel<IntPtr>()
            {
                Port = null,
                Name = "프로세스 이름",
                Editor = new FindWindowEditorViewModel()//new StringValueEditorViewModel()
            };
            hWnd.ValueChanged.Do((i) =>
            {
                if (i != IntPtr.Zero)
                {
                    PInvoke.User32.GetWindowRect(i, out PInvoke.RECT rect);
                    (MyPoint.Editor as PointRecordEditorViewModel).Value = new Point(rect.left, rect.top);
                }
            }).Subscribe();

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
