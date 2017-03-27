using Raven.Message.Kafka.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Message.Kafka.Abstract.Configuration;
using System.IO;
using System.Configuration;

namespace Raven.Message.Kafka.Impl.Configuration.App
{
    class ConfigFactory : IConfigFactory
    {
        IClientConfig _config;
        string _sectionName;
        string _file;
        public ConfigFactory(string sectionName = "ravenKafka")
        {
            _sectionName = sectionName;
        }

        public ConfigFactory(string file, string sectionName)
        {
            _file = file;
            _sectionName = sectionName;
        }
        
        public IClientConfig CreateConfig()
        {
            if (_config == null)
                _config = LoadFrom(_file, _sectionName);
            return _config;
        }

        IClientConfig LoadFrom(string file, string section)
        {
            if (!string.IsNullOrEmpty(file) && !File.Exists(file))
                throw new FileNotFoundException("raven kafka config file not found", file);

            if (!string.IsNullOrEmpty(file))
            {
                var fileMap = new ExeConfigurationFileMap();
                fileMap.ExeConfigFilename = file;
                var config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                return config.GetSection(section) as ClientConfig;
            }
            else
            {
                return ConfigurationManager.GetSection(section) as ClientConfig;
            }
        }
    }
}
