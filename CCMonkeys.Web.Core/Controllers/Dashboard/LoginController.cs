using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CCMonkeys.Web.Core.Code;
using CCMonkeys.Web.Core.Code.Filters;
using CCMonkeys.Web.Core.Models.Dashboard;
using Direct.ccmonkeys.Models;
using Direct;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

namespace CCMonkeys.Web.Core.Controllers.Dashboard
{
  [Route("api/login")]
  public class LoginController : MainController
  {
    public LoginController(IHostingEnvironment hostingEnvironment) : base(hostingEnvironment) { }

    // SUMMARY: Returns response if user is logged in or not (based on the cookies)
    [EnableCors("get")]
    public ModelBaseResponse Index()
    {
      if (this.Context.Admin != null)
        return new ModelBaseResponse() { Status = true, Message = this.Context.Admin.username };
      else
        return new ModelBaseResponse() { Status = false };
    }

    // SUMMARY: Try to login with username and password
    [EnableCors("get")]
    [HttpGet("{username}/{password}")]
    public async Task<ModelBaseResponse> Index(string username, string password)
    {
      var admin = await this.Database.Query<AdminDM>().Where("username={0}", username).LoadSingleAsync();
      if (admin == null)
        return ModelBaseResponse.GenerateError($"Account with name '{username}' does not exists!");

      password = Crypter.Encrypt(password);
      if(!admin.password.Equals(password))
        return ModelBaseResponse.GenerateError($"Wrong password");

      if(admin.GetStatus() == AdminStatusDM.NotActive)
        return ModelBaseResponse.GenerateError($"Activity error");

      return new LoginModelResponse() { AccessToken = Crypter.Encrypt(this.Context.StoreAdminCookie(admin.ID.Value, admin.username)) };
    }

    // SUMMARY: Changes password of the user
    // returns 404 if error
    // returns 'len' if password is more than 10 chars long
    // returns 'no' if there is no admin with id
    [HttpPatch("password/{id}")]
    public async Task<IActionResult> ChangePassword(string id)
    {
      try
      {
        string rawPostData = await this.Request.GetRawBodyStringAsync();
        if (string.IsNullOrEmpty(rawPostData))
          return this.NotFound("");

        if (rawPostData.Length > 10)
          return this.NotFound("len");

        AdminDM admin = await this.Database.Query<AdminDM>().Where("[id]={0}", id).LoadSingleAsync();
        if (admin == null)
          return this.NotFound("no");

        admin.password = Crypter.Encrypt(rawPostData);
        await admin.UpdateAsync();
        return this.Ok();
      }
      catch (Exception e)
      {
        return this.NotFound("exc");
      }
    }

    // SUMMARY: Change status of the user (privileges in database)
    // returns 404 if error
    // returns 'err1' if status cannot be converted to INT
    // returns 'err2' if status cannot be converted to AdminStatusDM
    // returns 'no' if there is no admin with id
    [HttpPatch("status/{id}")]
    public async Task<IActionResult> ChangeStatus(string id)
    {
      try
      {
        string rawPostData = await this.Request.GetRawBodyStringAsync();
        if (string.IsNullOrEmpty(rawPostData))
          return this.NotFound("");

        int statusID;
        if(!int.TryParse(rawPostData, out statusID))
          return this.NotFound("err1");

        if(!Enum.IsDefined(typeof(AdminStatusDM), statusID))
          return this.NotFound("err2");

        AdminDM admin = await this.Database.Query<AdminDM>().Where("[id]={0}", id).LoadSingleAsync();
        if (admin == null)
          return this.NotFound("no");

        admin.privileges = statusID;
        await admin.UpdateAsync();
        return this.Ok();
      }
      catch (Exception e)
      {
        return this.NotFound("exc");
      }
    }

  }
}
