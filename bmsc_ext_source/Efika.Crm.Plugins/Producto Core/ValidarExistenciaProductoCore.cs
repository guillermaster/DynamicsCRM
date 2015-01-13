using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Efika.Crm.Plugins.Producto_Core
{
    public class ValidarExistenciaProductoCore
    {
        
        private string FetchXmlObtenerProductoBanco
        {
            get
            {
                return
                    @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                      <entity name=""efk_producto_core"">
                        <attribute name=""efk_nombre"" />
                        <attribute name=""efk_transactioncurrencyid"" />
                        <attribute name=""efk_codigo_producto_core"" />
                        <attribute name=""efk_producto_coreid"" />
                        <order attribute=""efk_nombre"" descending=""false"" />
                        <filter type=""and"">
                          <condition attribute=""statecode"" operator=""eq"" value=""0"" />
                          <condition attribute=""efk_codigo_producto_core"" operator=""eq"" value=""{0}"" />
                          <condition attribute=""efk_producto_coreid"" operator=""ne"" uiname=""181-186 DIAS"" uitype=""efk_producto_core"" value=""{1}"" />
                        </filter>
                      </entity>
                    </fetch>";
            }
        }

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

                    if (entity.LogicalName != "efk_producto_core")
                        return;

                    if (context.Depth > 1)
                        return;

                    //Validamos si existe otro cliente con el mismo número de identificación
                    if (entity.Attributes.Contains("efk_codigo_producto_core"))
                    {
                        string codigo = entity.Attributes["efk_codigo_producto_core"].ToString();

                        EntityCollection ec = service.RetrieveMultiple(new FetchExpression(string.Format(FetchXmlObtenerProductoBanco, codigo, context.PrimaryEntityId)));

                        if (ec.Entities.Count > 0)
                        {
                            throw new InvalidPluginExecutionException("Ya existe un producto banco con el mismo código de producto.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                tracingService.Trace(this.GetType().ToString() + "_PreCreate, _PreUpdate: {0} ", ex.ToString());
                throw;
            }
        }
    }
}
