using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace April.Util.Config
{
    public class AprilConfig
    {
        public static IConfiguration Configuration;
        public static IServiceProvider ServiceProvider;

        #region ========加密========
        private static string _SecurityKey = string.Empty;
        /// <summary>
        /// 加密串
        /// </summary>
        public static string SecurityKey
        {
            get
            {
                if (string.IsNullOrEmpty(_SecurityKey))
                {
                    _SecurityKey = Configuration["CommonSettings:SecurityKey"];
                }
                return _SecurityKey;
            }
        }
        #endregion

        #region ========过期时间========
        private static string _WebExpire = string.Empty;
        private static string _AppExpire = string.Empty;
        private static string _MiniProgramExpire = string.Empty;
        private static string _OtherExpire = string.Empty;

        /// <summary>
        /// PC端过期时间
        /// </summary>
        public static double WebExpire
        {
            get
            {
                if (string.IsNullOrEmpty(_WebExpire))
                {
                    _WebExpire = Configuration["CommonSettings:WebExpire"];
                }
                double defaultvalue = 12;
                double.TryParse(_WebExpire, out defaultvalue);
                return defaultvalue;
            }
        }
        /// <summary>
        /// 手机端过期时间
        /// </summary>
        public static double AppExpire
        {
            get
            {
                if (string.IsNullOrEmpty(_AppExpire))
                {
                    _AppExpire = Configuration["CommonSettings:AppExpire"];
                }
                double defaultvalue = 12;
                double.TryParse(_AppExpire, out defaultvalue);
                return defaultvalue;
            }
        }
        /// <summary>
        /// 小程序过期时间
        /// </summary>
        public static double MiniProgramExpire
        {
            get
            {
                if (string.IsNullOrEmpty(_MiniProgramExpire))
                {
                    _MiniProgramExpire = Configuration["CommonSettings:MiniProgramExpire"];
                }
                double defaultvalue = 12;
                double.TryParse(_MiniProgramExpire, out defaultvalue);
                return defaultvalue;
            }
        }
        /// <summary>
        /// 其他过期时间
        /// </summary>
        public static double OtherExpire
        {
            get
            {
                if (string.IsNullOrEmpty(_OtherExpire))
                {
                    _OtherExpire = Configuration["CommonSettings:OtherExpire"];
                }
                double defaultvalue = 12;
                double.TryParse(_OtherExpire, out defaultvalue);
                return defaultvalue;
            }
        }
        #endregion

        #region ========数据库相关========
        private static string _DefaultSqlServerConnectionString = string.Empty;
        private static string _DefaultMySqlConnectionString = string.Empty;
        private static string _IsGetSql = string.Empty;
        private static string _IsDebugSql = string.Empty; 
        private static string _SqlType = string.Empty;

        /// <summary>
        /// 默认数据库连接串(SqlServer)
        /// </summary>
        public static string DefaultSqlServerConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_DefaultSqlServerConnectionString))
                {
                    _DefaultSqlServerConnectionString = Configuration["DefaultSqlConnectionString:SqlServer"];
                }
                return _DefaultSqlServerConnectionString;
            }
        }
        /// <summary>
        /// 默认数据库连接串(Mysql)
        /// </summary>
        public static string DefaultMySqlConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_DefaultMySqlConnectionString))
                {
                    _DefaultMySqlConnectionString = Configuration["DefaultSqlConnectionString:MySql"];
                }
                return _DefaultMySqlConnectionString;
            }
        }
        /// <summary>
        /// 是否记录Sql
        /// </summary>
        public static bool IsGetSql
        {
            get
            {
                if (string.IsNullOrEmpty(_IsGetSql))
                {
                    _IsGetSql = Configuration["CommonSettings:IsGetSql"];
                }
                if (_IsGetSql == "True")
                {
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// 是否测试Sql性能
        /// </summary>
        public static bool IsDebugSql
        {
            get
            {
                if (string.IsNullOrEmpty(_IsDebugSql))
                {
                    _IsDebugSql = Configuration["CommonSettings:IsDebugSql"];
                }
                if (_IsDebugSql == "True")
                {
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// 数据库类型[0:mySql,1:sqlServer,2:Sqlite,3:Oracle,4:PostgreSQL]
        /// </summary>
        public static int SqlType
        {
            get
            {
                if (string.IsNullOrEmpty(_SqlType))
                {
                    _SqlType = Configuration["CommonSettings:SqlType"];
                }
                int type = 1;
                int.TryParse(_SqlType, out type);
                return type;
            }
        }

        #endregion

        #region ========访问及权限========
        private static List<string> _AllowUrl = null;
        private static string _IsLocalTest = string.Empty;
        /// <summary>
        /// 链接白名单(无需登录)
        /// </summary>
        public static List<string> AllowUrl
        {
            get
            {
                if (_AllowUrl == null)
                {
                    _AllowUrl = new List<string>();
                    string urls = Configuration["CommonSettings:AllowUrl"];
                    if (!string.IsNullOrEmpty(urls))
                    {
                        string[] strurls = System.Text.RegularExpressions.Regex.Split(urls, ",");
                        foreach (string url in strurls)
                        {
                            if (string.IsNullOrEmpty(url) || _AllowUrl.Contains(url)) continue;
                            _AllowUrl.Add(url);
                        }
                    }
                }
                return _AllowUrl;
            }
        }
        /// <summary>
        /// 是否本地测试
        /// </summary>
        public static bool IsLocalTest
        {
            get
            {
                if (string.IsNullOrEmpty(_IsLocalTest))
                {
                    _IsLocalTest = Configuration["CommonSettings:IsLocalTest"];
                }
                if (_IsLocalTest == "True")
                {
                    return true;
                }
                return false;
            }
        }
        #endregion

        #region ========日志相关========
        private static string _SysLogLevel = string.Empty;
        /// <summary>
        /// 系统日志级别
        /// </summary>
        public static int SysLogLevel
        {
            get
            {
                if (string.IsNullOrEmpty(_SysLogLevel))
                {
                    _SysLogLevel = Configuration["LogLevel"];
                }
                else
                {
                    //临时切换日志记录等级，为了方便在线调试等业务场景需求
                    var cacheLogLevel = CacheUtil.GetString("LogLevel");
                    if (cacheLogLevel != null)
                    {
                        _SysLogLevel = cacheLogLevel.ToString();
                    }
                    else
                    {
                        _SysLogLevel = Configuration["LogLevel"];
                    }
                }
                int level = 0;
                if (!string.IsNullOrEmpty(_SysLogLevel))
                {
                    switch (_SysLogLevel)
                    {
                        case "DEBUG":
                            level = (int)AprilEnums.LogLevel.DEBUG;
                            break;
                        case "INFO":
                            level = (int)AprilEnums.LogLevel.INFO;
                            break;
                        case "ERROR":
                            level = (int)AprilEnums.LogLevel.ERROR;
                            break;
                        case "FATAL":
                            level = (int)AprilEnums.LogLevel.FATAL;
                            break;
                    }
                }
                return level;
            }
        }
        #endregion

        #region ========Redis相关========
        private static string _RedisHosts = string.Empty;
        private static string _IsOpenRedis = string.Empty;
        /// <summary>
        /// 是否打开Redis
        /// </summary>
        public static bool IsOpenRedis
        {
            get
            {
                if (string.IsNullOrEmpty(_IsOpenRedis))
                {
                    _IsOpenRedis = Configuration["RedisSettings:IsOpenCache"];
                }
                if (!string.IsNullOrEmpty(_IsOpenRedis) && _IsOpenRedis.ToLower() == "true")
                {
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// 本地端口地址
        /// </summary>
        public static string RedisHosts
        {
            get
            {
                if (string.IsNullOrEmpty(_RedisHosts))
                {
                    _RedisHosts = Configuration["RedisSettings:Hosts"];
                }
                return _RedisHosts;
            }
        }
        #endregion

        #region ========性能相关========
        private static string _IsOpenWatch = string.Empty;
        /// <summary>
        /// 接口检测时间
        /// </summary>
        public static bool IsOpenWatch
        {
            get
            {
                if (string.IsNullOrEmpty(_IsOpenWatch))
                {
                    _IsOpenWatch = Configuration["CommonSettings:IsOpenWatch"];
                }
                if (!string.IsNullOrEmpty(_IsOpenWatch) && _IsOpenWatch.ToLower() == "true") 
                {
                    return true;
                }
                return false;
            }
        }
        #endregion

        #region========权限登录========
        private static string _TokenSecretFormat = string.Empty;
        private static string _TokenCacheType = string.Empty;
        private static string _AllowSliding = string.Empty;
        private static string _AllowMuiltiLogin = string.Empty;
        /// <summary>
        /// token加密串格式
        /// </summary>
        public static string TokenSecretFormat
        {
            get
            {
                if (string.IsNullOrEmpty(_TokenSecretFormat))
                {
                    _TokenSecretFormat = Configuration["LoginSettings:TokenSecretFormat"];
                }
                if (string.IsNullOrEmpty(_TokenSecretFormat))
                {
                    //不设置采取默认
                    _TokenSecretFormat = "april{id}&&&&{ts}";
                }
                return _TokenSecretFormat;
            }
        }
        /// <summary>
        /// 存储Token方式
        /// </summary>
        public static AprilEnums.TokenCacheType TokenCacheType
        {
            get
            {
                if (string.IsNullOrEmpty(_TokenCacheType))
                {
                    _TokenCacheType = Configuration["LoginSettings:TokenCacheType"];
                }
                if (string.IsNullOrEmpty(_TokenCacheType))
                {
                    //不设置采取默认
                    _TokenCacheType = "Session";
                }
                switch (_TokenCacheType.ToLower())
                {
                    case "cookie":
                        return AprilEnums.TokenCacheType.Cookie;
                        break;
                    case "cache":
                        return AprilEnums.TokenCacheType.Cache;
                        break;
                    case "session":
                        return AprilEnums.TokenCacheType.Session;
                        break;
                    case "redis":
                        return AprilEnums.TokenCacheType.Redis;
                        break;
                    default:
                        return AprilEnums.TokenCacheType.Session;
                        break;
                }
            }
        }
        /// <summary>
        /// 允许滑动过期
        /// </summary>
        public static bool AllowSliding
        {
            get
            {
                if (string.IsNullOrEmpty(_AllowSliding))
                {
                    _AllowSliding = Configuration["LoginSettings:AllowSliding"];
                }
                if (!string.IsNullOrEmpty(_AllowSliding) && _AllowSliding.ToLower() == "true")
                {
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// 多端登录
        /// </summary>
        public static bool AllowMuiltiLogin
        {
            get
            {
                if (string.IsNullOrEmpty(_AllowMuiltiLogin))
                {
                    _AllowMuiltiLogin = Configuration["LoginSettings:AllowMuiltiLogin"];
                }
                if (!string.IsNullOrEmpty(_AllowMuiltiLogin) && _AllowMuiltiLogin.ToLower() == "true")
                {
                    return true;
                }
                return false;
            }
        }
        #endregion

        #region ========当前页面========
        /// <summary>
        /// 当前请求页面
        /// </summary>
        public static HttpContext HttpCurrent
        {
            get
            { 
                object factory = ServiceProvider.GetService(typeof(IHttpContextAccessor));
                HttpContext context = ((IHttpContextAccessor)factory).HttpContext;
                return context;
            }
        }
        #endregion
    }
}
