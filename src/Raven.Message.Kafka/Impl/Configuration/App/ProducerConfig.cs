using Raven.Message.Kafka.Abstract.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Message.Kafka.Impl.Configuration.App
{
    public class ProducerConfig : ConfigurationElement, IProducerConfig
    {
        [ConfigurationProperty(name: "acks")]
        public int? acks
        {
            get
            {
                return (int?)this["acks"];
            }
            set
            {
                this["acks"] = value;
            }
        }
        [ConfigurationProperty(name: "debug")]
        public string debug
        {
            get
            {
                return (string)this["debug"];
            }
            set
            {
                this["debug"] = value;
            }
        }

        [ConfigurationProperty(name: "metadata_max_age_ms")]
        public int? metadata_max_age_ms
        {
            get
            {
                return (int?)this["metadata_max_age_ms"];
            }
            set
            {
                this["metadata_max_age_ms"] = value;
            }
        }

        [ConfigurationProperty(name: "topic_metadata_refresh_inverval_ms")]
        public int? topic_metadata_refresh_inverval_ms
        {
            get
            {
                return (int?)this["topic_metadata_refresh_inverval_ms"];
            }
            set
            {
                this["topic_metadata_refresh_inverval_ms"] = value;
            }
        }
    }
}
