using Direct.Models;
using System;

namespace Direct.ccmonkeys.Models
{
public partial class SessionDM : DirectModel
{

public SessionDM() : base("tm_session", "sessionid", null){}
public SessionDM(DirectDatabaseBase db) : base("tm_session", "sessionid", db){}

[DColumn(Name = "sessionid", IsPrimary=true)]
public string sessionid { get; set; } = default;

[DColumn(Name = "sessiontype")]
public int sessiontype { get; set; } = default;

[DColumn(Name = "userid")]
public string userid { get; set; } = default;

[DColumn(Name = "actionid")]
public string actionid { get; set; } = default;

[DColumn(Name = "sessiondataid", Nullable = true)]
public string sessiondataid { get; set; } = default;

[DColumn(Name = "sessionrequestid", Nullable = true)]
public string sessionrequestid { get; set; } = default;

[DColumn(Name = "duration", HasDefaultValue=true)]
public double duration { get; set; } = default;

[DColumn(Name = "is_live", HasDefaultValue=true)]
public bool is_live { get; set; } = false;

[DColumn(Name = "created", NotUpdatable = true, HasDefaultValue=true)]
public DateTime created { get; set; } = default;

}
}
