Imports System

<System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")>
<System.Diagnostics.DebuggerNonUserCodeAttribute()>
<System.Runtime.CompilerServices.CompilerGeneratedAttribute()>
Friend Class EasyQueryForm_Resources
    Private Shared resourceMan As System.Resources.ResourceManager
    Private Shared resourceCulture As System.Globalization.CultureInfo

    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")>
    Friend Sub New()
    End Sub

    <System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)>
    Friend Shared ReadOnly Property ResourceManager As System.Resources.ResourceManager
        Get

            If Object.ReferenceEquals(resourceMan, Nothing) Then
                Dim temp As System.Resources.ResourceManager = New System.Resources.ResourceManager("EasyQueryForm.EasyQueryForm.Resources", GetType(EasyQueryForm_Resources).Assembly)
                resourceMan = temp
            End If

            Return resourceMan
        End Get
    End Property

    <System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)>
    Friend Shared Property Culture As System.Globalization.CultureInfo
        Get
            Return resourceCulture
        End Get
        Set(ByVal value As System.Globalization.CultureInfo)
            resourceCulture = value
        End Set
    End Property

    Friend Shared ReadOnly Property btnExportCsv_Image As System.Drawing.Bitmap
        Get
            Dim obj As Object = ResourceManager.GetObject("btnExportCsv.Image", resourceCulture)
            Return (CType((obj), System.Drawing.Bitmap))
        End Get
    End Property

    Friend Shared ReadOnly Property btnExportExel_Image As System.Drawing.Bitmap
        Get
            Dim obj As Object = ResourceManager.GetObject("btnExportExel.Image", resourceCulture)
            Return (CType((obj), System.Drawing.Bitmap))
        End Get
    End Property

    Friend Shared ReadOnly Property contextMenu1_TrayLocation As System.Drawing.Point
        Get
            Dim obj As Object = ResourceManager.GetObject("contextMenu1.TrayLocation", resourceCulture)
            Return (CType((obj), System.Drawing.Point))
        End Get
    End Property

    Friend Shared ReadOnly Property dataModel1_TrayLocation As System.Drawing.Point
        Get
            Dim obj As Object = ResourceManager.GetObject("dataModel1.TrayLocation", resourceCulture)
            Return (CType((obj), System.Drawing.Point))
        End Get
    End Property

    Friend Shared ReadOnly Property EntPanel_ImageAddColumns As System.Drawing.Bitmap
        Get
            Dim obj As Object = ResourceManager.GetObject("EntPanel.ImageAddColumns", resourceCulture)
            Return (CType((obj), System.Drawing.Bitmap))
        End Get
    End Property

    Friend Shared ReadOnly Property EntPanel_ImageAddConditions As System.Drawing.Bitmap
        Get
            Dim obj As Object = ResourceManager.GetObject("EntPanel.ImageAddConditions", resourceCulture)
            Return (CType((obj), System.Drawing.Bitmap))
        End Get
    End Property

    Friend Shared ReadOnly Property EntPanel_ImageSelectAll As System.Drawing.Bitmap
        Get
            Dim obj As Object = ResourceManager.GetObject("EntPanel.ImageSelectAll", resourceCulture)
            Return (CType((obj), System.Drawing.Bitmap))
        End Get
    End Property

    Friend Shared ReadOnly Property EntPanel_ImageSelectNone As System.Drawing.Bitmap
        Get
            Dim obj As Object = ResourceManager.GetObject("EntPanel.ImageSelectNone", resourceCulture)
            Return (CType((obj), System.Drawing.Bitmap))
        End Get
    End Property

    Friend Shared ReadOnly Property labelDbTypeHint_Text As String
        Get
            Return ResourceManager.GetString("labelDbTypeHint.Text", resourceCulture)
        End Get
    End Property

    Friend Shared ReadOnly Property openFileDlg_TrayLocation As System.Drawing.Point
        Get
            Dim obj As Object = ResourceManager.GetObject("openFileDlg.TrayLocation", resourceCulture)
            Return (CType((obj), System.Drawing.Point))
        End Get
    End Property

    Friend Shared ReadOnly Property query1_TrayLocation As System.Drawing.Point
        Get
            Dim obj As Object = ResourceManager.GetObject("query1.TrayLocation", resourceCulture)
            Return (CType((obj), System.Drawing.Point))
        End Get
    End Property

    Friend Shared ReadOnly Property ResultDS_TrayLocation As System.Drawing.Point
        Get
            Dim obj As Object = ResourceManager.GetObject("ResultDS.TrayLocation", resourceCulture)
            Return (CType((obj), System.Drawing.Point))
        End Get
    End Property

    Friend Shared ReadOnly Property saveFileDlg_TrayLocation As System.Drawing.Point
        Get
            Dim obj As Object = ResourceManager.GetObject("saveFileDlg.TrayLocation", resourceCulture)
            Return (CType((obj), System.Drawing.Point))
        End Get
    End Property

    Friend Shared ReadOnly Property toolTipCsv_TrayLocation As System.Drawing.Point
        Get
            Dim obj As Object = ResourceManager.GetObject("toolTipCsv.TrayLocation", resourceCulture)
            Return (CType((obj), System.Drawing.Point))
        End Get
    End Property

    Friend Shared ReadOnly Property toolTipExel_TrayLocation As System.Drawing.Point
        Get
            Dim obj As Object = ResourceManager.GetObject("toolTipExel.TrayLocation", resourceCulture)
            Return (CType((obj), System.Drawing.Point))
        End Get
    End Property
End Class