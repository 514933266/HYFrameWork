using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using HYFrameWork.Core;

namespace HYFrameWork.File
{
    /// <summary>
    /// 帮助类：文件操作
    /// </summary>
    public class FileHelper
    {
        
        #region 增加

        /// <summary>
        /// 在指定文件父路径创建目录
        /// </summary>
        /// <param name="filePath">父目录路径</param>
        public static void CreateParentDirectory(string filePath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        }

        /// <summary>
        /// 在指定路径创建一个文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public static void CreateEmpty(string filePath)
        {
            Create(filePath, new byte[0]);
        }
        /// <summary>
        /// 在指定路径创建一个文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="stream">文件流</param>
        public static void Create(string filePath, Stream stream)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                stream.CopyTo(fs);
            }
            stream.Close();
        }
        /// <summary>
        /// 在指定路径创建一个文件(会覆盖文件)
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="fileByte">文件字节流</param>
        public static void Create(string filePath, byte[] fileByte)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                fs.Write(fileByte, 0, fileByte.Length);
            }
        }
        #endregion

        #region 查询

        /// <summary>
        /// 读取指定路径文件并返回字节流
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>文件流</returns>
        public static Stream ReadStream(string filePath)
        {
            return new FileStream(filePath, FileMode.Open, FileAccess.Read);
        }
        /// <summary>
        /// 读取指定路径文件并返回字节流
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>文件字节流</returns>
        public static byte[] ReadByte(string filePath)
        {
            return ReadByte(filePath, 0);
        }
        /// <summary>
        /// 读取指定路径文件并返回字节流
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="length">指定读取长度</param>
        /// <returns>文件字节流</returns>
        public static byte[] ReadByte(string filePath, int length)
        {
            byte[] arr = null;
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var cur = length > 0 ? length : fs.Length;
                arr = new byte[cur];
                fs.Read(arr, 0, (int)cur);
            }
            return arr;
        }

        /// <summary>
        /// 获取指定目录下所有的文件信息(按时间递增排序)
        /// </summary>
        /// <param name="path">指定目录路径</param>
        public static FileInfo[] GetInfos(string path)
        {
            return GetInfos(path, SearchOption.TopDirectoryOnly);
        }
        /// <summary>
        /// 获取指定目录下所有的文件信息(按时间递增排序)
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <param name="option">指示是否搜索所有子目录</param>
        /// <param name="searchPattern">搜索约束：例如 *.txt</param>
        /// <param name="sortByTime">是否根据创建时间排序</param>
        /// <returns>文件集合</returns>
        public static FileInfo[] GetInfos(string path, SearchOption option, string searchPattern = "*.*", bool sortByTime = false)
        {
            FileInfo[] files = null;
            if (Directory.Exists(path))
            {
                files = new DirectoryInfo(path).GetFiles(searchPattern, option);
            }
            if (sortByTime && files.Length > 0)
            {
                SortByTime(files);
            }
            return files;
        }


        /// <summary>
        /// 获取当前目录下文件路径集合并进行排序(按照创建时间排序递增)
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <returns>文件路径集合</returns>
        public static string[] GetPaths(string path)
        {
            return GetPaths(path, SearchOption.TopDirectoryOnly);
        }
        /// <summary>
        /// 获取文件路径集合并进行排序(按照创建时间排序递增)
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <param name="option">指示是否搜索所有子目录</param>
        /// <param name="searchPattern">搜索约束：例如 *.txt</param>
        /// <param name="sortByTime">是否根据创建时间排序</param>
        /// <returns>子文件路径集合</returns>
        public static string[] GetPaths(string path, SearchOption option, string searchPattern = "*.*", bool sortByTime = false)
        {
            string[] pathArr = null;
            var fileArr = GetInfos(path, option, searchPattern, sortByTime);
            if (fileArr != null)
            {
                pathArr = new string[fileArr.Length];
                for (int i = 0; i < fileArr.Length; i++)
                {
                    pathArr[i] = fileArr[i].FullName;
                }
            }
            return pathArr;
        }

        #endregion

        #region 复制/移动

        /// <summary>
        /// 移动指定文件到新目录，并指定新名称。如果新目录已经包含同名文件则进行覆盖操作
        /// </summary>
        /// <param name="sourceFilePath">源文件路径</param>
        /// <param name="targetFilePath">目标文件路径</param>
        public static void Move(string sourceFilePath, string targetFilePath)
        {
            Copy(sourceFilePath, targetFilePath, true);
        }

        /// <summary>
        /// 移动距今指定时间差的文件
        /// </summary>
        /// <param name="sourceFilePath">源文件路径</param>
        /// <param name="targetFilePath">目标文件路径</param>
        /// <param name="dateType">时间类型</param>
        /// <param name="timeSpan">时间差</param>
        public static void MoveByCreateTime(string sourceFilePath, string targetFilePath,DateEnum dateType, int timeSpan)
        {
            if (System.IO.File.Exists(sourceFilePath))
            {
                var canMove = false;
                var fileCreateTime = System.IO.File.GetCreationTime(sourceFilePath);
                switch (dateType)
                {
                    case DateEnum.Year:        if (fileCreateTime.AddYears(timeSpan) < DateTime.Now)   canMove = true; break;
                    case DateEnum.Month:       if (fileCreateTime.AddMonths(timeSpan) < DateTime.Now)  canMove = true; break;
                    case DateEnum.Week:        if (fileCreateTime.AddDays(timeSpan*7) < DateTime.Now)  canMove = true; break;
                    case DateEnum.Day:         if (fileCreateTime.AddDays(timeSpan) < DateTime.Now)    canMove = true; break;
                    case DateEnum.Hours:       if (fileCreateTime.AddHours(timeSpan) < DateTime.Now)   canMove = true; break;
                    case DateEnum.Minutes:     if (fileCreateTime.AddMinutes(timeSpan) < DateTime.Now) canMove = true; break;
                    case DateEnum.Seconds:     if (fileCreateTime.AddSeconds(timeSpan) < DateTime.Now) canMove = true; break;
                }
                if (System.IO.File.Exists(targetFilePath))System.IO.File.Delete(targetFilePath);
                if (canMove&&CanReadWrite(sourceFilePath))
                {
                    System.IO.File.Copy(sourceFilePath, targetFilePath, true);
                }
            }
        }

        /// <summary>
        /// 复制文件到指定目录
        /// </summary>
        /// <param name="sourceFilePath">源文件路径</param>
        /// <param name="targetFilePath">目标文件路径</param>
        /// <param name="deleteSource">是否删除源文件</param>
        /// <param name="overWrite">是否覆盖</param>
        public static void Copy(string sourceFilePath, string targetFilePath, bool deleteSource = false, bool overWrite = true)
        {
            if (System.IO.File.Exists(sourceFilePath))
            {
                CreateParentDirectory(targetFilePath);
                if (CanReadWrite(sourceFilePath))
                {
                    System.IO.File.Copy(sourceFilePath, targetFilePath, overWrite);
                    if (deleteSource) System.IO.File.Delete(sourceFilePath);
                }
            }
        }


        #endregion

        #region 其他

        /// <summary>
        /// 确定文件是否可以进行读写
        /// </summary>
        /// <param name="filename">文件路径</param>
        public static bool CanReadWrite(string filename)
        {
            try
            {
                using (FileStream fs = new FileInfo(filename).Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    return true;
                }
            }
            catch (IOException)
            {
                return false;
            }
        }

        /// <summary>
        /// 对文件集合进行排序 时间递增
        /// </summary>
        /// <param name="files">指定文件集合</param>
        public static void SortByTime(FileInfo[] files)
        {
            Array.Sort(files, new FileCreateTimeComparer());
        }
        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>结果</returns>
        public static bool Exists(string filePath)
        {
            return System.IO.File.Exists(filePath);
        }

        #endregion

    }
    /// <summary>
    /// 对文件进行排序(按照创建时间排序递增)
    /// </summary>
    public class FileCreateTimeComparer : IComparer<FileInfo>
    {
        /// <summary>
        /// 判断文件创建时间并返回相差值
        /// </summary>
        /// <param name="x">文件信息一</param>
        /// <param name="y">文件信息二</param>
        /// <returns>目录创建的相差值</returns>
        public int Compare(FileInfo x, FileInfo y)
        {
            return x.CreationTime.CompareTo(y.CreationTime);
        }
    }
}
