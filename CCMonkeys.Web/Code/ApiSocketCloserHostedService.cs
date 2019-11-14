using CCMonkeys.Web.Core.Sockets.ApiSockets;
using CCMonkeys.Web.Core.Sockets.Dashboard;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CCMonkeys.Web.Code
{
  public class ApiSocketCloserHostedService : IHostedService, IDisposable
  {
    public static DateTime LastInteraction = DateTime.Now;

    private Timer _timer;

    public ApiSocketCloserHostedService()
    {

    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
      _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
      return Task.CompletedTask;
    }

    private void DoWork(object state)
    {
      LastInteraction = DateTime.Now;
      if (ApiSocketServer.Sessions != null && ApiSocketServer.Sessions.Count == 0)
        return;

      foreach(var session in ApiSocketServer.Sessions)
      {
        // if last interaction is more than 3 minutes
        if ((DateTime.Now - session.Value.LastInteraction).TotalMinutes >= 3 ||

          // if session is created more than 15 minutes
          (DateTime.Now - session.Value.Created).TotalMinutes >= 15 ||

          //  if session is created more than minute ago and still do not have open connection
          ((DateTime.Now - session.Value.Created).TotalMinutes > 1  && session.Value.WebSocket.State != System.Net.WebSockets.WebSocketState.Open))

          ApiSocketServer.CloseSession(session.Value);
      }

      foreach(var session in DashboardSocketsServer.Sessions)
      {
        //  if session is created more than minute ago and still do not have open connection
        if (((DateTime.Now - session.Value.Created).TotalMinutes > 1 && session.Value.WebSocket.State != System.Net.WebSockets.WebSocketState.Open))

          DashboardSocketsServer.CloseSession(session.Value);
      }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      _timer?.Change(Timeout.Infinite, 0);
      return Task.CompletedTask;
    }

    public void Dispose()
    {
      _timer?.Dispose();
    }
  }
}
