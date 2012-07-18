<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebServiceRGS.Tester.Default" validateRequest="false"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width:100%">
    
        <table ID="Table1" runat="server" width="100%">
            <tr>
                <td>
                    <asp:Button ID="Button1" runat="server" Text="Executar Teste" 
                        onclick="Button1_Click" />
                </td>
            </tr>
            <tr>
                <td style="width:100%">
                    <asp:TextBox ID="txtResultado" runat="server" Height="318px" TextMode="MultiLine" Width="100%"></asp:TextBox>
                </td>
            </tr>
            
        </Table>
    </div>
    </form>
</body>
</html>
