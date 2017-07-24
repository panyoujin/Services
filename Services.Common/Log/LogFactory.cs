
using log4net;
using System;
using System.IO;

namespace Services.Common
{
    public class LogFactory
    {
        static LogFactory()
        {
            FileInfo configFile = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "Config/log4net.config");
            log4net.Config.XmlConfigurator.Configure(configFile);
        }
        public static Log GetLogger(Type type)
        {
            return new Log(LogManager.GetLogger(type));
        }
        public static Log GetLogger(string str= "LogFile")
        {
            return new Log(LogManager.GetLogger(str));
        }
    }
}
