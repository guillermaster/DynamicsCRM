using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk.Metadata;

namespace Efika.Crm.Plugins.SFA.ProductoOportunidad
{
    public class ValidaProductoOportunidad : IPlugin
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
               context.InputParameters["Target"] is Entity && context.Depth == 1)
            {
                //Obtiene el target business entity para los parametros de entrada
                entity = (Entity)context.InputParameters["Target"];

                //Verifica que la entidad representa una cuenta
                EntityReference idProduct = (EntityReference)entity.Attributes["productid"];
                Guid idProducto = idProduct.Id;
                EntityReference Oportunidad = (EntityReference)entity.Attributes["opportunityid"];
                Guid idOportunidad = Oportunidad.Id;
                
                try
                {
                    string idCliente = string.Empty;                    

                    #region Consulta Id Cliente
                    //****armo objeto queryattribute para hacer la consulta
                    QueryExpression qbaOportunidad = new QueryExpression();
                    qbaOportunidad.EntityName = "opportunity";
                    qbaOportunidad.ColumnSet = new ColumnSet();
                    string[] columnasSegmento = new string[] { "opportunityid", "customerid" };
                    qbaOportunidad.ColumnSet.AddColumns(columnasSegmento);

                    qbaOportunidad.Criteria.AddCondition(new ConditionExpression("opportunityid", ConditionOperator.Equal, idOportunidad.ToString()));
                    EntityCollection retrievedOportunidad = servicio.RetrieveMultiple(qbaOportunidad);

                    if (retrievedOportunidad.Entities.Count > 0)
                    {
                        idCliente = ((EntityReference)retrievedOportunidad.Entities[0]["customerid"]).Id.ToString();
                    }
                    #endregion

                    bool bandera = verifica_producto_oportunidad(idCliente, idProducto.ToString(), servicio);

                    if (bandera)
                    {
                        throw new InvalidPluginExecutionException("No se puede crear el producto.  El producto ya existe en otra oportunidad abierta con el mismo cliente.");
                    }
                }
                catch (System.Exception ex)
                {
                    tracingService.Trace(this.GetType().ToString() + "_PreCreate: {0} ", ex.ToString());
                    throw new InvalidPluginExecutionException(ex.ToString());
                }
            }
            else
            {
                return;
            }
            #endregion
        }


        public static bool verifica_producto_oportunidad(string idCliente, string idProducto, IOrganizationService service)
        {
            QueryExpression qe = new QueryExpression("opportunity");
            qe.ColumnSet.AddColumns("opportunityid");

            LinkEntity link1 = new LinkEntity();
            link1.LinkFromEntityName = "opportunity";
            link1.LinkToEntityName = "opportunityproduct";
            link1.LinkFromAttributeName = "opportunityid";
            link1.LinkToAttributeName = "opportunityid";
            link1.JoinOperator = JoinOperator.Inner;

            ConditionExpression exp = new ConditionExpression();
            exp.AttributeName = "productid";
            exp.Operator = ConditionOperator.Equal;
            exp.Values.Add(idProducto);
            link1.LinkCriteria.Conditions.Add(exp);

            ConditionExpression exp2 = new ConditionExpression();
            exp2.AttributeName = "statecode";
            exp2.Operator = ConditionOperator.Equal;
            exp2.Values.Add(0);

            ConditionExpression exp3 = new ConditionExpression();
            exp3.AttributeName = "customerid";
            exp3.Operator = ConditionOperator.Equal;
            exp3.Values.Add(idCliente);

            qe.Criteria = new FilterExpression();
            qe.Criteria.Conditions.Add(exp2);
            qe.Criteria.Conditions.Add(exp3);
            qe.LinkEntities.Add(link1);

            qe.Criteria.FilterOperator = LogicalOperator.And;

            RetrieveMultipleRequest retrieve1 = new RetrieveMultipleRequest();
            retrieve1.Query = qe;
            RetrieveMultipleResponse response = (RetrieveMultipleResponse)service.Execute(retrieve1);
            EntityCollection bec = response.EntityCollection;
            if (bec.Entities.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}
