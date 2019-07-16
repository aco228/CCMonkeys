using CCMonkeys.Web.Core.Sockets.ApiSockets.Data;

namespace CCMonkeys.Web.Core.Sockets.ApiSockets
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
