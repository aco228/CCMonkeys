using Direct.Models;
using System;

namespace Direct.ccmonkeys.Models
{
public partial class CountryUsedDM : DirectModel
{

public CountryUsedDM() : base("tm_country_used", "countryusedid", null){}
public CountryUsedDM(DirectDatabaseBase db) : base("tm_country_used", "countryusedid", db){}

[DColumn(Name = "countryusedid", IsPrimary=true)]
public int countryusedid { get; set; } = default;

[DColumn(Name = "countryid")]
public int countryid { get; set; } = default;

}
}
