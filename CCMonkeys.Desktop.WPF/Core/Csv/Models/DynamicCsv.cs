using CsvHelper.Configuration.Attributes;
using System;

namespace CCMonkeys.Wpf.Desktop.Core.Csv.Models
{
    public class DynamicCsv
    {
        private int _LeadId;
        public string Msisdn { get; set; } = "";

        public string Email { get; set; } = "";

        public string FirstName { get; set; } = "";

        public string LastName { get; set; } = "";

        public string Country { get; set; } = "";

        public string Address { get; set; } = "";

        public string City { get; set; } = "";

        public string Zip { get; set; } = "";

        public string Device { get; set; } = "";

        public string Operator { get; set; } = "";

        public string DeviceMf { get; set; } = "";

        public string DeviceOs { get; set; } = "";
 
        //[Index(14)]
        //public DateTime Updated { get; set; }

        public int GetLeadId()
        {
            return this._LeadId;
        }

        public void SetLeadId(int leadId)
        {
            this._LeadId = leadId;
        }

    }
}
