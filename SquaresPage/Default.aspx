﻿<%@ Page Language="C#" Inherits="SquaresPage.Default" %>
<!DOCTYPE html>
<html>
<head runat="server">
	<title>Squares</title>
</head>
<body>
	<form id="formDefault" runat="server">
		Add a point to the list of points.<br>
		Enter X and Y coordinates respectively:<br>
		<input	id="XCoord" runat="server" type="text" size="10"/>
		<input	id="YCoord" runat="server" type="text" size="10"/><br>
		<asp:Button id="btnAddPt" runat="server" Text="Add" OnClick="btnAddPtClicked" /><br>

		<asp:Button id="btnClearList" runat="server" Text="Clear list" OnClick="btnClearListClicked" /><br>
		Remove points (enter the indices of the points to be deleted separated by white space and/or comma): 
		<input	id="toBeRemoved" runat="server" type="text" size="20" /><br>
		<asp:Button id="btnRemovePts" runat="server" Text="Remove!" OnClick="btnRemovePtsClicked" /><br>
		<br>

		<asp:Button id="btnImportList" runat="server" Text="Import a list of points" OnClick="btnImportListClicked" /><br>
		<asp:Button id="btnExportList" runat="server" Text="Save points to file" OnClick="btnExportListClicked" Enabled=false /><br>
		<asp:Button id="btnDeleteList" runat="server" Text="Delete file list" OnClick="btnDeleteListClicked" /><br>
		<br>
		<asp:Button id="btnCountSq" runat="server" Text="Count squares!" OnClick="btnCountSqClicked" Enabled=false Visible=true /><br>
		<br><br>
		<asp:Label id="listOfPoints" runat="server" Text=""/>
		<asp:Label id="listOfSquares" runat="server" Text=""/>

	</form>
</body>
</html>
