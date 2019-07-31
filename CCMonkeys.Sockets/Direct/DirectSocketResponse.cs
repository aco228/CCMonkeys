using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Sockets.Direct
{
  public class DirectSocketResponse
  {
    public bool IsDirect { get; set; } = true;
    public bool Success { get; set; } = true;
    public string Message { get; set; } = string.Empty;
    public string Ticket { get; set; } = string.Empty;
    public dynamic Data { get; set; }

  }
}
