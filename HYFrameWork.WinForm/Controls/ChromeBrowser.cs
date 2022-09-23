using HYFrameWork.Core;
using System;
using System.Collections.Generic;
using System.IO;
using WebKit;
using WebKit.DOM;

namespace HYFrameWork.WinForm.Controls
{
    /// <summary>
    /// 谷歌内核浏览器
    /// </summary>
    public class ChromeBrowser : IBrowser
    {
        #region 字段、委托
        string _url = string.Empty;
        WebKitBrowser _webBrowser;
        delegate void ParamsJavaScriptDelegate(string methord, params object[] pars);
        delegate void ParamsMethordDelegate(string t, string t1, params object[] pars);
        public ChromeBrowser(WebKitBrowser webBrowser)
        {
            _webBrowser = webBrowser;
        }
        #endregion

        #region 属性
        /// <summary>
        /// Cookies
        /// </summary>
        public string Cookies
        {
            get
            {
                string cookies = string.Empty;
                _webBrowser.Invoke(new Action(() =>
                {
                    cookies = _webBrowser.StringByEvaluatingJavaScriptFromString("function GetCookies(){ return document.body;};GetCookies();");
                }));
                return cookies;
            }
            set
            {

            }
        }
        /// <summary>
        /// 获取浏览器加载的页面对象
        /// </summary>
        public object Document
        {
            get
            {
                Document document = null;
                _webBrowser.Invoke(new Action(() =>
                {
                    document = _webBrowser.Document;
                }));
                return document;
            }
        }
        public Stream DocumentStream
        {
            get
            {
                return null;
            }
            set
            {
            }
        }
        /// <summary>
        /// 获取浏览器的HTML页面
        /// </summary>
        public string DocumentText
        {
            get
            {
                string documentText = string.Empty;
                _webBrowser.Invoke(new Action(() =>
                {
                    documentText = _webBrowser.DocumentText;
                }));
                return documentText;
            }
            set
            {
                _webBrowser.Invoke(new Action(() =>
                {
                    _webBrowser.DocumentText = value;
                }));
            }
        }

        /// <summary>
        /// 获取浏览器对象
        /// </summary>
        public object WebBrowser
        {
            get { return _webBrowser; }
            set { _webBrowser = value as WebKitBrowser; }
        }
        /// <summary>
        /// 浏览器标识
        /// </summary>
        public string UserAgent
        {
            get
            {
                string userAgent = string.Empty;
                _webBrowser.Invoke(new Action(() =>
                {
                    userAgent = _webBrowser.UserAgent;
                }));
                return userAgent;
            }
            set
            {
                _webBrowser.Invoke(new Action(() =>
                {
                    _webBrowser.UserAgent = value;
                }));
            }
        }

        public string Url
        {
            get
            {
                _webBrowser.Invoke(new Action(() =>
                {
                    if (_webBrowser.Url==null)
                    {
                        _url = _webBrowser.Url.ToString();
                    }
                }));
                return _url;
            }
            set
            {
                InvokeNavigate(value);
            }
        }
        /// <summary>
        /// 自定义请求头
        /// </summary>
        public Dictionary<string, string> SelfHeader { get; set; }

        /// <summary>
        /// 是否禁止js，如果是，则有可能屏蔽其他active窗口
        /// </summary>
        public bool NoJavascriptError
        {

            get
            {
                bool enable = false;
                _webBrowser.Invoke(new Action(() =>
                {
                    enable = _webBrowser.IsScriptingEnabled;
                }));
                return enable;

            }
            set
            {
                _webBrowser.Invoke(new Action(() =>
                {
                    _webBrowser.IsScriptingEnabled = value;
                }));
            }
        }
        #endregion

        #region 元素
        /// <summary>
        /// 获取具有某个id的元素
        /// </summary>
        public Element GetElementById(string id)
        {
            Element e = null;
            _webBrowser.Invoke(new Action<string>(u =>
            {
                e = _webBrowser.Document.GetElementById(id);
            }), id);
            return e;
        }
        #endregion

        #region 跳转、页面
        /// <summary>
        ///跳转链接
        /// </summary>
        /// <param name="url"></param>
        public void InvokeNavigate(string url)
        {
            _webBrowser.Invoke(new Action<string>(u =>
            {
                _webBrowser.Navigate(u);
            }), url);
        }
        #endregion

        #region Js
        /// <summary>
        /// 执行页面的元素的js方法
        /// </summary>
        public object InvokeElementJavaScript(string id, string methord, params object[] paras)
        {
            object result = null;
            _webBrowser.Invoke(new ParamsMethordDelegate((a, b, c) =>
            {
                result = _webBrowser.StringByEvaluatingJavaScriptFromString("var _elm = document.getElementById('" + id + "');var _evt = document.createEvent('MouseEvents');_evt.initEvent('" + methord + "', true, true);_elm.dispatchEvent(_evt);");
            }), id, methord, paras);
            return result;
        }
        /// <summary>
        /// 执行页面的js
        /// </summary>
        public object InvokeJavaScript(string methodName, params object[] paras)
        {
            object result = null;
            _webBrowser.Invoke(new ParamsJavaScriptDelegate((a, b) =>
            {
                result = _webBrowser.Document.InvokeScriptMethod(a, b);

            }), methodName, paras);
            return result;
        }

        /// <summary>
        /// 执行自定义js
        /// </summary>
        public object InvokeJavaScript(string script)
        {
            object result = null;
            _webBrowser.Invoke(new Action<string>((a) =>
            {
                result = _webBrowser.StringByEvaluatingJavaScriptFromString(a);
            }), script);
            return result;
        }
        /// <summary>
        /// 调用某个元素的点击事件
        /// </summary>
        public void InvokeClick(string id)
        {
            _webBrowser.Invoke(new Action<string>((a) =>
            {
                _webBrowser.StringByEvaluatingJavaScriptFromString("var _elm = document.getElementById('" + id + "');var _evt = document.createEvent('MouseEvents');_evt.initEvent('click', true, true);_elm.dispatchEvent(_evt);");
            }), id);
        }

        #endregion

        #region 元素
        /// <summary>
        /// 向某个文本框写入内容
        /// </summary>
        public void InvokeSetValue(string id, string value)
        {
            InvokeJavaScript("document.getElementById('" + id + "').value='{0}';".Fmt(value));
        }
        /// <summary>
        /// 获取某个元素值
        /// </summary>
        public object InvokeGetValue(string id)
        {
            return InvokeJavaScript("reutrn document.getElementById('" + id + "').value;");
        }
        /// <summary>
        /// 设置某个元素的值
        /// </summary>
        /// <param name="id">元素id</param>
        /// <param name="attribute">属性名</param>
        /// <param name="value">值</param>
        public void InvokeSetAttribute(string id, string attribute, string value)
        {
            _webBrowser.Invoke(new Action<string, string, string>((a, b, c) =>
              {
                  _webBrowser.Document.GetElementById(a).SetAttribute(b, c);
              }), id, attribute, value);
        }

        #endregion

        #region Cookie
        
        #endregion
    }
}
