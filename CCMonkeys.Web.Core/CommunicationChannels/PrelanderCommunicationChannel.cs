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
      if (key == "pl-init")
        return await this.OnInit(key, JsonConvert.DeserializeObject<PrelanderInitModel>(json));
      else if (key == "pl-tag")
        return await this.OnTag(key, JsonConvert.DeserializeObject<PrelanderTagModel>(json));
      else if (key == "pl-q")
        return await this.OnQuestion(key, JsonConvert.DeserializeObject<PrelanderTagModel>(json));
      return null;
    }

    public PreLanderCacheModel GetPrelander(int id)
    {
      if (this._session != null && ((PrelanderCommunication)this._session.Channels[SessionSocketChannel.Prelander]).Prelander != null)
        this.Prelander = ((PrelanderCommunication)this._session.Channels[SessionSocketChannel.Prelander]).Prelander;
      else
        this.Prelander = PrelandersCache.Instance.Get(id);

      return this.Prelander;
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

        foreach (var tag in model.tags)
          this.TagManager.Add(tag.name, null);

        this.Action.prelanderid = this.Prelander.ID;
        this.Action.prelandertypeid = this.Prelander.Type.ID;
        this.Action.prelander_data = this.ActionPrelanderCache;
        this.Action.UpdateLater();

        this.UpdateTagManager();
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
        GetPrelander(model.prelanderid);
        if (this.Prelander == null)
          return new DistributionModel() { Status = false };

        var tag = PrelandersCache.Instance.GetTag(this.Prelander.ID, model.tag);
        if (tag == null)
          return new DistributionModel() { Status = false };

        this.RecreateTagModel(this.Action.prelander_data);

        if (this.TagManager.ContainsKey(model.tag))
        {
          this.TagManager[model.tag] = 1;
          this.Action.prelander_data = this.ActionPrelanderCache;
          this.Action.UpdateLater();
        }

        var interaction = new PrelanderTagActionInteractionDM(this.Database)
        {
          prelanderid = this.Prelander.ID,
          actionid = this.Action.GetStringID(),
          prelandertagid = tag.GetStringID()
        };
        interaction.InsertLater();

        this.UpdateTagManager();
        this.Database.TransactionalManager.RunAsync();
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
        GetPrelander(model.prelanderid);
        if(this.Prelander == null)
          return new DistributionModel() { Status = false };

        var tag = PrelandersCache.Instance.GetTag(this.Prelander.ID, model.tag);
        if (tag == null)
          return new DistributionModel() { Status = false };

        var answer = PrelandersCache.Instance.GetAnswer(this.Prelander.ID, model.tag, model.answer);
        if (answer == null)
          return new DistributionModel() { Status = false };

        this.RecreateTagModel(this.Action.prelander_data);
        if (this.TagManager.ContainsKey(model.tag))
        {
          this.TagManager[model.tag] = model.index;
          this.Action.prelander_data = this.ActionPrelanderCache;
          this.Action.UpdateLater();
        }

        var interaction = new PrelanderTagActionInteractionDM(this.Database)
        {
          prelanderid = this.Prelander.ID,
          actionid = this.Action.GetStringID(),
          prelandertagid = tag.GetStringID(),
          answerid = answer.GetStringID()
        };
        interaction.InsertLater();

        this.UpdateTagManager();
        this.Database.TransactionalManager.RunAsync();
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
