using Direct.Core;
using Direct.Core.Models;
using System;

namespace Direct.ccmonkeys.Models
{
public partial class ActionAccountDM : DirectModel
{

public ActionAccountDM(DirectDatabaseBase db) : base("tm_action_account", "actionaccount", db){}

[DColumn(Name = "actionid")]
public int actionid { get; set; } = default;

[DColumn(Name = "username")]
public string username { get; set; } = default;

[DColumn(Name = "password")]
public string password { get; set; } = default;

[DColumn(Name = "created", NotUpdatable = true, HasDefaultValue=true)]
public DateTime created { get; set; } = default;

}
}
