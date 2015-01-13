using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;
using Efika.Crm.AccesoServicios.CRMSDK;
using Efika.Crm.AccesoServicios;
using Efika.Crm.Entidades;


namespace Efika.Crm.Negocio
{
    public class ImportacionListaMarketing
    {
        private OrganizationDetail currentOrganizationDetail { get; set; }
        private IOrganizationService orgService { get; set; }
        private CrmService Servicio;
        

        public ImportacionListaMarketing(string organizacion, string servidor)
        {
            DiscoveryServiceProxy dsp;
            var creds = new ClientCredentials();

            if (String.IsNullOrWhiteSpace(organizacion))
            {
                throw new ApplicationException("Debe indicar el nombre de la organización");
            }
            if (String.IsNullOrWhiteSpace(servidor))
            {
                throw new ApplicationException("Debe indicar el nombre del servidor");
            }
            
            IServiceConfiguration<IDiscoveryService> dinfo = ServiceConfigurationFactory.CreateConfiguration<IDiscoveryService>(GetDiscoveryServiceUri(servidor));

            dsp = new DiscoveryServiceProxy(dinfo, creds);
            dsp.Authenticate();

            RetrieveOrganizationsRequest orgRequest = new RetrieveOrganizationsRequest();
            RetrieveOrganizationsResponse orgResponse = dsp.Execute(orgRequest) as RetrieveOrganizationsResponse;

            foreach (var details in orgResponse.Details)
            {
                if (details.UniqueName == organizacion)
                {
                    currentOrganizationDetail = details as OrganizationDetail;
                    break;
                }
            }
            
            Uri orgServiceUri = new Uri(currentOrganizationDetail.Endpoints[EndpointType.OrganizationService]);
            IServiceConfiguration<IOrganizationService> orgConfigInfo =
                                        ServiceConfigurationFactory.CreateConfiguration<IOrganizationService>(orgServiceUri);
            orgService = (IOrganizationService) new OrganizationServiceProxy(orgConfigInfo, creds);

            
        }

        public ImportacionListaMarketing(CredencialesCRM credenciales)
        {
            Servicio = ServicioCRM.ObtenerServicioCRM(credenciales);
        }


        public Uri GetDiscoveryServiceUri(string serverName)
        {
            string discoSuffix = @"/XRMServices/2011/Discovery.svc";
            return new Uri(string.Format("{0}{1}", serverName, discoSuffix));
        }

        /** Guarda un nuevo registro en la entidad importación lista de marketing **/
        public Guid GuardarImpListaMarketing(Entidades.ImportacionListaMarketing dato, ref string resultadoError)
        {
            efk_importacion_listas_marketing objListMark = new efk_importacion_listas_marketing();
            objListMark.efk_nombre = dato.Nombre;
            objListMark.efk_ruta_archivo_carga = dato.RutaArchivoCarga;
            objListMark.efk_fecha_inicio_ejecucion = new CrmDateTime();
            objListMark.efk_fecha_inicio_ejecucion.Value = dato.FechaInicioEjecucion.ToString();
            objListMark.efk_estado_carga = new Picklist();
            objListMark.efk_estado_carga.Value = 100000000;
            objListMark.ownerid = new Owner();
            objListMark.ownerid.type = "systemuser";
            objListMark.ownerid.Value = new Guid(dato.IdPropietario);
            return Servicio.Create(objListMark);
        }
                
       
        public Guid AdjuntarArchivoRegistroImportacion(Guid IdRegistroImportacion, string RutaArchivo, string NombreArchivo)
        {
            Stream lector = File.OpenRead(RutaArchivo);
            return AdjuntarArchivoRegistroImportacion(IdRegistroImportacion, lector, NombreArchivo);
        }

        /** Crea una nota con un archivo adjunto y la relaciona con el registro de la entidad importación de listas de marketing **/
        public Guid AdjuntarArchivoRegistroImportacion(Guid IdRegistroImportacion, Stream fileStream, string NombreArchivo)
        {
            byte[] datosLog = new byte[fileStream.Length];
            fileStream.Read(datosLog, 0, datosLog.Length);
            string datosCodificados = Convert.ToBase64String(datosLog);

            annotation objAnnotation = new annotation();
            objAnnotation.subject = "Archivo de carga";
            objAnnotation.filename = NombreArchivo;
            objAnnotation.documentbody = datosCodificados;
            if (NombreArchivo.Substring(NombreArchivo.LastIndexOf(".")).ToUpper().Equals(".XLS"))
                objAnnotation.mimetype = @"application/vnd.ms-excel";
            else
                objAnnotation.mimetype = @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            
            // agregar relación entre registro de nota y registro de importación de listas de marketing
            Lookup reference = new Lookup();
            reference.Value = IdRegistroImportacion;
            objAnnotation.objectid = new Lookup();
            objAnnotation.objectid.type = EntityName.efk_importacion_listas_marketing.ToString();
            objAnnotation.objectid.Value = IdRegistroImportacion;
            objAnnotation.objecttypecode = new EntityNameReference();
            objAnnotation.objecttypecode.Value = EntityName.efk_importacion_listas_marketing.ToString();

            return Servicio.Create(objAnnotation);
        }

        public void ActualizarListaMarketing(Guid IdRegistroImportacion, Guid IdNota)
        {
            efk_importacion_listas_marketing registro = new efk_importacion_listas_marketing();
            registro.efk_importacion_listas_marketingid = new Key();
            registro.efk_importacion_listas_marketingid.Value=IdRegistroImportacion;
            registro.efk_ruta_archivo_carga = IdNota.ToString();

            Servicio.Update(registro);
        }

        
    }
}
