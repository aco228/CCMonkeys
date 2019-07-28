using Direct.Models;
using System;

namespace Direct.ccmonkeys.Models
{
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

}
}
