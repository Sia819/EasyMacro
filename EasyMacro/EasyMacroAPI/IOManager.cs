using System.Xml.Serialization;
using System.IO;

namespace EasyMacroAPI
{
    class IOManager
    {
        string savepath;
        public void Serialization(object obj)
        {
            // if you use Network driver location, this implementation to solve the permission problems. (backup and remove works)
            if (File.Exists(savepath))
                File.Move(savepath, savepath + ".bak");
            using (StreamWriter wr = new StreamWriter(savepath))
            {
                XmlSerializer xs = new XmlSerializer(obj.GetType());
                xs.Serialize(wr, obj);
            }
            if (File.Exists(savepath + ".bak"))
                File.Delete(savepath + ".bak");
        }

        public T DeSerialization<T>()
        {
            T obj;
            using (var reader = new StreamReader(savepath))
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                obj = (T)xs.Deserialize(reader);
            }
            return obj;
        }
    }
}
