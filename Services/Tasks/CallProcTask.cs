using DataAccessHelper.SQLHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Services.Abstract;
using Services.Log;

namespace Services.Tasks
{
    public class CallProcTask : ServiceBase
    {
        protected override void Exec()
        {
            try
            {
                if (_isStart)
                {
                    if (string.IsNullOrWhiteSpace(Config.Param))
                    {
                        Call();
                    }
                    else
                    {
                        LogFactory.GetLogger().Info(string.Format("开始执行存储过程 {0}", Config.Param));
                        SQLHelperFactory.Instance.ExecuteNonQuery(Config.Param, null);
                        LogFactory.GetLogger().Info(string.Format("执行存储过程 {0} 完成", Config.Param));
                    }
                }
            }
            catch (Exception ex)
            {
                LogFactory.GetLogger().Error(string.Format("执行存储过程 {0} 异常:{1}", Config.Param, ex));
            }
        }


        void Call()
        {

            try
            {
                if (_isStart)
                {
                    LogFactory.GetLogger().Info("开始执行存储过程 proc_ActivePlatform");
                    SQLHelperFactory.Instance.ExecuteNonQuery("proc_ActivePlatform", null);
                    LogFactory.GetLogger().Info("执行存储过程 proc_ActivePlatform 完成");
                }
            }
            catch (Exception ex)
            {
                LogFactory.GetLogger().Error(string.Format("执行存储过程 proc_ActivePlatform 异常:{0}", ex));
            }
            try
            {
                if (_isStart)
                {
                    LogFactory.GetLogger().Info("开始执行存储过程 proc_HomeStatis");
                    SQLHelperFactory.Instance.ExecuteNonQuery("proc_HomeStatis", null);
                    LogFactory.GetLogger().Info("执行存储过程 proc_HomeStatis 完成");
                }
            }
            catch (Exception ex)
            {
                LogFactory.GetLogger().Error(string.Format("执行存储过程 proc_HomeStatis 异常:{0}", ex));
            }
            try
            {
                if (_isStart)
                {
                    LogFactory.GetLogger().Info("开始执行存储过程 proc_UserStatis");
                    SQLHelperFactory.Instance.ExecuteNonQuery("proc_UserStatis", null);
                    LogFactory.GetLogger().Info("执行存储过程 proc_UserStatis 完成");
                }
            }
            catch (Exception ex)
            {
                LogFactory.GetLogger().Error(string.Format("执行存储过程 proc_UserStatis 异常:{0}", ex));
            }
            try
            {
                if (_isStart)
                {
                    LogFactory.GetLogger().Info("开始执行存储过程 proc_BankDayStatis");
                    SQLHelperFactory.Instance.ExecuteNonQuery("proc_BankDayStatis", null);
                    LogFactory.GetLogger().Info("执行存储过程 proc_BankDayStatis 完成");
                }
            }
            catch (Exception ex)
            {
                LogFactory.GetLogger().Error(string.Format("执行存储过程 proc_BankDayStatis 异常:{0}", ex));
            }
            try
            {
                if (_isStart)
                {
                    LogFactory.GetLogger().Info("开始执行存储过程 proc_BankFinancingStatis");
                    SQLHelperFactory.Instance.ExecuteNonQuery("proc_BankFinancingStatis", null);
                    LogFactory.GetLogger().Info("执行存储过程 proc_BankFinancingStatis 完成");
                }
            }
            catch (Exception ex)
            {
                LogFactory.GetLogger().Error(string.Format("执行存储过程 proc_BankFinancingStatis 异常:{0}", ex));
            }
        }
    }
}
