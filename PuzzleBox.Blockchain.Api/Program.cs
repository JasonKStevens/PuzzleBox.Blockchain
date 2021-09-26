using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Serilog;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using PuzzleBox.Blockchain.Api.Services;

namespace PuzzleBox.Blockchain.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            await CreateHost(args)
                .Build()
                .RunAsync();
        }

        private static IHostBuilder CreateHost(string[] args)
        {
            var configBuilder = new ConfigurationBuilder().AddCommandLine(args);
            var config = configBuilder.Build();
            var hostPort = config.GetValue<int>("host");
            var restPort = config.GetValue<int?>("rest");

            var builder = Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options =>
                    {
                        if (restPort.HasValue)
                        {
                            options.ListenLocalhost(restPort.Value, listenOptions =>
                            {
                                listenOptions.Protocols = HttpProtocols.Http1;
                            });
                        }

                        options.ListenLocalhost(hostPort, listenOptions =>
                        {
                            listenOptions.Protocols = HttpProtocols.Http2;
                        });
                    });

                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices(services =>
                {
                    services.AddHostedService<NodeService>();
                })
                .UseSerilog();

            return builder;
        }
    }
}
