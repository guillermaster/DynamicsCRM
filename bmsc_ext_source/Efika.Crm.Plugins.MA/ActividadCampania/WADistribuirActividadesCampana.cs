using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System;
using System.ServiceModel;
using System.Collections;
// Microsoft Dynamics CRM namespace(s)
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Workflow;

namespace Efika.Crm.Plugins.MA.ActividadCampania
{
    public class WADistribuirActividadesCampana : CodeActivity
    {
        private string fetchProductosCampania=@"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""true"">
                              <entity name=""product"">
                                <attribute name=""name"" />
                                <attribute name=""efk_tipo_productoid"" />
                                <attribute name=""efk_familia_productosid"" />
                                <attribute name=""statecode"" />
                                <attribute name=""efk_habilitado_comercializar"" />
                                <attribute name=""productid"" />
                                <order attribute=""name"" descending=""false"" />
                                <link-entity name=""campaignitem"" from=""entityid"" to=""productid"" visible=""false"" intersect=""true"">
                                  <link-entity name=""campaign"" from=""campaignid"" to=""campaignid"" alias=""aa"">
                                    <filter type=""and"">
                                      <condition attribute=""campaignid"" operator=""eq"" uitype=""campaign"" value=""{0}"" />
                                    </filter>
                                  </link-entity>
                                </link-entity>
                              </entity>
                            </fetch>";

        private string fetchCampaniasClientes = @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""false"">
                          <entity name=""efk_cliente_campania"">
                            <attribute name=""efk_cliente_campaniaid"" />
                            <attribute name=""efk_nombre"" />
                            <attribute name=""createdon"" />
                            <attribute name=""efk_productid"" />
                            <attribute name=""efk_monto_pre_aprobado"" />
                            <attribute name=""efk_campaignid"" />
                            <attribute name=""efk_accountid"" />
                            <order attribute=""efk_nombre"" descending=""false"" />
                            <filter type=""and"">
                              <condition attribute=""efk_accountid"" operator=""eq""  uitype=""account"" value=""{0}"" />
                                <condition attribute=""efk_campaignid"" operator=""eq"" uitype=""campaign"" value=""{1}"" />
                            </filter>
                          </entity>
                        </fetch>";

        protected override void Execute(CodeActivityContext executionContext)
        {
            //Obtenemos el objeto para el servicio de traicing.
            ITracingService tracingService = executionContext.GetExtension<ITracingService>();
            try
            {

                //Obtenemos el objeto con la información de contexto de MS CRM
                IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();

                QueryExpression q = new QueryExpression();
                q.EntityName = "asyncoperation";
                LinkEntity le = new LinkEntity();
                ConditionExpression ce1 = new ConditionExpression();
                ce1.AttributeName = "regardingobjectid";
                ce1.Operator = ConditionOperator.Equal;
                ce1.Values.AddRange(IdActividadCampana.Get(executionContext).Id);
                ConditionExpression ce2 = new ConditionExpression();
                ce2.AttributeName = "name";
                ce2.Operator = ConditionOperator.Equal;
                ce2.Values.Add("MyPropagateByExpression BulkOperation");
                q.Criteria = new FilterExpression();
                q.Criteria.Conditions.Add(ce1);
                q.Criteria.Conditions.Add(ce2);
                q.ColumnSet = new ColumnSet();
                q.ColumnSet.AddColumns("name", "statecode");

                IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
                IOrganizationService servicio = serviceFactory.CreateOrganizationService(context.UserId);

                int contadorIntentos = 0;
                Entity operacionAsincronica;
                OptionSetValue state = null;
                do
                {
                    EntityCollection bec1 = servicio.RetrieveMultiple(q);

                    if (bec1.Entities.Count > 0)
                    {


                        operacionAsincronica = bec1.Entities[0];
                        state = ((OptionSetValue)bec1.Entities[0].Attributes["statecode"]);
                        if (state.Value != 3)
                        {
                            System.Threading.Thread.Sleep(10000);
                        }

                    }
                    contadorIntentos++;
                } while ((state == null || state.Value != 3) && contadorIntentos < 3);

                //Comenzamos la distribución
                int canal = Canal.Get(executionContext).Value;
                string enlace = Enlacerecurso.Get(executionContext);
                //Preguntamos todas aquellas actividades que esten relacionadas con esta actividad de camapaña
                QueryByAttribute qe = new QueryByAttribute();
                qe.Attributes.Add("regardingobjectid");
                qe.Values.Add(IdActividadCampana.Get(executionContext).Id );
                qe.ColumnSet = new ColumnSet();
                switch (canal)
                {
                    case 1: qe.EntityName = "phonecall"; break;
                    case 2:
                        {
                            qe.EntityName = "appointment";
                            qe.ColumnSet.AddColumn("scheduledstart");
                            qe.ColumnSet.AddColumn("scheduleddurationminutes");
                        } break;
                    case 3: qe.EntityName = "letter"; break;
                    case 5: qe.EntityName = "fax"; break;
                    case 7: qe.EntityName = "email"; break;
                    default: return;
                }
                
                qe.ColumnSet.AddColumn("activityid");
                qe.ColumnSet.AddColumn("scheduledend");
                qe.ColumnSet.AddColumn("ownerid");
                qe.ColumnSet.AddColumn("to");
                qe.ColumnSet.AddColumn("description");

                EntityCollection bec = servicio.RetrieveMultiple(qe);

                ArrayList grupos = new ArrayList();
                if (bec.Entities.Count > 0)
                {
                    //Agrupamos por propietario
                    bool asignado = false;

                    foreach (object ap in bec.Entities)
                    {
                        foreach (ArrayList l in grupos)
                        {
                            Guid idProp1;
                            idProp1 = ((EntityReference)((Entity)l[0]).Attributes["ownerid"]).Id;

                            Guid idProp2;
                            idProp2 = ((EntityReference)((Entity)ap).Attributes["ownerid"]).Id;
                            if (idProp1 == idProp2)
                            {
                                l.Add(ap);
                                asignado = true;
                            }
                        }
                        if (!asignado)
                        {
                            //creamos un nuevo grupo
                            ArrayList nuevoGrupo = new ArrayList();
                            nuevoGrupo.Add(ap);
                            grupos.Add(nuevoGrupo);
                        }
                        else
                        {
                            asignado = false;
                        }
                    }
                }

                //ahora validamos la cantidad de dias que existen entre la fecha inicial y la fecha de cierre maxima
                DateTime fechaInicio = FechaInicio.Get(executionContext);
                DateTime fechaFinal = FechaFin.Get(executionContext);
                //calculamos la fecha final sin fin de semana 
                if (fechaFinal.DayOfWeek == DayOfWeek.Saturday)
                {
                    fechaFinal=fechaFinal.AddDays(-1);
                }
                else if (fechaFinal.DayOfWeek == DayOfWeek.Sunday)
                {
                    fechaFinal=fechaFinal.AddDays(-2);
                }
               
                TimeSpan span = fechaFinal.Subtract(fechaInicio);
                int cantDias = span.Days;

                int cantidadActividades = CantidadActividadesXDia.Get(executionContext);
                int cadaCantidadDias = CadaCantidadDias.Get(executionContext); ;
                int contador = 0;
                int quitados = 0;
                bool continuar = false;
                bool excluirSabados = ExcluirSabado.Get(executionContext); ;
                bool excluirDomingos = ExcluirDomingo.Get(executionContext);
                bool excluirFeriados = ExcluirFeriados.Get(executionContext);
                
                //Obtenemos lod datos pre-aprobados
                qe = new QueryByAttribute();
                qe.EntityName = "efk_datos_importados_campana";
                qe.Attributes.Add("efk_campana_id");
                qe.Values.Add(Campana.Get(executionContext).Id);
                qe.ColumnSet = new ColumnSet();
                qe.ColumnSet.AddColumns("efk_cliente_juridico_id", "efk_cliente_natural_id", "efk_prospecto_id", "efk_cupo_monto","efk_subtipo_producto_id","efk_tasa","efk_comision",
                    "efk_plazo","efk_comision","efk_atributo_1","efk_valor_1","efk_atributo_2","efk_valor_2","efk_atributo_3","efk_valor_3");

                EntityCollection bec2 = servicio.RetrieveMultiple(qe);
                Hashtable datosImportados = new Hashtable();

                if (bec2.Entities.Count > 0)
                {
                    foreach (Entity e in bec2.Entities)
                    {
                        if (e.Attributes.Contains("efk_cliente_juridico_id"))
                        {
                            Guid id=((EntityReference)e.Attributes["efk_cliente_juridico_id"]).Id;
                            if(!datosImportados.Contains(id))
                            {
                                datosImportados.Add(id,new List<Entity>());
                            }
                            List<Entity> lista=(List<Entity>)datosImportados[id];
                            lista.Add(e);
                            continue;
                        }
                        if (e.Attributes.Contains("efk_cliente_natural_id"))
                        {
                            Guid id = ((EntityReference)e.Attributes["efk_cliente_natural_id"]).Id;
                            if (!datosImportados.Contains(id))
                            {
                                datosImportados.Add(id, new List<Entity>());
                            }
                            List<Entity> lista = (List<Entity>)datosImportados[id];
                            lista.Add(e);
                            continue;
                        }
                        if (e.Attributes.Contains("efk_prospecto_id"))
                        {
                            Guid id = ((EntityReference)e.Attributes["efk_prospecto_id"]).Id;
                            if (!datosImportados.Contains(id))
                            {
                                datosImportados.Add(id, new List<Entity>());
                            }
                            List<Entity> lista = (List<Entity>)datosImportados[id];
                            lista.Add(e);
                            continue;
                        }
                    }
                }

                //Obtenemos el calendario de feriados
                QueryByAttribute query = new QueryByAttribute();
                query.EntityName = "calendar";
                query.Attributes.Add("name");
                query.Values.Add("Business Closure Calendar");
                query.ColumnSet.AllColumns = true;

                EntityCollection ent = servicio.RetrieveMultiple(query);
                Entity calendario = null;


                if (ent.Entities.Count > 0)
                {
                    calendario = (Entity)ent.Entities[0];
                }

                //Obtenemos todos los productos que están asociados
                string strFetchProductosCampania = String.Format(fetchProductosCampania, Campana.Get(executionContext).Id);
                EntityCollection productos = servicio.RetrieveMultiple(new FetchExpression(strFetchProductosCampania));

                for (int i = 0; i < cantDias; i++)
                {
                    continuar = false;
                    DateTime fecha1 = fechaInicio.AddDays(i);
                    DateTime fechaActividad;

                    fechaActividad = fecha1;
                    if (excluirSabados && fechaActividad.DayOfWeek == DayOfWeek.Saturday)
                    {
                        cantDias++;
                        contador++;
                        continue;
                    }
                    if (excluirDomingos && fechaActividad.DayOfWeek == DayOfWeek.Sunday)
                    {
                        cantDias++;
                        contador++;
                        continue;
                    }
                    if (excluirFeriados && CalculoFechaFinal.EsFeriado(fechaActividad,calendario,excluirSabados,excluirDomingos))
                    {
                        cantDias++;
                        contador++;
                        continue;
                    }
                    if (i == contador)
                    {
                        if (fechaActividad > fechaFinal)
                        {
                            fechaActividad = fechaFinal;
                        }
                        foreach (ArrayList l in grupos)
                        {
                            quitados = 0;
                            do
                            {
                                if (l.Count > 0)
                                {
                                    string descripcionOriginal = "";
                                    continuar = true;
                                    Entity entidad = (Entity)l[0];
                                    entidad.Attributes["scheduledend"]= fechaActividad;
                                    if (entidad.Attributes.Contains("description") && entidad.Attributes["description"] != null)
                                        descripcionOriginal = (string)entidad.Attributes["description"];

                                    //Lista con todos los productos
                                    List<Entity> listProductosParticipantes = new List<Entity>();

                                    ///Obtenemos el dato pre-aprobado
                                    string descripcion = "";
                                    if(entidad.Attributes["to"]!=null)
                                    {
                                        EntityReference party=(EntityReference)((EntityCollection)entidad.Attributes["to"]).Entities[0].Attributes["partyid"];
                                        string tipoEntidad = party.LogicalName;

                                        descripcion = GenerarDescripcionYProductos(datosImportados, party, Campana.Get(executionContext).Id, productos, listProductosParticipantes,servicio);
                                    }
                                    entidad.Attributes["description"] = descripcion + "\n\n"+descripcionOriginal;
                                    entidad.Attributes["efk_enlace_recursos"] = enlace;
                                    servicio.Update(entidad);

                                    l.RemoveAt(0);
                                    quitados++;
                                }
                            } while (quitados < cantidadActividades && l.Count > 0);
                        }

                        if (!continuar)
                            break;

                        contador += cadaCantidadDias;
                    }
                }
                
                //si aún quedan actividades, todas las actualizamos con la fecha final
                foreach (ArrayList l in grupos)
                {
                    foreach (Entity entidad in l)
                    {
                        List<Entity> listProductosParticipantes = new List<Entity>(); 
                        if (entidad.Attributes["to"] != null)
                        {
                            EntityReference party = (EntityReference)((EntityCollection)entidad.Attributes["to"]).Entities[0].Attributes["partyid"];
                            
                            string descripcionOriginal = "";
                            if (entidad.Attributes.Contains("description") && entidad.Attributes["description"] != null)
                                descripcionOriginal = (string)entidad.Attributes["description"];
                            string descripcion = GenerarDescripcionYProductos(datosImportados, party, Campana.Get(executionContext).Id, productos, listProductosParticipantes,servicio);

                            if(descripcion!=null && descripcion!="")
                                entidad.Attributes["description"] = descripcion+"\n"+descripcionOriginal;
                        }
                        entidad.Attributes["scheduledend"] = fechaFinal;
                        entidad.Attributes["efk_enlace_recursos"] = enlace;
                        servicio.Update(entidad);
                    }
                }

                //Actualizamos el inicio real de la actividad de campaña
                Entity actividadCamp = new Entity("campaignactivity");
                actividadCamp["actualstart"] = DateTime.Now;
                actividadCamp.Id = IdActividadCampana.Get(executionContext).Id;
                servicio.Update(actividadCamp);

            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("An error occurred in the FollupupPlugin plug-in.", ex);
            }
            //</snippetFollowupPlugin3>

            catch (Exception ex)
            {
                tracingService.Trace("FollowupPlugin: {0}", ex.ToString());
                throw;
            }
        }

        private string GenerarDescripcionYProductos(Hashtable datosImportados, EntityReference party, Guid idCampania, 
            EntityCollection productos, List<Entity> listProductosParticipantes, IOrganizationService servicio)
        {
            string descripcion = "";

            //Obtenemos el registro de datos pre-aprobados, si es que existiere alguno
            if (datosImportados.Contains(party.Id))
            {
                //Obtenemos los datos y los agregamos a la descpcion
                List<Entity> lista = (List<Entity>)datosImportados[party.Id];

                foreach (Entity e in lista)
                {
                    //Creamos el registro de Campaña participante
                    Entity campaniaParticipante = new Entity("efk_cliente_campania");
                    campaniaParticipante.Attributes["efk_campaignid"] = new EntityReference("campaign", idCampania);
                    if (party.LogicalName.Equals("account"))
                        campaniaParticipante.Attributes["efk_accountid"] = new EntityReference(party.LogicalName, party.Id);

                    if (e.Attributes.Contains("efk_subtipo_producto_id") && e.Attributes["efk_subtipo_producto_id"] != null)
                    {
                        descripcion += "\n";
                        descripcion += "Producto: " + ((EntityReference)e.Attributes["efk_subtipo_producto_id"]).Name;

                        campaniaParticipante.Attributes["efk_productid"] = new EntityReference("product", ((EntityReference)e.Attributes["efk_subtipo_producto_id"]).Id);
                    }
                    if (e.Attributes.Contains("efk_cupo_monto") && e.Attributes["efk_cupo_monto"] != null)
                    {
                        descripcion += "\n";
                        descripcion += "Cupo/Monto Pre-aprobado: " + ((Money)e.Attributes["efk_cupo_monto"]).Value.ToString("c");

                        campaniaParticipante.Attributes["efk_monto_pre_aprobado"] = (Money)e.Attributes["efk_cupo_monto"];
                    }
                    if (e.Attributes.Contains("efk_tasa") && e.Attributes["efk_tasa"] != null)
                    {
                        descripcion += "\n";
                        descripcion += "Tasa: " + ((decimal)e.Attributes["efk_tasa"]).ToString("F2")+"%";

                        campaniaParticipante.Attributes["efk_tasa"] = e.Attributes["efk_tasa"];
                    }
                    if (e.Attributes.Contains("efk_plazo") && e.Attributes["efk_plazo"] != null)
                    {
                        descripcion += "\n";
                        descripcion += "Plazo (días): " + ((int)e.Attributes["efk_plazo"]).ToString();

                        campaniaParticipante.Attributes["efk_plazo"] = e.Attributes["efk_plazo"];
                    }
                    if (e.Attributes.Contains("efk_comision") && e.Attributes["efk_comision"] != null)
                    {
                        descripcion += "\n";
                        descripcion += "Comisión: " + ((Money)e.Attributes["efk_comision"]).Value.ToString("c");

                        campaniaParticipante.Attributes["efk_comision"] = e.Attributes["efk_comision"];
                    }
                    if (e.Attributes.Contains("efk_atributo_1") && e.Attributes.Contains("efk_valor_1") 
                        && e.Attributes["efk_atributo_1"] != null && e.Attributes["efk_valor_1"] != null)
                    {
                        descripcion += "\n";
                        descripcion += e.Attributes["efk_atributo_1"].ToString() + ": " + e.Attributes["efk_valor_1"].ToString();

                        campaniaParticipante.Attributes["efk_atributo_1"] = e.Attributes["efk_atributo_1"];
                        campaniaParticipante.Attributes["efk_valor_1"] = e.Attributes["efk_valor_1"];
                    }
                    if (e.Attributes.Contains("efk_atributo_2") && e.Attributes.Contains("efk_valor_2")
                        && e.Attributes["efk_atributo_2"] != null && e.Attributes["efk_valor_2"] != null)
                    {
                        descripcion += "\n";
                        descripcion += e.Attributes["efk_atributo_2"].ToString() + ": " + e.Attributes["efk_valor_2"].ToString();

                        campaniaParticipante.Attributes["efk_atributo_2"] = e.Attributes["efk_atributo_2"];
                        campaniaParticipante.Attributes["efk_valor_2"] = e.Attributes["efk_valor_2"];
                    }
                    if (e.Attributes.Contains("efk_atributo_3") && e.Attributes.Contains("efk_valor_3")
                        && e.Attributes["efk_atributo_3"] != null && e.Attributes["efk_valor_3"] != null)
                    {
                        descripcion += "\n";
                        descripcion += e.Attributes["efk_atributo_3"].ToString() + ": " + e.Attributes["efk_valor_3"].ToString();

                        campaniaParticipante.Attributes["efk_atributo_3"] = e.Attributes["efk_atributo_3"];
                        campaniaParticipante.Attributes["efk_valor_3"] = e.Attributes["efk_valor_3"];
                    }
                    descripcion += "\n";

                    if (campaniaParticipante.Attributes.Contains("efk_accountid") && campaniaParticipante.Contains("efk_productid"))
                    {
                        listProductosParticipantes.Add(campaniaParticipante);
                    }
                }

                if (descripcion != "")
                {
                    descripcion = "Oferta para el cliente:" + descripcion;
                }
            }

            if (productos.Entities.Count > 0)
            {
                descripcion += "\nProductos ofertados en la campaña: \n";
                //Agregamos los productos de la campaña
                foreach (Entity producto in productos.Entities)
                {
                    descripcion += " - " + producto.Attributes["name"] + "\n";

                    Entity campaniaParticipante = new Entity("efk_cliente_campania");
                    campaniaParticipante.Attributes["efk_campaignid"] = new EntityReference("campaign", idCampania);
                    if (party.LogicalName.Equals("account"))
                        campaniaParticipante.Attributes["efk_accountid"] = new EntityReference(party.LogicalName, party.Id);
                    campaniaParticipante.Attributes["efk_productid"] = new EntityReference("product", producto.Id);

                    if (campaniaParticipante.Attributes.Contains("efk_accountid") && campaniaParticipante.Contains("efk_productid"))
                    {
                        //Validamos que la el producto no este en la lista
                        bool existe = false;
                        foreach (Entity registro in listProductosParticipantes)
                        {
                            if (((EntityReference)registro.Attributes["efk_productid"]).Id ==
                                 producto.Id)
                            {
                                existe = true;
                                break;
                            }
                        }
                        if (!existe)
                        {
                            listProductosParticipantes.Add(campaniaParticipante);
                        }
                    }
                }
            }

            //Primero eliminamos los registros participante de campaña
            string strFetchCampaniasClientes = String.Format(fetchCampaniasClientes, party.Id, idCampania);
            EntityCollection registrosExistentes = servicio.RetrieveMultiple(new FetchExpression(strFetchCampaniasClientes));

            foreach (Entity registro in registrosExistentes.Entities)
            {
                servicio.Delete(registro.LogicalName, registro.Id);
            }

            //Creamos los registros participantes de campaña
            foreach (Entity registro in listProductosParticipantes)
            {
                //Obtenemos el OwnerId del cliente y lo asignamos al registro
                Entity cliente=servicio.Retrieve(((EntityReference)registro["efk_accountid"]).LogicalName, ((EntityReference)registro["efk_accountid"]).Id,
                    new ColumnSet("ownerid"));
                registro["ownerid"] = (EntityReference)cliente.Attributes["ownerid"];

                servicio.Create(registro);
            }

            return descripcion;
        }

        [Input("Id Actividad de Campaña")]
        [ReferenceTarget("campaignactivity")]
        public InArgument<EntityReference> IdActividadCampana { get; set; }

        [Input("Id Campaña")]
        [ReferenceTarget("campaign")]
        public InArgument<EntityReference> Campana { get; set; }

        [Input("Tipo de canal")]
        [AttributeTarget("campaignactivity","channeltypecode")]
        public InArgument<OptionSetValue> Canal{ get; set; }

        [Input("Fecha de inicio")]
        public InArgument<DateTime> FechaInicio{ get; set; }

        [Input("Fecha de fin")]
        public InArgument<DateTime> FechaFin { get; set; }

        [Input("Cantidad de actividades por día")]
        public InArgument<int> CantidadActividadesXDia { get; set; }

        [Input("Cada cantidad de días")]
        public InArgument<int> CadaCantidadDias { get; set; }

        [Input("Enlace recurso")]
        public InArgument<string> Enlacerecurso { get; set; }

        [Input("Excluir sabados")]
        public InArgument<bool> ExcluirSabado { get; set; }

        [Input("Excluir domingo")]
        public InArgument<bool> ExcluirDomingo { get; set; }

        [Input("Excluir feriados")]
        public InArgument<bool> ExcluirFeriados { get; set; }
    }
}
