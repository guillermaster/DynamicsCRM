using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Crm.Sdk.Messages;


namespace Efika.Crm.Plugins.MA.ActividadCampania
{
    public class CerrarRespuestaCampania : IPlugin
    {
        private string fieldCerrarResp = "efk_cerrarrespuesta";
        private string fieldStatecode = "statecode";
        private string fieldStatuscode = "statuscode";
        private string fieldId = "activityid";


        public void Execute(IServiceProvider serviceProvider)
        {

            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);
            
            try
            {
                if (context.InputParameters.Contains("Target") &&
                    context.InputParameters["Target"] is Entity && context.Depth == 1 &&
                    ((Entity)context.InputParameters["Target"]).LogicalName == "campaignresponse")
                {
                    //obtener respuesta de campaña
                    Entity respCamp = (Entity)context.InputParameters["Target"];
                    if (context.MessageName == "Create")
                    {
                        //validar si el campo cerrar respuesta es verdadero
                        if (QuiereCerrarRespuesta(respCamp) )//&& !esRespuestaCerrada(service,respCamp))
                        {
                            CerrarRespuesta(context, respCamp);
                        } 
                    }
                    else if (context.MessageName == "Update")
                    {                        
                        Entity entidad = service.Retrieve(respCamp.LogicalName, respCamp.Id, new ColumnSet(fieldCerrarResp));

                        if (QuiereCerrarRespuesta(entidad) )//&& !esRespuestaCerrada(service, respCamp))
                        {
                            //throw new InvalidPluginExecutionException("si quiere cerrar");
                            CerrarRespuesta(context, respCamp, service, true); 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                tracingService.Trace(this.GetType().ToString() + "_PreCreate {0}", ex.Message);
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }

        protected void CerrarRespuesta(IPluginExecutionContext context, Entity respCamp, IOrganizationService servicio = null, bool update = false)
        {
            try
            {
                if (!update)
                {
                    OptionSetValue osState = new OptionSetValue(1);
                    OptionSetValue osStatus = new OptionSetValue(2);

                    if (!respCamp.Attributes.Contains(fieldStatecode))
                    {
                        respCamp.Attributes.Add(new KeyValuePair<String, Object>(fieldStatecode, osState));
                    }
                    if (!respCamp.Attributes.Contains(fieldStatuscode))
                    {
                        respCamp.Attributes.Add(new KeyValuePair<String, Object>(fieldStatuscode, osStatus));
                    }

                    respCamp.Attributes[fieldStatecode] = osState;
                    respCamp.Attributes[fieldStatuscode] = osStatus;

                    context.InputParameters["Target"] = respCamp;
                }
                else if(servicio != null)
                {
                    SetStateRequest stateReq = new SetStateRequest();
                    EntityReference er = respCamp.ToEntityReference();
                    stateReq.EntityMoniker = er;
                    stateReq.State = new OptionSetValue(1);
                    stateReq.Status = new OptionSetValue(2);
                    servicio.Execute(stateReq);
                }
            } catch(Exception ex)
            {
                throw new Exception("Error: "+ex.Message);
            }
        }

        protected bool esRespuestaCerrada(IOrganizationService servicio, Entity respCamp)
        {
            if (respCamp.Id != null && respCamp.Id != Guid.Empty)
            {
                Entity entidad = servicio.Retrieve(respCamp.LogicalName, respCamp.Id, new ColumnSet(fieldStatecode));

                if (entidad.Attributes.Contains(fieldStatecode))
                {
                    OptionSetValue opEstado = (OptionSetValue)entidad.Attributes[fieldStatecode];
                    if (opEstado != null && opEstado.Value == 1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected bool QuiereCerrarRespuesta(Entity resCamp)
        {            
            try
            {
                if (resCamp.Attributes.Contains(fieldCerrarResp))
                {
                    return bool.Parse(resCamp.Attributes[fieldCerrarResp].ToString());
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
