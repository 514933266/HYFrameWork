using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HYFrameWork.Core;
using HYFrameWork.Core.Utility;

namespace HYFrameWork.Web
{
    /// <summary>
    /// 本类用于Web页面接收前端传递参数
    /// </summary>
   public static class WebRequest
   {
        #region 请求参数获取

        private static T RequestPara<T>(this HttpContext context, string strName,Func<object,T>convertFunc)
        {
            try
            {
                var para = context.Request.QueryString[strName] ?? context.Request.Form[strName];
                if (para != null)
                {
                    return convertFunc(para);
                }
            }
            catch
            {

            }
            return default(T);
        }
        /// <summary>
        /// 返回字符串参数
        /// </summary>
        /// <param name="context">请求对象</param>
        /// <param name="strName">参数名</param>
        /// <returns>字符串值</returns>
        public static string GetStrPara(this HttpContext context, string strName)
        {
            var para = RequestPara(context, strName,Convert.ToString);
            if (para == null)
            {
                return null;
            }
            else
            {
                return Regex.Replace(para.ToString(), @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            }
        }
        /// <summary>
        /// 返回字符串数组参数
        /// </summary>
        /// <param name="context">请求对象</param>
        /// <param name="strName">参数名</param>
        /// <param name="action">执行方法</param>
        /// <returns>参数集合</returns>
        public static string[] GetStrParaValues(this HttpContext context, string strName,Func<string,string> action=null)
        {
            string[] reqArr = null;
            reqArr = context.Request.QueryString.GetValues(strName)??context.Request.Form.GetValues(strName);
            if (reqArr != null)
            {
                reqArr.ForEach(req =>
                {
                    req = Regex.Replace(req, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
                    if (action != null) req = action(req);
                });
            }
            return reqArr;
        }
        /// <summary>
        /// 返回整数参数
        /// </summary>
        /// <param name="context">请求对象</param>
        /// <param name="strName">参数名</param>
        /// <returns>Int参数值，默认返回0</returns>
        public static int GetIntPara(this HttpContext context, string strName)
        {
            return RequestPara(context, strName, Convert.ToInt32);
        }
        /// <summary>
        /// 返回double参数
        /// </summary>
        /// <param name="context">请求对象</param>
        /// <param name="strName">参数名</param>
        /// <returns>double参数值，默认返回0</returns>
        public static double GetDoublePara(this HttpContext context, string strName)
        {
            return RequestPara(context, strName, Convert.ToDouble);
        }
        /// <summary>
        /// 获取long类型参数
        /// </summary>
        /// <param name="context">请求对象</param>
        /// <param name="strName">参数名</param>
        /// <returns>long参数值，默认返回0，异常返回-1</returns>
        public static long GetLongPara(this HttpContext context, string strName)
        {
            return RequestPara(context, strName, Convert.ToInt64);
        }

        /// <summary>
        /// 返回布尔参数
        /// </summary>
        /// <param name="context">请求对象</param>
        /// <param name="strName">参数名</param>
        /// <returns>bool值</returns>
        public static bool GetBoolPara(HttpContext context, string strName)
        {
            var para = context.Request.QueryString[strName] ?? context.Request.Form[strName];
            return CheckBoolParameter(para);
        }
        /// <summary>
        /// 返回日期参数
        /// </summary>
        /// <param name="context">请求对象</param>
        /// <param name="strName">参数名</param>
        /// <returns>DateTime值</returns>
        public static DateTime GetDateTimePara(this HttpContext context, string strName)
        {
            return RequestPara(context, strName, Convert.ToDateTime);
        }
        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <returns>IP地址</returns>
        public static string GetIP()
        {
            return IPHelper.GetWebClientIp();
        }
        /// <summary>
        ///  取得网站的根目录的URL
        /// </summary>
        /// <returns>根目录url</returns>
        public static string GetRootURL()
        {
            string AppPath = "";
            HttpRequest Req;
            HttpContext HttpCurrent = HttpContext.Current;
            if (HttpCurrent != null)
            {
                Req = HttpCurrent.Request;
                string UrlAuthority = Req.Url.GetLeftPart(UriPartial.Authority);
                if (Req.ApplicationPath == null || Req.ApplicationPath == "/")
                    AppPath = UrlAuthority;//直接安装在   Web   站点   
                else
                    AppPath = UrlAuthority + Req.ApplicationPath;//安装在虚拟子目录下   
            }
            return AppPath;
        }
        /// <summary>
        /// 取得网站根目录的物理路径
        /// </summary>
        /// <returns>根目录物理路径</returns>
        public static string GetRootPath()
        {
            string AppPath = "";
            HttpContext HttpCurrent = HttpContext.Current;
            if (HttpCurrent != null)
            {
                AppPath = HttpCurrent.Server.MapPath("~");
            }
            else
            {
                AppPath = AppDomain.CurrentDomain.BaseDirectory;
                if (System.Text.RegularExpressions.Regex.Match(AppPath, @"\\$", System.Text.RegularExpressions.RegexOptions.Compiled).Success)
                    AppPath = AppPath.Substring(0, AppPath.Length - 1);
            }
            return AppPath;
        }
        #endregion

        #region 自动映射请求参数
        private static bool CheckBoolParameter(object para)
        {
            if (para != null)
            {
                if (para.ToString() == "0")         return false;
                else if (para.ToString() == "1")    return true;
                else return Convert.ToBoolean(para);
            }
            return false;
        }
        /// <summary>
        /// 根据实体字段获取请求参数值并进行映射
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="context">Http请求对象</param>
        /// <param name="attrCheck">是否根据对象特性检查参数</param>
        /// <param name="t">对象</param>
        public static BaseMessage AutoGet<T>(this HttpContext context, T t,bool attrCheck = false) where T : new()
        {
            var msg= new BaseMessage { Status = true,Data = t };
            var para = string.Empty;
            var pros = ReflectionHelper.GetPropertys(t);
            try
            {
                if (attrCheck)
                {
                    foreach(var p in pros)
                    {
                        var paraMsg = CheckLimit(context, p);
                        msg.Message = paraMsg.Message;
                        if (paraMsg.Status)
                        {
                            if (paraMsg.Data.IsNull())
                                 para = context.Request.QueryString[p.Name] ?? context.Request.Form[p.Name];
                            else para = paraMsg.Data.ToString();
                            if (!para.IsNullOrEmpty())
                            {
                                para = StringHelper.FilterScript(para);
                                if (p.PropertyType == typeof(Boolean))p.SetValue(t, CheckBoolParameter(para)); 
                                else p.SetValue(t, Convert.ChangeType(para, p.PropertyType));
                            }
                        }
                        else
                        {
                            msg.Status = false;
                            break;
                        }
                    }
                }
                else
                {
                    pros.ForEach(p =>
                    {
                        para = context.Request.QueryString[p.Name] ?? context.Request.Form[p.Name];
                        if (!para.IsNullOrEmpty())
                        {
                            para = StringHelper.FilterScript(para);
                            if (p.PropertyType == typeof(Boolean)) p.SetValue(t, CheckBoolParameter(para));
                            else p.SetValue(t, Convert.ChangeType(para, p.PropertyType));
                        }
                    });
                }
            }
            catch(Exception ex)
            {
                msg.Status = false;
                msg.Message += ex.Message;
            }
            return msg;
        }
        /// <summary>
        /// 根据特性检查检查对象属性值
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="t">对象</param>
        /// <returns>基础消息</returns>
        public static BaseMessage CheckLimit<T>(this T t)
        {
            var para = string.Empty;
            var dateType = string.Empty;
            var exception = string.Empty;
            var disName = string.Empty;
            var msg = new BaseMessage { Status = true, Data = t };
            try
            {
                var pros = ReflectionHelper.GetPropertys(t);
                foreach (var pro in pros)
                {
                    var attrs = pro.GetCustomAttributes(typeof(DisplayAttribute), false);
                    foreach (var attr in attrs)
                    {
                        var atype = attr.GetType();
                        var name = atype.GetProperty("Name").GetValue(attr);
                        if (name != null) disName = name.ToString();
                        break;
                    }
                    foreach (var attr in attrs)
                    {
                        var atype = attr.GetType();
                        if (atype == typeof(RequiredAttribute)) msg = CheckRequire(atype, disName, attr, para);
                        else if (atype == typeof(RangeAttribute)) msg = CheckRange(atype, disName, attr, para);
                        else if (atype == typeof(StringLengthAttribute)) msg = CheckString(atype, disName, "", attr, para);
                        else if (atype == typeof(RegularExpressionAttribute)) msg = CheckRegular(atype, disName, attr, para);
                        if (!msg.Status) break;
                    }
                    if (!para.IsNullOrEmpty()) msg.Data = para;
                    if (!msg.Status) break;
                }
            }
            catch(Exception ex)
            {
                exception = ex.Message;
            }
            msg.Message = msg.Message.IsNullOrEmpty() ? ("({0}){1}".Fmt(disName, exception)) : msg.Message;
            return msg;
        }
        //检查特性 基于约定：特性的优先级为Display、DataType、Required、Range、StringLength
        private static BaseMessage CheckLimit(HttpContext context, PropertyInfo pro)
        {
            var para = string.Empty;
            var disName = string.Empty;
            var dateType = string.Empty;
            var disShortName = string.Empty;
            var exception = string.Empty;
            var msg = new BaseMessage() { Status = true };
            try
            {
                var attrs = pro.GetCustomAttributes(false);
                foreach (var attr in attrs)
                {
                    var atype = attr.GetType();
                    if (atype == typeof(DisplayAttribute))
                    {
                        var name = atype.GetProperty("Name").GetValue(attr);
                        if (name != null) disName = name.ToString();
                        disShortName = atype.GetProperty("ShortName").GetValue(attr).ToString();
                        para = context.Request.QueryString[disShortName] ?? context.Request.Form[disShortName];
                        break;
                    }
                }
                if (disName.IsNullOrEmpty()) disName = disShortName;
                foreach (var attr in attrs)
                {
                    var atype = attr.GetType();
                    if (atype == typeof(RequiredAttribute)) msg = CheckRequire(atype, disName, attr, para);
                    else if (atype == typeof(RangeAttribute)) msg = CheckRange(atype, disName, attr, para);
                    else if (atype == typeof(StringLengthAttribute)) msg = CheckString(atype, disName, dateType, attr, para);
                    else if (atype == typeof(RegularExpressionAttribute)) msg = CheckRegular(atype, disName, attr, para);
                    if (!msg.Status) break;
                }
                if (!para.IsNullOrEmpty()) msg.Data = para;
            }
            catch (Exception ex)
            {
                exception = ex.Message;
            }
            msg.Message=msg.Message.IsNullOrEmpty() ? ("({0}){1}".Fmt(disName,exception)) : msg.Message;
            return msg;
        }
        private static BaseMessage CheckRequire(Type atype,string disName, object attr, string value)
        {
            var defaultMsg =disName + "不能为空";
            var errMsg = atype.GetProperty("ErrorMessage").GetValue(attr).TryString();
            if (value.IsNullOrEmpty()) return new BaseMessage() { Message = errMsg.IsNullOrEmpty()?defaultMsg:errMsg, Status = false };
            return new BaseMessage() { Status = true };
        }
        private static BaseMessage CheckString(Type atype, string disName, string dataType, object attr, string value)
        {
            int minValue = 0, maxValue = 0;
            var msg         = new BaseMessage() { Status = true };
            var min         = Convert.ToInt32(atype.GetProperty("MinimumLength").GetValue(attr));
            var max         = Convert.ToInt32(atype.GetProperty("MaximumLength").GetValue(attr));
            var errMsg      = atype.GetProperty("ErrorMessage").GetValue(attr).TryString();
            if (!dataType.IsNullOrEmpty())
            {
                switch (dataType)
                {
                    case "char":
                    case "varchar":
                        if (value.HasChinese())
                        {
                            minValue = min / 2;
                            maxValue = max / 2;
                        }
                        else
                        {
                            minValue = min;
                            maxValue = max;
                        }
                        break;
                    case "nchar":
                    case "nvarchar":
                        minValue = min;
                        maxValue = max;
                        break;
                }
                if (!value.IsNullOrEmpty())
                {
                    if (value.Length < minValue) return new BaseMessage() { Message = disName+"长度小于" + minValue, Status = false };
                    if (value.Length > maxValue) return new BaseMessage() { Message = disName + "长度大于" + maxValue, Status = false };
                }
            }
            else
            {
                if (!value.IsNullOrEmpty())
                {
                    if (value.Length < min) return new BaseMessage() { Message = disName + "长度小于" + minValue, Status = false };
                    if (value.Length > max) return new BaseMessage() { Message = disName + "长度大于" + maxValue, Status = false };
                }
            }
            if (!errMsg.IsNullOrEmpty()) msg.Message = errMsg;
            return msg;
        }
        //检查数值范围特性
        private static BaseMessage CheckRange(Type atype,string disName,object attr, string value)
        {
            var inRange = true;
            var msg = new BaseMessage() { Status = true };
            var minValue        = atype.GetProperty("Minimum").GetValue(attr);
            var maxValue        = atype.GetProperty("Maximum").GetValue(attr);
            var type            = atype.GetProperty("OperandType").GetValue(attr) as Type;
            var errMsg          = atype.GetProperty("ErrorMessage").GetValue(attr).TryString();
            try
            {
                if (value.IsNullOrEmpty()) return msg;
                if (type == typeof(byte))
                {
                    var conValue = Convert.ToByte(value);
                    var conMinValue = Convert.ToByte(minValue);
                    var conMaxValue = Convert.ToByte(maxValue);
                    if (conValue < conMinValue || conValue > conMaxValue) inRange = false;
                }
                else if (type == typeof(short))
                {
                    var conValue = Convert.ToInt16(value);
                    var conMinValue = Convert.ToInt16(minValue);
                    var conMaxValue = Convert.ToInt16(maxValue);
                    if (conValue < conMinValue || conValue > conMaxValue) inRange = false;
                }
                else if (type == typeof(int))
                {
                    var conValue = Convert.ToInt32(value);
                    var conMinValue = Convert.ToInt32(minValue);
                    var conMaxValue = Convert.ToInt32(maxValue);
                    if (conValue < conMinValue || conValue > conMaxValue) inRange = false;
                }
                else if (type == typeof(long))
                {
                    var conValue = Convert.ToInt64(value);
                    var conMinValue = Convert.ToInt64(minValue);
                    var conMaxValue = Convert.ToInt64(maxValue);
                    if (conValue < conMinValue || conValue > conMaxValue) inRange = false;
                }
                else if (type == typeof(decimal))
                {
                    var conValue = Convert.ToDecimal(value);
                    var conMinValue = Convert.ToDecimal(minValue);
                    var conMaxValue = Convert.ToDecimal(maxValue);
                    if (conValue < conMinValue || conValue > conMaxValue) inRange = false;
                }
                else if (type == typeof(float))
                {
                    var conValue = float.Parse(value);
                    var conMinValue = float.Parse(minValue.ToString());
                    var conMaxValue = float.Parse(maxValue.ToString());
                    if (conValue < conMinValue || conValue > conMaxValue) inRange = false;
                }
                else if (type == typeof(DateTime))
                {
                    var conValue = Convert.ToDateTime(value);
                    var conMinValue = Convert.ToDateTime(minValue);
                    var conMaxValue = Convert.ToDateTime(maxValue);
                    if (conValue < conMinValue || conValue > conMaxValue) inRange = false;
                }
                else if (type == typeof(Boolean))
                {
                    if (value.ToString() != "0" &&value.ToString() != "1")
                    {
                        var conValue = Convert.ToBoolean(value);
                    }
                }
            }
            catch
            {
                msg.Status = false;
                msg.Message = disName+"范围应在" + minValue + "~" + maxValue + "之间";
            }
            if (!inRange)
            {
                msg.Status = false;
                msg.Message = disName+"范围应在" + minValue + "~" + maxValue + "之间";
            }
            if (!errMsg.IsNullOrEmpty()) msg.Message = errMsg;
            return msg;
        }
        //检查正则
        private static BaseMessage CheckRegular(Type atype, string disName, object attr,string value)
        {
            var msg     = new BaseMessage() { Status = true };
            var pattern = atype.GetProperty("Pattern").GetValue(attr).ToString();
            var errMsg  = atype.GetProperty("ErrorMessage").GetValue(attr).TryString();
            Regex reg   = new Regex(pattern);
            if (!reg.IsMatch(value)){
                msg.Status = false;
                msg.Message = disName+"格式不正确";
            }
            if (!errMsg.IsNullOrEmpty()) msg.Message = errMsg;
            return msg;
        }

        /// <summary>
        /// 根据实体字段获取请求参数值并进行映射
        /// 基于约定：特性的优先级为Display、DataType、Required、Range、StringLength
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="context">Http请求对象</param>
        /// <returns>对象</returns>
        public static T AutoGet<T>(this HttpContext context) where T : class,new()
        {
            var t = typeof(T);
            var obj = ReflectionHelper.CreateInstance<T>(t.Assembly.FullName.ToString(), t.FullName);
            return AutoGet(context, obj).Data as T;
        }
        /// <summary>
        /// 根据实体的字段获取请求参数值并进行映射，同时根据字段值特性检查
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="context">Http请求对象</param>
        /// <returns>对象</returns>
        public static BaseMessage AutoGetCheck<T>(this HttpContext context) where T : new()
        {
            var t = typeof(T);
            var obj = ReflectionHelper.CreateInstance<T>(t.Assembly.FullName.ToString(), t.FullName);
            return AutoGet(context, obj,true);
        }

        #endregion
    }
}
