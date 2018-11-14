Imports System.IO
Imports DevExpress.Spreadsheet
Imports DevExpress.XtraSpreadsheet.Export

Public Class HtmlContentGenerator
    Private stream As Stream
    Public Sub New(ByVal stream As Stream)
        Me.stream = stream
    End Sub
    Public Sub Generate(ByVal workbook As IWorkbook, ByVal sheetIndex As Integer)
        Using ms As New MemoryStream()
            Dim options As HtmlDocumentExporterOptions = New HtmlDocumentExporterOptions With { _
                .SheetIndex = sheetIndex, _
                .EmbedImages = True, _
                .AnchorImagesToPage = True _
            }
            workbook.ExportToHtml(ms, options)
            ms.Seek(0, SeekOrigin.Begin)
            ms.CopyTo(Me.stream)
        End Using
    End Sub
    Public Sub Generate(ByVal workbook As IWorkbook)
        Generate(workbook, 0)
    End Sub
End Class