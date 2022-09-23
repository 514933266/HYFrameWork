using System.Collections.Generic;
using System.Data.Common;

namespace HYFrameWork.Core
{
    /// <summary>
    /// 数据库仓储模式：储存过程接口
    /// </summary>
    public partial interface IRepository<T>
    {
        /// <summary>
        /// 执行存储过程，返回存储过程返回值(只支持整型)
        /// </summary>
        /// <param name="procedure">存储过程名称</param>
        /// <param name="parms">参数，可以使用输出参数和返回参数</param>
        /// <returns>存储过程返回值</returns>
        int StoreProcedure(string procedure, DbParameter[] parms);

        /// <summary>
        /// 执行存储过程，返回影响行数
        /// </summary>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="procedure">存储过程名称</param>
        /// <param name="parms">参数，可以使用输出参数和返回参数</param>
        /// <returns>结果集</returns>
        IEnumerable<TResult> QueryStoreProcedure<TResult>(string procedure, DbParameter[] parms);

    }
}
