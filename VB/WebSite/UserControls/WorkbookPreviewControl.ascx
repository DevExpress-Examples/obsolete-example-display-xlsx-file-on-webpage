<%@ Control Language="vb" AutoEventWireup="true" CodeFile="WorkbookPreviewControl.ascx.vb"
    Inherits="UserControls_WorkbookPreviewControl" %>
<%
If Not CanShowOnlyFirstWorksheet Then
%>
<dx:ASPxPageControl ID="ASPxPageControl1" ClientInstanceName="ASPxPageControl1" runat="server"
    ActiveTabIndex="0" Width="100%" EnableViewState="False" EnableCallBacks="True"
    EnableHierarchyRecreation="True">
</dx:ASPxPageControl>
<%
Else
%>
    <iframe id="previewFrame0" runat="server"></iframe>
<%
End If
%>
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
    dx = <%=ClientInstanceName%> = new SpreadsheetPreviewControl(<%=Workbook.Worksheets.Count%>, <%=String.Format("'{0}'", GetBasePath())%>);
    dx.frames = [];
    <%
    For i As Integer = 0 To Workbook.Worksheets.Count - 1
    %>
    dx.frames[ <%=i%> ] = <%=String.Format("'previewFrame{0}'", i)%>;
    <%
    Next i
    %>
    // ]]> 
</script>