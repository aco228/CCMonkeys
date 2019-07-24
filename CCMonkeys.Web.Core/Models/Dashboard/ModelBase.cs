using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Web.Core.Models.Dashboard
{
  public class ModelBaseResponse
  {
    public bool Status { get; set; } = true;
    public string Message { get; set; } = string.Empty;


    public static ModelBaseResponse GenerateError(string message)
      => new ModelBaseResponse()
      {
        Status = false,
        Message = message
      };

  }

  public class ModelBaseRequest
  {

  }

}
