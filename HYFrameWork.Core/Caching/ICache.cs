using System;
using System.Collections.Generic;

namespace HYFrameWork.Core
{
    /// <summary>
    /// 接口：缓存
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// 添加缓存(当key已经存在时直接返回false)
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="seconds">过期时间(秒)</param>
        /// <returns>是否已经添加成功</returns>
        bool Add(string key, string value, int seconds = 0);

        /// <summary>
        /// 刷新缓存(如果key不存在会自动创建)
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="seconds">过期时间(秒)</param>
        /// <returns>是否刷新成功</returns>
        bool Set(string key, string value, int seconds = 0);

        /// <summary>
        /// 批量刷新缓存
        /// </summary>
        /// <param name="kvs">缓存键值对</param>
        /// <returns>是否成功批量刷新</returns>
        bool Set(KeyValuePair<string, string>[] kvs);

        /// <summary>
        /// 查询缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>缓存值</returns>
        string Get(string key);

        /// <summary>
        /// 批量查询缓存
        /// </summary>
        /// <param name="keys">缓存键数组</param>
        /// <returns>缓存数组</returns>
        string[] Get(string[] keys);

        /// <summary>
        /// 查询缓存，如果不存在则添加
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="aquire">读取数据的方法</param>
        /// <param name="seconds">过期时间(秒)</param>
        /// <returns>缓存值</returns>
        string GetOrAdd(string key, Func<string> aquire, int seconds = 0);

        /// <summary>
        /// 查询缓存，如果不存在则刷新
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="aquire">读取数据的方法</param>
        /// <param name="seconds">过期时间(秒)</param>
        /// <returns>缓存值</returns>
        string GetOrSet(string key, Func<string> aquire, int seconds = 0);

        /// <summary>
        /// 是否包含某缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>是否包含某缓存</returns>
        bool Contains(string key);

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>是否成功删除缓存</returns>
        bool Remove(string key);

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="keys">缓存键数组</param>
        /// <returns>删除的缓存数量</returns>
        long Remove(string[] keys);
    }
}
