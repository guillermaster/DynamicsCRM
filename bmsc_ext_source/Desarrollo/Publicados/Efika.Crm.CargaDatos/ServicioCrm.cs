using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Efika.Crm.CargaDatos.crmSDK;

namespace Efika.Crm.CargaDatos
{
    public class ServicioCrm
    {
        #region Variables
        public static string strFechaLog;
        public static string strHoraLog;
        public static string strRutaArchivo;
        public static string strNombreArchivo;
        public static string strExtensionArchivo;
        #endregion
        public static string strUsuAdmCRM;
        public static string strPwdAdmCRM;
        public static string strDominio;

        public static string strUsuarioLoginServicioCRM;

        private static CrmService ObjServicio;

        //Autenticación con el servidor del banco
        public static bool validarCertificado(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificado, 
            System.Security.Cryptography.X509Certificates.X509Chain cadena, System.Net.Security.SslPolicyErrors sslErrores)
        {
            return true;
        }
        

        public static CrmService GetCrmService(string strOrganizacion, string strServidor, string strPuerto)
        {
            string strUrlServer;

            if (strPuerto.Length == 0)
                strUrlServer = "http://" + strServidor;
            else
                strUrlServer = "http://" + strServidor + ":" + strPuerto;

            UriBuilder builder = new UriBuilder(strUrlServer);
            builder.Path = "//MSCRMServices//2007//CrmService.asmx";

            CrmAuthenticationToken objToken = new CrmAuthenticationToken();

            if (ServicioCrm.ObjServicio == null)
            {

                CrmService objServicio = new CrmService();
                objToken.OrganizationName = strOrganizacion;
                objServicio.Url = builder.Uri.ToString();
                objServicio.CrmAuthenticationTokenValue = objToken;
                objServicio.UnsafeAuthenticatedConnectionSharing = true;
                objServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
                objServicio.CrmAuthenticationTokenValue = objToken;
                objServicio.Proxy = null;
                objServicio.PreAuthenticate = true;
                ServicioCrm.ObjServicio = objServicio;
            }
            return ServicioCrm.ObjServicio;

        }
    }
}
