using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using DynamicData;
using EasyMacro.Model.Node;
using EasyMacro.ViewModel.Node.Editors;
using ExtendedXmlSerializer.ExtensionModel.Xml;
using NodeNetwork.Toolkit.ValueNode;
using NodeNetwork.ViewModels;
using NodeNetwork.Views;
using ReactiveUI;

namespace EasyMacro.ViewModel.Node.NodeObject
{
    // 그룹 내부에서 그룹 밖으로 내보내는 노드
    public class OutputNodeViewModel : CodeGenNodeViewModel, INodeSerializable, IExtendedXmlCustomSerializer
    { 
        static OutputNodeViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new NodeView(), typeof(IViewFor<OutputNodeViewModel>));
        }

        public ValueNodeInputViewModel<int?> ResultInput { get; }

        public override NodeOutputViewModel GetOutputViewModel => null;

        public override void Serializer(XmlWriter xmlWriter, object obj)
        {
            NodeSerializer.SerializerOfNodeViewModel(ref xmlWriter, ref obj);
        }

        public override object Deserialize(XElement xElement)
        {
            OutputNodeViewModel instance = (OutputNodeViewModel)NodeSerializer.DeserializeOfNoveViewModel(ref xElement, this);
            return instance;
        }

        public override void Connect(INodeSerializable instance, List<INodeSerializable> obj)
        {
            throw new NotImplementedException();
        }

        public OutputNodeViewModel()
        {
            Name = "Output";

            this.CanBeRemovedByUser = false;

            ResultInput = new ValueNodeInputViewModel<int?>
            {
                Name = "Value",
                Editor = new IntegerValueEditorViewModel()
            };
            this.Inputs.Add(ResultInput);

            this.Hash = Common.HashGen.RandomHashGen(10);
        }

        public OutputNodeViewModel(string hash)
        {
            this.Hash = hash;
        }
    }
}
