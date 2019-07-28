using Direct.Models;
using System;

namespace Direct.ccmonkeys.Models
{
public partial class SessionTypeDM : DirectModel
{

public SessionTypeDM() : base("tm_session_type", "sessiontypeid", null){}
public SessionTypeDM(DirectDatabaseBase db) : base("tm_session_type", "sessiontypeid", db){}

[DColumn(Name = "sessiontypeid", IsPrimary=true)]
public int sessiontypeid { get; set; } = default;

[DColumn(Name = "name")]
public string name { get; set; } = default;

}
}
