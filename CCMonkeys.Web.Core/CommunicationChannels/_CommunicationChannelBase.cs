using CCMonkeys.Direct;
using CCMonkeys.Loggings;
using CCMonkeys.Sockets;
using CCMonkeys.Web.Core.Sockets.ApiSockets;
using Direct;
using Direct.ccmonkeys.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CCMonkeys.Web.Core.CommunicationChannels
{
  public abstract class CommunicationChannelBase
  {
    private LoggingBase _logger { get; set; } = null;
    private ActionDM _action { get; set; } = null;
    private string _userInput { get; set; } = string.Empty;
    private UserDM _user { get; set; } = null;
    private int? _countryID { get; set; } = null;
    private CCSubmitDirect _database { get; set; } = null;
    protected SessionSocket _session { get; set; } = null;
    private LeadDM _lead { get; set; } = null;

    public ActionDM Action
    {
      get
      {
        if (this._action != null)
          return this._action;
        else if (this._session != null)
          return this._session.Action.Data;
        else
          return null;
      }
    }
    public UserDM User
    {
      get
      {
        if (this._user != null)
          return this._user;
        else if(!string.IsNullOrEmpty(this._userInput))
        {
          this._user = this.Database.Query<UserDM>().LoadByGuid(this._userInput);
          return this._user;
        }
        else if (this._session != null)
          return this._session.User.Data;
        else
          return null;
      }
    }
    public int? CountryID
    {
      get
      {
        if (this._countryID.HasValue)
          return this._countryID;
        else if (this._session != null)
          return this._session.CountryID;
        return null;
      }
    }
    public CCSubmitDirect Database
    {
      get
      {
        if (this._database != null)
          return this._database;
        else if (this._session != null)
          return this._session.Database;
        else
          return null;
      }
    }
    public LeadDM Lead
    {
      get
      {
        if (this._lead != null)
          return this._lead;
        if (this._action != null && this._action.leadid.HasValue)
        {
          this._lead = this.Database.Query<LeadDM>().Load(this._action.leadid.Value);
          return this._lead;
        }
        else if (this._session != null && this._session.Lead != null)
        {
          this._lead = this._session.Lead;
          return this._lead;
        }
        else
          return null;
      }
    }
    public LoggingBase Logger
    {
      get
      {
        if (this._logger != null)
          return this._logger;
        else if (this._session != null)
          return this._session.Logging;
        else
          return null;
      }
    }
    public async Task<LeadDM> TryToIdentifyLead(string msisdn, string email)
    {
      this._lead = await LeadDM.LoadByMsisdnOrEmailAsync(this.Database, msisdn, email);
      if (this._lead == null)
        this._lead = await new LeadDM(this.Database)
        {
          countryid = this.CountryID,
          msisdn = msisdn,
          email = email
        }.InsertAsync<LeadDM>();

      this.User.leadid = this._lead.ID.Value;
      this.User.UpdateLater();
      return this._lead;
    }

    public CommunicationChannelBase(LoggingBase logger, ActionDM action, string user, int? countryID, CCSubmitDirect db)
    {
      this._action = action;
      this._userInput = user;
      this._countryID = countryID;
      this._database = db;
    }

    public CommunicationChannelBase(SessionSocket session)
    {
      this._session = session;
    }

    public async Task<DistributionModel> Start(string key, string json)
    {
      var result = await this.Call(key, json);
      if (this._session != null)
        this._session.Send(key, result);
      return result;
    }

    public abstract Task<DistributionModel> Call(string key, string json);

    public void Send(string key, DistributionModel model)
    {
      if (this._session != null)
        this._session.Send(key, model);
    }


  }
}
