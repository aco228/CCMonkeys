﻿using CCMonkeys.Direct;
using CCMonkeys.Web.Core;
using CCMonkeys.Web.Core.Code;
using CCMonkeys.Web.Core.Code.CacheManagers;
using CCMonkeys.Web.Core.Code.IP2ID;
using CCMonkeys.Web.Core.Code.IPLocations.IpApi;
using CCMonkeys.Web.Core.Sockets.ApiSockets.Data;
using Direct.ccmonkeys.Models;
using Direct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCMonkeys.Web.Core.Sockets.ApiSockets.Models
{
  public enum SessionType { Default = 1, Prelander = 2, Lander = 3 };

  public class Session
  {
    public int? ID { get => this.Data.ID; }
    public CCSubmitDirect Database { get => this.Socket.Database; }
    public SessionSocket Socket { get; }
    public SessionType Type { get => Socket.SessionType; }
    public SessionDM Data { get; protected set; } = null;

    public SessionRequestDM Request { get; set; } = null;

    public int? CountryID { get; set; } = null;
    public string SessionDataGuid { get; set; } = null;
    public string CountryCode { get; protected set; }
    public SessionDataDM SessionData { get; protected set; } = null;

    public string Key { get; set; } = "no_key";

    public Session(SessionSocket socket)
    {
      this.Socket = socket;
      this.Data = new SessionDM(this.Database);
      this.Key = this.Data.GetStringID();
      this.PrepareRequest();
      this.PrepareSessionData();
    }

    public void Init()
    {
      this.Data.userid = Socket.User.Key;
      this.Data.actionid = Socket.Action.Data.GetStringID();
      this.Data.is_live = true;
      this.Data.sessionrequestid = this.Request.GetStringID();
      this.Data.sessiondataid = this.SessionDataGuid;
      this.Data.sessiontype = (int)this.Socket.SessionType;

      this.Data.InsertLater();
      Socket.User.SetCountry(this.CountryID, this.CountryCode);
    }

    private void PrepareRequest()
    {
      string ip = this.Socket.MainContext.HttpContext.Connection.RemoteIpAddress.ToString();
      if (ip.Equals("::1")) ip = "79.140.149.187";
      
      string url = string.Empty;
      if (this.Socket.MainContext.HttpContext.Request.Query.ContainsKey("url"))
        url = System.Web.HttpUtility.UrlDecode(this.Socket.MainContext.HttpContext.Request.Query["url"].ToString());

      this.Request = new SessionRequestDM(this.Database)
      {
        rawurl = url,
        ip = ip,
        useragent = this.Socket.MainContext.HttpContext.Request.Headers["User-Agent"]
      };
      this.Request.InsertLater();
    }

    private void PrepareSessionData()
    {
      this.CountryCode = Socket.MainContext.CookiesGet(Constants.CountryCode);
      this.CountryID = Socket.MainContext.CookiesGetInt(Constants.CountryID);
      this.SessionDataGuid = Socket.MainContext.CookiesGet(Constants.SessionDataID);

      if (!string.IsNullOrEmpty(this.SessionDataGuid) || string.IsNullOrEmpty(this.CountryCode) || !this.CountryID.HasValue)
      {
        this.SessionData = IPAPI.GetSessionData(this.Database, this.Request.ip, this.Request.useragent);
        if(this.SessionData == null)
        {
          // TODO: big problem!! very big
          throw new Exception("We could not get session data.. probably due to IP lookup");
        }
        this.SessionData.InsertLater();

        this.SessionDataGuid = this.SessionData.GetStringID();
        this.CountryCode = this.SessionData.countryCode;
        this.CountryID = CountryCache.Instance.Get(this.Database, this.CountryCode).Result;

        Socket.MainContext.SetCookie(Constants.SessionDataID, this.SessionDataGuid);
        Socket.MainContext.SetCookie(Constants.CountryCode, this.CountryCode);
        Socket.MainContext.SetCookie(Constants.CountryID, this.CountryID.Value.ToString());
      }
    }

    ///
    /// Socket event logic
    ///

    public async Task PrelanderRegistrationLogic(string domain, Dictionary<string, string> queryValues, ReceivingRegistrationModel model)
    {
      string msisdn = string.Empty, email = string.Empty;
      if (queryValues.ContainsKey("msisdn"))
        msisdn = queryValues["msisdn"];
      if (queryValues.ContainsKey("email"))
        email = queryValues["email"];

      if (!string.IsNullOrEmpty(msisdn) || !string.IsNullOrEmpty(email))
        await this.Socket.TryToIdentifyLead(msisdn, email);
    }

    public async void OnCreate()
    {
      //this.Data.initiated = true;
      //this.Data.is_live = true;
      //await this.Data.UpdateAsync();
    }

    public async void OnClose(DateTime created)
    {
      if (this.Data == null) return;
      this.Data.duration = (DateTime.Now - created).TotalSeconds;
      this.Data.is_live = false;
      await this.Data.UpdateAsync();
    }

  }
}
