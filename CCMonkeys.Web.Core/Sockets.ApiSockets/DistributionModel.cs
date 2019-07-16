using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCMonkeys.Web.Core.Sockets.ApiSockets
{
  public class DistributionModel
  {
    public bool Status { get; set; } = true;
    public string Message { get; set; } = string.Empty;

    public string Key { get; set; } = string.Empty;
    public object Data { get; set; } = null;
  }
}
