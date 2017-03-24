using Raven.Message.Kafka.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Message.Kafka.Abstract
{
    /// <summary>
    /// 配置工厂
    /// </summary>
    public interface IConfigFactory
    {
        /// <summary>
        /// 创建配置
        /// </summary>
        /// <returns></returns>
        ClientConfig CreateConfig();
    }
}
