using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using April.Util;
using April.Util.Aop;
using April.Util.Config;
using April.Util.Entities.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace April.Simple.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 示例获取
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AprilLog]
        public ResponseDataEntity Get()
        {
            var rng = new Random();
            var data = Enumerable.Range(1, 2).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)],
                Timestamp = (int)DateUtil.ConvertToUnixTimestamp(DateTime.Now.AddDays(index))
            })
            .ToList();
            return ResponseUtil.Success("", data);
        }

        /// <summary>
        /// 验证码
        /// </summary>
        /// <param name="type">是否画线</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Code")]
        public FileContentResult Code(int type)
        {
            string code = CodeUtil.GetSingleObj().CreateVerifyCode(CodeUtil.VerifyCodeType.MixVerifyCode, 6);
            //LogUtil.Info($"生成6位验证码:{code}");
            bool isAddLines = false;
            if (type == 1)
            {
                isAddLines = true;
            }
            var bitmap = CodeUtil.GetSingleObj().CreateBitmapByImgVerifyCode(code, 100, 40, isAddLines);
            code = EncryptUtil.EncryptDES(code, AprilConfig.SecurityKey);
            CookieUtil.AddString("code", code, 5);
            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Gif);
            return File(stream.ToArray(), "image/gif");
        }
    }
}
