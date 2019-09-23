using Direct.Models;
using System;
using System.Threading.Tasks;

namespace Direct.ccmonkeys.Models
{
  public partial class ActionDM : DirectModel
  {

    public async void Trace(string where)
    {
      //await this.GetDatabase().ExecuteAsync("INSERT INTO [].tm_action_trace (actionid, wheree)", this.GetStringID(), where);
    }

  }
}
