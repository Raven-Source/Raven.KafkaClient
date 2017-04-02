using Raven.Message.Kafka.Abstract.Configuration;
using Raven.Serializer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Message.Kafka.Impl.Configuration.App
{
    public class BrokerConfig : ConfigurationElement, IBrokerConfig
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
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

        List<TopicConfig> _topics = null;
        bool _topicsInited = false;
        public IEnumerable<ITopicConfig> Topics
        {
            get
            {
                if (_topicsInited)
                    return _topics;
                lock (this)
                {
                    if (_topicsInited)
                        return _topics;
                    if (_Topics != null)
                    {
                        _topics = new List<TopicConfig>();
                        foreach (TopicConfig topic in _Topics)
                        {
                            _topics.Add(topic);
                        }
                    }
                    _topicsInited = true;
                }
                return _topics;
            }
        }

        [ConfigurationProperty("topics")]
        [ConfigurationCollection(typeof(TopicConfigCollection), AddItemName = "topic")]
        public TopicConfigCollection _Topics
        {
            get
            {
                return (TopicConfigCollection)this["topics"];
            }
            set
            {
                this["topics"] = value;
            }
        }


        [ConfigurationProperty("uri")]
        public string Uri
        {
            get
            {
                return (string)this["uri"];
            }
            set
            {
                this["uri"] = value;
            }
        }
    }

    public class BrokerConfigCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new BrokerConfig();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((BrokerConfig)element).Name;
        }
    }
}
