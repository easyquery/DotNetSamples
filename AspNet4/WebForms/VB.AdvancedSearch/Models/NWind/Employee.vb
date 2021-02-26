Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports Korzh.EasyQuery


<DisplayColumn("FirstName")>
Public Class Employee
    <DatabaseGenerated(DatabaseGeneratedOption.None)>
    <Column("EmployeeID")>
    Public Property Id As Integer
    <Required>
    <Display(Name:="Last name")>
    Public Property LastName As String
    <Required>
    <Display(Name:="First name")>
    Public Property FirstName As String

    <NotMapped>
    Public ReadOnly Property FullName As String
        Get
            Dim res As String = Me.FirstName
            If Not String.IsNullOrEmpty(res) Then res += " "
            If Not String.IsNullOrEmpty(Me.LastName) Then res += Me.LastName
            Return res
        End Get
    End Property

    <MaxLength(30)>
    Public Property Title As String
    Public Property TitleOfCourtesy As String
    <Display(Name:="Birth date")>
    Public Property BirthDate As DateTime?
    Public Property HireDate As DateTime?
    Public Property Address As String
    Public Property City As String
    Public Property Region As String
    Public Property PostalCode As String
    <EqListValueEditor>
    Public Property Country As String
    <MaxLength(24)>
    Public Property HomePhone As String
    <MaxLength(4)>
    Public Property Extension As String
    <ScaffoldColumn(False)>
    Public Property Photo As Byte()
    Public Property PhotoPath As String
    Public Property Notes As String
    <ScaffoldColumn(False)>
    Public Property ReportsTo As Integer?
    <ForeignKey("ReportsTo")>
    Public Overridable Property Manager As Employee
    Public Overridable Property Orders As ICollection(Of Order)
End Class

