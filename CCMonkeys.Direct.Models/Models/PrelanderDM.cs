using Direct.Core;
using Direct.Core.Models;
using System;

namespace Direct.ccmonkeys.Models
{
public partial class PrelanderDM : DirectModel
{

public PrelanderDM(DirectDatabaseBase db) : base("tm_prelander", "prelanderid", db){}

[DColumn(Name = "prelandertypeid")]
public int prelandertypeid { get; set; } = default;

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
