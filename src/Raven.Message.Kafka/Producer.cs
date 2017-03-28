using Confluent.Kafka;
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
        /// <returns>生产消息任务</returns>
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
        /// <summary>
        /// 生产消息，且不需要返回值
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="topic">消息主题</param>
        /// <param name="message">消息</param>
        /// <remarks>
        /// 此方法性能更佳
        /// </remarks>
        public void ProduceAndForget<T>(string topic, T message)
        {
            try
            {
                if (_disposeCalled)
                    throw new InvalidOperationException("producer is disposed");
                var topicConfig = BrokerConfig?.Topics?.FirstOrDefault(t => t.Name == topic);
                var producer = _producerManager.GetProducer<T>(topic, topicConfig);
                producer.ProduceAsync(topic, null, message, DeliverHandler<T>.Instance);
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

        class DeliverHandler<T> : IDeliveryHandler<Null, T>
        {
            static DeliverHandler<T> _instance;
            public static DeliverHandler<T> Instance
            {
                get
                {
                    if (_instance == null)
                        _instance = new DeliverHandler<T>();
                    return _instance;
                }
            }

            public bool MarshalData
            {
                get
                {
                    return false;
                }
            }

            public void HandleDeliveryReport(Message<Null, T> message)
            {
                if (message != null && message.Error != null && message.Error.HasError)
                {
                    LogHelpler.Error("error produce report, {0}, {1}", message.Error.Code, message.Error.Reason);
                }
            }
        }
    }
}
