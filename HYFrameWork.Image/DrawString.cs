using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace HYFrameWork.Image
{
    /// <summary>
    /// 动态生成字符串图像
    /// </summary>
    public class DrawString
    {
        public Bitmap bp { get; set; }
        /// <summary>
        /// 把文字内容绘画到图像上
        /// 如果字符串是中文，请注意要获取真实的字节长度，否则长度不足将会换行显示
        /// </summary>
        /// <param name="config">绘图配置对象</param>
        /// <returns>图像</returns>
        public Bitmap Drawing(DrawStringConfig config)
        {
            //1.0 图像配置
            bp = new Bitmap(config.Width, config.Height);
            int fontSize = (int)config.FontSize;
            int[] margin = { config.Margins.top, config.Margins.right, config.Margins.buttom, config.Margins.left };//内容的magin
            int lineHeight = margin[0] + fontSize + margin[2];//文本框原始高度
            int rowHeight = 0;//行高
            int rowWidth = bp.Width - margin[1] - margin[3];//行宽
            int rowLocationX = margin[3];//行的X起点
            int lineLocationX_end = bp.Width - margin[3];//线的终点X起始位置
            int rowLocationY = 0;//行的Y起点
            int lineLocationY_end = 0;//线的Y终点
            int strWidth = 0;//字符内容所占的像素
            int lineCount = 0;//每行内容所占的行数
            Graphics g = Graphics.FromImage(bp);
            Font font = config.FontFamily;
            Color color = config.FontColor;
            g.FillRectangle(new SolidBrush(config.FontBgColor), 0, 0, bp.Width, bp.Height);

            SolidBrush solidBrush = new SolidBrush(color);
            StringFormat format = new StringFormat();//设定文字水平
            if (config.IsCenter)
            {
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
            }
            else
            {
                format.Alignment = StringAlignment.Near;
                format.LineAlignment = StringAlignment.Near;
            }

            //2.0 内容绘制
            RectangleF container;
            List<string> list = config.Content;
            for (int i = 0; i < list.Count; i++)
            {
                SizeF vSizeF = g.MeasureString(list[i], font);
                strWidth = Convert.ToInt32(Math.Ceiling(vSizeF.Width));
                lineCount = strWidth / (bp.Width - margin[1] - margin[3]);
                lineCount = lineCount < 1 ? 1 : lineCount;
                rowLocationY = lineLocationY_end;
                rowHeight = lineHeight + (fontSize + margin[0]) * lineCount;
                lineLocationY_end = rowLocationY + rowHeight;
                container = new RectangleF(new PointF(rowLocationX, rowLocationY), new SizeF(rowWidth, rowHeight));
                g.DrawString(list[i].ToString(), font, solidBrush, container, format);
            }
            return bp;
        }
        /// <summary>
        /// 绘画一个使用须知表格  表头字体加粗 
        /// </summary>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="content">内容集合</param>
        /// <returns>图像</returns>
        public Bitmap Drawing_Agreement(int width, int height, List<string> content)
        {
            //图像配置区
            bp = new Bitmap(width, height);
            int fontSize = 10;
            int[] margin = { 9, 10, 9, 30 };//内容的magin
            int lineHeight = margin[0] + fontSize + margin[2];//文本框原始高度
            int rowHeight = 0;//行高
            int rowWidth = bp.Width - margin[1] - margin[3];//行宽
            int rowLocationX = margin[3];//行的X起点
            int lineLocationX_end = bp.Width - margin[3];//线的终点X起始位置
            int rowLocationY = 0;//行的Y起点
            int lineLocationY_end = 0;//线的Y终点
            int borderHeight = 1;//表格线的高度
            int strWidth = 0;//字符内容所占的像素
            int lineCount = 0;//每行内容所占的行数
            Graphics g = Graphics.FromImage(bp);
            Font font = null;
            Color color = Color.IndianRed;
            SolidBrush solidBrush = new SolidBrush(color);
            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            Pen pen = new Pen(solidBrush, 1);
            pen.DashStyle = DashStyle.Custom;
            pen.DashPattern = new float[] { 5f, 1f };
            Graphics vGraphics = Graphics.FromImage(bp);
            RectangleF container;
            //内容绘制
            for (int i = 0; i < content.Count; i++)
            {
                if (i == 0)
                    font = new Font("宋体", 13, FontStyle.Bold);
                else
                    font = new Font("宋体", fontSize, FontStyle.Regular);
                SizeF vSizeF = vGraphics.MeasureString(content[i], font);
                strWidth = Convert.ToInt32(Math.Ceiling(vSizeF.Width));
                lineCount = strWidth / (bp.Width - margin[1] - margin[3]);



                rowLocationY = (i == 0 ? 0 : lineLocationY_end + borderHeight);
                rowHeight = lineHeight + (fontSize + margin[0]) * lineCount;
                lineLocationY_end = i == 0 ? lineHeight : rowLocationY + rowHeight;
                //绘制文本的矩形大小（用于设定文字水平居中/垂直居中）
                if (i < 2)
                {
                    format.Alignment = StringAlignment.Center;
                    container = new RectangleF(new PointF(20, rowLocationY), new SizeF(rowWidth, rowHeight));
                }
                else
                {
                    format.Alignment = StringAlignment.Near;
                    container = new RectangleF(new PointF(rowLocationX, rowLocationY), new SizeF(rowWidth, rowHeight));
                }
                g.DrawString(content[i].ToString(), font, solidBrush, container, format);
                g.DrawLine(pen, new Point(rowLocationX, lineLocationY_end), new Point(lineLocationX_end, lineLocationY_end));
            }
            return bp;
        }

    }
}
