using CCMonkeys.Direct;
using CCMonkeys.Web.Core.Code;
using Direct.ccmonkeys.Models;
using Direct;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.WebSockets;
using CCMonkeys.Web.Core.Sockets.ApiSockets.Data;
using CCMonkeys.Web.Core.Sockets.ApiSockets.Models;
using System.Collections.Generic;
using CCMonkeys.Sockets;
using CCMonkeys.Web.Core.Sockets.Dashboard;
using CCMonkeys.Web.Core.Logging;
using CCMonkeys.Web.Core.Code.CacheManagers;

namespace CCMonkeys.Web.Core.Sockets.ApiSockets
{
  public class SessionSocket
  {
    public MainContext MainContext = null;
    public WebSocket WebSocket { get; set; } = null;
    public CCSubmitDirect Database { get; protected set; } = null;

    public LeadDM Lead { get; set; } = null; // * we will try to load it on Init methods of User and Action
    public User User { get; protected set; } = null;
    public Session Session { get; protected set; } = null;
    public SessionType SessionType { get; set; } = SessionType.Default;
    public Models.Action Action { get; protected set; } = null;

    public int? CountryID { get => this.Session.CountryID; }
    public string Key { get => this.Session.Key; }
    public DateTime Created { get; set; }

    public SessionSocket(MainContext context, SessionType sessionType)
    {
      this.Database = new CCSubmitDirect();
      MSLogger mslogger = new MSLogger();

      this.Created = DateTime.Now;
      this.MainContext = context;
      this.SessionType = sessionType;

      mslogger.Track("before session");
      this.Session = new Session(this); // first we prepare all parts, and get country
      mslogger.Track("after session, before user");
      this.User = new User(this);
      mslogger.Track("after user, before action");
      this.Action = new Models.Action(this);
      mslogger.Track("after action, before session init");

      this.Database.TransactionalManager.RunAsync();
      mslogger.Track("after tm");
      int a = 0;
    }


    public async Task<LeadDM> TryToIdentifyLead(string msisdn, string email)
    {
      this.Lead = await LeadDM.LoadByMsisdnOrEmailAsync(this.Database, msisdn, email);
      if (this.Lead == null)
        this.Lead = await new LeadDM(this.Database)
        {
          countryid = Session.CountryID,
          msisdn = msisdn,
          email = email
        }.InsertAsync<LeadDM>();
      this.User.UpdateLead(this.Lead);
      return this.Lead;
    }

    /// 
    /// Communication
    /// 

    public async void OnRegistration(string key, ReceivingRegistrationModel model)
    {
      try
      {
        MSLogger logger = new MSLogger();
        if (this.SessionType == SessionType.Lander && !model.providerID.HasValue)
        {
          this.Send(key, new SendingRegistrationModel() { }.Pack(false, "providerID missing"));
          return;
        }

        logger.Track("sessions, after action");

        #if DEBUG

          if (model.url.StartsWith("file:"))
            model.url = this.SessionType == SessionType.Lander ?
              "https://reg.alivesports.co/smartphone-giveaway/?offer_id=2339&affiliate_id=183&country=montenegro&lxid=eWzK2z7bF2qQCGvx3OuWatePMSsWYjfPMYbtBQfsk0&utm_source=banana&utm_medium=183&utm_campaign=&utm_content=&utm_term=&ptype=cc2" :
              "https://lander.giveaways-online.com/l7/?lp=l7&msisdn=000015251255&s=24385385";

        #endif

        string domain = model.url.Split('?')[0];
        string query = string.Empty;
        if (model.url.Contains('?'))
          query = model.url.Split('?')[1];
        var querySplit = query.Split('&');
        Dictionary<string, string> queryValues = new Dictionary<string, string>();
        if (querySplit.Length > 1)
          queryValues = query.Split('&').Select(q => q.Split('=')).ToDictionary(k => k[0], v => v[1]);


        logger.Track("domains manipulation"); 

        if (this.SessionType == SessionType.Prelander)
        {
          var prelander = PrelandersCache.Instance.GetByUrl(domain);
          if (prelander == null)
          {
            this.Send(key, new SendingRegistrationModel() { }.Pack(false, "Prelander not found"));
            return;
          }

          this.Action.PreLanderID = prelander.ID;
          this.Action.PreLanderTypeID = prelander.Type.ID;

          await Session.PrelanderRegistrationLogic(domain, queryValues, model);
        }
        else if (this.SessionType == SessionType.Lander)
        {
          var lander = LandersCache.Instance.GetByUrl(domain);
          if (lander == null)
          {
            this.Send(key, new SendingRegistrationModel() { }.Pack(false, "Lander not found"));
            return;
          }

          this.Action.LanderID = lander.ID;
          this.Action.LanderTypeID = lander.Type.ID;
        }


        logger.Track("prelander or lander");

        this.Action.PrepareActionBasedOnQueries(queryValues);

        logger.Track("this.Action.PrepareActionBasedOnQueries(queryValues);");

        /// SENDING
        /// 

        var sendingModel = new SendingRegistrationModel()
        {
          lead = Lead,
          country = this.Session.CountryCode
        };
        if (SessionType == SessionType.Lander)
        {
          if (Lead != null)
            sendingModel.leadHasSubscription = await Lead.HasLeadSubscriptions(model.providerID.Value);
        }
        this.Send(sendingModel.Pack(key, true, "Welcome!!"));

        logger.Track("sending model");

        /// Inserting action and session
        /// 

        this.Action.Init(model.providerID);
        this.Session.Init();

        Session.Request.rawurl = model.url;
        Session.Request.UpdateLater();
        logger.Track("sessionRequest update");
        
        this.Send("reg-post", new SendingRegistrationPost()
        {
          actionID = this.Action.Data.GetStringID(),
          sessionID = this.Session.Data.GetStringID(),
          userID = this.User.Key,
          Loggers = logger.Tracks
        }.Pack());

        await this.Database.TransactionalManager.RunAsync();
      }
      catch (Exception e)
      {
        this.Send(new FatalModel() { Action = "OnRegistration", Exception = e.ToString() }.Pack(false, "error500"));
      }
    }
    public async void OnCreateUser(string key, ReceivingCreateUserModel model)
    {
      try
      {
        if (string.IsNullOrEmpty(model.email))
        {
          this.Send(key, new SendingCreateUserModel() { emptyEmail = true }.Pack(false));
          return;
        }

        bool blacklistMail = await this.Database.LoadBooleanAsync("SELECT COUNT(*) FROM [].tm_email_blacklist WHERE email={0};", model.email);
        if (blacklistMail)
        {
          this.Send(key, new SendingCreateUserModel() { refused = true }.Pack(false));
          return;
        }
        this.Send(key, new SendingCreateUserModel() { }.Pack(true));

        var lead = this.Lead;
        if (lead == null)
          lead = await this.TryToIdentifyLead(string.Empty, model.email);

        lead.TryUpdateEmail(this.Database, model.email);
        lead.UpdateLater();

        Action.UpdateLead(lead);
        Action.Data.input_email = true;
        Action.Data.UpdateLater();

        await this.Database.TransactionalManager.RunAsync();
      }
      catch (Exception e)
      {
        this.Send(new FatalModel() { Action = "OnCreateUser", Exception = e.ToString() }.Pack(false, "error500"));
      }
    }

    public async void OnSubscribeUser(string key, ReceivingSubscribeUser model)
    {
      try
      {
        if (this.Lead == null)
        {
          this.Send(key, new SendingSubscribeUser() { internalError_leadDoesNotExists = true }.Pack(false));
          return;
        }
        else
          this.Send(key, new SendingCreateUserModel() { }.Pack(true));

        this.Action.Data.input_contact = true;
        this.Action.Data.UpdateLater();

        this.Lead.TryUpdateFirstName(this.Database, model.firstName);
        this.Lead.TryUpdateLastName(this.Database, model.lastName);

        // in some weird case that we already have this msisdn for some other lead
        if (await this.Database.LoadBooleanAsync("SELECT COUNT(*) FROM [].tm_lead WHERE msisdn={0}", model.msisdn) == false)
          this.Lead.TryUpdateMsisdn(this.Database, model.msisdn);
        else
        {
          // TODO: do some logging
        }

        this.Lead.TryUpdateZip(this.Database, model.postcode);
        this.Lead.TryUpdateAddress(this.Database, model.address);
        this.Lead.TryUpdateCity(this.Database, model.city);
        this.Lead.UpdateLater();

        await this.Database.TransactionalManager.RunAsync();
      }
      catch (Exception e)
      {
        this.Send(new FatalModel() { Action = "OnSubscribeUser", Exception = e.ToString() }.Pack(false, "error500"));
      }
    }
    public async void OnUserRedirected(string key, ReceivingUserRedirected model)
    {
      try
      {
        this.Send(key, new SendingUserRedirected() { }.Pack(true));

        this.Action.Data.has_redirectedToProvider = true;
        this.Action.Data.UpdateLater();

        await this.Database.TransactionalManager.RunAsync();
      }
      catch (Exception e)
      {
        this.Send(new FatalModel() { Action = "OnUserRedirected", Exception = e.ToString() }.Pack(false, "error500"));
      }
    }

    /// 
    /// Base implementation of the sockets
    /// 

    public void OnCreate()
    {
      this.Created = DateTime.Now;
      this.Session.OnCreate();
    }

    public void OnClose()
    {
      if(this.Action.Data != null)
        DashboardSocket.OnActionOffline(this.Action.Data);
      this.Session.OnClose(this.Created);
    }
    public void Send(DistributionModel data)
      => ApiSocketServer.Send(this, data);

    public void Send(string key, DistributionModel data)
    {
      data.Key = key;
      ApiSocketServer.Send(this, data);
    }

  }
}
