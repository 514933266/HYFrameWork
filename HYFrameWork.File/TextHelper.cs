using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HYFrameWork.File
{
    /// <summary>
    /// 帮助类：文本
    /// </summary>
    public abstract class TextHelper
   {
        #region 写
        /// <summary>
        /// 将文本写入流文件,如果文件存在则覆盖
        /// </summary>
        /// <param name="fileName">文件绝对路径</param>
        /// <param name="content">要写入的内容</param>
        public static void Write(string fileName, string content)
        {
            CreateText(fileName, true);
            using (StreamWriter streamWriter = new StreamWriter(fileName, false))
            {
                foreach (var line in content)
                {
                    streamWriter.Write(line);
                }
            }
        }
        /// <summary>
        /// 将文本写入txt文件,如果文件存在则覆盖
        /// </summary>
        /// <param name="fileName">文件绝对路径</param>
        /// <param name="content">要写入的内容</param>
        /// <param name="encoding">编码格式</param>
        public static void Write(string fileName, string content, Encoding encoding)
        {
            CreateText(fileName, true);
            using (StreamWriter streamWriter = new StreamWriter(fileName, false, encoding))
            {
                streamWriter.Write(content);
            }
        }
        /// <summary>
        /// 将文本追加写入txt文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
        /// <param name="encoding"></param>
        public static void WriteTo(string fileName, string content, Encoding encoding)
        {
            FileStream fs = null;
            Encoding encoder = (encoding == null ? Encoding.Default : encoding);
            byte[] bytes = encoder.GetBytes(content);
            try
            {
                CreateText(fileName, false);
                if (FileHelper.CanReadWrite(fileName))
                {
                    fs = System.IO.File.OpenWrite(fileName);
                    fs.Position = fs.Length;
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                fs.Close();
            }
        }
        /// <summary>
        /// 创建一个新的txt格式文本,如果文本已经存在则覆盖
        /// </summary>
        /// <param name="fileName">文件绝对路径</param>
        /// <param name="recover">是否覆盖源文件</param>
        public static void CreateText(string fileName, bool recover)
        {
            FileHelper.CreateParentDirectory(fileName);
            if (!System.IO.File.Exists(fileName))
            {
                System.IO.File.CreateText(fileName).Close();
            }
            else
            {
                bool b = false;
                while (!b)
                {
                    b = FileHelper.CanReadWrite(fileName);
                }
                if (b && recover)
                    System.IO.File.WriteAllText(fileName, "");
            }

        }
        #endregion

        #region 读取
        /// <summary>
        /// 读取本地文本内容
        /// </summary>
        /// <param name="path">读取路径</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>读取的文本内容</returns>
        public static string Read(string path, Encoding encoding=null)
       {
           string content = string.Empty;
            ReadLine(path, encoding).ForEach(line =>
            {
                content += line+"\r\n";
            });
            return content;
        }
        /// <summary>
        /// 读取本地文本内容
        /// </summary>
        /// <param name="path">读取路径</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>读取的文本内容</returns>
        public static string ReadAllText(string path, Encoding encoding=null)
        {
            if (FileHelper.Exists(path))
            {
                encoding = encoding ?? Encoding.Default;
                return System.IO.File.ReadAllText(path, encoding);
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 读取本地文本行集合
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>文本集合</returns>
        public static List<string> ReadLine(string path, Encoding encoding)
        {
            string line=string.Empty;
            encoding=encoding ?? Encoding.Default;
            List<string> content = new List<string>();
            if (FileHelper.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path, encoding))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        content.Add(line);
                    }
                }
            }
            return content;
        }
        #endregion

        #region 其他
        /// <summary>
        /// 校验文件是否为文本文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>结果</returns>
        public static bool CheckIsText(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)||!System.IO.File.Exists(fileName)) return false;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                bool isTextFile = true;
                try
                {
                    int i = 0;
                    int length = (int)fs.Length;
                    byte data;
                    while (i < length && isTextFile)
                    {
                        data = (byte)fs.ReadByte();
                        isTextFile = (data != 0);
                        i++;
                    }
                    return isTextFile;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                    }
                }
        }
        /// <summary>
        /// 检查是否为文本类型文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>结果</returns>
        public bool CheckIsCurrentType(string filePath)
        {
            byte[] _fb = FileHelper.ReadByte(filePath, 2);
            if (_fb != null && _fb.Length > 1)
            {
                int _i = Convert.ToInt32(_fb[0].ToString() + _fb[1].ToString());
                switch (_i)
                {
                    case 5150: return true;
                    case 239187: return true;
                    case 4946: return true;
                    case 104116: return true;
                    default: return false;
                }
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
