using Direct.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using TrafficManagement.Core;
using TrafficManagement.Wpf.Importer.Core.Csv.Read.Models;
using TrafficManagement.Wpf.Importer.Core.Direct;

namespace TrafficManagement.Wpf.Importer.Direct
{
    public static class DirectWriter
    {
        public static (bool isSuccess, int numOfRows) InsertCsvData(List<DynamicCsv> csvParamsList)
        {
            //CCSubmitDirect db = CCSubmitDirect.Instance;
            ForTestDirect db = ForTestDirect.Instance;

            string query = "";

            query += "INSERT INTO [].tm_lead (msisdn, email, first_name, last_name, country, address, city, zip, device, operator, device_mf, device_os, updated) VALUES" 
                  + ConstructParamsCustom(csvParamsList);

            var numOfRows = db.Execute(query);

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

            ForTestDirect db = ForTestDirect.Instance;
            var dtManager = new DirectTransactionalManagerExtension(db);

            foreach (var record in csvParamsList)
            {
                if(record.Msisdn != "" || record.Email != "")
                {
                    string query = "UPDATE [].tm_lead SET " + record.Msisdn == "" ? " email='" + record.Email + "'" : " msisdn='" + record.Msisdn + "'" +
                        "WHERE lead_id = " + record.GetLeadId();

                    dtManager.Execute(query);
                }
            }

            var numOfRows = dtManager.Run();

            return (numOfRows > 0 ? true : false, numOfRows != null ? (int)numOfRows : 0);
        }



    }
}
