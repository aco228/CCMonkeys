﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCMonkeys.Web.Code.Sockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CCMonkeys.Web
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      //JsonConvert.DefaultSettings = () => new JsonSerializerSettings
      //{
      //  Formatting = Newtonsoft.Json.Formatting.Indented,
      //  ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
      //};

      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddCors(options =>
      {
        options.AddPolicy("get",
            builder => builder
            .SetIsOriginAllowed( (host) => { Console.WriteLine("HOST: " + host); return true; })
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
      });

      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
      services.AddApplicationInsightsTelemetry(Configuration);
      services.Configure<CookiePolicyOptions>(options =>
      {
        // This lambda determines whether user consent for non-essential cookies is needed for a given request.
        //options.CheckConsentNeeded = context => true;
        options.MinimumSameSitePolicy = SameSiteMode.None;
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseHsts();
      }

      app.UseWebSockets();
      app.UseMiddleware<WebSocketsMiddleware>();
      //app.UseCors("get");
      app.UseHttpsRedirection();
      app.UseMvc(routes =>
      {
        routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
      });
    }
  }
}
