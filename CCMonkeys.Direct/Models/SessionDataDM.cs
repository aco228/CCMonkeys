using Direct.Models;
using System;

namespace Direct.ccmonkeys.Models
{
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

}
}
