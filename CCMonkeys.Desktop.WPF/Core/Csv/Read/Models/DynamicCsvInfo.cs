using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficManagement.Wpf.Importer.Core.Csv.Read.Models
{
    public class DynamicCsvInfo<T>
    {
        public List<T> Records { get; set; }

        public List<T> RemainingRecords { get; set; }
        public bool IsHeadless { get; set; }
        public int AllRecordsCount { get; set; }
        public int CsvColumnCount { get; set; }


        public int UpdatedRecordsCount { get; set; }
        public int AlreadyInsertedRecordsCount { get; set; }
        public int NewlyInsertedRecordsCount { get; set; }
        public int FailedToInsertRecordsCount { get; set; }


    }
}
