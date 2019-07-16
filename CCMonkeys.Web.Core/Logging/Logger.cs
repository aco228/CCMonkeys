using CCMonkeys.Web.Core.Logging;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using SharpRaven;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Web.Core
{
  public class Logger
  {
    private static LoggingBase _instance = null;
    public static LoggingBase Instance
    {
      get
      {
        if (_instance != null)
          return _instance;
        _instance = new LoggingBase();
        return _instance;
      }
    }
  }
}
