using HYFrameWork.Core;
using NPOI.XWPF.UserModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;

namespace HYFrameWork.File
{
    /// <summary>
    /// Word文档操作
    /// </summary>
    public class NPOIWord : IWordHandler
    {

        /// <summary>
        /// 网络导出Word文件（模型赋值,插槽请用{{$Name}}格式）
        /// </summary>
        /// <typeparam name="T">word模板的对象</typeparam>
        /// <param name="fileName">文件名</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="t">赋值对象</param>
        /// <param name="type">导出格式</param>
        /// <param name="formatter">参数格式转换器</param>
        public void HttpExport<T>(string fileName, string filePath, T t,FileType type=FileType.docx, IPropertyFormatter formatter = null) where T : new()
        {
            if (System.IO.File.Exists(filePath))
            {
                var word = Read(filePath, t);
                if (word == null) return;
                var extension = (type == FileType.doc ? ".doc" : ".docx");
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Charset = "UTF-8";
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=\"" + HttpUtility.UrlEncode(fileName, Encoding.UTF8) + extension + "\"");
                HttpContext.Current.Response.ContentType = HttpMIMEType.AppWord.ToMIMEString();
                word.Write(HttpContext.Current.Response.OutputStream);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        private static XWPFDocument Read<T>(string filePath,T t, IPropertyFormatter formatter = null)
        {
            var stream = FileHelper.ReadStream(filePath);
            
            XWPFDocument doc = new XWPFDocument(stream);
            if (doc != null && t != null)
            {
                var props = ReflectionHelper.GetPropertys(t);
                // 段落替换
                foreach (var para in doc.Paragraphs)
                {
                    ReplaceParams(para, props, t, formatter);
                }
                // 表格替换
                var tables = doc.Tables;
                foreach (var table in tables)
                {
                    foreach (var row in table.Rows)
                    {
                        foreach (var cell in row.GetTableCells())
                        {
                            foreach (var para in cell.Paragraphs)
                            {
                                ReplaceParams(para, props, t, formatter);
                            }
                        }
                    }
                }
            }
            return doc;
        }
        // 替换word模板内容
        private static void ReplaceParams(XWPFParagraph para, PropertyInfo[] props, object t, IPropertyFormatter formatter = null)
        {
            var text = para.ParagraphText;
            para.Runs.ForEach(r =>
            {
                text = r.ToString();
                props.ForEach(p =>
                {
                    if (text.Contains("{{$" + p.Name + "}}"))
                    {
                        if (formatter != null)
                        {
                            text = text.Replace("{{$" + p.Name + "}}", formatter.Format(p, t));
                        }
                        else
                        {
                            text = text.Replace("{{$" + p.Name + "}}", p.GetValue(t).ToString());
                        }
                        r.SetText(text,0);
                    }
                });
            });
        }
    }
}
