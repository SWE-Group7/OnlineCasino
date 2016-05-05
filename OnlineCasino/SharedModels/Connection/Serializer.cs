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
            if (obj != null)
            {
                using (var memStream = new MemoryStream())
                {
                    formatter.Serialize(memStream, obj);
                    return memStream.ToArray();
                }
            }
            else
            {
                return new byte[0];
            }
            
            
        }

        public static object Deserialize(byte[] bytes)
        {
            if(bytes.Length != 0)
            {
                using (var memStream = new MemoryStream(bytes))
                {
                    return formatter.Deserialize(memStream);
                }
            }
            else
            {
                return null;
            }
            
        }
    }
}
