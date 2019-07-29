using Direct.Models;
using System;

namespace Direct.ccmonkeys.Models
{
public partial class PrelanderTagDM : DirectModel
{

public PrelanderTagDM() : base("tm_prelander_tag", "prelandertagid", null){}
public PrelanderTagDM(DirectDatabaseBase db) : base("tm_prelander_tag", "prelandertagid", db){}

[DColumn(Name = "prelandertagid", IsPrimary=true)]
public string prelandertagid { get; set; } = default;

[DColumn(Name = "prelanderid")]
public int prelanderid { get; set; } = default;

[DColumn(Name = "name")]
public string name { get; set; } = default;

[DColumn(Name = "value", Nullable = true)]
public string value { get; set; } = default;

[DColumn(Name = "isQuestion", HasDefaultValue=true)]
public bool isQuestion { get; set; } = false;

[DColumn(Name = "created", NotUpdatable = true, HasDefaultValue=true)]
public DateTime created { get; set; } = default;

}
}
