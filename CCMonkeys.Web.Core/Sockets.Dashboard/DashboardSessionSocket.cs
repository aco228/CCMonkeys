using CCMonkeys.Sockets;
using CCMonkeys.Sockets.Direct;
using CCMonkeys.Web.Core.Code.CacheManagers;
using CCMonkeys.Web.Core.Sockets.ApiSockets;
using CCMonkeys.Web.Core.Sockets.Base;
using CCMonkeys.Web.Core.Sockets.Dashboard.Data;
using Direct.ccmonkeys.Models;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

namespace CCMonkeys.Web.Core.Sockets.Dashboard
{
  public class DashboardSessionSocket : CCSocketBase
  {
    private string _key = string.Empty;
    public AdminDM Admin { get; protected set; } = null;
    public AdminSessionDM Session { get; protected set; } = null;

    public override string Key => this._key;

    public DashboardSessionSocket(MainContext context)
      :base(context)
    {
      this.Admin = this.Context.Admin;
      this._key = Guid.NewGuid().ToString();
    }

    /// 
    /// Base implementation of the sockets
    /// 

    public async void OnRegister()
    { 
      this.Session = await new AdminSessionDM(this.Context.Database)
      {
        adminid = this.Admin.ID.Value,
        guid = this.Key
      }.InsertAsync<AdminSessionDM>();

      // init
      DashboardSocketsServer.Send(this, new InitDashboardModel()
      {
        Privileges = this.Admin.privileges,
        AdminStatus = this.Admin.GetStatus().ToString(),
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


    public async void OnClose()
    {
      this.Session.duration = (DateTime.Now - this.Created).TotalSeconds;
      await this.Session.UpdateAsync();
    }

    public void Send(DashboardSocketDistributionModel data)
      => DashboardSocketsServer.Send(this, data);

  }
}
