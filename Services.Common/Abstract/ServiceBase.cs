using System;
using System.Collections.Generic;
using System.Threading;

namespace Services.Common
{
    /// <summary>
    /// 服务基类
    /// </summary>
    /// jimmy.pan ADD 2017/03/24
    public abstract class ServiceBase
    {
        public bool _isStart = false;
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// 服务线程
        /// </summary>
        protected List<Thread> _threadList = new List<Thread>();

        public ServiceConfigModel Config { get; set; }
        /// <summary>
        /// 启动服务
        /// </summary>
        public void ServiceStart()
        {
            _isStart = true;
            var t = new Thread(s =>
            {
                this.OneExec();
                while (_isStart)
                {
                    try
                    {
                        LogFactory.GetLogger().Info(string.Format("<!--==========执行 {0} 服务-开始==========-->\n", Config.ServiceName));
                        Exec();
                        LogFactory.GetLogger().Info(string.Format("<!--==========执行 {0} 服务-结束==========-->\n", Config.ServiceName));
                    }
                    catch (Exception ex)
                    {
                        LogFactory.GetLogger().Error(string.Format("{0}-OnStart-ServiceStart 异常:{1}", Config.ServiceName, ex));
                    }
                    this.SleepThread();
                }
                LogFactory.GetLogger().Info(string.Format("服务 {0} 停止执行，线程释放。", Config.ServiceName));

            });
            t.Name = Config.ServiceName;
            _threadList.Add(t);
            t.Start();
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        protected abstract void Exec();

        /// <summary>
        /// 停止服务
        /// </summary>
        public virtual bool ServiceStop()
        {
            var isScuss = true;
            try
            {
                LogFactory.GetLogger().Info(string.Format("ServiceBase-ServiceStop"));
                _isStart = false;
                if (_threadList != null && _threadList.Count > 0)
                {
                    foreach (var t in _threadList)
                    {
                        try
                        {
                            LogFactory.GetLogger().Info(string.Format("Name:{0},ThreadState:{1}", t.Name, t.ThreadState));
                            if (t.ThreadState != ThreadState.Stopped & t.ThreadState != ThreadState.StopRequested && t.ThreadState != ThreadState.Aborted && t.ThreadState != ThreadState.AbortRequested)
                            {
                                isScuss = false;
                                return isScuss;
                            }
                        }
                        catch (Exception ex)
                        {
                            LogFactory.GetLogger().Error(string.Format("Name:{0},ThreadState:{1} 异常:{2}", t.Name, t.ThreadState, ex));
                        }
                    }
                    //_threadList.Clear();
                }
            }
            catch (Exception ex)
            {
                LogFactory.GetLogger().Error(string.Format("ServiceBase-ServiceStop 异常:{0}", ex));
            }

            return isScuss;
        }

        /// <summary>
        /// 线程睡眠
        /// </summary>
        protected void SleepThread()
        {
            var interval = Config.SleepInterval();

            LogFactory.GetLogger().Info(string.Format("服务:{0}进入休眠区,休眠时间:{1}秒", Config.ServiceName,interval));
            while (_isStart && interval == -1)
            {
                Thread.Sleep(10 * 1000);
                interval = Config.SleepInterval();
            }
            if (interval == -2)
            {
                _isStart = false;
            }
            while (_isStart && interval > 0)
            {
                //分成多次等待的目的是在停止的时候能尽快停止并且不报错
                Thread.Sleep((interval > 10 ? 10 : interval) * 1000);
                interval -= 10;
            }
        }

        /// <summary>
        /// 启动是否马上执行 如果不是马上执行 就睡眠线程
        /// </summary>
        protected void OneExec()
        {
            if (!Config.OneExec())
            {
                SleepThread();
            }
        }
    }
}
