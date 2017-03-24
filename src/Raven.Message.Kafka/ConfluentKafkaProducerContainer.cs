using Confluent.Kafka;
using Raven.Message.Kafka.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Message.Kafka
{
    /// <summary>
    /// 生产者容器
    /// </summary>
    internal class ConfluentKafkaProducerContainer
    {
        internal Producer<Null, T> GetProducer<T>(string topic, TopicConfig topicConfig)
        {
            throw new NotImplementedException();
        }

        internal void ReleaseAllProducers()
        {
            throw new NotImplementedException();
        }
    }
}
