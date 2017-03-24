using Raven.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Message.Kafka.Configuration
{
    /// <summary>
    /// 客户端配置
    /// </summary>
    public class ClientConfig
    {
        /// <summary>
        /// 日志实现类
        /// </summary>
        public string LogType { get; set; }
        /// <summary>
        /// 序列化类型
        /// </summary>
        public SerializerType SerializerType { get; set; }
        /// <summary>
        /// 服务器配置
        /// </summary>
        public IEnumerable<BrokerConfig> Brokers { get; set; }

        public override string ToString()
        {
            return $"LogType:{LogType},SerializerType:{SerializerType}";
        }
    }
}
