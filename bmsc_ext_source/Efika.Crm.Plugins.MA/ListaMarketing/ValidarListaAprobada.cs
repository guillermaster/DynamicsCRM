using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Efika.Crm.Plugins.MA.ListaMarketing
{
    class ValidarListaAprobada
    {
        #region Variables
        private static string strFetchXmlListaAprobada;
        private static int intAprobada_No = 100000001;
        #endregion

        #region Main
        public void Execute(IServiceProvider serviceProvider)
        {

            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);

            IOrganizationService iServices = ((IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory))).CreateOrganizationService(new Guid?(context.UserId));

            // Get a reference to the tracing service.
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            try
            {
                string strListaId = string.Empty;
                string strListaEntity = string.Empty;
                string strCampaniaId = string.Empty;

                if (context.InputParameters.Contains("EntityName") && context.InputParameters.Contains("EntityId") && context.InputParameters.Contains("CampaignId"))
                {
                    foreach (KeyValuePair<string, object> parameter in context.InputParameters)
                    {

                        if (parameter.Value != null)
                        {
                            string strKey = parameter.Key;
                            string strValue = parameter.Value.ToString();

                            if (strKey == "EntityName" && strValue == "list")
                            {
                                strListaEntity = parameter.Value.ToString();
                            }
                            else
                            {
                                if (parameter.Key == "CampaignId")
                                {
                                    strCampaniaId = parameter.Value.ToString();
                                }
                                else
                                {
                                    if (parameter.Key == "EntityId")
                                    {
                                        strListaId = parameter.Value.ToString();
                                    }
                                }
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                    if (strListaId != string.Empty && strListaEntity != string.Empty && strCampaniaId != string.Empty)
                    {
                        strFetchXmlListaAprobada = ObtenerListaAprobada();
                        if (strListaEntity == "list")
                        {
                            EntityCollection ecLista = null;
                            strFetchXmlListaAprobada = string.Format(strFetchXmlListaAprobada, strListaId);

                            ecLista = service.RetrieveMultiple(new FetchExpression(strFetchXmlListaAprobada));

                            if (ecLista.Entities.Count > 0)
                            {
                                foreach (var reg in ecLista.Entities)
                                {
                                    if (reg.Contains("efk_aprobada"))
                                    {
                                        int intAprobado = reg.GetAttributeValue<OptionSetValue>("efk_aprobada").Value;
                                        if (intAprobado == intAprobada_No)
                                        {
                                            throw new Exception("No puede agregar una la lista de marketing que no está aprobada.");
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("No puede agregar una la lista de marketing que no está aprobada.");
                                    }
                                }
                            }
                        }
                    }

                }
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("ValidarListaAprobada_PreAddItem plug-in.", ex);
            }
            catch (Exception ex)
            {
                tracingService.Trace("BBCampania_PreAddItem plug-in: {0}", ex.ToString());
                throw;
            }
        }
        #endregion

        #region Funciones
        private string ObtenerListaAprobada()
        {
            strFetchXmlListaAprobada = string.Empty;

            strFetchXmlListaAprobada = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                                            <entity name=""list"">
                                                <attribute name=""listname"" />
                                                <attribute name=""efk_aprobada"" />
                                                <order attribute=""listname"" descending=""true"" />
                                                <filter type=""and"">
                                                    <condition attribute=""listid"" operator=""eq"" value=""{0}"" />
                                                </filter>
                                            </entity>
                                        </fetch>";

            return strFetchXmlListaAprobada;
        }
        #endregion
    }
}
