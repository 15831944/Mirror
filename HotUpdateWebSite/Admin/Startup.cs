using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Admin.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Admin
{
    public class Startup
    {
        public HotUpdateContainer Container { get; private set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            AssemblyName assemblyName = new AssemblyName(args.Name);
            var obj = AppDomain.CurrentDomain.GetData(assemblyName.Name);
            if (obj != null) return (Assembly)obj;

          
            var path = Path.Combine(AppContext.BaseDirectory, $"{assemblyName.Name}.dll");
            if (File.Exists(path))
            {
                byte[] data = File.ReadAllBytes(path);
                var assembly = Assembly.Load(data);
                AppDomain.CurrentDomain.SetData(assemblyName.Name, assembly);
                return assembly;
            }
            return null;
            //return args.RequestingAssembly;
            //throw new NotImplementedException();


        }

        public IConfiguration Configuration { get; }



        /// <summary>
        /// ����Լ��ķ���Ͷ��� ��������ȥ
        /// </summary>
        /// <param name="services"></param>
        //public IServiceProvider ConfigureServices(IServiceCollection services)
        public IServiceProvider ConfigureServices(IServiceCollection services)

        {
            services.AddMvc().ConfigureApplicationPartManager(manager =>
            {
                //�Ƴ�ASP.NET CORE MVC��������Ĭ�����õ�MetadataReferenceFeatureProvider����Provider������Ƴ������ǻ�����InvalidOperationException: Cannot find compilation library location for package 'MyNetCoreLib'�������
                manager.FeatureProviders.Remove(manager.FeatureProviders.First(f => f is MetadataReferenceFeatureProvider));
                //ע�����Ƕ����ReferencesMetadataReferenceFeatureProvider��ASP.NET CORE MVC�����������������Ƴ���MetadataReferenceFeatureProvider
                manager.FeatureProviders.Add(new HotUpdateMetadataReferenceFeatureProvider());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //�����������ȫ��ʵ��Provider���� �Ա�������
            services.AddSingleton<IActionDescriptorChangeProvider>(HotUpdateActionDescriptorChangeProvider.Instance);
            services.AddSingleton(HotUpdateActionDescriptorChangeProvider.Instance);

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc();
            var hotUpdateContainer = new HotUpdateContainer();
            var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
            hotUpdateContainer.RegisterAssemblyPaths(Path.Combine(basePath, "Services.dll"));
            return new HotUpdateServiceProvider(services, hotUpdateContainer);
        }

        /// <summary>
        /// ��ӹܵ��¼�  �м��
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "Default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "HotUpdate", action = "Index" }
                );
            });
        }
    }
}
