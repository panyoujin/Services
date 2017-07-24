using DataAccessHelper.SQLHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                //x.UseLog4Net("log4net.config");
                x.Service<TaskService>();
                x.SetDescription("后台服务");
                x.SetDisplayName("后台服务");
                x.SetServiceName("BackServer");
            });
        }
    }
}
