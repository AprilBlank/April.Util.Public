using April.Util.Config;
using April.Util.Entities.Admin;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace April.Util.Middles
{
    public class AprilAuthorizationMiddleware
    {
        private readonly RequestDelegate next;

        public AprilAuthorizationMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public Task Invoke(HttpContext context)
        {
            if (context.Request.Method != "OPTIONS")
            {
                string path = context.Request.Path.Value;
                if (!AprilConfig.AllowUrl.Contains(path))
                {
                    //获取管理员信息
                    AdminEntity admin = TokenUtil.GetUserByToken();
                    if (admin == null)
                    {
                        //重新登录
                        return ResponseUtil.HandleResponse(-2, "未登录");
                    }
                    if (!admin.IsSuperManager)
                    {
                        //格式统一为/api/Controller/Action，兼容多级如/api/Controller1/ConrolerInnerName/xxx/Action
                        string[] strValues = System.Text.RegularExpressions.Regex.Split(path, "/");

                        string controller = "";
                        bool isStartApi = false;
                        if (path.StartsWith("/api"))
                        {
                            isStartApi = true;
                        }
                        for (int i = 0; i < strValues.Length; i++)
                        {
                            //为空，为api，或者最后一个
                            if (string.IsNullOrEmpty(strValues[i]) || i == strValues.Length - 1)
                            {
                                continue;
                            }
                            if (isStartApi && strValues[i] == "api")
                            {
                                continue;
                            }
                            if (!string.IsNullOrEmpty(controller))
                            {
                                controller += "/";
                            }
                            controller += strValues[i];
                        }
                        if (string.IsNullOrEmpty(controller))
                        {
                            controller = strValues[strValues.Length - 1];
                        }
                        if (!admin.Controllers.Contains(controller.ToLower()))
                        {
                            //无权访问
                            return ResponseUtil.HandleResponse(401, "无权访问");
                        }

                    }
                }
            }
            return next.Invoke(context);
        }
    }
}
