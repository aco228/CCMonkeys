using CCMonkeys.Web.Code;
using CCMonkeys.Web.Core.Code.Filters;
using CCMonkeys.Web.Core.Sockets.ApiSockets;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Web.Core.Controllers.Core
{
  public class SocketsController : ControllerBase
  {
    public IActionResult Index()
    {
      string response = "";
      response += ApiSocketCloserHostedService.LastInteraction.ToString() + Environment.NewLine;

      foreach (var session in ApiSocketServer.Sessions)
        response += $"sid={session.Key}, act={session.Value.Action.Key}, created={session.Value.Created}, lastInteraction={session.Value.LastCommunicationMiliseconds}, state={session.Value.WebSocket.State.ToString()}" + Environment.NewLine;

      return this.Content(response);
    }

    public IActionResult Kill(string id)
    {
      ApiSocketServer.CloseSession(id);
      return this.Index();
    }

    public IActionResult KillAll()
    {
      foreach (var session in ApiSocketServer.Sessions)
        ApiSocketServer.CloseSession(session.Value);
      return this.Index();
    }

  }
}
