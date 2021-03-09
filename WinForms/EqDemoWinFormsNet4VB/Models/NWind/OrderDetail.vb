Public Class OrderDetail
    Public Property OrderID As Integer
    Public Overridable Property Order As Order
    Public Property ProductID As Integer
    Public Overridable Property Product As Product
    Public Property UnitPrice As Decimal
    Public Property Quantity As Short
    Public Property Discount As Single
End Class
