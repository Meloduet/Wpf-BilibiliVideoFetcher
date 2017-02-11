using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BilibiliVideoFetcher.Data
{
    class Log
    {
        private string _logFile = Environment.CurrentDirectory + @"\log.txt";
        private static bool _enable = true;
        private static Log _logger = new Log();
        private Log()
        {
            if (!File.Exists(_logFile))
            {
                File.WriteAllText(_logFile,
                    "这是日志文件, 可以随意删除, 或等待文件过长后自动删除, " +
                    "如果你在使用软件的过程中遇到问题, 可以把文件发送给开发者以便及时修复.\n");

            }
            if (new FileInfo(_logFile).Length > 10000000)
            {
                File.Delete(_logFile);
            }
        }
        public static Log GetLogger()
        {
            return _logger;
        }
        public void Write(string message)
        {
            if (!_enable) return;
            var ioe = false;
            do
            {
                try
                {
                    StreamWriter log = new StreamWriter(_logFile, true);
                    log.WriteLine(message);
                    log.Close();
                    ioe = false;
                }
                catch (IOException)
                {
                    ioe = true;
                    Thread.Sleep(100);
                }

            }
            while (ioe);


        }
        public void Info(string tag, string message)
        {
            Write(DateTime.Now.ToLongTimeString() + "[INFO]" + tag + ": " + message);
        }
        public void Warning(string tag, string message)
        {
            Write(DateTime.Now.ToLongTimeString() + "[WARN]" + tag + ": " + message);
        }
        public void Debug(string tag, string message)
        {
            Write(DateTime.Now.ToLongTimeString() + "[DEBG]" + tag + ": " + message);
        }
    }
}
