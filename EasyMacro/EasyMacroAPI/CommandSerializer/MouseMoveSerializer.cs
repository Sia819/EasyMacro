using System;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;
using ExtendedXmlSerializer;
using EasyMacroAPI.Command;
using ExtendedXmlSerializer.ExtensionModel.Xml;
using ExtendedXmlSerializer.ContentModel;
using ExtendedXmlSerializer.ContentModel.Format;

namespace EasyMacroAPI.CommandSerializer
{
    internal class MouseMoveSerializer : IExtendedXmlCustomSerializer<MouseMove>
    {
        public MouseMove Deserialize(XElement element)
        {
            object obj = Deserializer.Deserialize(element);
            return obj as MouseMove;
        }

        public void Serializer(XmlWriter xmlWriter, MouseMove obj)
        {
            xmlWriter.WriteElementString(nameof(MouseMove.X), obj.X.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteElementString(nameof(MouseMove.Y), obj.Y.ToString(CultureInfo.InvariantCulture));
        }
    }
}