using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Novel.Common.Utils
{
    public static class RedisExtents
    {

        public static bool LockKey(this IDatabase readis, string key)
        {
            return readis.LockTake(key, Environment.MachineName, TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// 阻塞锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        //public bool LockKey(string key, int time)
        //{
        //    int i = 0;
        //    while (!readis.LockTake(key, time) && i < time)
        //    {
        //        i += 2;
        //        Thread.Sleep(2000);
        //    }
        //    return true;
        //}

        public static bool LockRelease(this IDatabase readis, string key)
        {
            return readis.LockRelease(key, Environment.MachineName);
        }
        ///// <summary>
        ///// 获取readis缓存操作单例对象
        ///// </summary>
        ///// <returns></returns>
        //public static ReadisHelper GetReadisHelper()
        //{
        //    return new ReadisHelper();
        //    //if (readisHelper == null)
        //    //{

        //    //    readisHelper = new ReadisHelper();
        //    //}
        //    //return readisHelper;
        //}
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool SetCache(this IDatabase readis, string key, string str)
        {
            return readis.StringSet(key, str);
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="str"></param>
        /// <param name="t">过期时间</param>
        /// <returns></returns>
        public static bool SetCache(this IDatabase readis, string key, string str, int t)
        {
            return readis.StringSet(key, str, TimeSpan.FromSeconds(t));
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="str"></param>
        /// <param name="t">过期时间</param>
        /// <returns></returns>
        public static bool SetCache(this IDatabase readis, string key, string str, TimeSpan t)
        {
            return readis.StringSet(key, str, t);
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="t">过期时间</param>
        /// <returns></returns>
        public static bool SetCache(this IDatabase readis, string key, object obj, int t)
        {
            string json = JsonConvert.SerializeObject(obj);
            return readis.StringSet(key, json, TimeSpan.FromSeconds(t));
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="t">过期时间</param>
        /// <returns></returns>
        public static bool SetCache(this IDatabase readis, string key, object obj, TimeSpan t)
        {
            string json = JsonConvert.SerializeObject(obj);
            return readis.StringSet(key, json, t);
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool SetCache(this IDatabase readis, string key, object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            return readis.StringSet(key, json);
        }
        /// <summary>
        /// 获取缓存
        /// </summary>
        public static T GetCache<T>(this IDatabase readis, string key) where T : class, new()
        {

            RedisValue json = readis.StringGet(key);
            if (json.IsNullOrEmpty)
            {
                return null;
            }
            if (typeof(T) == typeof(RedisValue))
            {
                return json as T;
            }
            return JsonConvert.DeserializeObject<T>(json);
        }
        /// <summary>
        /// 获取缓存
        /// </summary>
        public static string GetCache(this IDatabase readis, string key)
        {
            return readis.StringGet(key);

        }
        public static bool HashDelete(this IDatabase readis, RedisKey key, RedisValue hashField)
        {
            return readis.HashDelete(key.ToString(), hashField);
        }
        /// <summary>
        /// 自增count，返回自增后的值
        /// </summary>
        public static double StringIncrement(this IDatabase readis, string key, double count)
        {
            return readis.StringIncrement(key, count);

        }
        /// <summary>
        /// 
        /// </summary>
        public static RedisValue StringIncrement(this IDatabase readis, string key, RedisValue val)
        {
            return readis.HashGet(key, val);
        }
        public static bool HashExists(this IDatabase readis, string key, RedisValue val)
        {
            return readis.HashExists(key, val);
        }
        public static RedisValue HashGet(this IDatabase readis, RedisKey key, RedisValue hashFeld)
        {
            return readis.HashGet((key.ToString()), hashFeld);
        }
        /// <summary>
        /// 删除缓存
        /// </summary>
        public static bool DeleteCache(this IDatabase readis, string key)
        {

            return readis.KeyDelete(key);
        }
        public static void HashSet(this IDatabase readis, RedisKey key, HashEntry[] fields)
        {
            readis.HashSet((key.ToString()), fields);
        }

        public static void HashSet(this IDatabase readis, RedisKey key, RedisValue field, RedisValue value)
        {
            readis.HashSet((key.ToString()), field, value);
        }

        public static IEnumerable<HashEntry> HashScan(this IDatabase readis, RedisKey key, RedisValue pattern)
        {
            return readis.HashScan((key.ToString()), pattern, 100);
        }

        public static IEnumerable<HashEntry> HashScan(this IDatabase readis, RedisKey key)
        {
            return readis.HashScan((key.ToString()));
        }

        public static RedisValue[] HashValues(this IDatabase readis, RedisKey key)
        {
            return readis.HashValues((key.ToString()));
        }

        public static long ListLeftPush(this IDatabase readis, RedisKey key, RedisValue value)
        {
            return readis.ListLeftPush(key, value);
        }

        public static long ListRightPush(this IDatabase readis, RedisKey key, RedisValue value)
        {
            return readis.ListRightPush(key, value);
        }

        public static long ListLength(this IDatabase readis, RedisKey key)
        {
            return readis.ListLength(key);
        }

        public static RedisValue ListLeftPop(this IDatabase readis, RedisKey key)
        {
            return readis.ListLeftPop(key);
        }

        public static RedisValue ListRightPop(this IDatabase readis, RedisKey key)
        {
            return readis.ListRightPop(key);
        }

        public static RedisValue[] ListRange(this IDatabase readis, RedisKey key)
        {
            return readis.ListRange(key);
        }

        public static RedisValue[] HashKeys(this IDatabase readis, RedisKey key)
        {
            return readis.HashKeys((key.ToString()));
        }

        public static bool KeyExists(this IDatabase readis, RedisKey key)
        {
            return readis.KeyExists((key.ToString()));
        }

        public static void HashSet(this IDatabase readis, RedisKey key, RedisValue field, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            readis.HashSet((key.ToString()), field, value, when, flags);
        }
        /// <summary>
        /// 获取缓存用户票证信息
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="versions">版本</param>
        /// <returns></returns>
        public static string GetSecretKeyCache(this IDatabase readis, string userid, string versions)
        {
            return readis.StringGet("SecretKey_" + userid + versions.Replace(".", ""));
        }
        /// <summary>
        /// 设置用户票证缓存
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="versions">版本</param>
        /// <param name="key">票证值</param>
        /// <returns></returns>
        public static bool SetSecretKeyCache(this IDatabase readis, string userid, string versions, string key)
        {
            return readis.StringSet("SecretKey_" + userid + versions.Replace(".", ""), key, new TimeSpan(24, 0, 0));
        }
        public static bool LockKey(this IDatabase readis, string key, int second)
        {
            return readis.LockTake(key, Environment.MachineName, TimeSpan.FromSeconds(second));
        }
    }
}
