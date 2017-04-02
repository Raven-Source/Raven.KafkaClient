using Raven.Message.Kafka.Abstract;
using Raven.Message.Kafka.Abstract.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Message.Kafka
{
    /// <summary>
    /// 客户端
    /// </summary>
    public class Client
    {
        static Dictionary<string, Connection> _connections = new Dictionary<string, Connection>();//连接字典
        static bool _released = false;//是否已释放过
        /// <summary>
        /// 获取客户端配置
        /// </summary>
        public static IClientConfig Config { get; private set; }
        /// <summary>
        /// 加载配置，配置工厂默认使用<see cref="Impl.Configuration.App.ConfigFactory"/>
        /// </summary>
        public static void LoadConfig()
        {
            Impl.Configuration.App.ConfigFactory factory = new Impl.Configuration.App.ConfigFactory();
            LoadConfig(factory);
        }
        /// <summary>
        /// 加载配置，使用自定义配置工厂
        /// </summary>
        /// <param name="configFactory">配置工厂</param>
        public static void LoadConfig(IConfigFactory configFactory)
        {
            if (configFactory == null)
                throw new ArgumentNullException(nameof(configFactory));
            IClientConfig clientConfig = configFactory.CreateConfig();
            LoadConfig(clientConfig);
        }
        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="config">客户端配置</param>
        public static void LoadConfig(IClientConfig config)
        {
            lock (typeof(Client))
            {
                if (config == null)
                    throw new ArgumentNullException(nameof(config));
                Config = config;
                var log = InitLog(config);
                InitConnections(config, log);
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public static void Release()
        {
            if (_released)
                return;
            lock (typeof(Client))
            {
                if (_released)
                    return;
                foreach (var connection in _connections.Values)
                {
                    connection.Dispose();
                }
                _released = true;
            }
        }
        /// <summary>
        /// 初始化日志
        /// </summary>
        /// <param name="config"></param>
        static ILog InitLog(IClientConfig config)
        {
            if (string.IsNullOrEmpty(config.LogType))
                return null;
            Type logType = Type.GetType(config.LogType);
            Type iLogType = typeof(ILog);
            if (!iLogType.IsAssignableFrom(logType))
                throw new ArgumentException($"{logType} is not assignable to {iLogType}");
            ILog log = Activator.CreateInstance(logType) as ILog;
            return log;
        }
        /// <summary>
        /// 初始化连接
        /// </summary>
        /// <param name="config"></param>
        static void InitConnections(IClientConfig config, ILog log)
        {
            if (config.Brokers == null)
                throw new ArgumentNullException(nameof(config.Brokers));
            foreach (var brokerConfig in config.Brokers)
            {
                if (_connections.ContainsKey(brokerConfig.Name))
                {
                    //todo log;
                    try
                    {
                        log?.Info("broker config {0} already exist, skip init connection", brokerConfig.Name);
                    }
                    catch { }
                    continue;
                }
                Connection connection = new Connection(brokerConfig, log);
                _connections.Add(brokerConfig.Name, connection);
            }
        }
        /// <summary>
        /// 获取连接
        /// </summary>
        /// <param name="brokerName">服务器名</param>
        /// <returns></returns>
        public static Connection GetConnection(string brokerName)
        {
            if (string.IsNullOrEmpty(brokerName))
                return null;
            if (_connections.ContainsKey(brokerName))
                return _connections[brokerName];
            return null;
        }
    }
}
