using Direct.Models;
using System;

namespace Direct.ccmonkeys.Models
{
public partial class LeadDM : DirectModel
{

public LeadDM() : base("tm_lead", "leadid", null){}
public LeadDM(DirectDatabaseBase db) : base("tm_lead", "leadid", db){}

[DColumn(Name = "leadid", IsPrimary=true)]
public int leadid { get; set; } = default;

[DColumn(Name = "msisdn", Nullable = true)]
public string msisdn { get; set; } = default;

[DColumn(Name = "email", Nullable = true)]
public string email { get; set; } = default;

[DColumn(Name = "first_name", Nullable = true)]
public string first_name { get; set; } = default;

[DColumn(Name = "last_name", Nullable = true)]
public string last_name { get; set; } = default;

[DColumn(Name = "countryid", Nullable = true)]
public int? countryid { get; set; } = default;

[DColumn(Name = "address", Nullable = true)]
public string address { get; set; } = default;

[DColumn(Name = "city", Nullable = true)]
public string city { get; set; } = default;

[DColumn(Name = "zip", Nullable = true)]
public string zip { get; set; } = default;

[DColumn(Name = "device", Nullable = true)]
public string device { get; set; } = default;

[DColumn(Name = "mno", Nullable = true)]
public string mno { get; set; } = default;

[DColumn(Name = "actions_count", HasDefaultValue=true)]
public int actions_count { get; set; } = 0;

[DColumn(Name = "updated", NotUpdatable = true, HasDefaultValue=true)]
public DateTime updated { get; set; } = default;

[DColumn(Name = "created", NotUpdatable = true, HasDefaultValue=true)]
public DateTime created { get; set; } = default;

}
}
