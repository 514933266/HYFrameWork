using System;
using System.Collections.Generic;
using System.Data.Common;
using HYFrameWork.Core;

namespace HYFrameWork.DAL.SQLite
{
   partial class SQLiteRepository<T> : IRepository<T>
    {
        /// <summary>
        /// 执行存储过程，返回存储过程返回值(只支持整型)
        /// </summary>
        /// <param name="procedure">存储过程名称</param>
        /// <param name="parms">参数，可以使用输出参数和返回参数</param>
        /// <returns>存储过程返回值</returns>
        public int StoreProcedure(string procedure, DbParameter[] parms)
        {
            throw new NotSupportedException("SQLite does not support stored procedures !");
        }

        /// <summary>
        /// 执行存储过程，返回影响行数
        /// </summary>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="procedure">存储过程名称</param>
        /// <param name="parms">参数，可以使用输出参数和返回参数</param>
        /// <returns>结果集</returns>
        public IEnumerable<TResult> QueryStoreProcedure<TResult>(string procedure, DbParameter[] parms)
        {
            throw new NotSupportedException("SQLite does not support stored procedures !");
        }
    }
}
