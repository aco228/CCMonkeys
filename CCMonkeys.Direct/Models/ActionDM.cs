using Direct.Core;
using Direct.Core.Models;
using System;

namespace Direct.ccmonkeys.Models
{
public partial class ActionDM : DirectModel
{

public ActionDM(DirectDatabaseBase db) : base("tm_action", "actionid", db){}

[DColumn(Name = "guid")]
public string guid { get; set; } = default;

[DColumn(Name = "trackingid", Nullable = true)]
public string trackingid { get; set; } = default;

[DColumn(Name = "userid")]
public int userid { get; set; } = default;

[DColumn(Name = "leadid", Nullable = true)]
public int? leadid { get; set; } = default;

[DColumn(Name = "affid", Nullable = true)]
public string affid { get; set; } = default;

[DColumn(Name = "pubid", Nullable = true)]
public string pubid { get; set; } = default;

[DColumn(Name = "prelandertypeid", Nullable = true)]
public int? prelandertypeid { get; set; } = default;

[DColumn(Name = "prelanderid", Nullable = true)]
public int? prelanderid { get; set; } = default;

[DColumn(Name = "landerid", Nullable = true)]
public int? landerid { get; set; } = default;

[DColumn(Name = "landertypeid", Nullable = true)]
public int? landertypeid { get; set; } = default;

[DColumn(Name = "providerid", Nullable = true)]
public int? providerid { get; set; } = default;

[DColumn(Name = "countryid", Nullable = true)]
public int? countryid { get; set; } = default;

[DColumn(Name = "input_redirect", HasDefaultValue=true)]
public bool input_redirect { get; set; } = false;

[DColumn(Name = "input_email", HasDefaultValue=true)]
public bool input_email { get; set; } = false;

[DColumn(Name = "input_contact", HasDefaultValue=true)]
public bool input_contact { get; set; } = false;

[DColumn(Name = "has_subscription", HasDefaultValue=true)]
public bool has_subscription { get; set; } = false;

[DColumn(Name = "has_chargeback", HasDefaultValue=true)]
public bool has_chargeback { get; set; } = false;

[DColumn(Name = "has_refund", HasDefaultValue=true)]
public bool has_refund { get; set; } = false;

[DColumn(Name = "times_charged", HasDefaultValue=true)]
public int times_charged { get; set; } = 0;

[DColumn(Name = "times_upsell", HasDefaultValue=true)]
public int times_upsell { get; set; } = 0;

[DColumn(Name = "has_redirectedToProvider", HasDefaultValue=true)]
public bool has_redirectedToProvider { get; set; } = false;

[DColumn(Name = "has_stolen", HasDefaultValue=true)]
public bool has_stolen { get; set; } = false;

[DColumn(Name = "updated", NotUpdatable = true, HasDefaultValue=true)]
public DateTime updated { get; set; } = default;

[DColumn(Name = "created", NotUpdatable = true, HasDefaultValue=true)]
public DateTime created { get; set; } = default;

}
}
