using System;
using System.Linq.Expressions;

namespace HYFrameWork.Core
{
    /// <summary>
    /// 数据库排序对象
    /// </summary>
    public class DbSort<T>
    {
        private string orderby;

        public DbSort(string str = null)
        {
            orderby = str;
        }
        /// <summary>
        /// 排序(正序)
        /// </summary>
        /// <typeparam name="TField">排序字段类型</typeparam>
        /// <param name="expression">排序表达式</param>
        /// <returns>排序对象</returns>
        public DbSort<T> Asc<TField>(Expression<Func<T, TField>> expression)
        {
            orderby += "," + (expression.Body as MemberExpression).Member.Name;
            return this;
        }

        /// <summary>
        /// 排序(反序)
        /// </summary>
        /// <typeparam name="TField">排序字段</typeparam>
        /// <param name="expression">排序表达式</param>
        /// <returns>排序对象</returns>
        public DbSort<T> Desc<TField>(Expression<Func<T, TField>> expression)
        {
            orderby += "," + (expression.Body as MemberExpression).Member.Name + " DESC";
            return this;
        }

        /// <summary>
        /// 构建排序语句
        /// </summary>
        /// <param name="value">返回排序语句</param>
        public static implicit operator string(DbSort<T> value)
        {
            return value == null ? null : value.orderby.TrimStart(',');
        }
    }
}
