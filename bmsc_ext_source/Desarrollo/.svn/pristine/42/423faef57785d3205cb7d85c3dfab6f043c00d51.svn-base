using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Efika.Crm.Plugins.MA.ActividadCampania
{
    public class ValidaRelacionActividad : IPlugin
    {
        #region Variables
        private static string strFetchXmlActividad;
        private static string strFetchXmlActividadCampania;
        private Entity entity = null;
        private Entity entidad = null;
        private EntityReference _regardingobjectid = null;
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

            string origen = context.GetType().GetProperty("CallerOrigin").GetValue(context, null).GetType().Name;

            try
            {
                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity && origen != "AsyncServiceOrigin")
                {
                    entity = (Entity)context.InputParameters["Target"];
                    if (entity != null)
                    {
                        strFetchXmlActividad = ObtenerActividad();
                        strFetchXmlActividadCampania = ObtenerActividadCampania();

                        if (context.MessageName == "Update")
                        {
                            EntityCollection ec = service.RetrieveMultiple(new FetchExpression(string.Format(strFetchXmlActividad, entity.LogicalName, context.PrimaryEntityId.ToString())));
                            if (ec.Entities.Count > 0 && entity.Attributes.Contains("regardingobjectid") )
                            {
                                _regardingobjectid = (EntityReference)entity.Attributes["regardingobjectid"];
                                EntityCollection ecac = service.RetrieveMultiple(new FetchExpression(string.Format(strFetchXmlActividadCampania, _regardingobjectid.Id.ToString())));
                                if (ecac.Entities.Count > 0)
                                    throw new InvalidPluginExecutionException("No puede relacionar una actividad a una Actividad de Campaña.");
                            }
                        }
                        else
                        {
                            if (context.MessageName == "Create")
                            {
                                if (entity.Attributes.Contains("regardingobjectid"))
                                {
                                    _regardingobjectid = (EntityReference)entity.Attributes["regardingobjectid"];
                                    EntityCollection ecac = service.RetrieveMultiple(new FetchExpression(string.Format(strFetchXmlActividadCampania, _regardingobjectid.Id.ToString())));
                                    if (ecac.Entities.Count > 0)
                                        throw new InvalidPluginExecutionException("No puede relacionar una actividad a una Actividad de Campaña.");
                                }
                            }
                        }
                    }
                }  
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("ValidaRelacionActividad plug-in." + ex.Message, ex);
            }
            catch (Exception ex)
            {
                tracingService.Trace("ValidaRelacionActividad plug-in: {0} " + ex.Message, ex.ToString());      
                throw;
            }
        }

        #endregion

        #region Funciones
        private string ObtenerActividad()
        {
            strFetchXmlActividad = string.Empty;

            strFetchXmlActividad = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                                                <entity name=""{0}"">
                                                    <attribute name=""activityid"" />
                                                    <attribute name=""regardingobjectid"" />                                                
                                                    <order attribute=""subject"" descending=""false"" />
                                                    <filter type=""and"">
                                                      <condition attribute=""regardingobjectid"" operator=""null"" />
                                                      <condition attribute=""activityid"" operator=""eq"" value=""{1}"" /> 
                                                    </filter>
                                                  </entity>
                                                </fetch>";

            return strFetchXmlActividad;
        }

        private string ObtenerActividadCampania()
        {
            strFetchXmlActividadCampania = string.Empty;

            strFetchXmlActividadCampania = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                                                <entity name=""campaignactivity"">
                                                    <attribute name=""activityid"" />
                                                    <attribute name=""regardingobjectid"" />                                                
                                                    <order attribute=""subject"" descending=""false"" />
                                                    <filter type=""and"">                                             
                                                      <condition attribute=""activityid"" operator=""eq"" value=""{0}"" /> 
                                                    </filter>
                                                  </entity>
                                                </fetch>";

            return strFetchXmlActividadCampania;
        }
        #endregion
    }
}
