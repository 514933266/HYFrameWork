using System;
using System.Collections.Generic;

namespace HYFrameWork.Core
{
    /// <summary>
    /// 数据库方法封装（本类方法不实现实际功能）
    /// </summary>
   public class DbMethod
    {
        /// <summary>
        /// 计算时间差（仅用于数据库表达式解析）
        /// </summary>
        /// <param name="type">计算类型</param>
        /// <param name="startdate">时间（被减数）</param>
        /// <param name="enddate">时间（减数）</param>
        /// <returns>0</returns>
        public int DateDiff(DateEnum type, object startdate, object enddate)
        {
            return 0;
        }
        /// <summary>
        /// 计算字段1是否存在字段2中（仅用于数据库表达式解析）
        /// </summary>
        /// <param name="expression1">字符串1</param>
        /// <param name="expression2">字符串2</param>
        /// <returns>0</returns>
        public int CharIndex(string expression1, object expression2)
        {
                return 0;
        }

        /// <summary>
        /// 统计数据库列（该方法仅用于数据库表达式解析）
        /// </summary>
        /// <typeparam name="T">返回对象类型</typeparam>
        /// <param name="colums">列名</param>
        /// <returns>返回对象</returns>
        public T Sum<T>(params object[] colums)
        {
            return default(T);
        }


    }
}
