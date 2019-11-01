using CCMonkeys.Loggings;
using CCMonkeys.Sockets;
using CCMonkeys.Web.Core.Code.CacheManagers;
using CCMonkeys.Web.Core.Sockets.ApiSockets.Code;
using CCMonkeys.Web.Core.Sockets.ApiSockets.Data;
using CCMonkeys.Web.Core.Sockets.ApiSockets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMonkeys.Web.Core.Sockets.ApiSockets.Communication
{
  public class LanderCommunication : CommunicationBase
  {
    public LanderCommunication(SessionSocket socket) : base(socket) { }


    public async void OnRegistration(string key, ReceivingRegistrationModel model)
    {
      try
      {
        MSLogger logger = new MSLogger();

        // provider id will be present in model.url
        //if (!model.providerID.HasValue)
        //{
        //  this.Socket.Send(key, new SendingRegistrationModel() { }.Pack(false, "providerID missing"));
        //  return;
        //}

        if(string.IsNullOrEmpty(model.url))
        {
          this.Socket.Send(key, new SendingRegistrationModel() { }.Pack(false, "url is missing"));
          return;
        }

#if DEBUG

        if (model.url.StartsWith("file:"))
          model.url = "https://giveaway.alivesports.co/s10-giveaway/index.html?offer_id=2470&affiliate_id=522&country=spain&lxid=Ugm81c7gHJ37w8rHndfViUl2eljZ2lxC&utm_source=banana&utm_medium=522&utm_campaign=40&utm_content=&utm_term=&ptype=cc2";

#endif
        DomainManager domainManager = DomainManager.InitiateLander(model.url);
        if(domainManager.HasError)
        {
          this.Socket.Send(key, new SendingRegistrationModel() { }.Pack(false, domainManager.ErrorMessage));
          return;
        }

        var lander = domainManager.Lander;
        this.Socket.Action.LanderID = lander.ID;
        this.Socket.Action.LanderTypeID = lander.Type.ID;
        this.Socket.Action.ProviderID = domainManager.Provider.ID;
        this.Socket.Action.PrepareActionBasedOnQueries(domainManager.Queries);

        /// SENDING
        /// 

        var sendingModel = new SendingRegistrationModel()
        {
          lead = this.Socket.Lead,
          country = this.Socket.Session.CountryCode,
          lander = lander
        };
        if (this.Socket.Lead != null)
          sendingModel.leadHasSubscription = await this.Socket.Lead.HasLeadSubscriptions(model.providerID.Value);

        this.Socket.Send(sendingModel.Pack(key, true, "Welcome!!"));

        /// Inserting action and session
        /// 

        //this.Socket.Action.Init(model.providerID);
        this.Socket.Action.Init();
        this.Socket.Session.Init();

        if(string.IsNullOrEmpty(this.Socket.Session.Request.rawurl))
          this.Socket.Session.Request.rawurl = model.url;
        this.Socket.Session.Request.UpdateLater();

        this.Socket.Send("reg-post", new SendingRegistrationPost()
        {
          lead = this.Socket.Lead,
          country = this.Socket.Session.CountryCode,
          actionID = this.Socket.Action.Data.GetStringID(),
          sessionID = this.Socket.Session.Data.GetStringID(),
          userID = this.Socket.User.Key,
          provider = domainManager.Provider,
          lander = domainManager.Lander,
          Loggers = logger.Tracks
        }.Pack());

        await this.Socket.Database.TransactionalManager.RunAsync();
      }
      catch (Exception e)
      { 
        this.Socket.Logging.StartLoggin("")
          .Where("lp-registration")
          .Add("url", model.url)
          .Add("providerID?", model.providerID.HasValue ? model.providerID.Value.ToString() : "null")
          .OnException(e);

        this.Socket.Send(new FatalModel() { Action = "OnRegistration", Exception = e.ToString() }.Pack(false, "error500"));
      }
    }
    public async void OnCreateUser(string key, ReceivingCreateUserModel model)
    {
      try
      {
        if (string.IsNullOrEmpty(model.email))
        {
          this.Socket.Send(key, new SendingCreateUserModel() { emptyEmail = true }.Pack(false));
          return;
        }

        bool blacklistMail = await this.Socket.Database.LoadBooleanAsync("SELECT COUNT(*) FROM [].tm_email_blacklist WHERE email={0};", model.email);
        if (blacklistMail)
        {
          this.Socket.Send(key, new SendingCreateUserModel() { refused = true }.Pack(false));
          return;
        }
        this.Socket.Send(key, new SendingCreateUserModel() { }.Pack(true));

        var lead = this.Socket.Lead;
        if (lead == null)
          lead = await this.Socket.TryToIdentifyLead(string.Empty, model.email);

        lead.TryUpdateEmail(this.Socket.Database, model.email);
        lead.UpdateLater();

        this.Socket.Action.UpdateLead(lead);
        this.Socket.Action.Data.input_email = true;
        this.Socket.Action.Data.UpdateLater();

        await this.Socket.Database.TransactionalManager.RunAsync();
      }
      catch (Exception e)
      {
        this.Socket.Logging.StartLoggin("")
          .Where("lp-createUser")
          .Add(model)
          .OnException(e);
        this.Socket.Send(new FatalModel() { Action = "OnCreateUser", Exception = e.ToString() }.Pack(false, "error500"));
      }
    }

    public async void OnSubscribeUser(string key, ReceivingSubscribeUser model)
    {
      try
      {
        if (this.Socket.Lead == null)
        {
          this.Socket.Send(key, new SendingSubscribeUser() { internalError_leadDoesNotExists = true }.Pack(false));
          return;
        }
        else
          this.Socket.Send(key, new SendingCreateUserModel() { }.Pack(true));

        this.Socket.Action.Data.input_contact = true;
        this.Socket.Action.Data.UpdateLater();

        this.Socket.Lead.TryUpdateFirstName(this.Socket.Database, model.firstName);
        this.Socket.Lead.TryUpdateLastName(this.Socket.Database, model.lastName);

        // in some weird case that we already have this msisdn for some other lead
        if (await this.Socket.Database.LoadBooleanAsync("SELECT COUNT(*) FROM [].tm_lead WHERE msisdn={0}", model.msisdn) == false)
          this.Socket.Lead.TryUpdateMsisdn(this.Socket.Database, model.msisdn);
        else
        {
          // TODO: do some logging
        }

        this.Socket.Lead.TryUpdateZip(this.Socket.Database, model.postcode);
        this.Socket.Lead.TryUpdateAddress(this.Socket.Database, model.address);
        this.Socket.Lead.TryUpdateCity(this.Socket.Database, model.city);
        this.Socket.Lead.UpdateLater();

        await this.Socket.Database.TransactionalManager.RunAsync();
      }
      catch (Exception e)
      {
        this.Socket.Logging.StartLoggin("")
          .Where("lp-subscribe")
          .Add(model)
          .OnException(e);
        this.Socket.Send(new FatalModel() { Action = "OnSubscribeUser", Exception = e.ToString() }.Pack(false, "error500"));
      }
    }
    public async void OnUserRedirected(string key, ReceivingUserRedirected model)
    {
      try
      {
        this.Socket.Send(key, new SendingUserRedirected() { }.Pack(true));

        this.Socket.Action.Data.has_redirectedToProvider = true;
        this.Socket.Action.Data.UpdateLater();

        await this.Socket.Database.TransactionalManager.RunAsync();
      }
      catch (Exception e)
      {
        this.Socket.Logging.StartLoggin("")
          .Where("lp-redirect")
          .Add(model)
          .OnException(e);
        this.Socket.Send(new FatalModel() { Action = "OnUserRedirected", Exception = e.ToString() }.Pack(false, "error500"));
      }
    }

  }
}
