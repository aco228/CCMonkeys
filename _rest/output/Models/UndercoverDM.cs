using Direct.Models;
using System;

namespace Direct.ccmonkeys.Models
{
public partial class UndercoverDM : DirectModel
{

public UndercoverDM() : base("cc_undercover", "id", null){}
public UndercoverDM(DirectDatabaseBase db) : base("cc_undercover", "id", db){}

[DColumn(Name = "id", IsPrimary=true)]
public int id { get; set; } = default;

[DColumn(Name = "affid")]
public string affid { get; set; } = default;

[DColumn(Name = "pubid", Nullable = true)]
public string pubid { get; set; } = default;

[DColumn(Name = "value", HasDefaultValue=true)]
public int value { get; set; } = 0;

[DColumn(Name = "transactions", HasDefaultValue=true)]
public int transactions { get; set; } = 0;

[DColumn(Name = "tcost", Nullable = true)]
public double? tcost { get; set; } = default;

[DColumn(Name = "undercoverTransactions", HasDefaultValue=true)]
public int undercoverTransactions { get; set; } = 0;

[DColumn(Name = "currentDay", NotUpdatable = true, HasDefaultValue=true)]
public DateTime currentDay { get; set; } = default;

[DColumn(Name = "created", DateTimeUpdate = true, HasDefaultValue=true)]
public string created { get; set; } = "CURRENT_TIMESTAMP";

}
}
