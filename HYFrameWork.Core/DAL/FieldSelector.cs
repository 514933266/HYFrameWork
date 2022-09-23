using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace HYFrameWork.Core
{
    /// <summary>
    /// 辅助类：Sql 字段选择
    /// </summary>
    public class FieldSelector<T>
    {
        /// <summary>
        /// 从表达式中获取列名拼接字符串
        /// </summary>
        /// <typeparam name="TResult">获取字段的对象</typeparam>
        /// <param name="selector">
        /// <para>例一: u=>u.UserId   例二: u=>new{ u.UserId, u.UserName}</para>  
        /// <para>例三: u=>new User() 例四:  u=>new object{ u.UserName,u.Id}</para> 
        /// </param>
        /// <returns>查询字段</returns>
        public static string Transform<TResult>(Expression<Func<T, TResult>> selector)
        {
            switch (selector.Body.NodeType)
            {
                case ExpressionType.MemberInit: return TransformMemberInit(selector);
                case ExpressionType.Constant: return TransformConstant(selector);
                case ExpressionType.MemberAccess: return TransformMemberAccess(selector);
                case ExpressionType.NewArrayInit: return TransformNewArrayInit(selector);
                case ExpressionType.New:
                case ExpressionType.Parameter: return TransformNew<TResult>();
                default: return "*";
            }
        }

        private static string TransformMemberAccess<TResult>(Expression<Func<T, TResult>> selector)
        {
            var body = selector.Body as MemberExpression;
            return body.Member.Name;
        }


        private static string TransformMemberInit<TResult>(Expression<Func<T, TResult>> selector)
        {
            StringBuilder fields = new StringBuilder();
            var body = selector.Body as MemberInitExpression;
            foreach (var bding in body.Bindings)
            {
                var member = bding.Member.Name;
                fields.Append("[");
                fields.Append(member);
                fields.Append("]");
                fields.Append(",");
            }
            if (fields.Length > 0)
                return fields.ToString().TrimEnd(',');
            else
                return "*";
        }
        private static string TransformConstant<TResult>(Expression<Func<T, TResult>> selector)
        {
            var body = selector.Body as ConstantExpression;
            string field = body.Value.ToString();
            if (!field.IsNullOrWhiteSpace())
                return "[" + field + "]";
            else
                return "*";
        }
        private static string TransformNew<TResult>()
        {
            StringBuilder fields = new StringBuilder();
            PropertyInfo[] pros = ReflectionHelper.GetPropertys<TResult>().Where(f =>
            !Attribute.IsDefined(f, typeof(NonWriteAttribute))).ToArray();
            foreach (PropertyInfo p in pros)
            {
                fields.Append("[");
                fields.Append(p.Name);
                fields.Append("]");
                fields.Append(",");
            }
            return fields.ToString().TrimEnd(',');
        }
        private static string TransformNewArrayInit<TResult>(Expression<Func<T, TResult>> selector)
        {
            StringBuilder fields = new StringBuilder();
            var body = selector.Body as NewArrayExpression;
            foreach (var expression in body.Expressions)
            {
                switch (expression.NodeType)
                {
                    case ExpressionType.MemberAccess:
                        var member = expression as MemberExpression;
                        fields.Append("[");
                        fields.Append(member.Member.Name);
                        fields.Append("]");
                        fields.Append(",");
                        break;
                    case ExpressionType.Constant:
                        var constant = expression as ConstantExpression;
                        fields.Append("[");
                        fields.Append(constant.Value);
                        fields.Append("]");
                        fields.Append(",");
                        break;
                    case ExpressionType.Convert:
                        var convert = expression as UnaryExpression;
                        if (convert.Operand.NodeType == ExpressionType.MemberAccess)
                        {
                            var operand = convert.Operand as MemberExpression;
                            fields.Append("[");
                            fields.Append(operand.Member.Name);
                            fields.Append("]");
                            fields.Append(",");
                        }
                        break;
                    default:
                        throw new NotSupportedException("不支持的表达式类型");
                }
            }
            return fields.ToString().TrimEnd(',');
        }
    }
}
