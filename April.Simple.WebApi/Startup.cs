using April.Util;
using April.Util.Config;
using April.Util.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.IO;

namespace April.Simple.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            AprilConfig.Configuration = configuration;
            LogUtil.InitLog();

            RedisUtil.InitRedis();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                //����ѭ������
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //��ʹ���շ���ʽ��key
                options.SerializerSettings.ContractResolver = new LowerCasePropertyNamesContractResolver();
                //����ʱ���ʽ
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm";
                //��ֵ����
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            #region Swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1.1.0",
                    Title = "April WebAPI",
                    Description = "��̨���",
                    Contact = new OpenApiContact() { Name = "Blank", Email = "1829027193@qq.com", Url = new Uri("https://www.cnblogs.com/AprilBlank/") }
                });
                // Ϊ Swagger JSON and UI����xml�ĵ�ע��·��
                var basePath = Path.GetDirectoryName(AppContext.BaseDirectory);//��ȡӦ�ó�������Ŀ¼�����ԣ����ܹ���Ŀ¼Ӱ�죬������ô˷�����ȡ·����
                var xmlPath = Path.Combine(basePath, "April.xml");
                options.IncludeXmlComments(xmlPath);
            });
            #endregion

            #region Session
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.Name = "April.Session";
                options.IdleTimeout = TimeSpan.FromSeconds(2000);//����session�Ĺ���ʱ��
                options.Cookie.HttpOnly = true;//���������������ͨ��js��ø�cookie��ֵ,ʵ�ʳ�������������Ҫ
                options.Cookie.IsEssential = true;
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            AprilConfig.ServiceProvider = app.ApplicationServices;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
                //������ø�Ŀ¼Ϊswagger,����ֵ�ÿ�
                options.RoutePrefix = string.Empty;
            });
            #endregion
            app.UseSession();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
