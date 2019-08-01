using CCMonkeys.Web.Core;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Sentry;

namespace CCMonkeys.Web
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CacheManager.Init();

      using (SentrySdk.Init("https://e2a9518558524ceeafd180cf83556583@sentry.io/1505328"))
      {
        CreateWebHostBuilder(args)
          .UseKestrel()
          //.UseIISIntegration() // Necessary for Azure.
          .UseSentry()
          .Build()
          .Run();
      }

    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>();
  }
}
