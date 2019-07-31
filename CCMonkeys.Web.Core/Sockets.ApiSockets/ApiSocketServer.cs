using CCMonkeys.Direct;
using CCMonkeys.Sockets;
using CCMonkeys.Web.Core;
using CCMonkeys.Web.Core.Code;
using CCMonkeys.Web.Core.Sockets.ApiSockets.Communication;
using CCMonkeys.Web.Core.Sockets.ApiSockets.Data;
using CCMonkeys.Web.Core.Sockets.ApiSockets.Models;
using CCMonkeys.Web.Core.Sockets.Dashboard;
using Direct.ccmonkeys.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
      {
        string type = httpContext.Request.Query["type"];
        if (!string.IsNullOrEmpty(type))
        {
          SessionSocket newSocket = new SessionSocket(new MainContext(null, httpContext), type.Equals("lp") ? Models.SessionType.Lander : Models.SessionType.Prelander);
          Sessions.Add(newSocket.Key, newSocket);
          sguid = newSocket.Key;
        }
      }

      Get(sguid).OnCreate();
      return sguid;
    }

    protected override void PassWebsocket(string uid, WebSocket webSocket)
      => Get(uid).WebSocket = webSocket;

    protected override Task OnClientConnect(string uid)
    {
      DashboardSocket.ActionConnected(Get(uid).Action.Key);
      return base.OnClientConnect(uid);
    }

    protected override Task OnClientDisconect(string uid)
    {
      DashboardSocket.ActionDisconnected(Get(uid).Action.Key);
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
      SessionType sessionType = Get(uid).SessionType;

      switch (key)
      {

        //
        // shared channels
        //

        case "register":

          if(sessionType == SessionType.Lander)
            ((LanderCommunication)Get(uid).Channels[SessionSocketChannel.Lander])
              .OnRegistration(key, JsonConvert.DeserializeObject<ReceivingRegistrationModel>(json));
          else if(sessionType == SessionType.Prelander)
            ((PrelanderCommunication)Get(uid).Channels[SessionSocketChannel.Prelander])
              .OnRegistration(key, JsonConvert.DeserializeObject<ReceivingRegistrationModel>(json));

          break;

        //
        // lander channels
        //

        case "user-create":

          if (sessionType == SessionType.Lander)
            ((LanderCommunication)Get(uid).Channels[SessionSocketChannel.Lander])
              .OnCreateUser(key, JsonConvert.DeserializeObject<ReceivingCreateUserModel>(json));

          break;
        case "user-subscribe":

          if (sessionType == SessionType.Lander)
            ((LanderCommunication)Get(uid).Channels[SessionSocketChannel.Lander])
              .OnSubscribeUser(key, JsonConvert.DeserializeObject<ReceivingSubscribeUser>(json));
          break;
        case "user-redirected":

          if (sessionType == SessionType.Lander)
            ((LanderCommunication)Get(uid).Channels[SessionSocketChannel.Lander])
              .OnUserRedirected(key, JsonConvert.DeserializeObject<ReceivingUserRedirected>(json));
          break;

        //
        // prelander channels
        //

        case "pl-init":

          if (sessionType == SessionType.Prelander)
            ((PrelanderCommunication)Get(uid).Channels[SessionSocketChannel.Prelander])
              .OnInit(key, JsonConvert.DeserializeObject<PrelanderInitModel>(json));

          break;

        case "pl-tag":

          if (sessionType == SessionType.Prelander)
            ((PrelanderCommunication)Get(uid).Channels[SessionSocketChannel.Prelander])
              .OnTag(key, JsonConvert.DeserializeObject<PrelanderTagModel>(json));

          break;

        case "pl-q":

          if (sessionType == SessionType.Prelander)
            ((PrelanderCommunication)Get(uid).Channels[SessionSocketChannel.Prelander])
              .OnQuestion(key, JsonConvert.DeserializeObject<PrelanderTagModel>(json));

          break;

      }

      try
      {
        
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

    public static List<string> ActiveActions
    {
      get
      {
        List<string> result = new List<string>();
        foreach (var s in Sessions)
          result.Add(s.Value.Action.Key);
        return result;
      }
    }

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

    public static bool IsActionOnline(ActionDM action)
      => (from s in Sessions where s.Value.Action.Data != null && s.Value.Action.Data.ID.HasValue && s.Value.Action.Data.ID == action.ID select s.Value).FirstOrDefault() != null;

  }
}
