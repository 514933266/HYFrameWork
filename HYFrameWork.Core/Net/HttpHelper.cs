using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using HYFrameWork.Core;
using System.IO.Compression;

namespace HYFrameWork.Core
{
    /// <summary>
    ///帮助类：Http网络请求
    /// </summary>
    public class HttpHelper
    {
        #region 字段/属性
        private HttpWebResponse _httpResponse;
        private HttpWebRequest  _httpRequest;
        private CookieContainer _cookies;
        private WebHeaderCollection _httpResponseHeader;
        
        /// <summary>
        /// 请求后的Cookie集合
        /// </summary>
        public CookieContainer Cookies
        {
            get
            {
                return _cookies;
            }
            set
            {
                _cookies = value;
            }
        }
        /// <summary>
        /// 响应头
        /// </summary>
        public WebHeaderCollection HttpResponseHeader
        {
            get
            {
                return _httpResponseHeader;
            }
            set
            {
                _httpResponseHeader = value;
            }
        }

        /// <summary>
        /// Http请求
        /// </summary>
        public HttpHelper()
        {
            _cookies = new CookieContainer() { PerDomainCapacity = 99 };

        }
        #endregion

        #region 请求头配置
        private void SetHttpRequest(HttpRequestHeader header)
        {
            if (header == null) { return; }
            _httpRequest = (HttpWebRequest)HttpWebRequest.Create(header.Url);
            _httpRequest.CookieContainer = Cookies;
            _httpRequest.Method = header.Method;
            _httpRequest.Referer = header.Referer;
            _httpRequest.UserAgent = header.UserAgent;
            _httpRequest.Timeout = header.Timeout;
            _httpRequest.ContentType = header.ContentType;
            _httpRequest.KeepAlive = header.KeepAlive;
            _httpRequest.Accept = header.Accept;
            _httpRequest.Headers["Accept-Language"] = header.AcceptLanguage;
            _httpRequest.AllowAutoRedirect = header.AllowAutoRedirect;
            //处理HttpWebRequest访问https有安全证书的问题（ 请求被中止: 未能创建 SSL/TLS 安全通道。）
            ServicePointManager.ServerCertificateValidationCallback += (s, cert, chain, sslPolicyErrors) => true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            //代理
            if (!string.IsNullOrEmpty(header.Proxy))
                _httpRequest.Proxy = new WebProxy(header.Proxy);
            if (_httpRequest.KeepAlive)
                _httpRequest.ServicePoint.Expect100Continue = false;
            //自定义请求头
            if (header.SelfHeader != null)
            {
                header.SelfHeader.ToList().ForEach(kv =>
                {
                    _httpRequest.Headers.Add(kv.Key, kv.Value);
                });
            }
            //证书
            if (!string.IsNullOrEmpty(header.CertPath) && !string.IsNullOrEmpty(header.CertPwd))
                _httpRequest.ClientCertificates.Add(SetCert(header));
        }

        #endregion

        #region Get
        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns>响应值</returns>
        public string Get(string url)
        {
            string content = string.Empty;
            using (MemoryStream ms = Request(new HttpRequestHeader() { Url = url }, null, null) as MemoryStream)
            {
                if (ms != null)
                {
                    byte[] b = ms.ToArray();
                    content = Encoding.UTF8.GetString(b);
                }
            }
            return content;
        }
        /// <summary>
        ///  Get请求 获取json对象
        /// </summary>
        /// <typeparam name="T">json对象类型</typeparam>
        /// <param name="url">请求url</param>
        /// <returns>json对象</returns>
        public T GetJson<T>(string url)
        {
            return Get(url).FromJson<T>();
        }
        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="header">Http请求头</param>
        /// <returns>响应值</returns>
        public string Get(HttpRequestHeader header)
        {
            string content = string.Empty;
            using (MemoryStream ms = Request(header, null, null) as MemoryStream)
            {
                if (ms != null)
                {
                    byte[] b = ms.ToArray();
                    content = header.Encode.GetString(b);
                }
            }
            return content;
        }
        #endregion

        #region Post
        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url">请求url</param>
        /// <param name="postData">post值</param>
        /// <returns>响应值</returns>
        public string Post(string url, string postData)
        {
            string content = string.Empty;
            using (MemoryStream ms = Request(new HttpRequestHeader()
            {
                Url = url,
                PostData = postData,
                Method = "Post"
            }, null, null) as MemoryStream)
            {
                if (ms != null)
                {
                    byte[] b = ms.ToArray();
                    content = Encoding.UTF8.GetString(b);
                }
            }
            return content;
        }
        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="header">Http请求头</param>
        /// <returns>响应值</returns>
        public string Post(HttpRequestHeader header)
        {
            header.Method = "POST";
            string content = string.Empty;
            using (MemoryStream ms = Request(header, null, null) as MemoryStream)
            {
                if (ms != null)
                {
                    byte[] b = ms.ToArray();
                    content = header.Encode.GetString(b);
                }
            }
            return content;
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url">请求url</param>
        /// <param name="parameters">Post参数集合</param>
        /// <returns>响应值</returns>
        public string Post(string url, IDictionary<string, string> parameters = null)
        {
            return Post(url, CreateParameter(parameters));
        }

        /// <summary>
        /// Post请求 获取返回的json
        /// </summary>
        /// <param name="url">请求url</param>
        /// <param name="parameters">Post参数集合</param>
        /// <returns>响应值</returns>
        public T Post<T>(string url, IDictionary<string, string> parameters = null)
        {
            return Post(url, CreateParameter(parameters)).FromJson<T>();
        }

        private string CreateParameter(IDictionary<string, string> parameters)
        {
            if (parameters == null || parameters.Count < 1)
            {
                return string.Empty;
            }
            var builder = new StringBuilder();
            foreach (var str in parameters.Keys)
            {
                builder.AppendFormat("&{0}={1}", str, parameters[str]);
            }
            return builder.ToString().TrimStart('&');
        }
        #endregion

        #region 下载
        /// <summary>
        /// 下载获取响应
        /// </summary>
        /// <param name="header"></param>
        /// <returns>数据字节数组</returns>
        public byte[] DownLoad(HttpRequestHeader header)
        {
            try
            {
                byte[] b = null;
                using (MemoryStream ms = Request(header, null, null) as MemoryStream)
                {
                    if (ms != null)
                    {
                        b = ms.ToArray();
                    }
                }
                return b;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 请求获取二进制流
        /// </summary>
        /// <param name="header">Http请求头</param>
        /// <param name="action">请求时执行的方法</param>
        /// <param name="complete">请求结束后执行的方法</param>
        /// <returns>数据流</returns>
        public Stream Request(HttpRequestHeader header, Action<HttpProgressbar> action = null, Action complete = null)
        {
            Stream ms = null;
            SetHttpRequest(header);
            WriteRequestStream(header);
            ms = WriteDownLoadStream(header, action);
            if (complete != null)
            {
                complete.Invoke();
            }
            return ms;
        }
       
        /// <summary>
        /// 下载文件，并可操作进度条
        /// </summary>
        /// <param name="header">Http请求头</param>
        /// <param name="action">请求时执行的方法</param>
        /// <param name="complete">请求结束后执行的方法</param>
        /// <returns>响应数据流</returns>
        public Stream DownLoad(HttpRequestHeader header, Action<HttpProgressbar> action, Action complete)
        {
            try
            {
                return Request(header, action, complete);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Stream WriteDownLoadStream(HttpRequestHeader header, Action<HttpProgressbar> action)
        {
            int size = 0;
            int offset = 0;
            MemoryStream ms = null;
            DateTime _startTime = DateTime.Now;
            byte[] buffer = new byte[header.BufferLength];
            using (_httpResponse = (HttpWebResponse)_httpRequest.GetResponse())
            {
                GetCookies(header);
                using (var st = _httpResponse.GetResponseStream())
                {
                    ms = new MemoryStream();
                    if (st != null)
                    {
                        if (action != null)
                        {
                            var responseProgressbar = new HttpProgressbar();
                            while ((size = st.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                ms.Write(buffer, 0, size);
                                //更新进度条
                                offset += size;
                                responseProgressbar.TotalByte = _httpResponse.ContentLength;
                                responseProgressbar.TimeSpan = DateTime.Now - _startTime;
                                responseProgressbar.Speed = offset / 1024 / responseProgressbar.TimeSpan.TotalSeconds;
                                responseProgressbar.Byte = offset;
                                action(responseProgressbar);
                            }
                        }
                        else
                        {
                            while ((size = st.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                ms.Write(buffer, 0, size);
                                offset += size;
                            }
                        }
                    }
                }
            }
            return ms;
        }
        private void WriteRequestStream(HttpRequestHeader header)
        {
            if (!string.IsNullOrEmpty(header.PostData))
            {
                if (_httpRequest.Method.ToLower() == "get") { throw new Exception("错误的请求方式:应为Post"); }
                var dataByte = header.Encode.GetBytes(header.PostData);
                _httpRequest.ContentLength = dataByte.Length;
                using (Stream stream = _httpRequest.GetRequestStream())
                {
                    stream.Write(dataByte, 0, dataByte.Length);
                }
            }
        }

        //获取浏览器返回的Cookie
        private void GetCookies(HttpRequestHeader header)
        {
            string domain = string.Empty;
            foreach (Cookie ck in _httpResponse.Cookies)
            {
                domain = ck.Domain;
                ck.Expires = DateTime.MaxValue;
            }
            Cookies.Add(_httpResponse.Cookies);
            if (header.GetSetCookies) GetSetCookies(domain);
            _httpResponseHeader = _httpResponse.Headers;
        }
        //获取Set-Cookie,此举用于某些情况下Httponly的Cookie没有被采集时
        private void GetSetCookies(string domain)
        {
            var ip = _httpRequest.Host.Split(':');
            var cookieStr = _httpResponse.Headers["Set-Cookie"];
            if (!string.IsNullOrEmpty(cookieStr))
            {
                CookieCollection ccn = cookieStr.ToCookieCollection(ip.Length > 1 ? ip[0] : domain ?? _httpRequest.Host.Replace("www", ""));
                if (ccn != null && ccn.Count > 0) Cookies.Add(ccn);
            }
        }
        private Stream GetResponseStream(HttpRequestHeader header)
        {
            int size = 0;
            int offset = 0;
            MemoryStream ms = null;
            DateTime _startTime = DateTime.Now;
            byte[] buffer = new byte[header.BufferLength];
            using (_httpResponse = (HttpWebResponse)_httpRequest.GetResponse())
            {
                GetCookies(header);
                using (var st = _httpResponse.GetResponseStream())
                {
                    ms = new MemoryStream();
                    if (st != null)
                    {
                        if (_httpResponse.ContentEncoding.Contains("gzip"))//压缩流
                        {
                            GZipStream gzip = new GZipStream(st, CompressionMode.Decompress);
                            while ((size = gzip.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                ms.Write(buffer, 0, size);
                                offset += size;
                            }
                        }
                        else
                        {
                            while ((size = st.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                ms.Write(buffer, 0, size);
                                offset += size;
                            }
                        }
                    }
                }
            }
            return ms;
        }
        #endregion

        #region 上传
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="header">请求头</param>
        /// <returns>响应值</returns>
        public string Upload(HttpUploadHeader header)
        {
            string response = string.Empty;
            try
            {
                header.ContentType = HttpMIMEType.Upload.ToMIMEString();
                using (MemoryStream ms = RequestUpload(header, null, null) as MemoryStream)
                {
                    if (ms != null)
                    {
                        byte[] b = ms.ToArray();
                        response = header.Encode.GetString(b);
                    }
                }
            }
            catch (Exception ex)
            {
                response = ex.Message;
            }
            return response;
        }
        /// <summary>
        /// 上传文件(并执行进度条)
        /// </summary>
        /// <param name="header">请求头</param>
        /// <param name="action">上传时执行的方法</param>
        /// <param name="complete">上传完成执行的方法</param>
        /// <returns>响应值</returns>
        public string Upload(HttpUploadHeader header, Action<HttpProgressbar> action, Action complete)
        {
            string response = string.Empty;
            try
            {
                header.ContentType = HttpMIMEType.Upload.ToMIMEString();
                using (MemoryStream ms = RequestUpload(header, action, complete) as MemoryStream)
                {
                    byte[] b = ms.ToArray();
                    response = Encoding.UTF8.GetString(b);
                }
            }
            catch (Exception ex)
            {
                response = ex.Message;
            }
            return response;
        }
        /// <summary>
        /// 请求获取二进制流
        /// </summary>
        /// <param name="header">Http请求头</param>
        /// <param name="action">请求时执行的方法</param>
        /// <param name="complete">请求结束后执行的方法</param>
        /// <returns>数据流</returns>
        public Stream RequestUpload(HttpUploadHeader header, Action<HttpProgressbar> action = null, Action complete = null)
        {
            Stream ms = null;
            SetHttpRequest(header);
            WriteUploadStream(header, action);
            ms = GetResponseStream(header);
            if (complete != null)
            {
                complete.Invoke();
            }
            return ms;
        }

        private void WriteUploadStream(HttpUploadHeader header, Action<HttpProgressbar> action)
        {
            var size = 0;
            var offset = 0;
            var _startTime = DateTime.Now;
            var buffer = new byte[header.BufferLength];
            if (!string.IsNullOrEmpty(header.PostData))
            {
                if (header.Method.ToLower() == "get") { throw new Exception("错误的请求方式:应为Post"); }
                var tcpSYN = GetTCPSYN(header);
                var tcpSYNEnd =Encoding.UTF8.GetBytes("\r\n--" + header.Boundary + "--");
                _httpRequest.ContentType = "{0};boundary={1}".Fmt(header.ContentType,header.Boundary);
                _httpRequest.ContentLength = tcpSYN.Length + header.FileData.Length+ tcpSYNEnd.Length;
                _httpRequest.AllowWriteStreamBuffering = false;
                //执行上传
                var stream = _httpRequest.GetRequestStream();
                stream.Write(tcpSYN, 0, tcpSYN.Length);
                using (MemoryStream ms = new MemoryStream(header.FileData))
                {
                    size = ms.Read(buffer, 0, header.BufferLength);
                    if (action != null)
                    {
                        var requestProgressbar = new HttpProgressbar();
                        while (size > 0)
                        {
                            stream.Write(buffer, 0, size);
                            offset += size;
                            size = ms.Read(buffer, 0, buffer.Length);
                            requestProgressbar.FileName = header.FileName;
                            requestProgressbar.TotalByte = _httpRequest.ContentLength;
                            requestProgressbar.TimeSpan = DateTime.Now - _startTime;
                            requestProgressbar.Speed = offset / 1024 / requestProgressbar.TimeSpan.TotalSeconds;
                            requestProgressbar.Byte = offset;
                            action(requestProgressbar);
                        }
                        action.Invoke(new HttpProgressbar() { FileName = header.FileName, TimeSpan = DateTime.Now - _startTime, Speed = 0, Byte = offset, TotalByte = _httpRequest.ContentLength });
                    }
                    else
                    {
                        while (size > 0)
                        {
                            stream.Write(buffer, 0, size);
                            offset += size;
                            size = ms.Read(buffer, 0, buffer.Length);
                        }
                    }
                }
                stream.Write(tcpSYNEnd, 0, tcpSYNEnd.Length);
                stream.Close();
            }
        }
        private byte[] GetTCPSYN(HttpUploadHeader header)
        {
            //请求报文拼接
            string content = string.Empty;
            Dictionary<string, string> param = GetFormDataTypeParams(header.PostData);
            foreach (KeyValuePair<string, string> kv in param)
            {
                content += "--"+header.Boundary.Append("\r\nContent-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}\r\n".Fmt(kv.Key, kv.Value));
            }
            content += "--" + header.Boundary.Append("\r\nContent-Disposition: form-data; name=\"Filedata\"; filename=\"{0}\"\r\n".Fmt(new FileInfo(header.FileName).Name));
            content += "Content-Type: {0}\r\n\r\n".Fmt(header.UploadContentType);
            return Encoding.UTF8.GetBytes(content);
        }

        //将postData字符串形式转换成字典
        private Dictionary<string, string> GetFormDataTypeParams(string postData)
        {
            string[] arr = null;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string[] kvArr = postData.Split('&');
            if (kvArr != null)
            {
                kvArr.ForEach(kv =>
                {
                    arr = kv.Split('=');
                    if (arr != null && arr.Length > 1) dic.Add(arr[0], arr[1]);
                });
            }
            return dic;
        }

        #endregion

        #region 图像
        /// <summary>
        /// 请求指定地址获取指定大小图像
        /// </summary>
        /// <param name="url">请求的url</param>
        /// <param name="width">图像宽度</param>
        /// <param name="height">图像高度</param>
        /// <returns>图像</returns>
        public Bitmap RequestBitmap(string url, int width, int height)
        {
            return RequestBitmap(url, width, height, width, height, 0, 0);
        }
        /// <summary>
        /// 请求获取图像
        /// </summary>
        /// <param name="url">请求的url</param>
        /// <param name="bmWidth">设置画布的宽度</param>
        /// <param name="bmHeight">设置画布的高度</param>
        /// <param name="width">设置图片的宽度</param>
        /// <param name="height">设置图片的高度</param>
        /// <param name="x">设置图片相对于画布的x轴位置</param>
        /// <param name="y">设置图片相对于画布的y轴位置</param>
        /// <returns>图像</returns>
        public Bitmap RequestBitmap(string url, int bmWidth, int bmHeight, int width, int height, int x, int y)
        {
            return RequestBitmap(new HttpRequestHeader()
            {
                Url = url,
                ContentType ="*/image",
                GetSetCookies=true
            }, bmWidth, bmHeight, width, height, x, y);
        }

        /// <summary>
        /// 请求获取图像
        /// </summary>
        /// <param name="header">HTTP请求头信息对象</param>
        /// <param name="bmWidth">设置画布的宽度</param>
        /// <param name="bmHeight">设置画布的高度</param>
        /// <param name="width">设置图片的宽度</param>
        /// <param name="height">设置图片的高度</param>
        /// <param name="x">设置图片相对于画布的x轴位置</param>
        /// <param name="y">设置图片相对于画布的y轴位置</param>
        /// <returns>图像</returns>
        public Bitmap RequestBitmap(HttpRequestHeader header, int bmWidth, int bmHeight, int width, int height, int x, int y)
        {
            try
            {
                var bp = new Bitmap(bmWidth, bmHeight); ;
                using (MemoryStream ms = Request(header, null, null) as MemoryStream)
                {
                    var g = Graphics.FromImage(bp);
                    g.DrawImage(Image.FromStream(ms), x, y, width, height);
                }
                Bitmap bp2= bp.Clone() as Bitmap;
                bp.Dispose();
                return bp2;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 证书
        private X509Certificate SetCert(HttpRequestHeader header)
        {
            ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
            X509Certificate cer = new X509Certificate(header.CertPath, header.CertPwd);
            //该部分是关键，若没有该部分则在IIS下会报 CA证书出错
            X509Certificate2 certificate = new X509Certificate2(header.CertPath, header.CertPwd);
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadWrite);
            store.Remove(certificate);   //可省略
            store.Add(certificate);
            store.Close();
            return cer;
        }
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            return false;
        }
        #endregion

    }
}