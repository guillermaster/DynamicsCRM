<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RespuestaCampanna.aspx.cs" Inherits="Efika.Crm.Web.RespuestaCampanna"
EnableSessionState ="True"
 %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Respuesta de Campaña</title>
    <base target="_self" />
     <link href="Styles/second.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        function CerrarVentana() {
            window.returnValue = "N";
            window.close();
        }

        function MostrarProcesando() {
            document.getElementById("divControles").style.display = "none";
            document.getElementById("tblProcesando").style.display = "block";
        }
</script>
       
    <style type="text/css">
        .style3
        {
            /*border: 1px solid #C0C0C0;*/
				font-family: Arial, Helvetica, sans-serif;
            font-size: 9pt;
            padding-left: 7px;
            font-weight: bold;
            height: 46px;
        }
        .style4
        {
            height: 38px;
        }
        .style5
        {
            /*border: 1px solid #C0C0C0;*/
				font-family: Arial, Helvetica, sans-serif;
            font-size: 9pt;
            padding-left: 7px;
            font-weight: bold;
            height: 38px;
        }
        </style>
       
</head>
<body class="page_content, DialogBkgd">
    <form id="form1" runat="server">
    <div style="position: absolute; top: 0; left: 0; width: 100%; height: 100%; z-index: 100">
        <div style="position: fixed; top: 40%; left: 50%; width: 401px; height: 150px; margin: -9em 0 0 -15em;
            border: 2px solid #000; background-color: #FFFFEE; filter: alpha(opacity=100);">
            <div style="position: fixed; top: 25%; left: 40%; text-align: center;">
                <img src="../waiting.gif" alt="Cargando" /><br />
                <br />
                <span style="color: #000099; font-weight: bold; font-size: 12">Procesando...</span>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
