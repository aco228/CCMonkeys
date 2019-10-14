using CCMonkeys.Direct;
using CCMonkeys.Web.Core.Code.CacheManagers.Core;
using Direct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMonkeys.Web.Core.Code.CacheManagers
{
  public class CountryCacheModel
  {
    public int ID;
    public string Code;
    public string Name;
  }

  public class CountryCache : CacheManagerBase
  {
    public static CountryCache Instance
    {
      get
      {
        if (!CacheManager.IsInitiated)
          CacheManager.Init();
        return (CountryCache)CacheManager.Get(CacheType.Country);
      }
    }


    private Dictionary<string, CountryCacheModel> _data = null;

    protected override async void Init()
    {
      CCSubmitDirect db = this.Database;
      _data = new Dictionary<string, CountryCacheModel>();
      DirectContainer dc = await db.LoadContainerAsync(
        @"SELECT c.countryid, c.name, c.code FROM [].tm_country AS c
          LEFT OUTER JOIN [].tm_country_used AS u ON u.countryid=c.countryid
          WHERE u.countryusedid IS NOT NULL;");

      foreach (var row in dc.Rows)
      {
        CountryCacheModel country = new CountryCacheModel()
        {
          ID = row.GetInt("countryid").Value,
          Name = row.GetString("name").ToLower(),
          Code = row.GetString("code").ToLower()
        };

        if(!_data.ContainsKey(country.Code))
          _data.Add(country.Code, country);

        if (!_data.ContainsKey(country.Name))
          _data.Add(country.Name, country);
      }
    }

    public async Task<int?> Get(CCSubmitDirect db, string input)
    {
      input = input.ToLower();
      if (string.IsNullOrEmpty(input))
        return null;

      if (_data.ContainsKey(input))
        return _data[input].ID;
      
      DirectContainer dc = await db.LoadContainerAsync(string.Format("SELECT countryid, name, code FROM [].tm_country WHERE name LIKE '%{0}%' OR code='{0}';", input));
      if (!dc.HasValue)
        return null;

      CountryCacheModel country = new CountryCacheModel()
      {
        ID = dc.GetInt("countryid").Value,
        Name = dc.GetString("name").ToLower(),
        Code = dc.GetString("code").ToLower()
      };

      if (!_data.ContainsKey(country.Code))
        _data.Add(country.Code, country);

      if (!_data.ContainsKey(country.Name))
        _data.Add(country.Name, country);

      db.TransactionalManager.Add("INSERT INTO [].tm_country_used (countryid)", dc.GetInt("countryid").Value);
      return dc.GetInt("countryid").Value;
    }


    public CountryCacheModel Get(int id) => (from c in _data where c.Value.ID == id select c.Value).FirstOrDefault();

    public List<CountryCacheModel> GetAll() => GetModel();
    public List<CountryCacheModel> GetModel()
    {
      List<CountryCacheModel> result = new List<CountryCacheModel>();
      foreach (var c in _data)
        if (!result.Contains(c.Value))
          result.Add(c.Value);
      return result;
    }
  }
}
