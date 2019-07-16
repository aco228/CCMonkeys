using CCMonkeys.Direct;
using CCMonkeys.Web.Core.Code;
using Direct.ccmonkeys.Models;
using Direct.Core;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.WebSockets;
using CCMonkeys.Web.Core.Sockets.ApiSockets.Data;
using CCMonkeys.Web.Core.Sockets.ApiSockets.Models;

namespace CCMonkeys.Web.Core.Sockets.ApiSockets
{
  public class SessionSocket
  {
    private MainContext MainContext = null;

    public WebSocket WebSocket { get; set; } = null;
    public CCSubmitDirect Database { get => this.MainContext.Database; }
    public Session Session { get; protected set; } = null;
    public SessionType SessionType { get; set; } = SessionType.Default;
    public User User { get; protected set; } = null;
    public LeadDM Lead { get; set; } = null;
    public Models.Action Action { get; protected set; } = null;
    public string Key { get => this.Session.Key; }
    public DateTime Created { get; set; }

    public SessionSocket(MainContext context, SessionType sessionType)
    {
      this.MainContext = context;
      this.SessionType = sessionType;
      var db = this.Database;
      this.Created = DateTime.Now;
      this.User = new User(this, context);
      this.Action = new Models.Action(this, context, (sessionType == SessionType.Prelander));
      this.Session = new Session(this, context);

      DateTime dt = DateTime.Now;
      this.SyncCountry();
      this.TryToIdentifyLead(string.Empty, string.Empty);
    }
    
    public async void SyncCountry()
    {
      if (string.IsNullOrEmpty(this.Session.CountryCode)) return;
      int? countryID = await CountryManager.GetCountryByCode(this.Database, this.Session.CountryCode);
      if (!countryID.HasValue)
        return;

      if (!this.User.Data.countryid.HasValue)
      {
        this.User.Data.countryid = countryID.Value;
        this.User.Data.UpdateLater();
      }

      if(!this.Action.Data.countryid.HasValue)
      {
        this.Action.Data.countryid = countryID.Value;
        this.Action.Data.UpdateLater();
      }
    }

    public async Task<LeadDM> TryToIdentifyLead(string msisdn, string email)
    {
      if(string.IsNullOrEmpty(msisdn) && string.IsNullOrEmpty(email))
      {
        if (this.User.Data.leadid.HasValue)
          this.Lead = await this.Database.Query<LeadDM>().LoadAsync(this.User.Data.leadid.Value);
        else if (this.Action.Data.leadid.HasValue)
          this.Lead = await this.Database.Query<LeadDM>().LoadAsync(this.Action.Data.leadid.Value);

        this.Action.UpdateLead(this.Lead);
        this.User.UpdateLead(this.Lead);
        return this.Lead;
      }

      this.Lead = await LeadDM.LoadByMsisdnOrEmailAsync(this.Database, msisdn, email);
      if (this.Lead == null)
        this.Lead = await new LeadDM(this.Database)
        {
          countryid = Action.Data.countryid,
          msisdn = msisdn,
          email = email
        }.InsertAsync<LeadDM>();

      this.Lead.actions_count++;
      this.Lead.UpdateLater();

      this.Action.UpdateLead(this.Lead);
      this.User.UpdateLead(this.Lead);
      return this.Lead;
    }

    /// 
    /// Communication
    /// 

    public async Task<DistributionModel> OnRegistration(ReceivingRegistrationModel model)
    {
      try
      {
        if (this.SessionType == SessionType.Lander && !model.providerID.HasValue)
          return new SendingRegistrationModel() { }.Pack(false, "providerID missing");

        if (model.url.StartsWith("file:"))
          model.url = this.SessionType == SessionType.Lander ?
            "https://reg.alivesports.co/smartphone-giveaway/?offer_id=2339&affiliate_id=183&country=montenegro&lxid=eWzK2z7bF2qQCGvx3OuWatePMSsWYjfPMYbtBQfsk0&utm_source=banana&utm_medium=183&utm_campaign=&utm_content=&utm_term=&ptype=cc2" :
            "https://lander.giveaways-online.com/l7/?lp=l7&msisdn=000015251255&s=24385385";

        string domain = model.url.Split('?')[0];
        string query = string.Empty;
        if (model.url.Contains('?'))
          query = model.url.Split('?')[1];
        var queryValues = query.Split('&').Select(q => q.Split('=')).ToDictionary(k => k[0], v => v[1]);

        if (this.SessionType == SessionType.Prelander)
        {
          await Session.PrelanderRegistrationLogic(domain, queryValues, model);
          await Action.OnPrelanderRegistration(domain, queryValues, model);
        }
        else if (this.SessionType == SessionType.Lander)
        {
          Action.Data.providerid = model.providerID;
          Action.Data.UpdateLater();

          await Action.OnLanderRegistration(domain, queryValues, model);
        }

        Session.Request.rawurl = model.url;
        Session.Request.UpdateLater();

        var sendingModel = new SendingRegistrationModel()
        {
          lead = Lead,
          sessionData = Session.SessionData,

          actionID = Action.Data.ID,
          sessionID = Session.Data.ID,
          userID = User.Data.ID
        };

        if (SessionType == SessionType.Lander)
        {
          if(Lead != null)
            sendingModel.leadHasSubscription = await this.Database.LoadBooleanAsync("SELECT COUNT(*) FROM [].tm_action WHERE leadid={0} AND providerid={1} AND (times_charged>0 OR has_subscription=1)", Lead.ID.Value, Action.Data.providerid);
          sendingModel.userVisitCount = await this.Database.LoadIntAsync("SELECT COUNT(*) FROM [].tm_action WHERE userid={0} AND landerid={1} AND providerid={2}", User.Data.ID, Action.Data.landerid, Action.Data.providerid);
        }

        return sendingModel.Pack(true, "Welcome!!");
      }
      catch(Exception e)
      {
        Logger.Instance.LogException(e);
        return new SendingRegistrationModel() { }.Pack(false, "fatal error " + e.ToString());
      }
    }
    public async Task<DistributionModel> OnCreateUser(ReceivingCreateUserModel model)
    {
      if (string.IsNullOrEmpty(model.email))
        return new SendingCreateUserModel() { emptyEmail = true }.Pack(false);

      bool blacklistMail = await this.Database.LoadBooleanAsync("SELECT COUNT(*) FROM [].tm_email_blacklist WHERE email={0};", model.email);
      if (blacklistMail)
        return new SendingCreateUserModel() { refused = true }.Pack(false);

      var lead = this.Lead;
      if(lead == null)
        lead = await this.TryToIdentifyLead(string.Empty, model.email);

      lead.TryUpdateEmail(this.Database, model.email);
      lead.UpdateLater();

      Action.Data.input_email = true;
      Action.Data.UpdateLater();



      return new SendingCreateUserModel() { }.Pack(true);
    }

    public async Task<DistributionModel> OnSubscribeUser(ReceivingSubscribeUser model)
    {
      if (this.Lead == null)
        return new SendingSubscribeUser() { internalError_leadDoesNotExists = true }.Pack(false);

      this.Action.Data.input_contact = true;
      this.Action.Data.UpdateLater();

      this.Lead.TryUpdateFirstName(this.Database, model.firstName);
      this.Lead.TryUpdateLastName(this.Database, model.lastName);

      // in some weird case that we already have this msisdn for some other lead
      if(await this.Database.LoadBooleanAsync("SELECT COUNT(*) FROM [].tm_lead WHERE msisdn={0}", model.msisdn) == false)
        this.Lead.TryUpdateMsisdn(this.Database, model.msisdn);
      else
      {
        // TODO: do some logging
      }

      this.Lead.TryUpdateZip(this.Database, model.postcode);
      this.Lead.TryUpdateAddress(this.Database, model.address);
      this.Lead.TryUpdateCity(this.Database, model.city);
      this.Lead.UpdateLater();

      return new SendingCreateUserModel() { }.Pack(true);
    }
    public async Task<DistributionModel> OnUserRedirected(ReceivingUserRedirected model)
    {
      this.Action.Data.has_redirectedToProvider = true;
      this.Action.Data.UpdateLater();

      return new SendingUserRedirected() { }.Pack(true);
    }

    /// 
    /// Base implementation of the sockets
    /// 

    public void OnClose()
    {
      this.Session.OnClose(this.Created);
    }
    public void Send(DistributionModel data)
      => ApiSocketServer.Send(this, data);

  }
}
