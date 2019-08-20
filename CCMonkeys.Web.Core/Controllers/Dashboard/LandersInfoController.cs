using CCMonkeys.Direct;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CCMonkeys.Web.Core.Controllers.Dashboard
{
  [Route("landerinfo")]
  public class LandersInfoController : ControllerBase
  {

    [HttpPost("set")]
    public async Task<IActionResult> Set()
    {
      using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
      {
        string data = await reader.ReadToEndAsync();
        await CCSubmitDirect.Instance.ExecuteAsync("UPDATE [].tm_landerinfo SET data={0}", data);
      }

      return this.Ok();
    }

    [HttpGet("get")]
    public async Task<dynamic> Get()
      => (await CCSubmitDirect.Instance.LoadAsync("SELECT data FROM [].tm_landerinfo WHERE id=1")).RawData;

  }
}
