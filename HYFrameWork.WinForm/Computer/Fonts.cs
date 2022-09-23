using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace HYFrameWork.WinForm
{
    /// <summary>
    /// 系统字体类 可以用于读取本地自定义字库/系统字库
    /// </summary>
    public abstract class Fonts
    {
        /// <summary>
        /// 获取系统字体样式
        /// </summary>
        /// <returns></returns>
        public static FontFamily[] GetFontFamily_Local()
        {
            InstalledFontCollection _myFont = new InstalledFontCollection();
            FontFamily[] _myFontFamilies = _myFont.Families;
            return _myFontFamilies;
        }
        /// <summary>
        /// 获取自定义字体库字形
        /// </summary>
        /// <param name="path">字体文件绝对路径</param>
        /// <param name="fontSize">字体大小</param>
        /// <param name="style">字体样式</param>
        public static Font GetFontFamily(string path, float fontSize, FontStyle style)
        {
            //读取字体文件             
            PrivateFontCollection pfc = new PrivateFontCollection();
            pfc.AddFontFile(path);
            if (pfc != null&& pfc.Families.Length>0)
            {
                //实例化字体             
                Font f = new Font(pfc.Families[0], fontSize, style);
                return f;
            }
            else
            {
                return null;
            }
            
        }
        /// <summary>
        /// 繁简转换
        /// </summary>
        /// <param name="input">要转换内容</param>
        /// <param name="type">要转换模式 0 简->繁 1 繁->简</param>
        public static string Simplified_Chinese_Traditional(string input, int type)
        {
            string newValue = string.Empty;
            if (type == 0)
                newValue = Microsoft.VisualBasic.Strings.StrConv(input, VbStrConv.TraditionalChinese, 0);
            else if (type == 1)
                newValue = Microsoft.VisualBasic.Strings.StrConv(input, VbStrConv.SimplifiedChinese, 0);
            return newValue;
        }
    }
}
