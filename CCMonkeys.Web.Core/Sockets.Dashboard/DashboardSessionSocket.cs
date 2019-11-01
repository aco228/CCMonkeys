using CCMonkeys.Sockets;
using CCMonkeys.Web.Core.Code.CacheManagers;
using CCMonkeys.Web.Core.Sockets.ApiSockets;
using CCMonkeys.Web.Core.Sockets.Dashboard.Data;
using Direct.ccmonkeys.Models;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;

namespace CCMonkeys.Web.Core.Sockets.Dashboard
{
  public class DashboardSessionSocket
  {
    private MainContext context = null;
    public WebSocket WebSocket { get; set; } = null;
    public string Key { get; protected set; } = string.Empty;
    public AdminDM Admin { get; protected set; } = null;
    public AdminSessionDM Session { get; protected set; } = null;
    public DateTime Created { get; set; } = DateTime.Now;

    public DashboardSessionSocket(MainContext context)
    {
      this.context = context;
      this.Admin = this.context.Admin;
      this.Key = Guid.NewGuid().ToString();
    }

    /// 
    /// Base implementation of the sockets
    /// 

    public async void OnRegister()
    {
      this.Session = await new AdminSessionDM(this.context.Database)
      {
        adminid = this.Admin.ID.Value,
        guid = this.Key
      }.InsertAsync<AdminSessionDM>();

      // init
      DashboardSocketsServer.Send(this, new InitDashboardModel()
      {
        Actions = ApiSocketServer.ActiveActions,
        DashboardSessions = DashboardSocketsServer.ActiveSessions,
        Countries = CountryCache.Instance.GetModel(),
        Landers = LandersCache.Instance.GetLandersModel(),
        LanderTypes = LandersCache.Instance.GetLanderTypesModel(),
        Prelanders = PrelandersCache.Instance.GetPrelandersModel(),
        PrelanderTypes = PrelandersCache.Instance.GetPrelanderTypesModel(),
        Providers = ProvidersCache.Instance.GetAll()
      }
      .Pack(DashboardEvents.INIT));

    }

    public void OnClose()
    {
      this.Session.duration = (DateTime.Now - this.Created).TotalSeconds;
      this.Session.UpdateLater();
    }

    public void Send(DashboardSocketDistributionModel data)
      => DashboardSocketsServer.Send(this, data);

    
  }
}
