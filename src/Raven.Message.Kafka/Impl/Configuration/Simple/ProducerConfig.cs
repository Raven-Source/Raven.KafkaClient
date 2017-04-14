using Raven.Message.Kafka.Abstract.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Message.Kafka.Impl.Configuration.Simple
{
    public class ProducerConfig : IProducerConfig
    {
        public int? acks
        {
            get;set;
        }

        public string debug
        {
            get;set;
        }

        public int? metadata_max_age_ms
        {
            get;set;
        }

        public int? topic_metadata_refresh_inverval_ms
        {
            get;set;
        }
    }
}
