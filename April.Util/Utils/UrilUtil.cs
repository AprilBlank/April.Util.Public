using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;

namespace April.Util
{
    public class UrilUtil
    {
        /// <summary>
        /// 页面路径添加参数
        /// </summary>
        /// <param name="url">页面路径</param>
        /// <param name="dicdata">参数</param>
        /// <returns>页面路径</returns>
        public static string GetAppendQueryString(string url, Dictionary<string, string> dicdata)
        {
            if (string.IsNullOrEmpty(url) || dicdata == null || dicdata.Count <= 0)
            {
                return url;
            }
            foreach (var item in dicdata)
            {
                if (url.IndexOf("?") <= 0)
                {
                    url += "?";
                }
                else
                {
                    url += "&";
                }
                if (!string.IsNullOrEmpty(item.Key) && !string.IsNullOrEmpty(item.Value))
                {
                    url += $"{item.Key}={item.Value}";
                }
            }

            return url;
        }

        /// <summary>
        /// 去除页面路径参数
        /// </summary>
        /// <param name="url">页面路径</param>
        /// <param name="key">参数键值</param>
        /// <returns></returns>
        public static string RemoveQueryString(string url, string key)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(key))
            {
                return url;
            }
            url = url.ToLower();
            key = key.ToLower();
            if (url.IndexOf($"{key}=") <= 0)
            {
                return url;
            }
            Uri uri = new Uri(url);
            NameValueCollection collection = HttpUtility.ParseQueryString(uri.Query);
            if (collection.Count == 0) return url;
            var val = collection[key];
            string fragmentToRemove = string.Format("{0}={1}", key, val);
            string result = url.ToLower().Replace("&" + fragmentToRemove, string.Empty).Replace("?" + fragmentToRemove, string.Empty);
            return result;
        }
        /// <summary>
        /// 获取页面路径参数
        /// </summary>
        /// <param name="strQuery">页面参数</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetRequestQuery(string strQuery)
        {
            if (string.IsNullOrEmpty(strQuery))
            {
                return null;
            }
            Dictionary<string, string> dicdata = new Dictionary<string, string>();
            strQuery = strQuery.Replace("?", string.Empty);
            string[] querys = System.Text.RegularExpressions.Regex.Split(strQuery, "&");
            if (querys.Length > 0)
            {
                foreach (string query in querys)
                {
                    if (string.IsNullOrEmpty(query)) continue;
                    string[] strs = System.Text.RegularExpressions.Regex.Split(query, "=");
                    if (strs.Length == 2)
                    {
                        string key = strs[0];
                        string value = strs[1];
                        if (!dicdata.ContainsKey(key))
                        {
                            dicdata.Add(key, value);
                        }
                    }
                }
            }
            return dicdata;
        }

    }
}
