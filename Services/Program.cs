using Services.Common;
using System;
using Topshelf;

namespace Services
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG

            new TaskService().Start(null);
            Console.Read();
            return;
#endif

            HostFactory.Run(x =>
            {
                x.Service<TaskService>();
                x.SetDescription(ConfigHelper.GetConfigValue("Description", "后台服务"));
                x.SetDisplayName(ConfigHelper.GetConfigValue("DisplayName", "后台服务"));
                x.SetServiceName(ConfigHelper.GetConfigValue("ServiceName", "BackService"));
            });
        }
    }
}
