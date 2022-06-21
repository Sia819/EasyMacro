/*
using System;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;
using ExtendedXmlSerializer;
using EasyMacroAPI.Command;
using ExtendedXmlSerializer.ExtensionModel.Xml;

namespace EasyMacroAPI.CommandSerializer
{
    internal class MouseMoveSerializer : IExtendedXmlCustomSerializer
    {
        public void Serializer(XmlWriter xmlWriter, object instance)
        {
            MouseMove obj = instance as MouseMove;
            xmlWriter.WriteElementString(nameof(MouseMove.X), obj.X.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteElementString(nameof(MouseMove.Y), obj.Y.ToString(CultureInfo.InvariantCulture));
        }

        object IExtendedXmlCustomSerializer.Deserialize(XElement xElement)
        {
            XElement element_X = xElement.Member(nameof(MouseMove.X));
            XElement element_Y = xElement.Member(nameof(MouseMove.Y));
            if (element_X != null && element_Y != null)
            {
                int element1 = Convert.ToInt32(element_X.Value);
                int element2 = Convert.ToInt32(element_Y.Value);
                return new MouseMove(element1, element2);
            }
            throw new InvalidOperationException("Invalid of xml input, class \"" + xElement.Name.LocalName + "\" is not compatible of \"" + nameof(Delay) + "\" with this Deserializer");
        }
    }
}
*/