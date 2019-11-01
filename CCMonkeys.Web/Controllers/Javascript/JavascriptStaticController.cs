using CCMonkeys.Web.Core.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Ajax.Utilities;

namespace CCMonkeys.Web.Controllers.Javascript
{
  [Route("js/static/")]
  public class JavascriptStaticController : MainController
  {
    public JavascriptStaticController(IHostingEnvironment hostingEnvironment) : base(hostingEnvironment) { }

    [HttpGet("lander")]
    public IActionResult GetLanderJavascript()
    {
      Response.ContentType = "text/javascript";
      string clientPath = this.HostingEnvironment.WebRootPath + @"/js/static/client.js";
      string landerPath = this.HostingEnvironment.WebRootPath + @"/js/shared/lander.js";

      var baseUrl = $"{(this.Request.Scheme.Equals("https") ? "wss" : "ws")}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}";
      string js = "var CC=Object;"
        + string.Format("CC.host='{0}';", baseUrl)
        + (new Microsoft.Ajax.Utilities.Minifier().MinifyJavaScript(System.IO.File.ReadAllText(clientPath)))
        + ";"
        + (new Microsoft.Ajax.Utilities.Minifier().MinifyJavaScript(System.IO.File.ReadAllText(landerPath)));
      return this.ReturnContent(js);
    }

    [HttpGet("prelander")]
    public IActionResult GetPrelanderJavascript()
    {
      Response.ContentType = "text/javascript";
      string clientPath = this.HostingEnvironment.WebRootPath + @"/js/static/client.js";
      string prelanderPath = this.HostingEnvironment.WebRootPath + @"/js/shared/prelander.js";
      string js = "var CC=Object;" 
        + (new Microsoft.Ajax.Utilities.Minifier().MinifyJavaScript(System.IO.File.ReadAllText(clientPath)))
        + ";"
        + (new Microsoft.Ajax.Utilities.Minifier().MinifyJavaScript(System.IO.File.ReadAllText(prelanderPath)));
      return this.ReturnContent(js);
    }
  }
}
