using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Message.Kafka.Abstract.Configuration
{
    /// <summary>
    /// 生产者配置
    /// </summary>
    /// <see cref="https://github.com/edenhill/librdkafka/blob/master/CONFIGURATION.md"/>
    public interface IProducerConfig
    {
        /// <summary>
        /// 元数据刷新毫秒
        /// </summary>
        int? topic_metadata_refresh_inverval_ms { get; }
        /// <summary>
        /// 元数据最大缓存时间
        /// </summary>
        int? metadata_max_age_ms { get; }
        /// <summary>
        /// 生产消息确认
        /// 0，服务器不给客户端发送确认
        /// 1，leader接受消息，服务器给客户端发送确认
        /// -1，所有isr节点接受消息，服务器给客户端发送确认
        /// </summary>
        int? acks { get; }
        /// <summary>
        /// 调试开关，多个调试用逗号分隔
        /// broker,topic,msg
        /// </summary>
        string debug { get;}
    }
}
