using Direct.Core;
using Direct.Core.Models;
using System;

namespace Direct.ccmonkeys.Models
{
public partial class PostbackDM : DirectModel
{

public PostbackDM(DirectDatabaseBase db) : base("cc_postback", "postbackid", db){}

[DColumn(Name = "providerid")]
public int providerid { get; set; } = default;

[DColumn(Name = "trackingid", Nullable = true)]
public string trackingid { get; set; } = default;

[DColumn(Name = "actionid", Nullable = true)]
public int? actionid { get; set; } = default;

[DColumn(Name = "url")]
public string url { get; set; } = default;

[DColumn(Name = "created", NotUpdatable = true, HasDefaultValue=true)]
public DateTime created { get; set; } = default;

}
}
