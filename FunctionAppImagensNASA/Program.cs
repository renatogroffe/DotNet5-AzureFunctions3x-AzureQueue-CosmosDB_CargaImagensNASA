using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Refit;
using FunctionAppImagensNASA.HttpClients;

namespace FunctionAppImagensNASA
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(services => {
                    services.AddRefitClient<IImagemDiariaAPI>()
                        .ConfigureHttpClient(
                            c => c.BaseAddress = new Uri(Environment.GetEnvironmentVariable("EndpointNASA")));
                })
                .Build();

            host.Run();
        }
    }
}