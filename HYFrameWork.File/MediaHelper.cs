using System.IO;

namespace HYFrameWork.File
{
    /// <summary>
    /// 帮助类：音频、视频操作
    /// </summary>
    public abstract class MediaHelper
    {
        static string[] vt = new[] {
            ".mp4", ".flv", ".f4v", ".webm", ".m4v",
            ".mov", ".3gp", ".3g2", ".rm", ".rmvb",
            ".wmv", ".avi", ".asf", ".mpg", ".mpeg",
            ".mpe", ".ts", ".vob",".dat",".mkv",".swf",
            ".lavf",".cpk",".mod",".ram",".mp3",".aac",
            ".ac3",".wav",".m4a",".ogg",".m3u8"
            };
        /// <summary>
        /// 从文件扩展名判断是否视频格式
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>结果</returns>
        public static bool CheckIsCurrentType(string filePath)
        {
            var extension = Path.GetExtension(filePath);
            for (var i = 0; i < vt.Length; i++)
            {
                if (extension.Contains(vt[i]))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 获取视频文件扩展名
        /// </summary>
        /// <param name="fileName">视频名称</param>
        /// <param name="defaultValue">自定义返回默认值</param>
        /// <returns>扩展名</returns>
        public static string GetExtension(string fileName,string defaultValue= ".mp4")
        {
            for (var i = 0; i < vt.Length; i++)
            {
                if (fileName.Contains(vt[i]))
                    return vt[i];
            }
            return defaultValue;
        }
    }
}
