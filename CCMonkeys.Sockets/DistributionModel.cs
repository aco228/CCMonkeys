using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Sockets
{
  public class DistributionModel
  {
    public bool Status { get; set; } = true;
    public string Message { get; set; } = string.Empty;
    public string Exception { get; set; } = string.Empty;

    public string Key { get; set; } = string.Empty;
    public object Data { get; set; } = null;
  }
}
