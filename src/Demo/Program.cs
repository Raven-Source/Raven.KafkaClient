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
                Raven.Message.Kafka.Client.LoadConfig();
                var connection = Raven.Message.Kafka.Client.GetConnection("localhost");
                for (int i = 0; i < 100; i++)
                {
                    connection.Producer.ProduceAndForget("test222", i, "hi" + i);
                }
                Console.WriteLine("send complete");
                Console.ReadLine();
                connection.Dispose();
                Raven.Message.Kafka.Client.Release();
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
