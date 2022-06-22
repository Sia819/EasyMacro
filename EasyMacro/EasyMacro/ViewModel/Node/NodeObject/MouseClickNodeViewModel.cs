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
using System.Drawing;
using System.Collections.Generic;

namespace EasyMacro.ViewModel.Node.NodeObject
{
    public class MouseClickNodeViewModel : CodeGenNodeViewModel, INodeSerializable, IExtendedXmlCustomSerializer
    {
        static MouseClickNodeViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new CodeGenNodeView(), typeof(IViewFor<MouseClickNodeViewModel>));
        }

        private static MouseClick _mouseClick;
        private MouseClick mouseClick
        {
            get
            {
                if (_mouseClick is null)
                    _mouseClick = new MouseClick(0, 0);
                return _mouseClick;
            }
        }

        public ValueNodeInputViewModel<Point> MyPoint { get; }

        public ValueNodeInputViewModel<int?> Delay { get; }

        public ValueNodeOutputViewModel<IStatement> FlowIn { get; }

        public ValueListNodeInputViewModel<IStatement> FlowOut { get; }

        public override NodeOutputViewModel GetOutputViewModel => this.FlowIn;
        public ValueNodeInputViewModel<int?> RunButton { get; }

        public bool IsCanExcute { get; set; } = true;

        Action Func()
        {
            Action action = () =>
            {
                if (CodeSimViewModel.Instance.IsRunning || Thread.CurrentThread.IsBackground is false)
                {
                    CodeSimViewModel.Instance.Print((FlowIn.CurrentValue as NodeCompile).CurrentValue);

                    mouseClick.X = (int)MyPoint.Value.X;
                    mouseClick.Y = (int)MyPoint.Value.Y;
                    mouseClick.Delay = (int)Delay.Value;
                    mouseClick.Do();

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

            MouseClickNodeViewModel instance = obj as MouseClickNodeViewModel;

            xmlWriter.WriteElementString("PointX", instance.MyPoint.Value.X.ToString());
            xmlWriter.WriteElementString("PointY", instance.MyPoint.Value.Y.ToString());
            xmlWriter.WriteElementString(nameof(Delay), instance.Delay.Value.ToString());
        }

        public override object Deserialize(XElement xElement)
        {
            MouseClickNodeViewModel instance = (MouseClickNodeViewModel)NodeSerializer.DeserializeOfNoveViewModel(ref xElement, new MouseClickNodeViewModel());
            Dictionary<string, XElement> dictionary = NodeSerializer.XElementToDictionary(xElement);

            (instance.MyPoint.Editor as PointRecordEditorViewModel).Value = new Point(int.TryParse(dictionary["PointX"].Value, out int x) ? x : 0,
                                                                                      int.TryParse(dictionary["PointY"].Value, out int y) ? y : 0);
            (instance.Delay.Editor as IntegerValueEditorViewModel).Value = int.TryParse(dictionary["Delay"].Value, out int Delay) ? Delay : 40;
            return instance;
        }

        public override void Connect(INodeSerializable instance, List<INodeSerializable> obj)
        {
            throw new NotImplementedException();
        }

        public MouseClickNodeViewModel() : base(NodeType.Function)
        {
            base.Name = "마우스 클릭";

            MyPoint = new CodeGenInputViewModel<Point>(PortType.Point)
            {
                Name = "Point",
                Editor = new PointRecordEditorViewModel()
            };
            this.Inputs.Add(MyPoint);

            Delay = new ValueNodeInputViewModel<int?>()
            {
                Name = "지연시간",
                Editor = new IntegerValueEditorViewModel(0) { Value = 40 },
                Port = null,
            };
            this.Inputs.Add(Delay);

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
                    Log = Observable.Merge(MyPoint.ValueChanged.Select(point => $"MouseMove - ({point.X}, {point.Y}, {this.Delay.Value})"),
                                           Delay.ValueChanged.Select(delay => $"MouseMove - ({this.MyPoint.Value.X}, {this.MyPoint.Value.Y}, {delay ?? 0})"))
                })
            };
            this.Outputs.Add(FlowIn);

            FlowOut = new CodeGenListInputViewModel<IStatement>(PortType.Execution)
            {
                Name = "Send",
                MaxConnections = 1
            };
            this.Inputs.Add(FlowOut);

            this.Hash = Common.HashGen.RandomHashGen(10);
        }

        public MouseClickNodeViewModel(string hash) : base(NodeType.Function)
        {
            this.Hash = hash;
        }
    }
}
