using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Sockets
{
  public static class DistributionModelHelper
  {

    public static DistributionModel Pack(this SendingObj input, string key, bool status = true, string message = "")
      => new DistributionModel()
      {
        Key = key,
        Status = status,
        Message = message,

        Data = input
      };

    public static DistributionModel Pack(this SendingObj input, bool status = true, string message = "")
      => new DistributionModel()
      {
        Status = status,
        Message = message,

        Data = input
      };

  }
}
