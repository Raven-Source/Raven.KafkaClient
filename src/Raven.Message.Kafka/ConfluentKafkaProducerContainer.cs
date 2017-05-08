using Confluent.Kafka;
using Raven.Message.Kafka.Abstract.Configuration;
using Raven.Message.Kafka.Serialization;
using Raven.Serializer;
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
        Producer _producer;
        Dictionary<string, object> _producerDict = new Dictionary<string, object>();
        const string ProducerKeyFormat = "{0}-@-#-{1}-@-#-{2}";

        internal ConfluentKafkaProducerContainer(Producer producer)
        {
            _producer = producer;
        }

        internal Producer<Null, T> GetProducer<T>(string topic, IBrokerConfig brokerConfig, Action<Producer<Null, T>> onProducerCreate, IDataSerializer serializer)
        {
            return GetProducer<Null, T>(topic, brokerConfig, onProducerCreate, serializer);
        }

        internal Producer<TKey, TValue> GetProducer<TKey, TValue>(string topic, IBrokerConfig brokerConfig, Action<Producer<TKey, TValue>> onProducerCreate, IDataSerializer serializer)
        {
            string producerKey = string.Format(ProducerKeyFormat, topic, typeof(TKey).FullName, typeof(TValue).FullName);
            if (_producerDict.ContainsKey(producerKey))
                return _producerDict[producerKey] as Producer<TKey, TValue>;
            lock (_producerDict)
            {
                if (_producerDict.ContainsKey(producerKey))
                    return _producerDict[producerKey] as Producer<TKey, TValue>;

                var producer = CreateProducer<TKey, TValue>(topic, brokerConfig, onProducerCreate, serializer);
                _producerDict.Add(producerKey, producer);
                return producer;
            }
        }


        Type[] _flushParmeters = new Type[0];
        internal void ReleaseAllProducers()
        {
            lock (_producerDict)
            {
                foreach (var producer in _producerDict.Values)
                {
                    var method = producer.GetType().GetMethod("Flush", _flushParmeters);
                    method.Invoke(producer, null);
                    var toDispose = producer as IDisposable;
                    toDispose.Dispose();
                }
            }
        }

        Producer<TKey, TValue> CreateProducer<TKey, TValue>(string topic, IBrokerConfig brokerConfig, Action<Producer<TKey, TValue>> onProducerCreate, IDataSerializer serializer)
        {
            Dictionary<string, object> config = new Dictionary<string, object> { { "bootstrap.servers", _producer.BrokerConfig.Uri } };
            var topicConfig = brokerConfig?.Topics?.FirstOrDefault(t => t.Name == topic);
            if (topicConfig != null && topicConfig.ProducerConfig != null)
            {
                var producerConfig = topicConfig.ProducerConfig;
                if (producerConfig.acks != null)
                    config.Add("default.topic.config", new Dictionary<string, object> { { "acks", producerConfig.acks.Value } });
                if (producerConfig.topic_metadata_refresh_inverval_ms != null)
                    config.Add("topic.metadata.refresh.interval.ms", producerConfig.topic_metadata_refresh_inverval_ms.Value);
                if (producerConfig.metadata_max_age_ms != null)
                    config.Add("metadata.max.age.ms", producerConfig.metadata_max_age_ms.Value);
                if (!string.IsNullOrEmpty(producerConfig.debug))
                    config.Add("debug", producerConfig.debug);
            }
            SerializerType serializerType = (topicConfig != null && topicConfig.SerializerType != null) ? topicConfig.SerializerType.Value : brokerConfig.SerializerType;
            ConfluentKafkaSerializer<TValue> kafkaSerializer = null;
            if (serializer == null)
                kafkaSerializer = new ConfluentKafkaSerializer<TValue>(serializerType);
            else
                kafkaSerializer = new ConfluentKafkaSerializer<TValue>(serializer);
            ConfluentKafkaSerializer<TKey> kafkaKeySerializer = null;
            if (typeof(TKey) != typeof(Null))
            {
                if (serializer == null)
                    kafkaKeySerializer = new ConfluentKafkaSerializer<TKey>(serializerType);
                else
                    kafkaKeySerializer = new ConfluentKafkaSerializer<TKey>(serializer);
            }
            Producer<TKey, TValue> producer = new Producer<TKey, TValue>(config, kafkaKeySerializer, kafkaSerializer);
            onProducerCreate?.Invoke(producer);
            return producer;
        }
    }
}
