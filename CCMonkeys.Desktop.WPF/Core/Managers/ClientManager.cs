using Direct;
using System.Collections.Generic;
using CCMonkeys.Wpf.Desktop.Core.Csv.Models;
using CCMonkeys.Wpf.Desktop.Models;
using CCMonkeys.Direct;

namespace CCMonkeys.Wpf.Desktop.Core.Managers
{

  public class ClientManager
    {
        #region READ (EMAIL CSV GENERATOR)

        public static List<DynamicCsv> QueryUnsubscribedUsersByEmail(PaymentProviderEnum subscribed, PaymentProviderEnum unsubscribed)
        {
            var emailList = new List<DynamicCsv>();

            var tableName = "[].cc_client";

            CCSubmitConnectionString.Type = CCSubmitConnectionStringType.LocalDV;
            CCSubmitDirect db = CCSubmitDirect.Instance;
            //ForTestDirect db = ForTestDirect.Instance;

            var query = "SELECT * FROM " + tableName +
                        " WHERE payment_provider == " + (int)unsubscribed +
                        " AND times_charged = 0 " +
                        " AND email IN ( SELECT email FROM " + tableName +
                        " WHERE payment_provider = " + (int)subscribed +
                        " AND times_charged > 0 " +
                        " AND email != '' " +
                        " AND email IS NOT NULL);";

            DirectContainer dc = db.LoadContainer(query);

            if (dc.RowsCount > 0)
            {
                foreach (var row in dc.Rows)
                {
                    var dynamicCsv = new DynamicCsv()
                    {
                        Email = row.GetString("email"),
                        FirstName = row.GetString("firstname"),
                        LastName = row.GetString("lastname"),
                        Country = row.GetString("country"),
                        Msisdn = row.GetString("msisdn"),
                        Address = row.GetString("address"),
                        City = row.GetString("city"),
                        Zip = row.GetString("zip")
                    };

                    emailList.Add(dynamicCsv);
                }
                return emailList;
            }
            else
                return emailList;
        }

        #endregion

        #region WRITE 

        #endregion
    }
}
