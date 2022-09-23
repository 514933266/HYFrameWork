using System;

namespace HYFrameWork.Core
{
    /// <summary>
    /// 时间操作类
    /// </summary>
    public static class TimeHelper
    {

        #region 时间戳
        /// <summary>
        /// 获取13位时间戳
        /// </summary>
        /// <returns>时间戳</returns>
        public static long GetLongTimeStamp()
        {
            return GetLongTimeStamp(DateTime.Now);
        }
        /// <summary>
        /// 获取指定时间为终止值的13位时间戳 
        /// </summary>
        /// <param name="dt">终止时间</param>
        /// <returns>时间戳</returns>
        public static long GetLongTimeStamp(DateTime dt)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (dt.Ticks - startTime.Ticks) / 10000;
            return t;
        }
        /// <summary>
        /// 获取10位时间戳
        /// </summary>
        /// <returns>时间戳</returns>
        public static double GetTimeStamp()
        {
            return GetTimeStamp(DateTime.Now);
        }
        /// <summary>
        /// 获取指定时间为终止值的10位时间戳
        /// </summary>
        /// <param name="dt">终止时间</param>
        /// <returns>时间戳</returns>
        public static double GetTimeStamp(DateTime dt)
        {
            TimeSpan ts = dt - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return ts.TotalSeconds;
        }
        /// <summary>
        /// 时间戳转为C#格式时间/错误的格式会返回当前时间
        /// </summary>
        /// <param name="timeStamp">时间戳</param>
        /// <param name="isLong">是否为13位时间戳</param>
        /// <returns>时间</returns>
        public static DateTime ToDateTime(string timeStamp, bool isLong = true)
        {
            try
            {
                long lTime;
                DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                if (isLong)
                {
                    lTime = long.Parse(timeStamp + "0000");
                }
                else
                {
                    lTime = long.Parse(timeStamp + "0000000");
                }
                TimeSpan toNow = new TimeSpan(lTime);
                DateTime dt = dtStart.Add(toNow);
                return dt;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        /// <summary>
        /// 将毫秒数转换成为秒
        /// </summary>
        /// <param name="ticks">毫秒</param>
        /// <returns>秒</returns>
        public static int ToSecond(long ticks)
        {
            return (int)ticks / 1000;
        }
        #endregion

        #region 中文格式时间
        /// <summary>
        /// 获取中文格式时间：星期x
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns>星期x</returns>
        public static string GetChineseWeek(DateTime dateTime)
        {
            string time = string.Empty;
            try
            {
                DayOfWeek weekDay = dateTime.DayOfWeek;
                switch (weekDay)
                {
                    case DayOfWeek.Sunday:
                        time = string.Format("星期{0}", "日");
                        break;
                    case DayOfWeek.Monday:
                        time = string.Format("星期{0}", "一");
                        break;
                    case DayOfWeek.Tuesday:
                        time = string.Format("星期{0}", "二");
                        break;
                    case DayOfWeek.Wednesday:
                        time = string.Format("星期{0}", "三");
                        break;
                    case DayOfWeek.Thursday:
                        time = string.Format("星期{0}", "四");
                        break;
                    case DayOfWeek.Friday:
                        time = string.Format("星期{0}", "五");
                        break;
                    case DayOfWeek.Saturday:
                        time = string.Format("星期{0}", "六");
                        break;
                }
            }
            catch
            {
                return "日期格式转换错误,请检查参数";
            }
            return time;
        }
        /// <summary>
        /// 获取中文格式时间：yyyy年MM月dd日
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns>yyyy年MM月dd日</returns>
        public static string GetChineseDate(DateTime dateTime)
        {
            string time = string.Empty;
            int y = dateTime.Year;
            string M = string.Empty;
            string d = string.Empty;
            try
            {
                M = AddZero(dateTime.Month);
                d = AddZero(dateTime.Day);
                time = string.Format("{0}年{1}月{2}日", y, M, d);
            }
            catch (Exception ex)
            {
                time = ex.ToString();
            }
            return time;
        }
        /// <summary>
        /// 获取中文格式时间： yyyy年MM月dd日 hh时mm分ss秒
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns>yyyy年MM月dd日 hh时mm分ss秒</returns>
        public static string GetTimeChineseLongDate(DateTime dateTime)
        {
            string _time = GetChineseDate(dateTime);
            string _tick = GetChineseTickDate(dateTime);
            return string.Format("{0} {1}", new object[] { _time, _tick });
        }
        /// <summary>
        /// 获取中文格式时间： hh时mm分ss秒
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns>hh时mm分ss秒</returns>
        public static string GetChineseTickDate(DateTime dateTime)
        {
            string h = string.Empty;
            string m = string.Empty;
            string s = string.Empty;
            if (dateTime.Hour < 10)
                h = "0" + dateTime.Hour;
            else
                h = dateTime.Hour.ToString();
            if (dateTime.Minute < 10)
                m = "0" + dateTime.Minute;
            else
                m = dateTime.Minute.ToString();
            if (dateTime.Second < 10)
                s = "0" + dateTime.Second;
            else
                s = dateTime.Second.ToString();
            string time = string.Format("{0}时{1}分{2}秒", h, m, s);
            return time;
        }
        /// <summary>
        /// 给时间部件/整数不足2位的补0
        /// </summary>
        /// <param name="partValue">值</param>
        /// <returns>计算后的值</returns>
        public static string AddZero(int partValue)
        {
            string newValue = string.Empty;
            if (partValue < 10)
                newValue = "0" + partValue.ToString();
            else
                newValue = partValue.ToString();
            return newValue;
        }

        #endregion

        #region 计算时间差
        /// <summary>
        /// 获取两个日期的时间差 格式:00:00:00.00000000
        /// </summary>
        /// <param name="start">起始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns>时间差字符串</returns>
        public static string TimeSpan(DateTime start, DateTime end)
        {
            TimeSpan tSpan = end - start;
            return string.Format("{0}:{1}:{2}.{3}", AddZero(tSpan.Hours), AddZero(tSpan.Minutes), AddZero(tSpan.Seconds), tSpan.Milliseconds.ToString("#0000"));
        }
        /// <summary>
        /// 获取时间差字符串
        /// </summary>
        /// <param name="timeSpan">时间差</param>
        /// <returns>时间差字符串</returns>
        public static string TimeSpanString(TimeSpan timeSpan)
        {
            return string.Format("{0}:{1}:{2}.{3}", AddZero(timeSpan.Hours), AddZero(timeSpan.Minutes), AddZero(timeSpan.Seconds), timeSpan.Milliseconds.ToString("#0000"));
        }

        #endregion
    }
}
