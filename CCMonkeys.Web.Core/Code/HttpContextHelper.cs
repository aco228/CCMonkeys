using CCMonkeys.Web.Core;
using CCMonkeys.Web.Core.Code;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CCMonkeys.Web
{
  public static class HttpContextHelper
  {

    public static string CookiesGet(this MainContext context, string key)
    {
      if (context.Request.Cookies.ContainsKey(key))
        return Crypter.Decrypt(context.Request.Cookies[key].ToString());
      return string.Empty;
    }

    public static int? CookiesGetInt(this MainContext context, string key)
    {
      int result;
      if (int.TryParse(context.CookiesGet(key), out result))
        return result;
      return null;
    }

    public static void SetCookie(this MainContext context, string key, string value, int? days = null)
    {
      context.Response.Cookies.Append(
        key, 
        Crypter.Encrypt(value),
         new Microsoft.AspNetCore.Http.CookieOptions
         {
           SameSite = SameSiteMode.None,
           Secure = false,
           HttpOnly = false,
           Expires = (days == null ? DateTimeOffset.Now.AddDays(1).AddYears(5) : DateTimeOffset.Now.AddDays(days.Value))
         });
    }

    public static void RemoveCookie(this MainContext context, string key)
    {
      context.Response.Cookies.Append(key, "", new CookieOptions()
      {
        Expires = DateTime.Now.AddDays(-1)
      });
    }


  }
}
