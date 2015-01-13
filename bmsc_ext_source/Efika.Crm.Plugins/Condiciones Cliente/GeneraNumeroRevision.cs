using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Efika.Crm.Plugins.CondicionesCliente
{
    public class GeneraNumeroRevision : IPlugin
    {
  
         public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);

            IOrganizationService servicio = ((IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory))).CreateOrganizationService(new Guid?(context.UserId));

            
            
            try
            {
                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity /*&& context.Depth == 1*/)
                {
                    Entity entity = (Entity)context.InputParameters["Target"];

                    if (entity.LogicalName != "efk_revision_condiciones_cliente")
                        return;

                    //Validamos si existe otro cliente con el mismo segmento
                    if (entity.Attributes.Contains("efk_condicion_clienteid")) //efk_segmento
                    {
                        EntityReference idcondicioncliente = (EntityReference)entity.Attributes["efk_condicion_clienteid"];

                        string NOMBRE = entity.Attributes["efk_nombre"].ToString();

                        QueryExpression qe = new QueryExpression("efk_revision_condiciones_cliente");
                        qe.ColumnSet.AddColumn("efk_revision_condiciones_clienteid");
                        qe.Criteria = new FilterExpression(LogicalOperator.And);
                        qe.Criteria.AddCondition(new ConditionExpression("efk_condicion_clienteid", ConditionOperator.Equal, idcondicioncliente.Id.ToString()));
                        qe.Criteria.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, 0));
                        EntityCollection queryCondiciones = servicio.RetrieveMultiple(qe);

                        int contador = queryCondiciones.Entities.Count;

                        entity.Attributes["efk_nombre"] = NOMBRE + " " + contador.ToString();

                        service.Update(entity);

                        ColumnSet columnas = new ColumnSet("efk_condicion_clienteid", "efk_numero_revisiones");

                        Entity condicion_cliente = service.Retrieve("efk_condicion_cliente", idcondicioncliente.Id, columnas);

                         if (condicion_cliente.Attributes.Contains("efk_numero_revisiones"))
                         {
                            condicion_cliente.Attributes["efk_numero_revisiones"] =contador.ToString();
                         }
                         else{
                             condicion_cliente.Attributes.Add("efk_numero_revisiones", contador.ToString());
                         }

                        service.Update(condicion_cliente);

                    }

                }
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("efk_revision_condiciones_cliente: " + ex.Message.ToString(), ex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }    
    }
}

