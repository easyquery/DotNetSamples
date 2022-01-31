<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EasyQueryForm
    Inherits System.Windows.Forms.Form

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer = Nothing

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso (components IsNot Nothing) Then
            components.Dispose()
        End If

        MyBase.Dispose(disposing)
    End Sub

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(EasyQueryForm))
        Me.ResultDataTable = New System.Data.DataTable()
        Me.openFileDlg = New System.Windows.Forms.OpenFileDialog()
        Me.saveFileDlg = New System.Windows.Forms.SaveFileDialog()
        Me.ResultDS = New System.Data.DataSet()
        Me.panelBottom = New System.Windows.Forms.Panel()
        Me.panelExport = New System.Windows.Forms.GroupBox()
        Me.btnExportCsv = New System.Windows.Forms.Button()
        Me.btnExportExel = New System.Windows.Forms.Button()
        Me.groupBoxResultSet = New System.Windows.Forms.GroupBox()
        Me.dataGrid1 = New System.Windows.Forms.DataGrid()
        Me.splitter1 = New System.Windows.Forms.Splitter()
        Me.groupBoxSQL = New System.Windows.Forms.GroupBox()
        Me.teSQL = New System.Windows.Forms.TextBox()
        Me.splitter2 = New System.Windows.Forms.Splitter()
        Me.panelBG = New System.Windows.Forms.Panel()
        Me.panelQuery = New System.Windows.Forms.Panel()
        Me.groupBoxColumns = New System.Windows.Forms.GroupBox()
        Me.CPanel = New Korzh.EasyQuery.WinForms.ColumnsPanel()
        Me.groupBoxConditions = New System.Windows.Forms.GroupBox()
        Me.QPanel = New Korzh.EasyQuery.WinForms.QueryPanel()
        Me.panelColumns = New System.Windows.Forms.Panel()
        Me.groupBoxSorting = New System.Windows.Forms.GroupBox()
        Me.SPanel = New Korzh.EasyQuery.WinForms.SortingPanel()
        Me.splitter4 = New System.Windows.Forms.Splitter()
        Me.groupBoxEntities = New System.Windows.Forms.GroupBox()
        Me.EntPanel = New Korzh.EasyQuery.WinForms.EntitiesPanel()
        Me.panelButtons = New System.Windows.Forms.Panel()
        Me.btClear = New System.Windows.Forms.Button()
        Me.btLoad = New System.Windows.Forms.Button()
        Me.btSave = New System.Windows.Forms.Button()
        Me.btExecute = New System.Windows.Forms.Button()
        Me.toolTipExel = New System.Windows.Forms.ToolTip(Me.components)
        Me.toolTipCsv = New System.Windows.Forms.ToolTip(Me.components)
        Me.ResultDataTable.BeginInit()
        Me.ResultDS.BeginInit()
        Me.panelBottom.SuspendLayout()
        Me.panelExport.SuspendLayout()
        Me.groupBoxResultSet.SuspendLayout()
        Me.dataGrid1.BeginInit()
        Me.groupBoxSQL.SuspendLayout()
        Me.panelBG.SuspendLayout()
        Me.panelQuery.SuspendLayout()
        Me.groupBoxColumns.SuspendLayout()
        Me.groupBoxConditions.SuspendLayout()
        Me.panelColumns.SuspendLayout()
        Me.groupBoxSorting.SuspendLayout()
        Me.groupBoxEntities.SuspendLayout()
        Me.panelButtons.SuspendLayout()
        Me.SuspendLayout()
        Me.ResultDataTable.TableName = "Result"
        Me.ResultDS.DataSetName = "ResultDataSet"
        Me.ResultDS.Locale = New System.Globalization.CultureInfo("en")
        Me.ResultDS.Tables.AddRange(New System.Data.DataTable() {Me.ResultDataTable})
        Me.panelBottom.Controls.Add(Me.panelExport)
        Me.panelBottom.Controls.Add(Me.groupBoxResultSet)
        Me.panelBottom.Controls.Add(Me.splitter1)
        Me.panelBottom.Controls.Add(Me.groupBoxSQL)
        Me.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.panelBottom.Location = New System.Drawing.Point(0, 470)
        Me.panelBottom.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.panelBottom.Name = "panelBottom"
        Me.panelBottom.Size = New System.Drawing.Size(1227, 219)
        Me.panelBottom.TabIndex = 23
        Me.panelExport.Controls.Add(Me.btnExportCsv)
        Me.panelExport.Controls.Add(Me.btnExportExel)
        Me.panelExport.Dock = System.Windows.Forms.DockStyle.Right
        Me.panelExport.Location = New System.Drawing.Point(1183, 0)
        Me.panelExport.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.panelExport.Name = "panelExport"
        Me.panelExport.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.panelExport.Size = New System.Drawing.Size(44, 219)
        Me.panelExport.TabIndex = 2
        Me.panelExport.TabStop = False
        Me.btnExportCsv.Image = My.Resources.EasyQueryForm_Resources.btnExportCsv_Image
        Me.btnExportCsv.Location = New System.Drawing.Point(3, 63)
        Me.btnExportCsv.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnExportCsv.Name = "btnExportCsv"
        Me.btnExportCsv.Size = New System.Drawing.Size(37, 34)
        Me.btnExportCsv.TabIndex = 1
        Me.btnExportCsv.Tag = ""
        Me.toolTipCsv.SetToolTip(Me.btnExportCsv, "Export to CSV")
        Me.btnExportCsv.UseVisualStyleBackColor = True
        AddHandler Me.btnExportCsv.Click, New System.EventHandler(AddressOf Me.btnExportCsv_Click)
        Me.btnExportExel.Image = My.Resources.EasyQueryForm_Resources.btnExportExel_Image
        Me.btnExportExel.Location = New System.Drawing.Point(3, 20)
        Me.btnExportExel.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnExportExel.Name = "btnExportExel"
        Me.btnExportExel.Size = New System.Drawing.Size(37, 34)
        Me.btnExportExel.TabIndex = 0
        Me.btnExportExel.Tag = ""
        Me.toolTipExel.SetToolTip(Me.btnExportExel, "Export to Excel")
        Me.btnExportExel.UseVisualStyleBackColor = True
        AddHandler Me.btnExportExel.Click, New System.EventHandler(AddressOf Me.btnExportXls_Click)
        Me.groupBoxResultSet.BackColor = System.Drawing.SystemColors.Control
        Me.groupBoxResultSet.Controls.Add(Me.dataGrid1)
        Me.groupBoxResultSet.Dock = System.Windows.Forms.DockStyle.Fill
        Me.groupBoxResultSet.Location = New System.Drawing.Point(490, 0)
        Me.groupBoxResultSet.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.groupBoxResultSet.Name = "groupBoxResultSet"
        Me.groupBoxResultSet.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.groupBoxResultSet.Size = New System.Drawing.Size(737, 219)
        Me.groupBoxResultSet.TabIndex = 2
        Me.groupBoxResultSet.TabStop = False
        Me.groupBoxResultSet.Text = "Result set"
        Me.dataGrid1.Anchor = (CType(((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles))
        Me.dataGrid1.DataMember = ""
        Me.dataGrid1.DataSource = Me.ResultDataTable
        Me.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.dataGrid1.Location = New System.Drawing.Point(4, 20)
        Me.dataGrid1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.dataGrid1.Name = "dataGrid1"
        Me.dataGrid1.Size = New System.Drawing.Size(732, 197)
        Me.dataGrid1.TabIndex = 1
        Me.splitter1.Location = New System.Drawing.Point(479, 0)
        Me.splitter1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.splitter1.Name = "splitter1"
        Me.splitter1.Size = New System.Drawing.Size(11, 219)
        Me.splitter1.TabIndex = 1
        Me.splitter1.TabStop = False
        Me.groupBoxSQL.Controls.Add(Me.teSQL)
        Me.groupBoxSQL.Dock = System.Windows.Forms.DockStyle.Left
        Me.groupBoxSQL.Location = New System.Drawing.Point(0, 0)
        Me.groupBoxSQL.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.groupBoxSQL.Name = "groupBoxSQL"
        Me.groupBoxSQL.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.groupBoxSQL.Size = New System.Drawing.Size(479, 219)
        Me.groupBoxSQL.TabIndex = 0
        Me.groupBoxSQL.TabStop = False
        Me.groupBoxSQL.Text = "SQL"
        Me.teSQL.Anchor = (CType(((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles))
        Me.teSQL.Location = New System.Drawing.Point(11, 20)
        Me.teSQL.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.teSQL.Multiline = True
        Me.teSQL.Name = "teSQL"
        Me.teSQL.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.teSQL.Size = New System.Drawing.Size(456, 189)
        Me.teSQL.TabIndex = 9
        Me.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.splitter2.Location = New System.Drawing.Point(0, 460)
        Me.splitter2.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.splitter2.Name = "splitter2"
        Me.splitter2.Size = New System.Drawing.Size(1227, 10)
        Me.splitter2.TabIndex = 24
        Me.splitter2.TabStop = False
        Me.panelBG.Controls.Add(Me.panelQuery)
        Me.panelBG.Controls.Add(Me.splitter4)
        Me.panelBG.Controls.Add(Me.groupBoxEntities)
        Me.panelBG.Controls.Add(Me.panelButtons)
        Me.panelBG.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelBG.Location = New System.Drawing.Point(0, 0)
        Me.panelBG.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.panelBG.Name = "panelBG"
        Me.panelBG.Size = New System.Drawing.Size(1227, 460)
        Me.panelBG.TabIndex = 25
        Me.panelQuery.Controls.Add(Me.groupBoxColumns)
        Me.panelQuery.Controls.Add(Me.groupBoxConditions)
        Me.panelQuery.Controls.Add(Me.panelColumns)
        Me.panelQuery.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelQuery.Location = New System.Drawing.Point(250, 0)
        Me.panelQuery.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.panelQuery.Name = "panelQuery"
        Me.panelQuery.Size = New System.Drawing.Size(873, 460)
        Me.panelQuery.TabIndex = 33
        Me.groupBoxColumns.Anchor = (CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles))
        Me.groupBoxColumns.Controls.Add(Me.CPanel)
        Me.groupBoxColumns.Location = New System.Drawing.Point(0, 4)
        Me.groupBoxColumns.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.groupBoxColumns.Name = "groupBoxColumns"
        Me.groupBoxColumns.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.groupBoxColumns.Size = New System.Drawing.Size(557, 175)
        Me.groupBoxColumns.TabIndex = 33
        Me.groupBoxColumns.TabStop = False
        Me.groupBoxColumns.Text = "Query Columns"
        Me.CPanel.Active = False
        Me.CPanel.ActiveRowIndex = -1
        Me.CPanel.Anchor = (CType(((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles))
        Me.CPanel.Appearance.ActiveBackColor = System.Drawing.Color.FromArgb((CInt(((CByte((190)))))), (CInt(((CByte((225)))))), (CInt(((CByte((190)))))))
        Me.CPanel.Appearance.ActiveForeColor = System.Drawing.SystemColors.HighlightText
        Me.CPanel.Appearance.AdditionRowColor = System.Drawing.Color.DarkGreen
        Me.CPanel.Appearance.AttrElementFormat = "{entity} {attr}"
        Me.CPanel.Appearance.BackColor = System.Drawing.Color.LightYellow
        Me.CPanel.Appearance.ButtonForeColor = System.Drawing.SystemColors.ControlText
        Me.CPanel.Appearance.FocusBorderStyle = System.Windows.Forms.BorderStyle.None
        Me.CPanel.Appearance.TitleElementFormat = "{attr}"
        Me.CPanel.BackColor = System.Drawing.Color.LightYellow
        Me.CPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.CPanel.EditMode = Korzh.EasyQuery.WinForms.ColumnsPanelEditMode.All
        Me.CPanel.Location = New System.Drawing.Point(9, 22)
        Me.CPanel.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.CPanel.Name = "CPanel"
        Me.CPanel.Query = Nothing
        Me.CPanel.Size = New System.Drawing.Size(541, 145)
        Me.CPanel.TabIndex = 27
        Me.CPanel.TabStop = True
        Me.groupBoxConditions.Anchor = (CType(((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles))
        Me.groupBoxConditions.Controls.Add(Me.QPanel)
        Me.groupBoxConditions.Location = New System.Drawing.Point(0, 178)
        Me.groupBoxConditions.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.groupBoxConditions.Name = "groupBoxConditions"
        Me.groupBoxConditions.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.groupBoxConditions.Size = New System.Drawing.Size(869, 282)
        Me.groupBoxConditions.TabIndex = 2
        Me.groupBoxConditions.TabStop = False
        Me.groupBoxConditions.Text = "Query Conditions"
        Me.QPanel.Active = False
        Me.QPanel.ActiveRowIndex = -1
        Me.QPanel.Anchor = (CType(((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles))
        Me.QPanel.Appearance.ActiveBackColor = System.Drawing.Color.FromArgb((CInt(((CByte((190)))))), (CInt(((CByte((225)))))), (CInt(((CByte((190)))))))
        Me.QPanel.Appearance.ActiveForeColor = System.Drawing.SystemColors.HighlightText
        Me.QPanel.Appearance.AdditionRowColor = System.Drawing.Color.DarkGreen
        Me.QPanel.Appearance.ButtonForeColor = System.Drawing.SystemColors.ControlText
        Me.QPanel.Appearance.DefaultListControlType = "LISTBOX"
        Me.QPanel.Appearance.ExprColor = System.Drawing.Color.Indigo
        Me.QPanel.Appearance.FocusBorderStyle = System.Windows.Forms.BorderStyle.None
        Me.QPanel.Appearance.OperatorColor = System.Drawing.Color.MediumBlue
        Me.QPanel.Appearance.ShowRootRow = True
        Me.QPanel.BackColor = System.Drawing.Color.White
        Me.QPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.QPanel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.QPanel.Location = New System.Drawing.Point(9, 17)
        Me.QPanel.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.QPanel.Name = "QPanel"
        Me.QPanel.Query = Nothing
        Me.QPanel.Size = New System.Drawing.Size(851, 260)
        Me.QPanel.TabIndex = 27
        Me.QPanel.TabStop = True
        AddHandler Me.QPanel.ListRequest, New Korzh.EasyQuery.WinForms.ListRequestEventHandler(AddressOf Me.QPanel_ListRequest)
        Me.panelColumns.Anchor = (CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles))
        Me.panelColumns.Controls.Add(Me.groupBoxSorting)
        Me.panelColumns.Location = New System.Drawing.Point(5, 4)
        Me.panelColumns.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.panelColumns.Name = "panelColumns"
        Me.panelColumns.Size = New System.Drawing.Size(868, 175)
        Me.panelColumns.TabIndex = 4
        Me.groupBoxSorting.Anchor = (CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles))
        Me.groupBoxSorting.Controls.Add(Me.SPanel)
        Me.groupBoxSorting.Location = New System.Drawing.Point(552, 0)
        Me.groupBoxSorting.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.groupBoxSorting.Name = "groupBoxSorting"
        Me.groupBoxSorting.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.groupBoxSorting.Size = New System.Drawing.Size(312, 175)
        Me.groupBoxSorting.TabIndex = 30
        Me.groupBoxSorting.TabStop = False
        Me.groupBoxSorting.Text = "Columns Sorting"
        Me.SPanel.Active = False
        Me.SPanel.ActiveRowIndex = -1
        Me.SPanel.Anchor = (CType(((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles))
        Me.SPanel.Appearance.ActiveBackColor = System.Drawing.Color.FromArgb((CInt(((CByte((190)))))), (CInt(((CByte((225)))))), (CInt(((CByte((190)))))))
        Me.SPanel.Appearance.ActiveForeColor = System.Drawing.SystemColors.HighlightText
        Me.SPanel.Appearance.AdditionRowColor = System.Drawing.Color.DarkGreen
        Me.SPanel.Appearance.AttrElementFormat = "{entity} {attr}"
        Me.SPanel.Appearance.BackColor = System.Drawing.Color.LightYellow
        Me.SPanel.Appearance.ButtonForeColor = System.Drawing.SystemColors.ControlText
        Me.SPanel.Appearance.FocusBorderStyle = System.Windows.Forms.BorderStyle.None
        Me.SPanel.Appearance.TitleElementFormat = "{attr}"
        Me.SPanel.BackColor = System.Drawing.Color.LightYellow
        Me.SPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SPanel.EditMode = Korzh.EasyQuery.WinForms.ColumnsPanelEditMode.All
        Me.SPanel.Location = New System.Drawing.Point(8, 22)
        Me.SPanel.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.SPanel.Name = "SPanel"
        Me.SPanel.Query = Nothing
        Me.SPanel.Size = New System.Drawing.Size(295, 145)
        Me.SPanel.SortEditMode = Korzh.EasyQuery.WinForms.SortingPanel.SortEditModeKind.All
        Me.SPanel.TabIndex = 28
        Me.SPanel.TabStop = True
        Me.splitter4.BackColor = System.Drawing.SystemColors.Control
        Me.splitter4.Location = New System.Drawing.Point(239, 0)
        Me.splitter4.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.splitter4.Name = "splitter4"
        Me.splitter4.Size = New System.Drawing.Size(11, 460)
        Me.splitter4.TabIndex = 32
        Me.splitter4.TabStop = False
        Me.groupBoxEntities.Controls.Add(Me.EntPanel)
        Me.groupBoxEntities.Dock = System.Windows.Forms.DockStyle.Left
        Me.groupBoxEntities.Location = New System.Drawing.Point(0, 0)
        Me.groupBoxEntities.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.groupBoxEntities.Name = "groupBoxEntities"
        Me.groupBoxEntities.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.groupBoxEntities.Size = New System.Drawing.Size(239, 460)
        Me.groupBoxEntities.TabIndex = 29
        Me.groupBoxEntities.TabStop = False
        Me.groupBoxEntities.Text = "Objects and their attributes"
        Me.EntPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.EntPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.EntPanel.ImageAddColumns = (CType((resources.GetObject("EntPanel.ImageAddColumns")), System.Drawing.Image))
        Me.EntPanel.ImageAddConditions = (CType((resources.GetObject("EntPanel.ImageAddConditions")), System.Drawing.Image))
        Me.EntPanel.ImageSelectAll = (CType((resources.GetObject("EntPanel.ImageSelectAll")), System.Drawing.Image))
        Me.EntPanel.ImageSelectNone = (CType((resources.GetObject("EntPanel.ImageSelectNone")), System.Drawing.Image))
        Me.EntPanel.Location = New System.Drawing.Point(4, 19)
        Me.EntPanel.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.EntPanel.Name = "EntPanel"
        Me.EntPanel.Query = Nothing
        Me.EntPanel.ShowFilter = True
        Me.EntPanel.Size = New System.Drawing.Size(231, 437)
        Me.EntPanel.TabIndex = 29
        Me.panelButtons.Controls.Add(Me.btClear)
        Me.panelButtons.Controls.Add(Me.btLoad)
        Me.panelButtons.Controls.Add(Me.btSave)
        Me.panelButtons.Controls.Add(Me.btExecute)
        Me.panelButtons.Dock = System.Windows.Forms.DockStyle.Right
        Me.panelButtons.Location = New System.Drawing.Point(1123, 0)
        Me.panelButtons.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.panelButtons.Name = "panelButtons"
        Me.panelButtons.Size = New System.Drawing.Size(104, 460)
        Me.panelButtons.TabIndex = 22
        Me.btClear.Location = New System.Drawing.Point(11, 20)
        Me.btClear.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btClear.Name = "btClear"
        Me.btClear.Size = New System.Drawing.Size(83, 30)
        Me.btClear.TabIndex = 12
        Me.btClear.Text = "Clear"
        AddHandler Me.btClear.Click, AddressOf Me.btClear_Click
        Me.btLoad.Location = New System.Drawing.Point(11, 69)
        Me.btLoad.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btLoad.Name = "btLoad"
        Me.btLoad.Size = New System.Drawing.Size(83, 30)
        Me.btLoad.TabIndex = 11
        Me.btLoad.Text = "Load"
        AddHandler Me.btLoad.Click, AddressOf Me.btLoad_Click
        Me.btSave.Location = New System.Drawing.Point(11, 108)
        Me.btSave.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btSave.Name = "btSave"
        Me.btSave.Size = New System.Drawing.Size(83, 30)
        Me.btSave.TabIndex = 10
        Me.btSave.Text = "Save"
        AddHandler Me.btSave.Click, AddressOf Me.btSave_Click
        Me.btExecute.Location = New System.Drawing.Point(11, 199)
        Me.btExecute.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btExecute.Name = "btExecute"
        Me.btExecute.Size = New System.Drawing.Size(83, 48)
        Me.btExecute.TabIndex = 9
        Me.btExecute.Text = "Fetch data"
        AddHandler Me.btExecute.Click, New System.EventHandler(AddressOf Me.btExecute_Click)
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8F, 16F)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1227, 689)
        Me.Controls.Add(Me.panelBG)
        Me.Controls.Add(Me.splitter2)
        Me.Controls.Add(Me.panelBottom)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Easy Query.NET WinForms demo"
        Me.ResultDataTable.EndInit()
        Me.ResultDS.EndInit()
        Me.panelBottom.ResumeLayout(False)
        Me.panelExport.ResumeLayout(False)
        Me.groupBoxResultSet.ResumeLayout(False)
        Me.dataGrid1.EndInit()
        Me.groupBoxSQL.ResumeLayout(False)
        Me.groupBoxSQL.PerformLayout()
        Me.panelBG.ResumeLayout(False)
        Me.panelQuery.ResumeLayout(False)
        Me.groupBoxColumns.ResumeLayout(False)
        Me.groupBoxConditions.ResumeLayout(False)
        Me.panelColumns.ResumeLayout(False)
        Me.groupBoxSorting.ResumeLayout(False)
        Me.groupBoxEntities.ResumeLayout(False)
        Me.panelButtons.ResumeLayout(False)
        Me.ResumeLayout(False)
    End Sub
End Class
