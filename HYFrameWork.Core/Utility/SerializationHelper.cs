using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace HYFrameWork.Core
{
    /// <summary>
    /// 帮助类：序列化/反序列化
    /// </summary>
   public static class SerializationHelper
    {
        #region Binary 序列化
        /// <summary>
        /// Binary 序列化对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>数组</returns>
        public static byte[] Serialize(object obj)
        {
            if (obj == null)
                return null;

            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, obj);
                var objectDataAsStream = memoryStream.ToArray();
                return objectDataAsStream;
            }
        }
        /// <summary>
        /// Binary 反序列化对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="stream">对象数组</param>
        /// <returns>对象</returns>
        public static T Deserialize<T>(byte[] stream)
        {
            if (stream == null)
                return default(T);

            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream(stream))
            {
                var result = (T)binaryFormatter.Deserialize(memoryStream);
                return result;
            }
        }

        #endregion
    }
}
