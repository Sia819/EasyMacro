using System;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;
using ExtendedXmlSerializer;
using EasyMacroAPI.Command;
using ExtendedXmlSerializer.ExtensionModel.Xml;
using EasyMacroAPI.Model;

namespace EasyMacroAPI.CommandSerializer
{
    //MacroSerializer<IAction>, IExtendedXmlCustomSerializer<IAction> 
    internal class MouseMoveSerializer : IExtendedXmlCustomSerializer<MouseMove>, IExtendedXmlCustomSerializer
    {

        public void Serializer(XmlWriter xmlWriter, MouseMove obj)
        {
            xmlWriter.WriteElementString(nameof(MouseMove.X), obj.X.ToString(CultureInfo.InvariantCulture));
            xmlWriter.WriteElementString(nameof(MouseMove.Y), obj.Y.ToString(CultureInfo.InvariantCulture));
        }

        public MouseMove Deserialize(XElement element)
        {
            XElement element_X = element.Member(nameof(MouseMove.X));
            XElement element_Y = element.Member(nameof(MouseMove.Y));
            if (element_X != null && element_Y != null)
            {
                int element1 = Convert.ToInt32(element_X.Value);
                int element2 = Convert.ToInt32(element_Y.Value);
                return new MouseMove(element1, element2);
            }
            throw new InvalidOperationException("Invalid xml for class \"" + nameof(MouseMove) + "\" with Deserializer");
        }

        public void Serializer(XmlWriter xmlWriter, object instance)
        {
            this.Serializer(xmlWriter, instance);
        }

        object IExtendedXmlCustomSerializer.Deserialize(XElement xElement)
        {
            return this.Deserialize(xElement);
        }

    }

    /*
    abstract class MacroSerializer<T> where T : IExtendedXmlCustomSerializer<T>, IExtendedXmlCustomSerializer
    {
        public object Deserialize(XElement xElement)
        {
            return ((IExtendedXmlCustomSerializer<T>)this).Deserialize(xElement);
        }

        public void Serializer(XmlWriter xmlWriter, object instance)
        {
            ((IExtendedXmlCustomSerializer<T>)this).Serializer(xmlWriter, (T)instance);
        }
    }
    */


}