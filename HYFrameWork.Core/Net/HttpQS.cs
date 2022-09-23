using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYFrameWork.Core
{
    /// <summary>
    /// Http参数拼接
    /// </summary>
    public static class HttpQS
    {
        /// <summary>
        /// 将字典集合转换成a=1&b=2形式的拼接类型
        /// </summary>
        /// <param name="parameters">参数字典</param>
        /// <returns>拼接的结果</returns>
        public static string ToQS(this IDictionary<string, string> parameters)
        {
            var sb = new StringBuilder();
            foreach(var kv in parameters)
            {
                sb.Append("&{0}={1}".Fmt(kv.Key, kv.Value));
            }
            return sb.ToString().Substring(1);
        }
    }
}
