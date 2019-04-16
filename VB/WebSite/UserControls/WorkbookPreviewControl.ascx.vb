Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DevExpress.Spreadsheet
Imports System.Net
Imports DevExpress.Web.ASPxTabControl
Imports System.Web.UI.HtmlControls
Imports DevExpress.Web.ASPxClasses

Partial Public Class UserControls_WorkbookPreviewControl
	Inherits System.Web.UI.UserControl

	Public Property Workbook() As IWorkbook
	Public Property ClientInstanceName() As String
	Public Property ShowOnlyFirstWorksheet() As Boolean
	Public ReadOnly Property CanShowOnlyFirstWorksheet() As Boolean
		Get
		If ShowOnlyFirstWorksheet Then
			Return True
		End If
		Return Workbook.Worksheets.Count < 2
		End Get
	End Property

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
		If Not CanShowOnlyFirstWorksheet Then
			PrepareMultiWorksheetForPreview()
		Else
			PrepareSingleWorksheetForPreview()
		End If
	End Sub
	Private Sub PrepareSingleWorksheetForPreview()
		PrepareIframeProperties(0, Me.previewFrame0)
		Me.previewFrame0.Attributes.Add("style", "border: 2px solid rgb(224, 224, 224);")
	End Sub
	Private Sub PrepareMultiWorksheetForPreview()
		Dim count As Integer = Workbook.Worksheets.Count
		ASPxPageControl1.ContentStyle.Paddings.Padding = Unit.Pixel(1)
		For i As Integer = 0 To count - 1
			Dim tabPage As New TabPage()
			Dim sheet As Worksheet = Workbook.Worksheets(i)
			tabPage.Text = sheet.Name
			ASPxPageControl1.TabPages.Add(tabPage)
			Dim iframe As New HtmlGenericControl("IFRAME")
			iframe.ID = String.Format("previewFrame{0}", i)
			PrepareIframeProperties(i, iframe)
			Dim whiteDiv As New HtmlGenericControl("DIV")
			whiteDiv.Style.Add(HtmlTextWriterStyle.BackgroundColor, "white")
			whiteDiv.Controls.Add(iframe)
			Dim contentControl As New ContentControl()
			contentControl.Controls.Add(whiteDiv)
			tabPage.ContentCollection.Add(contentControl)
		Next i
	End Sub
	Private Sub PrepareIframeProperties(ByVal i As Integer, ByVal iframe As HtmlGenericControl)
		iframe.ClientIDMode = System.Web.UI.ClientIDMode.Static
		iframe.Attributes.Add("src", String.Format("{0}?preview={1}", Request.Url, i))
		iframe.Attributes.Add("width", "100%")
		iframe.Attributes.Add("height", "500px")
		iframe.Attributes.Add("style", "border-style: none;")
	End Sub
	Public Function CanShowPreview() As Boolean
		Return Request.QueryString.Count > 0 AndAlso Request.QueryString.ToString().StartsWith("preview")
	End Function
	Public Function RenderSpreadsheetPreview() As Boolean
		If CanShowPreview() Then
			Dim sheetIndxString As String = Request.Params("preview")
			Dim sheetIndx As Integer = If(String.IsNullOrEmpty(sheetIndxString), 0, Convert.ToInt32(sheetIndxString))
			Response.StatusCode = CInt(HttpStatusCode.OK)
			Response.ContentType = "text/html"
			Dim htmlGenerator As New HtmlContentGenerator(Response.OutputStream)
			htmlGenerator.Generate(Workbook, sheetIndx)
			Response.End()
			Return True
		End If
		Return False
	End Function
	Protected Function GetBasePath() As String
		Return String.Format("{0}?preview", Request.Url.ToString())
	End Function
End Class
