﻿namespace EqDemo
{
    partial class EasyQueryForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EasyQueryForm));
            this.ResultDataTable = new System.Data.DataTable();
            this.openFileDlg = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDlg = new System.Windows.Forms.SaveFileDialog();
            this.ResultDS = new System.Data.DataSet();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.panelExport = new System.Windows.Forms.GroupBox();
            this.btnExportToCsv = new System.Windows.Forms.Button();
            this.btnExportToExcel = new System.Windows.Forms.Button();
            this.groupBoxResultSet = new System.Windows.Forms.GroupBox();
            this.dataGrid1 = new System.Windows.Forms.DataGrid();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.groupBoxSQL = new System.Windows.Forms.GroupBox();
            this.teSQL = new System.Windows.Forms.TextBox();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.panelBG = new System.Windows.Forms.Panel();
            this.panelQuery = new System.Windows.Forms.Panel();
            this.groupBoxColumns = new System.Windows.Forms.GroupBox();
            this.CPanel = new Korzh.EasyQuery.WinForms.ColumnsPanel();
            this.groupBoxConditions = new System.Windows.Forms.GroupBox();
            this.QPanel = new Korzh.EasyQuery.WinForms.QueryPanel();
            this.panelColumns = new System.Windows.Forms.Panel();
            this.groupBoxSorting = new System.Windows.Forms.GroupBox();
            this.SPanel = new Korzh.EasyQuery.WinForms.SortingPanel();
            this.splitter4 = new System.Windows.Forms.Splitter();
            this.groupBoxEntities = new System.Windows.Forms.GroupBox();
            this.EntPanel = new Korzh.EasyQuery.WinForms.EntitiesPanel();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btClear = new System.Windows.Forms.Button();
            this.btLoad = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.btExecute = new System.Windows.Forms.Button();
            this.toolTipExel = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipCsv = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ResultDataTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResultDS)).BeginInit();
            this.panelBottom.SuspendLayout();
            this.panelExport.SuspendLayout();
            this.groupBoxResultSet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
            this.groupBoxSQL.SuspendLayout();
            this.panelBG.SuspendLayout();
            this.panelQuery.SuspendLayout();
            this.groupBoxColumns.SuspendLayout();
            this.groupBoxConditions.SuspendLayout();
            this.panelColumns.SuspendLayout();
            this.groupBoxSorting.SuspendLayout();
            this.groupBoxEntities.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // ResultDataTable
            // 
            this.ResultDataTable.TableName = "Result";
            // 
            // ResultDS
            // 
            this.ResultDS.DataSetName = "ResultDataSet";
            this.ResultDS.Locale = new System.Globalization.CultureInfo("en");
            this.ResultDS.Tables.AddRange(new System.Data.DataTable[] {
            this.ResultDataTable});
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.panelExport);
            this.panelBottom.Controls.Add(this.groupBoxResultSet);
            this.panelBottom.Controls.Add(this.splitter1);
            this.panelBottom.Controls.Add(this.groupBoxSQL);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 588);
            this.panelBottom.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(1380, 274);
            this.panelBottom.TabIndex = 23;
            // 
            // panelExport
            // 
            this.panelExport.Controls.Add(this.btnExportToCsv);
            this.panelExport.Controls.Add(this.btnExportToExcel);
            this.panelExport.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelExport.Location = new System.Drawing.Point(1330, 0);
            this.panelExport.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelExport.Name = "panelExport";
            this.panelExport.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelExport.Size = new System.Drawing.Size(50, 274);
            this.panelExport.TabIndex = 2;
            this.panelExport.TabStop = false;
            // 
            // btnExportToCsv
            // 
            this.btnExportToCsv.Image = global::EqDemo.Properties.Resources.btnCsvExport;
            this.btnExportToCsv.Location = new System.Drawing.Point(3, 78);
            this.btnExportToCsv.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnExportToCsv.Name = "btnExportToCsv";
            this.btnExportToCsv.Size = new System.Drawing.Size(42, 43);
            this.btnExportToCsv.TabIndex = 1;
            this.btnExportToCsv.Tag = "";
            this.toolTipCsv.SetToolTip(this.btnExportToCsv, "Export to CSV");
            this.btnExportToCsv.UseVisualStyleBackColor = true;
            this.btnExportToCsv.Click += new System.EventHandler(this.btnExportCsv_Click);
            // 
            // btnExportToExcel
            // 
            this.btnExportToExcel.Image = global::EqDemo.Properties.Resources.btnExcelExport;
            this.btnExportToExcel.Location = new System.Drawing.Point(3, 25);
            this.btnExportToExcel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnExportToExcel.Name = "btnExportToExcel";
            this.btnExportToExcel.Size = new System.Drawing.Size(42, 43);
            this.btnExportToExcel.TabIndex = 0;
            this.btnExportToExcel.Tag = "";
            this.toolTipExel.SetToolTip(this.btnExportToExcel, "Export to Excel");
            this.btnExportToExcel.UseVisualStyleBackColor = true;
            this.btnExportToExcel.Click += new System.EventHandler(this.btnExportXls_Click);
            // 
            // groupBoxResultSet
            // 
            this.groupBoxResultSet.BackColor = System.Drawing.SystemColors.Control;
            this.groupBoxResultSet.Controls.Add(this.dataGrid1);
            this.groupBoxResultSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxResultSet.Location = new System.Drawing.Point(550, 0);
            this.groupBoxResultSet.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxResultSet.Name = "groupBoxResultSet";
            this.groupBoxResultSet.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxResultSet.Size = new System.Drawing.Size(830, 274);
            this.groupBoxResultSet.TabIndex = 2;
            this.groupBoxResultSet.TabStop = false;
            this.groupBoxResultSet.Text = "Result set";
            // 
            // dataGrid1
            // 
            this.dataGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGrid1.DataMember = "";
            this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGrid1.Location = new System.Drawing.Point(4, 29);
            this.dataGrid1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.Size = new System.Drawing.Size(771, 240);
            this.dataGrid1.TabIndex = 1;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(538, 0);
            this.splitter1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(12, 274);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // groupBoxSQL
            // 
            this.groupBoxSQL.Controls.Add(this.teSQL);
            this.groupBoxSQL.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBoxSQL.Location = new System.Drawing.Point(0, 0);
            this.groupBoxSQL.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxSQL.Name = "groupBoxSQL";
            this.groupBoxSQL.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxSQL.Size = new System.Drawing.Size(538, 274);
            this.groupBoxSQL.TabIndex = 0;
            this.groupBoxSQL.TabStop = false;
            this.groupBoxSQL.Text = "SQL";
            // 
            // teSQL
            // 
            this.teSQL.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.teSQL.Location = new System.Drawing.Point(12, 25);
            this.teSQL.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.teSQL.Multiline = true;
            this.teSQL.Name = "teSQL";
            this.teSQL.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.teSQL.Size = new System.Drawing.Size(512, 235);
            this.teSQL.TabIndex = 9;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 576);
            this.splitter2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(1380, 12);
            this.splitter2.TabIndex = 24;
            this.splitter2.TabStop = false;
            // 
            // panelBG
            // 
            this.panelBG.Controls.Add(this.panelQuery);
            this.panelBG.Controls.Add(this.splitter4);
            this.panelBG.Controls.Add(this.groupBoxEntities);
            this.panelBG.Controls.Add(this.panelButtons);
            this.panelBG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBG.Location = new System.Drawing.Point(0, 0);
            this.panelBG.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelBG.Name = "panelBG";
            this.panelBG.Size = new System.Drawing.Size(1380, 576);
            this.panelBG.TabIndex = 25;
            // 
            // panelQuery
            // 
            this.panelQuery.Controls.Add(this.groupBoxColumns);
            this.panelQuery.Controls.Add(this.groupBoxConditions);
            this.panelQuery.Controls.Add(this.panelColumns);
            this.panelQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelQuery.Location = new System.Drawing.Point(280, 0);
            this.panelQuery.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelQuery.Name = "panelQuery";
            this.panelQuery.Size = new System.Drawing.Size(983, 576);
            this.panelQuery.TabIndex = 33;
            // 
            // groupBoxColumns
            // 
            this.groupBoxColumns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxColumns.Controls.Add(this.CPanel);
            this.groupBoxColumns.Location = new System.Drawing.Point(0, 5);
            this.groupBoxColumns.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxColumns.Name = "groupBoxColumns";
            this.groupBoxColumns.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxColumns.Size = new System.Drawing.Size(628, 218);
            this.groupBoxColumns.TabIndex = 33;
            this.groupBoxColumns.TabStop = false;
            this.groupBoxColumns.Text = "Query Columns";
            // 
            // CPanel
            // 
            this.CPanel.Active = false;
            this.CPanel.ActiveRowIndex = -1;
            this.CPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CPanel.Appearance.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(225)))), ((int)(((byte)(190)))));
            this.CPanel.Appearance.ActiveForeColor = System.Drawing.SystemColors.HighlightText;
            this.CPanel.Appearance.AdditionRowColor = System.Drawing.Color.DarkGreen;
            this.CPanel.Appearance.AttrElementFormat = "{entity} {attr}";
            this.CPanel.Appearance.BackColor = System.Drawing.Color.LightYellow;
            this.CPanel.Appearance.ButtonForeColor = System.Drawing.SystemColors.ControlText;
            this.CPanel.Appearance.FocusBorderStyle = System.Windows.Forms.BorderStyle.None;
            this.CPanel.Appearance.TitleElementFormat = "{attr}";
            this.CPanel.BackColor = System.Drawing.Color.LightYellow;
            this.CPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CPanel.EditMode = Korzh.EasyQuery.WinForms.ColumnsPanelEditMode.All;
            this.CPanel.Location = new System.Drawing.Point(10, 28);
            this.CPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CPanel.Name = "CPanel";
            this.CPanel.Query = null;
            this.CPanel.Size = new System.Drawing.Size(609, 180);
            this.CPanel.TabIndex = 27;
            this.CPanel.TabStop = true;
            // 
            // groupBoxConditions
            // 
            this.groupBoxConditions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxConditions.Controls.Add(this.QPanel);
            this.groupBoxConditions.Location = new System.Drawing.Point(0, 223);
            this.groupBoxConditions.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxConditions.Name = "groupBoxConditions";
            this.groupBoxConditions.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxConditions.Size = new System.Drawing.Size(979, 353);
            this.groupBoxConditions.TabIndex = 2;
            this.groupBoxConditions.TabStop = false;
            this.groupBoxConditions.Text = "Query Conditions";
            // 
            // QPanel
            // 
            this.QPanel.Active = false;
            this.QPanel.ActiveRowIndex = -1;
            this.QPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.QPanel.Appearance.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(225)))), ((int)(((byte)(190)))));
            this.QPanel.Appearance.ActiveForeColor = System.Drawing.SystemColors.HighlightText;
            this.QPanel.Appearance.AdditionRowColor = System.Drawing.Color.DarkGreen;
            this.QPanel.Appearance.ButtonForeColor = System.Drawing.SystemColors.ControlText;
            this.QPanel.Appearance.DefaultListControlType = "LISTBOX";
            this.QPanel.Appearance.ExprColor = System.Drawing.Color.Indigo;
            this.QPanel.Appearance.FocusBorderStyle = System.Windows.Forms.BorderStyle.None;
            this.QPanel.Appearance.OperatorColor = System.Drawing.Color.MediumBlue;
            this.QPanel.Appearance.ShowRootRow = true;
            this.QPanel.BackColor = System.Drawing.Color.White;
            this.QPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.QPanel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.QPanel.Location = new System.Drawing.Point(10, 22);
            this.QPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.QPanel.Name = "QPanel";
            this.QPanel.Query = null;
            this.QPanel.Size = new System.Drawing.Size(959, 326);
            this.QPanel.TabIndex = 27;
            this.QPanel.TabStop = true;
            this.QPanel.ListRequest += new Korzh.EasyQuery.WinForms.ListRequestEventHandler(this.QPanel_ListRequest);
            // 
            // panelColumns
            // 
            this.panelColumns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelColumns.Controls.Add(this.groupBoxSorting);
            this.panelColumns.Location = new System.Drawing.Point(6, 5);
            this.panelColumns.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelColumns.Name = "panelColumns";
            this.panelColumns.Size = new System.Drawing.Size(977, 218);
            this.panelColumns.TabIndex = 4;
            // 
            // groupBoxSorting
            // 
            this.groupBoxSorting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSorting.Controls.Add(this.SPanel);
            this.groupBoxSorting.Location = new System.Drawing.Point(622, 0);
            this.groupBoxSorting.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxSorting.Name = "groupBoxSorting";
            this.groupBoxSorting.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxSorting.Size = new System.Drawing.Size(351, 218);
            this.groupBoxSorting.TabIndex = 30;
            this.groupBoxSorting.TabStop = false;
            this.groupBoxSorting.Text = "Columns Sorting";
            // 
            // SPanel
            // 
            this.SPanel.Active = false;
            this.SPanel.ActiveRowIndex = -1;
            this.SPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SPanel.Appearance.ActiveBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(225)))), ((int)(((byte)(190)))));
            this.SPanel.Appearance.ActiveForeColor = System.Drawing.SystemColors.HighlightText;
            this.SPanel.Appearance.AdditionRowColor = System.Drawing.Color.DarkGreen;
            this.SPanel.Appearance.AttrElementFormat = "{entity} {attr}";
            this.SPanel.Appearance.BackColor = System.Drawing.Color.LightYellow;
            this.SPanel.Appearance.ButtonForeColor = System.Drawing.SystemColors.ControlText;
            this.SPanel.Appearance.FocusBorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SPanel.Appearance.TitleElementFormat = "{attr}";
            this.SPanel.BackColor = System.Drawing.Color.LightYellow;
            this.SPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SPanel.EditMode = Korzh.EasyQuery.WinForms.ColumnsPanelEditMode.All;
            this.SPanel.Location = new System.Drawing.Point(9, 28);
            this.SPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SPanel.Name = "SPanel";
            this.SPanel.Query = null;
            this.SPanel.Size = new System.Drawing.Size(332, 180);
            this.SPanel.SortEditMode = Korzh.EasyQuery.WinForms.SortingPanel.SortEditModeKind.All;
            this.SPanel.TabIndex = 28;
            this.SPanel.TabStop = true;
            // 
            // splitter4
            // 
            this.splitter4.BackColor = System.Drawing.SystemColors.Control;
            this.splitter4.Location = new System.Drawing.Point(268, 0);
            this.splitter4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.splitter4.Name = "splitter4";
            this.splitter4.Size = new System.Drawing.Size(12, 576);
            this.splitter4.TabIndex = 32;
            this.splitter4.TabStop = false;
            // 
            // groupBoxEntities
            // 
            this.groupBoxEntities.Controls.Add(this.EntPanel);
            this.groupBoxEntities.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBoxEntities.Location = new System.Drawing.Point(0, 0);
            this.groupBoxEntities.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxEntities.Name = "groupBoxEntities";
            this.groupBoxEntities.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxEntities.Size = new System.Drawing.Size(268, 576);
            this.groupBoxEntities.TabIndex = 29;
            this.groupBoxEntities.TabStop = false;
            this.groupBoxEntities.Text = "Objects and their attributes";
            // 
            // EntPanel
            // 
            this.EntPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.EntPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EntPanel.ImageAddColumns = ((System.Drawing.Image)(resources.GetObject("EntPanel.ImageAddColumns")));
            this.EntPanel.ImageAddConditions = ((System.Drawing.Image)(resources.GetObject("EntPanel.ImageAddConditions")));
            this.EntPanel.ImageSelectAll = ((System.Drawing.Image)(resources.GetObject("EntPanel.ImageSelectAll")));
            this.EntPanel.ImageSelectNone = ((System.Drawing.Image)(resources.GetObject("EntPanel.ImageSelectNone")));
            this.EntPanel.Location = new System.Drawing.Point(4, 24);
            this.EntPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.EntPanel.Name = "EntPanel";
            this.EntPanel.Query = null;
            this.EntPanel.ShowFilter = true;
            this.EntPanel.Size = new System.Drawing.Size(260, 547);
            this.EntPanel.TabIndex = 29;
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.btClear);
            this.panelButtons.Controls.Add(this.btLoad);
            this.panelButtons.Controls.Add(this.btSave);
            this.panelButtons.Controls.Add(this.btExecute);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelButtons.Location = new System.Drawing.Point(1263, 0);
            this.panelButtons.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(117, 576);
            this.panelButtons.TabIndex = 22;
            // 
            // btClear
            // 
            this.btClear.Location = new System.Drawing.Point(12, 25);
            this.btClear.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btClear.Name = "btClear";
            this.btClear.Size = new System.Drawing.Size(93, 37);
            this.btClear.TabIndex = 12;
            this.btClear.Text = "Clear";
            this.btClear.Click += new System.EventHandler(this.btClear_Click);
            // 
            // btLoad
            // 
            this.btLoad.Location = new System.Drawing.Point(12, 86);
            this.btLoad.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btLoad.Name = "btLoad";
            this.btLoad.Size = new System.Drawing.Size(93, 37);
            this.btLoad.TabIndex = 11;
            this.btLoad.Text = "Load";
            this.btLoad.Click += new System.EventHandler(this.btLoad_Click);
            // 
            // btSave
            // 
            this.btSave.Location = new System.Drawing.Point(12, 135);
            this.btSave.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(93, 37);
            this.btSave.TabIndex = 10;
            this.btSave.Text = "Save";
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // btExecute
            // 
            this.btExecute.Location = new System.Drawing.Point(12, 249);
            this.btExecute.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btExecute.Name = "btExecute";
            this.btExecute.Size = new System.Drawing.Size(93, 60);
            this.btExecute.TabIndex = 9;
            this.btExecute.Text = "Fetch data";
            this.btExecute.Click += new System.EventHandler(this.btExecute_Click);
            // 
            // EasyQueryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1380, 862);
            this.Controls.Add(this.panelBG);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.panelBottom);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "EasyQueryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Easy Query.NET WinForms demo";
            ((System.ComponentModel.ISupportInitialize)(this.ResultDataTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResultDS)).EndInit();
            this.panelBottom.ResumeLayout(false);
            this.panelExport.ResumeLayout(false);
            this.groupBoxResultSet.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
            this.groupBoxSQL.ResumeLayout(false);
            this.groupBoxSQL.PerformLayout();
            this.panelBG.ResumeLayout(false);
            this.panelQuery.ResumeLayout(false);
            this.groupBoxColumns.ResumeLayout(false);
            this.groupBoxConditions.ResumeLayout(false);
            this.panelColumns.ResumeLayout(false);
            this.groupBoxSorting.ResumeLayout(false);
            this.groupBoxEntities.ResumeLayout(false);
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

    }
}
