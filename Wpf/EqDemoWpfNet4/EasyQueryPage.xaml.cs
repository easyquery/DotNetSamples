using System;
using System.Diagnostics;
using System.Data;
using System.Data.Entity.Migrations;
using System.IO;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Win32;
using Microsoft.Data.SqlClient;

using EasyData.Export;

using Korzh.EasyQuery;
using Korzh.EasyQuery.Wpf;
using Korzh.EasyQuery.Db;
using Korzh.EasyQuery.Services;
using Korzh.EasyQuery.EntityFramework;

using EqDemo.Models;

namespace EqDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class EasyQueryPage : Page
    {

        private SqlConnection _connection;

        public EasyQueryPage()
        {

            Korzh.EasyQuery.Wpf.License.Key = "Drkqtq3P4-xp8tj5EzzARwQCG8ES4I8Y0Q";

            InitDatabase();

            InitializeComponent();
            DataContext = this;

            InitEasyQuery();
        }

        private void InitDatabase()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ToString();
            _connection = new SqlConnection(connectionString);

            var migrator = new DbMigrator(new EqDemo.Migrations.Configuration());
            migrator.Update();
        }

        EasyQueryManagerSql EqManager { get; set; }

        public Query Query => EqManager.Query;


        private void InitEasyQuery()
        {
            var options = new EasyQueryOptions();
            EqManager = new EasyQueryManagerSql(options);

            EqManager.Model.LoadFromDbContext(ApplicationDbContext.Create());

            // How to load from connection
            // EasyQueryManagerSql.RegisterDbGate<SqlServerGate>();
            // EqManager.Model.LoadFromConnection(ApplicationDbContext.Create().Database.GetConnection());

            //query initialization
            EqManager.Query.ConditionsChanged += query_ConditionsChanged;
            EqManager.Query.ColumnsChanged += query_ColumnsChanged;

            //add handlers for ListRequest and ValueRequest events
            AddHandler(ListXElement.ListRequestEvent, new ListXElement.ListRequestEventHandler(queryPanel_ListRequest));
            AddHandler(SimpleConditionRow.ValueRequestEvent, new ValueRequestEventHandler(queryPanel_CustomValueRequest));

            //some additional configuration of EasyQuery visual controls
            queryPanel.SortEntities = XSortOrder.Ascending;

            columnsPanel.SortEntities = XSortOrder.Ascending;
            columnsPanel.ShowCheckBoxes = true;
            sortingPanel.SortEntities = XSortOrder.Ascending;
            entitiesPanel.SortEntities = XSortOrder.Ascending;

            PanelExport.Visibility = Visibility.Collapsed;

            textBoxEntityFilter.TextChanged += TextBoxEntityFilter_TextChanged;
            entitiesPanel.ItemAdding += EntitiesPanel_ItemAdding;
        }

        void query_ColumnsChanged(object sender, QueryColumnsChangeEventArgs e)
        {
            SetSql();
        }

        void query_ConditionsChanged(object sender, ConditionsChangeEventArgs e)
        {
            SetSql();
        }

        private void TextBoxEntityFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            entitiesPanel.FilterByText(textBoxEntityFilter.Text);
        }

        private void EntitiesPanel_ItemAdding(object sender, ItemAddingEventArgs e)
        {
            //set e.Accept to true only for those item which you want to leave in the tree
        }

        void SetSql()
        {
            var result = EqManager.BuildQuery();
            if (result == null) return;

            textBoxSql.Text = result.Statement;
            buttonExecute.IsEnabled = !string.IsNullOrEmpty(result.Statement);
        }

        private void CheckConnection()
        {
            if (_connection.State != System.Data.ConnectionState.Open)
                _connection.Open();
        }

        private void Execute_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string sql = textBoxSql.Text;
                CheckConnection();
                var resultDA = new SqlDataAdapter(sql, _connection);

                DataSet ResultDS = new DataSet();
                resultDA.Fill(ResultDS, "Result");
                datGrid.ItemsSource = ResultDS.Tables[0].DefaultView;

                _connection.Close();
                PanelExport.Visibility = Visibility.Visible;
            }
            catch (Exception error)
            {
                //if some error occurs just show the error message 
                MessageBox.Show(error.Message);
            }
            finally
            {
                _connection.Close();
            }
        }

        private void TextBoxSql_TextChanged(object sender, TextChangedEventArgs e)
        {
            buttonExecute.IsEnabled = !string.IsNullOrEmpty(textBoxSql.Text);
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Clear();
            PanelExport.Visibility = Visibility.Collapsed;
        }

        private void Clear()
        {
            EqManager.Query.Clear();

            textBoxSql.Clear();
            datGrid.ItemsSource = null;
            buttonExecute.IsEnabled = false;
        }

        private void queryPanel_ListRequest(object sender, ListRequestEventArgs e)
        {
            string listName = e.ListName;
            if (listName == "SQL")
            {
                GetSqlList(e.SQL, e.ListItems);
            }
            else if (listName == "RegionList")
            {
                e.ListItems.Clear();
                e.ListItems.Add("British Columbia", "BC");
                e.ListItems.Add("Colorado", "CO");
                e.ListItems.Add("Oregon", "OR");
                e.ListItems.Add("Washington", "WA");
            }
        }

        private void GetSqlList(string sql, ValueItemList items)
        {
            CheckConnection();
            var resultDA = new SqlDataAdapter(sql, _connection);

            DataSet tempDS = new DataSet();
            try
            {
                resultDA.Fill(tempDS, "Result");

                StringWriter strWriter = new StringWriter();
                tempDS.WriteXml(strWriter);
                items.LoadFromXml(strWriter.ToString());
            }
            catch (Exception)
            {
                items.Clear();
            }
        }

        /// <summary>
        /// An example of CUSTOM value editor processing. 
        /// This an event handler for SimpleConditionRow.ValueRequestEvent which is raised when we click on element with "CUSTOM" value editor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void queryPanel_CustomValueRequest(object sender, ValueRequestEventArgs e)
        {
            var postalCodeDlg = new PostalCodeDialog();
            postalCodeDlg.Owner = Window.GetWindow(this);

            bool? res = postalCodeDlg.ShowDialog();

            if (res != null && res.Value)
            {
                var district = postalCodeDlg.SelectedDistrict;
                e.Value = district.Code;
                e.Text = district.Name;
            }
        }


        private void Load_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDlg = new OpenFileDialog();
                openFileDlg.Filter = "JSON files (*.json)|*.json|XML files (*.xml)|*.xml";
                openFileDlg.FilterIndex = 1;
                openFileDlg.InitialDirectory = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "App_Data");
                bool? result = openFileDlg.ShowDialog();
                if (result == true)
                {
                    if (openFileDlg.FilterIndex == 1)
                    {
                        EqManager.Query.LoadFromJsonFile(openFileDlg.FileName);
                    }
                    else
                    {
                        EqManager.Query.LoadFromXmlFile(openFileDlg.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Specified query file is not valid:\n" + ex.Message);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDlg = new SaveFileDialog();
            saveFileDlg.Filter = "JSON files (*.json)|*.json|XML files (*.xml)|*.xml";
            saveFileDlg.FilterIndex = 1;
            saveFileDlg.AddExtension = true;
            saveFileDlg.InitialDirectory = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "App_Data");
            bool? result = saveFileDlg.ShowDialog();
            if (result == true)
            {
                if (saveFileDlg.FilterIndex == 1)
                {
                    EqManager.Query.SaveToJsonFile(saveFileDlg.FileName);
                }
                else
                {
                    EqManager.Query.SaveToXmlFile(saveFileDlg.FileName);
                }
            }
        }

        private void ExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable resultDt = ((DataView)datGrid.ItemsSource).ToTable();
                SaveFileDialog saveFileDlg = new SaveFileDialog();
                saveFileDlg.Filter = "xls files (*.xls)|*.xls";
                saveFileDlg.DefaultExt = "xls";
                saveFileDlg.FilterIndex = 2;
                saveFileDlg.RestoreDirectory = true;
                bool? result = saveFileDlg.ShowDialog();
                if (result == true)
                {
                    ExportData(new ExcelDataExporter(), saveFileDlg.FileName);
                }
            }
            catch (Exception error)
            {
                //if some error occurs just show the error message 
                MessageBox.Show(error.Message);
            }
        }

        private void ExportToCsv_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDlg = new SaveFileDialog();
                saveFileDlg.Filter = "csv files (*.csv)|*.csv";
                saveFileDlg.DefaultExt = "csv";
                saveFileDlg.FilterIndex = 2;
                saveFileDlg.RestoreDirectory = true;
                bool? result = saveFileDlg.ShowDialog();
                if (result == true)
                {
                    ExportData(new CsvDataExporter(), saveFileDlg.FileName);
                }
            }
            catch (Exception error)
            {
                //if some error occurs just show the error message 
                MessageBox.Show(error.Message);
            }
        }

        private void ExportData(IDataExporter exporter, string fileName)
        {
            var resultDt = ((DataView)datGrid.ItemsSource).ToTable();
            using (var resultSet = new EasyDbResultSet(EqManager.Query, resultDt.CreateDataReader(), EqManager.ResultSetOptions))
            using (var fileStream = File.OpenWrite(fileName))
                exporter.Export(resultSet, fileStream);

            new Process
            {
                StartInfo = new ProcessStartInfo(fileName)
                {
                    UseShellExecute = true
                }
            }.Start();
        }
    }
}
