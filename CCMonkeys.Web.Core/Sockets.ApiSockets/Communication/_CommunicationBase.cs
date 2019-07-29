using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Web.Core.Sockets.ApiSockets.Communication
{
  public abstract class CommunicationBase
  {
    public SessionSocket Socket { get; } = null;

    public CommunicationBase(SessionSocket socket)
    {
      this.Socket = socket;
    }

  }
}
