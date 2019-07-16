using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCMonkeys.Web.Code.ApiSockets.Data
{
  public class ReceivingCreateUserModel
  {
    public string email { get; set; } = string.Empty;
    public string password { get; set; } = string.Empty; // do we need it?
  }

  public class SendingCreateUserModel : SendingObj
  {
    public bool emptyEmail { get; set; } = false;
    public bool refused { get; set; } = false;
  }

}
