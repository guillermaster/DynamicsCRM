using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Efika.Crm.Plugins.MA.ListaMarketing
{
    public class ValidarListaMarketing_PreUpdate : IPlugin
    {
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

                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
                {
                    Entity entity = (Entity)context.InputParameters["Target"];

                    if (entity.LogicalName != "list")
                        return;

                    if (!entity.Attributes.Contains("efk_aprobada"))
                    {
                        return;
                    }
                    OptionSetValue aprobada=(OptionSetValue)entity.Attributes["efk_aprobada"];

                    if (aprobada!=null && aprobada.Value == 100000001)
                    {

                        string fecth = ObtenerCampanasDeLista();
                        string strFetchXmlCampaniaLista = string.Format(fecth, entity.Id.ToString());
                        EntityCollection ec = service.RetrieveMultiple(new FetchExpression(strFetchXmlCampaniaLista));

                        if (ec.Entities.Count != 0)
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine("No puede quitar la aprobación de la lista.");
                            sb.AppendLine(" ");
                            sb.AppendLine("La lista está siendo utilizada en al menos una campaña.");
                            throw new Exception(sb.ToString());
                        }
                    }
                }
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("ValidarListaMarketing plug-in: " + ex.Message.ToString(), ex);
            }
            catch (Exception ex)
            {
                tracingService.Trace("ValidarListaMarketing plug-in: {0}", ex.ToString());
                throw;
            }
        }

        private string ObtenerCampanasDeLista()
        {
            string strFetchXmlCampaniaLista = string.Empty;

            strFetchXmlCampaniaLista = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""true"">
                                                <entity name=""campaign"">
                                                <attribute name=""name"" />
                                                <attribute name=""statuscode"" />
                                                <attribute name=""createdon"" />
                                                <attribute name=""ownerid"" />
                                                <attribute name=""proposedstart"" />
                                                <attribute name=""proposedend"" />
                                                <attribute name=""campaignid"" />
                                                <order attribute=""name"" descending=""true"" />
                                                <link-entity name=""campaignitem"" from=""campaignid"" to=""campaignid"" visible=""false"" intersect=""true"">
                                                    <link-entity name=""list"" from=""listid"" to=""entityid"" alias=""aa"">
                                                    <filter type=""and"">
                                                        <condition attribute=""listid"" operator=""eq"" uitype=""list"" value=""{0}"" />
                                                    </filter>
                                                    </link-entity>
                                                </link-entity>
                                                </entity>
                                            </fetch>";

            return strFetchXmlCampaniaLista;
        }
    }

    public class ValidarListaMarketing_PostUpdate : IPlugin
    {
        #region Variables
        private static int intAprobada_Si = 100000000;
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
            bool blnEnviadoAprobacion = false;
            bool blnBloqueado = false;
            bool flagValidarAprobada = false;
            bool flagValidarEnviada = false;
            bool flagEnviarAprobacion = false;

            try
            {

                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
                {
                    Entity entity = (Entity)context.InputParameters["Target"];

                    if (entity.LogicalName != "list")
                        return;

                    Entity target = (Entity)context.PreEntityImages["list_EntityImage"];

                    if (entity.Attributes.Contains("efk_aprobada"))
                    {
                        if (entity.Attributes["efk_aprobada"] != null)
                        {
                            int intAprobado = entity.GetAttributeValue<OptionSetValue>("efk_aprobada").Value;
                            if (intAprobado == intAprobada_Si)
                            {
                                blnBloqueado = true;
                                blnEnviadoAprobacion = bool.Parse(target.Attributes["efk_enviada_aprobacion"].ToString());
                            }
                            else
                            {
                                if (intAprobado == intAprobada_No)
                                {
                                    blnBloqueado = false;
                                    blnEnviadoAprobacion = false;
                                    flagEnviarAprobacion = true;
                                }
                            }
                            flagValidarAprobada = true;
                        }
                    }

                    if (entity.Attributes.Contains("efk_enviada_aprobacion"))
                    {
                        if (flagEnviarAprobacion == false)
                        {
                            blnEnviadoAprobacion = bool.Parse(entity.Attributes["efk_enviada_aprobacion"].ToString());
                        }
                        flagValidarEnviada = true;
                    }
                    else
                    {
                        blnEnviadoAprobacion = bool.Parse(target.Attributes["efk_enviada_aprobacion"].ToString());
                    }

                    if (flagValidarAprobada == true || flagValidarEnviada == true)
                    {
                        Entity list = new Entity("list");
                        list.Id = entity.Id;
                        if (flagValidarAprobada == true)
                        {
                            list.Attributes["lockstatus"] = blnBloqueado;
                            list.Attributes["efk_enviada_aprobacion"] = blnEnviadoAprobacion;
                            service.Update(list);
                        }

                        list = null;
                    }


                }
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("BBList_PostUpdate plug-in.", ex);
            }
            catch (Exception ex)
            {
                tracingService.Trace("BBList_PostUpdate plug-in: {0}", ex.ToString());
                throw;
            }
        }
        #endregion

    }
}
