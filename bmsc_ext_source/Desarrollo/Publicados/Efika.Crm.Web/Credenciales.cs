using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Efika.Crm.Entidades;

namespace Efika.Crm.Web
{
    public class Credenciales
    {
        public static CredencialesCRM ObtenerCredenciales()
        {
            string organizacion = "";
            string dominio = "";
            string rutaArchivoCredenciales = "";
            string cPathLog = "";
            CredencialesCRM credenciales;

            organizacion = ConfigurationManager.AppSettings["CRMOrganizacion"];
            dominio = Environment.UserDomainName.ToUpper ();
            rutaArchivoCredenciales = ConfigurationManager.AppSettings["CRMRutaArchivoCredenciales"];
            cPathLog = ConfigurationManager.AppSettings["CRMRutaLogs"];

            //Obtenemos las credenciales
            credenciales = DPAPI.ObtenerUsuarioPasswordDPAPI(rutaArchivoCredenciales, organizacion, dominio);
                       

            return credenciales;
        }

        private static CredencialesBD ObtenerCredencialesBD(string rutaArchivoCredenc, string rutaArhivoLog)
        {
            string rutaArchivoCredencialesBD = "";
            string cPathLog = "";
            CredencialesBD credenciales;

            rutaArchivoCredencialesBD = ConfigurationManager.AppSettings[rutaArchivoCredenc];
            cPathLog = ConfigurationManager.AppSettings[rutaArhivoLog];

            //Obtenemos las credenciales
            credenciales = DPAPI.ObtenerVariablesConexionBDDPAPI(rutaArchivoCredencialesBD);
                       

            return credenciales;
        }

        public static CredencialesBD ObtenerCredendialesBD_BDI()
        {
            return ObtenerCredencialesBD("BDRutaArchivoCredenciales", "CRMRutaLogs");
            
        }


        public static string CadenaConexionBD(CredencialesBD credencialesbd)
        {
            String cadenaconexion = "Data Source={0};Initial Catalog={1};User Id={2};Password={3}";
            cadenaconexion = string.Format(cadenaconexion, credencialesbd.Servidor, credencialesbd.Catalogo, credencialesbd.Usuario, credencialesbd.Password);
            return cadenaconexion;
        }

        
    }
}