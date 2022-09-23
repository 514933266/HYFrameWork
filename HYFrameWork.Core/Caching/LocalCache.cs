using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace HYFrameWork.Core
{
    /// <summary>
    /// 本地缓存的实现
    /// </summary>
    public class LocalCache : ICache
    {
        private static ObjectCache cache
        {
            get { return MemoryCache.Default; }
        }

        private DateTimeOffset GetTimeOffset(int seconds)
        {
            if (seconds <= 0) return DateTimeOffset.MaxValue;
            return DateTimeOffset.Now.AddSeconds(seconds);
        }

        /// <summary>
        /// 添加缓存(当key已经存在时直接返回false)
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="seconds">过期时间(秒)</param>
        /// <returns>是否已经添加成功</returns>
        public bool Add(string key, string value, int seconds = 0)
        {
            if (value == null) return false;
            return cache.Add(key, value, GetTimeOffset(seconds));
        }
        /// <summary>
        /// 刷新缓存(如果key不存在会自动创建)
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="seconds">过期时间(秒)</param>
        /// <returns>是否刷新成功</returns>
        public bool Set(string key, string value, int seconds = 0)
        {
            if (value == null) return false;
            cache.Set(key, value, GetTimeOffset(seconds));
            return true;
        }

        /// <summary>
        /// 批量刷新缓存
        /// </summary>
        /// <param name="kvs">缓存键值对</param>
        /// <returns>是否成功批量刷新</returns>
        public bool Set(KeyValuePair<string, string>[] kvs)
        {
            if (kvs == null) return false;
            foreach (var item in kvs)
            {
                cache.Set(item.Key, item.Value, DateTimeOffset.MaxValue);
            }
            return true;
        }

        /// <summary>
        /// 查询缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>缓存值</returns>
        public string Get(string key)
        {
            return cache.Get(key) as string;
        }

        /// <summary>
        /// 批量查询缓存
        /// </summary>
        /// <param name="keys">缓存键数组</param>
        /// <returns>缓存数组</returns>
        public string[] Get(string[] keys)
        {
            if (keys == null) return null;
            var values = new string[keys.Length];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = cache.Get(keys[i]) as string;
            }
            return values;
        }
        /// <summary>
        /// 查询缓存，如果不存在则添加
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="aquire">读取数据的方法</param>
        /// <param name="seconds">过期时间(秒)</param>
        /// <returns>缓存值</returns>
        public string GetOrAdd(string key, Func<string> aquire, int seconds = 0)
        {
            var data = cache.Get(key) as string;
            if (data == null)
            {
                data = aquire();
                if (data == null)
                    return null;
                cache.Add(key, data, GetTimeOffset(seconds));
            }
            return data;
        }
        /// <summary>
        /// 查询缓存，如果不存在则刷新
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="aquire">读取数据的方法</param>
        /// <param name="seconds">过期时间(秒)</param>
        /// <returns>缓存值</returns>
        public string GetOrSet(string key, Func<string> aquire, int seconds = 0)
        {
            var data = cache.Get(key) as string;
            if (data == null)
            {
                data = aquire();
                if (data == null)
                    return null;
                cache.Set(key, data, GetTimeOffset(seconds));
            }
            return data;
        }

        /// <summary>
        /// 是否包含某缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>是否包含某缓存</returns>
        public bool Contains(string key)
        {
            return cache.Contains(key);
        }
        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>是否成功删除缓存</returns>
        public bool Remove(string key)
        {
            return cache.Remove(key) != null;
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="keys">缓存键数组</param>
        /// <returns>删除的缓存数量</returns>
        public long Remove(string[] keys)
        {
            if (keys == null) return 0;
            var removed = 0L;
            foreach (var item in keys)
            {
                if (cache.Remove(item) != null)
                    removed++;
            }
            return removed;
        }
    }
}
