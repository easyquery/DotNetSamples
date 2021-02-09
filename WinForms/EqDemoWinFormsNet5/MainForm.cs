﻿using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using Korzh.DbUtils;

using Korzh.EasyQuery;
using Korzh.EasyQuery.Db;
using Korzh.EasyQuery.EntityFrameworkCore;
using Korzh.EasyQuery.Services;
using Korzh.EasyQuery.WinForms;

using EqDemo.Models;
using System.Configuration;

namespace EqDemo
{
    public partial class MainForm : Form
    {
        private System.Windows.Forms.OpenFileDialog openFileDlg;
        private System.Windows.Forms.SaveFileDialog saveFileDlg;
        private System.Data.DataSet ResultDS;
        private System.Data.DataTable ResultDataTable;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.GroupBox groupBoxSQL;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.GroupBox groupBoxResultSet;
        private System.Windows.Forms.TextBox teSQL;
        private System.Windows.Forms.DataGridView dataGrid1;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Panel panelBG;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btClear;
        private System.Windows.Forms.Button btLoad;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Button btExecute;
        private GroupBox groupBoxEntities;
		private System.Windows.Forms.Panel panelQuery;
        private System.Windows.Forms.GroupBox groupBoxColumns;
        private System.Windows.Forms.GroupBox groupBoxConditions;
        private System.Windows.Forms.Panel panelColumns;
        private System.Windows.Forms.GroupBox groupBoxSorting;
        private System.Windows.Forms.Splitter splitter4;
        private Korzh.EasyQuery.WinForms.EntitiesPanel EntPanel;
        private Korzh.EasyQuery.WinForms.QueryPanel QPanel;
        private Korzh.EasyQuery.WinForms.ColumnsPanel CPanel;
        private Korzh.EasyQuery.WinForms.SortingPanel SPanel;


        private ToolTip toolTipExel;
        private ToolTip toolTipCsv;
        private GroupBox panelExport;
        private Button btnExportCsv;
        private Button btnExportExel;

        private readonly string _dataFolder = "App_Data";
        private readonly string _appDirectory;

        private SqlConnection _connection;

        private DbModel _dataModel;
        private DbQuery _query;

        private EntityAttr _countryAttr = null;


        public MainForm()
        {
            Korzh.EasyQuery.WinForms.License.Key = "M-Vm5PXqfpFr0P6bDruZ2wQIS6HV2Y";

            _appDirectory = System.IO.Directory.GetCurrentDirectory();
            _dataFolder = System.IO.Path.Combine(_appDirectory, "App_Data");

            InitializeComponent();

            InitEasyQuery();

            HideExportPanel();

            //postpone DB initialization / opening a connection
            var dbConnectTimer = new Timer();
            dbConnectTimer.Tick += new EventHandler(TimerEventProcessor);
            dbConnectTimer.Interval = 100;
            dbConnectTimer.Start();
        }

        private void InitEasyQuery()
        {
            //intialize the data model and load it from XML (or JSON) file
            _dataModel = new DbModel();
            using (var dbContext = ApplicationDbContext.Create()) 
            {
                _dataModel.LoadFromDbContext(dbContext);
            }

            // DbGate.Register<SqlServerGate>();
            // _dataModel.LoadFromConnection(ApplicationDbContext.Create().Database.Connection);

            //saving the reference to Customer Country attribute in our model (will be used on RequestList processing)
            _countryAttr = _dataModel.EntityRoot.FindAttributeById("Customers.Country");

            //initialize the query and assign it to all visual controls.
            _query = new DbQuery(_dataModel);
            QPanel.Query = _query;
            CPanel.Query = _query;
            SPanel.Query = _query;
            EntPanel.Query = _query;

            //setting differnt properties of EasyQuery visual controls
            this.CPanel.AllowEditCaptions = true;
            this.CPanel.AllowSorting = true;
            this.EntPanel.ShowFilter = true;
        }

        private void TimerEventProcessor(Object myObject, EventArgs myEventArgs)
        {
            CheckConnection();
        }

        private void CheckConnection()
        {
            var prevTitle = this.Text;
            this.Text += " (openning the connection to DB...)";
            try {
                if (_connection == null) {
                    var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ToString();
                    using (var dbContext = ApplicationDbContext.Create())
                    {
                        _connection = new SqlConnection(connectionString);
                        if (dbContext.Database.EnsureCreated())
                        {
                            Korzh.DbUtils.DbInitializer.Create(options => {
                                options.UseSqlServer(connectionString);
                                options.UseZipPacker(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "App_Data/EqDemoData.zip"));
                            })
                            .Seed();
                        }
                    };
                }
                if (_connection.State != ConnectionState.Open) {
                    _connection.Open();
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            this.Text = prevTitle;
        }

        private void btClear_Click(object sender, System.EventArgs e)
        {
            QPanel.Query.Clear();
            EntPanel.ClearFilter();
            teSQL.Clear();
            dataGrid1.DataSource = null;
            HideExportPanel();
        }

        private void btLoad_Click(object sender, System.EventArgs e)
        {
            openFileDlg.Filter = "JSON files (*.json)|*.json|XML files (*.xml)|*.xml";
            openFileDlg.FilterIndex = 1;
            openFileDlg.InitialDirectory = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "App_Data");
            if (openFileDlg.ShowDialog() == DialogResult.OK) {
                if (openFileDlg.FilterIndex == 1) {
                    QPanel.Query.LoadFromJsonFile(openFileDlg.FileName);
                }
                else {
                    QPanel.Query.LoadFromXmlFile(openFileDlg.FileName);
                }
            }
        }

        private void btSave_Click(object sender, System.EventArgs e)
        {
            saveFileDlg.Filter = "JSON files (*.json)|*.json|XML files (*.xml)|*.xml";
            saveFileDlg.FilterIndex = 1;
            saveFileDlg.AddExtension = true;
            openFileDlg.InitialDirectory = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "App_Data");
            saveFileDlg.FileName = System.IO.Path.Combine(_dataFolder, "queries\\query");
            if (saveFileDlg.ShowDialog() == DialogResult.OK) {
                if (saveFileDlg.FilterIndex == 1) {
                    QPanel.Query.SaveToJsonFile(saveFileDlg.FileName);
                }
                else {
                    QPanel.Query.SaveToXmlFile(saveFileDlg.FileName);
                }
            }
        }

        private void btExecute_Click(object sender, System.EventArgs e)
        {
            try {

                ResultDS.Reset();
                var builder = BuildSQL();
                var builderResult = builder.Result;

                CheckConnection();

                var command = _connection.CreateCommand();
                command.CommandText = builderResult.Statement;
                foreach (QueryParam param in builderResult.Params) {
                    command.Parameters.Add("@" + param.Id);
                    command.Parameters["@" + param.Id].Value = param.Value;
                }

                var resultDA = new SqlDataAdapter(command);
                resultDA.Fill(ResultDS, "Result");
                dataGrid1.DataSource = ResultDS.Tables[0].DefaultView;

                _connection.Close();
                ShowExportPanel();
            }
            catch (Exception error)
            {
                //if some error occurs just show the error message 
                MessageBox.Show(error.Message);
            }
        }


        private IQueryBuilder BuildSQL()
        {
            teSQL.Clear();
            try {
                SqlQueryBuilder builder = new SqlQueryBuilder(_query);
                builder.Formats.SetDefaultFormats(FormatType.MsSqlServer);

                if (builder.CanBuild) {
                    builder.BuildSQL();
                    string sql = builder.Result.SQL;
                    teSQL.Text = sql;

                    return builder;
                }

                return null;
            }
            catch (Exception ex) {
                //Simply ignore any possible exception
                MessageBox.Show(ex.Message);
            }

            return null;
        }

        private void QPanel_ListRequest(object sender, ListRequestEventArgs e)
        {
            if (e.ListName == "SQL") {
                CheckConnection();

                var sql = e.Data.ToString();
                DataSet tempDS = new DataSet();

                var tempDA = new SqlDataAdapter(sql, _connection);
                tempDA.Fill(tempDS, "Temp");

                var strWriter = new StringWriter();
                tempDS.WriteXml(strWriter);
                e.ResultXml = strWriter.ToString();

                //e.ListItems.Clear();
                //foreach (DataRow row in tempDS.Tables[0].Rows) {
                //    e.ListItems.Add(row[1].ToString(), row[0].ToString());
                //}            
            }
            else if (e.ListName == "RegionList") {
                e.ListItems.Clear();
                string country = _query.GetOneValueForAttr(_countryAttr);

                if (country == "Canada" || country == null) {
                    e.ListItems.Add("British Columbia", "BC");
                    e.ListItems.Add("Quebec", "Quebec");
                }

                if (country == "USA" || country == null) {
                    e.ListItems.Add("California", "CA");
                    e.ListItems.Add("Colorado", "CO");
                    e.ListItems.Add("Oregon", "OR");
                    e.ListItems.Add("Washington", "WA");
                }
            }

        }

        private void query1_ColumnsChanged(object sender, ColumnsChangeEventArgs e)
        {
            BuildSQL();
            ResultDS.Reset();
        }

        private void query1_ConditionsChanged(object sender, ConditionsChangeEventArgs e)
        {
            EntityAttr baseAttr = null;
            if (e.Condition != null)
                baseAttr = e.Condition.BaseAttr;

            if (baseAttr != null && baseAttr == _countryAttr) {
                QPanel.RefreshList("RegionList");
            }

            BuildSQL();
            ResultDS.Reset();
        }

        private void QPanel_ValueRequest(object sender, ValueRequestEventArgs e)
        {
            MessageBox.Show(e.Data);
        }

        private void QPanel_ConditionRender(object sender, ConditionRenderEventArgs e)
        {
            //for (int i = 0; i < e.Row.Elements.Count; i++) {
            //    e.Row.Elements[i].TextColor = Color.Red;
            //}
        }

        private void CloseConnections()
        {
            if (_connection != null)
                _connection.Close();
        }

        private void btnExportCsv_Click(object sender, EventArgs e)
        {
            try {
                //Save CSV file 
                using (SaveFileDialog saveFileDialog = new SaveFileDialog()) {
                    saveFileDialog.Filter = "csv files (*.csv)|*.csv";
                    saveFileDialog.FilterIndex = 2;
                    saveFileDialog.RestoreDirectory = true;

                    if (saveFileDialog.ShowDialog(this) == DialogResult.OK) {
                        var exporter = new CsvDataExporter();
                        var resultSet = new EasyDbResultSet(_query, ResultDS.Tables[0].CreateDataReader());
                        using (var fileStream = File.OpenWrite(saveFileDialog.FileName))
                            exporter.Export(resultSet, fileStream);
                    }
                }


            }
            catch (Exception error) {
                //if some error occurs just show the error message 
                MessageBox.Show(error.Message);
            }
        }

        private void btnExportXls_Click(object sender, EventArgs e)
        {
            try {
                //Save xls  file 
                using (SaveFileDialog saveFileDialog = new SaveFileDialog()) {
                    saveFileDialog.Filter = "xls files (*.xls)|*.xls";
                    saveFileDialog.FilterIndex = 2;
                    saveFileDialog.RestoreDirectory = true;

                    if (saveFileDialog.ShowDialog(this) == DialogResult.OK)  {
                        var exporter = new ExcelHtmlDataExporter();
                        var resultSet = new EasyDbResultSet(_query, ResultDS.Tables[0].CreateDataReader());
                        using (var fileStream = File.OpenWrite(saveFileDialog.FileName))
                            exporter.Export(resultSet, fileStream);
                    }
                }


            }
            catch (Exception error) {
                //if some error occurs just show the error message 
                MessageBox.Show(error.Message);
            }
        }

        private void ShowExportPanel()
        {
            this.panelExport.Show();
            this.groupBoxResultSet.Width = this.groupBoxResultSet.Parent.ClientSize.Width - this.panelExport.Width - this.groupBoxResultSet.Left - 4;
        }

        private void HideExportPanel()
        {
            this.panelExport.Hide();
            this.groupBoxResultSet.Width = this.groupBoxResultSet.Parent.ClientSize.Width - this.groupBoxResultSet.Left - 4;
        }
    }
}
