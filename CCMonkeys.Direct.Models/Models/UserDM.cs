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

[DColumn(Name = "leadid", Nullable = true)]
public int? leadid { get; set; } = default;

[DColumn(Name = "created", NotUpdatable = true)]
public DateTime created { get; set; } = default;

}
}
