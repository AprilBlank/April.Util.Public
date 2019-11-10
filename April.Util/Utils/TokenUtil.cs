using April.Util.Config;
using April.Util.Entities.Admin;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace April.Util
{
    public class TokenUtil
    {
        /// <summary>
        /// 设置token
        /// </summary>
        /// <returns></returns>
        public static string GetToken(AdminEntity user, out string expiretimstamp)
        {
            string id = user.ID.ToString();
            double exp = 0;
            switch ((AprilEnums.TokenType)user.TokenType)
            {
                case AprilEnums.TokenType.Web:
                    exp = AprilConfig.WebExpire;
                    break;
                case AprilEnums.TokenType.App:
                    exp = AprilConfig.AppExpire;
                    break;
                case AprilEnums.TokenType.MiniProgram:
                    exp = AprilConfig.MiniProgramExpire;
                    break;
                case AprilEnums.TokenType.Other:
                    exp = AprilConfig.OtherExpire;
                    break;
            }
            DateTime date = DateTime.Now.AddHours(exp);
            user.ExpireTime = date;
            double timestamp = DateUtil.ConvertToUnixTimestamp(date);
            expiretimstamp = timestamp.ToString();
            string token = AprilConfig.TokenSecretFormat.Replace("{id}", id).Replace("{ts}", expiretimstamp);
            token = EncryptUtil.EncryptDES(token, EncryptUtil.SecurityKey);
            //LogUtil.Debug($"用户{id}获取token：{token}");
            Add(token, user);
            //处理多点登录
            SetUserToken(token, user.ID);
            return token;
        }

        /// <summary>
        /// 通过token获取当前人员信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static AdminEntity GetUserByToken(string token = "")
        {
            if (string.IsNullOrEmpty(token))
            {
                token = GetTokenByContent();
            }
            if (!string.IsNullOrEmpty(token))
            {
                
                AdminEntity admin = Get(token);
                if (admin != null)
                {
                    //校验时间
                    if (admin.ExpireTime > DateTime.Now)
                    {
                        if (AprilConfig.AllowSliding)
                        {
                            //延长时间
                            admin.ExpireTime = DateTime.Now.AddMinutes(30);
                            //更新
                            Add(token, admin);
                        }
                        return admin;
                    }
                    else
                    {
                        //已经过期的就不再延长了，当然后续根据情况改进吧
                        return null;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 通过用户请求信息获取Token信息
        /// </summary>
        /// <returns></returns>
        public static string GetTokenByContent()
        {
            string token = "";
            //判断header
            var headers = AprilConfig.HttpCurrent.Request.Headers;
            if (headers.ContainsKey("token"))
            {
                token = headers["token"].ToString();
            }
            if (string.IsNullOrEmpty(token))
            {
                token = CookieUtil.GetString("token");
            }
            if (string.IsNullOrEmpty(token))
            {
                AprilConfig.HttpCurrent.Request.Query.TryGetValue("token", out StringValues temptoken);
                if (temptoken != StringValues.Empty)
                {
                    token = temptoken.ToString();
                }
            }
            return token;
        }
        /// <summary>
        /// 移除Token
        /// </summary>
        /// <param name="token"></param>
        public static void RemoveToken(string token = "")
        {
            if (string.IsNullOrEmpty(token))
            {
                token = GetTokenByContent();
            }
            if (!string.IsNullOrEmpty(token))
            {
                Remove(token);
            }
        }

        #region 多个登录
        /// <summary>
        /// 多个登录设置缓存
        /// </summary>
        /// <param name="token"></param>
        /// <param name="userid"></param>
        public static void SetUserToken(string token, int userid)
        {
            Dictionary<int, List<string>> dicusers = CacheUtil.Get<Dictionary<int, List<string>>>("UserToken");
            if (dicusers == null)
            {
                dicusers = new Dictionary<int, List<string>>();
            }
            List<string> listtokens = new List<string>();
            if (dicusers.ContainsKey(userid))
            {
                listtokens = dicusers[userid];
                if (listtokens.Count <= 0)
                {
                    listtokens.Add(token);
                }
                else
                {
                    if (!AprilConfig.AllowMuiltiLogin)
                    {
                        foreach (var item in listtokens)
                        {
                            RemoveToken(item);
                        }
                        listtokens.Add(token);
                    }
                    else
                    {
                        bool isAdd = true;
                        foreach (var item in listtokens)
                        {
                            if (item == token)
                            {
                                isAdd = false;
                            }
                        }
                        if (isAdd)
                        {
                            listtokens.Add(token);
                        }
                    }
                }
            }
            else
            {

                listtokens.Add(token);
                dicusers.Add(userid, listtokens);
            }
            CacheUtil.Add("UserToken", dicusers, new TimeSpan(6, 0, 0), true);
        }
        /// <summary>
        /// 多个登录删除缓存
        /// </summary>
        /// <param name="userid"></param>
        public static void RemoveUserToken(int userid)
        {
            Dictionary<int, List<string>> dicusers = CacheUtil.Get<Dictionary<int, List<string>>>("UserToken");
            if (dicusers != null && dicusers.Count > 0)
            {
                if (dicusers.ContainsKey(userid))
                {
                    //删除所有token
                    var listtokens = dicusers[userid];
                    foreach (var token in listtokens)
                    {
                        RemoveToken(token);
                    }
                    dicusers.Remove(userid);
                }
            }
        }
        /// <summary>
        /// 多个登录获取
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static List<string> GetUserToken(int userid)
        {
            Dictionary<int, List<string>> dicusers = CacheUtil.Get<Dictionary<int, List<string>>>("UserToken");
            List<string> lists = new List<string>();
            if (dicusers != null && dicusers.Count > 0)
            {
                foreach (var item in dicusers)
                {
                    if (item.Key == userid)
                    {
                        lists = dicusers[userid];
                        break;
                    }
                }
            }
            return lists;
        }
        #endregion

        #region 私有方法(这块儿还需要改进)

        private static void Add(string token,AdminEntity admin)
        {
            switch (AprilConfig.TokenCacheType)
            {
                //不推荐Cookie
                case AprilEnums.TokenCacheType.Cookie:
                    CookieUtil.Add(token, admin);
                    break;
                case AprilEnums.TokenCacheType.Cache:
                    CacheUtil.Add(token, admin, new TimeSpan(0, 30, 0));
                    break;
                case AprilEnums.TokenCacheType.Session:
                    SessionUtil.Add(token, admin);
                    break;
                case AprilEnums.TokenCacheType.Redis:
                    RedisUtil.Add(token, admin);
                    break;
            }
        }

        private static AdminEntity Get(string token)
        {
            AdminEntity admin = null;
            switch (AprilConfig.TokenCacheType)
            {
                case AprilEnums.TokenCacheType.Cookie:
                    admin = CookieUtil.Get<AdminEntity>(token);
                    break;
                case AprilEnums.TokenCacheType.Cache:
                    admin = CacheUtil.Get<AdminEntity>(token);
                    break;
                case AprilEnums.TokenCacheType.Session:
                    admin = SessionUtil.Get<AdminEntity>(token);
                    break;
                case AprilEnums.TokenCacheType.Redis:
                    admin = RedisUtil.Get<AdminEntity>(token);
                    break;
            }
            return admin;
        }

        private static void Remove(string token)
        {
            switch (AprilConfig.TokenCacheType)
            {
                case AprilEnums.TokenCacheType.Cookie:
                    CookieUtil.Remove(token);
                    break;
                case AprilEnums.TokenCacheType.Cache:
                    CacheUtil.Remove(token);
                    break;
                case AprilEnums.TokenCacheType.Session:
                    SessionUtil.Remove(token);
                    break;
                case AprilEnums.TokenCacheType.Redis:
                    RedisUtil.Remove(token);
                    break;
            }
        }
        #endregion
    }
}
