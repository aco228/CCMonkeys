using CCMonkeys.Direct;
using Direct.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CCMonkeys.Web.Core.Code
{
  public static class CountryManager
  {
    private static Dictionary<string, int> _countryNameMap = null;
    private static Dictionary<string, int> _countryCodeMap = null;

    private static async Task Configure(CCSubmitDirect db)
    {
      _countryNameMap = new Dictionary<string, int>();
      DirectContainer dc = await db.LoadContainerAsync(
        @"SELECT c.countryid, c.name, c.code FROM [].tm_country AS c
          LEFT OUTER JOIN [].tm_country_used AS u ON u.countryid=c.countryid
          WHERE u.countryusedid IS NOT NULL;");

      _countryNameMap = new Dictionary<string, int>();
      _countryCodeMap = new Dictionary<string, int>();
      foreach (var row in dc.Rows)
      {
        _countryNameMap.Add(row.GetString("name").ToLower(), row.GetInt("countryid").Value);
        _countryCodeMap.Add(row.GetString("code").ToLower(), row.GetInt("countryid").Value);
      }
    }


    public static async Task<int?> GetCountryByName(CCSubmitDirect db, string name)
    {
      if (string.IsNullOrEmpty(name))
        return null;

      if (_countryNameMap == null)
        await Configure(db);

      foreach (var entry in _countryNameMap)
        if (entry.Key.Contains(name.ToLower()))
          return entry.Value;

      DirectContainer dc = await db.LoadContainerAsync(string.Format("SELECT countryid, name, code FROM [].tm_country WHERE name LIKE '%{0}%';", name.ToLower()));
      if (!dc.HasValue)
        return null;

      _countryNameMap.Add(dc.GetString("name").ToLower(), dc.GetInt("countryid").Value);
      _countryCodeMap.Add(dc.GetString("code").ToLower(), dc.GetInt("countryid").Value);
      db.TransactionalManager.Add("INSERT INTO [].tm_country_used (countryid)", dc.GetInt("countryid").Value);

      return dc.GetInt("countryid").Value;
    }

    public static async Task<int?> GetCountryByCode(CCSubmitDirect db, string code)
    {
      if (string.IsNullOrEmpty(code))
        return null;

      if (_countryNameMap == null)
        await Configure(db);

      foreach (var entry in _countryCodeMap)
        if (entry.Key.Contains(code.ToLower()))
          return entry.Value;

      DirectContainer dc = await db.LoadContainerAsync(string.Format("SELECT countryid, name, code FROM [].tm_country WHERE code='{0}';", code.ToLower()));
      if (!dc.HasValue)
        return null;

      _countryNameMap.Add(dc.GetString("name").ToLower(), dc.GetInt("countryid").Value);
      _countryCodeMap.Add(dc.GetString("code").ToLower(), dc.GetInt("countryid").Value);
      db.TransactionalManager.Add("INSERT INTO [].tm_country_used (countryid)", dc.GetInt("countryid").Value);

      return dc.GetInt("countryid").Value;
    }



  }
}
