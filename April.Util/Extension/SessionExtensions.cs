using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;

namespace April.Util.Extension
{
    public static class SessionExtensions
    {
        /// <summary>
        /// 设置Session对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Set<T>(this ISession session, string key, T value)
        {
            if (string.IsNullOrEmpty(key) || value == null) return;
            try
            {
                string strValue = JsonConvert.SerializeObject(value);
                session.SetString(key, strValue);
            }
            catch (Exception ex)
            {
                LogUtil.Error($"设置Session数据失败：{ex.Message}");
            }
        }
        /// <summary>
        /// 获取Session对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(this ISession session, string key)
        {
            T obj = default;
            string value = session.GetString(key);
            try
            {
                if (!string.IsNullOrEmpty(value))
                {
                    obj = (T)JsonConvert.DeserializeObject(value);
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error($"获取Session数据失败：{ex.Message}");
            }
            return obj;
        }
    }
}
