using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Efika.Crm.Negocio;
using Efika.Crm.AccesoServicios.CRMSDK;
using System.Globalization;


namespace Efika.Crm.Web.SimuladorCredito
{
    public partial class rptSimPagoScore : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Utilidades.ValidarAccesosWeb(Request.QueryString[Utilidades.NombresParametros.Usuario], Privilegios.NombresPrivilegios.LeerOportunidad))
            {
                Response.Write(@"<script type=""text/javascript""> alert('No tiene privilegios para acceder a este formulario.'); window.close(); </script>");
                return;
            }

            try
            {
                Entidades.Oportunidad oportunidad = GetOportunidad(new Guid(Request.QueryString[Utilidades.NombresParametros.OportunidadId]));
                Entidades.ProductoSimulado productoSimulado = GetProductoSimulado(oportunidad.ProductoSimuladoId);
                MostrarReporte(oportunidad, productoSimulado);
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this, Request.QueryString[Utilidades.NombresParametros.Usuario], "No se pudo generar el reporte.", "RPT_SIMULADORPAGOSCORE", ex);
            }
        }

        public void MostrarReporte(Entidades.Oportunidad oportunidad, Entidades.ProductoSimulado productoSim)
        {
            Reporte negReporte = new Reporte(Credenciales.ObtenerCredenciales());
            Guid idReport = negReporte.obtenerIdReporte(ConfigurationManager.AppSettings["ReporteSimuladorPagoScore"]);
            string pathReporte = ConfigurationManager.AppSettings["ReporteRuta"];
            string urlReporte = "";
            string organizRep = ConfigurationManager.AppSettings["ReporteOrganizacion"];

            string paginaRep = "ReportViewer.aspx?%2f" + organizRep;
            paginaRep += "%2fCustomReports%2f%7b";
            paginaRep += idReport.ToString();
            paginaRep += "%7d&rs:Command=Render&";
            paginaRep += "ProductoSimuladoId=" + oportunidad.ProductoSimuladoId + "&";
            paginaRep += "OpportunityId=" + oportunidad.Id + "&";
            paginaRep += "SimulacionId=" + productoSim.SimulacionCreditoId.ToString() + "&";
            paginaRep += "Cliente=" + oportunidad.ClienteId + "&";
            paginaRep += "ProductId=" + productoSim.ProductoId + "&";
            paginaRep += "ProductTipo=" + productoSim.ProductoTipoId + "&";
            paginaRep += "ProductFamilia=" + productoSim.ProductoFamiliaId + "&";
            paginaRep += "Plazo=" + oportunidad.NumCuotas + "&";

            string separador = "";
            separador = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
            CultureInfo usCulture;
            if (Request.UserLanguages[0] != null)
                usCulture = new CultureInfo(Request.UserLanguages[0]);
            else
                usCulture = new CultureInfo("en-US");
                        
            /*paginaRep += "TasaFija=" + oportunidad.TasaFija.ToString().Replace('.',',') + "&";
            paginaRep += "Tre=" + oportunidad.TRE.ToString().ToString().Replace('.', ',') + "&";
            paginaRep += "SprFij=" + oportunidad.Spread.ToString().Replace('.', ',') + "&";

            paginaRep += "TasVarPar=" + oportunidad.InicioTasaVariable + "&";*/
            paginaRep += "Orden=" + oportunidad.Orden + "&";
            paginaRep += "Amocad=" + productoSim.FrecuenciaAmortizacion  + "&";
            
            //paginaRep += "TasVar=" + (Convert.ToDecimal((oportunidad.TRE + oportunidad.Spread).ToString(), usCulture)).ToString(CultureInfo.InvariantCulture) + "&";
            //paginaRep += "MonSol=" + (Convert.ToDecimal(oportunidad.MontoSolicitado.ToString(), usCulture)).ToString(CultureInfo.InvariantCulture) + "&";

            paginaRep += "SegCesantia=" + (productoSim.SeguroCesantia ? "1" : "0")  + "&";
            paginaRep += "SegDesgrav=" + (productoSim.SeguroDesgravamen ? "1" : "0") + "&";
            paginaRep += "OpportunityName=" + oportunidad.Nombre;

            urlReporte = pathReporte + paginaRep;

            Utilidades.ReportarError(this.Page, Request.QueryString[Utilidades.NombresParametros.Usuario], "CargaReporte", "", new Exception(urlReporte));

            Response.Redirect(urlReporte, true);
        }


        private Entidades.Oportunidad GetOportunidad(Guid oportunidadId)
        {
            try
            {
                Oportunidad negOport = new Oportunidad(Credenciales.ObtenerCredenciales());
                Entidades.Oportunidad oportunidad = negOport.GetOportunidadDatosSimulacion(oportunidadId);
                return oportunidad;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Entidades.ProductoSimulado GetProductoSimulado(Guid prodSimuladoId)
        {
            try
            {
                SimulacionCredito negSimCred = new SimulacionCredito(Credenciales.ObtenerCredenciales());
                ColumnSet cs = new ColumnSet();
                cs.Attributes = new string[] { "efk_producto_catalogoid", "efk_amortizacion_cada", "efk_conseguro_cesantia", 
                                               "efk_conseguro_desgravamen", "efk_producto_simuladoid" };
                Entidades.ProductoSimulado prodSim = negSimCred.GetProductoSimulado(prodSimuladoId, cs);
                return prodSim;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}