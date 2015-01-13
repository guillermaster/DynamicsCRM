using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Efika.Crm.AccesoServicios.CRMSDK;
using Efika.Crm.Entidades;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Efika.Crm.AccesoServicios
{
    public class ServicioCRM
    {
        public static CrmService ObtenerServicioCRM(CredencialesCRM credenciales)
        {
            CrmAuthenticationToken token = new CrmAuthenticationToken();
            token.OrganizationName = credenciales.Organizacion;

            CrmService service = new CrmService();
            if (credenciales.Usuario != null && credenciales.Password != null)
            {
                service.Credentials = new System.Net.NetworkCredential(credenciales.Usuario, credenciales.Password, credenciales.Dominio);
            }
            else
            {
                service.Credentials = System.Net.CredentialCache.DefaultCredentials;
            }
            service.CrmAuthenticationTokenValue = token;
            return service;
        }


        public static CRMMETADATA.MetadataService ObtenerServicioMetaData(CredencialesCRM credenciales)
        {
            CRMMETADATA.CrmAuthenticationToken token = new CRMMETADATA.CrmAuthenticationToken();
            token.OrganizationName = credenciales.Organizacion;

            CRMMETADATA.MetadataService service = new CRMMETADATA.MetadataService();
            if (credenciales.Usuario != null && credenciales.Password != null)
            {
                service.Credentials = new System.Net.NetworkCredential(credenciales.Usuario, credenciales.Password, credenciales.Dominio);
            }
            else
            {
                service.Credentials = System.Net.CredentialCache.DefaultCredentials;
            }
            service.CrmAuthenticationTokenValue = token;
            return service;
        }


        public static OrganizationServiceProxy ObtenerServicioCRMCargas(string direccionServidor, string organizacion)
        {
            System.ServiceModel.Description.ClientCredentials credenciales = new ClientCredentials();
            credenciales.Windows.ClientCredential = (System.Net.NetworkCredential)System.Net.CredentialCache.DefaultCredentials;

            OrganizationServiceProxy servicio = new OrganizationServiceProxy(new Uri(direccionServidor + "/" + organizacion + "/XRMServices/2011/Organization.svc"), null, credenciales, null);

            return servicio;
        }
    }
}
