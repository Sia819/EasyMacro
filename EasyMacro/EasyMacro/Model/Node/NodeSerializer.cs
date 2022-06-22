using ExtendedXmlSerializer.ExtensionModel.Xml;
using NodeNetwork.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Reflection;
using EasyMacro.ViewModel.Node;

namespace EasyMacro.Model.Node
{
    internal class NodeSerializer : IExtendedXmlCustomSerializer
    {
        // NodeViewModel에 대한 Serializer
        public static void SerializerOfNodeViewModel(ref XmlWriter xmlWriter, ref object obj)
        {
            NodeViewModel instance = obj as NodeViewModel;

            xmlWriter.WriteElementString(nameof(instance.Position.X), instance.Position.X.ToString());
            xmlWriter.WriteElementString(nameof(instance.Position.Y), instance.Position.Y.ToString());
        }

        // NodeViewModel에 대한 Deserialize
        public static object DeserializeOfNoveViewModel(ref XElement xElement, NodeViewModel obj)
        {
            Dictionary<string, XElement> dictionary = NodeSerializer.XElementToDictionary(xElement);
            obj.Position = new System.Windows.Point (double.TryParse(dictionary["X"].Value, out double x) ? x : 0,
                                                     double.TryParse(dictionary["Y"].Value, out double y) ? y : 0);
            return obj;
        }

        // Deserializer 전용이므로 사용되지 않음
        public void Serializer(XmlWriter xmlWriter, object instance)
        {
            throw new NotImplementedException();
        }

        // 모든 NodeViewModel의 하위클래스에 대한 분배기
        public object Deserialize(XElement xElement)
        {
            Dictionary<string, XElement> dictionary = NodeSerializer.XElementToDictionary(xElement);
            string hash = dictionary["NodeHash"].Value;
            Type type = Type.GetType("EasyMacro.ViewModel.Node.NodeObject." + xElement.Name.LocalName);
            var obj = Activator.CreateInstance(type, hash);
            CodeGenNodeViewModel instance = obj as CodeGenNodeViewModel;
            return instance.Deserialize(xElement);
        }

        public static Dictionary<string, XElement> XElementToDictionary(XElement xElement)
        {
            Dictionary<string, XElement> xNodeList = new();
            for (XElement xNode = xElement.FirstNode as XElement; xNode != null; xNode = xNode.NextNode as XElement)
            {
                xNodeList.Add(xNode.Name.LocalName, xNode);
            }
            return xNodeList;
        }
        
    }
}
