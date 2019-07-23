using CCMonkeys.Web.Core.Code;
using CCMonkeys.Web.Core.Code.CacheManagers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CCMonkeys.Consoles.Migration
{
  public class ClientDM
  {
    public int clientid { get; set; }
    public string clickid { get; set; }
    public string affid { get; set; }
    public int payment_provider { get; set; }
    public string pubid { get; set; }
    public string email { get; set; }
    public string msisdn { get; set; }
    public string firstname { get; set; }
    public string lastname { get; set; }
    public string country { get; set; }
    public string referrer { get; set; }
    public string address { get; set; }
    public string username { get; set; }
    public string password { get; set; }
    public bool has_subscription { get; set; }
    public bool has_chargeback { get; set; }
    public bool has_refund { get; set; }
    public int times_charged { get; set; }
    public int times_upsell { get; set; }
    public bool is_stolen { get; set; }
    public DateTime created { get; set; }

    private int? countryID = null;

    public int? GetCountry()
    {
      if (countryID.HasValue)
        return countryID;
      //côte d'ivoire


      if(country.Length == 2)
        countryID = CountryCache.Instance.Get(Program.Database, country).Result;
      else
      {
        if (country.Equals("côte d'ivoire"))
          countryID = 53;
        else if (country.Equals("réunion"))
          countryID = 178;
        else
          countryID = CountryCache.Instance.Get(Program.Database, country).Result;
      }

      return countryID;
    }

  }
}
