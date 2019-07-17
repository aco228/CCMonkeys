using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCMonkeys.Wpf.Desktop.Core.Csv.Models;

namespace CCMonkeys.Wpf.Desktop.Csv.Mapper
{
    public class DynamicCsvMap : ClassMap<DynamicCsv>
    {
        public DynamicCsvMap()
        {
            Map(m => m.Msisdn).Name("Msisdn");
            Map(m => m.Email).Name("Email");
            Map(m => m.FirstName).Name("FirstName");
            Map(m => m.LastName).Name("LastName");
            Map(m => m.Country).Name("Country");
            Map(m => m.Address).Name("Address");
            Map(m => m.City).Name("City");
            Map(m => m.Zip).Name("Zip");
            Map(m => m.Device).Name("Device");
            Map(m => m.Operator).Name("Operator");
            Map(m => m.DeviceMf).Name("DeviceMf");
            Map(m => m.DeviceOs).Name("DeviceOs");
            //Map(m => m.Updated).Ignore();
        }
    }
}
