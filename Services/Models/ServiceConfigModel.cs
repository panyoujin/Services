using System;

namespace Services.Models
{
    /// <summary>
    /// 服务配置模型
    /// </summary>
    /// jimmy.pan 2015-11-4 ADD
    public class ServiceConfigModel
    {
        public string ServiceName { get; set; }
        /// <summary>
        /// DLL的路径
        /// </summary>
        public string Assembly { get; set; }

        /// <summary>
        /// 服务的类路径
        /// </summary>
        public string Methods { get; set; }

        /// <summary>
        /// 执行类型 
        /// 0:按指定间隔时间执行
        /// 1:每天指定时间执行 每天一次
        /// 2:指定时间执行一次
        /// 3.每天指定开始和结束时间并且按照指定间隔时间执行
        /// </summary>
        public int ExecType { get; set; }

        /// <summary>
        /// 间隔时间 单位秒
        /// </summary>
        public int S_Interval { get; set; }

        /// <summary>
        /// 间隔时间 单位分
        /// </summary>
        public int M_Interval { get; set; }

        /// <summary>
        /// 间隔时间 单位小时
        /// </summary>
        public int H_Interval { get; set; }

        /// <summary>
        /// 每天指定时间执行 格式 HH:mm:ss 14:00:00
        /// </summary>
        public string ExecDayTime { get; set; }

        /// <summary>
        /// 指定时间执行一次 datetime yyyy-MM-dd HH:mm:ss 
        /// </summary>
        public DateTime? ExecTime { get; set; }


        /// <summary>
        /// 每天指定开始 格式 HH:mm:ss 14:00:00
        /// </summary>
        public string ExecDayStartTime { get; set; }


        /// <summary>
        /// 每天指定结束时间 格式 HH:mm:ss 14:00:00
        /// </summary>
        public string ExecDayEndTime { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public string Param { get; set; }
        /// <summary>
        /// 下一次执行间隔时间
        /// 如果返回-1 则意味着，下一次在很久很久之后
        /// 如果返回-2 则意味着，后面已经没有循环了，可以停止该次任务
        /// </summary>
        public int SleepInterval()
        {
            var interval = 0;
            DateTime dt = DateTime.Now;
            switch (ExecType)
            {
                case 0://0:按指定间隔时间执行
                    interval = this.H_Interval * 60 * 60 + this.M_Interval * 60 + this.S_Interval;
                    if (interval <= 0)
                    {
                        interval = 60;
                    }
                    break;
                case 1://1:每天指定时间执行 每天一次
                    dt = Convert.ToDateTime(DateTime.Now.ToString(string.Format("yyyy-MM-dd  {0}", this.ExecDayTime)));
                    if (DateTime.Now > dt)
                    {
                        dt = Convert.ToDateTime(DateTime.Now.AddDays(1).ToString(string.Format("yyyy-MM-dd  {0}", this.ExecDayTime)));
                    }
                    interval = (int)(dt - DateTime.Now).TotalSeconds;
                    break;
                case 2://2:指定时间执行一次
                    if (!this.ExecTime.HasValue)
                    {
                        throw new Exception("执行类型为【3:指定时间执行一次】时 ExecTime不能为空");
                    }
                    dt = this.ExecTime.Value;
                    if (DateTime.Now > dt)
                    {
                        //结束 -2
                        interval = -2;
                    }
                    if ((dt - DateTime.Now).Days > 365)
                    {
                        interval = -1;
                    }
                    else
                    {
                        interval = (int)(dt - DateTime.Now).TotalSeconds;
                    }
                    break;
                case 3://3.每天指定开始和结束时间并且按照指定间隔时间执行
                    dt = Convert.ToDateTime(DateTime.Now.ToString(string.Format("yyyy-MM-dd  {0}", this.ExecDayStartTime)));
                    if (DateTime.Now > dt)
                    {
                        dt = Convert.ToDateTime(DateTime.Now.ToString(string.Format("yyyy-MM-dd  {0}", this.ExecDayEndTime)));
                        //如果当前时间在开始和结束时间之间，并且下一次时间在结束之前
                        if (DateTime.Now < dt && ((dt - DateTime.Now).TotalSeconds) > this.H_Interval * 60 * 60 + this.M_Interval * 60 + this.S_Interval)
                        {
                            interval = this.H_Interval * 60 * 60 + this.M_Interval * 60 + this.S_Interval;
                            if (interval <= 0)
                            {
                                interval = 60;
                            }
                        }
                        else
                        {
                            dt = Convert.ToDateTime(DateTime.Now.AddDays(1).ToString(string.Format("yyyy-MM-dd  {0}", this.ExecDayStartTime)));
                        }
                    }
                    if (interval <= 0)
                    {
                        interval = (int)(dt - DateTime.Now).TotalSeconds;
                    }
                    break;
            }
            return interval;
        }
        /// <summary>
        /// 启动是否马上执行 如果不是马上执行 就睡眠线程
        /// </summary>
        /// <returns>true 马上执行  false睡眠线程</returns>
        public bool OneExec()
        {
            switch (ExecType)
            {
                case 0:
                    return true;
                case 3://3.每天指定开始和结束时间并且按照指定间隔时间执行
                    DateTime dt = Convert.ToDateTime(DateTime.Now.ToString(string.Format("yyyy-MM-dd  {0}", this.ExecDayStartTime)));
                    if (DateTime.Now > dt)
                    {
                        dt = Convert.ToDateTime(DateTime.Now.ToString(string.Format("yyyy-MM-dd  {0}", this.ExecDayEndTime)));
                        //如果当前时间在开始和结束时间之间，并且下一次时间在结束之前
                        if (DateTime.Now < dt)
                        {
                            return true;
                        }
                    }
                    return false;
                default:
                    //首次执行需要等到该执行时才执行
                    return false;
            }
        }
    }
}
