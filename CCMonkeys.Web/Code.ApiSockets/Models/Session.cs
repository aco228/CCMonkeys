using CCMonkeys.Direct;
using CCMonkeys.Web.Code.ApiSockets.Data;
using CCMonkeys.Web.Core;
using CCMonkeys.Web.Core.Code;
using CCMonkeys.Web.Core.Code.IP2ID;
using CCMonkeys.Web.Core.Code.IPLocations.IpApi;
using Direct.ccmonkeys.Models;
using Direct.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCMonkeys.Web.Code.ApiSockets
{
  public enum SessionType { Default = 1, Prelander = 2, Lander = 3 };

  public class Session
  {
    public int? ID { get => this.Data.ID; }
    public SessionType Type { get => Socket.SessionType; }
    public SessionRequestDM Request { get; set; } = null;
    public SessionDM Data { get; protected set; } = null;
    public SessionDataDM SessionData { get; protected set; } = null;
    public CCSubmitDirect Database { get => this.Socket.Database; }
    public string CountryCode { get; protected set; }
    public SessionSocket Socket { get; }

    public string Key { get => Data.guid; }

    public Session(SessionSocket socket, MainContext context)
    {
      this.Socket = socket;
      this.Init(context); 
    }

    private async void Init(MainContext context)
    {
      string ip = context.HttpContext.Connection.RemoteIpAddress.ToString();
      if (ip.Equals("::1")) ip = "62.4.55.231";

      this.Request = await new SessionRequestDM(this.Database)
      {
        rawurl = string.Empty,
        ip = ip,
        useragent = context.HttpContext.Request.Headers["User-Agent"]
      }
      .InsertAsync<SessionRequestDM>();

      int? sessionDataID = context.CookiesGetInt(Constants.SessionDataID);
      if (sessionDataID.HasValue)
        this.SessionData = await this.Database.Query<SessionDataDM>().LoadAsync(1);
      else
      {
        this.SessionData = IPAPI.GetSessionData(context.Database, ip, this.Request.useragent);
        await this.SessionData.InsertAsync<SessionDataDM>();
        context.SetCookie(Constants.SessionDataID, this.SessionData.ID.ToString());
      }

      if (this.SessionData != null)
        this.CountryCode = this.SessionData.countryCode;

      this.Data = await new SessionDM(this.Database)
      {
        userid = Socket.User.Data.ID.Value,
        actionid = Socket.Action.Data.ID.Value,
        sessionrequestid = this.Request.ID.Value,
        sessiondataid = this.SessionData.ID.Value,
        sessiontype = (int)this.Socket.SessionType,
        guid = Guid.NewGuid().ToString()
      }
      .InsertAsync<SessionDM>();
    }

    public async Task PrelanderRegistrationLogic(string domain, Dictionary<string, string> queryValues, ReceivingRegistrationModel model)
    {
      string msisdn = string.Empty, email = string.Empty;
      if (queryValues.ContainsKey("msisdn"))
        msisdn = queryValues["msisdn"];
      if (queryValues.ContainsKey("email"))
        email = queryValues["email"];

      if (!string.IsNullOrEmpty(msisdn) || !string.IsNullOrEmpty(email))
        await this.Socket.TryToIdentifyLead(msisdn, email);
    }


    public async void OnClose(DateTime created)
    {
      this.Data.duration = (DateTime.Now - created).TotalSeconds;
      await this.Data.UpdateAsync();
    }

  }
}
