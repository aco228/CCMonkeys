using Direct.Core;
using Direct.Core.Models;
using System;

namespace Direct.ccmonkeys.Models
{
public partial class CountryDM : DirectModel
{

public CountryDM(DirectDatabaseBase db) : base("tm_country", "countryid", db){}

[DColumn(Name = "name")]
public string name { get; set; } = default;

[DColumn(Name = "code")]
public string code { get; set; } = default;

}
}
