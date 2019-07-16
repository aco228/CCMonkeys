using CCMonkeys.Web.Code.ApiSockets.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCMonkeys.Web.Code.ApiSockets
{
  public static class DistributionModelHelper
  {

    public static DistributionModel Pack(this SendingObj input, bool status = true, string message = "")
      => new DistributionModel()
      { 
        Status = status,
        Message = message,

        Data = input
      };

  }
}
