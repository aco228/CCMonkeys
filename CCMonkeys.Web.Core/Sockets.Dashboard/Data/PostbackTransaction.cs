using CCMonkeys.Sockets;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Web.Core.Sockets.Dashboard.Data
{
  public class PostbackTransaction : SendingObj
  {
    public string ProviderName { get; set; }
    public string ActionID { get; set; }
  }
}
