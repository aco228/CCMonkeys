using Direct.Core;
using Direct.Core.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Direct.ccmonkeys.Models
{
  public partial class LeadDM : DirectModel
  {

    public Task<bool> HasLeadSubscriptions(int providerID)
      => this.GetDatabase().LoadBooleanAsync(
        "SELECT COUNT(*) FROM [].tm_action WHERE leadid={0} AND providerid={1} AND (times_charged>0 OR has_subscription=1)", 
        this.ID.Value, providerID);

    public void OnAction()
    {
      this.actions_count++;
      this.UpdateLater();
    }

   

    public static async Task<LeadDM> LoadByMsisdnOrEmailAsync(DirectDatabaseBase db, string msisdn, string email)
    {
      if (string.IsNullOrEmpty(msisdn) && string.IsNullOrEmpty(email))
        return null;

      LeadDM result = null;
      if (!string.IsNullOrEmpty(msisdn) && !string.IsNullOrEmpty(email))
        result = (await db.Query<LeadDM>().Select("email, msisdn").Where("msisdn={0} AND email={1}", msisdn, email).LoadAsync()).FirstOrDefault();
      else if (!string.IsNullOrEmpty(msisdn))
        result = (await db.Query<LeadDM>().Select("email, msisdn").Where("msisdn={0}", msisdn).LoadAsync()).FirstOrDefault();
      else if (!string.IsNullOrEmpty(email))
        result = (await db.Query<LeadDM>().Select("email, msisdn").Where("email={0}", email).LoadAsync()).FirstOrDefault();

      return result;
    }

    public static async Task<LeadDM> LoadAndInsertByMsisdnOrEmailAsync(DirectDatabaseBase db, string msisdn, string email)
    {
      var result = await LoadByMsisdnOrEmailAsync(db, msisdn, email);
      if (result != null)
        return result;

      result = new LeadDM(db)
      {
        msisdn = msisdn,
        email = email
      };
      await result.InsertAsync();

      return result;
    }

    public async void TryUpdateEmail(DirectDatabaseBase database, string input)
    {
      if (string.IsNullOrEmpty(input))
        return;

      if (!string.IsNullOrEmpty(this.email) && this.email.Equals(input))
        return;

      if (!string.IsNullOrEmpty(this.email))
      {
        var history = new LeadHistoryDM(database)
        {
          leadid = this.ID.Value,
          name = "email",
          old_value = this.email,
          new_value = input
        };
        await history.InsertAsync();
      }
      this.email = input;
    }
    public async void TryUpdateMsisdn(DirectDatabaseBase database, string input)
    {
      if (string.IsNullOrEmpty(input))
        return;

      if (!string.IsNullOrEmpty(this.msisdn) && this.msisdn.Equals(input))
        return;

      if (!string.IsNullOrEmpty(this.msisdn))
      {
        var history = new LeadHistoryDM(database)
        {
          leadid = this.ID.Value,
          name = "msisdn",
          old_value = this.msisdn,
          new_value = input
        };
        await history.InsertAsync();
      }
      this.msisdn = input;
    }
    public async void TryUpdateFirstName(DirectDatabaseBase database, string input)
    {
      if (string.IsNullOrEmpty(input))
        return;

      if (!string.IsNullOrEmpty(this.first_name) && this.first_name.Equals(input))
        return;

      if (!string.IsNullOrEmpty(this.first_name))
      {
        var history = new LeadHistoryDM(database)
        {
          leadid = this.ID.Value,
          name = "first_name",
          old_value = this.first_name,
          new_value = input
        };
        await history.InsertAsync();
      }
      this.first_name = input;
    }
    public async void TryUpdateLastName(DirectDatabaseBase database, string input)
    {
      if (string.IsNullOrEmpty(input))
        return;

      if (!string.IsNullOrEmpty(this.last_name) && this.last_name.Equals(input))
        return;

      if (!string.IsNullOrEmpty(this.last_name))
      {
        var history = new LeadHistoryDM(database)
        {
          leadid = this.ID.Value,
          name = "last_name",
          old_value = this.last_name,
          new_value = input
        };
        await history.InsertAsync();
      }
      this.last_name = input;
    }
    public async void TryUpdateZip(DirectDatabaseBase database, string input)
    {
      if (string.IsNullOrEmpty(input))
        return;

      if (!string.IsNullOrEmpty(this.zip) && this.zip.Equals(input))
        return;

      if (!string.IsNullOrEmpty(this.zip))
      {
        var history = new LeadHistoryDM(database)
        {
          leadid = this.ID.Value,
          name = "zip",
          old_value = this.zip,
          new_value = input
        };
        await history.InsertAsync();
      }
      this.zip = input;
    }
    public async void TryUpdateAddress(DirectDatabaseBase database, string input)
    {
      if (string.IsNullOrEmpty(input))
        return;

      if (!string.IsNullOrEmpty(this.address) && this.address.Equals(input))
        return;

      if (!string.IsNullOrEmpty(this.address))
      {
        var history = new LeadHistoryDM(database)
        {
          leadid = this.ID.Value,
          name = "address",
          old_value = this.address,
          new_value = input
        };
        await history.InsertAsync();
      }
      this.address = input;
    }
    public async void TryUpdateCity(DirectDatabaseBase database, string input)
    {
      if (string.IsNullOrEmpty(input))
        return;

      if (!string.IsNullOrEmpty(this.city) && this.city.Equals(input))
        return;

      if (!string.IsNullOrEmpty(this.city))
      {
        var history = new LeadHistoryDM(database)
        {
          leadid = this.ID.Value,
          name = "city",
          old_value = this.city,
          new_value = input
        };
        await history.InsertAsync();
      }
      this.city = input;
    }

  }
}
