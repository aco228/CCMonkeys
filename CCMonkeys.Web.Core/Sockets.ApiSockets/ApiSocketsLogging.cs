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
      LoggerPropertyBuilder result = new LoggerPropertyBuilder(this, (!string.IsNullOrEmpty(this.Socket.Key) ? this.Socket.Key : "no key"));
      result.Add("sessionType", (this.Socket != null) ? this.Socket.SessionType.ToString() : "socket.sessiontype is null");
      result.Add("actionid", (this.Socket.Action != null)? this.Socket.Action.Key : "action is null");
      result.Add("sessionid", (this.Socket.Session.Data != null ? this.Socket.Session.Data.GetStringID() : string.Empty));
      result.Add("leadid", this.Socket.Lead != null ? this.Socket.Lead.ID.ToString() : "null");
      result.Add("userid", (this.Socket.User != null)? this.Socket.User.Key : "user is null");
      result.Add("useragent", (this.Socket.Session.Request != null ? this.Socket.Session.Request.useragent : string.Empty));
      return result;
    }


  }
}
