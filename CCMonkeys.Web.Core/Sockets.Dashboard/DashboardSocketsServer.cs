using CCMonkeys.Sockets;
using CCMonkeys.Sockets.Direct;
using CCMonkeys.Web.Core.Code;
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
  public class DashboardSocketsServer : ServerSocketBase
  {
    public static Dictionary<string, DashboardSessionSocket> Sessions = new Dictionary<string, DashboardSessionSocket>();
    public static DashboardSessionSocket Get(string uid) => Sessions.ContainsKey(uid) && Sessions[uid].IsLive ? Sessions[uid] : null;

    protected override string OnCreateId(HttpContext httpContext, CancellationToken token)
    {
      string sguid = httpContext.Request.Query["sguid"];
      if (string.IsNullOrEmpty(sguid) || !Sessions.ContainsKey(sguid))
        return string.Empty;

      Get(sguid).CancellationToken = token;
      Get(sguid).Created = DateTime.Now;
      return sguid;
    }

    protected override void PassWebsocket(string uid, WebSocket webSocket)
      => Get(uid).WebSocket = webSocket;

    protected override async Task OnClientConnect(string uid)
      => DashboardSocket.AdminConnected(Get(uid)?.Admin);

    protected override async Task OnClientDisconect(string uid)
      => DashboardSocket.AdminDisconnected(Get(uid)?.Admin);

    protected override async Task OnReceiveMessage(string uid, ServerSocketResponse package)
    {
      string data = await package.GetTextAsync();
      if (string.IsNullOrEmpty(data) || !data.Contains('#'))
        return;

      string key = data.Substring(0, data.IndexOf('#'));
      string json = data.Substring(data.IndexOf('#') + 1);
      Get(uid).LastInteraction = DateTime.Now;

      switch (key)
      {

        case "register":
          Get(uid).OnRegister();
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
    public static void AddSession(DashboardSessionSocket socket)
    {
      if (Sessions.ContainsKey(socket.Key))
        Sessions[socket.Key] = socket;
      else
        Sessions.Add(socket.Key, socket);
    }
    public static void CloseSession(string uid)
    {
      Get(uid).CloseSocket();
      Sessions.Remove(uid);
    }


    public static async void Send(DashboardSessionSocket socket, DashboardSocketDistributionModel data)
      => await SendStringAsync(socket.WebSocket, JsonConvert.SerializeObject(data));
    public static async void Send(string uid, DashboardSocketDistributionModel data)
      => await SendStringAsync(Get(uid).WebSocket, JsonConvert.SerializeObject(data));
    public static async void SendToAll (DashboardSocketDistributionModel data)
      => (from s in Sessions where s.Value.IsLive select s).ToList().ForEach(s => Send(s.Value, data));

  }
}
