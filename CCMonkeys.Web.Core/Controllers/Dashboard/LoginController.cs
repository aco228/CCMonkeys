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
using CCMonkeys.Web.Core.Code.Filters;

namespace CCMonkeys.Web.Core.Controllers.Dashboard
{
  [AllowCrossSiteAttribute]
  [Route("api/login")]
  [AllowCrossSiteAttribute]
  public class LoginController : MainController
  {
    public LoginController(IHostingEnvironment hostingEnvironment) : base(hostingEnvironment) { }

    public ModelBaseResponse Index()
    {
      int? adminID = this.Context.TryGetAdminID();
      if (adminID.HasValue)
        return new ModelBaseResponse() { Status = true, Message = this.Context.Admin.username };
      else
        return new ModelBaseResponse() { Status = false };
    }

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
