using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CCMonkeys.Wpf.Desktop.Core.Csv.Mapper;
using CCMonkeys.Wpf.Desktop.Core.Csv.Models;
using CCMonkeys.Wpf.Desktop.Managers;

namespace CCMonkeys.Wpf.Desktop.Csv.Write
{
    public static class CsvWriterCustom
    {


        public static DynamicCsvInfo<DynamicCsv> WriteToDatabase(List<DynamicCsv> remainingRecords, int maxSize)
        {
            // separate (max 1000) records from allCsvs
            // search msisdn records from those
            // remove found ones
            // insert to db
            // remove those (max 1000) records from allCsvs
            // continue till all are inserted

            if(maxSize > 1000)
                maxSize = 1000;

            var csvInfo = new DynamicCsvInfo<DynamicCsv>();
            var recordsToInsert = new List<DynamicCsv>();
            var recordsToUpdate = new List<DynamicCsv>();

            var rangeToTake = remainingRecords.Count > maxSize ? maxSize : remainingRecords.Count;

            recordsToInsert = remainingRecords.Take(rangeToTake).ToList();
            remainingRecords.RemoveRange(0, rangeToTake);


            // check if msisdn or email exist, split list based on that and update those records
            recordsToUpdate = LeadManager.QueryLeadsByMsisdnAndEmail(recordsToInsert);

            if(recordsToUpdate.Count > 0)
            {
                // update records
                var updateResult = LeadManager.UpdateCsvData(recordsToUpdate);

                csvInfo.UpdatedRecordsCount += updateResult.numOfRows;

                recordsToInsert = recordsToInsert.Except(recordsToUpdate).ToList();
            }

            // insert other records
            var insertionResult = LeadManager.InsertCsvData(recordsToInsert);
            if (insertionResult.isSuccess)
            {
                csvInfo.NewlyInsertedRecordsCount += recordsToInsert.Count;
            }
            else
            {
                csvInfo.FailedToInsertRecordsCount += insertionResult.numOfRows == 0 ? insertionResult.numOfRows : recordsToInsert.Count;
            }

            csvInfo.RemainingRecords = remainingRecords;

            return csvInfo;
        }

        #region Write unsubscribed emails per provider

        public static bool WriteToCsvFile(List<DynamicCsv> records, string pathToFile)
        {
            try
            {
                using (var writer = new StreamWriter(pathToFile))
                using (var csv = new CsvWriter(writer))
                {
                    csv.Configuration.RegisterClassMap<DynamicCsvCClientMap>();
                    csv.WriteRecords(records);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion
    }
}
