using CCMonkeys.Direct;
using CCMonkeys.Web.Core;
using CCMonkeys.Web.Core.Code;
using CCMonkeys.Web.Core.Sockets.ApiSockets.Data;
using CCMonkeys.Web.Core.Sockets.Dashboard;
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
    public int? ID { get; protected set; } = null;
    public string Key { get; private set; } = string.Empty;
    public ActionDM Data { get; set; }
    public CCSubmitDirect Database { get => this.Socket.Database; }
    public SessionSocket Socket { get; }

    private string TrackingID { get; set; } = string.Empty;
    private string AffID { get; set; } = string.Empty;
    private string PubID { get; set; } = string.Empty;


    public Action(SessionSocket socket)
    {
      this.Socket = socket;
      this.ID = this.Socket.User.Data.actionid;
    }

    public async void Init(int? providerID, PrelanderDM prelander, LanderDM lander)
    {
      // if we had this ID stored in cookies or this user is now on lander
      if (this.ID.HasValue && Socket.SessionType == SessionType.Lander)
      {
        this.Data = await this.Database.Query<ActionDM>().LoadAsync(this.ID.Value);
        if(this.Data != null)
        {
          if(this.Data.leadid.HasValue && Socket.Lead == null)
            this.Socket.Lead = await this.Database.Query<LeadDM>().LoadAsync(this.Data.leadid.Value);

          if(prelander != null)
          {
            this.Data.prelanderid = prelander.ID;
            this.Data.prelandertypeid = prelander.prelandertypeid;
          }
          if(lander != null)
          {
            this.Data.landerid = lander.ID;
            this.Data.landertypeid = lander.landertypeid;
          }

          if(Socket.Lead != null)
          {
            Socket.Lead.OnAction();
            this.Data.leadid = Socket.Lead.ID;
          }

          this.Key = this.Data.guid;
          this.Data.affid = this.AffID;
          this.Data.trackingid = this.TrackingID;
          this.Data.pubid = this.PubID;
          this.Data.providerid = providerID;
          this.Data.input_redirect = (Socket.SessionType == SessionType.Lander);

          this.Data.SetOnAfterInsert(this.OnInsert);
          this.Data.SetOnAfterUpdate(this.OnUpdate);
          this.Data.UpdateLater();
          return;
        }
      }

      // in this case we will create new Action

      this.Key = Guid.NewGuid().ToString();
      this.Data = new ActionDM(this.Database)
      {
        leadid = (Socket.Lead != null ? Socket.Lead.ID : null),
        userid = Socket.User.ID.Value,
        prelanderid = (prelander != null ? prelander.ID : null),
        prelandertypeid = (prelander != null ? (int?)prelander.prelandertypeid : null),
        landerid = (lander != null ? lander.ID : null),
        landertypeid = (lander != null ? (int?)lander.landertypeid : null),
        guid = this.Key,
        providerid = providerID,
        input_redirect = (Socket.SessionType == SessionType.Lander),
        countryid = this.Socket.CountryID,
        affid = this.AffID,
        trackingid = this.TrackingID,
        pubid = this.PubID
      };
      if(Socket.Lead != null)
      {
        Socket.Lead.OnAction();
        this.Data.leadid = Socket.Lead.ID;
      }


      this.Data.SetOnAfterInsert(this.OnInsert);
      this.Data.SetOnAfterUpdate(this.OnUpdate);
      this.Data.Insert();

      this.ID = this.Data.ID.Value;
      this.Socket.User.UpdateAction(this.Data.ID);
    }

    public void OnInsert()
      => DashboardSocket.OnActionInsert(this.Data);
    public void OnUpdate()
      => DashboardSocket.OnActionUpdate(this.Data);

    public void PrepareActionBasedOnQueries(Dictionary<string, string> queryValues)
    {
      if (queryValues.ContainsKey("lxid"))
        this.TrackingID = queryValues["lxid"];
      if (queryValues.ContainsKey("affiliate_id"))
        this.AffID = queryValues["affiliate_id"];
      if (queryValues.ContainsKey("utm_campaign"))
        this.PubID = queryValues["utm_campaign"];
    }

    public void UpdateLead(LeadDM lead)
    {
      this.Data.leadid = lead.ID.Value;
      lead.OnAction();
      this.Data.UpdateLater();
    }
  }
}
