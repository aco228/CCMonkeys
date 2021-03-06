﻿using CCMonkeys.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCMonkeys.Web.Core.Sockets.ApiSockets.Data
{
  public class ReceivingCreateUserModel : ReceiveModel
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
