using CCMonkeys.Direct;
using CCMonkeys.Web.Core.Code;
using CCMonkeys.Web.Core.Controllers;
using Direct.ccmonkeys.Models;
using Microsoft.AspNetCore.Http;
using Direct;
using CCMonkeys.Web.Core.Sockets.Dashboard;

namespace CCMonkeys.Web.Core
{
  public class MainContext
  {
    private AdminDM _admin = null;

    public MainController MainController { get; set; }

    public HttpContext HttpContext { get; private set; } = null;
    public HttpRequest Request { get => HttpContext.Request; }
    public HttpResponse Response { get => HttpContext.Response; }
    public CCSubmitDirect Database { get => this.MainController.Database; }

    /// <summary>
    /// Try to get admin from cookies, request, and if exists try to find him in sockets or load from database
    /// </summary>
    public AdminDM Admin
    {
      get
      {
        if (this._admin != null)
          return this._admin;

        int? adminID = this.TryGetAdminID();
        if (!adminID.HasValue)
          return null;

        this._admin = DashboardSocketsServer.TryToGetAdminFromSessions(adminID.Value);

        if(this._admin == null)
          this._admin = this.Database.Query<AdminDM>().Where("[id]={0}", adminID.Value).LoadSingle();

        if (this._admin.GetStatus() == AdminStatusDM.NotActive)
        {
          this._admin = null;
          return null;
        }

        return this._admin;
      }
    }


    public MainContext(MainController controller, HttpContext context)
    {
      MainController = controller;
      this.HttpContext = context;
    }

    public MainContext(HttpContext context)
    {
      this.HttpContext = context;
    }


    public string StoreAdminCookie(int id, string username)
    {
      string token = $"{id}|{username}";
      this.SetCookie(Constants.AdminCookie, token, 99);
      return token;
    }

    /// <summary>
    /// Try to get AdminID from cookies
    /// </summary>
    /// <returns></returns>
    protected int? TryGetAdminID()
    {
      if (this._admin != null)
        return this._admin.ID;

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

      return id;
    }

  }
}
