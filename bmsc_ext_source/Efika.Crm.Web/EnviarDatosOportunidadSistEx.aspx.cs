using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Efika.Crm.AccesoServicios.CRMSDK;
using Microsoft.Xrm.Sdk;
using System.Configuration;
using Efika.Crm.Negocio;
using Efika.Crm.AccesoServicios;
using Efika.Crm.Entidades.Common;
using System.Net;

namespace Efika.Crm.Web
{
    public partial class CaratulaOnBase : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ServicePointManager.ServerCertificateValidationCallback += MyCertificatePolicy.ValidateCertificate;

            if (!IsPostBack)
            {
                if (!Utilidades.ValidarAccesosWeb(Request.QueryString[Utilidades.NombresParametros.Usuario], Privilegios.NombresPrivilegios.LeerOportunidad))
                {
                    Response.Write(@"<script type=""text/javascript""> alert('No tiene privilegios para acceder a este formulario.'); window.close(); </script>");
                    return;
                }
            }

            EnviarSolicitudOportunidadSistemaExterno();
        }


        protected void EnviarSolicitudOportunidadSistemaExterno()
        {
            try
            {
                Oportunidad negOport = new Oportunidad(Credenciales.ObtenerCredenciales());
                Entidades.Oportunidad oportunidad = GetDatosOportunidad(new Guid(Request.QueryString[Utilidades.NombresParametros.OportunidadId]));
                                
                if (!negOport.ExisteEnvioSolicitudSistExt(oportunidad.Id))
                {//si no se registra un envío previo de solicitud a sistema externo
                    negOport.SetEnvioSolicitudSistemaExterno(oportunidad.Id, true);
                    EnviarOfetaARiesgo(oportunidad);                    
                }
                RetornarValor();
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this, Request.QueryString[Utilidades.NombresParametros.Usuario], "No se pudieron enviar los datos", "ENVIO_DATOSOPORTUNIDAD_SISTEMAEXTERNO", ex);
            }
        }


        private bool EnviarDatos(Entidades.Cliente cliente, Entidades.Oportunidad oportunidad, Entidades.Producto productoOportunidad)
        {
            try
            {
                Entidades.CredencialesCRM credencCrm = Credenciales.ObtenerCredenciales();
                Entidades.CredencialesBD credencDB = Credenciales.ObtenerCredendialesBD_BDI();
                string dbConnStr = Credenciales.CadenaConexionBD(credencDB);

                ServicePointManager.ServerCertificateValidationCallback += MyCertificatePolicy.ValidateCertificate;

                BMSCOnBase.CaratulaOnbaseService webServ = new BMSCOnBase.CaratulaOnbaseService();
                BMSCOnBase.caratulaOnBase_request reqEnvDatos = new BMSCOnBase.caratulaOnBase_request();
                reqEnvDatos.pApellidos = cliente.Apellido1 + " " + cliente.Apellido2;
                reqEnvDatos.pEmail = cliente.Email;
                reqEnvDatos.pMontoFinanciado = oportunidad.MontoSolicitado;
                reqEnvDatos.pNombres = cliente.Nombre;
                reqEnvDatos.pNroIdentificacion = cliente.Identificacion;
                reqEnvDatos.pNroSolicitud = oportunidad.NumeroSolicitud;
                reqEnvDatos.pRazonSocial = cliente.RazonSocial;
                reqEnvDatos.pTipoCliente = Negocio.Cliente.DatosBDI_TipoCliente(dbConnStr, cliente.TipoClienteId.ToString());
                reqEnvDatos.pTipoCredito = productoOportunidad.Id.ToString();
                reqEnvDatos.pTipoIdentificacion = Negocio.Cliente.DatosBDI_TipoIdentificacion(dbConnStr, cliente.TipoIdentificacion.ToString());
                reqEnvDatos.pTipoTrabajo = Negocio.Cliente.DatosBDI_TipoTrabajo(dbConnStr, cliente.FuenteIngresoOfValId.ToString());
                reqEnvDatos.pContrasena = credencCrm.Password;
                reqEnvDatos.pDominio = credencCrm.Dominio;
                reqEnvDatos.pUsuario = credencCrm.Usuario;
                BMSCOnBase.caratulaOnBase_response respEnvDatos = webServ.CaratulaOnbase(reqEnvDatos);
                if (respEnvDatos.pRespuesta == "0")
                    return true;
                else
                    throw new Exception(respEnvDatos.pMensaje);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private Entidades.Cliente GetDatosCliente(Guid clienteId)
        {
            try
            {
                Cliente negCliente = new Cliente(Credenciales.ObtenerCredenciales());
                AccesoServicios.CRMSDK.ColumnSet cs = new AccesoServicios.CRMSDK.ColumnSet();
                cs.Attributes = new string[] { "name", "efk_nombre_persona", "efk_tipo_cliente", "efk_tipo_identificacion", "efk_fuente_ingresos_ov",
                                            "accountnumber", "efk_primerapellido", "efk_segundoapellido", "emailaddress1"};
                Entidades.Cliente cliente = negCliente.DatosCliente(clienteId, cs);
                return cliente;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private Entidades.Oportunidad GetDatosOportunidad(Guid oportunidadId)
        {
            try
            {
                Oportunidad negOportunidad = new Oportunidad(Credenciales.ObtenerCredenciales());
                AccesoServicios.CRMSDK.ColumnSet cs = new AccesoServicios.CRMSDK.ColumnSet();
                cs.Attributes = new string[] { "efk_monto_solicitado", "efk_nrosolicitud", "efk_numero_oferta" };
                Entidades.Oportunidad oportunidad = negOportunidad.GetOportunidad(oportunidadId, cs);
                return oportunidad;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private Entidades.Producto GetProductoOportunidad(Guid oportunidadId)
        {
            try
            {
                Oportunidad negOport = new Oportunidad(Credenciales.ObtenerCredenciales());
                Entidades.Producto producto = negOport.GetProductoOportunidadSimulacion(oportunidadId);
                return producto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void MostarReporte(Entidades.Cliente cliente, Entidades.Oportunidad oportunidad, Entidades.Producto producto)
        {
            Reporte negReporte = new Reporte(Credenciales.ObtenerCredenciales());
            Guid idReport = negReporte.obtenerIdReporte(ConfigurationManager.AppSettings["ReporteSistExt1"]);
            string pathReporte = ConfigurationManager.AppSettings["ReporteRuta"];
            string urlReporte = "";
            string organizRep = ConfigurationManager.AppSettings["ReporteOrganizacion"];

            string paginaRep = "ReportViewer.aspx?%2f" + organizRep;
            paginaRep += "%2fCustomReports%2f%7b";
            paginaRep += idReport.ToString();
            paginaRep += "%7d&rs:Command=Render&";
            paginaRep += "NumSolicitud=" + oportunidad.NumeroSolicitud + "&";
            paginaRep += "TipoCredito=" + producto.Nombre + "&";
            paginaRep += "Monto=" + oportunidad.MontoSolicitado + "&";
            paginaRep += "TipoIdent=" + cliente.TipoIdentificacionDesc + "&";
            paginaRep += "NumIdent=" + cliente.Identificacion + "&";
            paginaRep += "Nombres=" + cliente.Nombre + "&";
            paginaRep += "Apellidos=" + cliente.Apellido1 + " " + cliente.Apellido2 + "&";
            paginaRep += "Email=" + cliente.Email + "&";
            paginaRep += "TipoTrabajo=" + cliente.FuenteIngresoOfValDesc + "&";
            paginaRep += "RazonSocial=" + cliente.RazonSocial + "&";

            if (cliente.TipoClienteId == Entidades.Cliente.TipoCliente.Natural)
            {
                paginaRep += "TipoCliente=N";
            }
            else
            {
                paginaRep += "TipoCliente=J";
            }

            urlReporte = pathReporte + paginaRep;
            Response.Redirect(urlReporte, true);
        }

        private void RetornarValor()
        {
            string jscript0 = "<script type=\"text/javascript\"> alert('La oportunidad ha sido enviada exitosamente.'); </script>";
            string jscript1 = "<script type=\"text/javascript\"> window.returnValue = 'S';  </script>";
            string jscript2 = "<script type=\"text/javascript\"> window.close();  </script>";
            ClientScript.RegisterStartupScript(GetType(), "ENVIA_DATOS_SISTEXT0", jscript0);
            ClientScript.RegisterStartupScript(GetType(), "ENVIA_DATOS_SISTEXT1", jscript1);
            ClientScript.RegisterStartupScript(GetType(), "ENVIA_DATOS_SISTEXT2", jscript2);
        }



        private void EnviarOfetaARiesgo(Entidades.Oportunidad oportunidad)
        {
            try
            {
                Oferta negOferta = new Oferta(Credenciales.ObtenerCredenciales());
                negOferta.EnviarOfertaARiesgos(oportunidad);
            }
            catch (Exception e)
            {
                Utilidades.ReportarError(this, Request.QueryString[Utilidades.NombresParametros.Usuario], 
                    "No se pudo enviar la oferta a riesgos.", "ENVIO_DATOS_OFERTA_RIESGO", e);
            }
        }
    
    
    
    }
}