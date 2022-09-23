using System;

namespace HYFrameWork.Core
{
    /// <summary>
    /// 实体：Http进度条
    /// </summary>
   public class HttpProgressbar
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 上传/下载速度
        /// </summary>
        public double Speed { get; set; }
        /// <summary>
        /// 已上传/下载字节
        /// </summary>
        public double Byte { get; set; }
        /// <summary>
        /// 总字节
        /// </summary>
        public double TotalByte { get; set; }
        /// <summary>
        /// 用时
        /// </summary>
        public TimeSpan TimeSpan { get; set; }

    }
}
