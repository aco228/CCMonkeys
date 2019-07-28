using Direct.Models;
using System;

namespace Direct.ccmonkeys.Models
{
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

}
}
