using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CCMonkeys.Web
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


    // SUMMARY: Extension for Controller to get raw data from the request
    public static async Task<string> GetRawBodyStringAsync(this HttpRequest request, Encoding encoding = null)
    {
      if (encoding == null)
        encoding = Encoding.UTF8;

      using (StreamReader reader = new StreamReader(request.Body, encoding))
        return await reader.ReadToEndAsync();
    }

  }
}
