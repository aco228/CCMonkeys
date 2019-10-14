using CCMonkeys.Loggings;
using CCMonkeys.Sockets;
using CCMonkeys.Web.Core.Code;
using CCMonkeys.Web.Core.Code.CacheManagers;
using CCMonkeys.Web.Core.Sockets.ApiSockets.Code;
using CCMonkeys.Web.Core.Sockets.ApiSockets.Data;
using CCMonkeys.Web.Core.Sockets.ApiSockets.Models;
using Direct.ccmonkeys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMonkeys.Web.Core.Sockets.ApiSockets.Communication
{
  public class PrelanderCommunication : CommunicationBase
  {
    public PrelanderCommunication(SessionSocket socket) : base(socket) { }

    public PreLanderCacheModel Prelander { get; set; } = null;
    public Dictionary<string, int?> TagManager = new Dictionary<string, int?>();
    public string ActionPrelanderCache
    {
      get
      {
        if (TagManager == null || TagManager.Count == 0)
          return null;
        string result = ".";
        foreach (var t in TagManager)
          result += t.Key + "=" + (t.Value.HasValue ? t.Value.Value.ToString() : "") + ".";
        return result;
      }
    }

    public async void OnRegistration(string key, ReceivingRegistrationModel model)
    {
      try
      {
        MSLogger logger = new MSLogger();

#if DEBUG

        if (model.url.StartsWith("file:"))
        {
          string parameters = "";
          if (model.url.Contains("?"))
            parameters = "?" + model.url.Split('?')[1];

          model.url = "https://lander.giveaways-online.com/l7/" + parameters;
        }

#endif

        DomainManager domainManager = new DomainManager(model.url);

        if(domainManager.HasError)
        {
          this.Socket.Send(key, new SendingRegistrationModel() { }.Pack(false, "Prelander not found"));
          throw new Exception(string.Format("Prelander is not found for domain '{0}'", domainManager.Url));
          return;
        }
        this.Prelander = domainManager.Prelander;

        // 
        // Check for all neceseary parameters in url
        //

        if(!domainManager.Queries.ContainsKey("dbg"))
        {
          if(!domainManager.Queries.ContainsKey("msisdn"))
          {
            this.Socket.Send(key, new SendingRegistrationModel() { }.Pack(false, "Missing msidn params"));
            throw new Exception(string.Format("Msisdn parameter is not present in url '{0}'", domainManager.Url));
          }
        }



        this.Socket.Action.PreLanderID = this.Prelander.ID;
        this.Socket.Action.PreLanderTypeID = this.Prelander.Type.ID;

        await this.Socket.Session.PrelanderRegistrationLogic(domainManager.Url, domainManager.Queries, model);

        this.Socket.Action.PrepareActionBasedOnQueries(domainManager.Queries);

        logger.Track("this.Action.PrepareActionBasedOnQueries(queryValues);");

        /// SENDING
        /// 

        var sendingModel = new SendingRegistrationModel()
        {
          lead = this.Socket.Lead,
          country = this.Socket.Session.CountryCode,
          prelanderID = Prelander.ID
        };
        if (this.Socket.SessionType == SessionType.Lander)
        {
          if (this.Socket.Lead != null)
            sendingModel.leadHasSubscription = await this.Socket.Lead.HasLeadSubscriptions(model.providerID.Value);
        }
        this.Socket.Send(sendingModel.Pack(key, true, "Welcome!!"));

        logger.Track("sending model");

        /// Inserting action and session
        /// 

        this.Socket.Action.Init(model.providerID);
        this.Socket.Session.Init();

        this.Socket.Session.Request.rawurl = model.url;
        this.Socket.Session.Request.UpdateLater();
        logger.Track("sessionRequest update");

        this.Socket.Send("reg-post", new SendingRegistrationPost()
        {
          actionID = this.Socket.Action.Data.GetStringID(),
          sessionID = this.Socket.Session.Data.GetStringID(),
          userID = this.Socket.User.Key,
          Loggers = logger.Tracks
        }.Pack());

        await this.Socket.Database.TransactionalManager.RunAsync();
      }
      catch (Exception e)
      {
        this.Socket.Logging.StartLoggin("")
          .Where("pl-registration")
          .Add(model)
          .OnException(e);

        this.Socket.Send(new FatalModel() { Action = "OnRegistration", Exception = e.ToString() }.Pack(false, "error500"));
      }
    }



  }
}
