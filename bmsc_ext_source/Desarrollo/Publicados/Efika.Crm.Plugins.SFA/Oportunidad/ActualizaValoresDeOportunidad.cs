using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Efika.Crm.Plugins.SFA.Oportunidad
{

    public class ActualizaValoresDeOportunidad : IPlugin
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
                if (context.Depth == 1)
                {
                    if ((context.GetType().GetProperty("CallerOrigin").GetValue(context, null).GetType().Name != "ApplicationOrigin"))
                        return;

                    if (context.ParentContext != null)
                    {
                        if ((context.ParentContext.MessageName == "SetStateDynamicEntity") || context.ParentContext.MessageName.Contains("Assign"))
                            return;
                    }

                    Entity entity = (Entity)context.InputParameters["Target"];

                    if (entity.LogicalName != "opportunity") return;

                    String query = string.Format(getQueryOpportunityProduct(), context.PrimaryEntityId.ToString());

                    EntityCollection opportunityproducts = iServices.RetrieveMultiple(new FetchExpression(query));

                    if (opportunityproducts == null && opportunityproducts.Entities.Count == 0)
                        throw new InvalidPluginExecutionException("No hay productos relacionados con la Oportunidad");

                    // Para validar el monto solicitado
                    if (context.PostEntityImages.Contains("opportunityAfter") && context.PostEntityImages["opportunityAfter"] is Entity)
                    {
                        var treActual = new Decimal();
                        var spreadActual = new Decimal();
                        var nuevaTasaVariable = new Decimal();

                        Entity postImageOpportunity = (Entity)context.PostEntityImages["opportunityAfter"];

                        if (postImageOpportunity.Attributes.Contains("efk_spread_fijo") && postImageOpportunity.Attributes["efk_spread_fijo"] != null)
                            spreadActual = Convert.ToDecimal(postImageOpportunity.Attributes["efk_spread_fijo"]);
                        else
                            spreadActual = 0;

                        if (postImageOpportunity.Attributes.Contains("efk_tre_semana") && postImageOpportunity.Attributes["efk_tre_semana"] != null)
                            treActual = Convert.ToDecimal(postImageOpportunity.Attributes["efk_tre_semana"]);
                        else
                            treActual = 0;

                        nuevaTasaVariable = spreadActual + treActual;

                        foreach (Entity producto in opportunityproducts.Entities)
                        {
                            producto.Attributes["efk_tasa_variable"] = nuevaTasaVariable;

                            if (postImageOpportunity.Attributes.Contains("efk_monto_solicitado"))
                            {
                                if (postImageOpportunity.Attributes["efk_monto_solicitado"] != null)
                                {
                                    var montoSolicitado = new Money();
                                    montoSolicitado = postImageOpportunity.Attributes["efk_monto_solicitado"] as Money;
                                    producto.Attributes["priceperunit"] = montoSolicitado;
                                    producto.Attributes["ispriceoverridden"] = true;
                                    producto.Attributes["quantity"] = 1;
                                }
                            }

                            if (postImageOpportunity.Attributes.Contains("efk_tasa_fija"))
                            {
                                if (postImageOpportunity.Attributes["efk_tasa_fija"] != null)
                                {
                                    var tasaFija = new Decimal();
                                    tasaFija = Convert.ToDecimal(postImageOpportunity.Attributes["efk_tasa_fija"]);
                                    producto.Attributes["efk_tasa_fija"] = tasaFija;
                                }
                            }

                            if (postImageOpportunity.Attributes.Contains("efk_numero_cuotas"))
                            {
                                if (postImageOpportunity.Attributes["efk_numero_cuotas"] != null)
                                {
                                    var numeroDeCuotas = 0;
                                    numeroDeCuotas = Convert.ToInt32(postImageOpportunity.Attributes["efk_numero_cuotas"]);
                                    producto.Attributes["efk_plazo"] = (numeroDeCuotas);
                                }
                            }

                            service.Update(producto);
                       }
                        String queryopportunity = string.Format(getQueryOpportunity(), context.PrimaryEntityId.ToString());
                        EntityCollection opportunity = iServices.RetrieveMultiple(new FetchExpression(queryopportunity));
                        foreach (Entity objopportunity in opportunity.Entities)
                        {
                           if (postImageOpportunity.Attributes.Contains("efk_monto_solicitado"))
                            {
                                if (postImageOpportunity.Attributes["efk_monto_solicitado"] != null)
                                {
                                    var montoSolicitado = new Money();
                                    montoSolicitado = postImageOpportunity.Attributes["efk_monto_solicitado"] as Money;
                                    objopportunity.Attributes["estimatedvalue"] = montoSolicitado; //Monto de la oportunidad
                                }
                            }
                            service.Update(objopportunity);
                        }
                    }
                }
            }
            catch (System.ServiceModel.FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("Oportunidad_PostUpdate plug-in: " + ex.Message.ToString(), ex);
            }
            catch (Exception ex)
            {
                tracingService.Trace("Oportunidad_PostUpdate plug-in: {0}", ex.ToString());
                throw new Exception("Error --> " + ex.ToString());
            }
        }




        public static string getQueryOpportunityProduct()
        {
            string query = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                                    <entity name=""opportunityproduct"">
                                        <attribute name=""priceperunit"" />
                                        <attribute name=""ispriceoverridden"" />
                                        <attribute name=""efk_tasa_fija"" />
                                        <attribute name=""efk_tasa_variable"" />
                                        <attribute name=""efk_plazo"" />
                                        <attribute name=""opportunityproductid"" />
                                        <order attribute=""opportunityproductid"" descending=""false"" />
                                        <filter type=""and"">
                                            <condition attribute=""opportunityid"" operator=""eq"" value=""{0}"" />
                                        </filter>
                                    </entity>
                                    </fetch>";

            return query;
        }


        public static string getQueryOpportunity()
        {
            string query = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                             <entity name=""opportunity"">
                                <attribute name=""name"" />
                                <attribute name=""opportunityid"" />
                                <attribute name=""efk_monto_solicitado"" />
                                <attribute name=""estimatedvalue"" />
                                <order attribute=""name"" descending=""false"" />
                                <filter type=""and"">
                                  <condition attribute=""opportunityid"" operator=""eq""  value=""{0}"" />
                                </filter>
                              </entity>
                            </fetch>";

            return query;
        }
    }
}





