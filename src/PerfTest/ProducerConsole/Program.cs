using Raven.Message.Kafka;
using Raven.Message.Kafka.Abstract;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerConsole
{
    class Program
    {
        static Connection _connection = null;
        static long _sendCount = 0;
        static long _sendFailed = 0;
        static long _sendElapsedMilliseconds = 0;

        static long _lastSend = 0;
        static long _lastSendFailed = 0;
        static long _lastSendElapsedMilliseconds = 0;

        static string _action = ConfigurationManager.AppSettings["action"];
        static int _cpu = int.Parse(ConfigurationManager.AppSettings["cpu"]);
        static int _batchCount = int.Parse(ConfigurationManager.AppSettings["batchCount"]);
        static int _sleepTime = int.Parse(ConfigurationManager.AppSettings["sleepTime"]);
        static string _message = ConfigurationManager.AppSettings["message"];
        static string _topic = ConfigurationManager.AppSettings["topic"];
        static object _messageObj = null;

        static void Main(string[] args)
        {
            Client.LoadConfig();
            try
            {
                _messageObj = Newtonsoft.Json.JsonConvert.DeserializeObject(_message);
            }
            catch { }
            _connection = Client.GetConnection("perftest");

            List<Task> _tasks = new List<Task>();
            for (int i = 0; i < _cpu * 1; i++)
            {
                _tasks.Add(Task.Factory.StartNew(Run, TaskCreationOptions.LongRunning));
            }
            Console.WriteLine("all thread started");

            while (true)
            {
                Thread.Sleep(10000);
                PrintStats();
            }
        }

        static async void Run()
        {
            Stopwatch stopwatch = new Stopwatch();
            while (true)
            {
                int count = _batchCount;
                int failed = 0;
                stopwatch.Start();
                for (int i = 0; i < count; i++)
                {
                    try
                    {
                        switch (_action)
                        {
                            case "Produce":
                                await ProduceTest();
                                break;
                            case "ProduceAndForget":
                                ProduceAndForgetTest();
                                break;
                            case "ProduceNoAck":
                                ProduceNoAckTest();
                                break;
                        }
                    }
                    catch
                    {
                        failed++;
                    }
                }
                stopwatch.Stop();
                Interlocked.Add(ref _sendCount, count);
                Interlocked.Add(ref _sendFailed, failed);
                Interlocked.Add(ref _sendElapsedMilliseconds, stopwatch.ElapsedMilliseconds);
                stopwatch.Reset();
                Thread.Sleep(_sleepTime);
            }
        }

        static Task ProduceTest()
        {
            if (_messageObj == null)
                return _connection.Producer.ProduceAsync("producetest", _message);
            else
                return _connection.Producer.ProduceAsync("producetest", _messageObj);
        }

        static void ProduceNoAckTest()
        {
            if (_messageObj == null)
                _connection.Producer.ProduceAsync(string.IsNullOrEmpty(_topic) ? "producenoacktest" : _topic, _message);
            else
                _connection.Producer.ProduceAsync(string.IsNullOrEmpty(_topic) ? "producenoacktest" : _topic, _messageObj);
        }

        static void ProduceAndForgetTest()
        {
            if (_messageObj == null)
                _connection.Producer.ProduceAndForget(string.IsNullOrEmpty(_topic) ? "produceandforgettest" : _topic, _message);
            else
                _connection.Producer.ProduceAndForget(string.IsNullOrEmpty(_topic) ? "produceandforgettest" : _topic, _messageObj);
        }

        static void PrintStats()
        {
            long sendCount = _sendCount;
            long sendFailed = _sendFailed;
            long sendElapsedMilliseconds = _sendElapsedMilliseconds;
            Console.WriteLine($"{DateTime.Now}, totalSend : {sendCount}, sendFailed : {sendFailed}, elapsedMilliseconds : {sendElapsedMilliseconds}");
            Console.WriteLine($"lastSend : {sendCount - _lastSend}, lastSendFailed : {sendFailed - _lastSendFailed}, lastElapsedMilliseconds : {sendElapsedMilliseconds - _lastSendElapsedMilliseconds}");
            _lastSend = sendCount;
            _lastSendFailed = sendFailed;
            _lastSendElapsedMilliseconds = sendElapsedMilliseconds;
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
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.ToString());
            Console.ForegroundColor = oldColor;
        }

        public void Error(string format, params object[] pars)
        {
            Interlocked.Increment(ref ErrorCount);
            string log = string.Format(format, pars);
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("error:" + log);
            Console.ForegroundColor = oldColor;
        }

        public void Info(string format, params object[] pars)
        {
        }
    }
}
