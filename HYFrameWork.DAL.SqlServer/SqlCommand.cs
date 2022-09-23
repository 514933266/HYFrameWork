using Dapper;

namespace HYFrameWork.DAL.SqlServer
{
    /// <summary>
    /// 数据库语句对象
    /// </summary>
   public class SqlCommand
    {
        /// <summary>
        /// 构造：初始化参数集合对象
        /// </summary>
        public SqlCommand()
        {
            Parameters = new DynamicParameters();
        }

        /// <summary>
        /// Sql语句
        /// </summary>
        public string Sql { get; set; }

        /// <summary>
        /// SqlCommand参数集
        /// </summary>
        public DynamicParameters Parameters { get; set; }
    }
}
