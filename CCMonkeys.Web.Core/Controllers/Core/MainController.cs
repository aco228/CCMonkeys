using CCMonkeys.Direct;
using CCMonkeys.Web.Core.Code;
using Direct.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCMonkeys.Web.Core.Controllers
{
  [ApiController]
  public class MainController : ControllerBase
  {

    ///
    /// CONSTRUCTORS
    ///
    
    protected readonly IHostingEnvironment HostingEnvironment;
    private CCSubmitDirect _database = null;
    private MainContext _context = null;

    public CCSubmitDirect Database
    {
      get
      {
        if (this._database != null)
          return this._database;
        this._database = new CCSubmitDirect();
        return this._database;
      }
    }
    public MainContext Context
    {
      get
      {
        if (this._context != null)
          return this._context;
        this._context = new MainContext(this, HttpContext);
        return this._context;
      }
    }

    public MainController(IHostingEnvironment hostingEnvironment)
      => HostingEnvironment = hostingEnvironment;
    ~MainController() => OnDispose();

    protected void OnDispose()
    {
      if (this._database != null)
      {
        this._database.Dispose();
      }
    }

    ///
    /// RETURNS
    ///

    protected IActionResult ReturnContent(string content)
    {
      this.OnDispose();
      return this.Content(content);
    }

    protected IActionResult ReturnStatus(bool status, string message, object data = null)
    {
      this.OnDispose();
      return Ok(new
      {
        status = status,
        message = message,
        data = data
      });
    }

    protected IActionResult ReturnObject(object data)
    {
      this.OnDispose();
      return this.Ok(data);
    }

    protected IActionResult ReturnRedirection(string url)
    {
      this.OnDispose();
      return this.Redirect(url);
    }



  }
}
