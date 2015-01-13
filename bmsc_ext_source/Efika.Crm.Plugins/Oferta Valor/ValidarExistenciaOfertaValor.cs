using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Efika.Crm.Plugins.Oferta_Valor
{
    /// <summary>
    /// Valida la existencia de una oferta valor en base segmento
    /// </summary>
    public class ValidarExistenciaOfertaValor : IPlugin
    {

        private string FetchXmlObtenerOfertaValor
        {
            get
            {
                return
                  @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                      <entity name=""efk_oferta_valor_banco"">
                        <attribute name=""efk_name"" />      
                        <attribute name=""efk_segmento"" />
                        <attribute name=""efk_oferta_valor_bancoid"" />
                        <order attribute=""efk_name"" descending=""false"" />
                        <filter type=""and"">
                            <condition attribute=""statecode"" operator=""eq"" value=""0"" />
                            <condition attribute=""efk_segmento"" operator=""eq"" uiname=""{0}"" uitype=""efk_segmento"" value=""{1}"" />
                            <condition attribute=""efk_oferta_valor_bancoid"" operator=""ne"" uiname="""" uitype=""efk_oferta_valor_banco"" value=""{2}"" />
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

            try
            {
                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
                {
                    Entity entity = (Entity)context.InputParameters["Target"];

                    if (entity.LogicalName != "efk_oferta_valor_banco")
                        return;

                    if (context.Depth > 1)
                        return;

                    //Validamos si existe otro cliente con el mismo segmento
                    if (entity.Attributes.Contains("efk_segmento")) //efk_segmento
                     {
                        EntityReference idsegmento = (EntityReference)entity.Attributes["efk_segmento"];

                        EntityCollection ec = service.RetrieveMultiple(new FetchExpression(
                            string.Format(FetchXmlObtenerOfertaValor, "", idsegmento.Id.ToString(), context.PrimaryEntityId.ToString())));

                        if (ec.Entities.Count > 0)
                        {
                            throw new InvalidPluginExecutionException("Ya existe una oferta de valor con el segmento seleccionado.");
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
