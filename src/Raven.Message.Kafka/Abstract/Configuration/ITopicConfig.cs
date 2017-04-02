using Raven.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Message.Kafka.Abstract.Configuration
{
    /// <summary>
    /// 主题配置
    /// </summary>
    public interface ITopicConfig
    {
        /// <summary>
        /// 主题名
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 序列化类型，可覆盖<see cref="IBrokerConfig.SerializerType"/>
        /// </summary>
        SerializerType? SerializerType { get; }
        /// <summary>
        /// 生产者配置
        /// </summary>
        IProducerConfig ProducerConfig { get; }
    }
}
