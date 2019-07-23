using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Web.Core.Models.Dashboard
{
  public class ModelBaseResponse
  {
    public bool Status { get; set; } = true;
    public string Message { get; set; } = string.Empty;
  }

  public class ModelBaseRequest
  {

  }

}
