using CCMonkeys.Sockets;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Web.Core.Sockets.Dashboard
{

  public static class DashboardSocketDistributionModelHelper
  {

    public static DashboardSocketDistributionModel Pack(this SendingObj obj, DashboardEvents eventType)
      => new DashboardSocketDistributionModel()
      {
        Event = eventType,
        Data = obj
      };

  }

  public class DashboardSocketDistributionModel
  {

    public DashboardEvents Event { get; set; } = DashboardEvents.DEFAULT;
    public object Data { get; set; } = null;

  }
}
