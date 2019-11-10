using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using April.Util;
using April.Util.Config;
using April.Util.Entities.Admin;
using April.Util.Entities.Request;
using April.Util.Entities.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace April.Simple.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public async Task<ResponseDataEntity> Login(LoginFormEntity formEntity)
        {
            if (string.IsNullOrEmpty(formEntity.LoginName) || string.IsNullOrEmpty(formEntity.Password))
            {
                return ResponseUtil.Fail("请输入账号密码");
            }
            if (formEntity.LoginName == "admin")
            {
                //这里实际应该通过db获取管理员
                string password = EncryptUtil.MD5Encrypt(formEntity.Password, AprilConfig.SecurityKey);
                if (password == "B092956160CB0018")
                {
                    //获取管理员相关权限，同样是db获取，这里只做展示
                    AdminEntity admin = new AdminEntity
                    {
                        UserName = "超级管理员",
                        Avator = "",
                        IsSuperManager = true,
                        TokenType = (int)AprilEnums.TokenType.Web
                    };
                    string token = TokenUtil.GetToken(admin, out string expiretimestamp);
                    int expiretime = 0;
                    int.TryParse(expiretimestamp, out expiretime);
                    //可以考虑记录登录日志等其他信息
                    return ResponseUtil.Success("", new { username = admin.UserName, avator = admin.Avator, token = token, expire = expiretime });
                }
            }
            else if (formEntity.LoginName == "test")
            {
                //这里做权限演示
                AdminEntity admin = new AdminEntity
                {
                    UserName = "测试",
                    Avator = "",
                    TokenType = (int)AprilEnums.TokenType.Web
                };
                admin.Controllers.Add("weatherforecast");
                admin.Permissions.Add("weatherforecast_log");//控制器_事件(Add,Update...)
                string token = TokenUtil.GetToken(admin, out string expiretimestamp);
                int expiretime = 0;
                int.TryParse(expiretimestamp, out expiretime);
                //可以考虑记录登录日志等其他信息
                return ResponseUtil.Success("", new { username = admin.UserName, avator = admin.Avator, token = token, expire = expiretime });
            }
            //这里其实已经可以考虑验证码相关了，但是这是示例工程，后续可持续关注我，会有基础工程（带权限）的实例公开
            return ResponseUtil.Fail("账号密码错误");
        }
    }
}