using Raven.Message.Kafka.Abstract.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Message.Kafka.Impl.Configuration.Simple
{
    public class ClientConfig : IClientConfig
    {
        public IEnumerable<IBrokerConfig> Brokers
        {
            get;set;
        }

        public string LogType
        {
            get;set;
        }
    }
}
