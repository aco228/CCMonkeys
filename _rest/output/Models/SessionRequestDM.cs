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

public partial class EmailBlacklistDM : DirectModel
{

public EmailBlacklistDM() : base("tm_email_blacklist", "blacklistid", null){}
public EmailBlacklistDM(DirectDatabaseBase db) : base("tm_email_blacklist", "blacklistid", db){}

[DColumn(Name = "blacklistid", IsPrimary=true)]
public int blacklistid { get; set; } = default;

[DColumn(Name = "email")]
public string email { get; set; } = default;

[DColumn(Name = "created", NotUpdatable = true, HasDefaultValue=true)]
public DateTime created { get; set; } = default;

public partial class LanderDM : DirectModel
{

public LanderDM() : base("tm_lander", "landerid", null){}
public LanderDM(DirectDatabaseBase db) : base("tm_lander", "landerid", db){}

[DColumn(Name = "landerid", IsPrimary=true)]
public int landerid { get; set; } = default;

[DColumn(Name = "landertypeid")]
public int landertypeid { get; set; } = default;

[DColumn(Name = "name")]
public string name { get; set; } = default;

[DColumn(Name = "url")]
public string url { get; set; } = default;

[DColumn(Name = "updated", NotUpdatable = true, HasDefaultValue=true)]
public DateTime updated { get; set; } = default;

[DColumn(Name = "created", NotUpdatable = true, HasDefaultValue=true)]
public DateTime created { get; set; } = default;

public partial class LanderTypeDM : DirectModel
{

public LanderTypeDM() : base("tm_landertype", "landertypeid", null){}
public LanderTypeDM(DirectDatabaseBase db) : base("tm_landertype", "landertypeid", db){}

[DColumn(Name = "landertypeid", IsPrimary=true)]
public int landertypeid { get; set; } = default;

[DColumn(Name = "name")]
public string name { get; set; } = default;

[DColumn(Name = "description", Nullable = true)]
public string description { get; set; } = default;

[DColumn(Name = "created", NotUpdatable = true, HasDefaultValue=true)]
public DateTime created { get; set; } = default;

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

public partial class PrelanderDM : DirectModel
{

public PrelanderDM() : base("tm_prelander", "prelanderid", null){}
public PrelanderDM(DirectDatabaseBase db) : base("tm_prelander", "prelanderid", db){}

[DColumn(Name = "prelanderid", IsPrimary=true)]
public int prelanderid { get; set; } = default;

[DColumn(Name = "prelandertypeid")]
public int prelandertypeid { get; set; } = default;

[DColumn(Name = "name")]
public string name { get; set; } = default;

[DColumn(Name = "url")]
public string url { get; set; } = default;

[DColumn(Name = "updated", NotUpdatable = true, HasDefaultValue=true)]
public DateTime updated { get; set; } = default;

[DColumn(Name = "created", NotUpdatable = true, HasDefaultValue=true)]
public DateTime created { get; set; } = default;

public partial class PrelanderTypeDM : DirectModel
{

public PrelanderTypeDM() : base("tm_prelandertype", "prelandertypeid", null){}
public PrelanderTypeDM(DirectDatabaseBase db) : base("tm_prelandertype", "prelandertypeid", db){}

[DColumn(Name = "prelandertypeid", IsPrimary=true)]
public int prelandertypeid { get; set; } = default;

[DColumn(Name = "name")]
public string name { get; set; } = default;

[DColumn(Name = "description", Nullable = true)]
public string description { get; set; } = default;

[DColumn(Name = "created", NotUpdatable = true, HasDefaultValue=true)]
public DateTime created { get; set; } = default;

public partial class ProviderDM : DirectModel
{

public ProviderDM() : base("tm_provider", "providerid", null){}
public ProviderDM(DirectDatabaseBase db) : base("tm_provider", "providerid", db){}

[DColumn(Name = "providerid", IsPrimary=true)]
public int providerid { get; set; } = default;

[DColumn(Name = "name")]
public string name { get; set; } = default;

[DColumn(Name = "price", Nullable = true, HasDefaultValue=true)]
public double? price { get; set; } = default;

public partial class SessionDM : DirectModel
{

public SessionDM() : base("tm_session", "sessionid", null){}
public SessionDM(DirectDatabaseBase db) : base("tm_session", "sessionid", db){}

[DColumn(Name = "sessionid", IsPrimary=true)]
public string sessionid { get; set; } = default;

[DColumn(Name = "sessiontype")]
public int sessiontype { get; set; } = default;

[DColumn(Name = "userid")]
public string userid { get; set; } = default;

[DColumn(Name = "actionid")]
public string actionid { get; set; } = default;

[DColumn(Name = "sessiondataid", Nullable = true)]
public string sessiondataid { get; set; } = default;

[DColumn(Name = "sessionrequestid", Nullable = true)]
public string sessionrequestid { get; set; } = default;

[DColumn(Name = "duration", HasDefaultValue=true)]
public double duration { get; set; } = default;

[DColumn(Name = "is_live", HasDefaultValue=true)]
public bool is_live { get; set; } = false;

[DColumn(Name = "created", NotUpdatable = true, HasDefaultValue=true)]
public DateTime created { get; set; } = default;

public partial class SessionDataDM : DirectModel
{

public SessionDataDM() : base("tm_session_data", "sessiondataid", null){}
public SessionDataDM(DirectDatabaseBase db) : base("tm_session_data", "sessiondataid", db){}

[DColumn(Name = "sessiondataid", IsPrimary=true)]
public string sessiondataid { get; set; } = default;

[DColumn(Name = "guid")]
public string guid { get; set; } = default;

[DColumn(Name = "countryCode", Nullable = true)]
public string countryCode { get; set; } = default;

[DColumn(Name = "countryName", Nullable = true)]
public string countryName { get; set; } = default;

[DColumn(Name = "region", Nullable = true)]
public string region { get; set; } = default;

[DColumn(Name = "city", Nullable = true)]
public string city { get; set; } = default;

[DColumn(Name = "zipCode", Nullable = true)]
public string zipCode { get; set; } = default;

[DColumn(Name = "ISP", Nullable = true)]
public string ISP { get; set; } = default;

[DColumn(Name = "latitude", Nullable = true)]
public string latitude { get; set; } = default;

[DColumn(Name = "longitude", Nullable = true)]
public string longitude { get; set; } = default;

[DColumn(Name = "timezone", Nullable = true)]
public string timezone { get; set; } = default;

[DColumn(Name = "created", NotUpdatable = true, HasDefaultValue=true)]
public DateTime created { get; set; } = default;

public partial class SessionRequestDM : DirectModel
{

public SessionRequestDM() : base("tm_session_request", "sessionrequestid", null){}
public SessionRequestDM(DirectDatabaseBase db) : base("tm_session_request", "sessionrequestid", db){}

[DColumn(Name = "sessionrequestid", IsPrimary=true)]
public string sessionrequestid { get; set; } = default;

[DColumn(Name = "rawurl")]
public string rawurl { get; set; } = default;

[DColumn(Name = "ip")]
public string ip { get; set; } = default;

[DColumn(Name = "useragent")]
public string useragent { get; set; } = default;

[DColumn(Name = "created", NotUpdatable = true, HasDefaultValue=true)]
public DateTime created { get; set; } = default;

}
}
