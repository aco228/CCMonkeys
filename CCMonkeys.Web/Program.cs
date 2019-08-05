using CCMonkeys.Web.Core;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Sentry;
using System.Threading.Tasks;

namespace CCMonkeys.Web
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateWebHostBuilder(args)
        .UseKestrel()
        .UseIISIntegration() // Necessary for Azure.
                             //.UseSentry()
        .Build()
        .Run();

    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>();
  }
}
