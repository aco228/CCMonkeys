using CCMonkeys.Web.Core.Models.Dashboard;
using CCMonkeys.Web.Core.Sockets.Dashboard.Data;
using Direct.ccmonkeys.Models;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Web.Core.Sockets.Dashboard
{
  public enum DashboardEvents
  {
    DEFAULT,
    ADMIN_CONNECTED,
    ADMIN_DISCONNECTED,

    ACTION_INSERT,
    ACTION_UPDATE
  }

  public static class DashboardSocket
  {
    public static string PrintEvents()
    {
      string[] names = Enum.GetNames(typeof(DashboardEvents));
      DashboardEvents[] values = (DashboardEvents[])Enum.GetValues(typeof(DashboardEvents));
      string result = "";
      for (int i = 0; i < names.Length; i++)
        result += (!string.IsNullOrEmpty(result) ? "," : "") + string.Format("{0}:{1}", names[i], (int)values[i]);
      return result;
    }

    ///
    /// ADMIN CONNECTIONS
    ///

    public static void AdminConnected(AdminDM admin)
      => DashboardSocketsServer.SendToAll(new AdminConnectedModel() { Username = admin.username }
      .Pack(DashboardEvents.ADMIN_CONNECTED));

    public static void AdminDisconnected(AdminDM admin)
      => DashboardSocketsServer.SendToAll(new AdminDisconnectedModel() { Username = admin.username }
      .Pack(DashboardEvents.ADMIN_DISCONNECTED));

    ///
    /// ACTIONS 
    ///

    public static void OnActionInsert(ActionDM action)
      => DashboardSocketsServer.SendToAll(action.Pack(isOnline: true).Pack(DashboardEvents.ACTION_INSERT));
    public static void OnActionUpdate(ActionDM action)
      => DashboardSocketsServer.SendToAll(action.Pack(isOnline: true).Pack(DashboardEvents.ACTION_UPDATE));
    public static void OnActionOffline(ActionDM action)
      => DashboardSocketsServer.SendToAll(action.Pack(isOnline: false).Pack(DashboardEvents.ACTION_UPDATE));


  }
}
