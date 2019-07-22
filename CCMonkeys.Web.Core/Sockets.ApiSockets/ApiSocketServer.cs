using CCMonkeys.Direct;
using CCMonkeys.Sockets;
using CCMonkeys.Web.Core;
using CCMonkeys.Web.Core.Code;
using CCMonkeys.Web.Core.Sockets.ApiSockets.Data;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace CCMonkeys.Web.Core.Sockets.ApiSockets
{
  public class ApiSocketServer : ServerSocketBase
  {
    public static Dictionary<string, SessionSocket> Sessions = new Dictionary<string, SessionSocket>();
    public static SessionSocket Get(string uid) => Sessions.ContainsKey(uid) ? Sessions[uid] : null;

    ///
    /// Overrides
    ///

    protected override string OnCreateId(HttpContext httpContext)
    {
      string sguid = httpContext.Request.Query["sguid"];
      if (string.IsNullOrEmpty(sguid) || !Sessions.ContainsKey(sguid))
        return string.Empty;

      Get(sguid).Created = DateTime.Now;
      return sguid;
    }

    protected override void PassWebsocket(string uid, WebSocket webSocket)
      => Get(uid).WebSocket = webSocket;

    protected override Task OnClientConnect(string uid)
    {
      return base.OnClientConnect(uid);
    }

    protected override Task OnClientDisconect(string uid)
    {
      Get(uid)?.OnClose();
      Sessions.Remove(uid);
      return Task.FromResult(1);
    }

    protected override async Task OnReceiveMessage(string uid, ServerSocketResponse package)
    {
      string data = await package.GetTextAsync();
      if (string.IsNullOrEmpty(data) || !data.Contains('#'))
        return;

      string key = data.Substring(0, data.IndexOf('#'));
      string json = data.Substring(data.IndexOf('#') + 1);

      try
      {
        switch (key)
        {
          case "register":
            Get(uid).OnRegistration(key, JsonConvert.DeserializeObject<ReceivingRegistrationModel>(json));
            break;

          // lander
          case "user-create":
            Get(uid).OnCreateUser(key, JsonConvert.DeserializeObject<ReceivingCreateUserModel>(json));
            break;
          case "user-subscribe":
            Get(uid).OnSubscribeUser(key, JsonConvert.DeserializeObject<ReceivingSubscribeUser>(json));
            break;
          case "user-redirected":
            Get(uid).OnUserRedirected(key, JsonConvert.DeserializeObject<ReceivingUserRedirected>(json));
            break;
        }
      }
      catch(Exception e)
      {
        Get(uid).Send(new FatalModel() { Exception = e.ToString() }.Pack(false, "error500"));
        return;
      }
      finally
      {
      }
    }

    ///
    /// STATICS
    ///

    public static void AddSession(SessionSocket socket)
    {
      if (Sessions.ContainsKey(socket.Key))
        Sessions[socket.Key] = socket;
      else
        Sessions.Add(socket.Key, socket);
    }

    public static async void Send(SessionSocket socket, DistributionModel data)
      => await SendStringAsync(socket.WebSocket, JsonConvert.SerializeObject(data));
    public static async void Send(string uid, DistributionModel data)
      => await SendStringAsync(ApiSocketServer.Get(uid).WebSocket, JsonConvert.SerializeObject(data));

  }
}
