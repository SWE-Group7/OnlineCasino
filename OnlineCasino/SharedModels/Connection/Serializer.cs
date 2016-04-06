using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Connection
{
    public static class Serializer
    {
        public static BinaryFormatter formatter = new BinaryFormatter();

        public static byte[] Serialize(object obj)
        {
            using (var memStream = new MemoryStream())
            {
                formatter.Serialize(memStream, obj);
                return memStream.ToArray();
            }
        }

        public static object Deserialize(byte[] bytes)
        {
            using (var memStream = new MemoryStream(bytes))
            {
                return formatter.Deserialize(memStream);
            }
        }
    }
}
