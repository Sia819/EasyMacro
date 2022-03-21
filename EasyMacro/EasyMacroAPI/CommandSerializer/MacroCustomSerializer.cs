using EasyMacroAPI.Command;
using EasyMacroAPI.Model;
using ExtendedXmlSerializer;
using ExtendedXmlSerializer.ExtensionModel.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace EasyMacroAPI.CommandSerializer
{
    public class MacroCustomSerializer : IExtendedXmlCustomSerializer<IAction>
    {
        Dictionary<Type, IExtendedXmlCustomSerializer> registeredType;
        

        public MacroCustomSerializer Register<T>(IExtendedXmlCustomSerializer command)
        {
            registeredType.Add(typeof(T), command);

            return this;
        }

        public void Serializer(XmlWriter xmlWriter, IAction obj)
        {
            foreach (var i in registeredType)
            {
                if (obj.GetType().Name == i.Key.Name)
                {
                    // 등록된 각 타입에 맞는 시리얼라이저를 호출합니다.
                    i.Value.Serializer(xmlWriter, obj);
                    return;
                }
            }
            throw new InvalidOperationException("Invalid xml for class \"" + "Not Supported" + "\" with this Deserializer");
        }

        ///
        public IAction Deserialize(XElement element)
        {
            foreach (var i in registeredType)
            {
                if (element.Name.LocalName == i.Key.Name)
                {
                    return (IAction)i.Value.Deserialize(element);
                }
            }
            throw new InvalidOperationException("Invalid xml for class \"" + "Not Supported" + "\" with this Serializer");
        }

        public MacroCustomSerializer()
        {
            this.registeredType = new();
        }


    }
}
