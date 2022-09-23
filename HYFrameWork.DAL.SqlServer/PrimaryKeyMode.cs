using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYFrameWork.DAL.SqlServer
{
    /// <summary>
    /// 数据库表主键字段设计模式
    /// </summary>
    public enum PrimaryKeyMode
    {
        /// <summary>
        /// 使用GUID作为主键
        /// </summary>
        GUID,
        /// <summary>
        /// 使用自增作为主键
        /// </summary>
        IDENTITY
    }
}
