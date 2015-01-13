using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Configuration;
using System.Web.Services;
using Efika.Crm.Entidades;
using Efika.Crm.Negocio;
using Efika.Crm.AccesoServicios;

namespace Efika.Crm.WebService
{
    /// <summary>
    /// Summary description for ServiciosCarga
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ServiciosCarga : System.Web.Services.WebService
    {

        [WebMethod]
        public int IngresarOptionSetValue(string esquemaOptionSet, string valor)
        {
            return ConjuntoOpcionesCRM.IngresarOptionSetValue(ConfigurationManager.AppSettings["CRMDireccionServidor"],
                ConfigurationManager.AppSettings["CRMorganizacion"], esquemaOptionSet, valor);
        }

        [WebMethod]
        public void PublicarOptionSetValue(string esquemaOptionSet)
        {
            ConjuntoOpcionesCRM.PublicarOptionSetValue(ConfigurationManager.AppSettings["CRMDireccionServidor"],
                ConfigurationManager.AppSettings["CRMorganizacion"], new string[] { esquemaOptionSet });
        }

        [WebMethod]
        public decimal DatosMonedaCliente(Guid transactioncurrencyid)
        {

            CredencialesBD credencialesbd;
            string rutaArchivoCredencialesBD = "";
            string organizacion = "";
            string dominio = "";
            string cadenaConexion;
            cadenaConexion = "Data Source={0};Initial Catalog={1};User Id={2};Password={3}";
            organizacion = ConfigurationManager.AppSettings["CRMorganizacion"];
            dominio = Environment.UserDomainName;
            rutaArchivoCredencialesBD = ConfigurationManager.AppSettings["BDRutaArchivoCredenciales"];
            credencialesbd = DPAPI.ObtenerVariablesConexionBDDPAPI(rutaArchivoCredencialesBD);
            cadenaConexion = string.Format(cadenaConexion, credencialesbd.Servidor, credencialesbd.Catalogo, credencialesbd.Usuario, credencialesbd.Password);
            return Negocio.Cliente.DatosMonedaCliente(cadenaConexion, transactioncurrencyid);
        }

    }
}
