using System;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace CCMonkeys.Sockets
{
  public class ServerSocketResponse
  {
    public WebSocketMessageType Type { get; set; }
    //public byte[] Data { get; set; }
    public MemoryStream Stream { get; set; } = null;

    public bool HasTransimssion { get => Stream != null && Stream.Length > 0; }

    ~ServerSocketResponse()
    {
      if (Stream != null)
        Stream.Dispose();
    }

    public async Task<string> GetTextAsync()
    {
      if (this.Type != WebSocketMessageType.Text)
        return string.Empty;

      using (var reader = new StreamReader(this.Stream, Encoding.UTF8))
        return await reader.ReadToEndAsync();
    }

    public string GetText()
    {
      if (this.Type != WebSocketMessageType.Text)
        return string.Empty;

      using (var reader = new StreamReader(this.Stream, Encoding.UTF8))
        return reader.ReadToEnd();
    }

  }
}
