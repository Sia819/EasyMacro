using EasyMacro.Model.Node;
using EasyMacro.Model.Node.Compiler;
using DynamicData;
using NodeNetwork.Toolkit.ValueNode;
using EasyMacro.View.Node;
using ReactiveUI;
using NodeNetwork.ViewModels;
using ExtendedXmlSerializer.ExtensionModel.Xml;
using System.Xml.Linq;
using System.Xml;

namespace EasyMacro.ViewModel.Node.NodeObject
{
    public class StartNodeViewModel : CodeGenNodeViewModel, IExtendedXmlCustomSerializer
    {
        static StartNodeViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new CodeGenNodeView(), typeof(IViewFor<StartNodeViewModel>));
        }

        public ValueListNodeInputViewModel<IStatement> OnClickFlow { get; }

        public StartNodeViewModel() : base(NodeType.EventNode)
        {
            this.Name = "시작";

            OnClickFlow = new CodeGenListInputViewModel<IStatement>(PortType.Execution)
            {
                Name = "Start",
                MaxConnections = 1
            };

            this.Inputs.Add(OnClickFlow);
        }

        public void Serializer(XmlWriter xmlWriter, object obj)
        {
            NodeSerializer.Serializer(ref xmlWriter, ref obj);
        }

        public object Deserialize(XElement xElement)
        {
            StartNodeViewModel instance = (StartNodeViewModel)NodeSerializer.Deserialize(ref xElement, new StartNodeViewModel());
            return instance;
        }

        
    }
}
