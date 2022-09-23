using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace HYFrameWork.Core
{

    ///<summary>
    ///表达式求值类
    ///</summary>
    public class PredicateEvaluator : ExpressionVisitor
    {
        /// <summary>
        /// 解析表达式
        /// </summary>
        /// <param name="expression">需要计算的表达式</param>
        /// <returns>返回更简洁的结果表达式</returns>
        public override Expression Visit(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Lambda:                             return base.Visit(expression);
                case ExpressionType.NotEqual:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.LessThan:
                case ExpressionType.Equal:
                case ExpressionType.OrElse:
                case ExpressionType.AndAlso:
                case ExpressionType.GreaterThanOrEqual:                 return VisitBinary((BinaryExpression)expression);
                case ExpressionType.Add:                                return VisitAdd((BinaryExpression)expression);
                case ExpressionType.NewArrayInit:                       return VisitNewArray((NewArrayExpression)expression);
                case ExpressionType.New:                                return VisitNew((NewExpression)expression);
                case ExpressionType.MemberAccess:                       return VisitMember((MemberExpression)expression);
                case ExpressionType.MemberInit:                         return VisitMemberInit((MemberInitExpression)expression);
                case ExpressionType.Call:                               return VisitCall((MethodCallExpression)expression);
                case ExpressionType.Subtract:                           return VisitSubtract((BinaryExpression)expression);
                case ExpressionType.Convert:
                case ExpressionType.Not:                                return VisitUnary((UnaryExpression)expression);
                default:                                                return expression;
            }
        }
        private new Expression VisitBinary(BinaryExpression expression)
        {
            if (expression.Right != null &&
                expression.Left.Type == typeof(Boolean) &&
                expression.Left.NodeType == ExpressionType.MemberAccess)
            {
                //Order to support obj.Sex==true
                if (expression.Right.NodeType == ExpressionType.Constant)
                    return expression;
                else
                {
                    switch (expression.NodeType)
                    {
                        case ExpressionType.Equal: return Expression.Equal(expression.Left, Visit(expression.Right));
                        case ExpressionType.NotEqual: return Expression.NotEqual(expression.Left, Visit(expression.Right));
                    }
                }
            }
            return base.Visit(expression);
        }

        private new Expression VisitUnary(UnaryExpression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Not:
                    if (expression.Operand.Type==typeof(Boolean)&&
                        expression.Operand.NodeType== ExpressionType.MemberAccess)
                    {
                        //Convert !obj.Sex to obj.Sex==false
                        var exp = Visit(expression.Operand) as BinaryExpression;
                        return Expression.Equal(exp.Left, Expression.Constant(false));
                    }
                    break;
                case ExpressionType.Convert:
                    return Visit(expression.Operand) as ConstantExpression;
            }
            return expression;
        }
        private new Expression VisitMember(MemberExpression expression)
        {
            if (expression.Expression != null)
            {
                var member = Visit(expression.Expression);
                switch (member.NodeType)
                {
                    case ExpressionType.Constant:
                        var o = member as ConstantExpression;
                        object result = expression.Member.GetValue(o.Value);
                        return Expression.Constant(result);
                    case ExpressionType.Parameter:
                        if (expression.Type == typeof(Boolean))
                        { 
                            //Convert obj.Sex to obj.Sex==true
                            return Expression.Equal(expression, Expression.Constant(true));
                        }
                        break;
                }
                return expression;
            }
            else
            {
                object result =expression.Member.GetValue(expression.Member.DeclaringType);
                return Expression.Constant(result);
            }
        }
        private new Expression VisitMemberInit(MemberInitExpression expression)
        {
            var obj = Activator.CreateInstance(expression.Type);
            PropertyInfo[] pros = ReflectionHelper.GetPropertys(obj);
            object[] paras = new object[expression.Bindings.Count];
            for (int i = 0; i < expression.Bindings.Count; i++)
            {
                var bind = expression.Bindings[i] as MemberAssignment;
                foreach (PropertyInfo p in pros)
                {
                    if (p.Name == bind.Member.Name)
                    {
                        var value = Visit(bind.Expression) as ConstantExpression;
                        p.SetValue(obj, value.Value);
                        break;
                    }
                }
            }
            return Expression.Constant(obj);
        }
        private new Expression VisitNewArray(NewArrayExpression expression)
        {
            if (expression.Type == typeof(int[]))           return Expression.Constant(VisitNewArrayIntItems(expression));
            else                                            return Expression.Constant(VisitNewArrayStringItems(expression));
        }

        private int[] VisitNewArrayIntItems(NewArrayExpression expression)
        {
            int[] arr = new int[expression.Expressions.Count];
            for (int i = 0; i < arr.Length; i++)
            {
                var result = Visit(expression.Expressions[i]) as ConstantExpression;
                arr[i] = (int)result.Value;
            }
            return arr;
        }
        private string[] VisitNewArrayStringItems(NewArrayExpression expression)
        {
            string[] arr = new string[expression.Expressions.Count];
            for (int i = 0; i < arr.Length; i++)
            {
                var result = Visit(expression.Expressions[i]) as ConstantExpression;
                arr[i] = result.Value.ToString();
            }
            return arr;
        }
        private new Expression VisitNew(NewExpression expression)
        {
            var obj = Activator.CreateInstance(expression.Type);
            return Expression.Constant(obj);
        }
        private Expression VisitAdd(BinaryExpression expression)
        {
            object result = null;
            var left = Visit(expression.Left) as ConstantExpression;
            var right = Visit(expression.Right) as ConstantExpression;
            try
            {
                if (left.Type.Equals(typeof(string)) && right.Type.Equals(typeof(string)))
                    result = left.Value.ToString().Append(right.Value.ToString());
                else
                    result = Convert.ToInt32(left.Value) + Convert.ToInt32(right.Value);
            }
            catch
            {
                return expression;
            }
            return Expression.Constant(result);
        }
        private Expression VisitSubtract(BinaryExpression expression)
        {
            object result = null;
            var left = Visit(expression.Left) as ConstantExpression;
            var right = Visit(expression.Right) as ConstantExpression;
            try
            {
                result = Convert.ToInt32(left.Value) - Convert.ToInt32(right.Value);
            }
            catch
            {
               return expression;
            }
            return Expression.Constant(result);
        }
        private Expression VisitCall(MethodCallExpression expression)
        {
            if (expression.Object != null)
            {
                return VisitCallWithObject(expression);
            }
            else
            {
                return VisitCallWithoutObject(expression);
            }
            
        }

        private Expression VisitCallWithObject(MethodCallExpression expression)
        {
            object rv;
            var obj = Visit(expression.Object);
            var paras = VisitCallParameter(expression);
            if (paras != null)
            {
                switch (obj.NodeType)
                {
                    case ExpressionType.Constant:
                        var newObj = obj as ConstantExpression;
                        if (newObj.Value != null)
                            rv = expression.Method.Invoke(newObj.Value, paras);
                        else
                            rv = expression.Method.ReturnType.IsValueType ? Activator.CreateInstance(expression.Method.ReturnType) : null;
                        return Expression.Constant(rv);
                }
            }
            return Expression.Call(expression.Object,expression.Method, EvalCallParameter(expression));
        }

        private object[] VisitCallParameter(MethodCallExpression expression)
        {
            var paras = new object[expression.Arguments.Count];
            for (int i = 0; i < paras.Length; i++)
            {
                var result = Visit(expression.Arguments[i]) as ConstantExpression;
                if (result != null)
                {
                    paras[i] = result.Value;
                }
                else
                {
                    return null;
                }
            }
            return paras;
        }

        private Expression[] EvalCallParameter(MethodCallExpression expression)
        {
            var exps = new Expression[expression.Arguments.Count];
            for (int i = 0; i < exps.Length; i++)
            {
                var result = Visit(expression.Arguments[i]);
                exps[i] = result;
            }
            return exps;
        }
        private Expression VisitCallWithoutObject(MethodCallExpression expression)
        {
            object rv;
            var paras = VisitCallParameter(expression);
            if (paras==null)
            {
                return Expression.Call(expression.Method, EvalCallParameter(expression));
            }
            else
            {
                rv = expression.Method.Invoke(Activator.CreateInstance(expression.Method.ReturnType), paras);
                return Expression.Constant(rv);
            }
        }


    }


}
