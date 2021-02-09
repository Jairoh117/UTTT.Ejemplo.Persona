<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="prueba.aspx.cs" Inherits="UTTT.Ejemplo.Persona.prueba" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>

    <script>
        const txtClaveUnica = document.querySelector('#TextBox1');

        function valida() {
            if (txtClaveUnica === '') {
                alert("campo vacio");
            }
        }


    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        &nbsp; Clave unica&nbsp;&nbsp;
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <p>
            <asp:Button ID="Button1" runat="server" Text="Button" OnClick="valida()"/>
        </p>
    </form>
</body>
</html>
