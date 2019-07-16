using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Web.Core.Models
{
  public class CreateUserModel
  {
    public int aid { get; set; } = -1;
    public int cid { get; set; } = -1;
    public int? lid { get; set; } = null;

    public string ga_cid { get; set; } = "";
    public string email { get; set; } = "";
    public string password { get; set; } = "";
    public string country { get; set; } = "";
    public string lxid { get; set; } = "";
    public string pp { get; set; } = "";
    public int? affid { get; set; }
    public string pubid { get; set; } = "";
    public string landerName { get; set; } = "";

    public string prelander { get; set; } = "";
    public string type { get; set; } = "";
    public string referrer { get; set; } = "";
    public string host { get; set; } = "";
  }
}
