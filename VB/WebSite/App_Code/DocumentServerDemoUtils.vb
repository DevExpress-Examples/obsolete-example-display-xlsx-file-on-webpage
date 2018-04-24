Imports System.IO
Imports DevExpress.Spreadsheet

Public NotInheritable Class StreamCopyHelper

    Private Sub New()
    End Sub

    Public Shared Sub Copy(ByVal src As Stream, ByVal dst As Stream)
        Const bufferSize As Integer = 32768
        Dim buffer(bufferSize - 1) As Byte
        Dim bytesRead As Integer = 0
        Do
            bytesRead = src.Read(buffer, 0, bufferSize)
            dst.Write(buffer, 0, bytesRead)

        Loop While bytesRead = bufferSize
    End Sub
End Class

Public Class HtmlContentGenerator
    Private dstStream As Stream
    Public Sub New(ByVal dstStream As Stream)
        Me.dstStream = dstStream
    End Sub
    Public Sub Generate(ByVal book As IWorkbook, ByVal sheetIndx As Integer)
        Dim model As DevExpress.XtraSpreadsheet.Model.DocumentModel = book.Model
        Dim tempStream As New MemoryStream()
        Dim options As New DevExpress.XtraSpreadsheet.Export.HtmlDocumentExporterOptions()
        options.SheetIndex = sheetIndx
        options.EmbedImages = True
        model.InternalAPI.SaveDocumentHtmlContent(tempStream, options)
        tempStream.Seek(0, SeekOrigin.Begin)
        StreamCopyHelper.Copy(tempStream, Me.dstStream)
    End Sub
    Public Sub Generate(ByVal book As IWorkbook)
        Generate(book, 0)
    End Sub
End Class