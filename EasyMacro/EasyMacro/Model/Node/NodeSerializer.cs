using ExtendedXmlSerializer.ExtensionModel.Xml;
using NodeNetwork.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace EasyMacro.Model.Node
{
    internal static class NodeSerializer
    {
        public static void Serializer(ref XmlWriter xmlWriter, ref object obj)
        {
            NodeViewModel instance = obj as NodeViewModel;

            xmlWriter.WriteElementString(nameof(instance.Position.X), instance.Position.X.ToString());
            xmlWriter.WriteElementString(nameof(instance.Position.Y), instance.Position.Y.ToString());
        }

        public static object Deserialize(ref XElement xElement, NodeViewModel obj)
        {
            return obj;
        }
    }
}
