Imports System
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Public Class Category

    <DatabaseGenerated(DatabaseGeneratedOption.None)>
    <Column("CategoryID")>
    Public Property Id As Integer
    Public Property CategoryName As String
    Public Property Description As String

    <ScaffoldColumn(False)>
    Public Property Array As Byte()


End Class
