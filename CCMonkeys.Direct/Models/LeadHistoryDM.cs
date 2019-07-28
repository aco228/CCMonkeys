using Direct.Models;
using System;

namespace Direct.ccmonkeys.Models
{
public partial class LeadHistoryDM : DirectModel
{

public LeadHistoryDM() : base("tm_lead_history", "historyID", null){}
public LeadHistoryDM(DirectDatabaseBase db) : base("tm_lead_history", "historyID", db){}

[DColumn(Name = "historyID", IsPrimary=true)]
public int historyID { get; set; } = default;

[DColumn(Name = "leadid")]
public int leadid { get; set; } = default;

[DColumn(Name = "name", Nullable = true)]
public string name { get; set; } = default;

[DColumn(Name = "old_value", Nullable = true)]
public string old_value { get; set; } = default;

[DColumn(Name = "new_value", Nullable = true)]
public string new_value { get; set; } = default;

[DColumn(Name = "created", NotUpdatable = true, HasDefaultValue=true)]
public DateTime created { get; set; } = default;

}
}
