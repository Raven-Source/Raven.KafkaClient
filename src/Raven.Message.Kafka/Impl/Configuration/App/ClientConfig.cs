using Raven.Message.Kafka.Abstract.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Serializer;
using System.Configuration;

namespace Raven.Message.Kafka.Impl.Configuration.App
{
    public class ClientConfig : ConfigurationSection, IClientConfig
    {
        List<BrokerConfig> _brokers;
        bool _brokersInited = false;
        public IEnumerable<IBrokerConfig> Brokers
        {
            get
            {
                if (_brokersInited)
                    return _brokers;
                lock (this)
                {
                    if (_brokersInited)
                        return _brokers;
                    if (_Brokers != null)
                    {
                        _brokers = new List<BrokerConfig>();
                        foreach (BrokerConfig broker in _Brokers)
                        {
                            _brokers.Add(broker);
                        }
                    }
                    _brokersInited = true;
                }
                return _brokers;
            }
        }

        [ConfigurationProperty("brokers", IsRequired = true)]
        [ConfigurationCollection(typeof(BrokerConfig), AddItemName = "broker")]
        public BrokerConfigCollection _Brokers
        {
            get
            {
                return (BrokerConfigCollection)this["brokers"];
            }
            set
            {
                this["brokers"] = value;
            }
        }

        [ConfigurationProperty(name: "logType", IsRequired = true)]
        public string LogType
        {
            get
            {
                return (string)this["logType"];
            }
            set
            {
                this["logType"] = value;
            }
        }

        [ConfigurationProperty("serializerType")]
        public SerializerType SerializerType
        {
            get
            {
                return (SerializerType)this["serializerType"];
            }
            set
            {
                this["serializerType"] = value;
            }
        }
    }
}
