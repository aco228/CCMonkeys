using CCMonkeys.Web.Core;
using CCMonkeys.Web.Core.Sockets.ApiSockets;
using CCMonkeys.Web.Core.Sockets.Dashboard;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CCMonkeys.Web.Code.Sockets
{
  public class WebSocketsMiddleware
  {
    private readonly RequestDelegate _next;
    public static ApiSocketServer ApiSocketServer = new ApiSocketServer();
    public static DashboardSocketsServer DashboardServer = new DashboardSocketsServer();


    public WebSocketsMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
      try
      {
        if (!context.WebSockets.IsWebSocketRequest)
        {

          //if (context.Request.Path.StartsWithSegments("/api"))
          //{
          //  context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
          //  context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
          //  context.Response.Headers.Add("Access-Control-Allow-Headers", "*");
          //  context.Response.Headers.Add("Access-Control-Allow-Methods", "POST,GET,PUT,PATCH,DELETE,OPTIONS");
          //}

          await _next.Invoke(context);
          return;
        }
        if (context.Request.Path.StartsWithSegments("/ws_api"))
          await ApiSocketServer.CallInvoke(context);
        else if (context.Request.Path.StartsWithSegments("/ws_dashboard"))
          await DashboardServer.CallInvoke(context);

      }
      catch (Exception e)
      {
        Logger.Instance.LogException(e);
        int a = 0;
      }

      //if (context.Request.Path.StartsWithSegments("/ws_clientconsole"))
      //  await ConsoleClientServer.CallInvoke(context);

    }
  }
}
