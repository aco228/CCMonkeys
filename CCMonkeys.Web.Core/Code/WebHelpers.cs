using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Web.Core.Code
{
  public static class WebHelpers
  {

    public static int? ToInt(this string input)
    {
      int num;
      if (int.TryParse(input, out num))
        return num;
      return null;
    }

  }
}
