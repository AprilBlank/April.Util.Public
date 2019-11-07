using April.Util.Config;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;

namespace April.Util
{
    public class CookieUtil
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
            AprilConfig.HttpCurrent.Response.Cookies.Append(key, value, new CookieOptions()
            {
                Expires = DateTime.Now.AddMinutes(minutes)
            });
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
            string strValue = JsonConvert.SerializeObject(value);
            AddString(key, strValue);
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
                return "";
            }
            AprilConfig.HttpCurrent.Request.Cookies.TryGetValue(key, out string value);
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
            AprilConfig.HttpCurrent.Request.Cookies.TryGetValue(key, out string value);
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
        public static void Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }
            AprilConfig.HttpCurrent.Response.Cookies.Delete(key);
        }
    }
}
