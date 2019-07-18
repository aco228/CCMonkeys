using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using CCMonkeys.Wpf.Desktop.Core.Csv.Models;
using CCMonkeys.Wpf.Desktop.Core.Helpers;
using CCMonkeys.Wpf.Desktop.Core.Csv.Readers;
using CCMonkeys.Wpf.Desktop.Core.Csv.Write;

namespace CCMonkeys.Wpf.Desktop.Views
{

  public partial class CustomerImport : Page
  {

    private List<DynamicCsv> RecordsToInsert = null;
    private List<string> DynamicCsvObjectNames = null;
    private Dictionary<string, CsvNameByIndex> DynamicCsvComboNamesDictionary = new Dictionary<string, CsvNameByIndex>();
    private List<ComboBox> ComboBoxesCsv = null;


    public CustomerImport()
    {
      InitializeComponent();

      ColumnNamesGridFill();
    }

    private void Window_ContentRendered(object sender, EventArgs e)
    {
      pbStatus.Visibility = Visibility.Hidden;

    }

    private async void TestProgressBar()
    {
      for (int i = 0; i < 100; i++)
      {
        await Task.Delay(100);
        pbStatus.Value++;
      }
    }

    private void BtnCsvChoose_Click(object sender, RoutedEventArgs e)
    {
      var openFileDlg = new OpenFileDialog();
      Nullable<bool> result = openFileDlg.ShowDialog();

      allRecordsCount.Content = "Total records count: " + 0;

      if (result == true)
      {

        filePathLabel.Text = openFileDlg.FileName;
        var dynamicCsvInfo = CsvReaderCustom.Read<DynamicCsv>(openFileDlg.FileName, false);

        allRecordsCount.Content = "Total records count: " + dynamicCsvInfo.AllRecordsCount;

        var sampleDataList = new List<DynamicCsv>();

        if (dynamicCsvInfo.IsHeadless)
        {
          dynamicCsvInfo = CsvReaderCustom.Read<DynamicCsv>(openFileDlg.FileName, true);
          allRecordsCount.Content = "Total records count: " + dynamicCsvInfo.AllRecordsCount;
        }
        if (dynamicCsvInfo.Records.Count >= 30)
          sampleDataList = dynamicCsvInfo.Records.Take(30).ToList();
        else
          sampleDataList = dynamicCsvInfo.Records.ToList();

        if (sampleDataList.Count == 0)
        {
          return;
        }

        RecordsToInsert = new List<DynamicCsv>();
        RecordsToInsert.AddRange(dynamicCsvInfo.Records);
        btnCsvImport.IsEnabled = true;

        var dataListObservable = new ObservableCollection<DynamicCsv>(sampleDataList);

        this.csvDataGrid.ItemsSource = dataListObservable;

        SetEqualColumnWidth(csvDataGrid);

      }
    }

    private void AddColumnsToDynamicTable(string[] newColumnNames)
    {
      foreach (string name in newColumnNames)
      {
        csvDataGrid.Columns.Add(new DataGridTextColumn
        {
          // bind to a dictionary property
          Binding = new Binding("Custom[" + name + "]"),
          Header = name
        });
      }
    }

    private void SetEqualColumnWidth(DataGrid dataGrid)
    {
      foreach (var col in dataGrid.Columns)
      {
        col.Width = Width = dataGrid.Width / dataGrid.Columns.Count;
      }
    }

    private void BtnCsvImport_Click(object sender, RoutedEventArgs e)
    {
      pbStatus.Visibility = Visibility.Visible;

      var remainingRecords = new List<DynamicCsv>();
      remainingRecords.AddRange(RecordsToInsert);
      var insertedRecordsNumber = 0;
      var updatedRecordsNumber = 0;

      while (remainingRecords.Count > 0)
      {
        var result = CsvWriterCustom.WriteToDatabase(remainingRecords, 10);
        insertedRecordsNumber += result.NewlyInsertedRecordsCount;
        insertedRecordsCount.Content = "Inserted:" + insertedRecordsNumber;
        updatedRecordsNumber += result.UpdatedRecordsCount;
        updatedDbRecordsCount.Content = "Updated:" + updatedRecordsNumber;
        remainingRecords = result.RemainingRecords;

        pbStatus.Value = (insertedRecordsNumber / RecordsToInsert.Count()) * 100;
      }

    }

    private void ComboBoxCsvName_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var cbUnknown = (ComboBox)sender;
      DynamicCsvComboNamesDictionary[cbUnknown.Name].ColumnName = cbUnknown.SelectedValue.ToString();

      foreach (var col in csvDataGrid.Columns)
      {
        if (col.Header == cbUnknown.SelectedValue)
        {
          col.Header = "UNSET";
          break;
        }
      }

      csvDataGrid.Columns[DynamicCsvComboNamesDictionary[cbUnknown.Name].Index].Header = cbUnknown.SelectedValue.ToString();
    }

    private void ColumnNamesGridFill()
    {
      columnNamesGrid.Children.Clear();
      var objNamesList = ObjectHelper.ObjectPropertyNamesToList<DynamicCsv>(new DynamicCsv());
      var labelTableNames = new Label();
      labelTableNames.Height = 30;
      labelTableNames.VerticalAlignment = VerticalAlignment.Top;
      labelTableNames.HorizontalAlignment = HorizontalAlignment.Stretch;
      var objNamesText = "";
      foreach (var item in objNamesList)
      {
        objNamesText += item + " | ";
      }
      labelTableNames.Content = objNamesText;
      columnNamesGrid.Children.Add(labelTableNames);
    }

    private void CreateDynamicComboNames()
    {

      ComboBoxesCsv = new List<ComboBox>();
      //comboGrid.Children.Clear();

      var dynamicCsvObjectNames = ObjectHelper.ObjectPropertyNamesToList<DynamicCsv>(new DynamicCsv());
      dynamicCsvObjectNames.Add("UNSET");
      DynamicCsvObjectNames = dynamicCsvObjectNames;

      //comboGrid.Columns = dynamicCsvObjectNames.Count() - 1;

      for (int index = 1; index < dynamicCsvObjectNames.Count(); index++)
      {
        var cb = new ComboBox();
        cb.HorizontalAlignment = HorizontalAlignment.Stretch;
        cb.ItemsSource = dynamicCsvObjectNames;
        cb.SelectedIndex = dynamicCsvObjectNames.Count() - 1;
        cb.Name = "cbColumnName" + index;

        cb.SelectionChanged += ComboBoxCsvName_SelectionChanged;

        if (!DynamicCsvComboNamesDictionary.Keys.Contains(cb.Name))
          DynamicCsvComboNamesDictionary.Add(cb.Name, new CsvNameByIndex { Index = index - 1, ColumnName = cb.SelectedValue.ToString() });
        else
          DynamicCsvComboNamesDictionary[cb.Name].ColumnName = cb.SelectedValue.ToString();

        ComboBoxesCsv.Add(cb);
        //comboGrid.Children.Add(cb);
      }

      structureTitleLabel.Visibility = Visibility.Visible;

    }

    

  }
}
