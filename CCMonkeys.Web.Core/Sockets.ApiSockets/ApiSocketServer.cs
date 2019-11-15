using CCMonkeys.Direct;
using CCMonkeys.Loggings;
using CCMonkeys.Sockets;
using CCMonkeys.Web.Core;
using CCMonkeys.Web.Core.Code;
using CCMonkeys.Web.Core.CommunicationChannels;
using CCMonkeys.Web.Core.Sockets.ApiSockets.Communication;
using CCMonkeys.Web.Core.Sockets.ApiSockets.Data;
using CCMonkeys.Web.Core.Sockets.ApiSockets.Models;
using CCMonkeys.Web.Core.Sockets.Base;
using CCMonkeys.Web.Core.Sockets.Dashboard;
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

namespace CCMonkeys.Web.Core.Sockets.ApiSockets
{
  public class ApiSocketServer : CCSocketServerBase<SessionSocket>
  {
    ///
    /// Overrides
    ///

    protected override string OnCreateId(HttpContext httpContext)
    {
      string sguid = httpContext.Request.Query.ContainsKey("sguid") ? httpContext.Request.Query["sguid"].ToString() : string.Empty;
      if (string.IsNullOrEmpty(sguid) || !Sessions.ContainsKey(sguid))
      {
        string type = httpContext.Request.Query.ContainsKey("type") ? httpContext.Request.Query["type"].ToString() : string.Empty;
        if (!string.IsNullOrEmpty(type))
        {
          SessionType sessionType = type.Equals("lp") ? Models.SessionType.Lander : Models.SessionType.Prelander;
          SessionSocket newSocket = new SessionSocket(new MainContext(null, httpContext), sessionType);
          if (newSocket == null || string.IsNullOrEmpty(newSocket.Key))
          {
            Logger.Instance.StartLoggin("")
              .Where("OnCreateId")
              .OnException(new Exception("SessionSocket returned string.emptry as Key"));
            return string.Empty;
          }

          Sessions.Add(newSocket.Key, newSocket);
          sguid = newSocket.Key;
          return newSocket.Key;
        }
      }

      if(string.IsNullOrEmpty(sguid))
      {
        Logger.Instance.StartLoggin("OnCreateId").Where("ApiSocketServer.OnCreatedID")
          .OnException(new Exception("sguid is empty!"));
        return string.Empty;
      }

      if (!Sessions.ContainsKey(sguid))
      {
        Get(sguid).OnCreate();
        return sguid;
      }

      return string.Empty;
    }
    
    protected override Task OnClientConnect(string uid)
    {
      DashboardSocket.ActionConnected(ActionLiveModel.Convert(Get(uid)));
      return base.OnClientConnect(uid);
    }

    protected override Task OnClientDisconect(string uid)
    {
      var session = Get(uid);
      if (session == null)
        return Task.FromResult(1);

      DashboardSocket.ActionDisconnected(Get(uid).Action.Key);
      Get(uid)?.OnClose();
      CloseSession(uid);
      return Task.FromResult(1);
    }

    protected override async Task OnReceiveMessage(string uid, ServerSocketResponse package)
    {
      string key, json;
      SessionSocket socket = Get(uid);
      if (socket == null)
        return;

      try
      {

        string data = await package.GetTextAsync();
        if (string.IsNullOrEmpty(data) || !data.Contains('#'))
          return;

        key = data.Substring(0, data.IndexOf('#'));
        json = data.Substring(data.IndexOf('#') + 1);
        socket.OnInteraction();
      

        SessionType sessionType = Get(uid).SessionType;

        switch (key)
        {

          //
          // shared channels
          //

          case "register":

            if (sessionType == SessionType.Lander)
              ((LanderCommunication)Get(uid).Channels[SessionSocketChannel.Lander])
                .OnRegistration(key, JsonConvert.DeserializeObject<ReceivingRegistrationModel>(json));
            else if (sessionType == SessionType.Prelander)
              ((PrelanderCommunication)Get(uid).Channels[SessionSocketChannel.Prelander])
                .OnRegistration(key, JsonConvert.DeserializeObject<ReceivingRegistrationModel>(json));

            break;

          //
          // lander channels
          //

          case "user-create":
          case "user-subscribe":
          case "user-redirected":
            LanderCommunicationChannel channel = new LanderCommunicationChannel(Get(uid));
            await channel.Start(key, json);
            break;

          //
          // prelander channels
          //

          case "pl-init":
          case "pl-tag":
          case "pl-q":
            PrelanderCommunicationChannel prelanderCommunication = new PrelanderCommunicationChannel(Get(uid));
            await prelanderCommunication.Call(key, json);
            break;


        }
      }
      catch(Exception e)
      {
        CloseSession(socket);
        OnException("ApiSocket.OnReceiveMessage", uid, e);
        return;
      }

    }

    protected override void OnException(string location, string uid, Exception e)
    {
      Get(uid).Logging.StartLoggin("")
        .Add("location", location)
        .OnException(e);
    }
    ///
    /// STATICS
    ///

    // Get all currentrly active actions
    public static List<ActionLiveModel> ActiveActions
    {
      get
      {
        List<ActionLiveModel> result = new List<ActionLiveModel>();
        foreach (var s in Sessions)
          result.Add(ActionLiveModel.Convert(s.Value));
        return result;
      }
    }
    
    public static bool IsActionOnline(ActionDM action)
      => (from s in Sessions where s.Value.Action.Data != null && s.Value.Action.Data.ID.HasValue && s.Value.Action.Data.ID == action.ID select s.Value).FirstOrDefault() != null;

  }
}
