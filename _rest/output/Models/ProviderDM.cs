using Direct.Models;
using System;

namespace Direct.ccmonkeys.Models
{
public partial class ProviderDM : DirectModel
{

public ProviderDM() : base("tm_provider", "providerid", null){}
public ProviderDM(DirectDatabaseBase db) : base("tm_provider", "providerid", db){}

[DColumn(Name = "providerid", IsPrimary=true)]
public int providerid { get; set; } = default;

[DColumn(Name = "key")]
public string key { get; set; } = default;

[DColumn(Name = "name")]
public string name { get; set; } = default;

[DColumn(Name = "price", Nullable = true, HasDefaultValue=true)]
public double? price { get; set; } = default;

[DColumn(Name = "description", Nullable = true)]
public string description { get; set; } = default;

}
}
