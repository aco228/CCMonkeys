using Direct.Models;
using System;

namespace Direct.ccmonkeys.Models
{
  public enum AdminStatusDM
  {
      NotActive = 0,
      
      Default1 = 1,
      Default2 = 2,
      Default3 = 3,

      Marketing = 4,
      Admin = 5
  }

  public partial class AdminDM : DirectModel
  {

    public AdminStatusDM GetStatus()
      => (AdminStatusDM)this.privileges;

  }
}
