using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk.Metadata;

namespace Efika.Crm.Plugins.SFA.Opportunity
{
    public class ValidaOportunidad : IPlugin
    {

        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);
            IOrganizationService servicio = ((IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory))).CreateOrganizationService(new Guid?(context.UserId));

            // Get a reference to the tracing service.
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            Entity entity = null;

            #region Verify execution context
            if (context.GetType().GetProperty("CallerOrigin").GetValue(context, null).GetType().Name != "ApplicationOrigin")
                return;

            //Chequea si los parametros de entrada contienen un target
            //y crea una operacion y el target es de tipo DynamicEntity.

            if (context.InputParameters.Contains("Target") &&
               context.InputParameters["Target"] is Entity)
            {
                //Obtiene el target business entity para los parametros de entrada
                entity = (Entity)context.InputParameters["Target"];

                //Verifica que la entidad representa una cuenta
                EntityReference idcliente = (EntityReference)entity.Attributes["customerid"];
                Guid clienteid = idcliente.Id;

                if (clienteid != null)
                {
                    try
                    {                       

                        #region Obtengo creditovencido Base Cliente

                        //****armo objeto queryattribute para hacer la consulta
                        QueryExpression qbaSegmento = new QueryExpression();
                        qbaSegmento.EntityName = "account";
                        qbaSegmento.ColumnSet = new ColumnSet();
                        string[] columnasSegmento = new string[] { "accountid", "efk_posee_creditos_vencidos_o_ejec" };
                        qbaSegmento.ColumnSet.AddColumns(columnasSegmento);

                        qbaSegmento.Criteria.AddCondition(new ConditionExpression("accountid", ConditionOperator.Equal, clienteid.ToString()));
                        qbaSegmento.Criteria.AddCondition(new ConditionExpression("efk_posee_creditos_vencidos_o_ejec", ConditionOperator.Equal, true));
                        EntityCollection retrievedSegmento = servicio.RetrieveMultiple(qbaSegmento);

                        if (retrievedSegmento.Entities.Count > 0)
                        {
                            throw new InvalidPluginExecutionException("El cliente tiene créditos vencidos o en ejecución no se le puede crear la Oportunidad.");
                        }

                        #endregion
                    }
                    catch (System.Exception ex)
                    {
                        tracingService.Trace(this.GetType().ToString() + "_PreCreate: {0} ", ex.ToString());
                        throw;
                    }
                }
                else
                {
                    return;
                }
            }

            #endregion
        }


    }
}
