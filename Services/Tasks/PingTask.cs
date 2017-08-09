using Services.Common;

namespace Services.Tasks
{
    class PingTask : ServiceBase
    {
        protected override void Exec()
        {
            ExecCmd("ping baidu.com");
        }

        private void ExecCmd(string cmd)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
            p.StartInfo.CreateNoWindow = true;//不显示程序窗口
            p.Start();//启动程序
            //向cmd窗口发送输入信息
            p.StandardInput.WriteLine(cmd+"&exit");
            p.StandardInput.AutoFlush = true;
            //获取cmd窗口的输出信息
            string output = p.StandardOutput.ReadToEnd();

            p.WaitForExit();//等待程序执行完退出进程
            p.Close();
            LogFactory.GetLogger().Info(string.Format("执行命令：{0}; 执行结果：{1}", cmd, ""));
        }
    }
}
