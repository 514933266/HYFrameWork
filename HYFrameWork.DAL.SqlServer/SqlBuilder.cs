using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using HYFrameWork.Core;

namespace HYFrameWork.DAL.SqlServer
{
    /// <summary>
    /// 数据库构建类
    /// </summary>
    public class SqlBuilder<T>
    {
        #region 字段、属性
        private static readonly string _tableName = typeof(T).Name;
        private static string _insertSql = string.Empty;
        private static string _updateSql = string.Empty;
        private static string _deleteSql = string.Empty;
        private static string _fieldsStr = string.Empty;
        private static string _defaultWhere = string.Empty;
        private static PropertyInfo[] Fields { get; set; }
        /// <summary>
        /// 有效字段（用于增、删、改）
        /// </summary>
        public static PropertyInfo[] EffectiveFields { get; set; }
        /// <summary>
        /// 主键(联合主键)
        /// </summary>
        public static PropertyInfo[]PrimaryKeyField { get; set; }
        /// <summary>
        /// sql插入语句模板
        /// </summary>
        public static string InsertSql { get { return _insertSql; } }
        
        #endregion

        #region 构造、初始化

        static SqlBuilder()
        {
            InitFields();
            InitFieldStr();
            InitDefaultWhere();
            InitInsertSql();
            InitUpdateSql();
            InitDeletedSql();
        }
        // 初始化字段
        private static void InitFields()
        {
            Fields = ReflectionHelper.GetPropertys<T>();
            EffectiveFields = Fields.Where(f =>
            !Attribute.IsDefined(f, typeof(NonWriteAttribute)) &&
            !Attribute.IsDefined(f, typeof(AutoIncrementAttribute))).ToArray();
            PrimaryKeyField = Fields.Where(f => Attribute.IsDefined(f, typeof(PrimaryKeyAttribute))).ToArray();
        }
        // 初始化字段字符串
        private static void InitFieldStr()
        {
            var sb = new StringBuilder();
            var DbFields = Fields.Where(f => !Attribute.IsDefined(f, typeof(NonWriteAttribute))).ToArray();
            DbFields.ForEach(field =>
            {
                sb.Append("[{0}],".Fmt(field.Name));
            });
            _fieldsStr = sb.ToString().Substring(0, sb.Length - 1).Append(" ");
        }
        // 初始化基本的插入语句
        private static void InitInsertSql()
        {
            string f = string.Empty, v = string.Empty;
            EffectiveFields.ForEach(field =>
            {
                f += "[{0}],".Fmt(field.Name);
                v += "@{0},".Fmt(field.Name);
            });
            _insertSql = "INSERT INTO [{0}] ({1}) VALUES({2}) ".Fmt(_tableName, f.Substring(0, f.Length - 1), v.Substring(0, v.Length - 1));
        }
        // 初始化基本的更新语句
        private static void InitUpdateSql()
        {
            string f = string.Empty;
            EffectiveFields.ForEach(field =>
            {
                f += "[{0}]=@{1},".Fmt(field.Name, field.Name);
            });
            _updateSql = "UPDATE [{0}] SET {1} {2}".Fmt(_tableName, f.Substring(0, f.Length - 1), _defaultWhere);
        }
        // 初始化基本的删除语句
        private static void InitDeletedSql()
        {
            _deleteSql = "DELETE FROM [{0}] {1}".Fmt(_tableName, _defaultWhere);
        }
        // 初始化Where条件
        private static void InitDefaultWhere()
        {
            if (PrimaryKeyField != null)
            {
                StringBuilder sql = new StringBuilder("WHERE");
                PrimaryKeyField.ForEach(pk =>
                {
                    sql.Append(" [{0}]=@{1} AND".Fmt(pk.Name, pk.Name));
                });
                var index = sql.ToString().LastIndexOf("AND");
                if (index > 0) _defaultWhere = sql.ToString().Substring(0, index);
            }
            else
            {
                _defaultWhere = "";
            }
        }
        // 没有主键抛出异常
        private static void ThrowIfNonKeys()
        {
            var isNonKey = PrimaryKeyField.IsNull();
            if (isNonKey)
            {
                throw new Exception("{0}对象未设置主键特性：PrimaryKeyAttribute!".Fmt(typeof(T)));
            }
        }
        #endregion

        #region 查询
        
        /// <summary>
        /// 构建查询数量语句（COUNT）
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <param name="dbLock">数据库锁</param>
        /// <returns>sql命令</returns>
        public static SqlCommand BuildCountCommand(
            Expression<Func<T, bool>> predicate,
            string dbLock)
        {
            var sql = new StringBuilder();
            var cmd = new PredicateReader().Transform(predicate);
            sql.Append("SELECT COUNT(1) ");
            sql.Append("FROM [{0}] {1} ".Fmt(_tableName, dbLock));
            sql.Append(cmd.Sql);
            cmd.Sql = sql.ToString();
            return cmd;
        }
        /// <summary>
        /// 构建查询语句
        /// </summary>
        /// <typeparam name="TResult">返回对象类型</typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <param name="orderby">排序表达式</param>
        /// <param name="selector">字段选择器</param>
        /// <param name="top">数据量</param>
        /// <param name="dbLock">数据库锁</param>
        /// <returns>sql命令</returns>
        public static SqlCommand BuildSelectCommand<TResult>(
            Expression<Func<T, bool>> predicate,
            Func<DbSort<T>, DbSort<T>> orderby,
            Expression<Func<T, TResult>> selector,
            int top,
            string dbLock)
        {
            var sql = new StringBuilder();
            var orderbyStr = ParsingOrderBy(orderby);
            var cmd = new PredicateReader().Transform(predicate);
            sql.Append("SELECT ");
            sql.Append(GetTop(top));
            sql.Append(GetFields(selector));
            sql.Append("FROM [{0}] {1} ".Fmt(_tableName, dbLock));
            sql.Append(GetWhere(cmd.Sql));
            sql.Append(GetOrderBy(orderbyStr));
            cmd.Sql = sql.ToString();
            return cmd;
        }
        /// <summary>
        /// 创建一条分页查询语句和返回总数据量语句
        /// </summary>
        /// <typeparam name="TResult">返回对象类型</typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <param name="orderby">排序表达式</param>
        /// <param name="selector">字段选择器</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <param name="dbLock">数据库锁</param>
        /// <returns>Sql命令</returns>
        public static SqlCommand BuildPageCommand<TResult>(
            Expression<Func<T, bool>> predicate,
            Func<DbSort<T>, DbSort<T>> orderby,
            Expression<Func<T, TResult>> selector,
            int pageIndex,
            int pageSize,
            string dbLock)
        {
            int beginIndex = (pageIndex - 1) * pageSize;
            int endIndex = pageIndex * pageSize;
            var sql = new StringBuilder();
            var selectorStr = GetFields(selector);
            var orderbyStr = GeDefaultOrderBy(orderby);
            var cmd = new PredicateReader().Transform(predicate);
            var pageSql = BuildPageSql(cmd.Sql, selectorStr, orderbyStr, dbLock);
            sql.Append("SELECT COUNT(1) FROM [{0}] {1} {2};".Fmt(_tableName,dbLock,cmd.Sql));
            sql.Append("SELECT {0} FROM ({1}) A ".Fmt(selectorStr, pageSql));
            sql.Append("WHERE RowNumber > {0} AND RowNumber <= {1} ".Fmt(beginIndex, endIndex));
            cmd.Sql = sql.ToString();
            return cmd;
        }
        private static string BuildPageSql(string where,string selector,string orderby, string dbLock)
        {
            var sql = new StringBuilder();
            sql.Append("SELECT ");
            sql.Append(selector);
            sql.Append(",ROW_NUMBER() ");
            sql.Append("OVER(ORDER BY {0}) AS RowNumber ".Fmt(orderby));
            sql.Append("FROM [{0}] {1} {2}".Fmt(_tableName, dbLock,where));
            return sql.ToString();
        }

        private static string GetTop(int top)
        {
            return (top > 0 ? "TOP {0} ".Fmt(top) :string.Empty);
        }
        private static string GetFields<TResult>(Expression<Func<T, TResult>> selector)
        {
            return (selector == null ? _fieldsStr : FieldSelector<T>.Transform(selector).Append(" "));
        }
        private static string GetWhere(string where)
        {
            return where.Append(" ");
        }
        private static string GetOrderBy(string orderby)
        {
            return (orderby.IsNullOrEmpty() ? string.Empty : " ORDER BY {0} ".Fmt(orderby));
        }
        private static string GeDefaultOrderBy(Func<DbSort<T>, DbSort<T>> orderby)
        {
            string orderByStr = ParsingOrderBy(orderby);
            if (orderByStr.IsNullOrEmpty())
            {
                if (PrimaryKeyField != null &&
                    PrimaryKeyField.Length > 0)
                {
                    return PrimaryKeyField[0].Name.Append(" ASC");
                }
                else
                {
                    return EffectiveFields.First().Name.Append(" ASC");
                }
                        
            }
            else
            {
                return orderByStr;
            }
        }
        #endregion

        #region 增加
        /// <summary>
        /// 创建一条插入语句
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="keymode">主键字段设计模式</param>
        /// <returns>Sql命令</returns>
        public static SqlCommand BuildAddCommand(T entity, PrimaryKeyMode keymode)
        {
            var cmd = new SqlCommand();
            foreach (var field in EffectiveFields)
            {
                var value = field.GetValue(entity);
                if (value == null) cmd.Parameters.Add(("@" + field.Name), ConvetParamValue(value));
                else cmd.Parameters.Add(("@" + field.Name), ConvetParamValue(value));
            }
            cmd.Sql = _insertSql;
            if (keymode == PrimaryKeyMode.IDENTITY)
            {
                cmd.Sql += " SELECT SCOPE_IDENTITY();";
            }
            return cmd;
        }
        /// <summary>
        /// 创建一条批量插入语句
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <returns>Sql命令</returns>
        public static SqlCommand BuildAddCommand(IEnumerable<T> entities)
        {
            int index = 0;
            string fields = string.Empty;
            var cmd = new SqlCommand();
            var sql = new StringBuilder();
            EffectiveFields.ForEach(f =>
            {
                fields += "[{0}],".Fmt(f.Name);
            });
            foreach (var e in entities)
            {
                sql.Append("INSERT INTO [{0}] ({1}) ".Fmt(_tableName, fields.Substring(0, fields.Length - 1)));
                sql.Append("VALUES (");
                string vs = string.Empty;
                string vi = string.Empty;
                EffectiveFields.ForEach(f =>
                {
                    vi = "@P{0}".Fmt(index);
                    vs += vi + ",";
                    cmd.Parameters.Add(vi, f.GetValue(e));
                    index++;
                });
                sql.Append("{0}) ".Fmt(vs.Substring(0, vs.Length - 1)));
            }
            cmd.Sql = sql.ToString().Substring(0, sql.Length - 1);
            return cmd;
        }

        /// <summary>
        /// 创建一条(NOT EXISTS)插入语句
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="predicate">条件表达式</param>
        /// <param name="keymode">主键字段设计模式</param>
        /// <returns>Sql命令</returns>
        public static SqlCommand BuildAddIfNotExistsCommand(T entity, Expression<Func<T, bool>> predicate,PrimaryKeyMode keymode)
        {
            string f = string.Empty;
            string p = string.Empty;
            string v = string.Empty;
            var sql = new StringBuilder();
            var cmd = new PredicateReader().Transform(predicate);
            EffectiveFields.ForEach(field =>
            {
                f += "[{0}],".Fmt(field.Name);
                p += "@{0},".Fmt(field.Name);
                v = "@{0}".Fmt(field.Name);
                cmd.Parameters.Add(v, ConvetParamValue(field.GetValue(entity)));
            });
            sql.Append("INSERT INTO [{0}] ({1}) ".Fmt(_tableName, f.Substring(0, f.Length - 1)));
            sql.Append("SELECT {0} ".Fmt(p.Substring(0, p.Length - 1)));
            sql.Append("WHERE NOT EXISTS( ");
            sql.Append("SELECT {0} ".Fmt(_fieldsStr));
            sql.Append("FROM [{0}] {1});".Fmt(_tableName, cmd.Sql));
            cmd.Sql = sql.ToString();
            if (keymode == PrimaryKeyMode.IDENTITY)
            {
                cmd.Sql += " SELECT SCOPE_IDENTITY();";
            }
            return cmd;
        }
        #endregion

        #region 修改
        /// <summary>
        /// 创建一条更新语句
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>Sql命令</returns>
        public static SqlCommand BuildUpdateCommand(T entity)
        {
            ThrowIfNonKeys();
            var cmd = new SqlCommand() { Sql = _updateSql };
            EffectiveFields.ForEach(field =>
            {
                cmd.Parameters.Add("@" + field.Name, ConvetParamValue(field.GetValue(entity)));
            });
            PrimaryKeyField.ForEach(pk =>
            {
                cmd.Parameters.Add("@" + pk.Name, ConvetParamValue(pk.GetValue(entity)));
            });
            return cmd;
        }
        /// <summary>
        /// 创建一条更新语句（可选择列）
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <param name="updater">更新选择器</param>
        /// <returns>Sql命令</returns>
        public static SqlCommand BuildUpdateCommand(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> updater)
        {
            var sql = new StringBuilder();
            var cmd = new PredicateReader().Transform(predicate);
            sql.Append("UPDATE [{0}] SET ".Fmt(_tableName));
            sql.Append("{0} ".Fmt(ReadUpdater(updater, cmd)));
            sql.Append(cmd.Sql);
            cmd.Sql = sql.ToString();
            return cmd;
        }
        /// <summary>
        /// 创建一条更新查询并返回新行语句（可选择列）
        /// </summary>
        /// <typeparam name="TResult">返回结果对象类型</typeparam>
        /// <param name="predicate">条件表达式</param>
        /// <param name="updater">更新选择器</param>
        /// <param name="selector">字段选择器</param>
        /// <param name="top">数据量</param>
        /// <param name="isInserted">是否isInserted（如果为True则返回修改后的数据）</param>
        /// <returns>Sql命令</returns>
        public static SqlCommand BuildUpdateSelectCommand<TResult>(
          Expression<Func<T, bool>> predicate,
          Expression<Func<T, T>> updater,
          Expression<Func<T, TResult>> selector,
          int top,
          bool isInserted)
        {
            var sql = new StringBuilder();
            var cmd = new PredicateReader().Transform(predicate);
            sql.Append("UPDATE TOP({0}) [{1}] ".Fmt(top, _tableName));
            sql.Append("WITH (UPDLOCK, READPAST) SET {0} ".Fmt(ReadUpdater(updater, cmd)));
            sql.Append("OUTPUT {0} ".Fmt(ReadSelector(selector)));
            sql.Append(cmd.Sql);
            cmd.Sql = sql.ToString();
            return cmd;
        }

        //解析更新器并赋值给cmd
        private static string ReadUpdater(Expression<Func<T, T>> updater, SqlCommand cmd)
        {
            ConstantExpression constant = null;
            var sql = new StringBuilder();
            var evaluator = new PredicateEvaluator();
            var memberInitExpression = updater.Body as MemberInitExpression;
            memberInitExpression.ThrowIf(
                m => m == null,
                "The updater expression must be of type MemberInitExpression"
                );
            foreach (var binding in memberInitExpression.Bindings)
            {
                var obj = binding as MemberAssignment;
                constant = evaluator.Visit(obj.Expression) as ConstantExpression;
                sql.Append("[{0}] = @{1}, ".Fmt(binding.Member.Name, binding.Member.Name));
                cmd.Parameters.Add("@" + binding.Member.Name, ConvetParamValue(constant.Value));
            }
            var sqlStr = sql.ToString();
            return sqlStr.Substring(0, sqlStr.LastIndexOf(","));
        }

        //解析查询选择器，并决定是查询更新前还是更新后的数据
        private static string ReadSelector<TResult>(Expression<Func<T, TResult>> selector,string prefixes= "INSERTED.")
        {
            string str;
            string[] arr;
            var sql = new StringBuilder();
            if (selector == null)
            {
                arr=_fieldsStr.Split(',');
            }
            else
            {
                arr = FieldSelector<T>.Transform(selector).Split(',');
            }
            arr.ForEach(field =>
            {
                sql.Append("{0}{1} AS {1}, ".Fmt(prefixes, field));
            });
            str = sql.ToString();
            return str.Substring(0, str.LastIndexOf(","));
        }
        #endregion

        #region 删除
        /// <summary>
        /// 创建删除语句（以主键为条件）
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>Sql命令</returns>
        public static SqlCommand BuildDeleteCommand(T entity)
        {
            ThrowIfNonKeys();
            SqlCommand cmd = new SqlCommand() { Sql= _deleteSql };
            PrimaryKeyField.ForEach(pk =>
            {
                cmd.Parameters.Add("@" + pk.Name, ConvetParamValue(pk.GetValue(entity)));
            });
            return cmd;
        }
        /// <summary>
        /// 创建条件删除语句
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>Sql命令</returns>
        public static SqlCommand BuildDeleteCommand(Expression<Func<T, bool>> predicate)
        {
            var sql = new StringBuilder();
            var cmd = new PredicateReader().Transform(predicate);
            sql.Append("DELETE FROM [{0}] ".Fmt(_tableName));
            sql.Append(cmd.Sql);
            cmd.Sql = sql.ToString();
            return cmd;
        }
        #endregion

        #region 其他方法

        //得到orderby的字符串形式
        private static string ParsingOrderBy(Func<DbSort<T>, DbSort<T>> orderby)
        {
            return orderby == null ? string.Empty : orderby(new DbSort<T>());
        }

        private static object ConvetParamValue(object obj)
        {
            var t = obj.GetType();
            if (t.Equals(typeof(DateTime)))
            {
                var dt = (DateTime)obj;
                if (dt == DateTime.MinValue) return new DateTime(1900,1,1);
            }
            return obj;
        }
        #endregion

        #region Dapper
        /// <summary>
        /// Dapper插入语句
        /// </summary>
        /// <returns></returns>
        public static string DapperInsertSql()
        {
            var sql = new StringBuilder();
            string f = string.Empty, v = string.Empty;
            EffectiveFields.ForEach(field =>
            {
                f += "[{0}],".Fmt(field.Name);
                v += "@{0},".Fmt(field.Name);
            });
            sql.Append("INSERT INTO [{0}] ({1}) VALUES ({2})".Fmt(_tableName, f.Substring(0, f.Length - 1), v.Substring(0, v.Length - 1)));
            return sql.ToString();
        }
        /// <summary>
        /// 构建SELECT IN语句
        /// </summary>
        /// <typeparam name="TResult">结果对象类型</typeparam>
        /// <typeparam name="TTarget">IN字段选择器类型</typeparam>
        /// <param name="orderby">排序表达式</param>
        /// <param name="selector">字段选择器</param>
        /// <param name="target">条件字段选择器（仅能选择一个对象的单个属性）</param>
        /// <param name="top">数据量</param>
        /// <param name="dbLock">数据库锁</param>
        /// <returns>sql命令</returns>
        public static string DapperSelectInSql<TResult, TTarget>(
            Expression<Func<T, TTarget>> target,
            Func<DbSort<T>, DbSort<T>> orderby,
            Expression<Func<T, TResult>> selector,
            int top,
            string dbLock
            )
        {
            var sql = new StringBuilder();
            var orderbyStr = ParsingOrderBy(orderby);
            sql.Append("SELECT ");
            sql.Append(GetTop(top));
            sql.Append(GetFields(selector));
            sql.Append("FROM [{0}] {1} ".Fmt(_tableName, dbLock));
            sql.Append("WHERE ");
            sql.Append(GetFields(target));
            sql.Append("IN (@ins) ");
            sql.Append(GetOrderBy(orderbyStr));
            return sql.ToString();
        }
        #endregion

    }
}
