using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using EasyMacroAPI.Command;
using ExtendedXmlSerializer;
using ExtendedXmlSerializer.ExtensionModel.Xml;

namespace EasyMacroAPI.CommandSerializer
{
    internal class DelaySerializer : IExtendedXmlCustomSerializer<Delay>, IExtendedXmlCustomSerializer
    {
        public Delay Deserialize(XElement element)
        {
            XElement element_Time = element.Member(nameof(Delay.Time));
            if (element_Time != null)
            {
                int element1 = Convert.ToInt32(element_Time.Value);
                return new Delay(element1);
            }
            throw new InvalidOperationException("Invalid of xml input, class \"" + element.Name.LocalName + "\" is not compatible of \"" + nameof(Delay) + "\" with this Deserializer");
        }

        public void Serializer(XmlWriter xmlWriter, Delay obj)
        {
            xmlWriter.WriteElementString(nameof(Delay.Time), obj.Time.ToString(CultureInfo.InvariantCulture));
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
}
