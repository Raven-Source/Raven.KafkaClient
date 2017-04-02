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
        ILog _log;

        internal LogHelpler(ILog log)
        {
            _log = log;
        }

        internal void Debug(string format, params object[] pars)
        {
            try
            {
                _log?.Debug(format, pars);
            }
            catch { }
        }

        internal void Error(Exception ex)
        {
            try
            {
                _log?.Error(ex);
            }
            catch { }
        }

        internal void Error(string format, params object[] pars)
        {
            try
            {
                _log?.Error(format, pars);
            }
            catch { }
        }

        internal void Info(string format, params object[] pars)
        {
            try
            {
                _log?.Info(format, pars);
            }
            catch { }
        }
    }
}
