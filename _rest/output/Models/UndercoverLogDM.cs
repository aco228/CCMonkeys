using Direct.Core;
using Direct.Core.Models;
using System;

namespace Direct.ccmonkeys.Models
{
public partial class UndercoverLogDM : DirectModel
{

public UndercoverLogDM(DirectDatabaseBase db) : base("cc_undercover_log", "undercoverlogid", db){}

[DColumn(Name = "text")]
public string text { get; set; } = default;

[DColumn(Name = "actionid")]
public int actionid { get; set; } = default;

[DColumn(Name = "created", NotUpdatable = true)]
public DateTime created { get; set; } = default;

}
}
