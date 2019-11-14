using CCMonkeys.Web.Core;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Sentry;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CCMonkeys.Web
{
  public class Program
  {
    private static CancellationTokenSource cancelTokenSource = new System.Threading.CancellationTokenSource();
    public static DateTime Created;

    public static void Main(string[] args)
    {
      var host = CreateWebHostBuilder(args)
        .UseKestrel()
        .UseIISIntegration() // Necessary for Azure.
                             //.UseSentry()
        .Build();

      Created = DateTime.Now;
      host.RunAsync(cancelTokenSource.Token).GetAwaiter().GetResult();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>();

    public static void Restart() 
      => cancelTokenSource.Cancel();

  }
}
