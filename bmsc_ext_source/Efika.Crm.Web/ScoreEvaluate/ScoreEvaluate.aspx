<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScoreEvaluate.aspx.cs"
    Inherits="Efika.Crm.Web.ScoreEvaluate.ScoreEvaluate" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Styles/ribbon.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.8.3.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function CerrarVentana() {
            window.close();
        }

        $(window).load(function () {           


        });

        $(document).ready(function () {            

        });


        function EvaluarScore() {
            var queryString = location.search.substring(1);
            queryString = "?" + queryString + "&option=FOTOSOLICITUDCREDITO"

            $.get("ScoreEvaluateProcess.aspx" + queryString, function (response) {
                alert(response);
            });
        }

        function ValidateScore() {
            var queryString = location.search.substring(1);
            var url = "ScoreEvaluate.aspx?" + queryString;  //+ "&option=VALIDATESCORE"

            var param = {
                "option": "VALIDATESCORE"
            };

            $.getJSON(url, param, function (returndata) {
                alert(returndata);
            });

        }

        function Cerrar() {
            //$('#BtnEvaluarScore').attr('disabled', 'disabled');

            window.close();
        }
        function ShowProgessBar(value, msg) {
            $('#dvLoading').hide();
            $("#dvLoadingMsg").hide();

            if (msg == undefined)
            { msg = ''; }

            if (value == true) {
                $('#dvLoading').show();
                $("#dvLoadingMsg").html(msg);
                $("#dvLoadingMsg").show();
            }
        }

        function checklist(img, text) {
            $('#checkList').append("<div><img src='" + img + "'/> " + text + "</div>");
        }

    </script>
    <style type="text/css" id="style1">
        #dvLoading
        {
            background: url(../Images/progress.gif) no-repeat center center;
            height: 100px;
            width: 100px;
            position: fixed;
            z-index: 1000;
            left: 50%;
            top: 50%;
            margin: -25px 0 0 -25px;
        }
        
        #dvLoadingMsg
        {
            background: no-repeat center center;
            height: 100px;
            width: auto;
            position: fixed;
            z-index: 1000;
            left: 49%;
            top: 57%; /*margin: -25px 0 0 -25px;*/
        }
        #url
        {
            width: 421px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptMngr" runat="server">
    </asp:ScriptManager>
    <asp:Panel id="overlay" style="display:block; position:absolute; left:0; top: 0; width: 100%; height: 100%; background-color: #fff; z-index: 100;" runat="server"></asp:Panel>
    <div style="margin-top: -20px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px;">
        <div class="RibbonContextsContainer" id="OfficeRibbon1_RibbonContextsContainer">
            <div class="RibbonContext RibbonContext1" style='border-top-color: transparent; border-right-color: transparent;
                border-left-color: transparent; border-top-width: 3px; border-right-width: 1px;
                border-left-width: 1px; border-top-style: solid; border-right-style: solid; border-left-style: solid;
                float: left; background-image: url("none"); background-color: transparent;' contextid="RibbonContext1">
                <div class="RibbonContextTitle" style="clear: both;">
                </div>
                <div class="RibbonTab RibbonTabActive" style="display: block;" associatedtab="1e959ef0-c1cb-41f6-bdf4-dc646b9c36cd"
                    tabid="RibbonTab101">
                    <table style="border-collapse: collapse;" cellspacing="0" cellpadding="0">
                        <tbody>
                            <tr>
                                <td>
                                    Evaluar Score
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
            <div class="clear">
            </div>
            <div class="clear">
            </div>
        </div>
        <input name="OfficeRibbon1$OfficeWebUI_Ribbon_LastTab" id="OfficeRibbon1_OfficeWebUI_Ribbon_LastTab"
            type="hidden" />
        <div class="RibbonApplicationMenuContainer">
        </div>
        <div class="RibbonTabsContainer" id="OfficeRibbon1_RibbonTabsContainer">
            <div class="RibbonTabContent" id="OfficeRibbon1_RibbonTab101_TabContent" style="display: block;">
                <div class="RibbonGroupCollapsedContainer" id="OfficeRibbon1_RibbonGroup1_GroupCollapsedContainer"
                    style="display: none;">
                    <table class="RibbonGroupCollapsedContainerTable" style="border-collapse: collapse;"
                        cellspacing="0" cellpadding="2">
                        <tbody>
                            <tr>
                                <td>
                                    <img alt="text" style="width: 32px; height: 32px;" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Acciones
                                </td>
                            </tr>
                            <tr>
                                <td style='background-image: url(../Images/Ribbon/Ribbon_Background.png); background-attachment: scroll;
                                    background-repeat: no-repeat; background-position-x: center; background-position-y: bottom;
                                    background-size: auto; background-origin: padding-box; background-clip: border-box;
                                    background-color: transparent;'>
                                    <img alt="imgtext" style="height: 10px;" src="../Images/Ribbon/Ribbon_Background.png" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <div class="RibbonGroupCollapsedContainerDropDown">
                    </div>
                </div>
                <div class="RibbonGroupContent" id="OfficeRibbon1_RibbonGroup1_GroupContent" objtype="Element">
                    <table style="border-collapse: collapse;" cellspacing="0" cellpadding="0">
                        <tbody>
                            <tr>
                                <td class="RibbonGroupColumn" valign="top">
                                    <div class="RibbonZoneContent">
                                        <div class="RibbonItems RibbonItems_LargeItem RibbonItemWithTooltip" tooltip="<b>Evaluar</b><br/>Score."
                                            itemid="btnEvaluarScore">
                                            <table cellspacing="1" cellpadding="0">
                                                <tbody>
                                                    <tr>
                                                        <td align="center">
                                                            <!--<img style="width: 32px; height: 32px;" src="../Images/32/contractrenew_32.png" />-->
                                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:ImageButton ID="BtnEvaluarScore" OnClientClick="ShowProgessBar(true,'');" OnClick="BtnEvaluarScore_Click"
                                                                        runat="server" Style="width: 32px; height: 32px;" src="../Images/32/contractrenew_32.png" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 33px;">
                                                        <td align="center">
                                                            Evaluar Score
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                        <div class="RibbonItems RibbonItems_LargeItem RibbonItemWithTooltip" onclick="Cerrar();"
                                            tooltip="<b>Cerrar</b><br/>Cierra esta ventana." itemid="btnCerrar">
                                            <table cellspacing="1" cellpadding="0">
                                                <tbody>
                                                    <tr>
                                                        <td align="center">
                                                            <img style="width: 32px; height: 32px;" src="../Images/32/icon10_32.png" />
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 33px;">
                                                        <td align="center">
                                                            Cerrar
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="RibbonGroupTitle" colspan="1">
                                    Acciones
                                </td>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <div class="RibbonTooltipContainer" style="display: none;">
        </div>
    </div>
    <div class="OfficeWebUI_WorkspaceContainer" id="Workspace1_Workspace1_mainPanel">
        <table class="OfficeWebUI_Workspace" id="Workspace1_Workspace1_mainTable" style="height: 517px;
            border-collapse: collapse;" cellspacing="0">
            <tbody>
                <tr>
                    <td class="OfficeWebUI_WorkspaceContentCell" id="Workspace1_Workspace1_contentCell"
                        valign="top">
                        <div class="OfficeWebUI_WorkspaceContentPanel" id="Workspace1_Workspace1_contentPanel"
                            style="height: 517px;">
                            <div id="divControles">
                                <div id="checkList">
                                    <!-- <input id="url" type="text" />-->
                                </div>
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                    <ProgressTemplate>
                                        <div id="dvLoading" style="background: url(../Images/progress.gif) no-repeat center center;
                                            height: 100px; width: 100px; position: fixed; z-index: 1000; left: 50%; top: 50%;
                                            margin: -25px 0 0 -25px">
                                            <br />
                                            <div id="dvLoadingMsg">
                                                Validando informacion...
                                            </div>
                                        </div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </div>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
