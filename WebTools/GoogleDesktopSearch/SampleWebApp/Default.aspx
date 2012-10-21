<%@ Page language="c#" Codebehind="Default.aspx.cs" AutoEventWireup="false" Inherits="googleye.WebForm1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>WebForm1</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<div align="center"><asp:textbox id="Keywords" runat="server" Width="213px"></asp:textbox><asp:button id="Search" runat="server" Text="Search"></asp:button></div>
			<br>
			<br>
			<asp:label id="Counter" runat="server" Visible="False" Font-Size="14px" ForeColor="#0033cc"
				Font-Names="Arial" Font-Underline="True">Results counter</asp:label><br>
			<br>
			<asp:datalist id="Results" runat="server">
				<ItemTemplate>
					<table width="60%">
						<tr>
							<td style="FONT-WEIGHT: bold; FONT-SIZE: 16px; COLOR: #0033cc; FONT-FAMILY: arial, sans-serif; TEXT-DECORATION: underline"><%# DataBinder.Eval(Container.DataItem, "Title") %></td>
						</tr>
						<tr>
							<td style="FONT-SIZE: 14px; COLOR: #000000; FONT-FAMILY: arial,sans-serif; TEXT-DECORATION: none"><%# DataBinder.Eval(Container.DataItem, "Snippet") %></td>
						</tr>
						<tr>
							<td style="FONT-SIZE: 12px; COLOR: #009900; FONT-FAMILY: arial,sans-serif"><%# DataBinder.Eval(Container.DataItem, "Url") %>&nbsp;&nbsp;&nbsp;<a style="FONT-SIZE: 12px; COLOR: #6f6f6f; FONT-FAMILY: arial,sans-serif" href='<%# DataBinder.Eval(Container.DataItem, "HighlightedUrl") %>'>Highlighted</a></td>
						</tr>
					</table>
				</ItemTemplate>
			</asp:datalist></form>
	</body>
</HTML>
