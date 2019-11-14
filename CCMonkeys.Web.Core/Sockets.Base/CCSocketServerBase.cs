using CCMonkeys.Sockets;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

namespace CCMonkeys.Web.Core.Sockets.Base
{
  public abstract class CCSocketServerBase<T> : ServerSocketBase
    where T : CCSocketBase
  {
    public static Dictionary<string, T> Sessions = new Dictionary<string, T>();
    public static T Get(string uid) => Sessions.ContainsKey(uid) ? Sessions[uid] : null;

    protected override void PassWebsocket(string uid, WebSocket webSocket)
      => Get(uid).WebSocket = webSocket;

    public static void AddSession(T socket)
    {
      if (Sessions.ContainsKey(socket.Key))
        Sessions[socket.Key] = socket;
      else
        Sessions.Add(socket.Key, socket);
    }

    public static void CloseSession(T session)
      => CloseSession(session.Key);

    public static async void CloseSession(string uid)
    {
      var session = Get(uid);
      if (session == null)
        return;

      try
      {
        await session.WebSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
      }
      catch (Exception e)
      { }
      finally
      {
        Sessions.Remove(uid);
      }
    }

    public static async void Send(T socket, DistributionModel data)
     => await SendStringAsync(socket.WebSocket, JsonConvert.SerializeObject(data));
    public static async void Send(string uid, DistributionModel data)
      => await SendStringAsync(Get(uid).WebSocket, JsonConvert.SerializeObject(data));

  }
}
