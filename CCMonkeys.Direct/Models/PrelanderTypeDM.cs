using Direct.Core;
using Direct.Core.Models;
using System;

namespace Direct.ccmonkeys.Models
{
public partial class PrelanderTypeDM : DirectModel
{

public PrelanderTypeDM(DirectDatabaseBase db) : base("tm_prelandertype", "prelandertypeid", db){}

[DColumn(Name = "name")]
public string name { get; set; } = default;

[DColumn(Name = "description", Nullable = true)]
public string description { get; set; } = default;

[DColumn(Name = "created", NotUpdatable = true)]
public DateTime created { get; set; } = default;

}
}
