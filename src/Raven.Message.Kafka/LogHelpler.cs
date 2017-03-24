using Raven.Message.Kafka.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Message.Kafka
{
    internal class LogHelpler
    {
        static ILog _log;

        internal static void SetLog(ILog log)
        {
            _log = log;
        }

        internal static void Debug(string format, params object[] pars)
        {
            try
            {
                _log?.Debug(format, pars);
            }
            catch { }
        }

        internal static void Error(Exception ex)
        {
            try
            {
                _log?.Error(ex);
            }
            catch { }
        }

        internal static void Error(string format, params object[] pars)
        {
            try
            {
                _log?.Error(format, pars);
            }
            catch { }
        }

        internal static void Info(string format, params object[] pars)
        {
            try
            {
                _log?.Info(format, pars);
            }
            catch { }
        }
    }
}
