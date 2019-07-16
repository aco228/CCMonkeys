using CCMonkeys.Direct;
using CCMonkeys.Web.Core.Models;
using Direct.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Web.Core.Code.Legacy
{
  public class LegacyCreateUser : LegacyBase
  {
    private CreateUserModel _input = null;

    public LegacyCreateUser(CCSubmitDirect db, HttpContext request, CreateUserModel input) : base(db, request)
    {
      this._input = input;
    }

    public override void Run()
    {
      if (this.Database.LoadInt("SELECT COUNT(*) as 'br' FROM livesports.cc_client where clickid={0};", this._input.lxid).Value == 0)
      {
        this.Database.TransactionalManager.Add("INSERT INTO [].cc_client (clickid, affid, payment_provider, pubid, email, password, status, country, referrer, host, updated, created)",
          this._input.lxid,
          this._input.affid,
          this._input.pp,
          this._input.pubid,
          this._input.email,
          this._input.password,
          "created",
          this._input.country,
          this._input.referrer,
          this._input.host,
          DirectTime.Now, DirectTime.Now);
      }
    }
  }
}
