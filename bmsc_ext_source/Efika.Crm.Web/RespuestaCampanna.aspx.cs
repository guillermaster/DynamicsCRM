using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.SessionState;
using System.Reflection;
using System.Text;
using Efika.Crm.Negocio;

namespace Efika.Crm.Web
{
    public partial class RespuestaCampanna : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ConfigurarPagina();
            try
            {
                if (!IsPostBack)
                {
                    if (!Utilidades.ValidarAccesosWeb(Request.QueryString[Utilidades.NombresParametros.Usuario], Privilegios.NombresPrivilegios.RespuetaCampana))
                    {
                        Response.Write(@"<script type=""text/javascript""> alert('No tiene privilegios para acceder a este formulario.'); window.close(); </script>");
                        return;
                    }

                    CrearOportunidad();
                }
            }
            catch (Exception ex)
            {
                Utilidades.AgregarErrorAlLog(ex.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        Request.QueryString[Utilidades.NombresParametros.Usuario], ex.StackTrace);
            }
        }

        private void ConfigurarPagina()
        {
            Response.Expires = 0;
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            Response.AddHeader("pragma", "no-cache");
            Response.AddHeader("cache-control", "private");
            Response.CacheControl = "no-cache";
        }
        

        private DateTime GetCampaniaFechaFinProp(Guid idCampania)
        {
            DateTime campFechaFinProp;
            Negocio.Campania camp = new Negocio.Campania(Credenciales.ObtenerCredenciales());
            AccesoServicios.CRMSDK.ColumnSet cs = new AccesoServicios.CRMSDK.ColumnSet();
            cs.Attributes = new string[] { "proposedend" };
            if (camp.CargarCampania(idCampania, cs))
            {
                campFechaFinProp = camp.CampaniaActual.FechaFinPropuesta;
            }
            else
            {
                throw new Exception(camp.CampaniaActual.Errores);
            }
            return campFechaFinProp;
        }


        private void CrearOportunidad()
        {
            try
            {
                Guid idOportunidad;
                Negocio.Oportunidad oportunidad;
                Guid idCliente = new Guid(Request.QueryString[Utilidades.NombresParametros.ClienteId]);
                Guid idCampania = new Guid(Request.QueryString[Utilidades.NombresParametros.CampaniaId]);
                Guid idRespCampania = new Guid(Request.QueryString[Utilidades.NombresParametros.RespCampaniaId]);
                
                if (!esClienteDependiente(idCliente))//si no es cliente dependiente
                {
                    oportunidad = new Negocio.Oportunidad(Credenciales.ObtenerCredenciales());
                    string titulo = Request.QueryString[Utilidades.NombresParametros.Titulo];
                    DateTime fechaEstCierreOp = GetCampaniaFechaFinProp(idCampania);
                    idOportunidad = oportunidad.CrearOportunidadDesdeRespCamp(titulo, idCliente, idCampania, idRespCampania, fechaEstCierreOp);
                    oportunidad.AsignarPropietarioOportunidad(idOportunidad, new Guid(Request.QueryString[Utilidades.NombresParametros.Usuario]));
                    if (idOportunidad != Guid.Empty)
                    {
                        SetOportunidadCreada(idRespCampania);
                        Response.Write("<script type=\"text/javascript\"> alert(\"La oportunidad ha sido creada exitosamente.\"); </script>");
                    }
                    else
                    {
                        Response.Write("<script type=\"text/javascript\"> alert(\"La oportunidad no pudo ser creada.\"); </script>");
                    }
                    Response.Write("<script type=\"text/javascript\"> window.close(); </script>");
                }
                else//si el cliente ese dependiente
                {
                    AbrirSimulacionCrediticia(idCliente, idRespCampania);
                }
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this.Page, Request.QueryString[Utilidades.NombresParametros.Usuario], "Ha ocurrido un error al crear la oportunidad.", "RESPCAMP_A_OPORTUNIDAD", ex);
            }
        }


        private bool esClienteDependiente(Guid idCliente)
        {
            try
            {
                Cliente negCliente = new Cliente(Credenciales.ObtenerCredenciales());
                return negCliente.ClienteDependiente(idCliente);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void AbrirSimulacionCrediticia(Guid idCliente, Guid idRespCampania)
        {
            try
            {
                string codDivisa = CodMonedaOperacionCliente(idCliente);
                string url = "SimuladorCredito/SimuladorCredito.aspx?" + Utilidades.NombresParametros.Usuario + "=" + Request.QueryString[Utilidades.NombresParametros.Usuario] + "&" +
                    "&codDivisa={" + codDivisa + "}&accountid=" + idCliente + "&" +
                    Utilidades.NombresParametros.CampaniaId + "=" + Request.QueryString[Utilidades.NombresParametros.CampaniaId];

                string wndOpenScript = "<script language=\"javascript\" type=\"text/javascript\"> window.open('" + url + 
                    "', '', 'location=no,menubar=no,status=1,toolbar=no,resizable=1,scrollbars=1,width=800,height=600'); </script> ";

                ClientScript.RegisterStartupScript(GetType(), "CAMPANA_CREA_SIMULACION", wndOpenScript);
                SetOportunidadCreada(idRespCampania);
                ClientScript.RegisterStartupScript(GetType(), "CAMPANA_CIERRAVENTANA", "<script type=\"text/javascript\"> window.close(); </script>");
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this.Page, Request.QueryString[Utilidades.NombresParametros.Usuario], "Error al abrir simulación crediticia.", "RESPCAMP_A_SIMULACCRED", ex);
            }
        }

        private void SetOportunidadCreada(Guid idRespuestaCampania)
        {
            try
            {
                RespuestaCampania negRespCamp = new RespuestaCampania(Credenciales.ObtenerCredenciales());
                negRespCamp.SetOportunidadCreada(idRespuestaCampania, true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string CodMonedaOperacionCliente(Guid idCliente)
        {
            try
            {
                Cliente negCliente = new Cliente(Credenciales.ObtenerCredenciales());
                Guid monedaId = negCliente.GetMonedaOperacionCliente(idCliente);
                return monedaId.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}