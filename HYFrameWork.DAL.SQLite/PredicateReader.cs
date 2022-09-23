using System;
using System.Linq.Expressions;
using HYFrameWork.Core;
using System.Text;
using System.Linq;

namespace HYFrameWork.DAL.SQLite
{
    /// <summary>
    /// 将简单的表达式转换成Where语句
    /// </summary>
    public class PredicateReader:ExpressionVisitor
    {

        #region 字段、属性

        private int _parasIndex = 0;//单次SQL拼接参数的数量下标
        /// <summary>
        /// 数据库命令 Sql默认添加WHERE
        /// </summary>
        public SqlCommand SQLiteCommand { get; set; }
        #endregion
        #region 构造、初始化
        public PredicateReader()
        {
            SQLiteCommand = new SqlCommand() { Sql = "WHERE "};
        }
        /// <summary>
        /// 返回经过解析的数据库命令
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <returns>Sql命令</returns>
        public SqlCommand Transform(LambdaExpression expression)
        {
            if (expression != null)
            {
                var exp = ExpressionEvaluator.SqlEval(expression);
                var sqlExp = Visit(exp) as ConstantExpression;
                SQLiteCommand.Sql += sqlExp.Value.ToString();
                
            }
            else
            {
                SQLiteCommand.Sql = "";
            }
            return SQLiteCommand;
        }
        /// <summary>
        /// 返回经过解析的表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <returns>更简洁的表达式</returns>
        public Expression Transform(Expression expression)
        {
            if (expression != null)
            {
                var exp = ExpressionEvaluator.SqlEval(expression);
                return Visit(exp);
            }
            return null;
        }
        #endregion

        #region 解析器选择
        /// <summary>
        /// 解析表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <returns>更简洁的表达式</returns>
        public override Expression Visit(Expression expression)
        {
            if (expression == null) return expression;
            switch (expression.NodeType)
            {
                case ExpressionType.Call:               return VisitCall((MethodCallExpression)expression);
                case ExpressionType.MemberAccess:       return VisitMemberAccess((MemberExpression)expression);
                case ExpressionType.Constant:           return VisitConstant((ConstantExpression)expression) as ConstantExpression;
                case ExpressionType.Parameter:          return expression;
                case ExpressionType.Lambda:             var exp = expression as LambdaExpression;return Visit(exp.Body);
                case ExpressionType.NotEqual:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.LessThan:
                case ExpressionType.Equal:
                case ExpressionType.OrElse:
                case ExpressionType.AndAlso:
                case ExpressionType.GreaterThanOrEqual: return VisitBinary((BinaryExpression)expression);
                case ExpressionType.Convert:
                case ExpressionType.Not:                return VisitUnary((UnaryExpression)expression);
                case ExpressionType.New:                return expression;
                default:
                    throw new NotSupportedException("Unsupported expression type:" + expression.NodeType.ToString());
            }
        }

        #endregion

        #region 二元表达式
        private new Expression VisitBinary(BinaryExpression expression)
        {
            var left = Visit(expression.Left) as ConstantExpression;
            if (expression.Left.NodeType == ExpressionType.MemberAccess &&
                expression.Right.NodeType == ExpressionType.Constant &&
                expression.Left.Type == typeof(Boolean) &&
                expression.Right.Type == typeof(Boolean))
            {
                var right = expression.Right as ConstantExpression;
                return VisitBooleanBinary(expression, left, right);
            }
            else
            {
                var right = Visit(expression.Right) as ConstantExpression;
                return VisitBinary(expression, left, right);
            }
        }
        private string AcquireBinarySymbol(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.AndAlso:            return "AND";
                case ExpressionType.OrElse:             return "OR";
                case ExpressionType.GreaterThan:        return ">";
                case ExpressionType.GreaterThanOrEqual: return ">=";
                case ExpressionType.LessThan:           return "<";
                case ExpressionType.LessThanOrEqual:    return "<=";
                case ExpressionType.Equal:              return "=";
                case ExpressionType.NotEqual:           return "!=";
                default:
                    throw new NotSupportedException("Unsupported expression type: BinarySymbol. ");
            }
        }
        private Expression VisitBooleanBinary(BinaryExpression expression, ConstantExpression left, ConstantExpression right)
        {
            string symbol = AcquireBinarySymbol(expression);
            string rightValue = rightValue = AddParameters(right.Value, expression.Right.Type);
            return Expression.Constant("({0} {1} {2})".Fmt(left.Value, symbol, rightValue));
        }
        private Expression VisitBinary(BinaryExpression expression, ConstantExpression left, ConstantExpression right)
        {
            string symbol = AcquireBinarySymbol(expression);
            string rightValue = right.Value==null?string.Empty:right.Value.ToString();
            switch (expression.Right.NodeType)
            {
                case ExpressionType.Constant:
                    if (right.Value == null)                    symbol = (symbol == "!=" ? "IS NOT NULL" : "IS  NULL");
                    else if (right.Value.Equals(""))            rightValue = "''";
                    else
                    {
                        //this method use to support c=>true and c=>false
                        if (!right.Value.Equals("1=1")&&
                            !right.Value.Equals("1=0"))         rightValue = AddParameters(right.Value, expression.Right.Type);
                    }
                    break;
            }
            return Expression.Constant("({0} {1} {2})".Fmt(left.Value, symbol, rightValue));
        }
        #endregion

        #region  一元表达式
        private new Expression VisitUnary(UnaryExpression expression)
        {
            string sql = string.Empty;
            var left = Visit(expression.Operand) as ConstantExpression;
            switch (expression.NodeType)
            {
                case ExpressionType.Not:                    sql = VisitUnaryNot(expression, left); break;
                case ExpressionType.Convert:                sql = left.Value.ToString();           break;
            }
            return Expression.Constant(sql);
        }

        private string VisitUnaryNot(UnaryExpression expression, ConstantExpression left)
        {
            string lv = left.Value.ToString();
            switch (expression.Operand.NodeType)
            {
                case ExpressionType.Call:
                    var call = expression.Operand as MethodCallExpression;
                    if (call.Method.Name == "IsNullOrEmpty")
                    {
                        return lv.Replace("IS NULL", "IS NOT NULL");
                    }
                    else
                    {
                        throw new NotSupportedException("Unsupported expression type: operator (!). ");
                    }
                default:
                     throw new NotSupportedException("Unsupported expression type: operator (!). ");
            }
        }

        #endregion

        #region 对象、属性、字段
        private Expression VisitMemberAccess(MemberExpression expression)
        {
            var exp = Visit(expression.Expression);
            switch (exp.NodeType)
            {
                case ExpressionType.Constant:           var conExp = exp as ConstantExpression; return Expression.Constant(expression.Member.GetValue(conExp.Value));
                default:                                return Expression.Constant(expression.Member.Name);
            }

        }
        #endregion

        #region 方法
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
            var obj = Visit(expression.Object);
            var paras = VisitCallParameter(expression);
            switch (obj.NodeType)
            {
                case ExpressionType.New:
                case ExpressionType.Call:
                case ExpressionType.Constant:
                    return Expression.Constant(BuildMethodSql(obj, expression.Method.Name, paras));
                default:
                    throw new NotSupportedException("Unsupported expression type: function scheduling results are not constant ");
            }
        }

        private Expression VisitCallWithoutObject(MethodCallExpression expression)
        {
            var paras = VisitCallParameter(expression);
            return Expression.Constant(BuildMethodSql(null, expression.Method.Name, paras));
        }
        private object[] VisitCallParameter(MethodCallExpression expression)
        {
            var paras = new object[expression.Arguments.Count];
            for (int i = 0; i < paras.Length; i++)
            {
                var result = Visit(expression.Arguments[i]) as ConstantExpression;
                paras[i] = result.Value;
            }
            return paras;
        }

        private string BuildMethodSql(Expression obj,string methodName,object[]paras)
        {
            var para = string.Empty;
            var newObj = obj as ConstantExpression;
            switch (methodName)
            {
                case "Contains":            return MethodSqlBuilder.Contains(newObj.Value, paras);
                case "EndsWith":            return MethodSqlBuilder.LikeEnd(newObj.Value.ToString(), paras[0].ToString());
                case "StartsWith":          return MethodSqlBuilder.LikeStart(newObj.Value.ToString(), paras[0].ToString());
                case "IsNullOrEmpty":       return MethodSqlBuilder.IsNull(paras[0].ToString());
                case "DateDiff":            return MethodSqlBuilder.DateDiff(paras[0].ToString(), paras[1], paras[2]);
                case "In":                  return MethodSqlBuilder.In(paras[0].ToString(), paras[1]);
                case "NotIn":               return MethodSqlBuilder.NotIn(paras[0].ToString(), paras[1]);
                default:                    throw new NotSupportedException("Unsupported expression type: Unknown method type ");
            }
        }
        #endregion

        #region 常量
        private new Expression VisitConstant(ConstantExpression expression)
        {
            if (expression.Type == typeof(Boolean))
            {
                if ((bool)expression.Value) return Expression.Constant("1=1");
                else return Expression.Constant("1=0");
            }
            else if (expression.Type == typeof(DateTime))
            {
                //The SQLite time format must be：yyyy-MM-dd HH:mm:ss
                var dateTime = (DateTime)expression.Value;
                return ConstantExpression.Constant(dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else
            {
                return expression;
            }
        }
        #endregion

        #region 辅助方法
        private string AddParameters<T>(T para,Type type=null)
        {
            string paramName = "@P" + _parasIndex++;
            SQLiteCommand.Parameters.Add(paramName, para, DbTypeConvertor.Convert(para, type));
            return paramName;
        }
        #endregion

    }
}
