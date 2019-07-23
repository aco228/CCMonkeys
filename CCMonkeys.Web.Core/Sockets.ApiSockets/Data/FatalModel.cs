using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Web.Core.Sockets.ApiSockets.Data
{
  public class FatalModel : SendingObj
  {
    public string Action { get; set; } = string.Empty;
    public string Exception { get; set; } = string.Empty;
  }
}
