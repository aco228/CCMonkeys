using CCMonkeys.Direct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCMonkeys.Desktop.WPF.Core.Csv.Read.Models;
using Direct;

namespace CCMonkeys.Desktop.WPF.Direct
{
    public class DirectReader
    {
        public static List<DynamicCsv> QueryLeadsByMsisdnAndEmail(List<DynamicCsv> csvList)
        {
            var csvListToUpdate = new List<DynamicCsv>();

            var tableTypeString = "[].tm_lead";

            string msisdnParams;
            string emailParams;

            var result = CsvListToEMailAndMsisdnParams(csvList);
            msisdnParams = result.msisdns;
            emailParams = result.emails;

            //CCSubmitDirect db = CCSubmitDirect.Instance;
            CCSubmitConnectionString.Type = CCSubmitConnectionStringType.LocalDV;
            CCSubmitDirect db = CCSubmitDirect.Instance;

            var query = "SELECT * FROM " + tableTypeString + " ";

            if (msisdnParams != "" && emailParams != "")
            {
                query += "WHERE (msisdn in(" + msisdnParams + ") and (email = '' or email is null)) OR (email in(" + emailParams + ") and (msisdn = '' or msisdn is null))";
            }
            else if (msisdnParams != "" && emailParams == "")
            {
                query += "WHERE (msisdn in(" + msisdnParams + ") and (email = '' or email is null))";
            }
            else if (msisdnParams == "" && emailParams != "")
            {
                query += "WHERE (email in(" + emailParams + ") and (msisdn = '' or msisdn is null))";
            }
            else
                return new List<DynamicCsv>();

            DirectContainer dc = db.LoadContainer(query);

            if (dc.RowsCount > 0)
            {
                foreach (var row in dc.Rows)
                {
                    var csvItem = csvList.FirstOrDefault(record => record.Msisdn == row.GetString("msisdn") || record.Email == row.GetString("email"));

                    if (csvItem != null)
                        csvItem.SetLeadId((int)row.GetInt("leadid"));

                    csvListToUpdate.Add(csvList.FirstOrDefault());
                }
                return csvListToUpdate;
            }
            else
                return csvListToUpdate;
        }

        private static (string msisdns, string emails) CsvListToEMailAndMsisdnParams(List<DynamicCsv> csvList)
        {
            var msisdns = new List<string>();
            var emails = new List<string>();
            foreach (var record in csvList)
            {
                if (record.Msisdn != "")
                    msisdns.Add("'" + record.Msisdn + "'");
                if (record.Email != "")
                    emails.Add("'" + record.Email + "'");
            }

            return (String.Join(",", msisdns.ToArray()), String.Join(",", emails.ToArray()));

        }
    }
}
