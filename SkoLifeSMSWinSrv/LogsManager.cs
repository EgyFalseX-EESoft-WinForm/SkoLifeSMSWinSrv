using log4net;
using System;

namespace SkoLifeSMSWinSrv
{
    public class LogsManager
    {
        public static LogsManager DefaultInstance = new LogsManager();

        public enum LogType
        {
            Error,
            Success,
            Log,
            Debug,
        }
        private ConsoleColor _consoleColorError = ConsoleColor.Red;
        private ConsoleColor _consoleColorSuccess = ConsoleColor.Green;
        private ConsoleColor _consoleColorLog = ConsoleColor.DarkGray;

        public LogsManager()
        {
            System.IO.FileInfo appPath = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo($"{appPath.Directory}\\log4netConfig.xml"));
            // Set up a simple configuration that logs on the console.
            //log4net.Config.BasicConfigurator.Configure();
        }

        public void LogMsg(LogType type, string message, Type l4NClassType)
        {
            ILog l4N = LogManager.GetLogger(l4NClassType);
            switch (type)
            {
                case LogType.Error:
                    Console.ForegroundColor = _consoleColorError;
                    l4N.Error(message);
                    break;
                case LogType.Success:
                    Console.ForegroundColor = _consoleColorSuccess;
                    l4N.Info(message);
                    break;
                case LogType.Log:
                    Console.ForegroundColor = _consoleColorLog;
                    l4N.Info(message);
                    break;
                case LogType.Debug:
                    l4N.Debug(message);
                    break;
            }
            if (type != LogType.Debug)
                Console.WriteLine($@"[{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day} {DateTime.Now.Hour}:{DateTime.Now.Minute}] {message}");
            
            Console.ResetColor();
        }


    }
}
