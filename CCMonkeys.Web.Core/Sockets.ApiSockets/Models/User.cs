using CCMonkeys.Direct;
using CCMonkeys.Web.Core;
using CCMonkeys.Web.Core.Code;
using Direct.ccmonkeys.Models;
using Direct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCMonkeys.Web.Core.Logging;

namespace CCMonkeys.Web.Core.Sockets.ApiSockets.Models
{
  public class User
  {
    public UserDM Data { get; protected set; } = null;
    public CCSubmitDirect Database { get => this.Socket.Database; }
    public SessionSocket Socket { get; }

    public string Key { get; set; } = string.Empty;

    public User(SessionSocket socket)
    {
      this.Socket = socket;
      this.Key = Socket.MainContext.CookiesGet(Constants.UserGuidCookie);
      this.Init();
    }

    public async void Init()
    {
      MSLogger mslogger = new MSLogger();
      if (!string.IsNullOrEmpty(this.Key))
      {
        mslogger.Track("before load");
        this.Data = await this.Database.Query<UserDM>().LoadByGuidAsync(this.Key);
        mslogger.Track("after load");
        if (this.Data != null)
        {

          if (this.Data.leadid.HasValue && Socket.Lead == null)
            this.Socket.Lead = await this.Database.Query<LeadDM>().LoadAsync(this.Data.leadid.Value);

          mslogger.Track("after lead load");
          return;
        }
      }

      this.Data = new UserDM(this.Database)
      {
        countryid = Socket.CountryID,
        countryCode = Socket.Session.CountryCode,
        leadid = (Socket.Lead != null ? Socket.Lead.ID : null)
      };

      mslogger.Track("after create");
      this.Data.InsertLater();
      this.Key = this.Data.GetStringID();

      mslogger.Track("after insert");

      Socket.MainContext.SetCookie(Constants.UserGuidCookie, this.Key);

      mslogger.Track("after set cookie");
      int a = 0;
    }

    public void UpdateLead(LeadDM lead)
    {
      this.Data.leadid = lead.ID.Value;
      this.Data.UpdateLater();
    }

    public void SetCountry(int? cid, string countryCode)
    {
      this.Data.countryCode = countryCode;
      this.Data.countryid = cid;
      this.Data.UpdateLater();
    }

  }
}
