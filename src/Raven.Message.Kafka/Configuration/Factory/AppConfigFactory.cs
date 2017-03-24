using Raven.Message.Kafka.Abstract;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Message.Kafka.Configuration
{
    /// <summary>
    /// 应用程序配置工厂
    /// </summary>
    public class AppConfigFactory : IConfigFactory
    {
        string _sectionName;
        string _file;
        public AppConfigFactory(string sectionName = "ravenKafka")
        {
            _sectionName = sectionName;
        }

        public AppConfigFactory(string file, string sectionName)
        {
            _file = file;
            _sectionName = sectionName;
        }

        public ClientConfig CreateConfig()
        {
            throw new NotImplementedException();
        }

        public static ClientConfig LoadFrom(string file, string section)
        {
            throw new NotImplementedException();
            if (!File.Exists(file))
                return null;
            var fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = file;
            var config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            //var configSource = config.GetSection(section) as ClientConfig;
            //return configSource;
        }
    }
}
