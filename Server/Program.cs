using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace Server
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
                services.AddMassTransit(x =>
                {
                    //x.AddConsumer<MessageConsumer>();
                    x.SetKebabCaseEndpointNameFormatter();
                    x.UsingGrpc((context, cfg) =>
                    {
                        cfg.Host(h =>
                        {
                            Uri uri = new Uri("http://localhost:10010"); 
                            h.Host = uri.Host;
                            h.Port = uri.Port;
                            //h.AddServer(new Uri("http://localhost:11001")); //ClientA
                            //h.AddServer(new Uri("http://localhost:12001")); //ClientB
                        });

                        cfg.ConfigureEndpoints(context);
                    });
                });

                services.AddMassTransitHostedService(true);
            });
    }
}
