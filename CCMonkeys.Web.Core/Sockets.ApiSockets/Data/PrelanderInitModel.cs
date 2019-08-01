using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Web.Core.Sockets.ApiSockets.Data
{

  public class PrelanderInitModelTag
  {
    public bool isQuestion { get; set; }
    public string name { get; set; }
    public string value { get; set; }
    public string[] answers { get; set; }
  }

  public class PrelanderInitModel
  {
    public int prelanderid { get; set; }
    public List<PrelanderInitModelTag> tags { get; set; }
  }
}
