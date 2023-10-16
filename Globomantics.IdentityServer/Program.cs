using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Globomantics.IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((ctx, provider, loggerConfig) =>
                {
                    loggerConfig
                        .ReadFrom.Configuration(ctx.Configuration) // minimum levels defined per project in json files 
                        .Enrich.FromLogContext()
                        .WriteTo.Console()
                        .WriteTo.Seq("http://globoseq");
                        //.WriteTo.Seq("http://host.docker.internal:5341");
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //https://andrewlock.net/5-ways-to-set-the-urls-for-an-aspnetcore-app/
                    webBuilder.UseStartup<Startup>();
                });
    }
}
