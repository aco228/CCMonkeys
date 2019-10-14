using Direct.Models;
using System;

namespace Direct.ccmonkeys.Models
{
public partial class PrelanderDomainDM : DirectModel
{

public PrelanderDomainDM() : base("tm_prelander_domain", "domainid", null){}
public PrelanderDomainDM(DirectDatabaseBase db) : base("tm_prelander_domain", "domainid", db){}

[DColumn(Name = "domainid", IsPrimary=true)]
public int domainid { get; set; } = default;

[DColumn(Name = "url")]
public string url { get; set; } = default;

[DColumn(Name = "description", Nullable = true)]
public string description { get; set; } = default;

[DColumn(Name = "updated", NotUpdatable = true, HasDefaultValue=true)]
public DateTime updated { get; set; } = default;

[DColumn(Name = "created", NotUpdatable = true, HasDefaultValue=true)]
public DateTime created { get; set; } = default;

}
}
