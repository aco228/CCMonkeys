using CCMonkeys.Direct;
using Direct.ccmonkeys.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CCMonkeys.Web.Core.Code.IPLocations.IpApi
{
  public class IPAPI
  {
    public static IPAPIResult Get(string ipAddress, string userAgent)
    {
      string URL = "http://ip-api.com/json/" + ipAddress;
      var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
      httpWebRequest.ContentType = "application/json";
      httpWebRequest.Method = "GET";


      var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
      string result = "";
      using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        result = streamReader.ReadToEnd();


      return Desereliaze(result);
    }


    public static async Task<IPAPIResult> GetAsync(string ipAddress, string userAgent)
    {
      string URL = "http://ip-api.com/json/" + ipAddress;
      var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
      httpWebRequest.ContentType = "application/json";
      httpWebRequest.Method = "GET";


      var httpResponse = (HttpWebResponse)(await httpWebRequest.GetResponseAsync());
      string result = "";
      using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        result = streamReader.ReadToEnd();

      return Desereliaze(result);
    }

    public static IPAPIResult Desereliaze(string input)
    {
      IPAPIResult result = new IPAPIResult();
      var jObject = Newtonsoft.Json.Linq.JObject.Parse(input);

      result.AS = jObject["as"].ToString();
      result.city = jObject["city"].ToString();
      result.country = jObject["country"].ToString();
      result.countryCode = jObject["countryCode"].ToString();
      result.isp = jObject["isp"].ToString();
      result.lat = jObject["lat"].ToString();
      result.lon = jObject["lon"].ToString();
      result.org = jObject["org"].ToString();
      result.query = jObject["query"].ToString();
      result.region = jObject["region"].ToString();
      result.regionName = jObject["regionName"].ToString();
      result.status = jObject["status"].ToString();
      result.timezone = jObject["timezone"].ToString();
      result.zip = jObject["zip"].ToString();

      return result;
    }

    public static SessionDataDM GetSessionData(CCSubmitDirect database, string ip, string useragent)
    {
      var result = Get(ip, useragent);
      return new SessionDataDM(database)
      {
        countryCode = result.countryCode,
        countryName = result.country,
        region = result.region,
        city = result.city,
        ISP = result.AS,
        latitude = result.lat,
        longitude = result.lon,
        timezone = result.timezone,
        zipCode = result.zip
      };
    }


    public async static Task<SessionDataDM> GetSessionDataAsync(CCSubmitDirect database, string ip, string useragent)
    {
      var result = await GetAsync(ip, useragent);
      return new SessionDataDM(database)
      {
        countryCode = result.countryCode,
        countryName = result.country,
        region = result.region,
        city = result.city,
        ISP = result.AS,
        latitude = result.lat,
        longitude = result.lon,
        timezone = result.timezone,
        zipCode = result.zip
      };
    }

  }
}
