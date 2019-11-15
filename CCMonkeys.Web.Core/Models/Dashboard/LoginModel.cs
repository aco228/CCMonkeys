using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Web.Core.Models.Dashboard
{
  public class LoginModelResponse : ModelBaseResponse
  {
    public int Privilegies { get; set; } = 0;
    public string AccessToken { get; set; } = string.Empty;
  }
}
