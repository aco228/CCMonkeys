using Direct.Models;
using System;

namespace Direct.ccmonkeys.Models
{
public partial class ActionDM : DirectModel
{

public ActionDM() : base("tm_action", "actionid", null){}
public ActionDM(DirectDatabaseBase db) : base("tm_action", "actionid", db){}

[DColumn(Name = "actionid", IsPrimary=true)]
public string actionid { get; set; } = default;

[DColumn(Name = "trackingid", Nullable = true)]
public string trackingid { get; set; } = default;

[DColumn(Name = "userid")]
public string userid { get; set; } = default;

[DColumn(Name = "leadid", Nullable = true)]
public int? leadid { get; set; } = default;

[DColumn(Name = "affid", Nullable = true)]
public string affid { get; set; } = default;

[DColumn(Name = "pubid", Nullable = true)]
public string pubid { get; set; } = default;

[DColumn(Name = "fbid", Nullable = true)]
public string fbid { get; set; } = default;

[DColumn(Name = "prelandertypeid", Nullable = true)]
public int? prelandertypeid { get; set; } = default;

[DColumn(Name = "prelanderid", Nullable = true)]
public int? prelanderid { get; set; } = default;

[DColumn(Name = "prelander_data", Nullable = true)]
public string prelander_data { get; set; } = default;

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

[DColumn(Name = "http_flow", HasDefaultValue=true)]
public bool http_flow { get; set; } = false;

[DColumn(Name = "updated", NotUpdatable = true, HasDefaultValue=true)]
public DateTime updated { get; set; } = default;

[DColumn(Name = "created", NotUpdatable = true, HasDefaultValue=true)]
public DateTime created { get; set; } = default;

public partial class ActionAccountDM : DirectModel
{

public ActionAccountDM() : base("tm_action_account", "actionaccount", null){}
public ActionAccountDM(DirectDatabaseBase db) : base("tm_action_account", "actionaccount", db){}

[DColumn(Name = "actionaccount", IsPrimary=true)]
public int actionaccount { get; set; } = default;

[DColumn(Name = "actionid")]
public string actionid { get; set; } = default;

[DColumn(Name = "username")]
public string username { get; set; } = default;

[DColumn(Name = "password")]
public string password { get; set; } = default;

[DColumn(Name = "created", NotUpdatable = true, HasDefaultValue=true)]
public DateTime created { get; set; } = default;

public partial class AdminDM : DirectModel
{

public AdminDM() : base("tm_admin", "adminid", null){}
public AdminDM(DirectDatabaseBase db) : base("tm_admin", "adminid", db){}

[DColumn(Name = "adminid", IsPrimary=true)]
public int adminid { get; set; } = default;

[DColumn(Name = "username")]
public string username { get; set; } = default;

[DColumn(Name = "password")]
public string password { get; set; } = default;

[DColumn(Name = "created", NotUpdatable = true, HasDefaultValue=true)]
public DateTime created { get; set; } = default;

public partial class AdminSessionDM : DirectModel
{

public AdminSessionDM() : base("tm_admin_session", "adminsessionid", null){}
public AdminSessionDM(DirectDatabaseBase db) : base("tm_admin_session", "adminsessionid", db){}

[DColumn(Name = "adminsessionid", IsPrimary=true)]
public int adminsessionid { get; set; } = default;

[DColumn(Name = "guid")]
public string guid { get; set; } = default;

[DColumn(Name = "adminid")]
public int adminid { get; set; } = default;

[DColumn(Name = "duration", HasDefaultValue=true)]
public double duration { get; set; } = default;

[DColumn(Name = "created", NotUpdatable = true, HasDefaultValue=true)]
public DateTime created { get; set; } = default;

public partial class CountryDM : DirectModel
{

public CountryDM() : base("tm_country", "countryid", null){}
public CountryDM(DirectDatabaseBase db) : base("tm_country", "countryid", db){}

[DColumn(Name = "countryid", IsPrimary=true)]
public int countryid { get; set; } = default;

[DColumn(Name = "name")]
public string name { get; set; } = default;

[DColumn(Name = "code")]
public string code { get; set; } = default;

public partial class CountryUsedDM : DirectModel
{

public CountryUsedDM() : base("tm_country_used", "countryusedid", null){}
public CountryUsedDM(DirectDatabaseBase db) : base("tm_country_used", "countryusedid", db){}

[DColumn(Name = "countryusedid", IsPrimary=true)]
public int countryusedid { get; set; } = default;

[DColumn(Name = "countryid")]
public int countryid { get; set; } = default;

}
}
