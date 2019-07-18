using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CCMonkeys.Wpf.Desktop.ViewModels;

namespace CCMonkeys.Wpf.Desktop.Commands
{
  internal class UpdateCustomerCommand : ICommand
  {

    private CustomerViewModel viewModel;

    public UpdateCustomerCommand(CustomerViewModel viewModel)
    {
      this.viewModel = viewModel;
    }

    public event EventHandler CanExecuteChanged
    {
      add
      {
        CommandManager.RequerySuggested += value;
      }
      remove
      {
        CommandManager.RequerySuggested -= value;
      }
    }

    public bool CanExecute(object parameter)
    {
      return String.IsNullOrWhiteSpace(this.viewModel.Customer.Error);
    }

    public void Execute(object parameter)
    {
      viewModel.SaveChanges();
    }
  }
}
