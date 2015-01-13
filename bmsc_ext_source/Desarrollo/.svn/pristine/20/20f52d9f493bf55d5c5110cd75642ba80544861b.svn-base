using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.ServiceModel;
using System.Collections;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Crm.Sdk.Messages;

namespace Efika.Crm.Plugins.SFA.Oportunidad
{
    /// <summary>
    /// Actividad de flujo de trabajo que permite compartir o descompartir una oportunidad.
    /// </summary>
    public class WACompartirOportunidad : CodeActivity
    {
        protected override void Execute(CodeActivityContext executionContext)
        {
            bool leerPermitir = false;
            bool crearPermitir = false;
            bool escribirPermitir = false;
            bool eliminarPermitir = false;
            bool anexarPermitir = false;
            bool anexarAPermitir = false;
            bool compartirPermitir = false;
            bool asignarPermitir=false;
            bool compartir = false;
            bool descompartir = false;
            Guid idUsuario = Guid.Empty;
            Guid idEquipo = Guid.Empty;
            Guid idOportunidad=Guid.Empty;

            //Obtenemos el objeto para el servicio de traicing.
            ITracingService tracingService = executionContext.GetExtension<ITracingService>();
            try
            {
                //Seteamos las variables
                leerPermitir = LeerPermitir.Get(executionContext);
                crearPermitir = CrearPermitir.Get(executionContext);
                escribirPermitir = EscribirPermitir.Get(executionContext);
                eliminarPermitir = EliminarPermitir.Get(executionContext);
                anexarPermitir = AnexarPermitir.Get(executionContext);
                anexarAPermitir = AnexarAPermitir.Get(executionContext);
                compartirPermitir = CompartirPermitir.Get(executionContext);
                compartir = Compartir.Get(executionContext);
                descompartir = Descompartir.Get(executionContext);
                asignarPermitir=AsignarPermitir.Get(executionContext);
                if (IdUsuario.Get(executionContext) != null)
                {
                    idUsuario = IdUsuario.Get(executionContext).Id;
                }
                if (IdEquipo.Get(executionContext) != null)
                {
                    idEquipo = IdEquipo.Get(executionContext).Id;
                }
                if (IdOportunidad.Get(executionContext) != null)
                {
                    idOportunidad = IdOportunidad.Get(executionContext).Id;
                }

                if ((idEquipo != Guid.Empty || idUsuario != Guid.Empty) && idOportunidad!=Guid.Empty)
                {
                    //Es decir, se especifico al menos un usuario o un equipo

                    //Obtenemos el objeto con la información de contexto de MS CRM
                    IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
                    IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
                    IOrganizationService servicio = serviceFactory.CreateOrganizationService(context.UserId);

                    AccessRights accesos=0;
                    if(crearPermitir)
                        accesos=accesos | AccessRights.CreateAccess;
                    if(leerPermitir)
                        accesos=accesos | AccessRights.ReadAccess;
                    if(escribirPermitir)
                        accesos=accesos | AccessRights.WriteAccess;
                    if(eliminarPermitir)
                        accesos=accesos | AccessRights.DeleteAccess;
                    if(anexarPermitir)
                        accesos=accesos | AccessRights.AppendAccess;
                    if(anexarAPermitir)
                        accesos=accesos | AccessRights.AppendToAccess;
                    if(asignarPermitir)
                        accesos=accesos | AccessRights.AssignAccess;
                    if(compartirPermitir)
                        accesos=accesos | AccessRights.ShareAccess;
                    

                    if (compartir)
                    {
                        //Comaprtimos el registro
                        //Primero con el usuario
                        GrantAccessRequest ga = new GrantAccessRequest();
                        ga.Target = new EntityReference("opportunity", idOportunidad);

                        if(idUsuario!=Guid.Empty){
                            tracingService.Trace("Compartir a usuario: " + idUsuario.ToString() + ". ");

                            ga.PrincipalAccess = new PrincipalAccess();
                            ga.PrincipalAccess.Principal = new EntityReference("systemuser", idUsuario);
                            ga.PrincipalAccess.AccessMask = accesos;

                            servicio.Execute(ga);
                        }
                        if (idEquipo != Guid.Empty)
                        {
                            tracingService.Trace("Compartir a equipo: "+idEquipo.ToString()+". ");

                            ga.PrincipalAccess = new PrincipalAccess();
                            ga.PrincipalAccess.Principal = new EntityReference("team", idEquipo);
                            ga.PrincipalAccess.AccessMask = accesos;

                            servicio.Execute(ga);
                        }
                    }
                    if (descompartir)
                    {
                        RevokeAccessRequest ra = new RevokeAccessRequest();
                        ra.Target = new EntityReference("opportunity", idOportunidad);

                        if (idUsuario != Guid.Empty)
                        {
                            tracingService.Trace("Descompartir a usuario: " + idUsuario.ToString() + ". ");
                            ra.Revokee = new EntityReference("systemuser", idUsuario);

                            servicio.Execute(ra);
                        }

                        if (idEquipo != Guid.Empty)
                        {
                            tracingService.Trace("Descompartir a equipo: " + idEquipo.ToString() + ". ");
                            ra.Revokee = new EntityReference("team", idEquipo);

                            servicio.Execute(ra);
                        }
                    }

                    ResultadoCorrecto.Set(executionContext, true);
                    
                }
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("Ha ocurrido un error en el Workflow Activity: WACompartirOportunidad.", ex);
            }
            catch (Exception ex)
            {
                tracingService.Trace("FollowupPlugin: {0}", ex.ToString());
                throw;
            }
        }

        #region Variables

        [Input("Leer - permitir")]
        [Default("False")]
        public InArgument<bool> LeerPermitir { get; set; }

        [Input("Crear - permitir")]
        [Default("False")]
        public InArgument<bool> CrearPermitir { get; set; }

        [Input("Escribir - permitir")]
        [Default("False")]
        public InArgument<bool> EscribirPermitir { get; set; }

        [Input("Eliminar - permitir")]
        [Default("False")]
        public InArgument<bool> EliminarPermitir { get; set; }

        [Input("Anexar - permitir")]
        [Default("False")]
        public InArgument<bool> AnexarPermitir { get; set; }

        [Input("Anexar a - permitir")]
        [Default("False")]
        public InArgument<bool> AnexarAPermitir { get; set; }
        
        [Input("Compartir - permitir")]
        [Default("False")]
        public InArgument<bool> CompartirPermitir { get; set; }

        [Input("Asignar - permitir")]
        [Default("False")]
        public InArgument<bool> AsignarPermitir { get; set; }

        [Input("Usuario")]
        [ReferenceTarget("systemuser")]
        public InArgument<EntityReference> IdUsuario { get; set; }

        [Input("Equipo")]
        [ReferenceTarget("team")]
        public InArgument<EntityReference> IdEquipo { get; set; }

        [Input("Oportunidad")]
        [ReferenceTarget("opportunity")]
        public InArgument<EntityReference> IdOportunidad { get; set; }

        [Output("Resultado Correcto")]
        [Default("False")]
        public OutArgument<bool> ResultadoCorrecto { get; set; }

        [Input("Compartir")]
        [Default("True")]
        public InArgument<bool> Compartir { get; set; }

        [Input("Descompartir")]
        [Default("False")]
        public InArgument<bool> Descompartir { get; set; }

        #endregion
    }
}
