using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Reflection;
using NLog.Extensions.Logging;

namespace CareersPortal.Web
{
    public class CareersPortal
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        internal static IHostBuilder CreateDefaultHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        internal static IWebHostBuilder CreateHostBuilder(string[] args)
        {
             return new WebHostBuilder()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseWebRoot("wwwroot")
                    .ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        var env = hostingContext.HostingEnvironment;

                        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                              .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

                        if (env.IsDevelopment())
                        {
                            var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                            if (appAssembly != null)
                            {
                                config.AddUserSecrets(appAssembly, optional: true);
                            }
                        }

                        config.AddEnvironmentVariables();

                        if (args != null)
                        {
                            config.AddCommandLine(args);
                        }
                    })
                    .ConfigureLogging((hostingContext, loggingBuilder) =>
                                {
                                    loggingBuilder.ClearProviders()
                                    .AddConfiguration(hostingContext.Configuration.GetSection("Logging"))
                                    .AddDebug()
                                    .AddNLog();
                                })
                    .UseDefaultServiceProvider((context, options) =>
                                {
                                    options.ValidateScopes = true;//context.HostingEnvironment.IsDevelopment();
                                })
                    .UseKestrel()
                    .UseIIS()
                    .UseIISIntegration()
                    .UseStartup<Startup>();
        }
            
    }
}
