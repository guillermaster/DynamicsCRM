using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System;
using System.ServiceModel;
using System.Collections;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Workflow;

namespace Efika.Crm.Plugins.MA.DatosPreAprobados
{
    public class WAImportarDatosPreAprobados: CodeActivity
    {
        #region Parametros

        [Input("Id Importacion")]
        [ReferenceTarget("efk_importacion_datos_preaprobados")]
        public InArgument<EntityReference> PreAprobados { get; set; }

        [Input("Carpeta Log")]
        public InArgument<string> CarpetaLog { get; set; }

        #endregion

        #region Propiedades

        IOrganizationService ServicioCRM { get; set; }
        ITracingService tracingService = null;
        IWorkflowContext contexto = null;

        #endregion

        #region Eventos

        protected override void Execute(CodeActivityContext executionContext)
        {
            tracingService = null;
            try
            {
                //traza del workflow
                tracingService = executionContext.GetExtension<ITracingService>();
                //creando el contexto del Workflow
                tracingService.Trace("Creando enlace a servicio CRM.");
                contexto = executionContext.GetExtension<IWorkflowContext>();
                IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
                ServicioCRM = serviceFactory.CreateOrganizationService(contexto.UserId);
                tracingService.Trace("Enlace a servicio creado.");
                Guid IdLista = PreAprobados.Get(executionContext).Id;

                ProcesarPreaprobados ppa = new ProcesarPreaprobados(ServicioCRM, tracingService, contexto, IdLista);
                ppa.CarpetaLog = CarpetaLog.Get<string>(executionContext);
                ppa.EjecutarImportacion();
                ppa.EliminarArchivos();
                return;
            }
            catch (Exception ex)
            {
                tracingService.Trace("Error en método principal de ejecución: " + ex.Message);
                throw ex;
            }
        }

        #endregion
    }
}
