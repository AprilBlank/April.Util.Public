using April.Util.Config;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;

namespace April.Util.Aop
{
    public class AprilLogAttribute : Attribute, IActionFilter
    {
        private Stopwatch stopWatch = null;
        private string requestID;
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (AprilConfig.IsOpenWatch)
            {
                stopWatch.Stop();
                string result = JsonConvert.SerializeObject(context.Result);
                result = result.Replace("{", "&lt").Replace("}", "&gt");
                LogUtil.Debug("AprilLog OnActionExecuted");
                LogUtil.Debug($"\r\n[{requestID}_回调]\r\n回调数据:{result}\r\n用时:{stopWatch.Elapsed.TotalMilliseconds.ToString("0")}ms");
            }
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (AprilConfig.IsOpenWatch)
            {
                stopWatch = new Stopwatch();
                requestID = DateTime.Now.Ticks.ToString();
                stopWatch.Start();
                string requestQuery = context.HttpContext.Request.QueryString.Value;
                string requestData = "";
                if (context.HttpContext.Request.Method != "GET")
                {
                    Stream stream = context.HttpContext.Request.Body;
                    if (stream != null && stream.Length > 0)
                    {
                        using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8))
                        {
                            requestData = reader.ReadToEnd().ToString();
                        }
                    }
                }
                LogUtil.Debug("AprilLog OnActionExecuting");
                LogUtil.Debug($"\r\n[{requestID}_发起请求]\r\n路径:{context.HttpContext.Request.Path}\r\n页面请求参数:{requestQuery}\r\n页面body:{requestData}\r\n");
            }
        }
    }
}
