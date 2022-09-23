using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYFrameWork.Core
{
    /// <summary>
    /// 使用率比较高的ContentType和Accept
    /// </summary>
    public enum HttpMIMEType
    {
        /// <summary>
        /// text/html
        /// </summary>
        Html,
        /// <summary>
        /// text/plain
        /// </summary>
        Text,
        /// <summary>
        /// text/xml
        /// </summary>
        Xml,
        /// <summary>
        /// image/gif
        /// </summary>
        Gif,
        /// <summary>
        /// image/jpg
        /// </summary>
        Jpg,
        /// <summary>
        /// image/png
        /// </summary>
        Png,
        /// <summary>
        /// image/*
        /// </summary>
        Img,
        /// <summary>
        /// text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8
        /// </summary>
        All,
        /// <summary>
        /// application/xhtml+xml
        /// </summary>
        AppXHtml,
        /// <summary>
        /// application/xml
        /// </summary>
        AppXML,
        /// <summary>
        /// application/atom+xml
        /// </summary>
        AppAtomXML,
        /// <summary>
        /// application/json
        /// </summary>
        AppJson,
        /// <summary>
        /// application/pdf
        /// </summary>
        AppPdf,
        /// <summary>
        /// application/msword
        /// </summary>
        AppWord,
        /// <summary>
        /// application/octet-stream
        /// </summary>
        AppStream,
        /// <summary>
        /// application/x-www-form-urlencoded
        /// </summary>
        AppForm,
        /// <summary>
        /// multipart/form-data
        /// </summary>
        Upload,
        /// <summary>
        /// application/vnd.ms-excel
        /// </summary>
        Excel
    }
    /// <summary>
    /// Http枚举类
    /// </summary>
    public static class HttpEnum
    {
        /// <summary>
        /// 把AcceptType或者ContentType转化为string格式
        /// </summary>
        /// <param name="acceptType">使用率比较高的ContentType和Accept</param>
        /// <returns>类型的字符串值</returns>
        public static string ToMIMEString(this HttpMIMEType acceptType)
        {
            string str = string.Empty;
            switch (acceptType)
            {
                case HttpMIMEType.Html:     str = "text/html";                      break;
                case HttpMIMEType.Text:     str = "text/plain";                     break;
                case HttpMIMEType.Xml:      str = "text/xml";                       break;
                case HttpMIMEType.Gif:      str = "image/gif";                      break;
                case HttpMIMEType.Jpg:      str = "image/jpeg";                     break;
                case HttpMIMEType.Png:      str = "image/png";                      break;
                case HttpMIMEType.Img:      str = "image/*";                        break;
                case HttpMIMEType.AppXHtml: str = "application/xhtml+xml";          break;
                case HttpMIMEType.AppXML:   str = "application/xml";                break;
                case HttpMIMEType.AppAtomXML:str = "application/atom+xml";          break;
                case HttpMIMEType.AppJson:  str = "application/json";               break;
                case HttpMIMEType.AppPdf:   str = "application/pdf";                break;
                case HttpMIMEType.AppWord:  str = "application/msword";             break;
                case HttpMIMEType.AppStream:str = "application/octet-stream";       break;
                case HttpMIMEType.AppForm:  str = "application/x-www-form-urlencoded";break;
                case HttpMIMEType.Upload:   str = "multipart/form-data";            break;
                case HttpMIMEType.Excel:    str = "application/vnd.ms-excel";       break;
                default:
                    str = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                    break;
            }
            return str;
        }
    }
}
