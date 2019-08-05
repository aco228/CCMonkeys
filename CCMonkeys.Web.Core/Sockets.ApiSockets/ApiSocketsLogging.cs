using CCMonkeys.Loggings;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Web.Core.Sockets.ApiSockets
{
  public class ApiSocketsLogging : LoggingBase
  {
    private SessionSocket Socket { get; set; } = null;


    public ApiSocketsLogging(SessionSocket socket)
    {
      this.Socket = socket;
    }

    public override LoggerPropertyBuilder StartLoggin(string key)
    {
      if (this.Socket == null)
        return base.StartLoggin(key);

      LoggerPropertyBuilder result = new LoggerPropertyBuilder(this, this.Socket.Key);

      result.Add("sessionType", this.Socket.SessionType.ToString());

      if(this.Socket.Action != null)
        result.Add("actionid", this.Socket.Action.Key);

      if(this.Socket.Session != null && this.Socket.Session.Data != null)
        result.Add("sessionid", this.Socket.Session.Data.GetStringID());

      if(this.Socket.Lead != null)
        result.Add("leadid", this.Socket.Lead.ID.ToString());

      if(this.Socket.User != null)
        result.Add("userid", this.Socket.User.Key);

      if(this.Socket.Session != null && this.Socket.Session.Request != null)
      result.Add("useragent", this.Socket.Session.Request.useragent);

      return result;
    }


  }
}
