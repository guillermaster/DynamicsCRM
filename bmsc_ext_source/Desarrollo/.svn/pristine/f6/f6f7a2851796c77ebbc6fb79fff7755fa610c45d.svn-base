using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Reflection;
using Efika.Crm.AccesoServicios;
using Efika.Crm.Negocio;

namespace Efika.Crm.Web
{
    public class Utilidades
    {
        public static void AgregarErrorAlLog(string mensajeError, string nombreClase, string nombreMetodo, string usuario, string stackTrace)
        {
            Log log = new Log(ConfigurationManager.AppSettings["CRMRutaLogs"]);
            log.Add(mensajeError, nombreClase, nombreMetodo, usuario, stackTrace);
        }


        public static bool ValidarAccesosWeb(string idUsuario, string privilegio, bool guardarErroresEnLog = true)
        {
            bool bAutentica = false;
            try
            {
                return Privilegios.ValidarPrivilegio(privilegio, new Guid(idUsuario), Credenciales.ObtenerCredenciales());
            }
            catch (Exception ex)
            {
                if (guardarErroresEnLog)
                {
                    AgregarErrorAlLog(ex.Message, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name,
                            HttpContext.Current.User.Identity.Name, ex.StackTrace);
                }
                bAutentica = false;
            }
            return bAutentica;
        }


        public static void ReportarError(System.Web.UI.Page aspx, string usuario, string descError, string titulo, Exception ex)
        {
            string mensaje = @"<script type=""text/javascript""> alert(""Ha ocurrido un error: " + descError.Replace("\r\n", ". ") + @" "");</script>";
            aspx.ClientScript.RegisterStartupScript(aspx.GetType(), titulo, mensaje);
            AgregarErrorAlLog(ex.Message, aspx.GetType().Name, MethodBase.GetCurrentMethod().Name, usuario, ex.StackTrace);
        }

        public static void Alert(System.Web.UI.Page aspx, string mensaje, string titulo)
        {
            string alert = @"<script type=""text/javascript""> alert(""" + mensaje + @" "");</script>";
            aspx.ClientScript.RegisterStartupScript(aspx.GetType(), titulo, alert);
        }

        public static string DecimalToFormatStringMoney(decimal value, string moneySymbol)
        {
            try
            {
                return moneySymbol + " " + value.ToString("N", System.Globalization.CultureInfo.CreateSpecificCulture("de-DE"));
            }
            catch
            {
                return "";
            }
        }

        public static string DecimalToFormatStringMoney(decimal value)
        {
            try
            {
                return "$ " + value.ToString("N", System.Globalization.CultureInfo.CreateSpecificCulture("de-DE"));
            }
            catch
            {
                return "";
            }
        }

        public static string DecimalToFormatString(decimal value)
        {
            try
            {
                return value.ToString("N", System.Globalization.CultureInfo.CreateSpecificCulture("de-DE"));
            }
            catch
            {
                return "";
            }
        }

        public static decimal FormatStringMoneyToDecimal(string value, string moneySymbol)
        {

            string valueAux;
            try
            {
                int initPos = value.IndexOf(moneySymbol) + moneySymbol.Length;
                valueAux = value.Substring(initPos, value.Length - initPos).Trim();
                return decimal.Parse(valueAux, System.Globalization.CultureInfo.CreateSpecificCulture("de-DE"));
            }
            catch
            {
                return 0;
            }

        }

        public static decimal FormatStringMoneyToDecimal(string value)
        {
            try
            {
                int initPos = value.IndexOf("$") + 1;
                value = value.Substring(initPos, value.Length - initPos).Trim();
                return decimal.Parse(value, System.Globalization.CultureInfo.CreateSpecificCulture("de-DE"));
            }
            catch
            {
                return 0;
            }
        }

        public static decimal FormatStringToDecimal(string value)
        {
            try
            {
                return decimal.Parse(value, System.Globalization.CultureInfo.CreateSpecificCulture("de-DE"));
            }
            catch
            {
                return 0;
            }
        }

        public static string ReemplazarCaracteresEspeciales(string texto)
        {
            string consignos = "áàäéèëíìïóòöúùuñÁÀÄÉÈËÍÌÏÓÒÖÚÙÜÑçÇ";
            string sinsignos = "aaaeeeiiiooouuunAAAEEEIIIOOOUUUNcC";
            for (int v = 0; v < sinsignos.Length; v++)
            {
                string i = consignos.Substring(v, 1);
                string j = sinsignos.Substring(v, 1);
                texto = texto.Replace(i, j);
            }
            return texto;
        }

        public static string GetUrlCRM()
        {
            return ConfigurationManager.AppSettings["CRMDireccionServidor"] + "/" + ConfigurationManager.AppSettings["CRMOrganizacion"];
        }


        public class NombresParametros
        {
            public static string Usuario { get { return "strCodigoUsuario"; } }
            public static string OportunidadId { get { return "strCodOportunidad"; } }
            public static string ClienteId { get { return "accountid"; } }
            public static string MontoSolicitado { get { return "strMontoSol"; } }
            public static string NumeroSolicitud { get { return "numSolicitud"; } }
            public static string SimulacionId { get { return "strCodSimulacion"; } }
            public static string TipoProductoId { get { return "strIdTipoProd"; } }
            public static string DivisaId { get { return "codDivisa"; } }
            public static string CampaniaId { get { return "strCodigoCampania"; } }
            public static string RespCampaniaId { get { return "strCodigoRespCampania"; } }
            public static string Titulo { get { return "strTitulo"; } }
            public static string NumeroOferta { get { return "numOferta"; } }
            public static string Aprobacion { get { return "aprobacion"; } }
            public static string Moneda { get { return "moneda"; } }
            public static string Origen { get { return "strOrigen"; } }
            public static string CodIsoMoneda { get { return "codIsoDivisa"; } }

        }
    }
}