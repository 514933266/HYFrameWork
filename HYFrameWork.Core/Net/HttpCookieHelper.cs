using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace HYFrameWork.Core
{
    /// <summary>
    /// 帮助类：Cookie、HttpCookie
    /// </summary>
    public static class HttpCookieHelper
    {
        private static string[] _cookieHeader = { "expires", "domain", "path", "creation", "lastaccess", "persistent", "hostonly", "secureonly", "httponly" };
        private static string[] _cookieFitler = { "\r\n" };
        /// <summary>
        /// 将Cookies保存到指定路径
        /// </summary>
        /// <param name="cookies">Cookie集合(CookieCollection)</param>
        /// <param name="path">保存路径</param>
        public static void SaveTo(this CookieCollection cookies, string path)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (Cookie c in cookies)
                {
                    sb.AppendFormat("{0}={1};expires={2};domain={3};path={4};{5},", c.Name, c.Value, c.Expires.Ticks, c.Domain, c.Path, c.Secure);
                }
                File.WriteAllText(path, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 将Cookies保存到指定路径
        /// </summary>
        /// <param name="cookies">Cookie集合</param>
        /// <param name="path">保存路径</param>
        public static void SaveTo(this CookieContainer cookies, string path)
        {
            try
            {
                List<Cookie> list = cookies.ToList();
                StringBuilder sb = new StringBuilder();
                foreach (Cookie c in list)
                {
                    sb.AppendFormat("{0}={1};expires={2};domain={3};path={4};{5},", c.Name, c.Value, c.Expires.Ticks, c.Domain, c.Path, c.Secure);
                }
                File.WriteAllText(path, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 从指定路径读取Cookie
        /// </summary>
        /// <param name="path">Cookie文本路径</param>
        /// <param name="domain">如果Cookie没有域值，则会分配该值</param>
        /// <param name="perDomainCapacity">每个域可以容纳最大的Cookie数量</param>
        /// <returns>Cookie集合</returns>
        public static CookieContainer Get(string path, string domain, int perDomainCapacity = 99)
        {
            CookieContainer cc = new CookieContainer() { PerDomainCapacity = perDomainCapacity };
            if (File.Exists(path))
            {
                cc = File.ReadAllText(path).ToCookieContainer(domain);
            }
            return cc;
        }
        /// <summary>
        /// 获取CookieList列表
        /// </summary>
        /// <param name="cookies">Cookie集合</param>
        /// <returns>Cookie列表</returns>
        public static List<Cookie> ToList(this CookieContainer cookies)
        {
            List<Cookie> lstCookies = new List<Cookie>();
            Hashtable table = (Hashtable)cookies.GetType().InvokeMember("m_domainTable",
              System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField |
              System.Reflection.BindingFlags.Instance, null, cookies, new object[] { });
            foreach (object pathList in table.Values)
            {
                SortedList lstCookieCol = (SortedList)pathList.GetType().InvokeMember("m_list",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField
                    | System.Reflection.BindingFlags.Instance, null, pathList, new object[] { });
                foreach (CookieCollection colCookies in lstCookieCol.Values)
                    foreach (Cookie c in colCookies) lstCookies.Add(c);
            }
            return lstCookies;
        }
        /// <summary>
        /// 获取第一个匹配名称的Cookie
        /// </summary>
        /// <param name="cookies">Cookie集合</param>
        /// <param name="cookieName">查找的Cookie名称</param>
        /// <returns>查找的Cookie</returns>
        public static Cookie Find(this CookieContainer cookies, string cookieName)
        {
            List<Cookie> lstCookies = cookies.ToList();
            return lstCookies.FirstOrDefault(c => c.Name == cookieName);
        }
        /// <summary>
        /// 获取cookies集合的字符串形式
        /// </summary>
        /// <param name="cookies">Cookie集合</param>
        /// <returns>Cookie字符串格式</returns>
        public static string ToCookieString(this CookieContainer cookies)
        {
            string _cks = string.Empty;
            List<Cookie> lstCookies = cookies.ToList();
            foreach (Cookie ck in lstCookies)
            {
                _cks += string.Format("{0}={1};{2}={3};{4}={5};\r\n", ck.Name, ck.Value, "path", ck.Path, "Domain", ck.Domain);
            }
            return _cks;
        }
        /// <summary>
        ///  将cookie的字符串形式转换成指定域名下的CookieContainer
        /// </summary>
        /// <param name="cookieStr">Cookie字符串</param>
        /// <param name="domain">域名</param>
        /// <param name="perDomainCapacity">域可以容纳最大的Cookie数量</param>
        /// <returns>Cookie集合</returns>
        public static CookieContainer ToCookieContainer(this string cookieStr, string domain, int perDomainCapacity = 99)
        {

            CookieCollection ccn = new CookieCollection();
            CookieContainer cc = new CookieContainer() { PerDomainCapacity = perDomainCapacity };
            SetCookie(cookieStr, ccn, domain);
            cc.Add(ccn);
            return cc;
        }
        /// <summary>
        /// 获取CookieContainer里的COOKIE集合
        /// </summary>
        /// <param name="cookieStr">Cookie字符串</param>
        /// <param name="domain">域名</param>
        /// <returns>Cookie集合</returns>
        public static CookieCollection ToCookieCollection(this string cookieStr, string domain)
        {
            CookieCollection cc = new CookieCollection();
            SetCookie(cookieStr, cc, domain);
            return cc;
        }
        //设置cookie

        private static void SetCookie(string cookieStr, CookieCollection cc, string domain)
        {
            Cookie ck = null;
            cookieStr = StringHelper.RemoveSymbol(cookieStr,_cookieFitler);
            string[] cookieArr = cookieStr.Split(new char[] { '=', ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (cookieArr.Length > 0)
            {
                string name = string.Empty, value = string.Empty;
                for (int i = 0; i < cookieArr.Length; i++)
                {
                    var next = i + 1;
                    name = cookieArr[i].Trim();
                    if(next< cookieArr.Length)value = cookieArr[next];
                    if (!_cookieHeader.Any(c => c == name.ToLower().Trim()))
                    {
                        ck = new Cookie(name.Trim(), value);
                        ck.Domain = domain;
                        ck.Expires = DateTime.MaxValue;
                        cc.Add(ck);
                    }
                    else
                    {
                        if (name.ToLower() == "expires")
                        {
                            //cookie的时间格式特殊操作
                            value = cookieArr[(i + 1)] + cookieArr[(i + 2)];
                            i += 2;
                        }
                        ck.SetCookie(name, value);
                        continue;
                    }
                    i += 1;
                }
            }
        }
        //设置cookie
        private static void SetCookie(this Cookie ck, string name, string value)
        {
            if (ck == null) return;
            var lowername = name.ToLower().Trim();
            if (lowername == "domain" && 
                !string.IsNullOrEmpty(value))
            {
                ck.Domain = value;
            }
            else if (lowername == "path")
            {
                ck.Path = value;
            }
            else if (lowername == "expires")
            {
                ck.Expires = DateTime.MaxValue;
            }
            else if (lowername == "httponly")
            {
                ck.HttpOnly = true;
            }
        }

        private static string FormatValue(string value)
        {
            return HttpUtility.UrlEncode(value,Encoding.UTF8);
        }
    }
}
