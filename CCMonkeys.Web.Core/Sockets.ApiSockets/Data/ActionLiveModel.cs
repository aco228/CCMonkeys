using CCMonkeys.Sockets;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Web.Core.Sockets.ApiSockets.Data
{
  public class ActionLiveModel : SendingObj
  {

    // ActionID
    public string ID { get; set; }

    // SessionID
    public string sid { get; set; }

    // SessionRequestID
    public string srid { get; set; }

    // last interaction
    public DateTime li { get; set; }

    // created
    public DateTime crt { get; set; }

    public static ActionLiveModel Convert(SessionSocket socket)
      => new ActionLiveModel()
      {
        ID = socket.Action.Key,
        sid = socket.Session.Key,
        srid = socket.Session.Request.GetStringID(),
        li = socket.LastInteraction,
        crt = socket.Created
      };

  }
}
