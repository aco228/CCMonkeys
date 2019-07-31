using CCMonkeys.Web.Core.Logging;
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

    public LoggerPropertyBuilder StartLoggin()
    {
      LoggerPropertyBuilder result = new LoggerPropertyBuilder(this, this.Socket.Key);
      result.Add("sessionType", this.Socket.SessionType.ToString());
      result.Add("actionid", this.Socket.Action.Key);
      result.Add("sessionid", this.Socket.Session.Key);
      result.Add("leadid", this.Socket.Lead != null ? this.Socket.Lead.ID.ToString() : "null");
      result.Add("userid", this.Socket.User.Key);
      return result;
    }


  }
}
