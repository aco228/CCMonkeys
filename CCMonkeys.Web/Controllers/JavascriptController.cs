using CCMonkeys.Web.Code.Sockets;
using CCMonkeys.Web.Core.Code;
using CCMonkeys.Web.Core.Controllers;
using CCMonkeys.Web.Core.Sockets.ApiSockets;
using CCMonkeys.Web.Core.Sockets.ApiSockets.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCMonkeys.Web.Controllers
{
  [Route("js")]
  public class JavascriptController : MainController
  {
    private static string ClientJS = string.Empty;
    private static string PrelanderJS = string.Empty;
    private static string LanderJS = string.Empty;

    public JavascriptController(IHostingEnvironment hostingEnvironment) : base(hostingEnvironment) { }

    public IActionResult Index(string type, string dbg, string recompile = "0")
    {
      if (string.IsNullOrEmpty(type))
        return this.Content("console.error('ccsocket:: type missing');");

      SessionType sessionType = SessionType.Default;
      if (type.ToLower().Equals("pl")) sessionType = SessionType.Prelander;
      else if (type.ToLower().Equals("lp")) sessionType = SessionType.Lander;
      else return this.Content("console.error('ccsocket:: uknown type');");

      #region #compile.js#

      string js_extension = string.Empty;
      if (string.IsNullOrEmpty(ClientJS) || recompile.Equals("1"))
      {
        string path = this.HostingEnvironment.WebRootPath + @"/js/client.js";
        ClientJS = (new JSMinify.Minify(path)).getModifiedData();
      }

      if (sessionType == SessionType.Prelander)
      {
        if (string.IsNullOrEmpty(PrelanderJS) || recompile.Equals("1"))
        {
          string path = this.HostingEnvironment.WebRootPath + @"/js/prelander.js";
          PrelanderJS = (new JSMinify.Minify(path)).getModifiedData();
        }
        js_extension = PrelanderJS;
      }
      else if (sessionType == SessionType.Lander)
      {
        if (string.IsNullOrEmpty(LanderJS) || recompile.Equals("1"))
        {
          string path = this.HostingEnvironment.WebRootPath + @"/js/lander.js";
          LanderJS = (new JSMinify.Minify(path)).getModifiedData();
        }
        js_extension = LanderJS;
      }

      #endregion

      var socket = new SessionSocket(this.Context, sessionType);

      var baseUrl = $"{(this.Request.Scheme.Equals("https") ? "wss" : "ws")}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}";
      string variables = "var CC=Object;";
      variables += string.Format("CC.dbg={0};", string.IsNullOrEmpty(dbg) || dbg.Equals("0") ? "false" : "true");
      variables += string.Format("CC.host='{0}';", baseUrl);
      variables += string.Format("CC.type='{0}';", type.ToLower());
      variables += string.Format("CC.sguid='{0}';", socket.Key);
      variables += string.Format("CC.uid='{0}';", socket.User.Key);

        // reset cookies
      if(sessionType == SessionType.Lander)
        this.Context.RemoveCookie(Constants.ActionIDCookie);

      ApiSocketServer.AddSession(socket);
      return this.ReturnContent(variables+ClientJS+js_extension);
    }

  }
}
