using CCMonkeys.Direct;
using CCMonkeys.Web.Core;
using CCMonkeys.Web.Core.Code;
using CCMonkeys.Web.Core.Sockets.ApiSockets.Data;
using Direct.ccmonkeys.Models;
using Direct.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCMonkeys.Web.Core.Sockets.ApiSockets.Models
{
  public class Action
  {
    public string Key { get => this.Data.guid; }
    public ActionDM Data { get; set; }
    public CCSubmitDirect Database { get => this.Socket.Database; }
    public SessionSocket Socket { get; }

    public Action(SessionSocket socket, MainContext context, bool createNew = false)
    {
      this.Socket = socket;
      this.Init(context, createNew);
    }

    private async void Init(MainContext context, bool createNew)
    {
      string actionGuid = context.CookiesGet(Constants.ActionIDCookie);
      if (!string.IsNullOrEmpty(actionGuid) && !createNew)
      {
        this.Data = await this.Database.Query<ActionDM>().Where("guid={0}", actionGuid).LoadSingleAsync();
        this.Data.input_redirect = (Socket.SessionType == SessionType.Lander);
        this.Data.UpdateLater();
        return;
      }

      actionGuid = Guid.NewGuid().ToString();
      this.Data = await new ActionDM(this.Database)
      {
        leadid = (Socket.Lead != null ? Socket.Lead.ID : null),
        userid = Socket.User.ID.Value,
        guid = actionGuid,
        input_redirect = (Socket.SessionType == SessionType.Lander)
      }
      .InsertAsync<ActionDM>();

      context.SetCookie(Constants.ActionIDCookie, actionGuid);
    }

    #region # On registration of socket #

    public async Task<bool> OnPrelanderRegistration(string domain, Dictionary<string, string> queryValues, ReceivingRegistrationModel model)
    {
      PrelanderDM prelander = (await this.Database.Query<PrelanderDM>().Select("prelandertypeid").Where("url={0}", domain).LoadAsync()).FirstOrDefault();
      if (prelander == null)
      {
        // TODO: Some logging here
        return false;
      }
      else
      {
        this.Data.prelanderid = prelander.ID.Value;
        this.Data.prelandertypeid = prelander.prelandertypeid;
      }

      this.Data.UpdateLater();
      return true;
    }

    public async Task<bool> OnLanderRegistration(string domain, Dictionary<string, string> queryValues, ReceivingRegistrationModel model)
    {
      LanderDM lander = (await this.Database.Query<LanderDM>().Select("landertypeid").Where("url={0}", domain).LoadAsync()).FirstOrDefault();
      if (lander == null)
      {
        // TODO: Some logging here

        return false;
      }
      else
      {
        this.Data.landerid = lander.ID.Value;
        this.Data.landertypeid = lander.landertypeid;
      }

      //?offer_id=2454&affiliate_id=183&country=montenegro&lxid=EA4mYMyZYgHimr5UncQAR9U6dJz7KXbtPMYbtBQfsk0&utm_source=banana&utm_medium=183&utm_campaign=&utm_content=&utm_term=&ptype=cc2
      if (queryValues.ContainsKey("lxid"))
        this.Data.trackingid = queryValues["lxid"];
      if (queryValues.ContainsKey("affiliate_id"))
        this.Data.affid = queryValues["affiliate_id"];
      if (queryValues.ContainsKey("utm_campaign"))
        this.Data.pubid = queryValues["utm_campaign"];

      this.Data.UpdateLater();
      return true;
    }

    #endregion

    public void UpdateLead(LeadDM lead)
    {
      this.Data.leadid = lead.ID.Value;
      this.Data.UpdateLater();
    }
  }
}
