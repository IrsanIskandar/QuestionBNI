using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MMLib.SwaggerForOcelot.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PenyewaanMobil.APIGATEWAY
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel();
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureWebHostDefaults(webbuilder =>
                {
                    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                    webbuilder.ConfigureAppConfiguration((host, config) =>
                    {
                        var ocelotConfigPath = Path.Combine(host.HostingEnvironment.ContentRootPath, "RoutingUrls", "Version1");
                        config.AddJsonFile($"UrlRouting.{env}.json", optional: false, reloadOnChange: true);
                        config.AddOcelotWithSwaggerSupport((opt) =>
                        {
                            opt.FileOfSwaggerEndPoints = $"UrlRouting.{env}";
                        });
                    });
                });
    }
}
