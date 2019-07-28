using Direct.Core;
using Direct.Core.Models;
using System;

namespace Direct.ccmonkeys.Models
{
public partial class ProviderDM : DirectModel
{

public ProviderDM(DirectDatabaseBase db) : base("tm_provider", "providerid", db){}

[DColumn(Name = "name")]
public string name { get; set; } = default;

[DColumn(Name = "price", Nullable = true, HasDefaultValue=true)]
public double? price { get; set; } = default;

}
}
