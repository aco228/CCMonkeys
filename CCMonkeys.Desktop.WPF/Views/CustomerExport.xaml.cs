using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CCMonkeys.Wpf.Desktop.Core.Managers;
using CCMonkeys.Wpf.Desktop.Csv.Write;
using CCMonkeys.Wpf.Desktop.Models;
using CCMonkeys.Wpf.Desktop.ViewModels;

namespace CCMonkeys.Wpf.Desktop.Views
{
    /// <summary>
    /// Interaction logic for CustomerExport.xaml
    /// </summary>
    public partial class CustomerExport : Page
    {

        public CustomerExport()
        {
            InitializeComponent();

            DataContext = new CustomerCsvExportViewModel();

            btnSaveCsv.IsEnabled = false;
        }

        private void BtnCsvSavePath_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderDlg = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = folderDlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {

                var savePath = folderDlg.SelectedPath;

                //var subscribedProvider = (PaymentProviderEnum)customerCsvExportVM.Providers.FirstOrDefault(provider => provider.ComboBoxIndex == providerSubscribedCombo.SelectedIndex).ProviderId;
                //var notSubscribedProvider = (PaymentProviderEnum)customerCsvExportVM.Providers.FirstOrDefault(provider => provider.ComboBoxIndex == providerNotSubscribedCombo.SelectedIndex).ProviderId;

                //var recordsToWrite = ClientManager.QueryUnsubscribedUsersByEmail(subscribedProvider, notSubscribedProvider);
                //var isSuccess = CsvWriterCustom.WriteToCsvFile(recordsToWrite, savePath);

                //if (isSuccess)
                //{
                //    MessageBoxResult resultMsg = MessageBox.Show("Success", "File saved to - " + savePath);
                //}
                //else
                //{
                //    MessageBoxResult resultMsg = MessageBox.Show("Failed", "Failed to save to " + savePath);
                //}
            }

        }
    }
}
