using Confluent.Kafka;
using Raven.Message.Kafka.Abstract;
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

        internal Connection Connection { get; set; }

        LogHelpler Log
        {
            get
            {
                return Connection?.Log;
            }
        }

        internal Producer(IBrokerConfig brokerConfig, Connection connection)
        {
            BrokerConfig = brokerConfig;
            Connection = connection;
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
                    throw new InvalidOperationException(BuildIdentityString("producer is disposed"));
                var producer = _producerManager.GetProducer<T>(topic, BrokerConfig, OnProducerCreate);
                return producer.ProduceAsync(topic, null, message);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }
        /// <summary>
        /// 生产消息
        /// </summary>
        /// <typeparam name="TKey">消息关键字类型</typeparam>
        /// <typeparam name="TMessage">消息类型</typeparam>
        /// <param name="topic">主题</param>
        /// <param name="key">消息关键字</param>
        /// <param name="message">消息</param>
        /// <returns>生产消息任务</returns>
        public Task ProduceAsync<TKey, TMessage>(string topic, TKey key, TMessage message)
        {
            try
            {
                if (_disposeCalled)
                    throw new InvalidOperationException(BuildIdentityString("producer is disposed"));
                var producer = _producerManager.GetProducer<TKey, TMessage>(topic, BrokerConfig, OnProducerCreate);
                return producer.ProduceAsync(topic, key, message);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }
        /// <summary>
        /// 生产消息，且不需要返回值
        /// </summary>
        /// <typeparam name="TMessage">消息类型</typeparam>
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
                    throw new InvalidOperationException(BuildIdentityString("producer is disposed"));
                var producer = _producerManager.GetProducer<T>(topic, BrokerConfig, OnProducerCreate);
                var handler = DeliverHandler<T>.Instance;
                if (handler.Log == null)
                    handler.Log = Log;
                producer.ProduceAsync(topic, null, message, handler);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }
        /// <summary>
        /// 生产消息，且不需要返回值
        /// </summary>
        /// <typeparam name="TKey">消息关键字类型</typeparam>
        /// <typeparam name="TMessage">消息类型</typeparam>
        /// <param name="topic">消息主题</param>
        /// <param name="key">消息关键字</param>
        /// <param name="message">消息</param>
        /// <remarks>
        /// 此方法性能更佳
        /// </remarks>
        public void ProduceAndForget<TKey, TMessage>(string topic, TKey key, TMessage message)
        {
            try
            {
                if (_disposeCalled)
                    throw new InvalidOperationException(BuildIdentityString("producer is disposed"));
                var producer = _producerManager.GetProducer<TKey, TMessage>(topic, BrokerConfig, OnProducerCreate);
                var handler = DeliverHandler<TKey, TMessage>.Instance;
                if (handler.Log == null)
                    handler.Log = Log;
                producer.ProduceAsync(topic, key, message, handler);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }
        public void Dispose()
        {
            try
            {
                if (_disposeCalled)
                    return;
                _disposeCalled = true;
                Log.Info("producer disponsing:{0} {1}", BrokerConfig.Name, BrokerConfig.Uri);
                _producerManager.ReleaseAllProducers();
                Log.Info("producer disposed:{0} {1}", BrokerConfig.Name, BrokerConfig.Uri);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        string BuildIdentityString(string message)
        {
            return string.Format("{0} [{1},{2}]", message, BrokerConfig.Name, BrokerConfig.Uri);
        }

        class DeliverHandler<TKey, TMessage> : IDeliveryHandler<TKey, TMessage>
        {
            static DeliverHandler<TKey, TMessage> _instance;
            public static DeliverHandler<TKey, TMessage> Instance
            {
                get
                {
                    if (_instance == null)
                        _instance = new DeliverHandler<TKey, TMessage>();
                    return _instance;
                }
            }

            public LogHelpler Log { get; set; }

            public bool MarshalData
            {
                get
                {
                    return false;
                }
            }

            public void HandleDeliveryReport(Message<TKey, TMessage> message)
            {
                if (message != null && message.Error != null && message.Error.HasError)
                {
                    Log.Error("error produce report, {0}, {1}", message.Error.Code, message.Error.Reason);
                }
            }
        }
        class DeliverHandler<T> : DeliverHandler<Null, T> { }

        void OnProducerCreate<TKey, TValue>(Producer<TKey, TValue> producer)
        {
            producer.OnLog += Producer_OnLog;
            producer.OnStatistics += Producer_OnStatistics;
            producer.OnError += Producer_OnError;
        }

        void Producer_OnError(object sender, Error e)
        {
            Log.Error($"Code:{e.Code},HasError:{e.HasError},IsBrokerError:{e.IsBrokerError},IsLocalError:{e.IsLocalError},Reason:{e.Reason}");
        }

        void Producer_OnStatistics(object sender, string e)
        {
            Log.Debug(e);
        }

        void Producer_OnLog(object sender, LogMessage e)
        {
            switch (e.Level)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    Log.Error("{0}|{1}|{2}", e.Name, e.Facility, e.Message);
                    break;
                case 4:
                case 5:
                case 6:
                    Log.Info("{0}|{1}|{2}", e.Name, e.Facility, e.Message);
                    break;
                default:
                    Log.Debug("{0}|{1}|{2}", e.Name, e.Facility, e.Message);
                    break;
            }
        }
    }
}
