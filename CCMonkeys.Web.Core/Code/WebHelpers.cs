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


    public static void ForEach<TKey, TValue>(this Dictionary<TKey, TValue> dict, Action<KeyValuePair<TKey, TValue>> action)
    {
      foreach (var item in dict)
        action(item);
    }

  }
}
