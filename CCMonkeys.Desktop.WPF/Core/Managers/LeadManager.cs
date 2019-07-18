using System;
using System.Collections.Generic;
using System.Linq;
using CCMonkeys.Wpf.Desktop.Core.Csv.Models;
using CCMonkeys.Wpf.Desktop.Core.Manager;
using System.Reflection;
using Direct.Core;
using CCMonkeys.Direct;

namespace CCMonkeys.Wpf.Desktop.Managers
{
    public class LeadManager
    { 


        #region READ (IMPORTER)

        public static List<DynamicCsv> QueryLeadsByMsisdnAndEmail(List<DynamicCsv> csvList)
        {
            var csvListToUpdate = new List<DynamicCsv>();

            var tableName = "[].tm_lead";

            string msisdnParams;
            string emailParams;

            var result = CsvListToEMailAndMsisdnParams(csvList);
            msisdnParams = result.msisdns;
            emailParams = result.emails;

            //CCSubmitDirect db = CCSubmitDirect.Instance;
            CCSubmitConnectionString.Type = CCSubmitConnectionStringType.LocalDV;
            CCSubmitDirect db = CCSubmitDirect.Instance;

            var query = "SELECT * FROM " + tableName + " ";

            if (msisdnParams != "" && emailParams == "")
            {
                query += "WHERE msisdn in(" + msisdnParams + ") and (email = '' or email = null)";
            }
            else if (msisdnParams == "" && emailParams != "")
            {
                query += "WHERE email in(" + emailParams + ") and (msisdn = '' or msisdn = null)";
            }
            else
                return new List<DynamicCsv>();

            query += " LIMIT 1000;";

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

        #endregion

        #region WRITE (IMPORTER)

        public static (bool isSuccess, int numOfRows) InsertCsvData(List<DynamicCsv> csvParamsList)
        {
            //CCSubmitDirect db = CCSubmitDirect.Instance;
            CCSubmitConnectionString.Type = CCSubmitConnectionStringType.LocalDV;
            CCSubmitDirect db = CCSubmitDirect.Instance;

            string query = "";

            query += "INSERT INTO [].tm_lead (msisdn, email, first_name, last_name, country, address, city, zip, device, operator, device_mf, device_os, updated) VALUES"
                  + ConstructParamsCustom(csvParamsList);

            var result = db.Execute(query);
            var numOfRows = result.NumberOfRowsAffected;

            return (numOfRows > 0 ? true : false, numOfRows != null ? (int)numOfRows : 0);
        }

        private static string ConstructParamsCustom(List<DynamicCsv> csvParamsList)
        {
            var nlp = new List<string>();

            foreach (var item in csvParamsList)
            {

                List<object> newParamsList = new List<object>();

                foreach (PropertyInfo prop in item.GetType().GetProperties())
                {
                    if (prop.PropertyType == typeof(String))
                    {
                        newParamsList.Add("'" + prop.GetValue(item, null).ToString() + "'");
                    }
                    else if (prop.PropertyType == typeof(int))
                    {
                        newParamsList.Add(prop.GetValue(item, null).ToString());
                    }
                    else if (prop.PropertyType == typeof(DateTime))
                    {
                        newParamsList.Add("'" + DirectTime.Now + "'");
                    }
                }

                // DateTime.Now.ToString("yyyy-MM-dd H:mm:ss")

                newParamsList.Add("'" + DirectTime.Now + "'");


                nlp.Add("(" + String.Join(", ", newParamsList) + ")");

            }
            return String.Join(", ", nlp.ToArray());
        }

        // TODO
        public static (bool isSuccess, int numOfRows) UpdateCsvData(List<DynamicCsv> csvParamsList)
        {
            //CCSubmitDirect db = CCSubmitDirect.Instance;

            CCSubmitConnectionString.Type = CCSubmitConnectionStringType.LocalDV;
            CCSubmitDirect db = CCSubmitDirect.Instance;
            var dtManager = new DirectTransactionalManagerExtension(db);

            foreach (var record in csvParamsList)
            {
                if (record.Msisdn != "" || record.Email != "")
                {
                    string query = "UPDATE [].tm_lead SET " + record.Msisdn == "" ? " email='" + record.Email + "'" : " msisdn='" + record.Msisdn + "'" +
                        "WHERE lead_id = " + record.GetLeadId();

                    dtManager.Execute(query);
                }
            }

            var numOfRows = dtManager.Run();

            return (numOfRows > 0 ? true : false, numOfRows != null ? (int)numOfRows : 0);
        }

        #endregion

    }
}
