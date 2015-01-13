using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Efika.Crm.Negocio;

namespace Efika.Crm.Web.SimuladorCredito
{
    public partial class rptModeloEvaluador : System.Web.UI.Page
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

        public void MostrarReporte(string numSolicitud)
        {
            Reporte negReporte = new Reporte(Credenciales.ObtenerCredenciales());
            Guid idReport = negReporte.obtenerIdReporte(ConfigurationManager.AppSettings["ReporteModeloEvaluador"]);
            string pathReporte = ConfigurationManager.AppSettings["ReporteRuta"];
            string urlReporte = "";
            string organizRep = ConfigurationManager.AppSettings["ReporteOrganizacion"];

            string paginaRep = "ReportViewer.aspx?%2f" + organizRep;
            paginaRep += "%2fCustomReports%2f%7b";
            paginaRep += idReport.ToString();
            paginaRep += "%7d&rs:Command=Render&";
            paginaRep += "NroSolicitud=" + numSolicitud;

            urlReporte = pathReporte + paginaRep;

            Response.Redirect(urlReporte, true);
        }
    }
}