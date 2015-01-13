using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Efika.Crm.AccesoServicios.CRMSDK;
using Efika.Crm.AccesoServicios;
using Efika.Crm.Entidades;


namespace Efika.Crm.Negocio
{
    public class ImportacionDatosPreAprobados
    {
        private CrmService Servicio;

        public ImportacionDatosPreAprobados(CredencialesCRM credenciales)
        {
            Servicio = ServicioCRM.ObtenerServicioCRM(credenciales);
        }

        public Guid GuardarListaPreAprobado(Entidades.ImportacionDatosPreAprobados datoPreAp, ref string resultadoError)
        {
            AccesoServicios.CRMSDK.efk_importacion_datos_preaprobados objDatPreAp = new efk_importacion_datos_preaprobados();
            objDatPreAp.efk_name = datoPreAp.Nombre;
            objDatPreAp.efk_fecha_inicio_ejecucion = new CrmDateTime();
            objDatPreAp.efk_fecha_inicio_ejecucion.Value = datoPreAp.FechaInicioEjecucion.ToString();
            objDatPreAp.efk_estado_carga = new Picklist();
            objDatPreAp.efk_estado_carga.Value = 100000000;
            objDatPreAp.efk_ruta_archivo_carga = datoPreAp.RutaArchivoCarga;
            // relacionar con campaña
            Lookup refCampania = new Lookup();
            refCampania.Value = datoPreAp.IdCampania;
            refCampania.type = EntityName.campaign.ToString();
            objDatPreAp.efk_campaignid = refCampania;
            objDatPreAp.ownerid = new Owner();
            objDatPreAp.ownerid.type = "systemuser";
            objDatPreAp.ownerid.Value = new Guid(datoPreAp.IdPropietario);

            return Servicio.Create(objDatPreAp);
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
            objAnnotation.objectid.type = EntityName.efk_importacion_datos_preaprobados.ToString();
            objAnnotation.objectid.Value = IdRegistroImportacion;
            objAnnotation.objecttypecode = new EntityNameReference();
            objAnnotation.objecttypecode.Value = EntityName.efk_importacion_datos_preaprobados.ToString();

            return Servicio.Create(objAnnotation);
        }

        public void ActualizarImportacionDatosPreAprobados(Guid IdRegistroImportacion, Guid IdNota)
        {
            efk_importacion_datos_preaprobados registro = new efk_importacion_datos_preaprobados();
            registro.efk_importacion_datos_preaprobadosid = new Key();
            registro.efk_importacion_datos_preaprobadosid.Value = IdRegistroImportacion;
            registro.efk_ruta_archivo_carga = IdNota.ToString();

            Servicio.Update(registro);
        }
    }
}
