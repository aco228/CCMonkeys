using CCMonkeys.Sockets;
using CCMonkeys.Sockets.Direct;
using CCMonkeys.Web.Core.Code;
using CCMonkeys.Web.Core.Sockets.Base;
using Direct.ccmonkeys.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CCMonkeys.Web.Core.Sockets.Dashboard
{
  public class DashboardSocketsServer : CCSocketServerBase<DashboardSessionSocket>
  {

    protected override string OnCreateId(HttpContext httpContext)
    {
      string sguid = httpContext.Request.Query["sguid"];
      if (string.IsNullOrEmpty(sguid) || !Sessions.ContainsKey(sguid))
        return string.Empty;

      return sguid;
    }

    protected override async Task OnClientConnect(string uid)
      => DashboardSocket.AdminConnected(Get(uid)?.Admin);

    protected override async Task OnClientDisconect(string uid)
    {
      DashboardSocket.AdminDisconnected(Get(uid)?.Admin);
      CloseSession(uid);
    }

    protected override async Task OnReceiveMessage(string uid, ServerSocketResponse package)
    {
      DashboardSessionSocket socket = Get(uid);
      if (socket == null)
        return;

      string data = await package.GetTextAsync();
      if (string.IsNullOrEmpty(data) || !data.Contains('#'))
        return;

      string key = data.Substring(0, data.IndexOf('#'));
      string json = data.Substring(data.IndexOf('#') + 1);
      socket.OnInteraction();

      switch (key)
      {

        case "register":
          socket.OnRegister();
          return;

        /*
          DIRECT 
        */
        case "direct":
          DirectSocketManager.OnRequestLoad(this, key, uid, json);
          return;

      }
    }

    ///
    /// STATICS
    ///


    // Get all admins that are currently live on dashboard
    public static List<string> ActiveSessions
    {
      get
      {
        List<string> result = new List<string>();
        foreach (var s in Sessions)
          result.Add(s.Value.Admin.username);
        return result;
      }
    }

    public static async void Send(DashboardSessionSocket socket, DashboardSocketDistributionModel data)
      => await SendStringAsync(socket.WebSocket, JsonConvert.SerializeObject(data));
    public static async void Send(string uid, DashboardSocketDistributionModel data)
      => await SendStringAsync(Get(uid).WebSocket, JsonConvert.SerializeObject(data));
    public static async void SendToAll (DashboardSocketDistributionModel data)
      => (from s in Sessions where s.Value.IsLive select s).ToList().ForEach(s => Send(s.Value, data));

    public static AdminDM TryToGetAdminFromSessions(int id)
    {
      foreach (var s in Sessions)
        if (s.Value.Admin.ID == id)
          return s.Value.Admin;
      return null;
    }

  }
}
