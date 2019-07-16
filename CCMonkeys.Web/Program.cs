using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sentry;

namespace CCMonkeys.Web
{
  public class Program
  {
    public static void Main(string[] args)
    {
      using (SentrySdk.Init("https://e2a9518558524ceeafd180cf83556583@sentry.io/1505328"))
      {
        CreateWebHostBuilder(args).Build().Run();
      }
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>();
  }
}
