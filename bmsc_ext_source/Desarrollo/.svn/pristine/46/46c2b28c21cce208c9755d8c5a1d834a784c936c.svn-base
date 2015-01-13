using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Efika.Crm.Negocio;
using Efika.Crm.Entidades.TipoCambio;
using System.Globalization;

namespace Efika.Crm.Web.SimuladorCredito
{
    public partial class rptFlujoCaja : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Utilidades.ValidarAccesosWeb(Request.QueryString[Utilidades.NombresParametros.Usuario], Privilegios.NombresPrivilegios.LeerOportunidad))
            {
                Response.Write(@"<script type=""text/javascript""> alert('No tiene privilegios para acceder a este formulario.'); window.close(); </script>");
                return;
            }
            MostrarReporte(Request.QueryString[Utilidades.NombresParametros.NumeroSolicitud]);
        }

        public void MostrarReporte(string numSolicitud, string moneda = "dolar")
        {
            Negocio.Oportunidad varopor = new Oportunidad(Credenciales.ObtenerCredenciales());
            Entidades.Oportunidad entoportu = new Entidades.Oportunidad();
            entoportu = varopor.GetByNroSolicitud(numSolicitud);
            Entidades.Producto entpro = new Entidades.Producto();
            entpro = varopor.GetProductoOportunidadHomologado(entoportu.Id);
            string reporte_flujo;
            string tipoMoneda = (Request.QueryString[Utilidades.NombresParametros.Moneda] == null ? "us" : Request.QueryString[Utilidades.NombresParametros.Moneda]);
            string tasaCambio = "1";

            CultureInfo usCulture = new CultureInfo("en-US");

            //obtener la tasa de cambio desde la entidad fotosolicitud
            if (tipoMoneda != "us")
            {
                tasaCambio = GetTipoCambio(numSolicitud).ToString().Replace (".",",");
            }

            reporte_flujo = ConfigurationManager.AppSettings["ReporteFlujoCaja"];

            Reporte negReporte = new Reporte(Credenciales.ObtenerCredenciales());
            Guid idReport = negReporte.obtenerIdReporte(reporte_flujo);
            string pathReporte = ConfigurationManager.AppSettings["ReporteRuta"];
            string urlReporte = "";
            string organizRep = ConfigurationManager.AppSettings["ReporteOrganizacion"];

            string paginaRep = "ReportViewer.aspx?%2f" + organizRep;
            paginaRep += "%2fCustomReports%2f%7b";
            paginaRep += idReport.ToString();
            paginaRep += "%7d&rs:Command=Render&";
            paginaRep += "NroSolicitud=" + numSolicitud;
            paginaRep += "&FactorConversion=" + tasaCambio.ToString();

            urlReporte = pathReporte + paginaRep;

            Response.Redirect(urlReporte, true);
        }

        public decimal GetTipoCambio(string numSolicitud)
        {
            FotoSolicitudCredito fsc = new FotoSolicitudCredito(Credenciales.ObtenerCredenciales());
            TipoCambioResult tcr = new TipoCambioResult(1, 1, 1, 0, string.Empty);
            tcr = fsc.GetTipoCambioByNroSolicitud(numSolicitud);
            return (tcr.TCContable <= 0 ? 1 : tcr.TCContable);


        }
    }
}