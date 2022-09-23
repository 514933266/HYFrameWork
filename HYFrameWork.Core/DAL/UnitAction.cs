using System;
using System.Data;

namespace HYFrameWork.Core
{
    /// <summary>
    /// 工作单元单位
    /// </summary>
    public class UnitAction
    {
        public UnitAction(Func<IDbTransaction, int> action, IDbConnection conn)
        {
            Conn = conn;
            Action = action;
        }

        /// <summary>
        /// 连接对象
        /// </summary>
        public IDbConnection Conn { get; set; }
        /// <summary>
        /// 执行方法
        /// </summary>
        public Func<IDbTransaction, int> Action { get; set; }
    }
}
