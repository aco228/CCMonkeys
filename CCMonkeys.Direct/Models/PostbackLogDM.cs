using Direct.Core;
using Direct.Core.Models;
using System;

namespace Direct.ccmonkeys.Models
{
public partial class PostbackLogDM : DirectModel
{

public PostbackLogDM(DirectDatabaseBase db) : base("cc_postback_log", "postbacklog", db){}

[DColumn(Name = "postbackid")]
public int postbackid { get; set; } = default;

[DColumn(Name = "text")]
public string text { get; set; } = default;

[DColumn(Name = "created", NotUpdatable = true, HasDefaultValue=true)]
public DateTime created { get; set; } = default;

}
}
