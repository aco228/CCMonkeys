using CCMonkeys.Sockets;
using CCMonkeys.Web.Core.Code.CacheManagers;
using CCMonkeys.Web.Core.Logging;
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

    private PreLanderCacheModel Prelander { get; set; } = null;
    public Dictionary<string, int> TagManager = new Dictionary<string, int>();
    public string ActionPrelanderCache
    {
      get
      {
        if (TagManager == null || TagManager.Count == 0)
          return null;
        string result = ".";
        foreach (var t in TagManager)
          result += t.Key + "=" + t.Value + ".";
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
          model.url = "https://lander.giveaways-online.com/l7/?lp=l7&msisdn=000015251255&s=24385385";

#endif

        DomainManager domainManager = new DomainManager(model.url);

        this.Prelander = PrelandersCache.Instance.GetByUrl(domainManager.Domain);
        if (this.Prelander == null)
        {
          this.Socket.Send(key, new SendingRegistrationModel() { }.Pack(false, "Prelander not found"));
          return;
        }

        this.Socket.Action.PreLanderID = this.Prelander.ID;
        this.Socket.Action.PreLanderTypeID = this.Prelander.Type.ID;

        await this.Socket.Session.PrelanderRegistrationLogic(domainManager.Domain, domainManager.Queries, model);

        this.Socket.Action.PrepareActionBasedOnQueries(domainManager.Queries);

        logger.Track("this.Action.PrepareActionBasedOnQueries(queryValues);");

        /// SENDING
        /// 

        var sendingModel = new SendingRegistrationModel()
        {
          lead = this.Socket.Lead,
          country = this.Socket.Session.CountryCode
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
        this.Socket.Logging.StartLoggin()
          .Where("pl-registration")
          .Add(model)
          .OnException(e);

        this.Socket.Send(new FatalModel() { Action = "OnRegistration", Exception = e.ToString() }.Pack(false, "error500"));
      }
    }

    public async void OnInit(string key, PrelanderInitModel model)
    {
      try
      {
        if (this.Prelander.Answers == null || this.Prelander.Tags == null || this.Prelander.Tags.Count == 0 || this.Prelander.Answers.Count == 0)
        {
          foreach (var tag in model.tags)
          {
            var newTag = new PrelanderTagDM(this.Socket.Database)
            {
              prelandertagid = string.Format("{0}.{1}.{2}", this.Prelander.ID, (tag.isQuestion ? "a" : "t"), tag.name),
              name = tag.name,
              value = tag.value,
              prelanderid = this.Prelander.ID,
              isQuestion = tag.isQuestion
            };
            newTag.InsertLater();
            this.Prelander.Tags.Add(newTag);

            if (tag.answers != null)
              for (int i = 0; i < tag.answers.Length; i++)
              {
                var newAnswer = new PrelanderTagAnswerDM(this.Socket.Database)
                {
                  answerid = string.Format("{0}-{1}", newTag.GetStringID(), i),
                  prelandertagid = newTag.GetStringID(),
                  prelanderid = this.Prelander.ID,
                  tagName = tag.name,
                  name = string.Format("ccqa" + i),
                  value = tag.answers[i]
                };
                newAnswer.InsertLater();
                this.Prelander.Answers.Add(newAnswer);
              }

          }
        }

        foreach (var tag in model.tags)
          this.TagManager.Add(tag.name, 0);

        this.Socket.Action.UpdatePrelanderData(this.ActionPrelanderCache);
        this.Socket.Send(key);
        this.Socket.Database.TransactionalManager.RunAsync();
      }
      catch(Exception e)
      {
        this.Socket.Logging.StartLoggin()
          .Add("where", "pl-init")
          .Add("tagCount", model.tags.Count.ToString())
          .OnException(e);
      }
    }
    public async void OnTag(string key, PrelanderTagModel model)
    {
      try
      {
        var tag = PrelandersCache.Instance.GetTag(this.Prelander.ID, model.tag);
        if (tag == null)
        {
          this.Socket.Send(key);
          return;
        }

        if (this.TagManager.ContainsKey(model.tag))
        {
          this.TagManager[model.tag] = 1;
          this.Socket.Action.UpdatePrelanderData(this.ActionPrelanderCache);
        }

        var interaction = new PrelanderTagActionInteractionDM(this.Socket.Database)
        {
          prelanderid = this.Prelander.ID,
          actionid = this.Socket.Action.Key,
          prelandertagid = tag.GetStringID()
        };
        interaction.InsertLater();

        this.Socket.Send(key);
        this.Socket.Database.TransactionalManager.RunAsync();
      }
      catch(Exception e)
      {
        this.Socket.Logging.StartLoggin()
          .Add("where", "pl-tag")
          .Add("model.answer", model.answer)
          .Add("model.tag", model.tag)
          .OnException(e);
      }
    }
    public async void OnQuestion(string key, PrelanderTagModel model)
    {
      try
      {
        var tag = PrelandersCache.Instance.GetTag(this.Prelander.ID, model.tag);
        if (tag == null)
        {
          this.Socket.Send(key);
          return;
        }

        var answer = PrelandersCache.Instance.GetAnswer(this.Prelander.ID, model.tag, model.answer);
        if (answer == null)
        {
          this.Socket.Send(key);
          return;
        }

        if (this.TagManager.ContainsKey(model.tag))
        {
          this.TagManager[model.tag] = model.index;
          this.Socket.Action.UpdatePrelanderData(this.ActionPrelanderCache);
        }

        var interaction = new PrelanderTagActionInteractionDM(this.Socket.Database)
        {
          prelanderid = this.Prelander.ID,
          actionid = this.Socket.Action.Key,
          prelandertagid = tag.GetStringID(),
          answerid = answer.GetStringID()
        };
        interaction.InsertLater();

        this.Socket.Send(key);
        this.Socket.Database.TransactionalManager.RunAsync();
      }
      catch(Exception e)
      {
        this.Socket.Logging.StartLoggin()
          .Add("where", "pl-onQuestion")
          .Add("model.answer", model.answer)
          .Add("model.tag", model.tag)
          .OnException(e);
      }
    }


  }
}
