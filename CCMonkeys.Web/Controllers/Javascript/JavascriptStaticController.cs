using CCMonkeys.Web.Core.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
      string js = "var CC=Object;" + (new JSMinify.Minify(landerPath)).getModifiedData() + (new JSMinify.Minify(clientPath)).getModifiedData();
      return this.ReturnContent(js);
    }

    [HttpGet("prelander")]
    public IActionResult GetPrelanderJavascript()
    {
      Response.ContentType = "text/javascript";
      string clientPath = this.HostingEnvironment.WebRootPath + @"/js/static/client.js";
      string prelanderPath = this.HostingEnvironment.WebRootPath + @"/js/shared/prelander.js";
      string js = "var CC=Object;" + (new JSMinify.Minify(clientPath)).getModifiedData() + (new JSMinify.Minify(prelanderPath)).getModifiedData();
      return this.ReturnContent(js);
    }
  }
}
