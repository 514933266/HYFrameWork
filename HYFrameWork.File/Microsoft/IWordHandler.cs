using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYFrameWork.File
{
    /// <summary>
    /// 接口：Word处理
    /// </summary>
    public interface IWordHandler
    {
        /// <summary>
        /// Word模板替换导出
        /// </summary>
        /// <typeparam name="T">word模板的对象</typeparam>
        /// <param name="fileName">文件名</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="t">赋值对象</param>
        /// <param name="type">导出格式</param>
        /// <param name="formatter">参数格式转换器</param>
        void HttpExport<T>(string fileName, string filePath,T t,FileType type = FileType.docx, IPropertyFormatter formatter = null) where T : new();

    }
}
