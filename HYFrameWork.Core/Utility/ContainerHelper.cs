using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HYFrameWork.Core
{
    /// <summary>
    /// 帮助类：集合容器（有关集合、数组的方法）
    /// </summary>
    public static class ContainerHelper
    {
        /// <summary>
        /// 遍历集合每一个元素
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="list">集合</param>
        /// <param name="action">遍历方法</param>
        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var item in list)
            {
                action(item);
            }
        }

        /// <summary>
        /// 判断某个对象是否存在集合中
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="list">枚举集合</param>
        /// <param name="t">对象值</param>
        /// <returns>结果</returns>
        public static bool CheckIsIn<T>(this IEnumerable<T> list, T t)
        {
            if (list != null)
            {
                return list.Any(o => o.Equals(t));
            }
            return false;
        }
    }
}
