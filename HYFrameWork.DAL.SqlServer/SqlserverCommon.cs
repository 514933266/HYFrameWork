using HYFrameWork.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYFrameWork.DAL.SqlServer
{
    /// <summary>
    /// Sqlserver公共帮助类
    /// </summary>
    public class SqlserverCommon
    {
        #region 连接字符串
        /// <summary>
        /// 创建一个连接对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="conStr">连接字符串</param>
        /// <returns>连接对象</returns>
        public static IRepository<T> CreateSqlserverRepository<T>(string conStr)
        {
            var conn = new SqlConnection(conStr);
            return new SqlServerRepository<T>(conn);
        }
        #endregion
    }
}
