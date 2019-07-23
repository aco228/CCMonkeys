using CCMonkeys.Direct;
using CCMonkeys.Web.Core;
using CCMonkeys.Web.Core.Code;
using Direct.ccmonkeys.Models;
using Direct.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCMonkeys.Web.Core.Sockets.ApiSockets.Models
{
  public class User
  {
    public UserDM Data { get; protected set; } = null;
    public CCSubmitDirect Database { get => this.Socket.Database; }
    public SessionSocket Socket { get; }

    public int? ID { get; set; } = null;
    public string Key { get; set; } = string.Empty;

    public User(SessionSocket socket)
    {
      this.Socket = socket;
      this.ID = Socket.MainContext.CookiesGetInt(Constants.UserIdCookie);
      this.Key = Socket.MainContext.CookiesGet(Constants.UserGuidCookie);
      this.Init();
    }

    public async void Init()
    {
      if (this.ID.HasValue)
      {
        this.Data = await this.Database.Query<UserDM>().LoadAsync(this.ID.Value);
        if (this.Data != null)
        {

          if (this.Data.leadid.HasValue && Socket.Lead == null)
            this.Socket.Lead = await this.Database.Query<LeadDM>().LoadAsync(this.Data.leadid.Value);

          return;
        }
      }

      this.Key = Guid.NewGuid().ToString();
      this.Data = await new UserDM(this.Database)
      {
        countryid = Socket.CountryID,
        countryCode = Socket.Session.CountryCode,
        leadid = (Socket.Lead != null ? Socket.Lead.ID : null),
        guid = this.Key
      }.InsertAsync<UserDM>();

      this.ID = this.Data.ID;

      Socket.MainContext.SetCookie(Constants.UserIdCookie, this.ID.ToString());
      Socket.MainContext.SetCookie(Constants.UserGuidCookie, this.Key);
    }

    public void UpdateLead(LeadDM lead)
    {
      this.Data.leadid = lead.ID.Value;
      this.Data.UpdateLater();
    }

    public void UpdateSessionData(int? id)
    {
      this.Data.sessiondataid = id;
      this.Data.UpdateLater();
    }

    public void UpdateAction(int? id)
    {
      this.Data.actionid = id;
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
