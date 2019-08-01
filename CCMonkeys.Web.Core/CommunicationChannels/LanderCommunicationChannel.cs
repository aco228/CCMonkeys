using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CCMonkeys.Direct;
using CCMonkeys.Loggings;
using CCMonkeys.Sockets;
using CCMonkeys.Web.Core.Sockets.ApiSockets;
using CCMonkeys.Web.Core.Sockets.ApiSockets.Data;
using Direct.ccmonkeys.Models;
using Newtonsoft.Json;

namespace CCMonkeys.Web.Core.CommunicationChannels
{
  public class LanderCommunicationChannel : CommunicationChannelBase
  {
    public LanderCommunicationChannel(LoggingBase logger, ActionDM action, string user, int? countryID, CCSubmitDirect db) : base(logger, action, user, countryID, db) { }
    public LanderCommunicationChannel(SessionSocket session) : base(session) { }

    public override async Task<DistributionModel> Call(string key, string json)
    {
      if (key == "user-create")
        return await this.OnCreateUser(key, JsonConvert.DeserializeObject<ReceivingCreateUserModel>(json));
      else if (key == "user-subscribe")
        return await this.OnSubscribeUser(key, JsonConvert.DeserializeObject<ReceivingSubscribeUser>(json));
      else if (key == "user-redirected")
        return await this.OnUserRedirected(key, JsonConvert.DeserializeObject<ReceivingUserRedirected>(json));

      return null;
    }

    public async Task<DistributionModel> OnCreateUser(string key, ReceivingCreateUserModel model)
    {
      try
      {
        if (string.IsNullOrEmpty(model.email))
          return new SendingCreateUserModel() { emptyEmail = true }.Pack(false);

        bool blacklistMail = await this.Database.LoadBooleanAsync("SELECT COUNT(*) FROM [].tm_email_blacklist WHERE email={0};", model.email);
        if (blacklistMail)
          return new SendingCreateUserModel() { refused = true }.Pack(false);

        this.Send(key, new SendingCreateUserModel() { }.Pack(true));

        var lead = this.Lead;
        if (lead == null)
          lead = await this.TryToIdentifyLead(string.Empty, model.email);

        if(lead == null)
        {
          // TODO: we have a problem right here!!
          throw new Exception("We dont have lead on create user");
        }

        lead.actions_count++;
        lead.TryUpdateEmail(this.Database, model.email);
        lead.UpdateLater();

        this.Action.input_email = true;
        this.Action.UpdateLater();

        await this.Database.TransactionalManager.RunAsync();

        return new DistributionModel() { Status = true };
      }
      catch (Exception e)
      {
        this.Logger.StartLoggin(this.Action.GetStringID())
          .Where("lp-createUser")
          .Add(model)
          .OnException(e);

        return new FatalModel() { Action = "OnCreateUser", Exception = e.ToString() }.Pack(false, "error500");
      }
    }
    public async Task<DistributionModel> OnSubscribeUser(string key, ReceivingSubscribeUser model)
    {
      try
      {
        if (this.Lead == null)
          return new SendingSubscribeUser() { internalError_leadDoesNotExists = true }.Pack(false);
        else
          this.Send(key, new SendingCreateUserModel() { }.Pack(true));

        this.Action.input_contact = true;
        this.Action.UpdateLater();

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
        return new DistributionModel() { Status = true };
      }
      catch (Exception e)
      {
        this.Logger.StartLoggin(this.Action.GetStringID())
          .Where("lp-subscribe")
          .Add(model)
          .OnException(e);
        return new FatalModel() { Action = "OnSubscribeUser", Exception = e.ToString() }.Pack(false, "error500");
      }
    }
    public async Task<DistributionModel> OnUserRedirected(string key, ReceivingUserRedirected model)
    {
      try
      {
        this.Send(key, new SendingUserRedirected() { }.Pack(true));

        this.Action.has_redirectedToProvider = true;
        this.Action.UpdateLater();

        await this.Database.TransactionalManager.RunAsync();
        return new DistributionModel() { Status = true };
      }
      catch (Exception e)
      {
        this.Logger.StartLoggin(this.Action.GetStringID())
          .Where("lp-redirect")
          .Add(model)
          .OnException(e);
        return new FatalModel() { Action = "OnUserRedirected", Exception = e.ToString() }.Pack(false, "error500");
      }
    }

  }
}
