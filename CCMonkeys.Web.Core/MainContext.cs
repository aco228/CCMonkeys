using CCMonkeys.Direct;
using CCMonkeys.Web.Core.Code;
using CCMonkeys.Web.Core.Controllers;
using Direct.ccmonkeys.Models;
using Microsoft.AspNetCore.Http;
using Direct;

namespace CCMonkeys.Web.Core
{
  public class MainContext
  {
    public MainController MainController { get; set; }

    public HttpContext HttpContext { get; private set; } = null;
    public HttpRequest Request { get => HttpContext.Request; }
    public HttpResponse Response { get => HttpContext.Response; }
    public CCSubmitDirect Database { get => this.MainController.Database; }
    public AdminDM Admin { get; protected set; } = null;


    public MainContext(MainController controller, HttpContext context)
    {
      MainController = controller;
      this.HttpContext = context;
    }


    public string StoreAdminCookie(int id, string username)
    {
      string token = $"{id}|{username}";
      this.SetCookie(Constants.AdminCookie, token, 99);
      return token;
    }

    public int? TryGetAdminID()
    {
      string cookieValue = this.CookiesGet(Constants.AdminCookie);
      if (string.IsNullOrEmpty(cookieValue))
        cookieValue = (this.HttpContext.Request.Headers.ContainsKey("authentication") ? this.HttpContext.Request.Headers["authentication"].ToString() : string.Empty);
      if (string.IsNullOrEmpty(cookieValue))
        return null;
      string[] split = cookieValue.Split('|');
      if (split.Length != 2)
        return null;

      int? id = split[0].ToInt();
      if (!id.HasValue)
        return null;

      this.Admin = this.Database.CreateModel<AdminDM>(id.Value);
      this.Admin.username = split[1];
      return id;
    }

  }
}
