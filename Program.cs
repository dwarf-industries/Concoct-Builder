namespace Concoct_Builder
{

    using ElectronNET.API;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Program
    {
        private static List<string> ActiveUniqueIds { get; set; }

        public static void Main(string[] args)
        {
            ActiveUniqueIds = new List<string>();
            CreateHostBuilder(args).Build().Run();
        }


        public static string GetUniqueId()
        {
            var chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            var random = new System.Random();
            var uniqueName = $"{Guid.NewGuid().ToString().Split('-').FirstOrDefault()}{chars[random.Next(chars.Length - 1)]}{chars[random.Next(chars.Length + 1)]}{chars[random.Next(chars.Length - 2)]}";
            if (ActiveUniqueIds.Any(x => x == uniqueName))
                ActiveUniqueIds.Add($"{uniqueName}{chars[random.Next(chars.Length - 1)]}");
            else
                ActiveUniqueIds.Add(uniqueName);

            return uniqueName;
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
         Host.CreateDefaultBuilder(args)
             .ConfigureWebHostDefaults(webBuilder =>
             {
                 webBuilder.UseElectron(args);
                 webBuilder.UseStartup<Startup>();
             });
    }
}
