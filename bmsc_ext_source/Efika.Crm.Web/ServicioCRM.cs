using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk.Client;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Xrm.Sdk;
using System.Configuration;
using Microsoft.Xrm.Sdk.Query;
////Daniel Vaca
using Microsoft.Crm.Sdk.Messages;
using System.IO;
using Efika.Crm.Entidades;

namespace Efika.Crm.Web
{
    public class ServicioCRM
    {/*
        #region Propiedades

        private OrganizationDetail CurrentOrganizationDetail { get; set; }
        //private IDiscoveryService DiscoveryService { get; set; }
        private IOrganizationService OrgService { get; set; }
        private static string Organizacion = ConfigurationManager.AppSettings["Organizacion"];
        private static string Servidor = ConfigurationManager.AppSettings["ServidorCRM"];

        #endregion


        public ServicioCRM()
        {
            DiscoveryServiceProxy dsp;
            
            
            var creds = new ClientCredentials(); 
            if (Organizacion == null)
            {
                throw new ApplicationException("You must select an organization before connecting");
            }

            IServiceConfiguration<IDiscoveryService> dinfo = ServiceConfigurationFactory.CreateConfiguration<IDiscoveryService>(GetDiscoveryServiceUri(Servidor));

            dsp = new DiscoveryServiceProxy(dinfo, creds);
            dsp.Authenticate();

            RetrieveOrganizationsRequest orgRequest = new RetrieveOrganizationsRequest();
            RetrieveOrganizationsResponse orgResponse = dsp.Execute(orgRequest) as RetrieveOrganizationsResponse;
                
            foreach (var details in orgResponse.Details)
            {
                if (details.UniqueName == Organizacion)
                {
                    this.CurrentOrganizationDetail = details as OrganizationDetail;
                    break;
                }
            }

            //throw new Exception("DANIEL!!!");
    
            Uri orgServiceUri = new Uri(CurrentOrganizationDetail.Endpoints[EndpointType.OrganizationService]);
            IServiceConfiguration<IOrganizationService> orgConfigInfo =
                                        ServiceConfigurationFactory.CreateConfiguration<IOrganizationService>(orgServiceUri);
                                        //ServiceConfigurationFactory.CreateConfiguration<IOrganizationService>(new Uri("http://crmappsrv:5555/DESARROLLO1/XRMServices/2011/Organization.svc"));
            OrgService = new OrganizationServiceProxy(orgConfigInfo, creds);
                
        }

        public Uri GetDiscoveryServiceUri(string serverName)
        {
            string discoSuffix = @"/XRMServices/2011/Discovery.svc";
            //return new Uri("http://crmappsrv:5555/DESARROLLO1/XRMServices/2011/Discovery.svc");
            return new Uri(string.Format("{0}{1}", serverName, discoSuffix));
        }

        public Guid GuardarListaMarketing(ImportacionListaMarketing dato, ref string resultadoError)
        {
            Entity objListaMarketing = new Entity("efk_importacion_listas_marketing");
            objListaMarketing["efk_nombre"] = dato.Nombre;
            objListaMarketing["efk_ruta_archivo_carga"] = dato.RutaArchivoCarga;
            objListaMarketing["efk_fecha_inicio_ejecucion"] = dato.FechaInicioEjecucion;
            objListaMarketing["efk_estado_carga"] = new OptionSetValue(100000000);
            return OrgService.Create(objListaMarketing);
        }

        public void ActualizarListaMarketing(Guid IdRegistroImportacion, Guid IdNota)
        {
            Entity registro = new Entity("efk_importacion_listas_marketing");
            registro["efk_importacion_listas_marketingid"] = IdRegistroImportacion;
            registro["efk_ruta_archivo_carga"] = IdNota.ToString();

            OrgService.Update(registro);
        }

        public void ActualizarListaPreaprobado(Guid IdRegistroImportacion, Guid IdNota)
        {
            Entity registro = new Entity("efk_importacion_datos_preaprobados");
            registro["efk_importacion_datos_preaprobadosid"] = IdRegistroImportacion;
            registro["efk_ruta_archivo_carga"] = IdNota.ToString();

            OrgService.Update(registro);
        }

        public Guid AdjuntarArchivoRegistroImportacion(Guid IdRegistroImportacion, string RutaArchivo, string NombreArchivo)
        {
            Stream lector = File.OpenRead(RutaArchivo);
            byte[] datosLog = new byte[lector.Length];
            lector.Read(datosLog, 0, datosLog.Length);
            string datosCodificados = Convert.ToBase64String(datosLog);

            Entity adjunto2 = new Entity("annotation");
            adjunto2["subject"] = "Archivo de carga";
            adjunto2["filename"] = NombreArchivo;
            adjunto2["documentbody"] = datosCodificados;
            if (NombreArchivo.Substring(NombreArchivo.LastIndexOf(".")).ToUpper().Equals(".xls"))
            {
                adjunto2["mimetype"] = @"application/vnd.ms-excel";
            }
            else{
                adjunto2["mimetype"] = @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }
            
            adjunto2["objectid"] = new EntityReference("efk_importacion_listas_marketing", IdRegistroImportacion);

            return OrgService.Create(adjunto2);
        }

        public Guid AdjuntarArchivoRegistroImportacion(Guid IdRegistroImportacion, Stream stream, string NombreArchivo)
        {
            //Stream lector = File.OpenRead(RutaArchivo);
            Stream lector = stream;
            byte[] datosLog = new byte[lector.Length];
            lector.Read(datosLog, 0, datosLog.Length);
            string datosCodificados = Convert.ToBase64String(datosLog);

            Entity adjunto2 = new Entity("annotation");
            adjunto2["subject"] = "Archivo de carga";
            adjunto2["filename"] = NombreArchivo;
            adjunto2["documentbody"] = datosCodificados;
            if (NombreArchivo.Substring(NombreArchivo.LastIndexOf(".")).ToUpper().Equals(".XLS"))
            {
                adjunto2["mimetype"] = @"application/vnd.ms-excel";
            }
            else
            {
                adjunto2["mimetype"] = @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }

            adjunto2["objectid"] = new EntityReference("efk_importacion_listas_marketing", IdRegistroImportacion);

            return OrgService.Create(adjunto2);
        }

        public Guid AdjuntarArchivoRegistroImportacionPreAprobados(Guid IdRegistroImportacion, Stream stream, string NombreArchivo)
        {
            //Stream lector = File.OpenRead(RutaArchivo);
            Stream lector = stream;
            byte[] datosLog = new byte[lector.Length];
            lector.Read(datosLog, 0, datosLog.Length);
            string datosCodificados = Convert.ToBase64String(datosLog);

            Entity adjunto2 = new Entity("annotation");
            adjunto2["subject"] = "Archivo de carga";
            adjunto2["filename"] = NombreArchivo;
            adjunto2["documentbody"] = datosCodificados;
            if (NombreArchivo.Substring(NombreArchivo.LastIndexOf(".")).ToUpper().Equals(".XLS"))
            {
                adjunto2["mimetype"] = @"application/vnd.ms-excel";
            }
            else
            {
                adjunto2["mimetype"] = @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }

            adjunto2["objectid"] = new EntityReference("efk_importacion_datos_preaprobados", IdRegistroImportacion);

            return OrgService.Create(adjunto2);
        }

        public void DescargarArchivo(Guid IdNota)
        {
            ColumnSet cols=new ColumnSet(true);

            Entity nota= OrgService.Retrieve("annotation", IdNota, cols);

            
        }

        public Guid GuardarListaPreAprobado(ListaPreaprobado dato, ref string resultadoError)
        {
            Entity objPreAprobado = new Entity("efk_importacion_datos_preaprobados");
            objPreAprobado["efk_name"] = dato.Nombre;
            objPreAprobado["efk_fecha_inicio_ejecucion"] = dato.FechaInicioEjecucion;
            objPreAprobado["efk_estado_carga"] = new OptionSetValue(100000000);
            objPreAprobado["efk_ruta_archivo_carga"] = dato.RutaArchivoCarga;
            objPreAprobado["efk_campaignid"] = new EntityReference("campaign",(Guid)dato.Campania["campaignid"]);
            return OrgService.Create(objPreAprobado);
        }

        public Entity ObtenerCampaniaPorId(Guid IdCampania) 
        {
            Entity resultado = null;
            ColumnSet columnas = new ColumnSet();
            columnas.AllColumns = true;
            resultado =  OrgService.Retrieve("campaign", IdCampania, columnas);
            return resultado;
        }

        public bool ValidarPrivilegio(string privilegio, Guid idUsuario)
        {
            QueryExpression queryReadPrivilege = new QueryExpression
            {
                EntityName = "privilege",
                ColumnSet = new ColumnSet("privilegeid", "name"),
                Criteria = new FilterExpression()
            };
            queryReadPrivilege.Criteria.AddCondition("name", ConditionOperator.Equal, privilegio);

            // Retrieve the prvReadQueue privilege.
            Entity readPrivilege = OrgService.RetrieveMultiple(queryReadPrivilege)[0];
            Console.WriteLine("Retrieved {0}", readPrivilege.Attributes["name"]);

            //Obtenemos todos los privilegios de un usuario
            RetrieveUserPrivilegesRequest req = new RetrieveUserPrivilegesRequest();
            req.UserId = idUsuario;
            RetrieveUserPrivilegesResponse res = (RetrieveUserPrivilegesResponse)OrgService.Execute(req);

            foreach (RolePrivilege p in res.RolePrivileges)
            {
                if (p.PrivilegeId == (Guid)readPrivilege["privilegeid"])
                {
                    return true;
                }
            }
            return false;
        }

        //public Entity ObtenerOportunidadPorId(Guid IdOportunidad)
        //{
        //    Entity resultado = null;
        //    ColumnSet columnas = new ColumnSet();
        //    columnas.AllColumns = true;
        //    resultado = OrgService.Retrieve("opportunity", IdOportunidad, columnas);
        //    return resultado;
        //}

        public bool ValidarProductosOportunidad(Guid IdOportunidad)
        {
            QueryExpression query = new QueryExpression();
            query.Distinct = true;
            query.EntityName = "opportunityproduct";
            query.NoLock = true;
            query.Criteria.AddCondition(new ConditionExpression("opportunityid", ConditionOperator.Equal, IdOportunidad));
            EntityCollection resultado = OrgService.RetrieveMultiple(query);
            return resultado.Entities.Count > 0;
        }

        public List<ItemCombo> ObtenerCompetidores()
        {
            List<ItemCombo> resultado = new List<ItemCombo>();
            QueryExpression query = new QueryExpression();
            query.Distinct = true;
            query.EntityName = "competitor";
            query.NoLock = true;
            query.ColumnSet = new ColumnSet(true);
            query.Orders.Add(new OrderExpression("name", OrderType.Ascending));

            EntityCollection competidores = OrgService.RetrieveMultiple(query);
            if (competidores.Entities.Count > 0)
            {
                ItemCombo it = new ItemCombo();
                it.Texto = "";
                it.Valor = "";
                resultado.Add(it);
                foreach (var item in competidores.Entities)
                {
                    resultado.Add(new ItemCombo { Texto = item["name"].ToString(), Valor = item.Id.ToString() });
                }
            }
            return resultado;
        }

        public void PerderOportunidad(Entity oportunidad, int estado, Guid idCompetidor, string descripcion, DateTime fechaCierre, decimal monto)
        {
            //Actualizamos la fecha de cierre verbal de la oportunidad
            //Entity opp = new Entity("opportunity");
            ////opp["efk_fecha_cierre_verbal"]=fechaCierre;
            //opp["opportunityid"]=oportunidad["opportunityid"];
            //OrgService.Update(opp);

            MarcarProductosPerteneceOfertaValor(oportunidad);

            LoseOpportunityRequest perdida = new LoseOpportunityRequest();
            Entity oportunidadPerdida = new Entity("opportunityclose");

            oportunidadPerdida["subject"] = "Oportunidad perdida";
            oportunidadPerdida["opportunityid"] = new EntityReference("opportunity", oportunidad.Id);
            if(idCompetidor!=Guid.Empty)
                oportunidadPerdida["competitorid"] = new EntityReference("competitor", idCompetidor);
            oportunidadPerdida["description"] = descripcion;
            oportunidadPerdida["actualend"] = fechaCierre;
            oportunidadPerdida["actualrevenue"] = monto;
            perdida.OpportunityClose = oportunidadPerdida;
            perdida.Status = new OptionSetValue(estado);
            OrgService.Execute(perdida);
        }

        public void GanarOportunidad(Entity oportunidad, string descripcion, DateTime fechaCierre, int estado, decimal monto)
        {
            //Actualizamos la fecha de cierre verbal de la oportunidad
            Entity opp = new Entity("opportunity");
            opp["efk_fecha_cierre_verbal"] = fechaCierre;
            opp["opportunityid"] = oportunidad["opportunityid"];
            opp["efk_estado_cierre"] = new OptionSetValue(100000000);
            opp["efk_razon_cierre_estado"] = new OptionSetValue(100000000);
            opp["salesstagecode"]=new OptionSetValue(4);
            OrgService.Update(opp);

            //Actualizamos el producto de oportunidad
            QueryExpression qe = new QueryExpression("opportunityproduct");
            qe.ColumnSet = new ColumnSet("opportunityproductid");
            qe.Criteria = new FilterExpression();
            qe.Criteria.AddCondition(new ConditionExpression("opportunityid", ConditionOperator.Equal, (Guid)oportunidad["opportunityid"]));

            EntityCollection ec = OrgService.RetrieveMultiple(qe);
            if (ec.Entities.Count > 0)
            {
                foreach (Entity prod in ec.Entities)
                {
                    prod.Attributes["efk_estado_cierre"] = new OptionSetValue(100000000);
                    OrgService.Update(prod);
                }
            }

            MarcarProductosPerteneceOfertaValor(oportunidad);

            //Cerramos la oportunidad
            WinOpportunityRequest ganada = new WinOpportunityRequest();
            Entity oportunidadGanada = new Entity("opportunityclose");
            oportunidadGanada["subject"] = "Oportunidad ganada";
            oportunidadGanada["opportunityid"] = new EntityReference("opportunity", oportunidad.Id);
            oportunidadGanada["description"] = descripcion;
            oportunidadGanada["actualend"] = fechaCierre;
            oportunidadGanada["actualrevenue"] = monto;
            ganada.OpportunityClose = oportunidadGanada;
            ganada.Status = new OptionSetValue(estado);
            // ganada["description"] = descripcion;
            //  ganada["actualend"] = fechaCierre;
            OrgService.Execute(ganada);
        }

        private void MarcarProductosPerteneceOfertaValor(Entity oportunidad)
        {
            //Obtenemos los productos de la oportunidad
            QueryExpression qe = new QueryExpression("opportunityproduct");
            qe.ColumnSet = new ColumnSet("opportunityproductid", "productid");
            qe.Criteria = new FilterExpression();
            qe.Criteria.AddCondition(new ConditionExpression("opportunityid", ConditionOperator.Equal, (Guid)oportunidad["opportunityid"]));

            EntityCollection productos = OrgService.RetrieveMultiple(qe);
            if (productos.Entities.Count > 0)
            {
                //Obtenemos la oferta de valor del cliente
                qe = new QueryExpression("efk_oferta_valor");
                qe.ColumnSet = new ColumnSet("efk_product_id");
                qe.Criteria = new FilterExpression(LogicalOperator.Or);
                qe.Criteria.AddCondition(new ConditionExpression("efk_cliente_juridico_id", ConditionOperator.Equal, ((EntityReference)oportunidad["customerid"]).Id));
                qe.Criteria.AddCondition(new ConditionExpression("efk_cliente_natural_id", ConditionOperator.Equal, ((EntityReference)oportunidad["customerid"]).Id));

                EntityCollection ofertas = OrgService.RetrieveMultiple(qe);

                bool pertenece=false;
                foreach (Entity prod in productos.Entities)
                {
                    pertenece=false;
                    //Preguntamos si el producto pertenece a la oferta de valor del cliente
                    foreach (Entity oferta in ofertas.Entities)
                    {
                        if (((EntityReference)oferta["efk_product_id"]).Id == ((EntityReference)prod["productid"]).Id)
                        {
                            pertenece = true;
                            break;
                        }
                    }
                    //Actualizamos el producto de oportunidad
                    Entity producto = new Entity("opportunityproduct");
                    producto["opportunityproductid"] = prod["opportunityproductid"];
                    producto.Attributes["efk_pertenece_oferta_valor"] = pertenece;
                    OrgService.Update(producto);
                }
            }
        }

        /// <summary>
        /// Obtiene la cantidad de días máximo anteriores para fijar fecha de cierre de oportunidad
        /// </summary>
        public int CantidadDiasCierreOportunidad()
        {
            QueryExpression qe = new QueryExpression("efk_parametros_implementacion");
            qe.ColumnSet = new ColumnSet("efk_cantidad_dias_fecha_cierre_oportunidad");

            EntityCollection col= OrgService.RetrieveMultiple(qe);
            if (col.Entities.Count > 0)
            {
                return (int)col.Entities[0].Attributes["efk_cantidad_dias_fecha_cierre_oportunidad"];
            }
            return 0;
        }
*/
    }
}