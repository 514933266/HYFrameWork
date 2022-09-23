using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace HYFrameWork.Core
{
    /// <summary>
    /// 帮助类：xml操作
    /// </summary>
    public abstract class XMLHelper
    {
        private static void SerializeInternal(Stream stream, object o, Encoding encoding)
        {
            if (o == null)
                throw new ArgumentNullException("o");
            if (encoding == null)
                throw new ArgumentNullException("encoding");
            var serializer = new XmlSerializer(o.GetType());
            var settings = new XmlWriterSettings
            {
                Indent = true,
                NewLineChars = "\r\n",
                Encoding = encoding,
                IndentChars = "    "
            };
            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, o);
                writer.Close();
            }
        }
        /// <summary>
        /// 将一个对象序列化为XML字符串
        /// </summary>
        /// <param name="o">对象</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>结果值</returns>
        public static string Serialize(object o, Encoding encoding)
        {
            using (var stream = new MemoryStream())
            {
                SerializeInternal(stream, o, encoding);
                stream.Position = 0;
                using (var reader = new StreamReader(stream, encoding))
                {
                    return reader.ReadToEnd();
                }
            }
        }
        /// <summary>
        /// 将一个对象按XML序列化的方式写入到一个文件
        /// </summary>
        /// <param name="o">对象</param>
        /// <param name="path">保存路径</param>
        /// <param name="encoding">编码格式</param>
        public static void SerializeToFile(object o, string path, Encoding encoding)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");
            using (var file = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                SerializeInternal(file, o, encoding);
            }
        }
        /// <summary>
        /// 从XML字符串中反序列化对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="s">xml字符串</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>对象</returns>
        public static T Deserialize<T>(string s, Encoding encoding) where T : new()
        {
            try
            {
                using (StringReader sr = new StringReader(s))
                {
                    var xmldes = new XmlSerializer(typeof(T));
                    return (T)xmldes.Deserialize(sr);
                }
            }
            catch
            {
               return new T();
            }
        }
        /// <summary>
        /// 读入一个文件，并按XML的方式反序列化对象。
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="path">保存路径</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>对象值</returns>
        public static T DeserializeFromFile<T>(string path, Encoding encoding) where T : new()
        {
            var xml = string.Empty;
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");
            if (encoding == null)
                throw new ArgumentNullException("encoding");
            if (File.Exists(path))
                xml = File.ReadAllText(path, encoding);
            return Deserialize<T>(xml, encoding);
        }
    }
}
