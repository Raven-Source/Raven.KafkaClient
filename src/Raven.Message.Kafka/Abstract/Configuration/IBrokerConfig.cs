using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Message.Kafka.Abstract.Configuration
{
    /// <summary>
    /// 服务器配置
    /// </summary>
    public interface IBrokerConfig
    {
        /// <summary>
        /// 名字
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 地址，多个地址用逗号分隔
        /// </summary>
        string Uri { get; }
        /// <summary>
        /// 主题配置
        /// </summary>
        IEnumerable<ITopicConfig> Topics { get; }
    }
}
