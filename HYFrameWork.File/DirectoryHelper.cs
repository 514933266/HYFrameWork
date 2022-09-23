using System;
using System.Collections.Generic;
using System.IO;
using HYFrameWork.Core;

namespace HYFrameWork.File
{
    /// <summary>
    /// 帮助类：文件目录
    /// </summary>
    public static class DirectoryHelper
    {

        #region 新增
        /// <summary>
        /// 在指定路径下创建目录
        /// </summary>
        /// <param name="path">目录创建路径</param>
        public static void Create(string path)
        {
            Directory.CreateDirectory(path);
        }

        #endregion

        #region 删除

        /// <summary>
        /// 删除所有的子目录和文件
        /// </summary>
        /// <param name="path">目录路径</param>
        public static void DelteChildren(string path)
        {
            Delete(path);
            Create(path);
        }
        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="path">目录路径</param>
        ///  <param name="recursive">是否递归（会删除子目录和文件）</param>
        public static void Delete(string path,bool recursive=true)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, recursive);
            }
        }
        /// <summary>
        /// 检索目录及所有子目录，删除指定日期前的所有文件
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <param name="time">时间</param>
        public static void DeleteFileByCreateTime(string path, DateTime time)
        {
            DeleteFileByCreateTime(path, time, SearchOption.AllDirectories, true);
        }
        /// <summary>
        /// 处理指定目录下，超过某个日期的文件
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <param name="time">时间</param>
        /// <param name="option">检索类型</param>
        /// <param name="deleteDirectory">是否同时删除目录</param>
        public static void DeleteFileByCreateTime(string path,DateTime time, SearchOption option,bool deleteDirectory)
        {
            if (Directory.Exists(path))
            {
                var directory = new DirectoryInfo(path);
                FileInfo[] files = directory.GetFiles();
                foreach (FileInfo file in files)
                {
                    if (file.CreationTime < time && FileHelper.CanReadWrite(file.FullName))
                    {
                        System.IO.File.Delete(file.FullName);
                    }
                }
                if (option == SearchOption.AllDirectories)
                {
                    // 检索子目录  
                    DirectoryInfo[] directoryInfoArray = directory.GetDirectories();
                    foreach (DirectoryInfo dir in directoryInfoArray)
                    {
                        DeleteFileByCreateTime(Path.Combine(path, dir.Name), time, option, deleteDirectory);
                    }
                }
                // 删除目录
                if (deleteDirectory &&
                    directory.GetFiles().Length < 1 &&
                    directory.GetDirectories().Length < 1)
                {
                    Delete(path);
                }
            }
        }

        #endregion

        #region 移动/复制
        /// <summary>
        /// 移动目录
        /// </summary>
        /// <param name="sourceDir">要移动的目录</param>
        /// <param name="targetDir">目标目录</param>
        public static void DirectoryMove(string sourceDir, string targetDir)
        {
            if (Directory.Exists(sourceDir))
            {
                Directory.Move(sourceDir, targetDir);
            }
        }
        /// <summary>
        /// 移动指定路径下的所有文件夹和文件(包含父目录)
        /// </summary>
        /// <param name="directorySource">源目录</param>
        /// <param name="directoryTarget">要移动到的新目录</param>
        /// <param name="option">指示是否搜索子目录</param>
        public static void MoveDirectoryFiles(string directorySource, string directoryTarget, SearchOption option)
        {
            MoveDirectoryFiles(directorySource, directoryTarget, option, DateEnum.Seconds, 0);
        }
        /// <summary>
        /// 移动指定路径下的所有文件夹和文件(包含父目录) 若指定了特殊的日期时间差，则会移动过期的文件
        /// </summary>
        /// <param name="directorySource">源目录</param>
        /// <param name="directoryTarget">要移动到的新目录</param>
        /// <param name="option">指示是否递归搜索子目录</param>
        /// <param name="dateType">日期类型</param>
        /// <param name="timeSpan">距今日为止的目录建立时间差</param>
        public static void MoveDirectoryFiles(string directorySource, string directoryTarget, SearchOption option,DateEnum dateType,int timeSpan)
        {
            //创建目录
            DirectoryInfo directoryInfo = new DirectoryInfo(directorySource);
            Create(directorySource);
            //移动子文件
            FileInfo[] files = directoryInfo.GetFiles();
            foreach (FileInfo file in files)
            {
                FileHelper.MoveByCreateTime(Path.Combine(directorySource, file.Name), Path.Combine(directoryTarget, file.Name), dateType, timeSpan);
            }
            //复制子目录  
            DirectoryInfo[] directoryInfoArray = directoryInfo.GetDirectories();
            foreach (DirectoryInfo dir in directoryInfoArray)
            {
                Copy(Path.Combine(directorySource, dir.Name), Path.Combine(directoryTarget, dir.Name));
                if (option == SearchOption.AllDirectories)
                {
                    MoveDirectoryFiles(Path.Combine(directorySource, dir.Name), Path.Combine(directoryTarget, dir.Name), SearchOption.AllDirectories, dateType, timeSpan);
                }
            }
            //删除目录
            if (directoryInfo.GetFiles().Length < 1 && directoryInfo.GetDirectories().Length < 1)
            {
                Delete(directorySource);
            }
        }
        
        #endregion

        #region 其他
        /// <summary>
        /// 对文件目录集合进行排序
        /// </summary>
        /// <param name="dir">目录集合</param>
        public static void SortedDirectoryInfo(DirectoryInfo[] dir)
        {
            Array.Sort(dir, new DirectoryCreateTimeComparer());
        }
        /// <summary>
        /// 复制目录
        /// </summary>
        /// <param name="source">来源路径</param>
        /// <param name="dirTarget">目标路径</param>
        public static void Copy(string source, string dirTarget)
        {
            Create(dirTarget);
        }
        #endregion

    }

    /// <summary>
    /// 对文件夹进行排序(按照创建时间排序递增)
    /// </summary>
    public class DirectoryCreateTimeComparer : IComparer<DirectoryInfo>
    {
        /// <summary>
        /// 判断目录创建时间并返回相差值
        /// </summary>
        /// <param name="x">目录信息一</param>
        /// <param name="y">目录信息二</param>
        /// <returns>目录创建的相差值</returns>
        public int Compare(DirectoryInfo x, DirectoryInfo y)
        {
            return x.CreationTime.CompareTo(y.CreationTime);
        }
    }
}
