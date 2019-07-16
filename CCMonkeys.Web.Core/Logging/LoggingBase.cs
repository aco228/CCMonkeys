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


    public void LogException(Exception ex, string sessionID = "", IDictionary<string, string> properties = null)
    {
      TelemetryClient.TrackException(ex);
      int a = 1;
      // ai.Flush();  
    }

    public void Event(LogEventType logEventType, IDictionary<string, string> properties = null)
    {
      if (!string.IsNullOrEmpty(this.SessionID))
        TelemetryClient.Context.User.AccountId = this.SessionID;
      TelemetryClient.TrackEvent(logEventType.ToString(), properties);
      //ai.Flush();
    }

  }
}
