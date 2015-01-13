using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Efika.Crm.Plugins.Margen_Credito
{
    public class GenerarNumeroMargenCredito : IPlugin
    {
        private const string colNumero = "efk_numero";
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
                    
                    if (entity.LogicalName != "efk_margen_credito") //valida que la entidad sea margen de credito
                        return;
                    if (context.Depth > 1)
                        return;
                                 
                    IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                    IOrganizationService service = factory.CreateOrganizationService(context.UserId);
                    EntityCollection entCollection = service.RetrieveMultiple(new FetchExpression(FetchXmlObtenerNumeroMargenCredito));
                    
                    int sgteNumero;
                    //validar que exista al menos una entidad y si en la primera entidad (efk_margen_credito) contenga valores en la columna efk_numero
                    if (entCollection.Entities.Count > 0 && entCollection.Entities[0].Contains(colNumero))
                    {
                        string strNumero = entCollection.Entities[0][colNumero].ToString();//obtener el mayor numero
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
                    Entity margenCredito = service.Retrieve("efk_margen_credito", context.PrimaryEntityId, new ColumnSet(colNumero));
                    margenCredito.Attributes[colNumero] = sgteNumero.ToString();
                    service.Update(margenCredito);
                    
                }
            }
            catch (Exception ex)
            {
                tracingService.Trace(this.GetType().ToString() + "_PreCreate {0}", ex.Message);
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }

        // query para obtener los valores en la columna efk_numero ordenados descendentemente, el primer resultado será el del mayor número
        private string FetchXmlObtenerNumeroMargenCredito
        {
            get
            {
                return @"<fetch distinct=""false"" mapping=""logical"" output-format=""xml-platform"" version=""1.0"">
                            <entity name=""efk_margen_credito"">
                                <attribute name=""efk_margen_creditoid""/>
                                <attribute name=""efk_numero""/>
                                <order descending=""true"" attribute=""efk_numero""/>
                            </entity>
                         </fetch>";
            }
        }
    }
}
