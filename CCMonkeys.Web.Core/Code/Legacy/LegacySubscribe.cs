using CCMonkeys.Direct;
using CCMonkeys.Web.Core.Models;
using Direct.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Web.Core.Code.Legacy
{
  public class LegacySubscribe : LegacyBase
  {
    private SubscribeUserModel _input = null;

    public LegacySubscribe(CCSubmitDirect db, HttpContext request, SubscribeUserModel input) : base(db, request)
    {
      this._input = input;
    }

    public override void Run()
    {
      this.Database.TransactionalManager.Add("INSERT INTO livesports.subscribers (firstname,lastname,country,zip,cc_number,cc_expiry_month,cc_expiry_year,ccv,time,gacid,lxid,url)",
        this._input.firstname,
        this._input.lastname,
        this._input.country,
        this._input.zip,
        this._input.cc_number,
        this._input.cc_expiry_month,
        this._input.cc_expiry_year,
        this._input.ccv,
        DirectTime.Now,
        this._input.gacid,
        this._input.lxid,
        this._input.url);

      this.Database.TransactionalManager.Add("UPDATE livesports.cc_client SET firstname={0}, msisdn={1}, lastname={2} WHERE clickid={3};",
        this._input.firstname,
        this._input.msisdn,
        this._input.lastname,
        this._input.lxid);
    }
  }
}
