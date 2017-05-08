using Raven.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Message.Kafka.Serialization
{
    internal class ConfluentKafkaSerializer<T> : Confluent.Kafka.Serialization.ISerializer<T>
    {
        IDataSerializer _dataSerializer;

        public ConfluentKafkaSerializer()
        {
            _dataSerializer = SerializerContainer.GetSerializer();
        }

        public ConfluentKafkaSerializer(SerializerType serializerType)
        {
            _dataSerializer = SerializerContainer.GetSerializer(serializerType);
        }

        public ConfluentKafkaSerializer(IDataSerializer serializer)
        {
            _dataSerializer = serializer;
        }

        public byte[] Serialize(T data)
        {
            return _dataSerializer.Serialize(data);
        }
    }
}
