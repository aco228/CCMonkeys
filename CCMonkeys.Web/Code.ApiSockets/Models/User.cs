using CCMonkeys.Direct;
using CCMonkeys.Web.Core;
using CCMonkeys.Web.Core.Code;
using Direct.ccmonkeys.Models;
using Direct.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCMonkeys.Web.Code.ApiSockets
{
  public class User
  {
    public int? ID { get => this.Data.ID; }
    public UserDM Data { get; protected set; } = null;
    public CCSubmitDirect Database { get => this.Socket.Database; }
    public SessionSocket Socket { get; }
    public string Key { get; set; } = string.Empty;

    public User(SessionSocket socket, MainContext context)
    {
      this.Socket = socket;
      this.Key = context.CookiesGet(Constants.UserGuidCookie);
      this.Init(context);
    }

    public async void Init(MainContext context)
    {

      if (string.IsNullOrEmpty(this.Key))
      {
        this.Data = await new UserDM(this.Database)
        {
          guid = Guid.NewGuid().ToString()
        }
        .InsertAsync<UserDM>();
        this.Key = this.Data.guid;
      }
      else
      {
        this.Data =
          (await this.Database.Query<UserDM>().Where("guid={0}", this.Key).LoadSingleAsync());
      }

      context.SetCookie(Constants.UserGuidCookie, this.Key);
    }

    public void UpdateLead(LeadDM lead)
    {
      this.Data.leadid = lead.ID.Value;
      this.Data.UpdateLater();
    }

  }
}
