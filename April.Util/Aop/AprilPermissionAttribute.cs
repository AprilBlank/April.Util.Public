using April.Util.Entities.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace April.Util.Aop
{
    public class AprilPermissionAttribute : Attribute, IActionFilter
    {

        public string Permission;
        public string Controller;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_controller">控制器</param>
        /// <param name="_permission">接口事件</param>
        public AprilPermissionAttribute(string _controller, string _permission)
        {
            Permission = _permission;
            Controller = _controller;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            LogUtil.Debug("AprilPermission OnActionExecuted");
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            AdminEntity admin = TokenUtil.GetUserByToken();
            if (admin == null || admin.ExpireTime <= DateTime.Now)
            {
                context.Result = new ObjectResult(new { msg = "未登录", code = -2 });
            }
            if (!admin.IsSuperManager)
            {
                string controller_permission = $"{Controller}_{Permission}";
                if (!admin.Controllers.Contains(Controller) || !admin.Permissions.Contains(controller_permission))
                {
                    context.Result = new ObjectResult(new { msg = "无权访问", code = 401 });
                }
            }
        }
    }
}
