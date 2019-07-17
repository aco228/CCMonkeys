using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Web.Core.Sockets.ApiSockets.Data
{
  public class FatalModel : SendingObj
  {
    public string Exception { get; set; } = string.Empty;
  }
}
