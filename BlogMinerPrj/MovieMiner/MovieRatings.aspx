<%@ Page Language="C#" MasterPageFile="~/design/MasterPage.master" ValidateRequest="false" AutoEventWireup="true" CodeFile="MovieRatings.aspx.cs" Inherits="_Default" Title="Untitled Page" %>
<%@ Import Namespace="Artisteer" %>
<%@ Register TagPrefix="artisteer" Namespace="Artisteer" %>
<%@ Register TagPrefix="art" TagName="DefaultMenu" Src="DefaultMenu.ascx" %> 
<%@ Register TagPrefix="art" TagName="DefaultHeader" Src="DefaultHeader.ascx" %> 
<%@ Register TagPrefix="art" TagName="DefaultSidebar1" Src="DefaultSidebar1.ascx" %>
          <%@ Register TagPrefix="art" TagName="DefaultSidebar2" Src="DefaultSidebar2.ascx" %>
          

<asp:Content ID="PageTitle" ContentPlaceHolderID="TitleContentPlaceHolder" Runat="Server">
    Movie Miner!
</asp:Content>

<asp:Content ID="MenuContent" ContentPlaceHolderID="MenuContentPlaceHolder" Runat="Server">
    <art:DefaultMenu ID="DefaultMenuContent" runat="server" />
</asp:Content>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeaderContentPlaceHolder" Runat="Server">
    <art:DefaultHeader ID="DefaultHeader" runat="server" />
</asp:Content>
<asp:Content ID="SideBar1" ContentPlaceHolderID="Sidebar1ContentPlaceHolder" Runat="Server">
    <art:DefaultSidebar1 ID="DefaultSidebar1Content" runat="server" />
</asp:Content>
<asp:Content ID="SideBar2" ContentPlaceHolderID="Sidebar2ContentPlaceHolder" Runat="Server">
    <art:DefaultSidebar2 ID="DefaultSidebar2Content" runat="server" />
</asp:Content>

<asp:Content ID="SheetContent" ContentPlaceHolderID="SheetContentPlaceHolder" Runat="Server">
<asp:TextBox ID="dispBgWork" AutoPostBack="true" Font-Bold="true" ReadOnly="true" runat="server" TextMode="MultiLine" Rows="10" Columns="50">
</asp:TextBox>
<asp:PlaceHolder ID="PlaceHolderNum" runat="server" />
<br /><hr />
<asp:PlaceHolder ID="PlaceHolderA" runat="server" />
<br /><hr />
<asp:PlaceHolder ID="PlaceHolderB" runat="server" />
<br /><hr />
<asp:PlaceHolder ID="PlaceHolderC" runat="server" />
<br /><hr />
<asp:PlaceHolder ID="PlaceHolderD" runat="server" />
<br /><hr />
<asp:PlaceHolder ID="PlaceHolderE" runat="server" />
<br /><hr />
<asp:PlaceHolder ID="PlaceHolderF" runat="server" />
<br /><hr />
<asp:PlaceHolder ID="PlaceHolderG" runat="server" />
<br /><hr style ="color" />
<asp:PlaceHolder ID="PlaceHolderH" runat="server" />
<br /><hr />
<asp:PlaceHolder ID="PlaceHolderI" runat="server" />
<br /><hr />
<asp:PlaceHolder ID="PlaceHolderJ" runat="server" />
<br /><hr />
<asp:PlaceHolder ID="PlaceHolderK" runat="server" />
<br /><hr />
<asp:PlaceHolder ID="PlaceHolderL" runat="server" />
<br /><hr />
<asp:PlaceHolder ID="PlaceHolderM" runat="server" />
<br /><hr />
<asp:PlaceHolder ID="PlaceHolderN" runat="server" />
<br /><hr />
<asp:PlaceHolder ID="PlaceHolderO" runat="server" />
<br /><hr />
<asp:PlaceHolder ID="PlaceHolderP" runat="server" />
<br /><hr />
<asp:PlaceHolder ID="PlaceHolderQ" runat="server" />
<br /><hr />
<asp:PlaceHolder ID="PlaceHolderR" runat="server" />
<br /><hr />
<asp:PlaceHolder ID="PlaceHolderS" runat="server" />
<br /><hr />
<asp:PlaceHolder ID="PlaceHolderT" runat="server" />
<br /><hr />
<asp:PlaceHolder ID="PlaceHolderU" runat="server" />
<br /><hr />
<asp:PlaceHolder ID="PlaceHolderV" runat="server" />
<br /><hr />
<asp:PlaceHolder ID="PlaceHolderW" runat="server" />
<br /><hr />
<asp:PlaceHolder ID="PlaceHolderX" runat="server" />
<br /><hr />
<asp:PlaceHolder ID="PlaceHolderY" runat="server" />
<br /><hr />
<asp:PlaceHolder ID="PlaceHolderZ" runat="server" />
<br /><hr />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
<asp:Button ID="ViewButton" runat="server" Font-Bold="true" Text="View" Width="200" Height="50" OnClick="chkChkBoxes" BackColor="#BCDA29" ForeColor="#4B570F" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
<asp:Button ID="CompareButton" runat="server" Font-Bold="true" Text="Compare" Width="200" Height="50" OnClick="chkChkBoxes" BackColor="#BCDA29" ForeColor="#4B570F" />
</asp:Content>