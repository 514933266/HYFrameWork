using HYFrameWork.Core;
using HYFrameWork.WinForm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace HYFrameWork.WinForm.Controls
{
    public class IEBrowser : IBrowser
    {
        #region 字段、委托
        private WebBrowser _webBrowser;
        private string _userAgent = string.Empty;
        private delegate void ParamsJavaScriptDelegate(string methord, params object[] pars);
        private delegate void ParamsMethordDelegate(string t, string t1, params object[] pars);
        public IEBrowser(WebBrowser webBrowser)
        {
            _webBrowser = webBrowser;
            _webBrowser.ScriptErrorsSuppressed = true;
        }
        public IEBrowser()
        {
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
                string _cookies = string.Empty;
                _webBrowser.Invoke(new Action(() =>
                {
                    _cookies = GetCookies(_webBrowser.Document.Url.ToString());
                }));
                return _cookies;
            }
            set
            {
                _webBrowser.Invoke(new Action(() =>
                {
                    _webBrowser.Document.Cookie = value;
                }));
            }
        }
        /// <summary>
        /// 获取浏览器加载的页面对象
        /// </summary>
        public object Document
        {
            get
            {
                HtmlDocument _document = null;
                _webBrowser.Invoke(new Action(() =>
                {
                    _document = _webBrowser.Document;
                }));
                return _document;
            }
        }
        /// <summary>
        /// 获取浏览器的HTML页面
        /// </summary>
        public string DocumentText
        {
            get
            {
                string _documentText = string.Empty;
                _webBrowser.Invoke(new Action(() =>
                {
                    _documentText = _webBrowser.DocumentText;
                }));
                return _documentText;

            }
            set
            {
                _webBrowser.Invoke(new Action(() =>
                {
                    _webBrowser.DocumentText = value;
                }));
            }
        }

        public Stream DocumentStream
        {
            get
            {
                Stream documentStream = null;
                _webBrowser.Invoke(new Action(() =>
                {
                    documentStream = _webBrowser.DocumentStream;
                }));
                return documentStream;
            }
            set
            {
                _webBrowser.Invoke(new Action(() =>
                {
                    _webBrowser.DocumentStream = value;
                }));
            }
        }
        /// <summary>
        /// 获取浏览器对象
        /// </summary>
        public object WebBrowser
        {
            get { return _webBrowser; }
            set { _webBrowser = value as WebBrowser; }
        }
        /// <summary>
        /// 浏览器标识
        /// </summary>
        public string UserAgent
        {
            get
            {
                if (string.IsNullOrEmpty(_userAgent))
                {
                    _webBrowser.Invoke(new Action(() =>
                    {
                        object window = _webBrowser.Document.Window.DomWindow;
                        Type wt = window.GetType();
                        object navigator = wt.InvokeMember("navigator", BindingFlags.GetProperty, null, window, new object[] { });
                        Type nt = navigator.GetType();
                        _userAgent = nt.InvokeMember("userAgent", BindingFlags.GetProperty, null, navigator, new object[] { }).ToString();
                    }));
                }
                return _userAgent;
            }
            set
            {
                _userAgent = value;
            }
        }
        public Uri Uri
        {
            get
            {
                Uri uri = null;
                _webBrowser.Invoke(new Action(() =>
                {
                    uri = _webBrowser.Url;
                }));
                return uri;
            }
            set
            {
                _webBrowser.Invoke(new Action(() =>
                {
                    _webBrowser.Url = value;
                }));
            }
        }
        public string Url
        {
            get
            {
                string url = null;
                _webBrowser.Invoke(new Action(() =>
                {
                    url = _webBrowser.Url.ToString();
                }));
                return url;
            }
            set
            {
                _webBrowser.Invoke(new Action(() =>
                {
                    _webBrowser.Url = new Uri(value);
                }));
            }
        }
        /// <summary>
        /// 是否禁止js错误，如果是，则有可能屏蔽其他active窗口
        /// </summary>
        public bool NoJavascriptError
        {
            get
            {
                bool noJavascriptError = true;
                _webBrowser.Invoke(new Action(() =>
                {
                    noJavascriptError = _webBrowser.ScriptErrorsSuppressed;
                }));
                return noJavascriptError;
            }

            set
            {
                _webBrowser.Invoke(new Action(() =>
                {
                    _webBrowser.ScriptErrorsSuppressed = value;
                }));

            }
        }

        public Dictionary<string, string> SelfHeader { get; set; }

        #endregion

        #region 创建新对象
        /// <summary>
        /// 创建一个新的浏览器对象 并绑定页面加载完成方法
        /// </summary>
        /// <param name="form">窗体对象</param>
        /// <param name="completedFunc">加载完成后执行方法</param>
        public void InvokeCreate(Form form,WebBrowserDocumentCompletedEventHandler completedFunc)
        {
            form.Invoke(new Action(() =>
            {
                _webBrowser = new WebBrowser();
                _webBrowser.ScriptErrorsSuppressed = true;
                _webBrowser.DocumentCompleted += completedFunc;
            }));
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
                string header = string.Empty;
                if (SelfHeader != null)
                {
                    foreach (KeyValuePair<string, string> kv in SelfHeader)
                    {
                        header += kv.Key + ":" + kv.Value + "\r\n";
                    }
                    _webBrowser.Navigate(new Uri(u), "_self", null, header);
                }
                else
                {
                    _webBrowser.Navigate(u);
                }
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
                result = _webBrowser.Document.GetElementById(a).InvokeMember(b, c);
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
                result = _webBrowser.Document.InvokeScript(a, b);
            }), methodName, paras);
            return result;
        }
        /// <summary>
        /// 执行自定义js(未实现)
        /// </summary>
        public object InvokeJavaScript(string script)
        {
            object result = null;
            _webBrowser.Invoke(new Action<string>((a) =>
            {
                HtmlElement js = _webBrowser.Document.CreateElement("script");
                js.SetAttribute("type", "text/javascript");
                js.SetAttribute("text", script);
                HtmlElement head = _webBrowser.Document.Body.AppendChild(js);
            }), script);
            return result;
        }
        #endregion

        #region 元素

        /// <summary>
        /// 选中页面下拉框   
        /// </summary>
        /// <param name="id">元素id</param>
        /// <param name="slectedText">selectIndex</param>
        public void InvokeSelectValue(string id, string slectedText)
        {
            _webBrowser.Invoke(new Action<string, string>((a, b) =>
            {
                HtmlElementCollection _options = _webBrowser.Document.GetElementById(a).GetElementsByTagName("option");
                int _index = 0;
                foreach (HtmlElement option in _options)
                {
                    if (option.InnerText == b)
                    {
                        _webBrowser.Document.GetElementById(a).SetAttribute("selectedindex", _index.ToString());
                        InvokeJavaScript(a, "onchange", null);
                    }
                    _index++;
                }
            }), id, slectedText);
        }
        /// <summary>
        /// 选中某个元素内的第一个checkedbox
        /// </summary>
        /// <param name="id">元素id</param>
        public void InvokeCheckedBox(string id)
        {
            _webBrowser.Invoke(new Action<string>((a) =>
            {
                _webBrowser.Document.GetElementById(a).GetElementsByTagName("input")[0].SetAttribute("checked", "true");
            }), id);
        }
        /// <summary>
        /// 点击某段html的A标签元素
        /// </summary>
        public bool InvokeClickA(string innerText)
        {
           return InvokeClickElement("a", innerText);
        }

        /// <summary>
        /// 点击含有指定内容的TagName的第一个元素
        /// </summary>
        /// <param name="tagName">元素TagName</param>
        /// <param name="innerText">元素innerText</param>
        /// <returns></returns>
        public bool InvokeClickElement(string tagName,string innerText)
        {
            HtmlElementCollection hrefList = null;
            _webBrowser.Invoke(new Action(() =>
            {
                hrefList = _webBrowser.Document.GetElementsByTagName(tagName);
            }));
            if (hrefList == null)
            {
                return false;
            }
            HtmlElement resEle = null;
            foreach (HtmlElement ele in hrefList)
            {
                if (!ele.InnerText.IsNullOrEmpty()&&ele.InnerText.Contains(innerText))
                {
                    resEle = ele;
                    break;
                }
            }
            if (resEle != null)
            {
                resEle.InvokeMember("click");
                return true;
            }
            return false;
        }

        /// <summary>
        /// 向某个文本框写入内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public void InvokeInputText(string id, string value)
        {
            _webBrowser.Invoke(new Action<string, string>((a, b) =>
            {
                _webBrowser.Document.GetElementById(a).SetAttribute("value", b);
            }), id, value);
        }
        #endregion

        #region Cookie

        private const int INTERNET_COOKIE_HTTPONLY = 0x00002000;
        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetGetCookieEx(
            string url,
            string cookieName,
            StringBuilder cookieData,
            ref int size,
            int flags,
            IntPtr pReserved);
        /// <summary>
        /// 获取指定链接地址的cookies（当docment.cookie 获取不到httponly类型的cookie时可用此方法）
        /// </summary>
        public string GetCookies(string url)
        {
            int size = 512;
            StringBuilder sb = new StringBuilder(size);
            if (!InternetGetCookieEx(url, null, sb, ref size, INTERNET_COOKIE_HTTPONLY, IntPtr.Zero))
            {
                if (size < 0)
                {
                    return null;
                }
                sb = new StringBuilder(size);
                if (!InternetGetCookieEx(url, null, sb, ref size, INTERNET_COOKIE_HTTPONLY, IntPtr.Zero))
                {
                    return null;
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// 清理当前网站的Cookie
        /// </summary>
        public void ClearCookie()
        {
            if (_webBrowser != null && _webBrowser.Document != null&& _webBrowser.Document.Cookie!=null)
            {
                _webBrowser.Document.Cookie.Remove(0, _webBrowser.Document.Cookie.Length-1);
            }
        }
        public enum ShowCommands : int
        {
            SW_HIDE = 0,
            SW_SHOWNORMAL = 1,
            SW_NORMAL = 1,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMAXIMIZED = 3,
            SW_MAXIMIZE = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,
            SW_RESTORE = 9,
            SW_SHOWDEFAULT = 10,
            SW_FORCEMINIMIZE = 11,
            SW_MAX = 11
        }
        [DllImport("shell32.dll")]
        static extern IntPtr ShellExecute(IntPtr hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, ShowCommands nShowCmd);

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        //写入cookie
        public static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);

        public void ClearAllCookie()
        {
            ShellExecute(IntPtr.Zero, "open", "rundll32.exe", " InetCpl.cpl,ClearMyTracksByProcess 255", "", ShowCommands.SW_HIDE);
        }
        #endregion
    }
}
