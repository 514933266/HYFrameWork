using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace HYFrameWork.Log
{
     
    /// <summary>
    /// 使用时要将Log4Net.config文件放在项目的根目录（Debug/Release）
    /// </summary>
    public class Log4net
    {
        /// <summary>
        /// 输出日志到Log4Net
        /// </summary>
        /// <param name="t">类型</param>
        /// <param name="ex">异常</param>
        public static void WriteLog(Type t, Exception ex)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(t);
            log.Error("Error", ex);
        }


        /// <summary>
        /// 输出日志到Log4Net
        /// </summary>
        /// <param name="t">类型</param>
        /// <param name="msg">消息</param>
        public static void WriteLog(Type t, string msg)
        {   
            log4net.ILog log = log4net.LogManager.GetLogger(t);
            log.Error(msg);
        }
    }
}
