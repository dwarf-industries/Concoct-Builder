namespace Concoct_Builder
{
    using Concoct_Builder.Datalayer;
    using Concoct_Builder.Models;
    using ElectronNET.API;
    using ElectronNET.API.Entities;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Linq;
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
            services.AddDbContext<ConcoctbuilderDbContext>(options => options.UseSqlite("Data Source=ConcoctBuilder.db"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IFileHandler fileHandler, ConcoctbuilderDbContext context)
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
            var settings = fileHandler.GetUserSettings();
            var activeSetting = default(UserSettings);
            context = new ConcoctbuilderDbContext();

            if (Settings == null)
                Settings = new Settings();

            if (settings.Count >0)
                activeSetting = settings.FirstOrDefault(x => x.IsActive == 1);
            else
            {
                activeSetting = context.UserSettings.Add(new UserSettings
                {
                    Key = new Guid().ToString(),
                    OrganizationName = "Offline",
                    IsActive = 1,
                    Username = "--",
                    Password = "--",
                    ConnectionType = 0,
                    Endpoint = "--",
                    InstanceAddress = "Local"
                }).Entity;
                context.SaveChanges();
            }


            Settings.SingInType = activeSetting.ConnectionType.ToString();
            Settings.ConcoctInstance = activeSetting.InstanceAddress;
            Settings.UserName = activeSetting.Username;
            Settings.Password = activeSetting.Password;
            Settings.APIKey = activeSetting.Key;

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
            options.Fullscreen = false;
            options.Title = "Concoct Builder V 0.1";
            options.TitleBarStyle = TitleBarStyle.hidden;
            options.Frame = false;
            options.FullscreenWindowTitle = true;
            options.Closable = true;
            options.Fullscreenable = true;
 
            Task.Run(async () => await Electron.WindowManager.CreateWindowAsync(options));

        }
    }                       
}
