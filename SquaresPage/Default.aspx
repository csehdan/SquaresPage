<%@ Page Language="C#" Inherits="SquaresPage.Default" %>
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
		<asp:Button id="btnRemovePts" runat="server" Text="Remove!" OnClick="btnRemovePtsClicked" Enabled=false /><br>
		<br>

		<asp:Button id="btnImportList" runat="server" Text="Import a list of points" OnClick="btnImportListClicked" /><br>
		<asp:Button id="btnExportList" runat="server" Text="Save points to file" OnClick="btnExportListClicked" Enabled=false /><br>
		<asp:Button id="btnDeleteList" runat="server" Text="Delete file list" OnClick="btnDeleteListClicked" /><br>
		<br>
		<asp:Button id="btnCountSq" runat="server" Text="Count squares!" OnClick="btnCountSqClicked" Enabled=false /><br>
		<br><br>

		<asp:Label id="lblPPP" runat="server" Text="Points per page: " Enabled=false/>
		<asp:DropDownList id="dropPages" runat="server" AutoPostBack=true OnSelectedIndexChanged="DropPagesChanged" >
			<asp:ListItem Text="5" Value="5" />
			<asp:ListItem Text="10" Value="10" />
			<asp:ListItem Text="20" Value="20" />
			<asp:ListItem Text="50" Value="50" />
		</asp:DropDownList>
		<br>
		Sort by: 
		<asp:DropDownList id="dropSortBy" runat="server" AutoPostBack=true OnSelectedIndexChanged="ChangeSort" >
			<asp:ListItem Text="X" Value="x" />
			<asp:ListItem Text="Y" Value="y" />
		</asp:DropDownList>
		<asp:DropDownList id="dropAscDesc" runat="server" AutoPostBack=true OnSelectedIndexChanged="ChangeSort" >
			<asp:ListItem Text="Ascending" Value="a" />
			<asp:ListItem Text="Descending" Value="d" />
		</asp:DropDownList>
		<br>
		<asp:Button id="btnLeft" runat="server" Text="&lt;" OnClick="btnLeftClicked" Enabled=false />
		<asp:Label id="lblNofN" runat="server" Text="" Enabled=false/>
		<asp:Button id="btnRight" runat="server" Text="&gt;" OnClick="btnRightClicked" Enabled=false /><br>
		<asp:Label id="listOfPoints" runat="server" Text="Currently the following points are added:"/>
		<br><br>
		<asp:Label id="listOfSquares" runat="server" Text=""/>

	</form>
</body>
</html>
