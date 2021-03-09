Imports System
Imports System.Threading
Imports System.Windows.Forms

Partial Public Class MainForm
    Inherits Form

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub button1_Click(ByVal sender As Object, ByVal ev As EventArgs)
        button1.Invoke(CType(Function()
                                 button1.Text = "Loading..."
                                 button1.Enabled = False
                             End Function, MethodInvoker))
        Dim eqForm = New EasyQueryForm()
        AddHandler eqForm.FormClosed, (Function(s, e)
                                           Application.Exit()
                                       End Function)
        Hide()
        eqForm.Show()
    End Sub
End Class
