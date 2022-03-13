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
    internal class DelaySerializer : IExtendedXmlCustomSerializer<Delay>
    {
        public Delay Deserialize(XElement element)
        {
            object obj = Deserializer.Deserialize(element);
            return obj as Delay;
        }

        public void Serializer(XmlWriter xmlWriter, Delay obj)
        {
            xmlWriter.WriteElementString(nameof(Delay.Time), obj.Time.ToString(CultureInfo.InvariantCulture));
        }
    }
}
