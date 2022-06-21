/*
using System;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using EasyMacroAPI.Command;
using ExtendedXmlSerializer;
using ExtendedXmlSerializer.ExtensionModel.Xml;

namespace EasyMacroAPI.CommandSerializer
{
    internal class DelaySerializer : IExtendedXmlCustomSerializer
    {
        public void Serializer(XmlWriter xmlWriter, object instance)
        {
            Delay obj = instance as Delay;
            xmlWriter.WriteElementString(nameof(Delay.Time), obj.Time.ToString(CultureInfo.InvariantCulture));
        }

        object IExtendedXmlCustomSerializer.Deserialize(XElement xElement)
        {
            XElement element_Time = xElement.Member(nameof(Delay.Time));
            if (element_Time != null)
            {
                int element1 = Convert.ToInt32(element_Time.Value);
                return new Delay(element1);
            }
            throw new InvalidOperationException("Invalid of xml input, class \"" + xElement.Name.LocalName + "\" is not compatible of \"" + nameof(Delay) + "\" with this Deserializer");
        }
    }
}
*/