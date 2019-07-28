using Direct.Core;
using Direct.Core.Models;
using System;

namespace Direct.ccmonkeys.Models
{
public partial class AdminSessionDM : DirectModel
{

public AdminSessionDM(DirectDatabaseBase db) : base("tm_admin_session", "adminsessionid", db){}

[DColumn(Name = "guid")]
public string guid { get; set; } = default;

[DColumn(Name = "adminid")]
public int adminid { get; set; } = default;

[DColumn(Name = "duration", HasDefaultValue=true)]
public double duration { get; set; } = default;

[DColumn(Name = "created", NotUpdatable = true, HasDefaultValue=true)]
public DateTime created { get; set; } = default;

}
}
