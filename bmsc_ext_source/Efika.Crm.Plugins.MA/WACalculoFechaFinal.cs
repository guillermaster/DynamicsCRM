using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.ServiceModel;
using System.Collections;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Workflow;

namespace Efika.Crm.Plugins.MA
{
    public class WACalculoFechaFinal : CodeActivity
    {
        protected override void Execute(CodeActivityContext executionContext)
        {
            int cantidadDias = 0;
            DateTime fechaInicial=DateTime.Now;
            bool excluirSabados=true;
            bool excluirDomingos=true;
            bool excluirFeriados=true;
            
            //Obtenemos el objeto para el servicio de traicing.
            ITracingService tracingService = executionContext.GetExtension<ITracingService>();
            try
            {
                //Seteamos las variables
                cantidadDias = CantidadDias.Get(executionContext);
                fechaInicial = FechaInicial.Get(executionContext);
                excluirSabados = ExcluirSabados.Get(executionContext);
                excluirDomingos = ExcluirDomingos.Get(executionContext);
                excluirFeriados = ExcluirFeriados.Get(executionContext);

                //Obtenemos el objeto con la información de contexto de MS CRM
                IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
                IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
                IOrganizationService servicio = serviceFactory.CreateOrganizationService(context.UserId);

                DateTime fechaFinal = CalculoFechaFinal.CalcularFechaFinal(fechaInicial, cantidadDias, excluirSabados, excluirDomingos, excluirFeriados, servicio);

                //Devolvemos la fecha final
                FechaFinal.Set(executionContext, fechaFinal);
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("Ha ocurrido un error en el siguiente Workflow Activity.", ex);
            }
            catch (Exception ex)
            {
                tracingService.Trace("FollowupPlugin: {0}", ex.ToString());
                throw;
            }
        }

        #region Variables
        [Input("Fecha de inicio")]
        public InArgument<DateTime> FechaInicial { get; set; }

        [Input("Cantidad de dias")]
        public InArgument<int> CantidadDias { get; set; }

        [Input("Excluir Sábados")]
        public InArgument<bool> ExcluirSabados { get; set; }

        [Input("Excluir Domingos")]
        public InArgument<bool> ExcluirDomingos { get; set; }

        [Input("Excluir Feriados")]
        public InArgument<bool> ExcluirFeriados { get; set; }

        [Output("Fecha final")]
        public OutArgument<DateTime> FechaFinal { get; set; }
        #endregion
    }
}
