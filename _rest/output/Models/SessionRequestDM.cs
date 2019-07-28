using Direct.Models;
using System;

namespace Direct.ccmonkeys.Models
{
public partial class SessionRequestDM : DirectModel
{

public SessionRequestDM() : base("tm_session_request", "sessionrequestid", null){}
public SessionRequestDM(DirectDatabaseBase db) : base("tm_session_request", "sessionrequestid", db){}

[DColumn(Name = "sessionrequestid", IsPrimary=true)]
public string sessionrequestid { get; set; } = default;

[DColumn(Name = "rawurl")]
public string rawurl { get; set; } = default;

[DColumn(Name = "ip")]
public string ip { get; set; } = default;

[DColumn(Name = "useragent")]
public string useragent { get; set; } = default;

[DColumn(Name = "created", NotUpdatable = true, HasDefaultValue=true)]
public DateTime created { get; set; } = default;

}
}
