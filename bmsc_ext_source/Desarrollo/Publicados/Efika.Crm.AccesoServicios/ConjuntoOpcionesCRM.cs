using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Efika.Crm.Entidades;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Crm.Sdk.Messages;
using Efika.Crm.AccesoServicios.CRMMETADATA;

namespace Efika.Crm.AccesoServicios
{
    public class ConjuntoOpcionesCRM
    {
        public static int IngresarOptionSetValue(string direccionServidor, string organizacion, string esquemaOptionSet, string valor)
        {
            return IngresarOptionSetValue(ServicioCRM.ObtenerServicioCRMCargas(direccionServidor, organizacion), esquemaOptionSet, valor);
        }

        public static int IngresarOptionSetValue(OrganizationServiceProxy servicio, string esquemaOptionSet, string valor)
        {
            Microsoft.Xrm.Sdk.Messages.InsertOptionValueRequest req3 = new Microsoft.Xrm.Sdk.Messages.InsertOptionValueRequest();
            req3.OptionSetName = esquemaOptionSet;
            req3.Label = new Label(valor, 3082);

            Microsoft.Xrm.Sdk.Messages.InsertOptionValueResponse respuesta = (Microsoft.Xrm.Sdk.Messages.InsertOptionValueResponse)servicio.Execute(req3);

            RetrieveOptionSetRequest req = new RetrieveOptionSetRequest();
            req.Name = esquemaOptionSet;

            RetrieveOptionSetResponse res = (RetrieveOptionSetResponse)servicio.Execute(req);
            Microsoft.Xrm.Sdk.Metadata.OptionSetMetadata datos = (Microsoft.Xrm.Sdk.Metadata.OptionSetMetadata)res.OptionSetMetadata;

            foreach (Microsoft.Xrm.Sdk.Metadata.OptionMetadata opcion in datos.Options)
            {
                if (opcion.Label.LocalizedLabels[0].Label == valor)
                    return (int)opcion.Value;
            }

            return -1;
        }

        public static void PublicarOptionSetValue(string direccionServidor, string organizacion, string[] esquemasOptionSet)
        {
            PublicarOptionSetValue(ServicioCRM.ObtenerServicioCRMCargas(direccionServidor, organizacion), esquemasOptionSet);
        }

        public static void PublicarOptionSetValue(OrganizationServiceProxy servicio, string[] esquemasOptionSet)
        {
            string xml = "<importexportxml><optionsets>";

            foreach (string s in esquemasOptionSet)
            {
                xml += "<optionset>"+s+"</optionset>";
            }
            xml += "</optionsets></importexportxml>";

            PublishXmlRequest pub = new PublishXmlRequest();
            pub.ParameterXml = xml;
            servicio.Execute(pub);
        }

        public static List<string[]> ObtenerOptionSetValue(MetadataService metadataServ, string nombreEntidad, string nombreAtributo)
        {
            List<string[]> optionSet = new List<string[]>();

            try
            {
                Efika.Crm.AccesoServicios.CRMMETADATA.RetrieveAttributeRequest peticion = new Efika.Crm.AccesoServicios.CRMMETADATA.RetrieveAttributeRequest();

                peticion.EntityLogicalName = nombreEntidad;
                peticion.LogicalName = nombreAtributo;

                Efika.Crm.AccesoServicios.CRMMETADATA.RetrieveAttributeResponse res = (Efika.Crm.AccesoServicios.CRMMETADATA.RetrieveAttributeResponse)metadataServ.Execute(peticion);

                if (res != null)
                {
                    Efika.Crm.AccesoServicios.CRMMETADATA.PicklistAttributeMetadata pick = (Efika.Crm.AccesoServicios.CRMMETADATA.PicklistAttributeMetadata)res.AttributeMetadata;
                    foreach (Option opc in pick.Options)
                    {
                        string[] opcVal = new string[] { opc.Label.LocLabels[0].Label, opc.Value.Value.ToString() };
                        optionSet.Add(opcVal);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return optionSet;
        }
    }
}
