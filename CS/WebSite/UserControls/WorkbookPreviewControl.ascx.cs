using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Spreadsheet;
using System.Net;
using DevExpress.Web.ASPxTabControl;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxClasses;

public partial class UserControls_WorkbookPreviewControl : System.Web.UI.UserControl {
    public IWorkbook Workbook { get; set; }
    public string ClientInstanceName { get; set; }
    public bool ShowOnlyFirstWorksheet { get; set; }
    public bool CanShowOnlyFirstWorksheet { get {
        if(ShowOnlyFirstWorksheet)
            return true;
        return Workbook.Worksheets.Count < 2;
    } }

    protected void Page_Load(object sender, EventArgs e) {
        if(!CanShowOnlyFirstWorksheet)
            PrepareMultiWorksheetForPreview();
        else
            PrepareSingleWorksheetForPreview();
    }
    void PrepareSingleWorksheetForPreview() {
        PrepareIframeProperties(0, this.previewFrame0);
        this.previewFrame0.Attributes.Add("style", "border: 2px solid rgb(224, 224, 224);");
    }
    void PrepareMultiWorksheetForPreview() {
        int count = Workbook.Worksheets.Count;
        ASPxPageControl1.ContentStyle.Paddings.Padding = Unit.Pixel(1);
        for(int i = 0; i < count; i++) {
            TabPage tabPage = new TabPage();
            Worksheet sheet = Workbook.Worksheets[i];
            tabPage.Text = sheet.Name;
            ASPxPageControl1.TabPages.Add(tabPage);
            HtmlGenericControl iframe = new HtmlGenericControl("IFRAME");
            iframe.ID = String.Format("previewFrame{0}", i);
            PrepareIframeProperties(i, iframe);
            HtmlGenericControl whiteDiv = new HtmlGenericControl("DIV");
            whiteDiv.Style.Add(HtmlTextWriterStyle.BackgroundColor, "white");
            whiteDiv.Controls.Add(iframe);
            ContentControl contentControl = new ContentControl();
            contentControl.Controls.Add(whiteDiv);
            tabPage.ContentCollection.Add(contentControl);
        }
    }
    void PrepareIframeProperties(int i, HtmlGenericControl iframe) {
        iframe.ClientIDMode = System.Web.UI.ClientIDMode.Static;
        iframe.Attributes.Add("src", String.Format("{0}?preview={1}", Request.Url, i));
        iframe.Attributes.Add("width", "100%");
        iframe.Attributes.Add("height", "500px");
        iframe.Attributes.Add("style", "border-style: none;");
    }
    public bool CanShowPreview() {
        return Request.QueryString.Count > 0 && Request.QueryString.ToString().StartsWith("preview");
    }
    public bool RenderSpreadsheetPreview() {
        if(CanShowPreview()) {
            string sheetIndxString = Request.Params["preview"];
            int sheetIndx = (String.IsNullOrEmpty(sheetIndxString)) ? 0 : Convert.ToInt32(sheetIndxString);
            Response.StatusCode = (int)HttpStatusCode.OK;
            Response.ContentType = "text/html";
            HtmlContentGenerator htmlGenerator = new HtmlContentGenerator(Response.OutputStream);
            htmlGenerator.Generate(Workbook, sheetIndx);
            Response.End();
            return true;
        }
        return false;
    }
    protected string GetBasePath() {
        return String.Format("{0}?preview", Request.Url.ToString());
    }
}
