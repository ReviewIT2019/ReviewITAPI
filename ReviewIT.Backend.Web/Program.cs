using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;

namespace ReviewIT.Backend.Web
{
    public class Program
    {
        public static int Main(string[] args)
        {
            const string Path = @"D:\home\LogFiles\Application\myapp.txt";

            // Setup SeriLog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level: u3}] ({SourceContext}) {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(
                path: Path,
                fileSizeLimitBytes: 1_000_000,
                rollOnFileSizeLimit: true,
                shared: true,
                flushToDiskInterval: TimeSpan.FromSeconds(1))
                .CreateLogger();

            try
            {
                Log.Information("Starting Web Host");

                CreateWebHostBuilder(args).Run();

                return 0;
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Host Terminated Unexpectedly!");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHost CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddEnvironmentVariables();
            })
            .UseSerilog()
            .UseStartup<Startup>()
            .Build();
    }
}
