using CCMonkeys.Web.Core.Sockets.Dashboard;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Web.Core.Logging
{

  public class LoggingBase
  {
    protected TelemetryClient TelemetryClient;
    public LoggerPropertyBuilder PropertyBuilder { get; } = null;
    protected string SessionID = string.Empty;

    public LoggingBase()
    {
      TelemetryClient = new TelemetryClient();
      if (string.IsNullOrEmpty(TelemetryClient.InstrumentationKey))
      {
        // attempt to load instrumentation key from app settings
        var appSettingsTiKey = "43479a34-a52b-4580-a5c6-3b60e8325b59";
        if (!string.IsNullOrEmpty(appSettingsTiKey))
        {
          TelemetryConfiguration.Active.InstrumentationKey = appSettingsTiKey;
          TelemetryClient.InstrumentationKey = appSettingsTiKey;
        }
        else
        {
          throw new Exception("Could not find instrumentation key for Application Insights");
        }
      }
    }

    public LoggerPropertyBuilder StartLoggin(string key)
    {
      LoggerPropertyBuilder result = new LoggerPropertyBuilder(this, key);
      return result;
    }

    public void LogException(Exception ex, string sessionID = "", IDictionary<string, string> properties = null)
    {
      if (!string.IsNullOrEmpty(sessionID))
        TelemetryClient.Context.User.AccountId = sessionID;
      TelemetryClient.TrackException(ex, properties);
      int a = 1;

      DashboardSocket.OnFatal(sessionID, ex.ToString());
      // ai.Flush();  
    }

    public void Event(string logEventType, IDictionary<string, string> properties = null)
    {
      if (!string.IsNullOrEmpty(this.SessionID))
        TelemetryClient.Context.User.AccountId = this.SessionID;
      TelemetryClient.TrackEvent(logEventType, properties);
      //ai.Flush();
    }

  }
}
