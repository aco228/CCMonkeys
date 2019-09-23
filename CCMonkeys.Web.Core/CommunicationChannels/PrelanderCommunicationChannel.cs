using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CCMonkeys.Direct;
using CCMonkeys.Loggings;
using CCMonkeys.Sockets;
using CCMonkeys.Web.Core.Code.CacheManagers;
using CCMonkeys.Web.Core.Sockets.ApiSockets;
using CCMonkeys.Web.Core.Sockets.ApiSockets.Communication;
using CCMonkeys.Web.Core.Sockets.ApiSockets.Data;
using CCMonkeys.Web.Core.Sockets.Dashboard;
using Direct.ccmonkeys.Models;
using Newtonsoft.Json;

namespace CCMonkeys.Web.Core.CommunicationChannels
{
  public class PrelanderCommunicationChannel : CommunicationChannelBase
  {
    public PrelanderCommunicationChannel(SessionSocket session) 
      : base(session) { }
    public PrelanderCommunicationChannel(LoggingBase logger, ActionDM action, string user, int? countryID, CCSubmitDirect db) 
      : base(logger, action, user, countryID, db) { }

    private PreLanderCacheModel Prelander { get; set; } = null;
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

    public override async Task<DistributionModel> Call(string key, string json)
    {
      try
      {
        if (key == "pl-init")
          return await this.OnInit(key, JsonConvert.DeserializeObject<PrelanderInitModel>(json));
        else if (key == "pl-tag")
          return await this.OnTag(key, JsonConvert.DeserializeObject<PrelanderTagModel>(json));
        else if (key == "pl-q")
          return await this.OnQuestion(key, JsonConvert.DeserializeObject<PrelanderTagModel>(json));
      }
      catch(Exception e)
      {
        this.Logger.StartLoggin("prelander")
          .Add("where", "pl-call")
          .Add("key", key)
          .Add("json", json)
          .OnException(e);
        return new DistributionModel() { Status = false };
      }

      return null;
    }

    public PreLanderCacheModel GetPrelander(int? id)
    {
      try
      {
        if (this._session != null && ((PrelanderCommunication)this._session.Channels[SessionSocketChannel.Prelander]).Prelander != null)
          this.Prelander = ((PrelanderCommunication)this._session.Channels[SessionSocketChannel.Prelander]).Prelander;
        else if(id.HasValue)
          this.Prelander = PrelandersCache.Instance.Get(id.Value);

        return this.Prelander;
      }
      catch (Exception e)
      {
        this.Logger.StartLoggin("prelander")
          .Add("where", "GetPrelander")
          .Add("id", id.ToString())
          .OnException(e);

        return null;
      }
    }

    public async Task<DistributionModel> OnInit(string key, PrelanderInitModel model)
    {
      try
      {
        GetPrelander(model.prelanderid);
        if (this.Prelander == null)
        {
          this.Logger.StartLoggin(this.Action.GetStringID())
            .Where("pl-init")
            .Add("tagCount", model.tags != null ? model.tags.Count.ToString() : "null")
            .Add(model.tags)
            .OnException(new Exception("Could not get prealnder from id: " + model.prelanderid));
          return new DistributionModel() { Status = false };
        }

        if (this.Prelander.Answers == null || this.Prelander.Tags == null || this.Prelander.Tags.Count == 0 || this.Prelander.Answers.Count == 0)
        {
          foreach (var tag in model.tags)
          {
            var newTag = new PrelanderTagDM(this.Database)
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
                var newAnswer = new PrelanderTagAnswerDM(this.Database)
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

        this.Action.Trace("init.before tag manager");

        foreach (var tag in model.tags)
          this.TagManager.Add(tag.name, null);

        this.Action.Trace("init.after tag manager");

        this.Action.prelanderid = this.Prelander.ID;
        this.Action.prelandertypeid = this.Prelander.Type.ID;
        this.Action.prelander_data = this.ActionPrelanderCache;
        this.Action.UpdateLater();
        DashboardSocket.OnActionUpdate(this.Action);
        this.Action.Trace("init.after update");

        this.UpdateTagManager();
        this.Action.Trace("init.after update tag manager");
        this.Database.TransactionalManager.RunAsync();
        return new DistributionModel() { Status = true };

      }
      catch (Exception e)
      {
        this.Logger.StartLoggin("")
          .Add("where", "pl-init")
          .Add("tagCount", model.tags != null ? model.tags.Count.ToString() : "null")
          .Add(model.tags)
          .OnException(e);
        return new DistributionModel() { Status = false };
      }
    }
    public async Task<DistributionModel> OnTag(string key, PrelanderTagModel model)
    {
      try
      {
        this.Action.Trace("ontag. start");
        GetPrelander(model.prelanderid);
        if (this.Prelander == null)
          return new DistributionModel() { Status = false };

        this.Action.Trace("ontag. after prelander");

        var tag = PrelandersCache.Instance.GetTag(this.Prelander.ID, model.tag);
        if (tag == null)
          return new DistributionModel() { Status = false };

        this.Action.Trace("ontag. after tag load");

        this.RecreateTagModel(this.Action.prelander_data);

        this.Action.Trace("ontag. after recreate tag");

        if (this.TagManager.ContainsKey(model.tag))
        {
          this.TagManager[model.tag] = 1;
          this.Action.prelander_data = this.ActionPrelanderCache;
          this.Action.UpdateLater();
        }

        this.Action.Trace("ontag. after tagmanager");

        var interaction = new PrelanderTagActionInteractionDM(this.Database)
        {
          prelanderid = this.Prelander.ID,
          actionid = this.Action.GetStringID(),
          prelandertagid = tag.GetStringID()
        };
        interaction.InsertLater();

        this.Action.Trace("ontag. after interaction");

        this.UpdateTagManager();
        DashboardSocket.OnActionUpdate(this.Action);
        this.Database.TransactionalManager.RunAsync();
        this.Action.Trace("ontag. after updates");
        return new DistributionModel() { Status = true };
      }
      catch (Exception e)
      {
        this.Logger.StartLoggin("")
          .Add("where", "pl-tag")
          .Add("model.answer", model.answer)
          .Add("model.tag", model.tag)
          .OnException(e);
        return new DistributionModel() { Status = false };
      }
    }
    public async Task<DistributionModel> OnQuestion(string key, PrelanderTagModel model)
    {
      try
      {
        this.Action.Trace("q.start");
        GetPrelander(model.prelanderid);
        if(this.Prelander == null)
          return new DistributionModel() { Status = false };

        this.Action.Trace("q.after prelander");

        var tag = PrelandersCache.Instance.GetTag(this.Prelander.ID, model.tag);
        if (tag == null)
          return new DistributionModel() { Status = false };
        this.Action.Trace("q.after tag");

        var answer = PrelandersCache.Instance.GetAnswer(this.Prelander.ID, model.tag, model.answer);
        if (answer == null)
          return new DistributionModel() { Status = false };

        this.Action.Trace("q.after answer");

        this.RecreateTagModel(this.Action.prelander_data);
        if (this.TagManager.ContainsKey(model.tag))
        {
          this.TagManager[model.tag] = model.index;
          this.Action.prelander_data = this.ActionPrelanderCache;
          this.Action.UpdateLater();
        }

        this.Action.Trace("q.after recreate and update");

        var interaction = new PrelanderTagActionInteractionDM(this.Database)
        {
          prelanderid = this.Prelander.ID,
          actionid = this.Action.GetStringID(),
          prelandertagid = tag.GetStringID(),
          answerid = answer.GetStringID()
        };
        interaction.InsertLater();

        this.Action.Trace("q.after interaction");

        this.UpdateTagManager();
        this.Database.TransactionalManager.RunAsync();
        this.Action.Trace("q.after update");
        DashboardSocket.OnActionUpdate(this.Action);
        return new DistributionModel() { Status = true };
      }
      catch (Exception e)
      {
        this.Logger.StartLoggin("")
          .Add("where", "pl-onQuestion")
          .Add("model.answer", model.answer)
          .Add("model.tag", model.tag)
          .OnException(e);
        return new DistributionModel() { Status = false };
      }
    }

    private void RecreateTagModel(string input)
    {
      if (this._session != null)
      {
        this.TagManager = ((PrelanderCommunication)this._session.Channels[SessionSocketChannel.Prelander]).TagManager;
        return;
      }

      if (input != null || string.IsNullOrEmpty(input))
        return;

      string[] split = input.Split('.');
      foreach(string s in split)
      {
        if (string.IsNullOrEmpty(s))
          continue;
        string[] info = s.Split('=');
        if (info.Length != 2)
          continue;
        int refVal = 0;
        if (int.TryParse(info[1], out refVal))
          TagManager.Add(info[0], refVal);
        else
          TagManager.Add(info[0], null);
      }
    }
    private void UpdateTagManager()
    {
      if (this._session == null)
        return;
      ((PrelanderCommunication)this._session.Channels[SessionSocketChannel.Prelander]).TagManager = this.TagManager;
    }


  }
}
