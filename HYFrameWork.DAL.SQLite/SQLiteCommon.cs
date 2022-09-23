using HYFrameWork.Core;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using System.Text;

namespace HYFrameWork.DAL.SQLite
{
    /// <summary>
    /// 公共的Sqlite操作
    /// </summary>
    public class SQLiteCommon
    {
        #region 库创建
        /// <summary>
        ///  创建一个空的Sqlite数据库
        /// </summary>
        /// <param name="dbPath">数据库路径</param>
        public static void CreateDb(string dbPath)
        {
            if (!System.IO.File.Exists(dbPath))
            {
                string path = Path.GetDirectoryName(dbPath);
                Directory.CreateDirectory(path);
                using (FileStream fs = new FileStream(dbPath, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(new byte[0], 0, 0);
                }
            }
        }
        #endregion

        #region 表创建
        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="sqlite">sqlite连接对象</param>
        /// <param name="columNames">自定义字段</param>
        public static void CreateTable<T>(IRepository<T> sqlite, string columNames) where T : class, new()
        {
            var sql = string.Format("CREATE  TABLE IF NOT EXISTS {0} ( {1} )", typeof(T).Name, columNames);
            sqlite.Execute(sql, null);
        }

        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="sqlite">数据库连接对象</param>
        public static void CreateTable<T>(IRepository<T> sqlite) where T : class, new()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(string.Format("CREATE  TABLE IF NOT EXISTS [{0}] ",typeof(T).Name));
            sql.Append("(");
            PropertyInfo[] pros = ReflectionHelper.GetPropertys<T>();
            pros.ForEach(p =>
            {
                sql.Append(GetColumnString(p));
            });
            sql.Remove(sql.ToString().LastIndexOf(','), 1);
            sql.Append(") ");
            sqlite.Execute(sql.ToString(), null);
        }

        private static string GetColumnString(PropertyInfo p)
        {
            StringBuilder colums = new StringBuilder();
            IEnumerable<CustomAttributeData> attrs = p.CustomAttributes;
            foreach (var attr in attrs)
            {
                if (attr.AttributeType != typeof(NonWriteAttribute))
                {
                    colums.Append(GetCoustraint(attr));
                }
                else
                {
                    return "";
                }
            }
            return "\"{0}\" {1} {2},".Fmt(p.Name, GetDataType(p), colums.ToString()); 
        }

        private static string GetDataType(PropertyInfo p)
        {
            if (p.PropertyType == typeof(int)||
                p.PropertyType.BaseType==typeof(Enum) ||
                p.PropertyType == typeof(long))                     return "INTEGER ";
            else if (p.PropertyType == typeof(string))              return "TEXT ";
            else if (p.PropertyType == typeof(DateTime))            return "DATETIME ";
            else if (p.PropertyType == typeof(bool))                return "BOOLEAN ";
            else
            {
                throw new NotSupportedException("Data types that are not supported ");
            }
        }

        private static string GetCoustraint(CustomAttributeData attr)
        {
            if (attr.AttributeType == typeof(PrimaryKeyAttribute))              return "PRIMARY KEY ";
            else if (attr.AttributeType == typeof(AutoIncrementAttribute))      return "AUTOINCREMENT ";
            else if(attr.AttributeType == typeof(NotNullAttribute))             return "NOT NULL ";
            else if(attr.AttributeType == typeof(UniqueAttribute))              return "UNIQUE ";
            else if(attr.AttributeType == typeof(CheckAttribute))               return "CHECK({0}) ".Fmt(attr.ConstructorArguments[0].Value);
            else
            {
                return "";
            }
        }
        #endregion

        #region 连接字符串

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="dataSouce">数据库绝对路径</param>
        /// <returns>sqlite连接字符串</returns>
        public static string GetConnectString(string dataSouce)
        {
            return string.Format("Data Source={0};Pooling=true;FailIfMissing=false", dataSouce);
        }

        /// <summary>
        /// 创建一个连接对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="dbPath">数据库路径</param>
        /// <returns>连接对象</returns>
        public static IRepository<T> CreateSQLiteRepository<T>(string dbPath)
        {
            var conn = new SQLiteConnection(GetConnectString(dbPath));
            return new SQLiteRepository<T>(conn);
        }
        #endregion
    }
}
