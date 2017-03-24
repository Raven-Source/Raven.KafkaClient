using Raven.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Message.Kafka.Serialization
{
    /// <summary>
    /// 序列化器容器
    /// </summary>
    internal class SerializerContainer
    {
        static List<Tuple<SerializerType, IDataSerializer>> Serizlizers = new List<Tuple<SerializerType, IDataSerializer>>(8);

        public static IDataSerializer GetSerializer(SerializerType serializerType)
        {
            var s = Serizlizers.Find((p) => p.Item1 == serializerType);
            if (s == null)
            {
                lock (Serizlizers)
                {
                    s = new Tuple<SerializerType, IDataSerializer>(serializerType, SerializerFactory.Create(serializerType));
                    List<Tuple<SerializerType, IDataSerializer>> copy = new List<Tuple<SerializerType, IDataSerializer>>(Serizlizers);
                    copy.Add(s);
                    Serizlizers = copy;
                }
            }
            return s.Item2;
        }
    }
}
