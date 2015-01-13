using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Efika.Crm.Plugins.Portafolio
{
    /// <summary>
    /// Valida la existencia de un portafolio, por oferta, tipo, y prioridad
    /// </summary>
    public class ValidarExistenciaPortafolio : IPlugin
    {
        private string FetchXmlObtenerPortafolio
        {
            get
            {
                return
                  @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                      <entity name=""efk_portafolio_segmento"">
                        <attribute name=""efk_name"" />      
                        <attribute name=""efk_portafolioid"" />
                        <attribute name=""efk_prioridad"" />
                        <attribute name=""efk_tipo_portafolio"" />
                        <order attribute=""efk_name"" descending=""false"" />
                        <filter type=""and"">
                            <condition attribute=""statecode"" operator=""eq"" value=""0"" />
                            <condition attribute=""efk_portafolioid"" operator=""eq"" uiname=""{0}"" uitype=""efk_oferta_valor_banco"" value=""{1}"" />
                            <condition attribute=""efk_prioridad"" operator=""eq"" value=""{2}"" />
                            <condition attribute=""efk_tipo_portafolio"" operator=""eq"" value=""{3}"" />
                        </filter>
                    </entity>
                  </fetch>";
            }
        }

        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(null);

            IOrganizationService iServices = ((IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory))).CreateOrganizationService(new Guid?(context.UserId));

            // Get a reference to the tracing service.
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            string _idofertavalor = "";
            string prioridad = "";
            string tipo_portafolio = "";

            try
            {
                Entity entidad = null;
               if(context.MessageName == "Update") 
                    entidad = service.Retrieve("efk_portafolio_segmento",context.PrimaryEntityId, new ColumnSet(new string[]{"efk_portafolioid","efk_prioridad","efk_tipo_portafolio"}));

                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
                {
                    Entity entity = (Entity)context.InputParameters["Target"];

                    if (entity.LogicalName != "efk_portafolio_segmento")
                        return;

                    if (context.Depth > 1)
                        return;

                    //Validamos si algún campo ha sido modificado
                    if (entity.Attributes.Contains("efk_portafolioid") || entity.Attributes.Contains("efk_prioridad") || entity.Attributes.Contains("efk_tipo_portafolio")) //efk_segmento
                    {
                        EntityReference idofertavalor = null;
                        if (entity.Attributes.Contains("efk_portafolioid"))
                        {
                            idofertavalor = (EntityReference)entity.Attributes["efk_portafolioid"];
                            _idofertavalor = idofertavalor.Id.ToString();
                        }
                        else
                        {
                            if (entidad != null)
                            {
                                idofertavalor = (EntityReference)entidad.Attributes["efk_portafolioid"];
                                _idofertavalor = idofertavalor.Id.ToString();
                            }
                        }

                        if (entity.Attributes.Contains("efk_prioridad"))
                            prioridad = entity.Attributes["efk_prioridad"].ToString();
                        else
                        {
                            if (entidad != null) prioridad = entidad.Attributes["efk_prioridad"].ToString(); ;
                        }

                        if (entity.Attributes.Contains("efk_tipo_portafolio"))
                            tipo_portafolio = ((Microsoft.Xrm.Sdk.OptionSetValue)(entity.Attributes["efk_tipo_portafolio"])).Value.ToString();
                        else
                        {
                            if (entidad != null) tipo_portafolio = ((Microsoft.Xrm.Sdk.OptionSetValue)(entidad.Attributes["efk_tipo_portafolio"])).Value.ToString();
                        }

                        EntityCollection ec = service.RetrieveMultiple(new FetchExpression(string.Format(FetchXmlObtenerPortafolio, "", _idofertavalor, prioridad, tipo_portafolio)));

                        if (ec.Entities.Count > 0)
                        {
                            throw new InvalidPluginExecutionException("Ya existe un portafolio con la oferta, tipo de portafolio y prioridad indicados.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                tracingService.Trace(this.GetType().ToString() + "_PreCreate: {0} ", ex.ToString());
                throw;
            }
        }
    }
}
