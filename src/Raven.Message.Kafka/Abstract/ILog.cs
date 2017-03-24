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
