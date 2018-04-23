using DevExpress.Spreadsheet;

public partial class _Default : System.Web.UI.Page {
    protected void Page_Load(object sender, System.EventArgs e) {
        previewControl.Workbook = GenerateBook();
        if (previewControl.CanShowPreview())
            previewControl.RenderSpreadsheetPreview();
    }

    private IWorkbook GenerateBook() {
        IWorkbook result = new Workbook();
        string path = Page.MapPath("~/App_Data/EmployeeInformation_template.xlsx");
        result.LoadDocument(path);
        return result;
    }
}