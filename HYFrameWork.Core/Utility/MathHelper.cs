
namespace HYFrameWork.Core
{
    /// <summary>
    /// 帮助类：数值处理
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// 按指定位数舍去数字后面的小数
        /// </summary>
        /// <param name="number">数值</param>
        /// <param name="digits">舍去位数</param>
        /// <returns>计算后的结果</returns>
        public static decimal Retain(decimal number, int digits)
        {
            var temp = 1;
            for (int i = 1; i <= digits; i++)
            {
                temp *= 10;
            }
            return System.Math.Truncate(number * temp) / temp;
        }
    }
}
