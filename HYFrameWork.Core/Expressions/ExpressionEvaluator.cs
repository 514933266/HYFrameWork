using System;
using System.Linq.Expressions;

namespace HYFrameWork.Core
{
    /// <summary>
    /// <para>表达式树求值计算</para>
    ///  <para>表达式求值计算功能访问入口类，应用场景</para>
    ///  <para>1、计算SQL查询条件表达式中的本地变量</para>
    ///  <para>2、对表达式成员的访问</para>
    /// </summary>
    public class ExpressionEvaluator
    {
        /// <summary>
        /// 解析SqlWhere语句
        /// </summary>
        /// <param name="expression">需要解析的表达式</param>
        /// <returns>返回更简洁的表达式</returns>
        public static Expression SqlEval(Expression expression)
        {
            return new PredicateEvaluator().Visit(expression);
        }
    }
}
