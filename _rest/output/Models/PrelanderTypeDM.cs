using Direct.Models;
using System;

namespace Direct.ccmonkeys.Models
{
public partial class PrelanderTypeDM : DirectModel
{

public PrelanderTypeDM() : base("tm_prelandertype", "prelandertypeid", null){}
public PrelanderTypeDM(DirectDatabaseBase db) : base("tm_prelandertype", "prelandertypeid", db){}

[DColumn(Name = "prelandertypeid", IsPrimary=true)]
public int prelandertypeid { get; set; } = default;

[DColumn(Name = "name")]
public string name { get; set; } = default;

[DColumn(Name = "description", Nullable = true)]
public string description { get; set; } = default;

[DColumn(Name = "created", NotUpdatable = true, HasDefaultValue=true)]
public DateTime created { get; set; } = default;

}
}
