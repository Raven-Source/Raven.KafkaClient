using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Message.Kafka.Abstract
{
    /// <summary>
    /// 日志接口
    /// </summary>
    /// <remarks>
    /// rdkafka中日志等级   本日志接口方法
    /// LOG_EMERG   0       Error
    /// LOG_ALERT   1       Error
    /// LOG_CRIT    2       Error
    /// LOG_ERR     3       Error
    /// LOG_WARNING 4       Info
    /// LOG_NOTICE  5       Info
    /// LOG_INFO    6       Info
    /// LOG_DEBUG   7       Debug
    /// </remarks>
    public interface ILog
    {
        /// <summary>
        /// 调试日志
        /// </summary>
        /// <param name="format"></param>
        /// <param name="pars"></param>
        void Debug(string format, params object[] pars);
        /// <summary>
        /// 信息日志
        /// </summary>
        /// <param name="format"></param>
        /// <param name="pars"></param>
        void Info(string format, params object[] pars);
        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="format"></param>
        /// <param name="pars"></param>
        void Error(string format, params object[] pars);
        /// <summary>
        /// 异常日志
        /// </summary>
        /// <param name="ex"></param>
        void Error(Exception ex);
    }
}
