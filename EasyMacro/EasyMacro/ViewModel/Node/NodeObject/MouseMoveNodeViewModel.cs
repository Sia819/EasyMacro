using DynamicData;
using EasyMacro.Model.Node;
using EasyMacro.Model.Node.Compiler;
using EasyMacro.View.Node;
using EasyMacro.ViewModel.Node.Editors;
using EasyMacroAPI.Command;
using ExtendedXmlSerializer;
using ExtendedXmlSerializer.ExtensionModel.Xml;
using NodeNetwork.Toolkit.ValueNode;
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
    public class MouseMoveNodeViewModel : CodeGenNodeViewModel, INodeSerializable, IExtendedXmlCustomSerializer
    {
        static MouseMoveNodeViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new CodeGenNodeView(), typeof(IViewFor<MouseMoveNodeViewModel>));
        }

        private static MouseMove _mouseMove;
        private MouseMove mouseMove
        {
            get
            {
                if (_mouseMove is null)
                    _mouseMove = new MouseMove(0, 0);
                return _mouseMove;
            }
        }

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

                    mouseMove.X = (int)MyPoint.Value.X;
                    mouseMove.Y = (int)MyPoint.Value.Y;
                    mouseMove.Do();

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

            MouseMoveNodeViewModel instance = obj as MouseMoveNodeViewModel;

            xmlWriter.WriteElementString(nameof(MyPoint.Value.X), instance.MyPoint.Value.X.ToString());
            xmlWriter.WriteElementString(nameof(MyPoint.Value.Y), instance.MyPoint.Value.Y.ToString());
        }

        public override object Deserialize(XElement xElement)
        {
            MouseMoveNodeViewModel instance = (MouseMoveNodeViewModel)NodeSerializer.DeserializeOfNoveViewModel(ref xElement, new MouseMoveNodeViewModel());
            Dictionary<string, XElement> dictionary = NodeSerializer.XElementToDictionary(xElement);
            (instance.MyPoint.Editor as PointRecordEditorViewModel).Value = new Point(
                int.TryParse(dictionary["MyPoint.Value.X"].Value, out int x) ? x : 0,
                int.TryParse(dictionary["MyPoint.Value.Y"].Value, out int y) ? y : 0);
            return instance;
        }

        public override void Connect()
        {
            throw new NotImplementedException();
        }

        public MouseMoveNodeViewModel() : base(NodeType.Function)
        {
            base.Name = "마우스 절대좌표 이동";

            MyPoint = new CodeGenInputViewModel<Point>(PortType.Point)
            {
                Name = "Point",
                Editor = new PointRecordEditorViewModel()
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
                    Log = Observable.Merge(MyPoint.ValueChanged.Select(point => $"MouseMove - ({point.X}, {point.Y})"))
                })
            };
            this.Outputs.Add(FlowIn);

            // Send Flow, 현재 노드에서 내보낼 (매크로의 실행이 끝났다는) 신호
            FlowOut = new CodeGenListInputViewModel<IStatement>(PortType.Execution)
            {
                Name = "Send",
                MaxConnections = 1
            };
            this.Inputs.Add(FlowOut);
        }
    }
}
