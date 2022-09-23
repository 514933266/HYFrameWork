using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HYFrameWork.Core
{
    /// <summary>
    /// 表达式创建器：常用语创建或追加where条件语句
    /// </summary>
    public static class PredicateBuilder
    {
        /// <summary>
        /// 创建一条恒真表达式
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>恒真表达式</returns>
        public static Expression<Func<T, bool>> True<T>()
        {
            return param => true;
        }
        /// <summary>
        /// 创建一条恒假表达式
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>恒假表达式</returns>
        public static Expression<Func<T, bool>> False<T>()
        {
            return param => false;
        }
        /// <summary>
        /// 创建一条表达式
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="predicate">表达式</param>
        /// <returns>表达式</returns>
        public static Expression<Func<T, bool>> Create<T>(Expression<Func<T, bool>> predicate)
        {
            return predicate;
        }
        /// <summary>
        /// 追加表达式内容并返回新内容
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="first">原始表达式</param>
        /// <param name="second">追加表达式</param>
        /// <returns>新表达式</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.AndAlso);
        }
        /// <summary>
        /// 创建一条或表达式
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="first">表达式1</param>
        /// <param name="second">表达式2</param>
        /// <returns>新表达式</returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.OrElse);
        }
        /// <summary>
        /// 创建一条非表达式
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="expression">表达式</param>
        /// <returns>新表达式</returns>
        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression)
        {
            var negated = Expression.Not(expression.Body);
            return Expression.Lambda<Func<T, bool>>(negated, expression.Parameters);
        }

        static Expression<T> Compose<T>(
            this Expression<T> first, Expression<T> second,
            Func<Expression, Expression, Expression> merge
            )
        {
            var map = first.Parameters
                .Select((f, i) => new { f, s = second.Parameters[i] })
                .ToDictionary(p => p.s, p => p.f);

            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        class ParameterRebinder : ExpressionVisitor
        {
            readonly Dictionary<ParameterExpression, ParameterExpression> _map;

            ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
            {
                _map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }

            public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
            {
                return new ParameterRebinder(map).Visit(exp);
            }

            protected override Expression VisitParameter(ParameterExpression p)
            {
                ParameterExpression replacement;

                if (_map.TryGetValue(p, out replacement))
                {
                    p = replacement;
                }

                return base.VisitParameter(p);
            }
        }
    }

}
