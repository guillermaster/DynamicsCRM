using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk.Metadata;
using System.Data;
using System.Web;

namespace Efika.Crm.Plugins.SFA.Opportunity
{
    public class CargaProductosOportunidad : IPlugin
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

                Guid oportunidadid = ((Guid)context.OutputParameters["id"]);
                Guid Idcrm_unidad = Guid.Empty;
                Guid Idcrm_pricelevel = Guid.Empty;
                Guid IdSegmento = Guid.Empty;

                if (clienteid != null)
                {
                    try
                    {
                        #region Obtengo Unidad Medida
                        //Necesito Guid de la Unidad de Medida ya que es un valor requerido por CRM

                        //****armo objeto queryattribute para hacer la consulta
                        QueryExpression qbauom = new QueryExpression();
                        qbauom.EntityName = "uom";
                        qbauom.ColumnSet = new ColumnSet();
                        string[] columnasuom = new string[] { "name", "uomid" };
                        qbauom.ColumnSet.AddColumns(columnasuom);

                        qbauom.Criteria.AddCondition(new ConditionExpression("name", ConditionOperator.Equal, "Unidad principal"));
                        EntityCollection queryCondiciones1 = servicio.RetrieveMultiple(qbauom);

                        //****Ejecuto el query y si encuentro registro instancio entidad y lo retorno para análisis
                        if (queryCondiciones1.Entities.Count > 0)
                        {
                            Entity crm_unit = (Entity)queryCondiciones1.Entities[0];
                            Idcrm_unidad = crm_unit.Id;
                        }
                        #endregion

                        #region Obtengo ListaPrecios
                        //Necesito Guid de la Unidad de Medida ya que es un valor requerido por CRM

                        //****armo objeto queryattribute para hacer la consulta
                        QueryExpression qbapriceleve = new QueryExpression();
                        qbapriceleve.EntityName = "pricelevel";
                        qbapriceleve.ColumnSet = new ColumnSet();
                        string[] columnasprice = new string[] { "name", "pricelevelid" };
                        qbapriceleve.ColumnSet.AddColumns(columnasprice);

                        qbapriceleve.Criteria.AddCondition(new ConditionExpression("name", ConditionOperator.Equal, "lista de precios prdeterminada"));
                        EntityCollection conditionprice = servicio.RetrieveMultiple(qbapriceleve);

                        //****Ejecuto el query y si encuentro registro instancio entidad y lo retorno para análisis
                        if (conditionprice.Entities.Count > 0)
                        {
                            Entity crm_price = (Entity)conditionprice.Entities[0];
                            Idcrm_pricelevel = crm_price.Id;
                        }
                        #endregion

                        #region Obtengo Segmento Base Cliente

                        //****armo objeto queryattribute para hacer la consulta
                        QueryExpression qbaSegmento = new QueryExpression();
                        qbaSegmento.EntityName = "account";
                        qbaSegmento.ColumnSet = new ColumnSet();
                        string[] columnasSegmento = new string[] { "accountid", "efk_segmento_ovid" };
                        qbaSegmento.ColumnSet.AddColumns(columnasSegmento);

                        qbaSegmento.Criteria.AddCondition(new ConditionExpression("accountid", ConditionOperator.Equal, clienteid.ToString()));
                        EntityCollection retrievedSegmento = servicio.RetrieveMultiple(qbaSegmento);
                        
                        if (conditionprice.Entities.Count > 0)
                        {
                            Entity crm_segmento = (Entity)retrievedSegmento.Entities[0];
                            IdSegmento = crm_segmento.Id;                           
                            string[] strDatos = new string[] { IdSegmento.ToString() };
                        }

                        #endregion


                        #region ProductosSegmento
                        QueryExpression qbaOfertaValor = new QueryExpression();
                        qbaOfertaValor.EntityName = "efk_oferta_valor";
                        qbaOfertaValor.ColumnSet = new ColumnSet();
                        string[] columnasOfertaValor = new string[] { "efk_cliente_juridico_id", "efk_tipo_productos_id","efk_product_id","efk_prioridad" 
                                                                     ,"efk_familia_productos_id","efk_cliente_natural_id","efk_oferta_valorid"
                                                                     ,"efk_portafolio" };
                        qbaOfertaValor.ColumnSet.AddColumns(columnasOfertaValor);

                        qbaOfertaValor.Criteria.AddCondition(new ConditionExpression("efk_cliente_juridico_id", ConditionOperator.Equal, clienteid.ToString()));
                        qbaOfertaValor.Criteria.AddCondition(new ConditionExpression("efk_portafolio", ConditionOperator.In, new string[] { "INICIAL", "BASICO" }));
                        EntityCollection retrievedOfertaValor = servicio.RetrieveMultiple(qbaOfertaValor);

                        //****si tiene productos de segmento
                        if (retrievedOfertaValor.Entities.Count > 0)
                        {   
                            int anidoproducto = 0, noanido = 0;
                            string message_final = string.Empty;
                            string message = string.Empty;
                            string message1 = string.Empty;
                            string saltoLinea = "\n";
                            for (int i = 0; i < retrievedOfertaValor.Entities.Count; i++)
                            {
                                try
                                {
                                    //Creo el producto oportunidad
                                    Guid productid = ((EntityReference)retrievedOfertaValor.Entities[i]["efk_product_id"]).Id;
                                    bool bandera = verifica_producto_oportunidad(clienteid.ToString(), productid.ToString(), servicio);
                                    if (bandera)
                                    {
                                        noanido = noanido + 1;                                        
                                    }
                                    else
                                    {
                                        Entity miproducto = new Entity("opportunityproduct");
                                        EntityReference opportunityid = new EntityReference("opportunity",oportunidadid);
                                        miproducto["opportunityid"] =opportunityid;
                                        EntityReference productoid = new EntityReference("product",productid);  
                                        miproducto["productid"] =productoid;
                                        EntityReference unidadmedida = new EntityReference("uom",Idcrm_unidad);  
                                        miproducto["uomid"] =unidadmedida;
                                        anidoproducto = anidoproducto + 1;
                                        service.Create(miproducto);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }

                            if (anidoproducto > 0)
                            {
                                message = "Se creó la Oportunidad con Productos (" + anidoproducto.ToString() + ") de Oferta de Valor agregados automáticamente a la Oportunidad.";
                            }

                            if (noanido > 0)
                            {
                                message1 = "Algunos Productos (" + noanido.ToString() + 
                                    ") de la Oferta de Valor se encuentran en otras Oportunidades abiertas. La oportunidad se ha creado sin esos productos.";
                            }

                            message_final = message + saltoLinea + message1 + saltoLinea;
                            if (message_final != string.Empty) Actualizarnotasprocesossistem(oportunidadid, servicio, message_final);
                        }
                        #endregion
                    }
                    catch (System.Exception ex)
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
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

        private static void Actualizarnotasprocesossistem(Guid idoportunidad, IOrganizationService servicio, string mensaje)
        {
            ColumnSet columnas = new ColumnSet("opportunityid", "efk_detalle_productos_agregados");
            Entity oportunidad = servicio.Retrieve("opportunity", idoportunidad, columnas);

            if (oportunidad.Attributes.Contains("efk_detalle_productos_agregados"))
            {
                oportunidad.Attributes["efk_detalle_productos_agregados"] = mensaje;
            }
            else
            {
                oportunidad.Attributes.Add("efk_detalle_productos_agregados", mensaje);
            }
            oportunidad.Attributes.Add("efk_mensaje_productos_agregados_mostrado", false);
            oportunidad.Attributes.Add("efk_productos_agregados_oferta_valor",true);

            servicio.Update(oportunidad);
        }

    }
}

