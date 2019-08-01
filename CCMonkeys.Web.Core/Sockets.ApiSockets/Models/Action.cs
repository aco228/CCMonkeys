using CCMonkeys.Direct;
using CCMonkeys.Web.Core;
using CCMonkeys.Web.Core.Code;
using CCMonkeys.Web.Core.Sockets.ApiSockets.Data;
using CCMonkeys.Web.Core.Sockets.Dashboard;
using Direct.ccmonkeys.Models;
using Direct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCMonkeys.Loggings;

namespace CCMonkeys.Web.Core.Sockets.ApiSockets.Models
{
  public class Action
  {
    public string Key { get; private set; } = string.Empty;
    public ActionDM Data { get; set; }
    public CCSubmitDirect Database { get => this.Socket.Database; }
    public SessionSocket Socket { get; }
    public bool WeHadKey = true;

    private string TrackingID { get; set; } = string.Empty;
    private string AffID { get; set; } = string.Empty;
    private string PubID { get; set; } = string.Empty;
    public int? LanderID { get; set; } = null;
    public int? LanderTypeID { get; set; } = null;
    public int? PreLanderID { get; set; } = null;
    public int? PreLanderTypeID { get; set; } = null;

    public Action(SessionSocket socket)
    {
      this.Socket = socket;
      this.Key = this.Socket.MainContext.CookiesGet(Constants.ActionID);

      // if we dont have key stored then we will create blank new one
      // or if we are in prelander, we must create new action
      if(string.IsNullOrEmpty(this.Key) || this.Socket.SessionType == SessionType.Prelander)
      {
        this.WeHadKey = false;
        this.Data = new ActionDM(this.Database);
        this.Key = this.Data.GetStringID();
        this.Socket.MainContext.SetCookie(Constants.ActionID, this.Key);
      }
    }

    public async void Init(int? providerID)
    {
      var logger = new MSLogger();
      // if we had this ID stored in cookies or this user is now on lander
      if (this.WeHadKey && !string.IsNullOrEmpty(this.Key) && Socket.SessionType == SessionType.Lander)
      {
        this.Data = await this.Database.Query<ActionDM>().LoadByGuidAsync(this.Key);
        logger.Track("after load");
        if(this.Data != null)
        {
          if(this.Data.leadid.HasValue && Socket.Lead == null)
            this.Socket.Lead = await this.Database.Query<LeadDM>().LoadAsync(this.Data.leadid.Value);

          logger.Track("after lead");

          if (Socket.Lead != null)
          {
            Socket.Lead.OnAction();
            this.Data.leadid = Socket.Lead.ID;
          }
        }
      }

      if (this.Data == null)
      {
        this.Data = new ActionDM(this.Database);
        this.Data.actionid = this.Key;
        this.WeHadKey = false;
      }

      this.Data.leadid = (Socket.Lead != null ? Socket.Lead.ID : null);
      this.Data.userid = Socket.User.Key;
      this.Data.prelanderid = this.PreLanderID;
      this.Data.prelandertypeid = this.PreLanderTypeID;
      this.Data.landerid = this.LanderID;
      this.Data.landertypeid = this.LanderTypeID;
      this.Data.providerid = providerID;
      this.Data.input_redirect = (Socket.SessionType == SessionType.Lander);
      this.Data.countryid = this.Socket.CountryID;
      this.Data.affid = this.AffID;
      this.Data.trackingid = this.TrackingID;
      this.Data.pubid = this.PubID;

      logger.Track("after create");
      if (Socket.Lead != null)
      {
        Socket.Lead.OnAction();
        this.Data.leadid = Socket.Lead.ID;
      }


      logger.Track("after set cookie");

      this.Data.SetOnAfterInsert(this.OnInsert);
      this.Data.SetOnAfterUpdate(this.OnUpdate);
      logger.Track("after actions to database");

      if (this.WeHadKey)
        this.Data.UpdateLater();
      else
        this.Data.InsertLater();

      logger.Track("after insert later");
      int a = 0;
    }

    ///
    /// Prelander data
    ///

    public async void UpdatePrelanderData(string input)
    {
      this.Data.prelander_data = input;
      await this.Data.UpdateAsync();
      DashboardSocket.OnActionUpdate(this.Data);
    }

    ///
    /// 
    ///

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
