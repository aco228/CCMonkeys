using System;
using System.ComponentModel;

namespace CCMonkeys.Wpf.Desktop.Models
{
    class CustomerModel : INotifyPropertyChanged, IDataErrorInfo
    {

        private string firstname;


        public CustomerModel(String customerFirstName)
        {
            FirstName = customerFirstName;
        }

        public String FirstName
        {
            get
            {
                return firstname;
            }
            set
            {
                firstname = value;
                OnPropertyChanged("FirstName");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region IDataErrorInfo members

        public string this[string columnName]
        {

            get
            {
                if (columnName == "Name")
                {
                    if (String.IsNullOrWhiteSpace(FirstName))
                    {
                        Error = "Name cannot be null of empty";
                    }
                    else
                    {
                        Error = null;
                    }
                }

                return Error;

            }

        }

        public string Error
        {

            get;
            private set;

        }

        #endregion

    }
}
