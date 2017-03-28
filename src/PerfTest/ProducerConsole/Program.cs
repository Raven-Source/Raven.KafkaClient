using Raven.Message.Kafka.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Raven.Message.Kafka.Client.Init();
            var client = Raven.Message.Kafka.Client.GetInstance("localhost");



        }
    }

    public class ConsoleLog : ILog
    {
        static int ErrorCount;

        public void Debug(string format, params object[] pars)
        {
        }

        public void Error(Exception ex)
        {
            Interlocked.Increment(ref ErrorCount);
            Console.WriteLine(ex.ToString());
        }

        public void Error(string format, params object[] pars)
        {
            Interlocked.Increment(ref ErrorCount);
            string log = string.Format(format, pars);
            Console.WriteLine("error:" + log);
        }

        public void Info(string format, params object[] pars)
        {
        }
    }
}
