using Direct.Models;
using System;

namespace Direct.ccmonkeys.Models
{
public partial class PrelanderTagActionInteractionDM : DirectModel
{

public PrelanderTagActionInteractionDM() : base("tm_prelander_action_interaction", "actioninteractionid", null){}
public PrelanderTagActionInteractionDM(DirectDatabaseBase db) : base("tm_prelander_action_interaction", "actioninteractionid", db){}

[DColumn(Name = "actioninteractionid", IsPrimary=true)]
public int actioninteractionid { get; set; } = default;

[DColumn(Name = "prelanderid")]
public int prelanderid { get; set; } = default;

[DColumn(Name = "actionid")]
public string actionid { get; set; } = default;

[DColumn(Name = "prelandertagid")]
public string prelandertagid { get; set; } = default;

[DColumn(Name = "answerid", Nullable = true)]
public string answerid { get; set; } = default;

[DColumn(Name = "created", NotUpdatable = true, HasDefaultValue=true)]
public DateTime created { get; set; } = default;

}
}
