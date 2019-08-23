using Direct.Models;
using System;

namespace Direct.ccmonkeys.Models
{
public partial class LanderSubtypeDM : DirectModel
{

public LanderSubtypeDM() : base("tm_landersubtype", "id", null){}
public LanderSubtypeDM(DirectDatabaseBase db) : base("tm_landersubtype", "id", db){}

[DColumn(Name = "id", IsPrimary=true)]
public int id { get; set; } = default;

[DColumn(Name = "name")]
public string name { get; set; } = default;

[DColumn(Name = "description", Nullable = true)]
public string description { get; set; } = default;

[DColumn(Name = "created", NotUpdatable = true, HasDefaultValue=true)]
public DateTime created { get; set; } = default;

}
}
