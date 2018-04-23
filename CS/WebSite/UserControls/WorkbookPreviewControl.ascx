<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WorkbookPreviewControl.ascx.cs"
    Inherits="UserControls_WorkbookPreviewControl" %>
<% if(!CanShowOnlyFirstWorksheet) { %>
<dx:ASPxPageControl ID="ASPxPageControl1" ClientInstanceName="ASPxPageControl1" runat="server"
    ActiveTabIndex="0" Width="100%" EnableViewState="False" EnableCallBacks="True"
    EnableHierarchyRecreation="True">
</dx:ASPxPageControl>
<% } else { %>
    <iframe id="previewFrame0" runat="server"></iframe>
<% } %>
<script type="text/javascript">
    // <![CDATA[
    SpreadsheetPreviewControl = _aspxCreateClass(null, {
        constructor: function (sheetCount, basePath) {
            this.constructor.prototype.constructor.call(this);
            this.sheetCount = sheetCount;
            this.basePath = basePath;
        },
        Update: function(param) {
            for(var i = 0; i < this.sheetCount; i++) {
                var iframeElementName = this.frames[i];
                if (!iframeElementName)
                    continue;
                var iframeElement = document.getElementById(iframeElementName);
                if (!iframeElement)
                    continue;
                var additionalParams = "&" + new Date().valueOf();
                if (param)
                    additionalParams = param;
                iframeElement.src = this.basePath + "=" + i + additionalParams;
            }
        }
    });
    dx = <%= ClientInstanceName %> = new SpreadsheetPreviewControl(<%= Workbook.Worksheets.Count %>, <%= String.Format("'{0}'", GetBasePath()) %>);
    dx.frames = [];
    <% for (int i=0; i< Workbook.Worksheets.Count; i++) { %>
    dx.frames[ <%= i %> ] = <%= String.Format("'previewFrame{0}'", i) %>;
    <% } %>
    // ]]> 
</script>
