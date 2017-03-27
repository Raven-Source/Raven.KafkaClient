using Confluent.Kafka;
using Raven.Message.Kafka.Abstract.Configuration;
using Raven.Message.Kafka.Serialization;
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
        const string ProducerKeyFormat = "{0}-@-#-{1}";

        internal ConfluentKafkaProducerContainer(Producer producer)
        {
            _producer = producer;
        }

        internal Producer<Null, T> GetProducer<T>(string topic, ITopicConfig topicConfig)
        {
            string producerKey = string.Format(ProducerKeyFormat, topic, typeof(T).FullName);
            if (_producerDict.ContainsKey(producerKey))
                return _producerDict[producerKey] as Producer<Null, T>;
            lock (_producerDict)
            {
                if (_producerDict.ContainsKey(producerKey))
                    return _producerDict[producerKey] as Producer<Null, T>;

                var producer = CreateProducer<T>(topic, topicConfig);
                _producerDict.Add(producerKey, producer);
                return producer;
            }
        }


        internal void ReleaseAllProducers()
        {
            lock (_producerDict)
            {
                foreach (var producer in _producerDict.Values)
                {
                    var method = producer.GetType().GetMethod("Flush");
                    method.Invoke(producer, null);
                    var toDispose = producer as IDisposable;
                    toDispose.Dispose();
                }
            }
        }

        Producer<Null, T> CreateProducer<T>(string topic, ITopicConfig topicConfig)
        {
            Dictionary<string, object> config = new Dictionary<string, object> { { "bootstrap.servers", _producer.BrokerConfig.Uri } };
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
            ConfluentKafkaSerializer<T> serializer;
            if (topicConfig != null && topicConfig.SerializerType != null)
                serializer = new ConfluentKafkaSerializer<T>(topicConfig.SerializerType.Value);
            else
                serializer = new ConfluentKafkaSerializer<T>();
            Producer<Null, T> producer = new Producer<Null, T>(config, null, serializer);
            producer.OnLog += Producer_OnLog;
            producer.OnError += Producer_OnError;
            producer.OnStatistics += Producer_OnStatistics;
            return producer;
        }

        private void Producer_OnStatistics(object sender, string e)
        {
            LogHelpler.Debug(e);
        }

        private void Producer_OnError(object sender, Error e)
        {
            LogHelpler.Error(e.ToString());
        }

        private void Producer_OnLog(object sender, LogMessage e)
        {
            LogHelpler.Info(e.ToString());
        }
    }
}
