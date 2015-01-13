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
using System.Data;


namespace Efika.Crm.WebService
{
    /// <summary>
    /// Summary description for Privilegios
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Privilegios : System.Web.Services.WebService
    {

        [WebMethod]
        public bool ValidarPrivilegio(string p_strPrivilegio, string p_strIdUsuario)
        {
            //Creamos una instancia de la clase de log
            Log cLog = null;
            string organizacion = "";
            string dominio = "";
            string rutaArchivoCredenciales = "";
            string cPathLog = "";
            CredencialesCRM credenciales;
            try
            {
                organizacion = ConfigurationManager.AppSettings["CRMorganizacion"];
                dominio = Environment.UserDomainName;
                rutaArchivoCredenciales = ConfigurationManager.AppSettings["CRMRutaArchivoCredenciales"];
                cPathLog = ConfigurationManager.AppSettings["CRMRutaLogs"];

                //Obtenemos las credenciales
                credenciales = DPAPI.ObtenerUsuarioPasswordDPAPI(rutaArchivoCredenciales, organizacion, dominio);

                return Efika.Crm.Negocio.Privilegios.ValidarPrivilegio(p_strPrivilegio, new Guid(p_strIdUsuario), credenciales);
            }
            catch (Exception ex)
            {
                cLog = new Log(cPathLog);
                cLog.Add(ex.Message, "Privilegios", "ValidarPrivilegio", User.ToString(), ex.StackTrace);
                throw new Exception(ex.Message);
            }
        }
    }
}
