using Raven.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Message.Kafka.Configuration
{
    /// <summary>
    /// 主题配置
    /// </summary>
    public class TopicConfig
    {
        /// <summary>
        /// 主题名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 序列化类型，可覆盖<see cref="ClientConfig.SerializerType"/>
        /// </summary>
        public SerializerType? SerializerType { get; set; }
        /// <summary>
        /// 生产者配置
        /// </summary>
        public ProducerConfig ProducerConfig { get; set; }
    }
}
