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
    public class Client : IDisposable
    {
        static Dictionary<string, Client> _instanceDict = new Dictionary<string, Client>();//客户端对象字典
        static bool _inited = false;//是否已初始化过
        static bool _released = false;//是否已释放过
        /// <summary>
        /// 获取客户端配置
        /// </summary>
        public static IClientConfig Config { get; private set; }
        /// <summary>
        /// 初始化，配置工厂默认使用<see cref="Impl.Configuration.App.ConfigFactory"/>
        /// </summary>
        public static void Init()
        {
            Impl.Configuration.App.ConfigFactory factory = new Impl.Configuration.App.ConfigFactory();
            Init(factory);
        }
        /// <summary>
        /// 初始化，使用自定义配置工厂
        /// </summary>
        /// <param name="configFactory">配置工厂</param>
        public static void Init(IConfigFactory configFactory)
        {
            if (configFactory == null)
                throw new ArgumentNullException(nameof(configFactory));
            IClientConfig clientConfig = configFactory.CreateConfig();
            Init(clientConfig);
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="config">客户端配置</param>
        public static void Init(IClientConfig config)
        {
            if (_inited)
                return;
            lock (typeof(Client))
            {
                if (_inited)
                    return;
                if (config == null)
                    throw new ArgumentNullException(nameof(config));
                Config = config;
                try
                {
                    InitLog(config);
                    LogHelpler.Info("init config");
                    InitSerializer(config);
                    InitClient(config);
                    _inited = true;
                }
                catch (Exception ex)
                {
                    LogHelpler.Error(ex);
                    throw;
                }
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
                LogHelpler.Info("begin release");
                foreach (var client in _instanceDict.Values)
                {
                    try
                    {
                        client.Dispose();
                    }
                    catch (Exception ex)
                    {
                        LogHelpler.Error(ex);
                    }
                }
                LogHelpler.Info("release complete");
                _released = true;
            }
        }
        /// <summary>
        /// 初始化日志
        /// </summary>
        /// <param name="config"></param>
        static void InitLog(IClientConfig config)
        {
            if (string.IsNullOrEmpty(config.LogType))
                return;
            Type logType = Type.GetType(config.LogType);
            Type iLogType = typeof(ILog);
            if (!logType.IsAssignableFrom(iLogType))
                throw new ArgumentException($"{logType} is not assignable from {iLogType}");
            ILog log = Activator.CreateInstance(logType) as ILog;
            LogHelpler.SetLog(log);
        }
        /// <summary>
        /// 初始化序列化参数
        /// </summary>
        /// <param name="config"></param>
        static void InitSerializer(IClientConfig config)
        {
            Serialization.SerializerContainer.DefaultSerializerType = config.SerializerType;
        }
        /// <summary>
        /// 初始化客户端实例
        /// </summary>
        /// <param name="config"></param>
        static void InitClient(IClientConfig config)
        {
            if (config.Brokers == null)
                throw new ArgumentNullException(nameof(config.Brokers));
            foreach (var brokerConfig in config.Brokers)
            {
                Client client = new Client(brokerConfig);
                _instanceDict.Add(brokerConfig.Name, client);
            }
        }
        /// <summary>
        /// 获取客户端实例
        /// </summary>
        /// <param name="brokerName">服务器名</param>
        /// <returns></returns>
        public static Client GetInstance(string brokerName)
        {
            if (string.IsNullOrEmpty(brokerName))
                return null;
            if (_instanceDict.ContainsKey(brokerName))
                return _instanceDict[brokerName];
            return null;
        }
        /// <summary>
        /// 服务器配置
        /// </summary>
        public IBrokerConfig BrokerConfig { get; private set; }
        /// <summary>
        /// 获取生产者实例
        /// </summary>
        public Producer Producer { get; private set; }
        /// <summary>
        /// 关闭客户端，释放资源
        /// </summary>
        public void Dispose()
        {
            Producer.Dispose();
        }

        Client(IBrokerConfig brokerConfig)
        {
            if (brokerConfig == null)
                throw new ArgumentNullException(nameof(brokerConfig));
            LogHelpler.Info("create client for broker {0}, {1}", brokerConfig.Name, brokerConfig.Uri);
            BrokerConfig = brokerConfig;
            Producer = new Producer(brokerConfig);
        }
    }
}
