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

namespace Efika.Crm.Plugins.MA.ListaMarketing
{
    public class WAImportarListaMarketing: CodeActivity
    {
        #region Parametros

        [Input("Id Lista")]
        [ReferenceTarget("efk_importacion_listas_marketing")]
        public InArgument<EntityReference> ListaMarketing { get; set; }

        [Input("Carpeta Log")]
        public InArgument<string> CarpetaLog { get; set; }

        #endregion

        #region Eventos

        protected override void Execute(CodeActivityContext executionContext)
        {
            ITracingService tracingService = null;
            try
            {
                //traza del workflow
                tracingService = executionContext.GetExtension<ITracingService>();
                //creando el contexto del Workflow
                tracingService.Trace("Creando enlace a servicio CRM.");
                IWorkflowContext contexto = executionContext.GetExtension<IWorkflowContext>();
                IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
                IOrganizationService ServicioCRM = serviceFactory.CreateOrganizationService(contexto.UserId);
                tracingService.Trace("Enlace a servicio creado.");
                Guid IdLista = ListaMarketing.Get(executionContext).Id;

                ProcesarListaMarketing plm = new ProcesarListaMarketing(ServicioCRM, tracingService, contexto, IdLista);
                plm.CarpetaLog = CarpetaLog.Get<string>(executionContext);
                plm.EliminarArchivos();
                plm.EjecutarImportacion();

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
