using EasyMacroAPI.Command;
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
    internal class Deserializer
    {
        public static object Deserialize(XElement element)
        {
            switch (element.Name.LocalName)
            {
                case nameof(MouseMove):
                    XElement element_X = element.Member(nameof(MouseMove.X));
                    XElement element_Y = element.Member(nameof(MouseMove.Y));
                    if (element_X != null && element_Y != null)
                    {
                        int element1 = Convert.ToInt32(element_X.Value);
                        int element2 = Convert.ToInt32(element_Y.Value);
                        return new MouseMove(element1, element2);
                    }
                    throw new InvalidOperationException("Invalid xml for class \"" + nameof(MouseMove) + "\" with serializer");
                    break;
                case nameof(Delay):
                    XElement element_Time = element.Member(nameof(Delay.Time));
                    if (element_Time != null)
                    {
                        int element1 = Convert.ToInt32(element_Time.Value);
                        return new Delay(element1);
                    }
                    throw new InvalidOperationException("Invalid xml for class \"" + nameof(Delay) + "\" with serializer");
                    break;
                default:
                    throw new InvalidOperationException("Invalid xml for class \"" + "Not Supported" + "\" with serializer");
                    break;
            }
            
            
        }

    }
}
