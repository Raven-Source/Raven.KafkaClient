using Raven.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Message.Kafka.Abstract.Configuration
{
    /// <summary>
    /// 客户端配置
    /// </summary>
    public interface IClientConfig
    {
        /// <summary>
        /// 日志实现类
        /// </summary>
        string LogType { get; }
        /// <summary>
        /// 服务器配置
        /// </summary>
        IEnumerable<IBrokerConfig> Brokers { get; }
    }
}
