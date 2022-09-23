using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using HYFrameWork.Core;

namespace HYFrameWork.DAL.SqlServer
{
    /// <summary>
    /// C# 方法转换Sql语句
    /// </summary>
   public static class MethodSqlBuilder
    {
        /// <summary>
        /// 获取sql contains 对应功能语句 
        /// </summary>
        /// <param name="obj">对象值（为string时获得Like语句）</param>
        /// <param name="paras">参数数组</param>
        /// <returns>sql Like 或 In 语句</returns>
        public static string Contains(object obj, object[] paras)
        {
            if (obj.GetType() == typeof(string))
            {
                return Like(obj.ToString(), paras[0].ToString());
            }
            else
            {
                return In(paras[0].ToString(), obj);
            }
        }
        /// <summary>
        /// 获取like语句（全匹配模式）
        /// </summary>
        /// <param name="columName">列名</param>
        /// <param name="para">参数值</param>
        /// <returns>Like语句</returns>
        public static string Like(string columName, object para)
        {
            return "({0} LIKE '%{1}%')".Fmt(columName, para.ToString().Replace("'","''"));
        }
        /// <summary>
        /// 获取like语句（前半匹配模式）
        /// </summary>
        /// <param name="columName">列名</param>
        /// <param name="para">参数值</param>
        /// <returns>Like语句</returns>
        public static string LikeStart(string columName, object para)
        {
            return "({0} LIKE '{1}%')".Fmt(columName, para.ToString().Replace("'", "''"));
        }
        /// <summary>
        /// 获取like语句（后半匹配模式）
        /// </summary>
        /// <param name="columName">列名</param>
        /// <param name="para">参数值</param>
        /// <returns>Like语句</returns>
        public static string LikeEnd(string columName, object para)
        {
            return "({0} LIKE '%{1}')".Fmt(columName, para.ToString().Replace("'", "''"));
        }
        /// <summary>
        /// 获取IS NULL语句
        /// </summary>
        /// <param name="columName">列名</param>
        /// <returns>IS NULL语句</returns>
        public static string IsNull(string columName)
        {
            return "({0} IS NULL)".Fmt(columName);
        }
        /// <summary>
        /// 时间差语句
        /// </summary>
        /// <param name="datepart">时间部件（Day.Year,Month）区分大小写</param>
        /// <param name="startdate">起始时间</param>
        /// <param name="enddate">结束时间</param>
        /// <returns>时间差语句</returns>
        public static string DateDiff(string datepart, object startdate, object enddate)
        {
            DateTime date1;
            DateTime date2;
            if (datepart == "Day")
            {
                if (DateTime.TryParse(enddate.ToString(), out date2)) enddate = "'{0}'".Fmt(enddate);
                if (DateTime.TryParse(startdate.ToString(), out date1)) startdate = "'{0}'".Fmt(enddate);
                return "(DATEDIFF({0},{1},{2}))".Fmt(datepart.ToString().Replace("'", "''"), startdate.ToString().Replace("'", "''"), enddate.ToString().Replace("'", "''"));
            }
            else
            {
                return "(DATEDIFF({0},{1},{2}))".Fmt(datepart.ToString().Replace("'", "''"), startdate.ToString().Replace("'", "''"), enddate.ToString().Replace("'", "''"));
            }
        }
        /// <summary>
        /// 获取 In语句
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <param name="list">列表对象（可以是Arr）</param>
        /// <returns>In语句</returns>
        public static string In(string columnName, dynamic list)
        {
            string sql = string.Empty;
            Type type = list.GetType();
            if (type.IsArray)                   sql = GetArrayInStr(list);
            else if (type.IsGenericType)        sql = GetListInStr(list);
            else throw new NotSupportedException("Must be list or array!");
            return "({0} IN ({1}))".Fmt(columnName, sql.Substring(0, sql.Length - 1));
        }

        private static string GetListInStr<T>(IEnumerable<T> list)
        {
            string format = string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach (var e in list)
            {
                if (e.GetType() == typeof(string))
                {
                    format = "'" + e.ToString().Replace("'", "''") + "',";
                    sb.Append(format);
                }
                else
                {
                    sb.Append(e.ToString().Replace("'", "''")).Append(",");
                }
            }
            return sb.ToString();
        }
        private static string GetArrayInStr(dynamic list)
        {
            string format = string.Empty;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i].GetType() == typeof(string))
                {
                    format = "'" + list[i].ToString().Replace("'", "''") + "',";
                    sb.Append(format);
                }
                else
                {
                    sb.Append(list[i].ToString().Replace("'", "''")).Append(",");
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// 获取Not In语句
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <param name="list">集合对象</param>
        /// <returns>Not In语句</returns>
        public static string NotIn(string columnName, dynamic list)
        {
            return In(columnName, list).Replace("IN", "NOT IN");
        }
        /// <summary>
        /// 获取CharIndex语句
        /// </summary>
        /// <param name="expression1">字符串1</param>
        /// <param name="expression2">字符串2</param>
        /// <returns>CharIndex语句</returns>
        public static string CharIndex(string expression1,string expression2)
        {
            return "CHARINDEX('{0}',{1}) ".Fmt(expression1.ToString().Replace("'", "''"), expression2.ToString().Replace("'", "''"));
        }
    }
}
