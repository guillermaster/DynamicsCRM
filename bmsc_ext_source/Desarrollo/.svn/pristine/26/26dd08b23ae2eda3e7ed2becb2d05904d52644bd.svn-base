using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk.Metadata;

namespace Efika.Crm.Plugins.SFA.CondicionesGarantias
{
    public class ReglasCondicionesGarantias : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {                   
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);
            IOrganizationService iServices = ((IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory))).CreateOrganizationService(new Guid?(context.UserId));
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            try
            {
                object callerorigin =
                     context.GetType().GetProperty("CallerOrigin").GetValue(context, null);

                if ((callerorigin.GetType().Name == "ApplicationOrigin") && context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity && context.Depth == 1)
                {
                    Entity entity = (Entity)context.InputParameters["Target"];

                    if (entity.LogicalName != "efk_garantia_credito")
                    {
                        throw new InvalidPluginExecutionException("No es de este tipo efk_garantia_credito");
                    }
                   
                    // Si creamos una nueva garantia

                    if (context.MessageName == "Create")
                    {
                        if (context.PostEntityImages.Contains("postImage_efk_garantia_credito") && context.PostEntityImages["postImage_efk_garantia_credito"] is Entity)
                        {
                            Entity postImageGarantiaCredito = (Entity)context.PostEntityImages["postImage_efk_garantia_credito"];

                            if (postImageGarantiaCredito.Attributes.Contains("efk_garantia_oportunidadid") && postImageGarantiaCredito.Attributes["efk_garantia_oportunidadid"] != null)
                            {
                                EntityReference oportunidad = (EntityReference)postImageGarantiaCredito.Attributes["efk_garantia_oportunidadid"];
                             
                                String query = string.Format(getQueryForCreditoGarantias(), oportunidad.Id.ToString());

                                EntityCollection collection = iServices.RetrieveMultiple(new FetchExpression(query));

                                // validamos si existen mas garantias principales

                                int counter = 0;

                                foreach (Entity entidad in collection.Entities)
                                {
                                    if (entidad.Attributes.Contains("efk_principal"))
                                    {
                                        if (entidad.Attributes["efk_principal"].ToString().ToLower() == "true" && entity.Attributes["efk_principal"].ToString().ToLower() == "true")
                                        {
                                            //throw new InvalidPluginExecutionException("No puede haber dos garantias marcadas como principales. \n Cambie otra si desea establecer uan nueva como principal.");
                                            counter++;
                                        }
                                        if ( counter > 1 )
                                            throw new InvalidPluginExecutionException("No puede haber dos garantias marcadas como principales. \n Cambie otra si desea establecer uan nueva como principal.");
                                    }
                                }

                                // validamos si la oportunidad ya esta evaluada

                                Entity oportunidadCompleta = iServices.Retrieve("opportunity", oportunidad.Id, new ColumnSet("opportunityid", "efk_solicitud_enviada_fabrica"));
                                
                                if (oportunidadCompleta.Attributes.Contains("efk_solicitud_enviada_fabrica") && oportunidadCompleta.Attributes["efk_solicitud_enviada_fabrica"] != null)
                                {
                                    if (oportunidadCompleta.Attributes["efk_solicitud_enviada_fabrica"].ToString().ToLower() == "true")
                                    {
                                        throw new Exception("No puede crear nuevas garantias cuando la oportunidad ya fue evaluada.");
                                    }                                                                      
                                }
                            }
                        }
                    }

                    // Si actualizamos una garantia

                    if (context.MessageName == "Update")
                    {
                        Entity garantia = iServices.Retrieve("efk_garantia_credito", entity.Id, new ColumnSet("efk_garantia_oportunidadid", "efk_principal"));
                        
                        if (garantia.Attributes.Contains("efk_garantia_oportunidadid") && garantia.Attributes["efk_garantia_oportunidadid"] != null)
                        {                            
                            EntityReference referenceOportunidad = garantia.Attributes["efk_garantia_oportunidadid"] as EntityReference;

                            String query = string.Format(getQueryForCreditoGarantias(), referenceOportunidad.Id.ToString());

                            EntityCollection collection = iServices.RetrieveMultiple(new FetchExpression(query));

                            // validamos las garantias principales

                            foreach (Entity entidad in collection.Entities)
                            {
                                if (entidad.Attributes.Contains("efk_principal") && entidad.Attributes["efk_principal"] != null)
                                {
                                    if (entidad.Attributes["efk_principal"].ToString().ToLower() == "true")
                                    {
                                        if (entity.Attributes.Contains("efk_principal") && entity.Attributes["efk_principal"] != null && entity.Attributes["efk_principal"].ToString().ToLower() == "true")
                                        {
                                            throw new InvalidPluginExecutionException("No puede haber dos garantias marcadas como principales. \n Cambie otra si desea establecer uan nueva como principal.");
                                        }                                        
                                    }
                                }
                            }

                            // validamos si la oportunidad ya esta evaluada

                            Entity oportunidadCompleta = iServices.Retrieve("opportunity", referenceOportunidad.Id, new ColumnSet("opportunityid", "efk_solicitud_enviada_fabrica"));

                            if (oportunidadCompleta.Attributes.Contains("efk_solicitud_enviada_fabrica") && oportunidadCompleta.Attributes["efk_solicitud_enviada_fabrica"] != null)
                            {
                                if (oportunidadCompleta.Attributes["efk_solicitud_enviada_fabrica"].ToString().ToLower() == "true")
                                {
                                    throw new Exception("No puede actualizar garantias cuando la oportuniad ya fue evaluada.");
                                }
                            }
                        }
                    }                    
                }

                // cuando vamos a borrar una entidad de garantia

                else if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is EntityReference && context.Depth == 1)
                {
                    EntityReference entityReference = (EntityReference)context.InputParameters["Target"];

                    if (context.MessageName == "Delete")
                    {
                        // validamos si la oportunidad ya esta evaluada

                        Entity garantia = iServices.Retrieve("efk_garantia_credito", entityReference.Id, new ColumnSet("efk_garantia_oportunidadid"));

                        if (garantia.Attributes.Contains("efk_garantia_oportunidadid") && garantia.Attributes["efk_garantia_oportunidadid"] != null)
                        {
                            EntityReference referenceOportunidad = garantia.Attributes["efk_garantia_oportunidadid"] as EntityReference;

                            Entity oportunidadCompleta = iServices.Retrieve("opportunity", referenceOportunidad.Id, new ColumnSet("opportunityid", "efk_solicitud_enviada_fabrica"));
                            
                            if (oportunidadCompleta.Attributes.Contains("efk_solicitud_enviada_fabrica") && oportunidadCompleta.Attributes["efk_solicitud_enviada_fabrica"] != null)
                            {
                                if (oportunidadCompleta.Attributes["efk_solicitud_enviada_fabrica"].ToString().ToLower() == "true")
                                {
                                    throw new Exception("No puede eliminar garantias cuando la oportuniad ya fue evaluada.");
                                }
                            }
                        }
                    }
                }
            }
            catch (System.ServiceModel.FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("CondicionesGarantias_PreValidation plug-in: " + ex.Message.ToString(), ex);
            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException("CondicionesGarantias_PreValidation plug-in: " + e.Message.ToString(), e);
            }
        }


        /*
         * 
         * 
         */

        public string getQueryForCreditoGarantias()
        {
            string query = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                                    <entity name=""efk_garantia_credito"">
                                        <attribute name=""efk_garantia_creditoid"" />
                                        <attribute name=""efk_garantia_oportunidadid"" />                                                                         
                                        <attribute name=""efk_principal"" />
                                        <filter type=""and"">
                                            <condition attribute=""efk_garantia_oportunidadid"" operator=""eq"" value=""{0}"" />
                                        </filter>
                                    </entity>
                                    </fetch>";
            return query;
        }


        /*
         * 
         * 
         */    
         
    }
}
