using CCMonkeys.Direct;
using CCMonkeys.Sockets;
using CCMonkeys.Web.Code.ApiSockets;
using CCMonkeys.Web.Code.ApiSockets.Data;
using CCMonkeys.Web.Core;
using CCMonkeys.Web.Core.Code;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace CCMonkeys.Web.Code.Sockets
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

      DistributionModel response = null;
      switch (key)
      {
        case "register":
          response = await Get(uid).OnRegistration(JsonConvert.DeserializeObject<ReceivingRegistrationModel>(json));
          break;

        // lander
        case "user-create":
          response = await Get(uid).OnCreateUser(JsonConvert.DeserializeObject<ReceivingCreateUserModel>(json));
          break;
        case "user-subscribe":
          response = await Get(uid).OnSubscribeUser(JsonConvert.DeserializeObject<ReceivingSubscribeUser>(json));
          break;
        case "user-redirected":
          response = await Get(uid).OnUserRedirected(JsonConvert.DeserializeObject<ReceivingUserRedirected>(json));
          break;
      }

      response.Key = key;
      Get(uid).Send(response);
      Get(uid).Database.Dispose();
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
