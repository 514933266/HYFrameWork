using System.Data;
using System.Data.SqlClient;

namespace HYFrameWork.Core
{
    /// <summary>
    /// SQL语句扩展类
    /// </summary>
    public static class SqlExtension
    {
        /// <summary>
        /// 转换为数据库参数对象
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <param name="direction">相对于查询内使用的参数类型</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns>参数对象</returns>
        public static SqlParameter ToSqlParameter(this string name, object value, ParameterDirection? direction = null, DbType? dbType = null)
        {
            var parm = new SqlParameter(name, value);
            if (direction.HasValue)
                parm.Direction = direction.Value;

            if (dbType.HasValue)
                parm.DbType = dbType.Value;

            return parm;
        }
    }
}
