using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMonkeys.Wpf.Desktop.Models
{
    public class CustomerCsvExportModel : INotifyPropertyChanged
    {

        private CustomerQueryEnum queryType;
        private PaymentProviderEnum providerOne;
        private PaymentProviderEnum providerTwo;

        public CustomerCsvExportModel()
        {
            QueryType = CustomerQueryEnum.AllUnsubscribed;
        }

        public CustomerQueryEnum QueryType
        {
            get
            {
                return queryType;
            }
            set
            {
                queryType = value;
                OnPropertyChanged("QueryType");
            }
        }

        public PaymentProviderEnum ProviderOne
        {
            get
            {
                return providerOne;
            }
            set
            {
                providerOne = value;
                OnPropertyChanged("ProviderOne");
            }
        }

        public PaymentProviderEnum ProviderTwo
        {
            get
            {
                return providerTwo;
            }
            set
            {
                providerTwo = value;
                OnPropertyChanged("ProviderTwo");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
