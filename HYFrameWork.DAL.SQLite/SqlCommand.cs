using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYFrameWork.DAL.SQLite
{
    /// <summary>
    /// 数据库语句对象
    /// </summary>
   public class SqlCommand
    {
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
