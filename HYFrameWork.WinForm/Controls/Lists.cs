using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace HYFrameWork.WinForm.Controls
{
    /// <summary>
    /// List对象操作类
    /// </summary>
   public class Lists
    {
       /// <summary>
        /// 用序列化的方式对引用对象完成深拷贝，此种方法最可靠
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="RealObject"></param>
       /// <returns></returns>
       public static T Clone<T>(T RealObject)
       {
           using (Stream objectStream = new MemoryStream())
           {
               //利用 System.Runtime.Serialization序列化与反序列化完成引用对象的复制  
               IFormatter formatter = new BinaryFormatter();
               formatter.Serialize(objectStream, RealObject);
               objectStream.Seek(0, SeekOrigin.Begin);
               return (T)formatter.Deserialize(objectStream);
           }
       }
       /// <summary>
       /// 利用System.Xml.Serialization来实现序列化与反序列化
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="RealObject"></param>
       /// <returns></returns>
       public static T Clone_ForXml<T>(T RealObject)
       {
           using (Stream stream = new MemoryStream())
           {
               XmlSerializer serializer = new XmlSerializer(typeof(T));
               serializer.Serialize(stream, RealObject);
               stream.Seek(0, SeekOrigin.Begin);
               return (T)serializer.Deserialize(stream);
           }
       }  

    }
}
