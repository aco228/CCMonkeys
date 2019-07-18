﻿using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCMonkeys.Desktop.WPF.Core.Csv.Read.Models;

namespace CCMonkeys.Desktop.WPF.Core.Csv.Read.Mapper
{
    public class DynamicCsvMap : ClassMap<DynamicCsv>
    {
        public DynamicCsvMap()
        {
            Map(m => m.Msisdn).Name("Number");
            Map(m => m.FirstName).Name("Name");
            Map(m => m.Country).Name("Country");
            //Map(m => m.Clicker).Name("Clicker");
            //Map(m => m.Status).Name("Status");
            Map(m => m.Email).Name("Email");
            Map(m => m.Device).Name("Device");
            Map(m => m.Operator).Name("Operator");
            Map(m => m.LastName).Name("LastName");
            Map(m => m.City).Name("City");
            Map(m => m.DeviceMf).Name("DeviceMf");
            Map(m => m.DeviceOs).Name("DeviceOs");
            //Map(m => m.Updated).Ignore();
        }
    }
}
