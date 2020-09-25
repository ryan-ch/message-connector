using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XB.IBM.MQ;

namespace XB.Astrea.Connector
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
                    services.AddAstreaClient();
                    services.AddMqClient();

                    services.AddHostedService<Worker>();
                });
    }
}
