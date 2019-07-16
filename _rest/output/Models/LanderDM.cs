using Direct.Core;
using Direct.Core.Models;
using System;

namespace Direct.ccmonkeys.Models
{
public partial class LanderDM : DirectModel
{

public LanderDM(DirectDatabaseBase db) : base("tm_lander", "landerid", db){}

[DColumn(Name = "landertypeid")]
public int landertypeid { get; set; } = default;

[DColumn(Name = "name")]
public string name { get; set; } = default;

[DColumn(Name = "url")]
public string url { get; set; } = default;

[DColumn(Name = "updated", NotUpdatable = true)]
public DateTime updated { get; set; } = default;

[DColumn(Name = "created", NotUpdatable = true)]
public DateTime created { get; set; } = default;

}
}
