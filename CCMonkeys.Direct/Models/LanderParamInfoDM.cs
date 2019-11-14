using Direct.Models;
using System;

namespace Direct.ccmonkeys.Models
{
public partial class LanderParamInfoDM : DirectModel
{

public LanderParamInfoDM() : base("tm_lander_param_info", "id", null){}
public LanderParamInfoDM(DirectDatabaseBase db) : base("tm_lander_param_info", "id", db){}

[DColumn(Name = "id", IsPrimary=true)]
public int id { get; set; } = default;

[DColumn(Name = "name")]
public string name { get; set; } = default;

[DColumn(Name = "value", Nullable = true)]
public string value { get; set; } = default;

[DColumn(Name = "description", Nullable = true, HasDefaultValue=true)]
public string description { get; set; } = "...";

}
}
