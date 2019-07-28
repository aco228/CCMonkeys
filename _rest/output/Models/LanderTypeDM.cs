using Direct.Models;
using System;

namespace Direct.ccmonkeys.Models
{
public partial class LanderTypeDM : DirectModel
{

public LanderTypeDM() : base("tm_landertype", "landertypeid", null){}
public LanderTypeDM(DirectDatabaseBase db) : base("tm_landertype", "landertypeid", db){}

[DColumn(Name = "landertypeid", IsPrimary=true)]
public int landertypeid { get; set; } = default;

[DColumn(Name = "name")]
public string name { get; set; } = default;

[DColumn(Name = "description", Nullable = true)]
public string description { get; set; } = default;

[DColumn(Name = "created", NotUpdatable = true, HasDefaultValue=true)]
public DateTime created { get; set; } = default;

}
}
