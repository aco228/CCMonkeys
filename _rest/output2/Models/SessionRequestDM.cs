using Direct.Core;
using Direct.Core.Models;
using System;

namespace Direct.ccmonkeys.Models
{
public partial class SessionRequestDM : DirectModel
{

public SessionRequestDM(DirectDatabaseBase db) : base("tm_session_request", "sessionrequestid", db){}

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
