﻿using CCMonkeys.Web.Core;
using CCMonkeys.Web.Core.Code.CacheManagers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCMonkeys.Web.Controllers
{
  [Route("restart")]
  public class RestartController : ControllerBase
  {

    [EnableCors("get")]
    [HttpGet("cache")]
    public IActionResult Index()
    {
      string result = CacheManager.Restart();
      return this.Ok(new { result = result });
    }


    [EnableCors("get")]
    [HttpGet("app")]
    public IActionResult RestartApplication()
    {
      Program.Restart();
      return this.Ok(new { result = true });
    }

    public IActionResult Status()
      => this.Ok(new
      {
        status = CacheManager.IsInitiated ? "true" : "false",
        paymentProviders = (from p in (CacheManager.Get(CacheType.Providers) as ProvidersCache).GetAll() select p.Name).ToArray(),
        prelanders = (from p in (CacheManager.Get(CacheType.Prelander) as PrelandersCache).GetAll() select p.Name).ToArray(),
        landers = (from p in (CacheManager.Get(CacheType.Lander) as LandersCache).GetAll() select p.Name).ToArray(),
        country = (from p in (CacheManager.Get(CacheType.Country) as CountryCache).GetAll() select p.Name).ToArray()
      });



  }
}
