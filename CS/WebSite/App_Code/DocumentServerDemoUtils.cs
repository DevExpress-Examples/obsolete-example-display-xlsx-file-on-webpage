using System.IO;
using DevExpress.Spreadsheet;
using DevExpress.XtraSpreadsheet.Export;

public class HtmlContentGenerator
{
    Stream stream;
    public HtmlContentGenerator(Stream stream)
    {
        this.stream = stream;
    }
    public void Generate(IWorkbook workbook, int sheetIndex)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            HtmlDocumentExporterOptions options = new HtmlDocumentExporterOptions
            {
                SheetIndex = sheetIndex,
                EmbedImages = true,
                AnchorImagesToPage = true
            };
            workbook.ExportToHtml(ms, options);
            ms.Seek(0, SeekOrigin.Begin);
            ms.CopyTo(this.stream);
        }
    }
    public void Generate(IWorkbook workbook)
    {
        Generate(workbook, 0);
    }
}