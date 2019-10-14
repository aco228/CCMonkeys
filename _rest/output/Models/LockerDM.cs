using Direct.Models;
using System;

namespace Direct.ccmonkeys.Models
{
public partial class LockerDM : DirectModel
{

public LockerDM() : base("tm_locker", "lockerid", null){}
public LockerDM(DirectDatabaseBase db) : base("tm_locker", "lockerid", db){}

[DColumn(Name = "lockerid", IsPrimary=true)]
public string lockerid { get; set; } = default;

}
}
