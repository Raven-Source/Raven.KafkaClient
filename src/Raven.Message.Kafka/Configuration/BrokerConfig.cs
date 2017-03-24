using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Message.Kafka.Configuration
{
    /// <summary>
    /// 服务器配置
    /// </summary>
    public class BrokerConfig
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 地址，多个地址用逗号分隔
        /// </summary>
        public string Uri { get; set; }
        /// <summary>
        /// 主题配置
        /// </summary>
        public IEnumerable<TopicConfig> Topics { get; set; }

        public override string ToString()
        {
            return $"Name:{Name},Uri:{Uri}";
        }
    }
}
