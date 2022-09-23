using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace HYFrameWork.Image
{
    /// <summary>
    /// 动态生成字符串图像 对象
    /// </summary>
    public class DrawStringConfig
    {
        private bool _isCenter;
        /// <summary>
        /// 是否让内容居中显示
        /// </summary>
        public bool IsCenter
        {
            get { return _isCenter; }
            set { _isCenter = value; }
        }
        private Margin _margins;
        /// <summary>
        /// 字符的外边距
        /// </summary>
        public Margin Margins
        {
            get { return _margins; }
            set { _margins = value; }
        }
        private int _height;
        /// <summary>
        /// 图像高度
        /// </summary>
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }
        private int _width;
        /// <summary>
        /// 画布宽度
        /// </summary>
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }
        private Font _fontFamily;
        /// <summary>
        /// 字形
        /// </summary>
        public Font FontFamily
        {
            get { return _fontFamily; }
            set { _fontFamily = value; }
        }
        private double _fontSize;
        /// <summary>
        /// 字号
        /// </summary>
        public double FontSize
        {
            get { return _fontSize; }
            set { _fontSize = value; }
        }
        private Color _fontColor;
        /// <summary>
        /// 字体颜色
        /// </summary>
        public Color FontColor
        {
            get { return _fontColor; }
            set { _fontColor = value; }
        }
        private Color _fontBgColor;
        /// <summary>
        /// 背景颜色
        /// </summary>
        public Color FontBgColor
        {
            get { return _fontBgColor; }
            set { _fontBgColor = value; }
        }
        private List<string> _content;
        /// <summary>
        /// 文本内容
        /// </summary>
        public List<string> Content
        {
            get { return _content; }
            set { _content = value; }
        }

        private FontTypes _fontType;
        /// <summary>
        /// 字体类型
        /// </summary>
        public FontTypes FontType
        {
            get { return _fontType; }
            set { _fontType = value; }
        }
        /// <summary>
        /// 0 简体中文 1 英文 2 繁体中文
        /// </summary>
        public enum FontTypes
        {
            SimplifiedChinese,
            English,
            TraditionalChinese
        }
        /// <summary>
        /// 实体：偏移量
        /// </summary>
        public class Margin
        {
            /// <summary>
            /// 上
            /// </summary>
            public int top { get; set; }
            /// <summary>
            /// 左
            /// </summary>
            public int left { get; set; }
            /// <summary>
            /// 右
            /// </summary>
            public int right { get; set; }
            /// <summary>
            /// 下
            /// </summary>
            public int buttom { get; set; }
        }
    }
}
