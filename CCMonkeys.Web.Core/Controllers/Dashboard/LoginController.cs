using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CCMonkeys.Web.Core.Code;
using CCMonkeys.Web.Core.Models.Dashboard;
using Direct.ccmonkeys.Models;
using Direct.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace CCMonkeys.Web.Core.Controllers.Dashboard
{
  [Route("api/login")]
  public class LoginController : MainController
  {
    public LoginController(IHostingEnvironment hostingEnvironment) : base(hostingEnvironment) { }

    [HttpGet("{username}/{password}")]
    public async Task<ModelBaseResponse> Index(string username, string password)
    {
      var admin = await this.Database.Query<AdminDM>().Where("username={0}", username).LoadSingleAsync();
      if (admin == null)
        return ModelBaseResponse.GenerateError($"Account with name '{username}' does not exists!");

      if(!admin.password.Equals(password))
        return ModelBaseResponse.GenerateError($"Wrong password");

      return new LoginModelResponse() { AccessToken = Crypter.Encrypt(this.Context.StoreAdminCookie(admin.ID.Value, admin.username)) };
    }

  }
}
