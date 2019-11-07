using April.Util.Entities.Response;

namespace April.Util
{
    public class ResponseUtil
    {
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
