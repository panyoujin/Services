using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services;

namespace Services.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            TaskService t = new TaskService();
            while (true)
            {
                Console.Write("请输入操作序号 1启动 0停止 2退出");
                var s = Console.ReadLine();
                switch (s)
                {
                    case "1":
                        t.Start(null);
                        break;
                    case "0":
                        t.Stop(null);
                        break;
                    case "2":
                        return;
                        break;
                }
            }
        }
    }
}
