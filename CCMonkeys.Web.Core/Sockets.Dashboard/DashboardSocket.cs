using CCMonkeys.Web.Core.Models.Dashboard;
using CCMonkeys.Web.Core.Sockets.ApiSockets.Data;
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
    INIT,

    ACTION_CONNECT,
    ACTION_DISCONNECT,

    ADMIN_CONNECTED,
    ADMIN_DISCONNECTED,

    ACTION_INSERT,
    ACTION_UPDATE,

    POSTBACK_TRANSACTION,
    POSTBACK_REFUND,
    POSTABACK_CHARGEBACK,
    POSTBACK_UPSELL,

    FATAL

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
    /// ACTION CONNECT / DISCONNECT
    ///

    public static void ActionConnected(ActionLiveModel model)
      => DashboardSocketsServer.SendToAll(model.Pack(DashboardEvents.ACTION_CONNECT));
    public static void ActionDisconnected(string actionid)
      => DashboardSocketsServer.SendToAll(new ActionConnectedDisconnectedModel() { ID = actionid }.Pack(DashboardEvents.ACTION_DISCONNECT));

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
      => DashboardSocketsServer.SendToAll(new ActionUpdateModel() { Data = action }.Pack(DashboardEvents.ACTION_INSERT));
    public static void OnActionUpdate(ActionDM action)
      => DashboardSocketsServer.SendToAll(new ActionUpdateModel() { Data = action }.Pack(DashboardEvents.ACTION_UPDATE));
    public static void OnActionOffline(ActionDM action)
      => DashboardSocketsServer.SendToAll(new ActionUpdateModel() { Data = action }.Pack(DashboardEvents.ACTION_UPDATE));

    ///
    /// POSTBACKS
    ///

    public static void OnNewTransaction(string provider, string actionID)
      => DashboardSocketsServer.SendToAll(new PostbackTransaction() { ProviderName = provider, ActionID = actionID }.Pack(DashboardEvents.POSTBACK_TRANSACTION));
    public static void OnNewChargeback(string provider, string actionID)
      => DashboardSocketsServer.SendToAll(new PostbackTransaction() { ProviderName = provider, ActionID = actionID }.Pack(DashboardEvents.POSTABACK_CHARGEBACK));
    public static void OnNewRefund(string provider, string actionID)
      => DashboardSocketsServer.SendToAll(new PostbackTransaction() { ProviderName = provider, ActionID = actionID }.Pack(DashboardEvents.POSTBACK_REFUND));
    public static void OnNewUpsell(string provider, string actionID)
      => DashboardSocketsServer.SendToAll(new PostbackTransaction() { ProviderName = provider, ActionID = actionID }.Pack(DashboardEvents.POSTBACK_UPSELL));

    ///
    /// ERRORS
    ///

    public static void OnFatal(string sessionID, string exception)
      => DashboardSocketsServer.SendToAll(new ExceptionModel() { SessionID = sessionID, Exception= exception}.Pack(DashboardEvents.FATAL));
  }
}
