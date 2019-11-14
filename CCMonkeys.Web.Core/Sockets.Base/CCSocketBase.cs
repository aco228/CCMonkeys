using CCMonkeys.Sockets;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

namespace CCMonkeys.Web.Core.Sockets.Base
{
  public abstract class CCSocketBase
  {
    public MainContext Context { get; protected set; } = null;
    public WebSocket WebSocket { get; set; } = null;
    public abstract string Key { get; }
    public bool IsLive { get => this.WebSocket.State == WebSocketState.Open; }
    public DateTime Created { get; private set; } = DateTime.Now;
    public DateTime LastInteraction { get; protected set; } = DateTime.Now;
    public double LastCommunicationMiliseconds { get => (DateTime.Now - LastInteraction).TotalMilliseconds; }

    public CCSocketBase(MainContext context)
    {
      this.Created = DateTime.Now;
      this.Context = context;
    }

    public void OnInteraction()
    {
      this.LastInteraction = DateTime.Now;
    }

    // Overrides

    protected virtual void OnCloseSocket() { }

  }

}
