using CCMonkeys.Direct;
using CCMonkeys.Web.Core.Controllers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Web.Core
{
  public class MainContext
  {
    public MainController MainController { get; set; }

    public HttpContext HttpContext { get; private set; } = null;
    public HttpRequest Request { get => HttpContext.Request; }
    public HttpResponse Response { get => HttpContext.Response; }
    public CCSubmitDirect Database { get => this.MainController.Database; }


    public MainContext(MainController controller, HttpContext context)
    {
      MainController = controller;
      this.HttpContext = context;
    }

  }
}
