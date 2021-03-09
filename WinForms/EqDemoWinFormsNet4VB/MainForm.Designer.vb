<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Me.button1 = New System.Windows.Forms.Button()
            Me.label1 = New System.Windows.Forms.Label()
            Me.SuspendLayout()
            Me.button1.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (CByte((0))))
            Me.button1.Location = New System.Drawing.Point(143, 39)
            Me.button1.Name = "button1"
            Me.button1.Size = New System.Drawing.Size(368, 130)
            Me.button1.TabIndex = 0
            Me.button1.Text = "Open EasyQuery Form"
            Me.button1.UseVisualStyleBackColor = True
            AddHandler Me.button1.Click, New System.EventHandler(AddressOf Me.button1_Click)
            Me.label1.Location = New System.Drawing.Point(140, 185)
            Me.label1.Name = "label1"
            Me.label1.Size = New System.Drawing.Size(371, 61)
            Me.label1.TabIndex = 1
        Me.label1.Text = "(we placed all EasyQuery functionality to a separate form to make it easier to co" & "py it to your own project)"
        Me.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8F, 16F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(634, 301)
            Me.Controls.Add(Me.label1)
            Me.Controls.Add(Me.button1)
            Me.Name = "MainForm"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "MainForm"
            Me.ResumeLayout(False)
        End Sub

        Private button1 As System.Windows.Forms.Button
        Private label1 As System.Windows.Forms.Label

End Class


