using System;
using System.Collections.Generic;
using System.Text;
using CCMonkeys.Loggings;
using Direct.ccmonkeys.Models;

namespace CCMonkeys.Web.Core.Controllers.SocketBackup
{
  public class SocketBackupControllerLogger : Loggings.LoggingBase
  {
    private string _actionID = string.Empty;
    private string _userID = string.Empty;
    private int? _countryID = null;
    private string _userAgent = string.Empty;

    public SocketBackupControllerLogger(string action, string userID, int? countryID, string userAgent)
    {
      this._actionID = action;
      this._userID = userID;
      this._countryID = countryID;
      this._userAgent = userAgent;
    }

    public override LoggerPropertyBuilder StartLoggin(string key)
    {
      return base.StartLoggin(key)
        .Add("http", "nazalost:(")
        .Add("actionid", this._actionID)
        .Add("userid", this._userID)
        .Add("countryID", this._countryID.ToString())
        .Add("useragent", this._userAgent);
    }

  }
}
