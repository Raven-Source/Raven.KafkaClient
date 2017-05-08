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
    /// 服务器连接
    /// </summary>
    public class Connection : IDisposable
    {
        /// <summary>
        /// 服务器配置
        /// </summary>
        public IBrokerConfig BrokerConfig { get; private set; }
        /// <summary>
        /// 获取生产者实例
        /// </summary>
        public Producer Producer { get; private set; }
        /// <summary>
        /// 日志
        /// </summary>
        internal LogHelpler Log { get; set; }
        /// <summary>
        /// 设置序列化器
        /// </summary>
        /// <param name="serializer"></param>
        public void SetSerializer(Serializer.IDataSerializer serializer)
        {
            Producer.Serializer = serializer;
        }

        public void Dispose()
        {
            Producer.Dispose();
        }

        internal Connection(IBrokerConfig brokerConfig, ILog log)
        {
            if (brokerConfig == null)
                throw new ArgumentNullException(nameof(brokerConfig));
            Log = new LogHelpler(log);
            Log.Info("create connection for broker {0}, {1}", brokerConfig.Name, brokerConfig.Uri);
            BrokerConfig = brokerConfig;
            Producer = new Producer(brokerConfig, this);
        }
    }
}
