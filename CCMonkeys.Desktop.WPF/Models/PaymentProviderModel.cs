using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMonkeys.Wpf.Desktop.Models
{
    public class PaymentProviderModel : INotifyPropertyChanged
    {
        private int comboBoxIndex;
        private int providerId;
        private string providerName;

        public int ComboBoxIndex
        {
            get
            {
                return comboBoxIndex;
            }
            set
            {
                comboBoxIndex = value;
                OnPropertyChanged("ComboBoxIndex");
            }
        }

        public int ProviderId
        {
            get
            {
                return providerId;
            }
            set
            {
                providerId = value;
                OnPropertyChanged("ProviderId");
            }
        }

        public string ProviderName
        {
            get
            {
                return providerName;
            }
            set
            {
                providerName = value;
                OnPropertyChanged("ProviderName");
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
