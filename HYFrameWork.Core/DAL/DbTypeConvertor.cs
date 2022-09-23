using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYFrameWork.Core
{
    /// <summary>
    /// 数据库类型转换器
    /// </summary>
    public class DbTypeConvertor
    {
        /// <summary>
        ///  根据参数类型转换为对应的数据库类型
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="parameter">参数值</param>
        /// <param name="type">强制指示参数类型</param>
        /// <returns>数据库类型</returns>
        public static DbType Convert<T>(T parameter,Type type)
        {
            string typeName = string.Empty;
            if (type != null)
            {
                typeName = type.Name;
            }
            else
            {
                typeName = typeof(T).Name;
            }
            return (DbType)Enum.Parse(typeof(DbType),typeName);
        }
    }
}
