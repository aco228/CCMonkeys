using Direct.Core;
using Direct.Core.Models;
using System;

namespace Direct.ccmonkeys.Models
{
public partial class UserDM : DirectModel
{

public UserDM(DirectDatabaseBase db) : base("tm_user", "userid", db){}

[DColumn(Name = "guid")]
public string guid { get; set; } = default;

[DColumn(Name = "countryid", Nullable = true)]
public int? countryid { get; set; } = default;

[DColumn(Name = "countryCode", Nullable = true)]
public string countryCode { get; set; } = default;

[DColumn(Name = "leadid", Nullable = true)]
public int? leadid { get; set; } = default;

[DColumn(Name = "actionid", Nullable = true)]
public int? actionid { get; set; } = default;

[DColumn(Name = "sessiondataid", Nullable = true)]
public int? sessiondataid { get; set; } = default;

[DColumn(Name = "created", NotUpdatable = true, HasDefaultValue=true)]
public DateTime created { get; set; } = default;

}
}
