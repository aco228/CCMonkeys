using CCMonkeys.Direct;
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

namespace CCMonkeys.Desktop.WPF
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      // ovdje definises onaj connection string koji je program da koristi dok god je u debug 
      // connection string prepravljas u CCSubmitConnectionString klasi za taj id
      // :*

      CCSubmitConnectionString.Type = CCSubmitConnectionStringType.DebugDesktop;

      InitializeComponent();
    }
  }
}
