using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Topshelf;
using Services.Abstract;
using Services.Log;
using Services.Models;

namespace Services
{
    public class TaskService : ServiceControl
    {
        /// <summary>
        /// 启动的服务集合
        /// </summary>
        List<ServiceBase> _startServices = new List<ServiceBase>();
        public bool Start(HostControl hostControl)
        {
            LogFactory.GetLogger().Info("启动服务");
            #region 配置文件
            try
            {
                var serviceConfigList = GetServerList();
                foreach (var item in serviceConfigList)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(item.Assembly) || string.IsNullOrWhiteSpace(item.Methods))
                        {
                            continue;
                        }
                        var serviceDll = Assembly.LoadFrom(AppDomain.CurrentDomain.BaseDirectory + "/" + item.Assembly);
                        System.Type t = serviceDll.GetType(item.Methods);//获得类型
                        object o = System.Activator.CreateInstance(t);//创建实例
                        if (o is ServiceBase)
                        {
                            ServiceBase service = o as ServiceBase;
                            _startServices.Add(service);
                            service.Config = item;
                            service.ServiceStart();
                        }
                    }
                    catch (Exception ex)
                    {
                        LogFactory.GetLogger().Error(string.Format("TaskService-OnStart-ServiceStart 异常:{0}", ex));
                    }
                }



            }
            catch (Exception ex)
            {
                LogFactory.GetLogger().Error(string.Format("TaskService-Start 异常:{0}", ex));
            }
            return true;
            #endregion
        }

        public bool Stop(HostControl hostControl)
        {
            try
            {
                LogFactory.GetLogger().Info(string.Format("开始停止服务TaskService-OnStop"));
                //LogHelper.Info("停止服务");
                var count = _startServices.Where(s => s != null).Count();
                while (count > 0)
                {
                    for (var i = 0; i < _startServices.Count; i++)
                    {
                        try
                        {
                            if (_startServices[i] != null && _startServices[i].ServiceStop())
                            {
                                _startServices[i] = null;
                            }
                        }
                        catch (Exception ex)
                        {
                            LogFactory.GetLogger().Error(string.Format("TaskService-OnStop-ServiceStop 异常:{0}", ex));
                        }
                    }
                    count = _startServices.Where(s => s != null).Count();
                    if (count > 0)
                    {
                        Thread.Sleep(10 * 1000);
                    }
                }
                GC.Collect();
                LogFactory.GetLogger().Info(string.Format("完成停止服务TaskService-OnStop"));
            }
            catch (Exception ex)
            {
                LogFactory.GetLogger().Error(string.Format("TaskService-OnStop 异常:{0}", ex));
            }
            return true;
        }


        /// <summary>
        /// 获取服务列表
        /// </summary>
        /// <returns></returns>
        IList<ServiceConfigModel> GetServerList()
        {
            return File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"/Config/ServicesConfig.json", Encoding.Default).ToList<ServiceConfigModel>();
        }
    }

}
