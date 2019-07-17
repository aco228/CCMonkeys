using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CCMonkeys.Wpf.Desktop.Commands;
using CCMonkeys.Wpf.Desktop.Models;

namespace CCMonkeys.Wpf.Desktop.ViewModels
{
    internal class CustomerCsvExportViewModel : INotifyPropertyChanged
    {

        private BindableCollection<PaymentProviderModel> providers;

        private CustomerCsvExportModel customerCsvExport;

        public CustomerCsvExportViewModel()
        {
            providers = new BindableCollection<PaymentProviderModel>(GenerateProviderList());

            customerCsvExport = new CustomerCsvExportModel();

            SearchCustomersCommand = new SearchCustomersCommand(this);
        }

        public BindableCollection<PaymentProviderModel> Providers
        {
            get
            {
                return providers;
            }
        }

        public CustomerCsvExportModel CustomerCsvExport
        {
            get
            {
                return customerCsvExport;
            }
        }

        public ICommand SearchCustomersCommand
        {
            get;
            private set;
        }

        public void SaveChanges()
        {
            //Customer
        }

        #region Methods

        public List<PaymentProviderModel> GenerateProviderList()
        {
            var providersInfoList = new List<PaymentProviderModel>();

            var i = 0;

            foreach (var item in Enum.GetValues(typeof(PaymentProviderEnum)))
            {

                PaymentProviderEnum paymentProviderEnum = (PaymentProviderEnum)item;

                var provider = new PaymentProviderModel()
                {
                    ComboBoxIndex = i,
                    ProviderId = (int)item,
                    ProviderName = paymentProviderEnum.ToString()
                };

                providersInfoList.Add(provider);

                i++;
            }

            return providersInfoList;
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
