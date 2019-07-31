using CCMonkeys.Sockets;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Web.Core.Sockets.Dashboard.Data
{
  public class ExceptionModel : SendingObj
  {
    public string SessionID { get; set; }
    public string Exception { get; set; }
  }
}
