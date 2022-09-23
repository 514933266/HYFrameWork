using Aspose.Words;
using Aspose.Words.Replacing;
using HYFrameWork.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace HYFrameWork.File
{
    /// <summary>
    /// Aspose文档处理
    /// </summary>
    public class AsposeWord:IWordHandler
    {
        #region 网络导出
        /// <summary>
        /// Word模板替换异步导出（插槽请使用{{$Name}})
        /// </summary>
        /// <typeparam name="T">word模板的对象</typeparam>
        /// <param name="fileName">文件名</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="t">赋值对象</param>
        /// <param name="type">导出格式</param>
        /// <param name="formatter">参数格式转换器</param>
        /// <returns></returns>
        public void HttpExport<T>(string fileName, string filePath, T t, FileType type = FileType.docx, IPropertyFormatter formatter = null) where T : new()
        {
            if (System.IO.File.Exists(filePath))
            {
                while (!FileHelper.CanReadWrite(filePath));
                var doc = new Document(filePath);
                var props = ReflectionHelper.GetPropertys(t);
                props.ForEach(p =>
                {
                    if (formatter != null)
                    {
                        doc.Range.Replace("{{$" + p.Name + "}}", formatter.Format(p, t),new FindReplaceOptions() { MatchCase=false,FindWholeWordsOnly=true  });
                    }
                    else
                    {
                        doc.Range.Replace("{{$" + p.Name + "}}", p.GetValue(t).ToString(), new FindReplaceOptions() { MatchCase = false,FindWholeWordsOnly = true });
                    }
                });
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Charset = "UTF-8";
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=\"" + HttpUtility.UrlEncode(fileName, Encoding.UTF8) + (type == FileType.doc ? ".doc" : ".docx") + "\"");
                HttpContext.Current.Response.ContentType = HttpMIMEType.AppWord.ToMIMEString();
                MemoryStream ms = new MemoryStream();
                doc.Save(ms, (type == FileType.doc ? SaveFormat.Doc : SaveFormat.Docx));
                ms.WriteTo(HttpContext.Current.Response.OutputStream);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        #endregion
    }
}
