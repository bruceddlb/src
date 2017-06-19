using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace QSDMS.Util
{
    /// <summary>
    /// 文件夹常用工具类
    /// </summary>
    public static class DirectoryUtil
    {
        /// <summary>
        /// 创建文件夹同步锁
        /// </summary>
        private static readonly object DIRECTORY_LOCK = new object();

        /// <summary>
        /// 创建文件夹
        /// <remarks>多线程间同步创建文件夹</remarks>
        /// </summary>
        /// <param name="folder"></param>
        public static void CreateFolder(string folder)
        {
            if (!Directory.Exists(folder))
            {
                lock (DIRECTORY_LOCK)
                {
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }
                }
            }
        }

        /// <summary>
        /// 检查有效的文件夹名称
        /// <remarks>如果文件名为空则生成唯一GUID作为文件名</remarks>
        /// </summary>
        /// <param name="directoryName"></param>
        /// <returns></returns>
        public static string GetValidName(string directoryName)
        {
            if (directoryName != null)
            {
                directoryName = directoryName.Replace("\\", "")
                                             .Replace("\"", "")
                                             .Replace("/", "")
                                             .Replace(":", "")
                                             .Replace("*", "")
                                             .Replace("?", "")
                                             .Replace(">", "")
                                             .Replace("<", "")
                                             .Replace("'", "").Trim();
            }

            if (directoryName == null || directoryName.Length == 0)
            {
                directoryName = Util.NewLowerGuid();
            }

            return directoryName;
        }

        /// <summary>
        /// 获取文件夹中的文件
        /// </summary>
        /// <param name="folder"></param>
        public static string[] GetFiles(string folder)
        {
            return GetFiles(folder, null);
        }

        /// <summary>
        /// 获取文件夹中的文件
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="searchPatterns"></param>
        public static string[] GetFiles(string folder, string[] searchPatterns)
        {
            return GetFiles(folder, searchPatterns, SearchOption.TopDirectoryOnly);
        }

        /// <summary>
        /// 获取文件夹中的文件
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="searchPatterns"></param>
        /// <param name="searchOption"></param>
        public static string[] GetFiles(string folder, string[] searchPatterns, SearchOption searchOption)
        {
            if (folder == null)
            {
                throw new ArgumentNullException();
            }

            if (searchPatterns == null || searchPatterns.Length == 0)
            {
                return Directory.GetFiles(folder, null, searchOption);
            }

            List<string> results = new List<string>();

            foreach (string searchPattern in searchPatterns)
            {
                string[] files = Directory.GetFiles(folder, searchPattern, searchOption);
                if (files != null)
                {
                    results.AddRange(files);
                }
            }

            return results.ToArray();
        }
    }
}
