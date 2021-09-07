namespace Concoct_Builder
{
    using Concoct_Builder.Datalayer;
    using Concoct_Builder.Models;
    using ElectronNET.API;
    using ElectronNET.API.Entities;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using System.Threading.Tasks;

    public class Startup
    {
        public static Settings Settings { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IFileHandler, FileHandler>();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IFileHandler fileHandler)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseDeveloperExceptionPage();

                // app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }
            var rawSettings = fileHandler.ReadFileRaw("Settings.cf");
            Settings = fileHandler.ReadConfig(rawSettings);
            if (Settings == null)
                return;

            var getOs = new OS();

            switch (OS.GetCurrent())
            {
                case "gnu":
                    Settings.SystemFolderDelimiter = @"\";
                    break;
                case "win":
                    Settings.SystemFolderDelimiter = @"/";
                    break;
                case "mac":
                    Settings.SystemFolderDelimiter = @"\";
                    break;
                default:
                    Settings.SystemFolderDelimiter = @"\";
                    break;
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            var options = new BrowserWindowOptions();
            options.AutoHideMenuBar = true;
            options.DarkTheme = true;
            options.Fullscreen = true;
            options.Title = "Concoct Builder V 0.1";
            options.TitleBarStyle = TitleBarStyle.hidden;
          
            Task.Run(async () => await Electron.WindowManager.CreateWindowAsync(options));

        }
    }                       
}
