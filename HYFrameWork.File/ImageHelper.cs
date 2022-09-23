using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace HYFrameWork.File
{
    /// <summary>
    /// 帮助类：图片帮助方法
    /// </summary>
    public abstract class ImageHelper
    {

        #region 保存图片流到指定位置（默认jpg）
        /// <summary>
        /// 保存图片流到指定位置（默认jpg）
        /// </summary>
        /// <param name="path">绝对路径</param>
        /// <param name="s">文件流</param>
        public static void Write(string path, Stream s)
        {
            Save(path, s, null);
        }
        #endregion

        #region 保存指定格式图片流到指定位置
        /// <summary>
        /// 保存指定格式图片流到指定位置
        /// </summary>
        /// <param name="path">绝对路径</param>
        /// <param name="s">图片流</param>
        /// <param name="format">要保存的格式</param>
        public static void Write(string path, Stream s, ImageFormat format)
        {
            Save(path, s, format);
        }
        #endregion

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="s">数据流</param>
        /// <param name="format">文件格式</param>
        private static void Save(string path, Stream s, ImageFormat format)
        {
            if (s == null) { return; }
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
            using (Bitmap bp = new Bitmap(s))
            {
                if (format != null)
                    bp.Save(path, format);
                else
                    bp.Save(path);
            }
        }
        #region 图片对象转二进制对象
        /// <summary>
        /// 图片对象转二进制对象
        /// </summary>
        /// <param name="img">图像</param>
        /// <returns>二进制图象</returns>
        public byte[] ImageToByte(Bitmap img)
        {
            byte[] bt = null;
            if (img != null)
            {
                using (var mostream = new MemoryStream())
                {
                    img.Save(mostream, ImageFormat.Jpeg);//将图像以指定的格式存入缓存内存流
                    bt = new byte[mostream.Length];
                    mostream.Position = 0;//设置留的初始位置
                    mostream.Read(bt, 0, Convert.ToInt32(bt.Length));
                }

            }
            return bt;
        } 
        #endregion

        #region 二进制转图片对象
        /// <summary>
        /// 二进制转图片对象
        /// </summary>
        /// <param name="bytes">二进制图片</param>
        /// <returns>图像</returns>
        public System.Drawing.Image ByteToImage(byte[] bytes)
        {
            System.Drawing.Image photo = null;
            using (var ms = new MemoryStream(bytes))
            {
                ms.Write(bytes, 0, bytes.Length);
                photo = System.Drawing.Image.FromStream(ms, true);
            }
            return photo;
        }
        #endregion

        #region 图片对象转内容流对象

        /// <summary>
        /// 图片对象转内容流对象
        /// </summary>
        /// <param name="img">图像</param>
        /// <returns>数据流</returns>

        public MemoryStream ImageToStream(Bitmap img)
        {
            var mostream = new MemoryStream();
            img.Save(mostream, ImageFormat.Jpeg);
            return mostream;
        } 
        #endregion
    }
}
