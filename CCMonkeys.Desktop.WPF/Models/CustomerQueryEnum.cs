using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMonkeys.Wpf.Desktop.Models
{
    public enum CustomerQueryEnum
    {
        AllUnsubscribed = 1,
        SubscribedOnlyToOneProvider = 2,
        SubscribedToProvider1UnsubscribedToProvider2 = 3
    }
}
