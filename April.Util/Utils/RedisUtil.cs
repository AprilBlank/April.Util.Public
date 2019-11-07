using April.Util.Config;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;

namespace April.Util
{
    public class RedisUtil
    {
        private static ConnectionMultiplexer redis;
        private static IDatabase db;
        /// <summary>
        /// 获取连接串
        /// </summary>
        /// <returns></returns>
        protected static ConnectionMultiplexer GetConn()
        {
            if (redis == null || redis.IsConnected)
            {
                try
                {
                    redis = ConnectionMultiplexer.Connect(AprilConfig.RedisHosts);
                    db = redis.GetDatabase();
                }
                catch (Exception ex)
                {
                    LogUtil.Error($"初始化Redis失败:{ex.Message}");
                }
            }
            return redis;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public static void InitRedis()
        {
            if (AprilConfig.IsOpenRedis)
            {
                GetConn();
            }
        }
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <returns></returns>
        public void AddString(string key, string value)
        {
            if (!AprilConfig.IsOpenRedis || string.IsNullOrEmpty(key))
            {
                return;
            }
            db.StringSet(key, value);
        }
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <returns></returns>
        public void Add<T>(string key, T value)
        {
            if (!AprilConfig.IsOpenRedis || string.IsNullOrEmpty(key) || value == null)
            {
                return;
            }
            string strValue = JsonConvert.SerializeObject(value);
            if (!string.IsNullOrEmpty(strValue))
            {
                AddString(key, strValue);
            }
        }
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public string GetString(string key, string defaultValue = "")
        {
            if (!AprilConfig.IsOpenRedis || string.IsNullOrWhiteSpace(key))
            {
                return defaultValue;
            }
            string value = db.StringGet(key);
            if (string.IsNullOrWhiteSpace(value))
            {
                return defaultValue;
            }
            return value;
        }
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            string value = GetString(key);
            if (string.IsNullOrEmpty(value))
            {
                return default;
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
        }
        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public void Remove(string key)
        {
            if (!AprilConfig.IsOpenRedis || string.IsNullOrWhiteSpace(key))
            {
                return;
            }
            db.KeyDelete(key);
        }
    }
}
