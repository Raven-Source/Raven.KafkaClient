using Raven.Message.Kafka.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Raven.Message.Kafka.Client.Init();
                var client = Raven.Message.Kafka.Client.GetInstance("localhost");

                var task = client.Producer.ProduceAsync("test1", "hello world");
                Task.WaitAll(task);
                Console.WriteLine("send complete");
                Console.ReadLine();
                client.Dispose();
                Console.WriteLine("disposed");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.ReadLine();
        }
    }

    public class ConsoleLog : ILog
    {
        public void Debug(string format, params object[] pars)
        {
            string log = string.Format(format, pars);
            Console.WriteLine("debug:" + log);
        }

        public void Error(Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        public void Error(string format, params object[] pars)
        {
            string log = string.Format(format, pars);
            Console.WriteLine("error:" + log);
        }

        public void Info(string format, params object[] pars)
        {
            string log = string.Format(format, pars);
            Console.WriteLine("info:" + log);
        }
    }
}
