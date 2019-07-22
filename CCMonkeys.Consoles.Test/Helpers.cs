using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Consoles.Test
{
  public static class Helpers
  {

    internal static string EscapeString(this string input)
    {
      if (input[input.Length - 1] == '\\')
        input = input.Substring(0, input.Length - 1);

      return System.Security.SecurityElement.Escape(input.ToString()
        .Replace("'", string.Empty));
    }
  }
}
