using CCMonkeys.Direct;
using CCMonkeys.Sockets;
using CCMonkeys.Web.Core.Code;
using CCMonkeys.Web.Core.CommunicationChannels;
using CCMonkeys.Web.Core.Sockets.ApiSockets;
using CCMonkeys.Web.Core.Sockets.ApiSockets.Models;
using Direct;
using Direct.ccmonkeys.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CCMonkeys.Web.Core.Controllers.SocketBackup
{
  [Route("sb")]
  public class SocketBackupController : MainController
  {
    private ActionDM _action = null;

    public SocketBackupController(IHostingEnvironment hostingEnvironment) : base(hostingEnvironment) { }

    public ActionDM Action
    {
      get
      {
        if (this._action != null)
          return _action;

        string actionID = this.Context.CookiesGet(Constants.ActionID);
        this._action = this.Database.Query<ActionDM>().LoadByGuid(actionID);
        return this._action;
      }
    }

    [HttpGet("{type}/{data}")]
    [EnableCors("get")]
    public async Task<IActionResult> Index(string type, string data)
    {
      SocketBackupControllerLogger logger = new SocketBackupControllerLogger(
        this.Context.CookiesGet(Constants.ActionID),
        this.Context.CookiesGet(Constants.UserGuidCookie),
        this.Context.CookiesGetInt(Constants.CountryID),
        this.Context.HttpContext.Request.Headers["User-Agent"]);

      SessionType sessionType = (type.Equals("lp") ? Sockets.ApiSockets.Models.SessionType.Lander : Sockets.ApiSockets.Models.SessionType.Prelander);
      SessionSocket socket = new SessionSocket(this.Context, sessionType);

      ActionDM action = socket.Action.Data;
      if (action == null)
      {
        logger.StartLoggin("")
          .Add("type", type)
          .Add("data", data)
          .OnException(new Exception("Could not load action"));
        return this.ReturnObject(new DistributionModel() { Status = false });
      }

      string[] split = data.Split('|');
      if(split.Length != 2)
      {
        logger.StartLoggin("")
          .Add("type", type)
          .Add("data", data)
          .OnException(new Exception("Could not get data from object"));
        return this.ReturnObject(new DistributionModel() { Status = false });
      }

      string userID = socket.User.Key;
      int? country = socket.CountryID;

      if(action.http_flow == false)
      {
        action.http_flow = true;
        action.UpdateLater();
      }

      if(type.Equals("pl"))
      {
        PrelanderCommunicationChannel channel = new PrelanderCommunicationChannel(logger, action, userID, country, this.Database);
        return this.ReturnObject(await channel.Call(split[0], split[1]));
      }
      else if(type.Equals("lp"))
      {
        LanderCommunicationChannel channel = new LanderCommunicationChannel(logger, action, userID, country, this.Database);
        return this.ReturnObject(await channel.Call(split[0], split[1]));
      }
      else
      {
        logger.StartLoggin("")
          .Add("type", type)
          .Add("data", data)
          .OnException(new Exception("Type was not present "));
        return this.ReturnObject(new DistributionModel() { Status = false });
      }
    }

  }
}
