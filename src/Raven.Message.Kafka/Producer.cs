using Raven.Message.Kafka.Abstract.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Message.Kafka
{
    /// <summary>
    /// 生产者
    /// </summary>
    public class Producer : IDisposable
    {
        ConfluentKafkaProducerContainer _producerManager;
        bool _disposeCalled = false;//是否已开始关闭

        internal IBrokerConfig BrokerConfig { get; set; }
        internal Producer(IBrokerConfig brokerConfig)
        {
            BrokerConfig = brokerConfig;
            _producerManager = new ConfluentKafkaProducerContainer(this);
        }
        /// <summary>
        /// 生产消息
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="topic">消息主题</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        public Task ProduceAsync<T>(string topic, T message)
        {
            try
            {
                if (_disposeCalled)
                    throw new InvalidOperationException("producer is disposed");
                var topicConfig = BrokerConfig?.Topics?.FirstOrDefault(t => t.Name == topic);
                var producer = _producerManager.GetProducer<T>(topic, topicConfig);
                return producer.ProduceAsync(topic, null, message);
            }
            catch (Exception ex)
            {
                LogHelpler.Error(ex);
                throw;
            }
        }

        public void Dispose()
        {
            try
            {
                _disposeCalled = true;
                LogHelpler.Info("producer disponsing");
                _producerManager.ReleaseAllProducers();
                LogHelpler.Info("producer disposed");
            }
            catch (Exception ex)
            {
                LogHelpler.Error(ex);
            }
        }
    }
}
