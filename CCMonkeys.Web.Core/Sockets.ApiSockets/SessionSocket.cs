﻿using CCMonkeys.Direct;
using CCMonkeys.Web.Core.Code;
using Direct.ccmonkeys.Models;
using Direct;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.WebSockets;
using CCMonkeys.Web.Core.Sockets.ApiSockets.Data;
using CCMonkeys.Web.Core.Sockets.ApiSockets.Models;
using System.Collections.Generic;
using CCMonkeys.Sockets;
using CCMonkeys.Web.Core.Sockets.Dashboard;
using CCMonkeys.Web.Core.Code.CacheManagers;
using CCMonkeys.Web.Core.Sockets.ApiSockets.Communication;
using CCMonkeys.Loggings;
using System.Threading;
using CCMonkeys.Web.Core.Sockets.Base;

namespace CCMonkeys.Web.Core.Sockets.ApiSockets
{
  public enum SessionSocketChannel
  {
    Default,
    Lander,
    Prelander
  }

  public class SessionSocket : CCSocketBase
  {
    public MainContext MainContext = null;
    public CCSubmitDirect Database { get; protected set; } = null;
    public ApiSocketsLogging Logging { get; protected set; } = null;
    public Dictionary<SessionSocketChannel, CommunicationBase> Channels = new Dictionary<SessionSocketChannel, CommunicationBase>();

    public LeadDM Lead { get; set; } = null; // * we will try to load it on Init methods of User and Action
    public User User { get; protected set; } = null;
    public Session Session { get; protected set; } = null;
    public SessionType SessionType { get; set; } = SessionType.Default;
    public Models.Action Action { get; protected set; } = null;
    public override string Key { get => (this.Session != null ? this.Session.Key : ""); }

    public int? CountryID { get => this.Session.CountryID; }


    public SessionSocket(MainContext context, SessionType sessionType)
      : base(context)
    {
      try
      {
        this.Database = new CCSubmitDirect();
        this.Logging = new ApiSocketsLogging(this);
        MSLogger mslogger = new MSLogger();
        this.MainContext = context;
        this.SessionType = sessionType;

        this.Session = new Session(this); // first we prepare all parts, and get country
        this.User = new User(this);
        this.Action = new Models.Action(this);

        Channels.Add(SessionSocketChannel.Default, new SharedCommunication(this));
        if (sessionType == SessionType.Lander)
          Channels.Add(SessionSocketChannel.Lander, new LanderCommunication(this));
        else if (sessionType == SessionType.Prelander)
          Channels.Add(SessionSocketChannel.Prelander, new PrelanderCommunication(this));

        this.Database.TransactionalManager.RunAsync();
      }
      catch (Exception e)
      {
        Logger.Instance.StartLoggin("")
          .Where("SessionSocket.Constructor")
          .Add("type", sessionType.ToString())
          .OnException(e);

        ApiSocketServer.CloseSession(this);
      }
    }

    public async Task<LeadDM> TryToIdentifyLead(string msisdn, string email)
    {
      this.Lead = await LeadDM.LoadByMsisdnOrEmailAsync(this.Database, msisdn, email);
      if (this.Lead == null)
        this.Lead = await new LeadDM(this.Database)
        {
          countryid = Session.CountryID,
          msisdn = msisdn,
          email = email
        }.InsertAsync<LeadDM>();
      this.User.UpdateLead(this.Lead);
      return this.Lead;
    }

    /// 
    /// Base implementation of the sockets
    /// 

    public void OnCreate()
    {
      this.Session.OnCreate();
    }
    public void OnClose()
    {
      if(this.Action.Data != null)
        DashboardSocket.OnActionOffline(this.Action.Data);
      this.Session.OnClose(this.Created);
    }

    public void Send(string key)
      => ApiSocketServer.Send(this, new DistributionModel() { Key = key, Status = true });
    public void Send(DistributionModel data)
      => ApiSocketServer.Send(this, data);

    public void Send(string key, DistributionModel data)
    {
      data.Key = key;
      ApiSocketServer.Send(this, data);
    }

  }
}
