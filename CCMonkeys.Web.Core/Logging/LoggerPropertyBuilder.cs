using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CCMonkeys.Web.Core.Logging
{
  public class LoggerPropertyBuilder
  {
    private LoggingBase LoggingBase = null;
    private string Key = string.Empty;
    public Dictionary<string, string> Values { get; private set; } = new Dictionary<string, string>();

    public LoggerPropertyBuilder(LoggingBase logger, string key = "")
    {
      this.LoggingBase = logger;
      this.Key = key;
    }

    public LoggerPropertyBuilder Where(string value)
    {
      this.Values.Add("where", value);
      return this;
    }

    public LoggerPropertyBuilder Add(string key, string value)
    {
      if (!this.Values.ContainsKey(key))
        this.Values.Add(key, value);
      return this;
    }

    public LoggerPropertyBuilder Add(object model)
    {
      try
      {
        var props = model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
        foreach (var prop in props)
          this.Values.Add("model." + prop.Name, prop.GetValue(model, null).ToString());
      }
      catch(Exception e)
      {
        this.Values.Add("--modelExceptionParsing", e.ToString());
      }
      return this;
    }

    public void OnException(Exception e)
    {
      this.LoggingBase.LogException(e, this.Key, this.Values);
    }

  }
}
