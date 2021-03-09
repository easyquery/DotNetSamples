Imports System
Imports System.Diagnostics
Imports System.Configuration
Imports System.Data
Imports System.Data.Entity.Migrations
Imports System.IO
Imports System.Windows.Forms

Imports Microsoft.Data.SqlClient

Imports EasyData.Export
Imports Korzh.EasyQuery
Imports Korzh.EasyQuery.Db
Imports Korzh.EasyQuery.EntityFramework
Imports Korzh.EasyQuery.Services
Imports Korzh.EasyQuery.WinForms

Partial Public Class EasyQueryForm
    Inherits Form

    Private openFileDlg As System.Windows.Forms.OpenFileDialog
    Private saveFileDlg As System.Windows.Forms.SaveFileDialog
    Private ResultDS As System.Data.DataSet
    Private ResultDataTable As System.Data.DataTable
    Private panelBottom As System.Windows.Forms.Panel
    Private groupBoxSQL As System.Windows.Forms.GroupBox
    Private splitter1 As System.Windows.Forms.Splitter
    Private groupBoxResultSet As System.Windows.Forms.GroupBox
    Private teSQL As System.Windows.Forms.TextBox
    Private dataGrid1 As System.Windows.Forms.DataGrid
    Private splitter2 As System.Windows.Forms.Splitter
    Private panelBG As System.Windows.Forms.Panel
    Private panelButtons As System.Windows.Forms.Panel
    Private btClear As System.Windows.Forms.Button
    Private btLoad As System.Windows.Forms.Button
    Private btSave As System.Windows.Forms.Button
    Private btExecute As System.Windows.Forms.Button
    Private groupBoxEntities As GroupBox
    Private panelQuery As System.Windows.Forms.Panel
    Private groupBoxColumns As System.Windows.Forms.GroupBox
    Private groupBoxConditions As System.Windows.Forms.GroupBox
    Private panelColumns As System.Windows.Forms.Panel
    Private groupBoxSorting As System.Windows.Forms.GroupBox
    Private splitter4 As System.Windows.Forms.Splitter
    Private EntPanel As Korzh.EasyQuery.WinForms.EntitiesPanel
    Private QPanel As Korzh.EasyQuery.WinForms.QueryPanel
    Private CPanel As Korzh.EasyQuery.WinForms.ColumnsPanel
    Private SPanel As Korzh.EasyQuery.WinForms.SortingPanel
    Private toolTipExel As ToolTip
    Private toolTipCsv As ToolTip
    Private panelExport As GroupBox
    Private btnExportCsv As Button
    Private btnExportExel As Button
    Private ReadOnly _dataFolder As String = "App_Data"
    Private ReadOnly _appDirectory As String
    Private _connection As SqlConnection
    Private _countryAttr As EntityAttr = Nothing
    Private EqManager As EasyQueryManagerSql

    Public Sub New()
        Korzh.EasyQuery.WinForms.License.Key = "M-Vm5PXqfpFr0P6bDruZ2wQIS6HV2Y"
        _appDirectory = System.IO.Directory.GetCurrentDirectory()
        _dataFolder = System.IO.Path.Combine(_appDirectory, "App_Data")
        InitializeComponent()
        InitEasyQuery()
        HideExportPanel()
        Dim dbConnectTimer = New Timer()
        AddHandler dbConnectTimer.Tick, New EventHandler(AddressOf TimerEventProcessor)
        dbConnectTimer.Interval = 100
        dbConnectTimer.Start()
    End Sub

    Private Sub InitEasyQuery()
        Dim options = New EasyQueryOptions()
        EqManager = New EasyQueryManagerSql(options)
        EqManager.Model.LoadFromDbContext(ApplicationDbContext.Create())
        _countryAttr = EqManager.Model.EntityRoot.FindAttributeById("Customers.Country")
        QPanel.Query = EqManager.Query
        CPanel.Query = EqManager.Query
        SPanel.Query = EqManager.Query
        EntPanel.Query = EqManager.Query
        Me.CPanel.AllowEditCaptions = True
        Me.CPanel.AllowSorting = True
        Me.EntPanel.ShowFilter = True
    End Sub

    Private Sub TimerEventProcessor(ByVal myObject As Object, ByVal myEventArgs As EventArgs)
        CheckConnection()
    End Sub

    Private Sub CheckConnection()
        Dim prevTitle = Me.Text
        Me.Text += " (openning the connection to DB...)"

        Try

            If _connection Is Nothing Then
                Dim currentDir As String = Directory.GetCurrentDirectory()
                Dim connectionString = ConfigurationManager.ConnectionStrings("DefaultConnection")?.ToString()
                _connection = New SqlConnection(connectionString)
                Dim migrator = New DbMigrator(New Migrations.Configuration())
                migrator.Update()
            End If

            If _connection.State <> ConnectionState.Open Then
                _connection.Open()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        Me.Text = prevTitle
    End Sub

    Private Sub btClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        EqManager.Query.Clear()
        EntPanel.ClearFilter()
        teSQL.Clear()
        dataGrid1.DataSource = Nothing
        HideExportPanel()
    End Sub

    Private Sub btLoad_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        openFileDlg.Filter = "JSON files (*.json)|*.json|XML files (*.xml)|*.xml"
        openFileDlg.FilterIndex = 1
        openFileDlg.InitialDirectory = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "App_Data")

        If openFileDlg.ShowDialog() = DialogResult.OK Then

            If openFileDlg.FilterIndex = 1 Then
                EqManager.Query.LoadFromJsonFile(openFileDlg.FileName)
            Else
                EqManager.Query.LoadFromXmlFile(openFileDlg.FileName)
            End If
        End If
    End Sub

    Private Sub btSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        saveFileDlg.Filter = "JSON files (*.json)|*.json|XML files (*.xml)|*.xml"
        saveFileDlg.FilterIndex = 1
        saveFileDlg.AddExtension = True
        openFileDlg.InitialDirectory = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "App_Data")
        saveFileDlg.FileName = System.IO.Path.Combine(_dataFolder, "queries\query")

        If saveFileDlg.ShowDialog() = DialogResult.OK Then

            If saveFileDlg.FilterIndex = 1 Then
                EqManager.Query.SaveToJsonFile(saveFileDlg.FileName)
            Else
                EqManager.Query.SaveToXmlFile(saveFileDlg.FileName)
            End If
        End If
    End Sub

    Private Sub btExecute_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            ResultDS.Reset()
            Dim builder = BuildSQL()
            Dim builderResult = builder.Result
            CheckConnection()
            Dim command = _connection.CreateCommand()
            command.CommandText = builderResult.Statement

            For Each param As QueryParam In builderResult.Params
                command.Parameters.Add(New SqlParameter("@" & param.Id, param.Value))
            Next

            Dim resultDA = New SqlDataAdapter(command)
            resultDA.Fill(ResultDS, "Result")
            dataGrid1.DataSource = ResultDS.Tables(0).DefaultView
            _connection.Close()
            ShowExportPanel()
        Catch [error] As Exception
            MessageBox.Show([error].Message)
        End Try
    End Sub

    Private Function BuildSQL() As IQueryBuilder
        teSQL.Clear()

        Try
            EqManager.QueryBuilder.Formats.SetDefaultFormats(FormatType.MsSqlServer)

            If EqManager.QueryBuilder.CanBuild Then
                EqManager.QueryBuilder.BuildParamSQL()
                Dim sql As String = EqManager.QueryBuilder.Result.SQL
                teSQL.Text = sql
                Return EqManager.QueryBuilder
            End If

            Return Nothing
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        Return Nothing
    End Function

    Private Sub QPanel_ListRequest(ByVal sender As Object, ByVal e As ListRequestEventArgs)
        If e.ListName = "SQL" Then
            CheckConnection()
            Dim sql = e.Data.ToString()
            Dim tempDS As DataSet = New DataSet()
            Dim tempDA = New SqlDataAdapter(sql, _connection)
            tempDA.Fill(tempDS, "Temp")
            Dim strWriter = New StringWriter()
            tempDS.WriteXml(strWriter)
            e.ResultXml = strWriter.ToString()
        ElseIf e.ListName = "RegionList" Then
            e.ListItems.Clear()
            Dim country As String = EqManager.Query.GetOneValueForAttr(_countryAttr)

            If country = "Canada" OrElse country Is Nothing Then
                e.ListItems.Add("British Columbia", "BC")
                e.ListItems.Add("Quebec", "Quebec")
            End If

            If country = "USA" OrElse country Is Nothing Then
                e.ListItems.Add("California", "CA")
                e.ListItems.Add("Colorado", "CO")
                e.ListItems.Add("Oregon", "OR")
                e.ListItems.Add("Washington", "WA")
            End If
        End If
    End Sub

    Private Sub query1_ColumnsChanged(ByVal sender As Object, ByVal e As QueryColumnsChangeEventArgs)
        BuildSQL()
        ResultDS.Reset()
    End Sub

    Private Sub query1_ConditionsChanged(ByVal sender As Object, ByVal e As ConditionsChangeEventArgs)
        Dim baseAttr As EntityAttr = Nothing
        If e.Condition IsNot Nothing Then baseAttr = e.Condition.BaseAttr


        ' If baseAttr IsNot Nothing AndAlso baseAttr = _countryAttr Then
        '   QPanel.RefreshList("RegionList")
        ' End If

        BuildSQL()
        ResultDS.Reset()
    End Sub

    Private Sub QPanel_ValueRequest(ByVal sender As Object, ByVal e As ValueRequestEventArgs)
        MessageBox.Show(e.Data)
    End Sub

    Private Sub QPanel_ConditionRender(ByVal sender As Object, ByVal e As ConditionRenderEventArgs)
    End Sub

    Private Sub CloseConnections()
        If _connection IsNot Nothing Then _connection.Close()
    End Sub

    Private Sub btnExportCsv_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try

            Using saveFileDialog As SaveFileDialog = New SaveFileDialog()
                saveFileDialog.Filter = "csv files (*.csv)|*.csv"
                saveFileDialog.FilterIndex = 2
                saveFileDialog.RestoreDirectory = True

                If saveFileDialog.ShowDialog(Me) = DialogResult.OK Then
                    ExportData(New CsvDataExporter(), saveFileDialog.FileName)
                End If
            End Using

        Catch [error] As Exception
            MessageBox.Show([error].Message)
        End Try
    End Sub

    Private Sub btnExportXls_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try

            Using saveFileDialog As SaveFileDialog = New SaveFileDialog()
                saveFileDialog.Filter = "xlsx files (*.xlsx)|*.xlsx"
                saveFileDialog.FilterIndex = 2
                saveFileDialog.RestoreDirectory = True

                If saveFileDialog.ShowDialog(Me) = DialogResult.OK Then
                    ExportData(New ExcelDataExporter(), saveFileDialog.FileName)
                End If
            End Using

        Catch [error] As Exception
            MessageBox.Show([error].Message)
        End Try
    End Sub

    Private Sub ExportData(ByVal exporter As IDataExporter, ByVal fileName As String)
        Using resultSet = New EasyDbResultSet(EqManager.Query, ResultDS.Tables(0).CreateDataReader(), EqManager.ResultSetOptions)

            Using fileStream = File.OpenWrite(fileName)
                exporter.Export(resultSet, fileStream)
            End Using
        End Using

        Process.Start(fileName)
    End Sub

    Private Sub ShowExportPanel()
        Me.panelExport.Show()
        Me.groupBoxResultSet.Width = Me.groupBoxResultSet.Parent.ClientSize.Width - Me.panelExport.Width - Me.groupBoxResultSet.Left - 4
    End Sub

    Private Sub HideExportPanel()
        Me.panelExport.Hide()
        Me.groupBoxResultSet.Width = Me.groupBoxResultSet.Parent.ClientSize.Width - Me.groupBoxResultSet.Left - 4
    End Sub
End Class