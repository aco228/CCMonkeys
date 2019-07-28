using Direct.Models;
using System;

namespace Direct.ccmonkeys.Models
{
public partial class CountryDM : DirectModel
{

public CountryDM() : base("tm_country", "countryid", null){}
public CountryDM(DirectDatabaseBase db) : base("tm_country", "countryid", db){}

[DColumn(Name = "countryid", IsPrimary=true)]
public int countryid { get; set; } = default;

[DColumn(Name = "name")]
public string name { get; set; } = default;

[DColumn(Name = "code")]
public string code { get; set; } = default;

}
}
