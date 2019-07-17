using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CCMonkeys.Wpf.Desktop.Commands;
using CCMonkeys.Wpf.Desktop.Models;

namespace CCMonkeys.Wpf.Desktop.ViewModels
{
    internal class CustomerViewModel : INotifyPropertyChanged
    {
        private CustomerModel customer;

        public CustomerViewModel()
        {
            UpdateCommand = new UpdateCustomerCommand(this);
        }

        public CustomerModel Customer
        {
            get
            {
                return Customer;
            }
        }

        public ICommand UpdateCommand
        {
            get;
            private set;
        }

        public void SaveChanges()
        {
            //Customer
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
