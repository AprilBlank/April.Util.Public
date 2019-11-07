using April.Util.Config;
using April.Util.Extension;
using Microsoft.AspNetCore.Http;

namespace April.Util
{
    public class SessionUtil
    {
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="minutes">过期时间</param>
        /// <returns></returns>
        public static void AddString(string key, string value, int minutes = 30)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
            {
                return;
            }
            AprilConfig.HttpCurrent.Session.SetString(key, value);
        }
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <returns></returns>
        public static void Add<T>(string key, T value)
        {
            if (string.IsNullOrEmpty(key) || value == null)
            {
                return;
            }
            AprilConfig.HttpCurrent.Session.Set(key, value);
        }
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string GetString(string key, string defaultValue = "")
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            string value = AprilConfig.HttpCurrent.Session.GetString(key);
            if (string.IsNullOrEmpty(value))
            {
                value = defaultValue;
            }
            return value;
        }
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return default;
            }
            return AprilConfig.HttpCurrent.Session.Get<T>(key);
        }
        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public static void Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }
            AprilConfig.HttpCurrent.Session.Remove(key);
        }
    }
}
