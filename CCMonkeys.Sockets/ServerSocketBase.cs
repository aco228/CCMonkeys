using CCMonkeys.Loggings;
using CCMonkeys.Sockets.Direct;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CCMonkeys.Sockets
{
  public abstract class ServerSocketBase
  {
    protected ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

    public WebSocket this[string id] => _sockets.ContainsKey(id) ? _sockets[id] : null;

    public async Task CallInvoke(HttpContext context)
    {
      CancellationToken ct = context.RequestAborted;
      var socketId = this.OnCreateId(context);
      if(string.IsNullOrEmpty(socketId))
      {
        Logger.Instance.StartLoggin("ServerSocketBase").Where("ServerSocketBase.CallInvoke")
          .OnException(new Exception("socketid is empty!"));

        context.Response.StatusCode = 400;
        return;
      }

      context.Response.Cookies.Append("test", "aa");
      WebSocket currentSocket = await context.WebSockets.AcceptWebSocketAsync();
      

      // in case that token is not set propperly
      if (string.IsNullOrEmpty(socketId))
      {
        await currentSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", ct);
        return;
      }

      _sockets.TryAdd(socketId, currentSocket);
      this.PassWebsocket(socketId, currentSocket);
      await this.OnClientConnect(socketId);

      while (true)
      {
        if (ct.IsCancellationRequested)
          break;

        ServerSocketResponse package = await ReceiveResponseAsync(socketId, currentSocket);

        if (package == null || !package.HasTransimssion)
        {
          if (currentSocket.State != WebSocketState.Open)
            break;
          if (this.CloseIfConnectionProblem(socketId))
            break;
        }
        else
          await this.OnReceiveMessage(socketId, package);

      }

      WebSocket dummy;
      _sockets.TryRemove(socketId, out dummy);

      await this.OnClientDisconect(socketId);
      try
      {
        await currentSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", ct);
      }
      catch(Exception e) { }
      currentSocket.Dispose();
    }

    public string CreateDummySocket()
    {
      var socketId = Guid.NewGuid().ToString();
      _sockets.TryAdd(socketId, null);
      return socketId;
    }

    /*
    +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
      IMPLEMENTATIONS
    */

    private async Task<ServerSocketResponse> ReceiveResponseAsync(string uid, WebSocket socket, CancellationToken ct = default(CancellationToken))
    {
      try
      {
        ServerSocketResponse response = new ServerSocketResponse();
        response.Stream = new MemoryStream();
        var buffer = new ArraySegment<byte>(new byte[8192]);

        WebSocketReceiveResult result;
        do
        {
          ct.ThrowIfCancellationRequested();

          result = await socket.ReceiveAsync(buffer, ct);
          if (result == null)
            continue;

          response.Stream.Write(buffer.Array, buffer.Offset, result.Count);
        }
        while (!result.EndOfMessage);
        response.Stream.Seek(0, SeekOrigin.Begin);

        response.Type = result.MessageType;
        return response;
      }
      catch (Exception e)
      {
        //this.OnException("ReceiveResponseAsync", uid, e);
        return null;
      }
    }

    // Original
    internal void SendAsync(string uid, object data)
    {
      try
      {
        SendStringAsync(_sockets[uid], JsonConvert.SerializeObject(data));
      }
      catch (Exception e)
      {
        this.OnException("SendAsync", uid, e);
        throw e;
      }
    }
    protected static Task SendStringAsync(WebSocket socket, string data, CancellationToken ct = default(CancellationToken))
    {
      try
      {
        if (socket == null || socket.State != WebSocketState.Open)
          return Task.FromResult(0);

        var buffer = Encoding.UTF8.GetBytes(data);
        var segment = new ArraySegment<byte>(buffer);
        return socket.SendAsync(segment, WebSocketMessageType.Text, true, ct);
      }
      catch (Exception e)
      {
        Logger.Instance.StartLoggin("")
          .Where("ServerSocketBase.SendStringAsync")
          .Add("data", data)
          .OnException(e);
        return Task.FromResult(1);
      }
    }
    protected async Task<string> ReceiveStringAsync(string uid, WebSocket socket, CancellationToken ct = default(CancellationToken))
    {
      try
      {
        var buffer = new ArraySegment<byte>(new byte[8192]);
        using (var ms = new MemoryStream())
        {
          WebSocketReceiveResult result;
          do
          {
            ct.ThrowIfCancellationRequested();

            result = await socket.ReceiveAsync(buffer, ct);
            ms.Write(buffer.Array, buffer.Offset, result.Count);
          }
          while (!result.EndOfMessage);

          ms.Seek(0, SeekOrigin.Begin);
          if (result.MessageType != WebSocketMessageType.Text)
            return null;

          // Encoding UTF8: https://tools.ietf.org/html/rfc6455#section-5.6
          using (var reader = new StreamReader(ms, Encoding.UTF8))
            return await reader.ReadToEndAsync();
        }
      }
      catch (Exception e)
      {
        this.OnException("ReceiveStringAsync", uid, e);
        return string.Empty;
      }
    }



    /*
    +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
      APSTRACTIOn
    */

    protected virtual void PassWebsocket(string uid, WebSocket webSocket) { }
    protected virtual string OnCreateId(HttpContext context) { return Guid.NewGuid().ToString(); }
    protected virtual Task OnReceiveMessage(string uid, ServerSocketResponse package) => Task.FromResult(default(object));
    protected virtual Task OnClientConnect(string uid) => Task.FromResult(default(object));
    protected virtual Task OnClientDisconect(string uid) => Task.FromResult(default(object));
    protected virtual bool CloseIfConnectionProblem(string uid) => false;
    protected virtual void OnException(string location, string uid, Exception e)
    {
      Logger.Instance.StartLoggin(uid)
        .OnException(e);
    }

  }
}
