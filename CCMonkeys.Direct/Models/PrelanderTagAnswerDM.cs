using Direct.Models;
using System;

namespace Direct.ccmonkeys.Models
{
public partial class PrelanderTagAnswerDM : DirectModel
{

public PrelanderTagAnswerDM() : base("tm_prelander_tag_answer", "answerid", null){}
public PrelanderTagAnswerDM(DirectDatabaseBase db) : base("tm_prelander_tag_answer", "answerid", db){}

[DColumn(Name = "answerid", IsPrimary=true)]
public string answerid { get; set; } = default;

[DColumn(Name = "prelandertagid")]
public string prelandertagid { get; set; } = default;

[DColumn(Name = "prelanderid")]
public int prelanderid { get; set; } = default;

[DColumn(Name = "tagName")]
public string tagName { get; set; } = default;

[DColumn(Name = "name")]
public string name { get; set; } = default;

[DColumn(Name = "value", Nullable = true)]
public string value { get; set; } = default;

[DColumn(Name = "created", NotUpdatable = true, HasDefaultValue=true)]
public DateTime created { get; set; } = default;

}
}
