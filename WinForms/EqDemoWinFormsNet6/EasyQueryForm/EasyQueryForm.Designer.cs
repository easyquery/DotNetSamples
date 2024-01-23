namespace EqDemo
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EasyQueryForm));
            openFileDlg = new System.Windows.Forms.OpenFileDialog();
            saveFileDlg = new System.Windows.Forms.SaveFileDialog();
            ResultDS = new System.Data.DataSet();
            panelBottom = new System.Windows.Forms.Panel();
            panelExport = new System.Windows.Forms.GroupBox();
            btnExportCsv = new System.Windows.Forms.Button();
            btnExportExel = new System.Windows.Forms.Button();
            groupBoxResultSet = new System.Windows.Forms.GroupBox();
            dataGrid1 = new System.Windows.Forms.DataGridView();
            splitter1 = new System.Windows.Forms.Splitter();
            groupBoxSQL = new System.Windows.Forms.GroupBox();
            teSQL = new System.Windows.Forms.TextBox();
            splitter2 = new System.Windows.Forms.Splitter();
            panelBG = new System.Windows.Forms.Panel();
            panelQuery = new System.Windows.Forms.Panel();
            groupBoxColumns = new System.Windows.Forms.GroupBox();
            CPanel = new Korzh.EasyQuery.WinForms.ColumnsPanel();
            groupBoxConditions = new System.Windows.Forms.GroupBox();
            QPanel = new Korzh.EasyQuery.WinForms.QueryPanel();
            panelColumns = new System.Windows.Forms.Panel();
            groupBoxSorting = new System.Windows.Forms.GroupBox();
            SPanel = new Korzh.EasyQuery.WinForms.SortingPanel();
            splitter4 = new System.Windows.Forms.Splitter();
            groupBoxEntities = new System.Windows.Forms.GroupBox();
            EntPanel = new Korzh.EasyQuery.WinForms.EntitiesPanel();
            panelButtons = new System.Windows.Forms.Panel();
            btClear = new System.Windows.Forms.Button();
            btLoad = new System.Windows.Forms.Button();
            btSave = new System.Windows.Forms.Button();
            btExecute = new System.Windows.Forms.Button();
            toolTipExel = new System.Windows.Forms.ToolTip(components);
            toolTipCsv = new System.Windows.Forms.ToolTip(components);
            ((System.ComponentModel.ISupportInitialize)ResultDS).BeginInit();
            panelBottom.SuspendLayout();
            panelExport.SuspendLayout();
            groupBoxResultSet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGrid1).BeginInit();
            groupBoxSQL.SuspendLayout();
            panelBG.SuspendLayout();
            panelQuery.SuspendLayout();
            groupBoxColumns.SuspendLayout();
            groupBoxConditions.SuspendLayout();
            panelColumns.SuspendLayout();
            groupBoxSorting.SuspendLayout();
            groupBoxEntities.SuspendLayout();
            panelButtons.SuspendLayout();
            SuspendLayout();
            // 
            // ResultDS
            // 
            ResultDS.DataSetName = "ResultDataSet";
            ResultDS.Locale = new System.Globalization.CultureInfo("en");
            // 
            // panelBottom
            // 
            panelBottom.Controls.Add(panelExport);
            panelBottom.Controls.Add(groupBoxResultSet);
            panelBottom.Controls.Add(splitter1);
            panelBottom.Controls.Add(groupBoxSQL);
            panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            panelBottom.Location = new System.Drawing.Point(0, 587);
            panelBottom.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            panelBottom.Name = "panelBottom";
            panelBottom.Size = new System.Drawing.Size(1227, 274);
            panelBottom.TabIndex = 23;
            // 
            // panelExport
            // 
            panelExport.Controls.Add(btnExportCsv);
            panelExport.Controls.Add(btnExportExel);
            panelExport.Dock = System.Windows.Forms.DockStyle.Right;
            panelExport.Location = new System.Drawing.Point(1183, 0);
            panelExport.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            panelExport.Name = "panelExport";
            panelExport.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            panelExport.Size = new System.Drawing.Size(44, 274);
            panelExport.TabIndex = 2;
            panelExport.TabStop = false;
            // 
            // btnExportCsv
            // 
            btnExportCsv.Image = (System.Drawing.Image)resources.GetObject("btnExportCsv.Image");
            btnExportCsv.Location = new System.Drawing.Point(3, 79);
            btnExportCsv.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            btnExportCsv.Name = "btnExportCsv";
            btnExportCsv.Size = new System.Drawing.Size(37, 42);
            btnExportCsv.TabIndex = 1;
            btnExportCsv.Tag = "";
            toolTipCsv.SetToolTip(btnExportCsv, "Export to CSV");
            btnExportCsv.UseVisualStyleBackColor = true;
            btnExportCsv.Click += btnExportCsv_Click;
            // 
            // btnExportExel
            // 
            btnExportExel.Image = (System.Drawing.Image)resources.GetObject("btnExportExel.Image");
            btnExportExel.Location = new System.Drawing.Point(3, 25);
            btnExportExel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            btnExportExel.Name = "btnExportExel";
            btnExportExel.Size = new System.Drawing.Size(37, 42);
            btnExportExel.TabIndex = 0;
            btnExportExel.Tag = "";
            toolTipExel.SetToolTip(btnExportExel, "Export to Excel");
            btnExportExel.UseVisualStyleBackColor = true;
            btnExportExel.Click += btnExportXls_Click;
            // 
            // groupBoxResultSet
            // 
            groupBoxResultSet.BackColor = System.Drawing.SystemColors.Control;
            groupBoxResultSet.Controls.Add(dataGrid1);
            groupBoxResultSet.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxResultSet.Location = new System.Drawing.Point(490, 0);
            groupBoxResultSet.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            groupBoxResultSet.Name = "groupBoxResultSet";
            groupBoxResultSet.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            groupBoxResultSet.Size = new System.Drawing.Size(737, 274);
            groupBoxResultSet.TabIndex = 2;
            groupBoxResultSet.TabStop = false;
            groupBoxResultSet.Text = "Result set";
            // 
            // dataGrid1
            // 
            dataGrid1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dataGrid1.ColumnHeadersHeight = 29;
            dataGrid1.Location = new System.Drawing.Point(0, 21);
            dataGrid1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            dataGrid1.Name = "dataGrid1";
            dataGrid1.RowHeadersWidth = 51;
            dataGrid1.Size = new System.Drawing.Size(732, 246);
            dataGrid1.TabIndex = 1;
            // 
            // splitter1
            // 
            splitter1.Location = new System.Drawing.Point(479, 0);
            splitter1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            splitter1.Name = "splitter1";
            splitter1.Size = new System.Drawing.Size(11, 274);
            splitter1.TabIndex = 1;
            splitter1.TabStop = false;
            // 
            // groupBoxSQL
            // 
            groupBoxSQL.Controls.Add(teSQL);
            groupBoxSQL.Dock = System.Windows.Forms.DockStyle.Left;
            groupBoxSQL.Location = new System.Drawing.Point(0, 0);
            groupBoxSQL.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            groupBoxSQL.Name = "groupBoxSQL";
            groupBoxSQL.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            groupBoxSQL.Size = new System.Drawing.Size(479, 274);
            groupBoxSQL.TabIndex = 0;
            groupBoxSQL.TabStop = false;
            groupBoxSQL.Text = "SQL";
            // 
            // teSQL
            // 
            teSQL.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            teSQL.Location = new System.Drawing.Point(11, 25);
            teSQL.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            teSQL.Multiline = true;
            teSQL.Name = "teSQL";
            teSQL.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            teSQL.Size = new System.Drawing.Size(456, 235);
            teSQL.TabIndex = 9;
            // 
            // splitter2
            // 
            splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            splitter2.Location = new System.Drawing.Point(0, 575);
            splitter2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            splitter2.Name = "splitter2";
            splitter2.Size = new System.Drawing.Size(1227, 12);
            splitter2.TabIndex = 24;
            splitter2.TabStop = false;
            // 
            // panelBG
            // 
            panelBG.Controls.Add(panelQuery);
            panelBG.Controls.Add(splitter4);
            panelBG.Controls.Add(groupBoxEntities);
            panelBG.Controls.Add(panelButtons);
            panelBG.Dock = System.Windows.Forms.DockStyle.Fill;
            panelBG.Location = new System.Drawing.Point(0, 0);
            panelBG.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            panelBG.Name = "panelBG";
            panelBG.Size = new System.Drawing.Size(1227, 575);
            panelBG.TabIndex = 25;
            // 
            // panelQuery
            // 
            panelQuery.Controls.Add(groupBoxColumns);
            panelQuery.Controls.Add(groupBoxConditions);
            panelQuery.Controls.Add(panelColumns);
            panelQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            panelQuery.Location = new System.Drawing.Point(250, 0);
            panelQuery.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            panelQuery.Name = "panelQuery";
            panelQuery.Size = new System.Drawing.Size(873, 575);
            panelQuery.TabIndex = 33;
            // 
            // groupBoxColumns
            // 
            groupBoxColumns.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            groupBoxColumns.Controls.Add(CPanel);
            groupBoxColumns.Location = new System.Drawing.Point(0, 5);
            groupBoxColumns.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            groupBoxColumns.Name = "groupBoxColumns";
            groupBoxColumns.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            groupBoxColumns.Size = new System.Drawing.Size(557, 219);
            groupBoxColumns.TabIndex = 33;
            groupBoxColumns.TabStop = false;
            groupBoxColumns.Text = "Query Columns";
            // 
            // CPanel
            // 
            CPanel.Active = false;
            CPanel.ActiveRowIndex = -1;
            CPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            CPanel.Appearance.ActiveBackColor = System.Drawing.Color.FromArgb(190, 225, 190);
            CPanel.Appearance.ActiveForeColor = System.Drawing.SystemColors.HighlightText;
            CPanel.Appearance.AdditionRowColor = System.Drawing.Color.DarkGreen;
            CPanel.Appearance.AttrElementFormat = "{entity} {attr}";
            CPanel.Appearance.BackColor = System.Drawing.Color.LightYellow;
            CPanel.Appearance.ButtonForeColor = System.Drawing.SystemColors.ControlText;
            CPanel.Appearance.FocusBorderStyle = System.Windows.Forms.BorderStyle.None;
            CPanel.Appearance.RowHeight = 24;
            CPanel.Appearance.TitleElementFormat = "{attr}";
            CPanel.BackColor = System.Drawing.Color.LightYellow;
            CPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            CPanel.EditMode = Korzh.EasyQuery.WinForms.ColumnsPanelEditMode.All;
            CPanel.Location = new System.Drawing.Point(9, 28);
            CPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            CPanel.Name = "CPanel";
            CPanel.Query = null;
            CPanel.Size = new System.Drawing.Size(541, 181);
            CPanel.TabIndex = 27;
            CPanel.TabStop = true;
            // 
            // groupBoxConditions
            // 
            groupBoxConditions.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            groupBoxConditions.Controls.Add(QPanel);
            groupBoxConditions.Location = new System.Drawing.Point(0, 222);
            groupBoxConditions.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            groupBoxConditions.Name = "groupBoxConditions";
            groupBoxConditions.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            groupBoxConditions.Size = new System.Drawing.Size(869, 352);
            groupBoxConditions.TabIndex = 2;
            groupBoxConditions.TabStop = false;
            groupBoxConditions.Text = "Query Conditions";
            // 
            // QPanel
            // 
            QPanel.Active = false;
            QPanel.ActiveRowIndex = -1;
            QPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            QPanel.Appearance.ActiveBackColor = System.Drawing.Color.FromArgb(190, 225, 190);
            QPanel.Appearance.ActiveForeColor = System.Drawing.SystemColors.HighlightText;
            QPanel.Appearance.AdditionRowColor = System.Drawing.Color.DarkGreen;
            QPanel.Appearance.ButtonForeColor = System.Drawing.SystemColors.ControlText;
            QPanel.Appearance.DefaultListControlType = "LISTBOX";
            QPanel.Appearance.ExprColor = System.Drawing.Color.Indigo;
            QPanel.Appearance.FocusBorderStyle = System.Windows.Forms.BorderStyle.None;
            QPanel.Appearance.OperatorColor = System.Drawing.Color.MediumBlue;
            QPanel.Appearance.RowHeight = 24;
            QPanel.Appearance.ShowRootRow = true;
            QPanel.BackColor = System.Drawing.Color.White;
            QPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            QPanel.ForeColor = System.Drawing.SystemColors.ControlText;
            QPanel.Location = new System.Drawing.Point(9, 21);
            QPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            QPanel.Name = "QPanel";
            QPanel.Query = null;
            QPanel.Size = new System.Drawing.Size(851, 324);
            QPanel.TabIndex = 27;
            QPanel.TabStop = true;
            QPanel.ListRequest += QPanel_ListRequest;
            // 
            // panelColumns
            // 
            panelColumns.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            panelColumns.Controls.Add(groupBoxSorting);
            panelColumns.Location = new System.Drawing.Point(5, 5);
            panelColumns.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            panelColumns.Name = "panelColumns";
            panelColumns.Size = new System.Drawing.Size(868, 219);
            panelColumns.TabIndex = 4;
            // 
            // groupBoxSorting
            // 
            groupBoxSorting.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            groupBoxSorting.Controls.Add(SPanel);
            groupBoxSorting.Location = new System.Drawing.Point(552, 0);
            groupBoxSorting.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            groupBoxSorting.Name = "groupBoxSorting";
            groupBoxSorting.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            groupBoxSorting.Size = new System.Drawing.Size(312, 219);
            groupBoxSorting.TabIndex = 30;
            groupBoxSorting.TabStop = false;
            groupBoxSorting.Text = "Columns Sorting";
            // 
            // SPanel
            // 
            SPanel.Active = false;
            SPanel.ActiveRowIndex = -1;
            SPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            SPanel.Appearance.ActiveBackColor = System.Drawing.Color.FromArgb(190, 225, 190);
            SPanel.Appearance.ActiveForeColor = System.Drawing.SystemColors.HighlightText;
            SPanel.Appearance.AdditionRowColor = System.Drawing.Color.DarkGreen;
            SPanel.Appearance.AttrElementFormat = "{entity} {attr}";
            SPanel.Appearance.BackColor = System.Drawing.Color.LightYellow;
            SPanel.Appearance.ButtonForeColor = System.Drawing.SystemColors.ControlText;
            SPanel.Appearance.FocusBorderStyle = System.Windows.Forms.BorderStyle.None;
            SPanel.Appearance.RowHeight = 24;
            SPanel.Appearance.TitleElementFormat = "{attr}";
            SPanel.BackColor = System.Drawing.Color.LightYellow;
            SPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            SPanel.EditMode = Korzh.EasyQuery.WinForms.ColumnsPanelEditMode.All;
            SPanel.Location = new System.Drawing.Point(8, 28);
            SPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            SPanel.Name = "SPanel";
            SPanel.Query = null;
            SPanel.Size = new System.Drawing.Size(295, 181);
            SPanel.SortEditMode = Korzh.EasyQuery.WinForms.SortingPanel.SortEditModeKind.All;
            SPanel.TabIndex = 28;
            SPanel.TabStop = true;
            // 
            // splitter4
            // 
            splitter4.BackColor = System.Drawing.SystemColors.Control;
            splitter4.Location = new System.Drawing.Point(239, 0);
            splitter4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            splitter4.Name = "splitter4";
            splitter4.Size = new System.Drawing.Size(11, 575);
            splitter4.TabIndex = 32;
            splitter4.TabStop = false;
            // 
            // groupBoxEntities
            // 
            groupBoxEntities.Controls.Add(EntPanel);
            groupBoxEntities.Dock = System.Windows.Forms.DockStyle.Left;
            groupBoxEntities.Location = new System.Drawing.Point(0, 0);
            groupBoxEntities.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            groupBoxEntities.Name = "groupBoxEntities";
            groupBoxEntities.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            groupBoxEntities.Size = new System.Drawing.Size(239, 575);
            groupBoxEntities.TabIndex = 29;
            groupBoxEntities.TabStop = false;
            groupBoxEntities.Text = "Objects and their attributes";
            // 
            // EntPanel
            // 
            EntPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            EntPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            EntPanel.ImageAddColumns = (System.Drawing.Image)resources.GetObject("EntPanel.ImageAddColumns");
            EntPanel.ImageAddConditions = (System.Drawing.Image)resources.GetObject("EntPanel.ImageAddConditions");
            EntPanel.ImageSelectAll = (System.Drawing.Image)resources.GetObject("EntPanel.ImageSelectAll");
            EntPanel.ImageSelectNone = (System.Drawing.Image)resources.GetObject("EntPanel.ImageSelectNone");
            EntPanel.Location = new System.Drawing.Point(4, 25);
            EntPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            EntPanel.Name = "EntPanel";
            EntPanel.Query = null;
            EntPanel.ShowFilter = true;
            EntPanel.Size = new System.Drawing.Size(231, 545);
            EntPanel.TabIndex = 29;
            // 
            // panelButtons
            // 
            panelButtons.Controls.Add(btClear);
            panelButtons.Controls.Add(btLoad);
            panelButtons.Controls.Add(btSave);
            panelButtons.Controls.Add(btExecute);
            panelButtons.Dock = System.Windows.Forms.DockStyle.Right;
            panelButtons.Location = new System.Drawing.Point(1123, 0);
            panelButtons.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new System.Drawing.Size(104, 575);
            panelButtons.TabIndex = 22;
            // 
            // btClear
            // 
            btClear.Location = new System.Drawing.Point(11, 25);
            btClear.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            btClear.Name = "btClear";
            btClear.Size = new System.Drawing.Size(83, 38);
            btClear.TabIndex = 12;
            btClear.Text = "Clear";
            btClear.Click += btClear_Click;
            // 
            // btLoad
            // 
            btLoad.Location = new System.Drawing.Point(11, 86);
            btLoad.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            btLoad.Name = "btLoad";
            btLoad.Size = new System.Drawing.Size(83, 38);
            btLoad.TabIndex = 11;
            btLoad.Text = "Load";
            btLoad.Click += btLoad_Click;
            // 
            // btSave
            // 
            btSave.Location = new System.Drawing.Point(11, 135);
            btSave.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            btSave.Name = "btSave";
            btSave.Size = new System.Drawing.Size(83, 38);
            btSave.TabIndex = 10;
            btSave.Text = "Save";
            btSave.Click += btSave_Click;
            // 
            // btExecute
            // 
            btExecute.Location = new System.Drawing.Point(11, 249);
            btExecute.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            btExecute.Name = "btExecute";
            btExecute.Size = new System.Drawing.Size(83, 60);
            btExecute.TabIndex = 9;
            btExecute.Text = "Fetch data";
            btExecute.Click += btExecute_Click;
            // 
            // EasyQueryForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1227, 861);
            Controls.Add(panelBG);
            Controls.Add(splitter2);
            Controls.Add(panelBottom);
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            Name = "EasyQueryForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Easy Query.NET WinForms demo";
            ((System.ComponentModel.ISupportInitialize)ResultDS).EndInit();
            panelBottom.ResumeLayout(false);
            panelExport.ResumeLayout(false);
            groupBoxResultSet.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGrid1).EndInit();
            groupBoxSQL.ResumeLayout(false);
            groupBoxSQL.PerformLayout();
            panelBG.ResumeLayout(false);
            panelQuery.ResumeLayout(false);
            groupBoxColumns.ResumeLayout(false);
            groupBoxConditions.ResumeLayout(false);
            panelColumns.ResumeLayout(false);
            groupBoxSorting.ResumeLayout(false);
            groupBoxEntities.ResumeLayout(false);
            panelButtons.ResumeLayout(false);
            ResumeLayout(false);
        }
        #endregion

    }
}
