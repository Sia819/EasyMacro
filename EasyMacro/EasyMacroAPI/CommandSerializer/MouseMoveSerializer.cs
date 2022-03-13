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
/*
 * 
 * public MouseMove Deserialize(XElement element)
        {
            XElement element_X = element.Member(nameof(MouseMove.X));
            XElement element_Y = element.Member(nameof(MouseMove.Y));
            if (element_X != null && element_Y != null)
            {
                int element1 = Convert.ToInt32(element_X.Value);
                int element2 = Convert.ToInt32(element_Y.Value);
                return new MouseMove(element1, element2);
            }
            throw new InvalidOperationException("Invalid xml for class \"" + nameof(MouseMove) + "\" with serializer");
        }

        public void Serializer(XmlWriter xmlWriter, MouseMove obj)
        {
            xmlWriter.WriteElementString(nameof(MouseMove.X), obj.X.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteElementString(nameof(MouseMove.Y), obj.Y.ToString(CultureInfo.InvariantCulture));
        }
 */