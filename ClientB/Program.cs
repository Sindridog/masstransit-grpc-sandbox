using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.Reflection;

namespace ClientB
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(
                   new LoggerConfiguration()
                       .MinimumLevel.Debug()
                       .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                       .MinimumLevel.Override("System", LogEventLevel.Error)
                       .Enrich.FromLogContext()
                       .WriteTo.Console()
                       .CreateLogger(),
                   dispose: true));

                services.AddMassTransit(x =>
                {
                    x.AddConsumer<MessageConsumer>();
                    x.SetKebabCaseEndpointNameFormatter();
                    x.UsingGrpc((context, cfg) =>
                    {
                        cfg.Host(h =>
                        {
                            Uri uri = new Uri("http://localhost:12001");
                            h.Host = uri.Host;
                            h.Port = uri.Port;
                            //h.AddServer(new Uri("http://localhost:11001")); //ClientA
                            //h.AddServer(new Uri("http://localhost:10010")); //Server
                        });

                        cfg.ConfigureEndpoints(context);
                    });

                });

                services.AddMassTransitHostedService(true);
            });
    }
}
