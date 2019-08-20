using CCMonkeys.Direct;
using CCMonkeys.Web.Postbacks.Undercover;
using Direct.ccmonkeys.Models;
using Direct;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CCMonkeys.Web.Core.Sockets.Dashboard;
using CCMonkeys.Web.Core.Code.CacheManagers;
using CCMonkeys.Loggings;

namespace CCMonkeys.Web.Postbacks
{
  [ApiController]
  public abstract class PostbackControllerBase<T> : ControllerBase 
    where T : PostbackModelBase
  {
    private CCSubmitDirect _database = null;
    protected PostbackDM Postback = null;
    protected int ProviderID { get; set; } = -1;
    protected bool RequireAction { get; set; } = false;
    protected ActionDM Action { get; set; } = null;
    protected string TrackingID { get; set; } = string.Empty;
    protected LoggingBase Logger { get; set; } = new LoggingBase();
    protected CCSubmitDirect Database
    {
      get
      {
        if (this._database != null)
          return this._database;
        this._database = new CCSubmitDirect();
        return this._database;
      }
    }

    public PostbackControllerBase(int providerID, bool requireAction)
    {
      this.ProviderID = providerID;
      this.RequireAction = requireAction;
    }

    /// 
    /// Main entry point
    /// 

    public async Task<IActionResult> Index([FromQuery] T model)
    {
      try
      {
        model.Prepare(this.Request);
        this.TrackingID = model.TrackingID;
        this.Action = await this.Database.Query<ActionDM>().Where("trackingid={0}", this.TrackingID).LoadSingleAsync();

        await this.PreparePostbackObject();

        if (string.IsNullOrEmpty(this.TrackingID))
        {
          this.Postback.Log("Could not get trackingGuid.. Flow is interupted");
          return this.StatusCode(400);
        }

        if (this.Action == null && this.RequireAction)
        {
          this.Postback.Log("Could not load Action for trackingId: " + this.TrackingID);
          return this.StatusCode(400);
        }

        this.Action = await this.Call(model);
        if (this.Action == null) // abstraction class had some error. we assume that that class will make some log
          return this.StatusCode(400);

        string providerName = (this.Action.providerid.HasValue ? ProvidersCache.Instance.Get(this.Action.providerid.Value).Name : "provider null?");
        if (model.Type == ActionModelEvent.Charge || model.Type == ActionModelEvent.Subscribe)
        {
          DashboardSocket.OnNewTransaction(providerName, this.Action.actionid);
          this.Action.times_charged++;
          this.SystemPostback();
        }

        if (model.Type == ActionModelEvent.Refund)
        {
          DashboardSocket.OnNewRefund(providerName, this.Action.actionid);
          this.Action.has_refund = true;
        }

        if (model.Type == ActionModelEvent.Chargeback)
        {
          DashboardSocket.OnNewChargeback(providerName, this.Action.actionid);
          this.Action.has_chargeback = true;
        }

        this.Action.UpdateLater();
        await this.Database.TransactionalManager.RunAsync();

        // Redirect to page for facebook pixel
        if(this.Action != null && !string.IsNullOrEmpty(this.Action.fbid))
          return this.Redirect("/fbid/" + this.Action.fbid);

        return StatusCode(200);
      }
      catch (Exception e)
      {
        this.Logger.StartLoggin(model != null ? model.TrackingID : "notracking")
          .Add("providername", ProvidersCache.Instance.Get(this.ProviderID).Name)
          .Add("query", HttpContext.Request.QueryString.Value)
          .Add(model != null ? model : null)
          .OnException(e);
        return StatusCode(200);
      }
    }

    /// 
    /// Implementation method for apstraction
    /// 

    protected abstract Task<ActionDM> Call(T model);

    /// 
    /// Prepare object Postback for database (log url and all other logs)
    /// 

    private async Task<PostbackDM> PreparePostbackObject()
    {
      this.Postback = await new PostbackDM(this.Database)
      {
        providerid = this.ProviderID,
        trackingid = this.TrackingID,
        actionid = (this.Action != null ? this.Action.GetStringID() : null),
        url = Request.GetEncodedUrl()
      }.InsertAsync<PostbackDM>();
      return this.Postback;
    }


    // SUMMARY: Send postback informations to every other instance (legacy, bananaclicks, undercover)
    // Return if conversion is stolen
    protected void SystemPostback()
    {
      UndercoverResult undercover = CCUndercoverAgent.Init(this.Action, this.Postback);
      if (!undercover.DontSendConversionToBananaclicks)
      {
        this.Postback.Log("This transaction will not be stolen and will be sent to banana ::clickid=" + this.TrackingID);
        this.SendPostbackToBananaclicks();
      }
      else
      {
        this.Action.has_stolen = true;
        this.Action.UpdateLater();

        this.Postback.Log("THIS TRANSACTION WILL BE STOLEN ::clickid=" + this.TrackingID);
      }
    }


    // SEND POSTBACK TO BANANA
    private void SendPostbackToBananaclicks()
    {
      return;

      //if (string.IsNullOrEmpty(this.TrackingID))
      //{
      //  this.Postback.Log("Transaction IS NULL so callback to banana will not be sent");
      //  return;
      //}

      //string postbackLink = "http://conversions.bananaclicks.com/?transaction_id=" + this.TrackingID;
      //this.Postback.Log("BANANA: " + postbackLink);
      //HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(postbackLink);
      //webRequest.AllowAutoRedirect = false;
      //HttpWebResponse response;
      //HttpStatusCode statusCode;

      //try
      //{
      //  response = (HttpWebResponse)webRequest.GetResponse();
      //}
      //catch (WebException we)
      //{
      //  // TODO: Add better logging
      //  this.Postback.Log("Banana postback returned expection: " + we.Message);
      //  return;
      //}
    }

    
  }
}
