using Direct.Core;
using Direct.Core.Models;
using System;

namespace Direct.ccmonkeys.Models
{
public partial class SessionDM : DirectModel
{

public SessionDM(DirectDatabaseBase db) : base("tm_session", "sessionid", db){}

[DColumn(Name = "guid")]
public string guid { get; set; } = default;

[DColumn(Name = "sessiontype")]
public int sessiontype { get; set; } = default;

[DColumn(Name = "userid")]
public int userid { get; set; } = default;

[DColumn(Name = "actionid")]
public int actionid { get; set; } = default;

[DColumn(Name = "sessiondataid", Nullable = true)]
public int? sessiondataid { get; set; } = default;

[DColumn(Name = "sessionrequestid", Nullable = true)]
public int? sessionrequestid { get; set; } = default;

[DColumn(Name = "duration", HasDefaultValue=true)]
public double duration { get; set; } = default;

[DColumn(Name = "created", NotUpdatable = true, HasDefaultValue=true)]
public DateTime created { get; set; } = default;

}
}
