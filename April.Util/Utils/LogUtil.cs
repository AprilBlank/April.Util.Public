using April.Util.Config;
using log4net;
using log4net.Config;
using log4net.Repository;
using Newtonsoft.Json;
using System;
using System.IO;

namespace April.Util
{
    public class LogUtil
    {
        private static ILog log;

        //log4net日志
        private static ILoggerRepository logger { get; set; }

        /// <summary>
        /// 初始化日志配置
        /// </summary>
        public static void InitLog()
        {
            logger = LogManager.CreateRepository("April.Log");
            FileInfo file = new FileInfo(AprilConfig.Configuration["FilePath:Log4"]);
            XmlConfigurator.Configure(logger, file);
            BasicConfigurator.Configure(logger);
            log = LogManager.GetLogger(logger.Name, typeof(LogUtil));
            //Debug($"初始化日志配置:{file.FullName}");
        }

        /// <summary>
        /// 记录调试日志
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="objs">参数</param>
        public static void Debug(string msg, params object[] objs)
        {
            if (AprilConfig.SysLogLevel <= (int)AprilEnums.LogLevel.DEBUG && !string.IsNullOrEmpty(msg))
            {
                if (objs == null)
                {
                    log.Debug(msg);
                }
                else
                {
                    log.DebugFormat(msg, objs);
                }
            }
        }
        /// <summary>
        /// 记录日常日志
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="objs">参数</param>
        public static void Info(string msg, params object[] objs)
        {
            if (AprilConfig.SysLogLevel <= (int)AprilEnums.LogLevel.INFO && !string.IsNullOrEmpty(msg))
            {
                if (objs == null)
                {
                    log.Info(msg);
                }
                else
                {
                    log.InfoFormat(msg, objs);
                }
            }
        }
        /// <summary>
        /// 记录警告日志
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="objs">参数</param>
        public static void Warn(string msg, params object[] objs)
        {
            if (AprilConfig.SysLogLevel <= (int)AprilEnums.LogLevel.WARN && !string.IsNullOrEmpty(msg))
            {
                if (objs == null)
                {
                    log.Warn(msg);
                }
                else
                {
                    log.WarnFormat(msg, objs);
                }
            }
        }
        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="objs">参数</param>
        public static void Error(string msg, params object[] objs)
        {
            if (AprilConfig.SysLogLevel <= (int)AprilEnums.LogLevel.ERROR && !string.IsNullOrEmpty(msg))
            {
                if (objs == null)
                {
                    log.Error(msg);
                }
                else
                {
                    log.ErrorFormat(msg, objs);
                }
            }
        }
        /// <summary>
        /// 记录重要日志
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="objs">参数</param>
        public static void Fatal(string msg, params object[] objs)
        {
            if (AprilConfig.SysLogLevel <= (int)AprilEnums.LogLevel.FATAL && !string.IsNullOrEmpty(msg))
            {
                if (objs == null)
                {
                    log.Fatal(msg);
                }
                else
                {
                    log.FatalFormat(msg, objs);
                }
            }
        }

        #region 特殊日志文件记录
        /// <summary>
        /// 记录Sql日志
        /// </summary>
        /// <param name="sql">执行的Sql语句</param>
        /// <param name="obj">参数</param>
        /// <param name="isError">是否为错误日志</param>
        /// <param name="errormsg">错误信息</param>
        public static void WriteLogForSql(string sql, object obj, bool isError = false, string errormsg = "")
        {
            try
            {
                lock (lockmodel)
                {
                    string extramsg = string.Empty;
                    if (obj != null)
                    {
                        extramsg = JsonConvert.SerializeObject(obj);
                    }
                    string filename = AppDomain.CurrentDomain.DynamicDirectory + string.Format("logs\\sqllog\\{0}.txt", DateTime.Now.ToString("yyyyMMddHH"));
                    if (isError)
                    {
                        filename = AppDomain.CurrentDomain.DynamicDirectory + string.Format("logs\\sqlerror\\{0}.txt", DateTime.Now.ToString("yyyyMMddHH"));
                    }
                    string dic = Path.GetDirectoryName(filename);
                    if (!Directory.Exists(dic))
                    {
                        Directory.CreateDirectory(dic);
                    }
                    using (StreamWriter sw = new StreamWriter(filename, true, System.Text.Encoding.UTF8))
                    {
                        sw.WriteLine("========================================");
                        sw.WriteLine("****************************************");
                        sw.WriteLine("========================================");
                        sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":");
                        sw.Write(sql);
                        sw.WriteLine("数据详情:" + extramsg);
                        if (isError)
                        {
                            sw.WriteLine(string.Empty);
                            sw.Write(errormsg);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="msg">日志信息</param>
        public static void WriteLog(string msg)
        {
            try
            {
                lock (lockmodel)
                {
                    string filename = AppDomain.CurrentDomain.DynamicDirectory + string.Format("logs\\sqllog\\{0}.txt", DateTime.Now.ToString("yyyyMMddHH"));
                    string dic = Path.GetDirectoryName(filename);
                    if (!Directory.Exists(dic))
                    {
                        Directory.CreateDirectory(dic);
                    }
                    using (StreamWriter sw = new StreamWriter(filename, true, System.Text.Encoding.UTF8))
                    {
                        sw.WriteLine("========================================");
                        sw.WriteLine("****************************************");
                        sw.WriteLine("========================================");
                        sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":");
                        sw.Write(msg);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static object lockmodel = new object();
        #endregion
    }
}
