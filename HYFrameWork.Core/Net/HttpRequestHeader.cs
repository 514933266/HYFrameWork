using System;
using System.Collections.Generic;
using System.Text;

namespace HYFrameWork.Core
{
    /// <summary>
    /// 实体：Http请求头
    /// </summary>
    public class HttpRequestHeader
    {

        #region 字段、属性
        /// <summary>
        /// 证书路径
        /// </summary>
        public string CertPath { get; set; }
        /// <summary>
        /// 证书密码
        /// </summary>
        public string CertPwd { get; set; }
        /// <summary>
        /// 编码解码类型
        /// </summary>
        public Encoding Encode { get; set; } = Encoding.UTF8;
        /// <summary>
        /// 请求时的缓冲区，会影响上传或下载的速度
        /// </summary>
        public int BufferLength { get; set; } = 1024;
        /// <summary>
        /// Post的请求报文内容 Get请求时此属性可为空
        /// </summary>
        public string PostData { get; set; }
        /// <summary>
        /// 是否跟随重定向响应 默认false
        /// </summary>
        public bool AllowAutoRedirect { get; set; } = true;
        /// <summary>
        /// 语言类型 默认zh-CN
        /// </summary>
        public string AcceptLanguage { get; set; } = "zh-CN,zh;q=0.8,en-US;q=0.5,en;q=0.3";
        /// <summary>
        /// 请求方式 GET/POST/HEAD等等 默认GET
        /// </summary>
        public string Method { get; set; } = "Get";
        /// <summary>
        /// 请求地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 获取或设置 User-agentHTTP标头的值
        /// </summary>
        public string UserAgent { get; set; }= "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:59.0) Gecko/20100101 Firefox/59.0";
        /// <summary>
        /// 设置请求超时时间 非必须
        /// </summary>
        public int Timeout { get; set; } = 20000;
        /// <summary>
        /// 获取或设置请求的来源地址（发起请求的页面地址）
        /// </summary>
        public string Referer { get; set; }


        public string _contentType = string.Empty;
        /// <summary>
        /// 设置连接方式 
        /// POST application/x-www-form-urlencoded
        /// GET 
        /// </summary>
        public string ContentType
        {
            get
            {
                if (!string.IsNullOrEmpty(_contentType))
                    return _contentType;
                else
                {
                    if (Method == "GET")
                        return HttpMIMEType.All.ToMIMEString();
                    else
                        return HttpMIMEType.AppForm.ToMIMEString();
                }
            }
            set { _contentType = value; }
        }
        /// <summary>
        /// 传输接收的数据类型 非必须 默认接收所有类型
        /// </summary>
        public string Accept { get; set; }= HttpMIMEType.All.ToMIMEString();
        /// <summary>
        /// 是否持续连接 默认true
        /// </summary>
        public bool KeepAlive { get; set; } = false;
        
        /// <summary>
        /// 自定义的Http请求报文头键值对
        /// </summary>
        public Dictionary<string, string> SelfHeader { get; set; }
        /// <summary>
        /// 代理ip的端口
        /// </summary>
        public string Proxy { get; set; }

        /// <summary>
        /// 是否通过响应头获取Cookies
        /// </summary>
        public bool GetSetCookies { get; set; } = true;
        /// <summary>
        /// 此值设置为false可以避免Post数据时出现2次请求判定
        /// </summary>
        public bool Expect100Continue { get; set; } = false;

        /// <summary>
        ///  true 允许对发送到 Internet 资源的数据进行缓冲处理
        ///  为true时不可发送大数据，否则可能造成内存溢出
        /// </summary>
        public bool AllowWriteStreamBuffering { get; set; } = true;
        #endregion
    }
}
