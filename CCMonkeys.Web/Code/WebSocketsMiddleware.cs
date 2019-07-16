using CCMonkeys.Web.Core;
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


    public WebSocketsMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
      if (!context.WebSockets.IsWebSocketRequest)
      {
        await _next.Invoke(context);
        return;
      }

      try
      {
        if (context.Request.Path.StartsWithSegments("/ws_api"))
          await ApiSocketServer.CallInvoke(context);
      }
      catch(Exception e)
      {
        Logger.Instance.LogException(e);
        int a = 0;
      }

      //if (context.Request.Path.StartsWithSegments("/ws_clientconsole"))
      //  await ConsoleClientServer.CallInvoke(context);

    }
  }
}
