using System.IO;
using DevExpress.Spreadsheet;

public static class StreamCopyHelper {
    public static void Copy(Stream src, Stream dst) {
        const int bufferSize = 32768;
        byte[] buffer = new byte[bufferSize];
        int bytesRead = 0;
        do {
            bytesRead = src.Read(buffer, 0, bufferSize);
            dst.Write(buffer, 0, bytesRead);

        }
        while(bytesRead == bufferSize);
    }
}

public class HtmlContentGenerator {
    Stream dstStream;
    public HtmlContentGenerator(Stream dstStream) {
        this.dstStream = dstStream;
    }
    public void Generate(IWorkbook book, int sheetIndx) {
        DevExpress.XtraSpreadsheet.Model.DocumentModel model = book.Model;
        MemoryStream tempStream = new MemoryStream();
        DevExpress.XtraSpreadsheet.Export.HtmlDocumentExporterOptions options = new DevExpress.XtraSpreadsheet.Export.HtmlDocumentExporterOptions();
        options.SheetIndex = sheetIndx;
        options.EmbedImages = true;
        model.InternalAPI.SaveDocumentHtmlContent(tempStream, options);
        tempStream.Seek(0, SeekOrigin.Begin);
        StreamCopyHelper.Copy(tempStream, this.dstStream);
    }
    public void Generate(IWorkbook book) {
        Generate(book, 0);
    }
}