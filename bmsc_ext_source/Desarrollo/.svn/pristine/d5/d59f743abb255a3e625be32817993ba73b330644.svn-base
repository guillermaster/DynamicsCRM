using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Efika.Crm.Plugins.CondicionesCliente
{
    public class GeneraNumeroCondicionPactada : IPlugin
    {
        private const int sequenceStart = 1;

        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            try
            {
                object callerOrigin = context.GetType().GetProperty("CallerOrigin").GetValue(context, null);

                //validar que una entidad haya generado el evento
                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
                {
                    Entity entity = (Entity)context.InputParameters["Target"];

                    if (entity.LogicalName != "efk_condicion_cliente") //valida que la entidad sea margen de credito
                        return;
                    if (context.Depth > 1)
                        return;

                    IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                    IOrganizationService service = factory.CreateOrganizationService(context.UserId);
                    EntityCollection entCollection = service.RetrieveMultiple(new FetchExpression(FetchXmlObtenerNumeroCondicionPactada));

                    int sgteNumero;
                    //validar que exista al menos una entidad y si en la primera entidad (efk_margen_credito) contenga valores en la columna efk_numero
                    if (entCollection.Entities.Count > 0 && entCollection.Entities[0].Contains("efk_numero"))
                    {
                        string strNumero = entCollection.Entities[0]["efk_numero"].ToString();//obtener el mayor numero
                        if (strNumero == null || strNumero.Length > 0)
                            sgteNumero = int.Parse(strNumero) + 1; //calcular el siguiente numero
                        else
                            sgteNumero = sequenceStart;
                    }
                    else
                    {
                        sgteNumero = sequenceStart;
                    }
                    // asignar el siguiente numero                    
                    Entity condicionPactada = service.Retrieve("efk_condicion_cliente", context.PrimaryEntityId, new ColumnSet("efk_numero"));
                    condicionPactada.Attributes["efk_numero"] = sgteNumero.ToString();
                    service.Update(condicionPactada);

                }
            }
            catch (Exception ex)
            {
                tracingService.Trace(this.GetType().ToString() + "_PreCreate {0}", ex.Message);
                throw new InvalidPluginExecutionException(ex.Message);
            }
            
        }

        private string FetchXmlObtenerNumeroCondicionPactada
        {
            get
            {
                return @"<fetch distinct=""false"" mapping=""logical"" output-format=""xml-platform"" version=""1.0"">
                            <entity name=""efk_condicion_cliente"">
                                <attribute name=""efk_condicion_clienteid""/>
                                <attribute name=""efk_numero""/>
                                <order descending=""true"" attribute=""efk_numero""/>
                            </entity>
                         </fetch>";
            }
        }
    }
}

