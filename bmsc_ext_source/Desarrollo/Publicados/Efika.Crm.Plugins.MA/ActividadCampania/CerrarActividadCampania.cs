using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Crm.Sdk.Messages;


namespace Efika.Crm.Plugins.MA.ActividadCampania
{
    public class CerrarActividadCampania : IPlugin
    {
        private const string APPOINTMENT = "appointment";
        private const string EMAIL = "email";
        private const string FAX = "fax";
        private const string LETTER = "letter";
        private const string PHONECALL = "phonecall";
        private const string TASK = "task";
        

        public void Execute(IServiceProvider serviceProvider)
        {

            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);

            try
            {
                Entity entRespuesta = (Entity)context.InputParameters["Target"];
                if (entRespuesta.Contains("originatingactivityid"))
                {
                    EntityReference refActividadCamp = (EntityReference)entRespuesta.Attributes["originatingactivityid"];
                    cerrarActividadCampania(refActividadCamp, service);
                }
            }
            catch (Exception ex)
            {
                tracingService.Trace(this.GetType().ToString() + "_PreCreate {0}", ex.Message);
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }

        private void cerrarActividadCampania(EntityReference refActividadCamp, IOrganizationService servicio)
        {
            SetStateRequest stateReq = new SetStateRequest();
            stateReq.EntityMoniker = refActividadCamp;
            stateReq.State = getCompletedState(refActividadCamp);
            stateReq.Status = getCompletedStatus(refActividadCamp);
            servicio.Execute(stateReq);
        }

        private OptionSetValue getCompletedState(EntityReference refActividad)
        {
            OptionSetValue optionSet;
            switch (refActividad.LogicalName)
            {
                case APPOINTMENT:
                case EMAIL:
                case FAX:
                case LETTER:
                case PHONECALL:
                case TASK:
                default:
                    optionSet = new OptionSetValue(1);//Completed
                    break;
            }
            return optionSet;
        }

        private OptionSetValue getCompletedStatus(EntityReference refActividad)
        {
            OptionSetValue optionSet;
            switch (refActividad.LogicalName)
            {
                case APPOINTMENT:
                    optionSet = new OptionSetValue(3);//Completed
                    break;
                case EMAIL:
                case FAX:
                case PHONECALL:
                    optionSet = new OptionSetValue(2);//Completed
                    break;
                case LETTER:
                    optionSet = new OptionSetValue(4);//Sent
                    break;
                case TASK:
                    optionSet = new OptionSetValue(5);//Completed
                    break;
                default:
                    optionSet = new OptionSetValue(1);
                    break;
            }
            return optionSet;
        }
    }
}
