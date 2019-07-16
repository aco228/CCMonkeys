using CCMonkeys.Direct;
using Direct.ccmonkeys.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CCMonkeys.Web.Core.Code.IP2ID
{
  public class IP2IDManager
  {

    public static IP2IDResult Get(string ipAddress, string userAgent)
    {
      string URL = "https://api.ip2id.com/api/lookup";
      var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
      httpWebRequest.ContentType = "application/json";
      httpWebRequest.Method = "POST";

      using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
      {
        string json =
          "{'ipAddress':'[ipAddress]','userAgent':'[userAgent]','login':{'application':'ef717de2-0f60-4955-a2bf-03daddad9f64','username':'ccprelander','password':'GPpxd5*XRj'}}"
          .Replace("[ipAddress]", ipAddress)
          .Replace("[userAgent]", userAgent);

        streamWriter.Write(json);
      }

      var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
      string result = "";
      using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        result = streamReader.ReadToEnd();


      return Desereliaze(result);
    }

    public static async Task<IP2IDResult> GetAsync(string ipAddress, string userAgent)
    {
      string URL = "https://api.ip2id.com/api/lookup";
      var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
      httpWebRequest.ContentType = "application/json";
      httpWebRequest.Method = "POST";

      using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
      {
        string json =
          "{'ipAddress':'[ipAddress]','userAgent':'[userAgent]','login':{'application':'ef717de2-0f60-4955-a2bf-03daddad9f64','username':'ccprelander','password':'GPpxd5*XRj'}}"
          .Replace("[ipAddress]", ipAddress)
          .Replace("[userAgent]", userAgent);

        streamWriter.Write(json);
      }

      var httpResponse = (HttpWebResponse)(await httpWebRequest.GetResponseAsync());
      string result = "";
      using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        result = streamReader.ReadToEnd();

      return Desereliaze(result);
    }

    private static IP2IDResult Desereliaze(string input)
    {
      IP2IDResult result = new IP2IDResult();
      var jObject = Newtonsoft.Json.Linq.JObject.Parse(input);

      result.countryCode = jObject["data"]["geolocationData"]["countryCode"].ToString();
      result.countryName = jObject["data"]["geolocationData"]["countryName"].ToString();
      result.IDDCode = jObject["data"]["geolocationData"]["IDDCode"].ToString();
      result.region = jObject["data"]["geolocationData"]["region"].ToString();
      result.city = jObject["data"]["geolocationData"]["city"].ToString();
      result.elevation = jObject["data"]["geolocationData"]["elevation"].ToString();
      result.zipCode = jObject["data"]["geolocationData"]["zipCode"].ToString();
      result.areaCode = jObject["data"]["geolocationData"]["areaCode"].ToString();
      result.timezone = jObject["data"]["geolocationData"]["timezone"].ToString();
      result.ISP = jObject["data"]["geolocationData"]["ISP"].ToString();
      result.domainName = jObject["data"]["geolocationData"]["domainName"].ToString();
      result.weatherStationCode = jObject["data"]["geolocationData"]["weatherStationCode"].ToString();
      result.weatherStationName = jObject["data"]["geolocationData"]["weatherStationName"].ToString();
      result.mobileBrand = jObject["data"]["geolocationData"]["mobileBrand"].ToString();
      result.MCC = jObject["data"]["geolocationData"]["MCC"].ToString();
      result.MNC = jObject["data"]["geolocationData"]["MNC"].ToString();
      result.ipVersion = jObject["data"]["geolocationData"]["ipVersion"].ToString();
      result.latitude = jObject["data"]["geolocationData"]["latitude"].ToString();
      result.longitude = jObject["data"]["geolocationData"]["longitude"].ToString();

      result.deviceType = jObject["data"]["deviceData"]["deviceType"].ToString();
      result.hardwareVendor = jObject["data"]["deviceData"]["hardwareVendor"].ToString();
      result.hardwareFamily = jObject["data"]["deviceData"]["hardwareFamily"].ToString();
      result.hardwareModel = jObject["data"]["deviceData"]["hardwareModel"].ToString();
      result.hardwareName = jObject["data"]["deviceData"]["hardwareName"].ToString();
      result.platformName = jObject["data"]["deviceData"]["platformName"].ToString();
      result.platformVersion = jObject["data"]["deviceData"]["platformVersion"].ToString();
      result.browserName = jObject["data"]["deviceData"]["browserName"].ToString();
      result.browserVersion = jObject["data"]["deviceData"]["browserVersion"].ToString();
      result.screenPixelsWidth = jObject["data"]["deviceData"]["screenPixelsWidth"].ToString();
      result.screenPixelsHeight = jObject["data"]["deviceData"]["screenPixelsHeight"].ToString();

      return result;

    }

    public static SessionDataDM GetSessionData(CCSubmitDirect database, string ip, string useragent)
    {
      return null;
      //var result = Get(ip, useragent);
      //return new SessionDataDM(database)
      //{
      //  countryCode = result.countryCode,
      //  countryName = result.countryName,
      //  IDDCode = result.IDDCode,
      //  region = result.region,
      //  city = result.city,
      //  elevation = result.elevation,
      //  zipCode = result.zipCode,
      //  areaCode = result.areaCode,
      //  timezone = result.timezone,
      //  ISP = result.ISP,
      //  domainName = result.domainName,
      //  weatherStationCode = result.weatherStationCode,
      //  weatherStationName = result.weatherStationName,
      //  mobileBrand = result.mobileBrand,
      //  MCC = result.MCC,
      //  MNC = result.MNC,
      //  ipVersion = result.ipVersion,
      //  latitude = result.latitude,
      //  longitude = result.longitude,
      //  deviceType = result.deviceType,
      //  hardwareVendor = result.hardwareVendor,
      //  hardwareFamily = result.hardwareFamily,
      //  hardwareModel = result.hardwareModel,
      //  hardwareName = result.hardwareName,
      //  platformName = result.platformName,
      //  platformVersion = result.platformVersion,
      //  browserName = result.browserName,
      //  browserVersion = result.browserVersion,
      //  screenPixelsWidth = result.screenPixelsWidth,
      //  screenPixelsHeight = result.screenPixelsHeight
      //};
    }

  }
}
