<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="salesAgentStudentCourseTable.ascx.cs" Inherits="E_LearningWeb_Mobile.salesAgentStudentCourseTable" %>

<asp:Table runat="server" Width="100%">
    <asp:TableRow>
        <asp:TableCell Height="5px"></asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell HorizontalAlign="Center">
            <asp:Label ID="lblStudentName" runat="server" Text="<%$resources:global,student%>"></asp:Label>
            <asp:DropDownList ID="ddlStudentName" runat="server" 
                AutoPostBack="True" OnSelectedIndexChanged="ddlStudentName_SelectedIndexChanged">
            </asp:DropDownList>
        </asp:TableCell>
    </asp:TableRow>
</asp:Table>

<asp:Table runat="server" width="100%">
    <asp:TableRow runat="server">
        <asp:TableCell>
            &nbsp;&nbsp;
            <asp:LinkButton onclick="PrevWeek_Click" runat="server" Text="<%$resources:global,LastWeekLeft%>"></asp:LinkButton>
        </asp:TableCell>
        <asp:TableCell>
            <h3 style="text-align:center;margin-top:8px;margin-bottom:8px">
                <asp:Label runat="server" Text="<%$resources:global,studentCourseTimeTable%>"></asp:Label>
            </h3>
        </asp:TableCell>
        <asp:TableCell HorizontalAlign="right">
            <asp:LinkButton onclick="NextWeek_Click" runat="server" Text="<%$resources:global,NextWeekRight%>"></asp:LinkButton>
            &nbsp;&nbsp;
        </asp:TableCell>
    </asp:TableRow>
</asp:Table>

<asp:DataList ID="dtlStudentScheduleInfo" OnItemDataBound="Item_Bound" runat="server" Width="100%" HorizontalAlign="Center">
    <ItemTemplate>
        <asp:Table ID="tblStudentScheduleList" runat="server" HorizontalAlign="Center" Width="100%">
            <asp:TableRow ID="TableRow1" runat="server">
                <asp:TableCell ID="TableCel1" runat="server" Width="50%" Font-Bold="True" Font-Size="Medium" HorizontalAlign="center">
                    <asp:Label ID="Label1" runat="server" Text='<%#Eval("startDT").ToString()%>'></asp:Label>
                </asp:TableCell>
                <asp:TableCell ID="TableCel2" runat="server" Font-Size="Medium" HorizontalAlign="center">
                    <asp:Label ID="Label3" runat="server" Text='<%#Eval("courseName").ToString()%>'></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </ItemTemplate>
	<FooterTemplate>
        <asp:Table ID="tblStudentScheduleFooter" runat="server" HorizontalAlign="Center" Width="100%">
            <asp:TableRow ID="TableRow2" runat="server">
                <asp:TableCell runat="server" Width="100%" HorizontalAlign="center">
                    <asp:Label ID="lblNoRecord" Visible="false" runat="server" Text="<%$resources:global,emptyData%>"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </FooterTemplate>
</asp:DataList>

<asp:Table runat="server" width="100%">
    <asp:TableRow>
        <asp:TableCell Height="5px"></asp:TableCell>
    </asp:TableRow>
    <asp:TableRow runat="server">
        <asp:TableCell HorizontalAlign="center">
            <asp:Label ID="Label6" runat="server" ForeColor="red" Text="<%$resources:global,color2%>"></asp:Label>&nbsp;&nbsp;
            <asp:Label ID="Label8" runat="server" ForeColor="gray" Text="<%$resources:global,color4%>"></asp:Label>&nbsp;&nbsp;
            <asp:Label ID="Label7" runat="server" ForeColor="black" Text="<%$resources:global,color3%>"></asp:Label>&nbsp;&nbsp;
            <asp:Label ID="Label5" runat="server" ForeColor="blue" Text="<%$resources:global,color1%>"></asp:Label>&nbsp;&nbsp;
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell Height="5px"></asp:TableCell>
    </asp:TableRow>
</asp:Table> 
<asp:HiddenField ID="currentWeek" runat="server"></asp:HiddenField>
