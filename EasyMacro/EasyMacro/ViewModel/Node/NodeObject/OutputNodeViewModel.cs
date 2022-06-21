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
    public class OutputNodeViewModel : NodeViewModel, IExtendedXmlCustomSerializer
    {
        static OutputNodeViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new NodeView(), typeof(IViewFor<OutputNodeViewModel>));
        }

        public ValueNodeInputViewModel<int?> ResultInput { get; }

        public void Serializer(XmlWriter xmlWriter, object obj)
        {
            NodeSerializer.Serializer(ref xmlWriter, ref obj);
        }

        public object Deserialize(XElement xElement)
        {
            OutputNodeViewModel instance = (OutputNodeViewModel)NodeSerializer.Deserialize(ref xElement, new OutputNodeViewModel());
            return instance;
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
        }
    }
}
