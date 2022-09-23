using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYFrameWork.WinForm.Controls
{

    /// <summary>
    /// 浏览器接口
    /// </summary>
   public interface IBrowser
    {
        #region 属性
        /// <summary>
        /// Cookies
        /// </summary>
        string Cookies{get;set;}
        /// <summary>
        /// 获取浏览器加载的页面对象
        /// </summary>
        object Document{get;}
        /// <summary>
        /// 获取浏览器的HTML页面
        /// </summary>
         string DocumentText { get; set; }
        /// <summary>
        /// 浏览器的内容流
        /// </summary>
        Stream DocumentStream { get; set; }
        /// <summary>
        /// 获取浏览器对象
        /// </summary>
        object WebBrowser { get; set; }
        /// <summary>
        /// 浏览器标识
        /// </summary>
        string UserAgent { get; set; }
        string Url { get; set; }
        /// <summary>
        /// 是否禁止js错误，如果是，则有可能屏蔽其他active窗口
        /// </summary>
        bool NoJavascriptError { get; set; }

        Dictionary<string, string> SelfHeader { get; set; }

        #endregion

        #region 跳转、页面
        /// <summary>
        ///跳转链接
        /// </summary>
        void InvokeNavigate(string url);

        #endregion

        #region Js
        /// <summary>
        /// 执行页面的元素的js方法
        /// </summary>
        object InvokeElementJavaScript(string id, string methodName, params object[] paras);
        /// <summary>
        /// 执行页面的js
        /// </summary>
        object InvokeJavaScript(string methodName, params object[] paras);

        /// <summary>
        /// 执行自定义js
        /// </summary>
        object InvokeJavaScript(string script);
        #endregion
    }
}
