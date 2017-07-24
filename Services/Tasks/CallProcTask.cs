using DataAccessHelper.SQLHelper;
using Services.Common;
using System;

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
                    if (!string.IsNullOrWhiteSpace(Config.Param))
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
    }
}
