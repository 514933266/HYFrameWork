using System.Globalization;
using System.Text.RegularExpressions;

namespace HYFrameWork.Core
{
    /// <summary>
    /// 帮助类：编码解码
    /// </summary>
    public static class CodeHelper
    {
        #region Unicode
        /// <summary>
        ///  Unicode解码
        /// </summary>
        /// <param name="str">要解码的字符串</param>
        /// <returns>解码后的内容</returns>
        public static string DeUnicode(string str)
        {
            var reUnicode = new Regex(@"\\u([0-9a-fA-F]{4})", RegexOptions.Compiled);
            return reUnicode.Replace(str, m =>
            {
                short c;
                if (short.TryParse(m.Groups[1].Value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out c))
                {
                    return "" + (char)c;
                }
                return m.Value;
            });
        }
        #endregion

        #region 全角/半角转换
        /// <summary>
        /// 半角转成全角
        /// </summary>
        /// <param name="input">字符串值</param>
        /// <returns>全角字符串</returns>
        public static string DBCToSBC(string input)
        {
            char[] cc = input.ToCharArray();
            for (int i = 0; i < cc.Length; i++)
            {
                if (cc[i] == 32)
                {
                    // 表示空格   
                    cc[i] = (char)12288;
                    continue;
                }
                if (cc[i] < 127 && cc[i] > 32)
                {
                    cc[i] = (char)(cc[i] + 65248);
                }
            }
            return new string(cc);
        }
        /// <summary>
        /// 全角转半角   
        /// 半角空格32,全角空格12288   
        /// 其他字符半角33~126,其他字符全角65281~65374,相差65248   
        /// </summary>  
        /// <param name="input">字符串值</param>
        /// <returns>半角字符串</returns>
        public static string SBCToDBC(string input)
        {
            char[] cc = input.ToCharArray();
            for (int i = 0; i < cc.Length; i++)
            {
                if (cc[i] == 12288)
                {
                    // 表示空格   
                    cc[i] = (char)32;
                    continue;
                }
                if (cc[i] > 65280 && cc[i] < 65375)
                {
                    cc[i] = (char)(cc[i] - 65248);
                }

            }
            return new string(cc);
        }
        #endregion


    }
}
