using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CommunicationService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                hostBuilder.UseWindowsService();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                hostBuilder.UseSystemd();

            hostBuilder.ConfigureServices((hostContext, services) =>
                    {
                        services.AddHostedService<Worker>();
                    });


            return hostBuilder;
        }
    }
}
