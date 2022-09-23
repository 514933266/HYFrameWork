using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HYFrameWork.Core
{
    /// <summary>
    /// Object 扩展类
    /// </summary>
   public static class ObjectHelper
    {
        #region 转换

        /// <summary>
        /// 尝试转换Long类型
        /// </summary>
        /// <param name="o">转换对象</param>
        /// <param name="nullVal">NULL值时的返回值</param>
        /// <returns>转换值</returns>
        public static long? TryLong(this object o, long? nullVal = null)
        {
            if (null == o)
                return nullVal;
            long oInt;
            if (long.TryParse(o.ToString(), out oInt))
                return oInt;
            return nullVal;
        }


        /// <summary>
        /// 尝试转换Int类型
        /// </summary>
        /// <param name="o">转换对象</param>
        /// <param name="nullVal">NULL值时的返回值</param>
        /// <returns>转换值</returns>
        public static int? TryInt(this object o, int? nullVal = null)
        {
            if (null == o)
                return nullVal;
            int oInt;
            if (int.TryParse(o.ToString(), out oInt))
                return oInt;
            return nullVal;
        }


        /// <summary>
        /// 尝试转换Decimal类型
        /// </summary>
        /// <param name="o">转换对象</param>
        /// <param name="nullVal">NULL值时的返回值</param>
        /// <returns>转换值</returns>
        public static decimal? TryDecimal(this object o, decimal? nullVal = null)
        {
            if (null == o)
                return nullVal;
            decimal oInt;
            if (decimal.TryParse(o.ToString(), out oInt))
                return oInt;
            return nullVal;
        }

        /// <summary>
        /// 尝试转换Bool类型
        /// </summary>
        /// <param name="o">转换对象</param>
        /// <param name="trueVal">值为真时的默认返回字符串</param>
        /// <param name="falseVal">值为假时的默认返回字符串</param>
        /// <param name="nullVal">NULL值时的返回值</param>
        /// <returns>转换值</returns>
        public static bool? TryBoolean(this object o, string trueVal = "1", string falseVal = "0", bool? nullVal = null)
        {
            if (null == o)
                return nullVal;
            bool oBool;
            var oStr = o.ToString();
            if (bool.TryParse(oStr, out oBool))
                return oBool;
            else if (trueVal == oStr)
                return true;
            else if (falseVal == oStr)
                return false;

            return nullVal;
        }

        /// <summary>
        /// 尝试转换Guid类型
        /// </summary>
        /// <param name="o">转换对象</param>
        /// <param name="nullVal">NULL值时的返回值</param>
        /// <returns>转换值</returns>
        public static Guid? TryGuid(this object o, Guid? nullVal = null)
        {
            if (null == o)
                return nullVal;
            Guid oGuid;
            if (Guid.TryParse(o.ToString(), out oGuid))
                return oGuid;
            return nullVal;
        }

        /// <summary>
        /// 尝试转换DateTime类型
        /// </summary>
        /// <param name="o">转换对象</param>
        /// <param name="nullVal">NULL值时的返回值</param>
        /// <returns>转换值</returns>
        public static DateTime? TryDateTime(this object o, DateTime? nullVal = null)
        {
            if (null == o)
                return nullVal;
            DateTime oDateTime;
            if (DateTime.TryParse(o.ToString(), out oDateTime))
                return oDateTime;
            return nullVal;
        }

        /// <summary>
        /// 尝试转换String类型
        /// </summary>
        /// <param name="o">转换对象</param>
        /// <param name="nullVal">NULL值时的返回值</param>
        /// <returns>转换值</returns>
        public static string TryString(this object o, string nullVal = "")
        {
            if (null == o)
                return nullVal;

            return o.ToString();
        }

        /// <summary>
        /// 尝试转换String类型
        /// </summary>
        /// <param name="o">转换对象</param>
        /// <param name="fn">转换对象时执行的方法</param>
        /// <param name="nullVal">NULL值时的返回值</param>
        /// <returns>转换值</returns>
        public static string TryString(this object o, Func<string> fn, string nullVal = "")
        {
            if (null == o)
                return nullVal;
            return fn();
        }

        /// <summary>
        /// 尝试转换String类型(去除空字符串)
        /// </summary>
        /// <param name="o">转换对象</param>
        /// <param name="nullVal">NULL值时的返回值</param>
        /// <returns>转换值</returns>
        public static string TryTrimString(this object o, string nullVal = "")
        {
            if (null == o)
                return nullVal;
            return o.ToString().Trim();
        }

        /// <summary>
        ///  检查对象是否为null
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">对象值</param>
        /// <returns>是否为NULL</returns>
        public static bool IsNull<T>(this T obj)
        {
            return obj == null;
        }
        #endregion

        #region 特殊

        /// <summary>
        /// 判断对象是否存在于集合中
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">对象值</param>
        /// <param name="list">集合对象</param>
        /// <returns>是否为NULL</returns>
        public static bool In<T>(this T obj, IEnumerable<T> list)
        {
            return list.FirstOrDefault(o => o.Equals(obj)) != null;
        }
        /// <summary>
        /// 判断对象是否不存在集合中
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">对象值</param>
        /// <param name="list">集合对象</param>
        /// <returns>是否为NULL</returns>
        public static bool NotIn<T>(this T obj, IEnumerable<T> list)
        {
            return !list.Any(o => o.Equals(obj));
        }
        #endregion

        #region 异常处理

        /// <summary>
        /// 如果NULL值则抛出异常
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="argument">对象值</param>
        /// <param name="paramName">异常名称</param>
        public static void ThrowIfNull<T>(this T argument, string paramName) where T : class
        {
            if (argument == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }
        /// <summary>
        /// 符合条件抛出异常
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="argument">对象值</param>
        /// <param name="predicate">条件</param>
        /// <param name="msg">异常消息</param>
        public static void ThrowIf<T>(this T argument, Func<T, bool> predicate, string msg)
        {
            if (predicate(argument))
            {
                throw new ArgumentException(msg);
            }
        }

        #endregion

        #region JSON 序列化

        /// <summary>
        /// 序列化对象为JSON格式
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="dateFormat">日期序列化格式</param>
        /// <param name="ignoreNull">是否忽略NULL值</param>
        /// <returns>Json字符串</returns>
        public static string ToJson(this object obj, string dateFormat = "yyyy-MM-dd HH:mm:ss", bool ignoreNull = false)
        {

            return JsonConvert.SerializeObject(obj, Formatting.None, new JsonSerializerSettings
            {
                DateFormatString = dateFormat,
                NullValueHandling = ignoreNull ? NullValueHandling.Ignore : NullValueHandling.Include
            });
        }

        /// <summary>
        /// 反序列化JSON对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <returns>json对象</returns>
        public static T FromJson<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        #endregion

        #region XML序列化
        /// <summary>
        /// 序列化xml
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>xml字符串</returns>
        public static string ToXml(this object obj)
        {
            return XMLHelper.Serialize(obj, Encoding.UTF8);
        }
        /// <summary>
        /// 反序列化xml
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="xmlStr">xml字符串</param>
        /// <returns>xml对象</returns>
        public static T FromXml<T>(this string xmlStr) where T : new()
        {
            return XMLHelper.Deserialize<T>(xmlStr, Encoding.UTF8);
        }

        /// <summary>
        /// 输出xml对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="path">输出路径</param>
        public static void WriteToXml(this object obj, string path)
        {
            XMLHelper.SerializeToFile(obj, path, Encoding.UTF8);
        }
        #endregion

    }
}
