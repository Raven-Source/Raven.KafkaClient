﻿using Raven.Message.Kafka.Abstract.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Serializer;

namespace Raven.Message.Kafka.Impl.Configuration.Simple
{
    public class TopicConfig : ITopicConfig
    {
        public string Name
        {
            get;set;
        }

        public IProducerConfig ProducerConfig
        {
            get;set;
        }

        public SerializerType? SerializerType
        {
            get;set;
        }
    }
}
