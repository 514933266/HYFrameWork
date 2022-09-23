using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using ThoughtWorks.QRCode.Codec;

namespace HYFrameWork.Image
{
   /// <summary>
    /// 本类用于生成二维码,后续可添加扩展,使用本类需引用ThoughtWorks.QRCode.dll
   /// </summary>
   public class QRcode
    {
        #region 2016/05/16
        /// <summary>
        /// 二维码生成（中心无图片）
        /// </summary>
        /// <param name="x">二维码相对画布的位置x</param>
        /// <param name="y">二维码相对画布的位置y</param>
        /// <param name="code">二维码中放的数据（网页地址等）</param>
        /// <param name="size">二维码的尺寸</param>
        public Bitmap GetQRCode(string code, int x, int y, int size)
        {
            QRCodeEncoder qrEntity = new QRCodeEncoder();
            qrEntity.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;//二维码编码方式
            qrEntity.QRCodeScale = 8;//每个小方格的宽度
            qrEntity.QRCodeVersion = 5;//二维码版本号
            qrEntity.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;//纠错码等级
            System.Drawing.Bitmap srcimage;
            //动态调整二维码版本号,上限40，过长返回空白图片，编码后字符最大字节长度2953
            while (true)
            {
                try
                {
                    srcimage = qrEntity.Encode(code, System.Text.Encoding.UTF8);
                    break;
                }
                catch
                {
                    if (qrEntity.QRCodeVersion < 40)
                    {
                        int i = qrEntity.QRCodeVersion;
                    }
                    else
                    {
                        srcimage = new Bitmap(size, size);
                        break;
                    }
                }
            }
            //调整二维码图像大小输出
            Bitmap bitmap = new Bitmap(size, size);
            Graphics g = Graphics.FromImage(bitmap);
            g.DrawImage(srcimage, x, y, size, size);
            return bitmap;
        }
        #endregion
    }
}
