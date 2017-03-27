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
    public class TopicConfig : ConfigurationElement, ITopicConfig
    {
        [ConfigurationProperty(name: "name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }

            set
            {
                this["name"] = value;
            }
        }

        public IProducerConfig ProducerConfig
        {
            get
            {
                return _ProducerConfig;
            }
            set
            {
                _ProducerConfig = value as ProducerConfig;
            }
        }

        [ConfigurationProperty(name: "producer")]
        public ProducerConfig _ProducerConfig
        {
            get
            {
                return (ProducerConfig)this["producer"];
            }

            set
            {
                this["producer"] = value;
            }
        }

        [ConfigurationProperty("serializerType")]
        public SerializerType? SerializerType
        {
            get
            {
                return (SerializerType?)this["serializerType"];
            }
            set
            {
                this["serializerType"] = value;
            }
        }
    }

    public class TopicConfigCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TopicConfig();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TopicConfig)element).Name;
        }
    }
}
