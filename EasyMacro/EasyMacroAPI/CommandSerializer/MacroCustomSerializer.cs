/*
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using EasyMacroAPI.Command;
using EasyMacroAPI.Model;
using ExtendedXmlSerializer;
using ExtendedXmlSerializer.ExtensionModel.Xml;

namespace EasyMacroAPI.CommandSerializer
{
    /// <summary>
    /// 매크로 명령과 ExtendedXmlSerializer라이브러리를 연결시켜주는 클래스입니다.
    /// 매크로 명령이 추가되어도 이 클래스는 수정할 필요가 없습니다.
    /// 새로운 매크로 명령 추가시 CommandSerializer 폴더내 새로운 시리얼라이저 객체를 만들어주세요.
    /// </summary>
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
*/