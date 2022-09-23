using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HYFrameWork.Core
{
    /// <summary>
    /// 帮助类：字符串处理
    /// </summary>
    public static class StringHelper
    {
        #region 随机字符串
        /// <summary>
        /// 随机字母字符串（纯数字）
        /// </summary>
        /// <param name="len">长度</param>
        /// <returns>字符串</returns>
        public static string GetRandomInt(int len)
        {
            string str = string.Empty;
            int seed = System.Math.Abs((int) BitConverter.ToUInt32(Guid.NewGuid().ToByteArray(), 0));
            Random random = new Random(seed);
            for (int i = 0; i < len; i++)
            {
                str += random.Next(10);
            }
            return str;
        }
        /// <summary>
        /// 随机字母字符串(数字字母混合)
        /// </summary>
        /// <param name="len">长度</param>
        /// <returns>字符串</returns>
        public static string GetRandomBlend(int len)
        {
            char[] arrChar = new char[]
            {
                'a', 'b', 'd', 'c', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'p', 'r', 'q', 's', 't', 'u', 'v',
                'w', 'z', 'y', 'x',
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'Q', 'P', 'R', 'T', 'S', 'V', 'U',
                'W', 'X', 'Y', 'Z',
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            };
            StringBuilder sb = new StringBuilder();
            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < len; i++)
            {
                sb.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());
            }
            return sb.ToString();
        }
        /// <summary>
        /// 随机字母字符串（纯英文）
        /// </summary>
        /// <param name="len">长度</param>
        /// <returns>字符串</returns>
        public static string GetRandomString(int len)
        {
            char[] arrChar = new char[]
            {
                'a', 'b', 'd', 'c', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'p', 'r', 'q', 's', 't', 'u', 'v',
                'w', 'z', 'y', 'x',
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'Q', 'P', 'R', 'T', 'S', 'V', 'U',
                'W', 'X', 'Y', 'Z'
            };
            StringBuilder num = new StringBuilder();
            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < len; i++)
            {
                num.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());
            }
            return num.ToString();
        }

        /// <summary>
        /// 随机生成编号（前缀+时间（毫秒级，举例：20160111081245023）+4位随机数字。），只负责生成，不进行重复性校验,批量生成需对生成的值进行数据库校验
        /// </summary>
        /// <param name="PreStr">前缀</param>
        /// <param name="lenght">长度</param>
        /// <returns>字符串</returns>
        public static string GenerateCommonNo(string PreStr, int lenght = 4)
        {
            Random rd = new Random();
            string str = "0123456789";
            string Last_CommonNo = "";
            for (int i = 0; i < lenght; i++)
            {
                Last_CommonNo += str[rd.Next(str.Length)];
            }
            string CommonNo = string.Format("{0}{1}{2}", PreStr, DateTime.Now.ToString("yyyyMMddHHmmssfff"), Last_CommonNo);
            return CommonNo;
        }
        #endregion

        #region 字符串处理
        /// <summary>
        /// 获取中文内容
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>匹配内容集合</returns>
        public static MatchCollection MatchChinese(string str)
        {
            return Regex.Matches(str, @"[\u4e00-\u9fa5]");
        }
        /// <summary>
        /// 匹配第一个中文字符
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>匹配内容</returns>
        public static string MatchFirstChinese(string str)
        {
            return Regex.Match(str, @"^[\u4e00-\u9fa5]+$").Value;
        }
        /// <summary>
        /// 匹配所有邮箱地址
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>内容集合</returns>
        public static List<string> MatchEmail(string str)
        {
            List<string> emails = new List<string>();
            MatchCollection mc=Regex.Matches(str, @"^[\u4e00-\u9fa5]+$");
            if (mc != null)
            {
                foreach (Match c in mc)
                {
                    emails.Add(c.Value);
                }
            }
            return emails;
        }
        /// <summary>
        /// 正则匹配项替换为指定项
        /// </summary>
        /// <param name="pattern">正则表达式</param>
        /// <param name="content">内容</param>
        /// <param name="replace">替换内容</param>
        /// <returns>字符串</returns>
        public static string ReplaceRegex(string pattern,string content,string replace)
        {
            Regex regx = new Regex(pattern, RegexOptions.IgnoreCase);
            return regx.Replace(content, replace);
        }
        /// <summary>
        /// 填充
        /// </summary>
        /// <param name="format">字符串值</param>
        /// <param name="args">填充的值集合</param>
        /// <returns>填充后的字符串值</returns>
        public static string Fmt(this string format, params object[] args)
        {
            return string.Format(format, args);
        }
        /// <summary>
        /// 追加
        /// </summary>
        /// <param name="str">字符串值</param>
        /// <param name="append">追加的字符串值</param>
        /// <returns>字符串值</returns>
        public static string Append(this string str, string append)
        {
            return str += append;
        }
        /// <summary>
        /// 半角转成全角  
        /// </summary>
        /// <param name="input">字符串值</param>
        /// <returns>全角值</returns>
        public static string DBCToSBC(this string input)
        {
            return CodeHelper.DBCToSBC(input);
        }
        /// <summary>
        /// 全角转半角
        /// </summary>
        /// <param name="input">字符串值</param>
        /// <returns>全角值</returns>
        public static string SBCToDBC(this string input)
        {
            return CodeHelper.SBCToDBC(input);
        }
        /// <summary>
        /// 用制定字符串作为分隔符切割字符串
        /// </summary>
        /// <param name="str">字符串值</param>
        /// <param name="split">分割数组</param>
        /// <returns>结果集</returns>
        public static List<string> ToList(this string str, string[] split)
        {
            List<string> list = new List<string>();
            string[] cSplit = str.Split(split, StringSplitOptions.RemoveEmptyEntries);
            cSplit.ForEach(s =>
            {
                list.Add(s);
            });
            return list;
        }
        /// <summary>
        /// 计算 compare 在 字符串中出现的次数
        /// </summary>
        /// <param name="str">字符串值</param>
        /// <param name="compare">比较值</param>
        /// <returns>出现的数量</returns>
        public static int Count(this string str, string compare)
        {
            int index = str.IndexOf(compare);
            if (index != -1)
            {
                return 1 + Count(str.Substring(index + compare.Length), compare);
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 替换特殊符号为空(仅剩数字和英文字母)
        /// </summary>
        /// <param name="str">字符串值</param>
        /// <returns>替换的值</returns>
        public static string RemoveSymbol(string str)
        {
            return Regex.Replace(str, "[^0-9A-Za-z]", "");
        }

        /// <summary>
        ///  将指定的字符串集合替换指定的字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="symbols">要移除的字符串集合</param>
        /// <param name="replacStr">此字符会用来代替被替换的字符串</param>
        /// <returns>更简洁的字符串</returns>
        public static string RemoveSymbol(string str, string[] symbols, string replacStr = "")
        {
            if (symbols != null && symbols.Length > 0)
            {
                symbols.ForEach(s =>
                {
                    str = str.Replace(s, replacStr);
                });
            }
            return str;
        }
        /// <summary>
        /// 截取某段字符串,获取中间值
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="begin">前缀</param>
        /// <param name="end">后缀</param>
        /// <returns>截取的内容值</returns>
        public static string GetStrByRegex(this string str, string begin, string end)
        {
            Regex regex = new Regex(string.Concat(new string[]
           {
                "(?<=(",
                begin,
                "))[.\\s\\S]*?(?=(",
                end,
                "))"
           }), RegexOptions.Multiline | RegexOptions.Singleline);
            return regex.Match(str).Value;
        }
        /// <summary>
        /// 删除字符串内的HTML和Javasctipt元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>更简洁的字符串</returns>
        public static string FilterHTMLAndScript(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return str;
            str = Regex.Replace(str, @"<script[^>]*>([\s\S](?!<script))*?</script>", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"-->", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"<!--.*", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            str.Replace("<", "");
            str.Replace(">", "");
            str.Replace("\r\n", "");
            return str;
        }
        /// <summary>
        /// 正则过滤script标签
        /// </summary>
        /// <param name="str">包含script标签的字符串</param>
        /// <returns>返回替换后的字符串</returns>
        public static string FilterScript(string str)
        {
            return ReplaceRegex(@"<script[^>]*?>.*?</script>", str, "");
        }
        /// <summary>
        ///  电话掩码
        /// </summary>
        /// <param name="mobile">电话号码字符串</param>
        /// <returns>掩码后的电话</returns>
        public static string FmtMobile(string mobile)
        {
            if (!string.IsNullOrEmpty(mobile) && mobile.Length > 7)
            {
                return ReplaceRegex(@"(?<=\d{3}).+(?=\d{4})", mobile, "****");
            }
            return mobile;
        }
        /// <summary>
        /// 身份证掩码
        /// </summary>
        /// <param name="cert">身份证字符串</param>
        /// <returns>掩码后的身份证字符串</returns>
        public static string FmtCert(string cert)
        {
            if (!string.IsNullOrEmpty(cert) && cert.Length > 10)
            {
                return ReplaceRegex(@"(?<=\w{6}).+(?=\w{4})", cert, "********");
            }
            return cert;
        }

        /// <summary>
        /// 银行卡掩码
        /// </summary>
        /// <param name="bankCark">银行卡号字符串</param>
        /// <returns>掩码后的银行卡号</returns>
        public static string FmtBankCard(string bankCark)
        {
            if (!string.IsNullOrEmpty(bankCark) && bankCark.Length > 4)
            {
                return ReplaceRegex(@"(?<=\d{4})\d+(?=\d{4})", bankCark, " **** **** ");
            }
            return bankCark;
        }
        #endregion

        #region 字符串校验

        /// <summary>
        /// 是否为null或空
        /// </summary>
        /// <param name="str">字符串值</param>
        /// <returns>结果</returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// 是否为null、空或空白
        /// </summary>
        /// <param name="str">字符串值</param>
        /// <returns>结果</returns>
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }
        /// <summary>
        /// 是否包含中文格式
        /// </summary>
        /// <param name="str">字符串值</param>
        /// <returns>结果</returns>
        public static bool HasChinese(this string str)
        {
            return Regex.IsMatch(str, @"[\u4e00-\u9fa5]");
        }
        /// <summary>
        /// 是否仅包含中文
        /// </summary>
        /// <param name="str">字符串值</param>
        /// <returns>结果</returns>
        public static bool IsChinese(this string str)
        {
            return Regex.IsMatch(str, @"^[\u4e00-\u9fa5]+$");
        }
        /// <summary>
        /// 是否为邮箱格式
        /// </summary>
        /// <param name="str">字符串值</param>
        /// <returns>结果</returns>
        public static bool IsEmail(this string str)
        {
            return Regex.IsMatch(str, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
        }
        /// <summary>
        /// 是否为手机号（2.1的交互规则：11位，全数字，1开头）
        /// </summary>
        /// <param name="str">字符串值</param>
        /// <returns>结果</returns>
        public static bool IsMobile(this string str)
        {
            return new Regex("^1[0-9]{10}$").IsMatch(str);
        }
        /// <summary>
        /// 是否为固话号
        /// </summary>
        /// <param name="str">字符串值</param>
        /// <returns>结果</returns>
        public static bool IsPhone(this string str)
        {
            return new Regex(@"^(\d{3,4}-?)?\d{7,8}$").IsMatch(str);
        }
        /// <summary>
        /// 是否为IP
        /// </summary>
        /// <param name="str">字符串值</param>
        /// <returns>结果</returns>
        public static bool IsIP(this string str)
        {
            if (str.IsIPV4() || str.IsIPV6()) return true;
            return false;
        }
        /// <summary>
        ///  是否为IPV4地址
        /// </summary>
        /// <param name="str">字符串值</param>
        /// <returns>结果</returns>
        public static bool IsIPV4(this string str)
        {
            string[] IPs = str.Split('.');

            for (int i = 0; i < IPs.Length; i++)
            {
                if (!Regex.IsMatch(IPs[i], @"^\d+$"))
                {
                    return false;
                }
                if (Convert.ToUInt16(IPs[i]) > 255)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 是否为IPV6格式IP
        /// </summary>
        /// <param name="str">字符串值</param>
        /// <returns>结果</returns>
        public static bool IsIPV6(this string str)
        {
            string pattern = "";
            string temp = str;
            string[] strs = temp.Split(':');
            if (strs.Length > 8)
            {
                return false;
            }
            int count = Count(str, "::");
            if (count > 1)
            {
                return false;
            }
            else if (count == 0)
            {
                pattern = @"^([\da-f]{1,4}:){7}[\da-f]{1,4}$";
                return Regex.IsMatch(pattern, str);
            }
            else
            {
                pattern = @"^([\da-f]{1,4}:){0,5}::([\da-f]{1,4}:){0,5}[\da-f]{1,4}$";
                return Regex.IsMatch(pattern, str);
            }
        }

        /// <summary>
        /// 是否是身份证号
        /// </summary>
        /// <param name="id">字符串值</param>
        /// <returns>结果</returns>
        public static bool IsIDCard(this string id)
        {
            if (string.IsNullOrEmpty(id))
                return false;
            if (id.Length == 18)
                return IsIDCard18(id);
            else if (id.Length == 15)
                return IsIDCard15(id);
            else
                return false;
        }
        /// <summary>
        ///  是否为15位身份证号
        /// </summary>
        /// <param name="Id">字符串值</param>
        /// <returns>结果</returns>
        static bool IsIDCard15(this string Id)
        {
            long n = 0;
            if (long.TryParse(Id, out n) == false || n < System.Math.Pow(10, 14))
                return false;//数字验证

            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
                return false;//省份验证

            string birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
                return false;//生日验证

            return true;//符合15位身份证标准
        }
        /// <summary>
        ///  是否为18位身份证号
        /// </summary>
        /// <param name="Id">字符串</param>
        /// <returns>结果</returns>
        static bool IsIDCard18(this string Id)
        {
            long n = 0;
            if (long.TryParse(Id.Remove(17), out n) == false || n < System.Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
                return false;//数字验证

            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
                return false;//省份验证

            string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
                return false;//生日验证

            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = Id.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());

            int y = -1;
            System.Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
                return false;//校验码验证

            return true;//符合GB11643-1999标准
        }
        /// <summary>
        /// 是否为日期
        /// </summary>
        /// <param name="str">字符串值</param>
        /// <returns>结果</returns>
        public static bool IsDate(this string str)
        {
            return new Regex(@"(\d{4})-(\d{1,2})-(\d{1,2})").IsMatch(str);
        }
        /// <summary>
        /// 是否是数值(包括整数和小数)
        /// </summary>
        /// <param name="numericStr">字符串值</param>
        /// <returns>结果</returns>
        public static bool IsNumeric(this string numericStr)
        {
            return new Regex(@"^[-]?[0-9]+(\.[0-9]+)?$").IsMatch(numericStr);
        }
        /// <summary>
        /// 是否为纯字母
        /// </summary>
        /// <param name="str">字符串值</param>
        /// <returns>结果</returns>
        public static bool IsLetters(this string str)
        {
            return new Regex(@"^[A-Za-z]+$").IsMatch(str);
        }
        /// <summary>
        /// 是否为邮政编码
        /// </summary>
        /// <param name="str">字符串值</param>
        /// <returns>结果</returns>
        public static bool IsZipCode(this string str)
        {
            return new Regex(@"^\d{6}$").IsMatch(str);
        }
        /// <summary>
        /// 字符串是否包含数组里面的某个元素（支持模糊匹配）
        /// </summary>
        /// <param name="str">字符串值</param>
        /// <param name="list">集合</param>
        /// <returns>结果</returns>
        public static bool Contains(this string str, List<string> list)
        {
            return list.Any(str.Contains);
        }
        /// <summary>
        /// 判断字符串是否包含为数组的成员
        /// </summary>
        /// <param name="str">字符串值</param>
        /// <param name="arr">数组</param>
        /// <returns>查找的对象值，可能为NULL</returns>
        public static string Contains(this string str, string[] arr)
        {
            return arr.FirstOrDefault(c => c == str);
        }

        #endregion

        #region 读写配置文件
        
        #endregion

    }
}
