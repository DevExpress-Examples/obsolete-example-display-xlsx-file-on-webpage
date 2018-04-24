Imports DevExpress.Spreadsheet

Partial Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        previewControl.Workbook = GenerateBook()
        If previewControl.CanShowPreview() Then
            previewControl.RenderSpreadsheetPreview()
        End If
    End Sub

    Private Function GenerateBook() As IWorkbook
        Dim result As IWorkbook = New Workbook()
        Dim path As String = Page.MapPath("~/App_Data/EmployeeInformation_template.xlsx")
        result.LoadDocument(path)
        Return result
    End Function
End Class