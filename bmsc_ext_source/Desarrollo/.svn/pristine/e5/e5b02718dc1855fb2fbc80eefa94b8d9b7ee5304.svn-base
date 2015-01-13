using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Efika.Crm.Plugins
{
    public class CrearElementosListaPrecios : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);

            IOrganizationService servicio = ((IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory))).CreateOrganizationService(new Guid?(context.UserId));

            // Get a reference to the tracing service.
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            Guid idProducto;


            try
            {
                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity && context.Depth == 1)
                {
                    Entity entity = (Entity)context.InputParameters["Target"];

                    if (entity.LogicalName != "product")
                        return;

                    idProducto = entity.Id;


                    //Obtenemos la lista de precio
                    QueryExpression qe = new QueryExpression("pricelevel");
                    qe.Criteria = new FilterExpression(LogicalOperator.And);
                    qe.ColumnSet.AddColumn("pricelevelid");
                    qe.Criteria.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, 0));

                    EntityCollection listaPrecios = servicio.RetrieveMultiple(qe);

                    //Obtenemos la unidad
                    qe = new QueryExpression("uom");
                    qe.ColumnSet.AddColumn("uomid");

                    EntityCollection unidades = servicio.RetrieveMultiple(qe);

                    //Obtenemos los elementos existentes
                    qe = new QueryExpression("productpricelevel");
                    qe.ColumnSet.AddColumn("pricelevelid");
                    qe.Criteria = new FilterExpression(LogicalOperator.And);
                    qe.Criteria.AddCondition(new ConditionExpression("productid", ConditionOperator.Equal, idProducto));


                    EntityCollection elementos = servicio.RetrieveMultiple(qe);


                    if (listaPrecios.Entities.Count > 0 && unidades.Entities.Count > 0)
                    {
                        bool elementoExistente = false;
                        foreach (Entity lista in listaPrecios.Entities)
                        {
                            elementoExistente = false;
                            //primero buscamos si ya existe el elemento
                            foreach (Entity ele in elementos.Entities)
                            {
                                if (((EntityReference)ele["pricelevelid"]).Id == lista.Id)
                                {
                                    elementoExistente = true;
                                    break;
                                }
                            }
                            if (elementoExistente)
                                continue;

                            Entity elemento = new Entity("productpricelevel");
                            elemento.Attributes["pricelevelid"] = new EntityReference("pricelevel", lista.Id);
                            elemento.Attributes["productid"] = new EntityReference("product", idProducto);
                            elemento.Attributes["uomid"] = new EntityReference("uom", unidades.Entities[0].Id);
                            elemento["quantitysellingcode"] = new OptionSetValue(1);
                            elemento["amount"] = 0.0;

                            try
                            {
                                servicio.Create(elemento);
                            }
                            catch (Exception ex)
                            {
                                //Código para evitar los duplicados en elemento de lista de precios
                            }

                        }
                    }
                }
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("Opportunity: " + ex.Message.ToString(), ex);
            }
            catch (Exception ex)
            {
                tracingService.Trace("BBOpportunity_PostCreate plug-in: {0}", ex.ToString());
                throw;
            }
        }
    }
}
