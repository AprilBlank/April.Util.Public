namespace April.Util.Config
{
    public class AprilEnums
    {
        /// <summary>
        /// 日志级别
        /// </summary>
        public enum LogLevel
        {
            DEBUG,
            INFO,
            WARN,
            ERROR,
            FATAL
        }
        /// <summary>
        /// 登录方式
        /// </summary>
        public enum TokenType
        {
            Web,
            App,
            MiniProgram,
            Other
        }
        /// <summary>
        /// 管理员缓存获取方式
        /// </summary>
        public enum TokenCacheType
        { 
            Cookie,
            Cache,
            Session,
            Redis
        }

    }
}
