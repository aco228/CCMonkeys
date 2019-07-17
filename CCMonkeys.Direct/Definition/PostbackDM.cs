using Direct.Core;
using Direct.Core.Models;
using System;

namespace Direct.ccmonkeys.Models
{
  public partial class PostbackDM : DirectModel
  {

    public async void Log(string text)
    {
      await new PostbackLogDM(this.GetDatabase())
      {
        postbackid = this.ID.Value,
        text = text
      }.InsertAsync<PostbackLogDM>();
    }

  }
}
