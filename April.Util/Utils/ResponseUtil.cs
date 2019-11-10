using April.Util.Config;
using April.Util.Entities.Response;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace April.Util
{
    public class ResponseUtil
    {

        /// <summary>
        /// 回调
        /// </summary>
        /// <param name="statusCode">html状态码</param>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static Task HandleResponse(int statusCode, string msg)
        {
            var data = new { code = statusCode, msg = msg };
            string text = JsonConvert.SerializeObject(data);
            var response = AprilConfig.HttpCurrent.Response;
            if (string.IsNullOrEmpty(response.ContentType))
            {
                //跨域的时候注意，不带header没法接收回调
                response.Headers.Add("Access-Control-Allow-Origin", "*");
                response.Headers.Add("Access-Control-Allow-Credentials", "true");
                //因为这个是json
                response.ContentType = "application/json;charset=utf-8";
                response.StatusCode = 200;
                return response.WriteAsync(text);
            }
            else
            {
                return response.WriteAsync(text);
            }
        }

        /// <summary>
        /// 成功回调
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public static ResponseDataEntity Success(string msg = "", object data = null)
        {
            return Info(0, msg, data);
        }
        /// <summary>
        /// 失败回调
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public static ResponseDataEntity Fail(string msg = "", object data = null)
        {
            return Info(-1, msg, data);
        }
        /// <summary>
        /// 未登录
        /// </summary>
        /// <returns></returns>
        public static ResponseDataEntity NoLogin()
        {
            return Info(-2, "请重新登录");
        }
        /// <summary>
        /// 无权限
        /// </summary>
        /// <returns></returns>
        public static ResponseDataEntity NoPermission()
        {
            return Info(401, "无权访问");
        }
        /// <summary>
        /// 常规回调
        /// </summary>
        /// <param name="code">回调码</param>
        /// <param name="msg">消息</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public static ResponseDataEntity Info(int code, string msg = "", object data = null)
        {
            ResponseDataEntity message = new ResponseDataEntity();
            message.Code = code;
            message.Msg = msg;
            message.Data = data;
            return message;
        }
    }
}
