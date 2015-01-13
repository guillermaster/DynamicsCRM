using System;
using System.Collections.Generic;
using System.Text;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk.Query;
using ex = Net.SourceForge.Koogra.Excel;
using ex2 = Net.SourceForge.Koogra.Excel2007;
using Net.SourceForge.Koogra;
using System.IO;
using Microsoft.Crm.Sdk.Messages;
using System.Security.Principal;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Messages;


namespace Efika.Crm.Plugins.MA.ListaMarketing
{
    class ProcesarListaMarketing
    {

        #region Propiedades

        private List<Entity> DetalleProspectoNuevo { get; set; }

        private int CuentaClienteNatural { get; set; }
        private int CuentaClienteJuridico { get; set; }
        private int CuentaProspectoNuevo { get; set; }
        private int CuentaProspectoExistente { get; set; }
        private int CuentaErrores { get; set; }
        private StringBuilder Errores { get; set; }
        private string RutaArchivoLog { get; set; }
        IOrganizationService ServicioCRM { get; set; }
        ITracingService tracingService { get; set; }
        private Guid IdLista { get; set; }

        List<OptionMetadata> Ciudades { get; set; }


        public Entity ImportacionListaMarketing { get; set; }

        private IWorkflowContext contexto { get; set; }

        public string CarpetaLog { get; set; } 

        #endregion

        public ProcesarListaMarketing(IOrganizationService ServicioCRM,
            ITracingService tracingService, IWorkflowContext contexto, Guid idLista)
        {
            this.ServicioCRM = ServicioCRM;
            this.tracingService = tracingService;
            this.contexto = contexto;
            this.IdLista = idLista;
        }


        public void EjecutarImportacion()
        {
            Errores = new StringBuilder();
            Errores.AppendLine("-- Proceso de carga iniciado.");
            tracingService.Trace("Creando enlace a servicio CRM.");
            tracingService.Trace("Enlace a servicio creado.");
            tracingService.Trace("Obteniendo parámetro.");
            tracingService.Trace("IdLista actual : " + IdLista);
            ColumnSet columnas = new ColumnSet();
            columnas.AllColumns = true;

            ImportacionListaMarketing = ServicioCRM.Retrieve("efk_importacion_listas_marketing", IdLista, columnas);

            tracingService.Trace("Obteniendo registros de ciudades desde CRM.");
            
            tracingService.Trace("Procesando datos del archivo Excel.");
            List<DatoListaMarketing> datosLista = ProcesarArchivoExcel(ImportacionListaMarketing["efk_ruta_archivo_carga"].ToString());
            tracingService.Trace("Analizando los datos obtenido del archivo.");
            AnalizarDatosImportados(datosLista, ImportacionListaMarketing);
            tracingService.Trace("Actualizando datos de la Importación de la lista de Marketing.");
            ActualizarImportacionListaMarketing(ImportacionListaMarketing);
            EnviarCorreoElectronico(ImportacionListaMarketing);
            tracingService.Trace("Proceso concluido con éxito.");
        }

        private List<DatoListaMarketing> ProcesarArchivoExcel(string IdNota)
        {
            List<DatoListaMarketing> lista = new List<DatoListaMarketing>();
            uint numfila = 0;
            try
            {
                ColumnSet cols = new ColumnSet(true);
                Entity nota = ServicioCRM.Retrieve("annotation", new Guid(IdNota), cols);
                string nombreArchivo = (string)nota["filename"];

                IWorkbook libro;
                IWorksheet hoja;

                if (Path.GetExtension(nombreArchivo).ToUpper() == ".XLS")
                {
                    byte[] fileContent = Convert.FromBase64String((string)nota["documentbody"]);
                    MemoryStream stream = new MemoryStream(fileContent);
                    libro = new ex.Workbook(stream);
                    hoja = libro.Worksheets.GetWorksheetByIndex(0);
                }
                else
                {
                    byte[] fileContent = Convert.FromBase64String((string)nota["documentbody"]);
                    MemoryStream stream = new MemoryStream(fileContent);
                    libro = new ex2.Workbook((Stream)stream);
                    hoja = libro.Worksheets.GetWorksheetByIndex(0);
                }


                for (uint fila = hoja.FirstRow + 1; fila <= hoja.LastRow; fila++)
                {
                    numfila = fila;
                    IRow datos = hoja.Rows.GetRow(fila);

                    if (datos.IsEmpty())
                        continue;
                    try
                    {
                        DatoListaMarketing registro = new DatoListaMarketing();

                        registro.numeroFila = (int)numfila;
                        //Comenzamos a preguntar campo por campo
                        if (datos.GetCell(0) != null && datos.GetCell(0).Value != null)
                        {
                            int valor = 0;
                            if (Int32.TryParse(datos.GetCell(0).Value.ToString(), out valor))
                            {
                                registro.TipoIdentificacion = valor;
                            }
                            else
                            {
                                throw new Exception("Error en línea " + numfila + ": El tipo de dato para la columna TIPO_IDENTIFICACION es incorrecto.");
                            }

                            if (datos.GetCell(1) != null && datos.GetCell(1).Value != null)
                            {
                                registro.Identificacion = datos.GetCell(1).Value.ToString();
                            }
                            else{
                                throw new Exception("Error en línea " + numfila + ": El registro posee tipo de identificación pero no número de identificación.");
                            }
                            
                        }
                        else if (datos.GetCell(1) != null && datos.GetCell(1).Value != null)
                        {
                            throw new Exception("Error en línea " + numfila + ": El registro posee número de identificación pero no tipo de identificación.");
                        }
                        
                        //Obtenemos si existe o no el cliente, ya sea como cliente natural, jurídico o prospecto
                        Guid id = Guid.Empty;
                        registro.IdRegistroCRM = Guid.Empty;
                        if (!string.IsNullOrEmpty(registro.Identificacion))
                        {
                            //Para cliente jurídico buscamos también por nombre
                            if (datos.GetCell(2) != null && datos.GetCell(2).Value != null)
                                registro.Nombre = datos.GetCell(2).Value.ToString();
                            registro.IdRegistroCRM = BuscarEnClientesJuridicos(registro);
                        }

                        //Llenamos los demás datos si el cliente no existe
                        if (registro.IdRegistroCRM == Guid.Empty)
                        {
                            int valor = 0;
                            //Obtenemos primero el tipo de cliente
                            if (datos.GetCell(5) == null || datos.GetCell(5).Value == null)
                            {
                                throw new Exception("Error en línea " + numfila + ": No existen datos en la columna TIPO_CLIENTE.");
                            }
                            else
                            {
                                if (Int32.TryParse(datos.GetCell(5).Value.ToString(), out valor))
                                {
                                    registro.TipoCliente = valor;
                                }
                                else
                                {
                                    throw new Exception("Error en línea " + numfila + ": El dato en la columna TIPO_CLIENTE posee un formato incorrecto.");
                                }
                            }

                            ////EL nombre
                            if (datos.GetCell(2) == null || datos.GetCell(2).Value == null)
                            {
                                throw new Exception("Error en línea " + numfila + ": Cliente no existente. El registro debe poseer un valor en NOMBRE.");
                            }
                            else
                            {
                                registro.Nombre = datos.GetCell(2).Value.ToString();
                            }

                            if (registro.TipoCliente == DatoListaMarketing.TIPO_CLIENTE_NATURAL)
                            {

                                //Primer Apellido
                                if (datos.GetCell(3) != null && datos.GetCell(3).Value != null)
                                {
                                    registro.PrimerApellido = datos.GetCell(3).Value.ToString();
                                }
                                else
                                {
                                    throw new Exception("Error en línea " + numfila + ": El tipo de cliente es NATURAL y no posee un valor en la columna PRIMER APELLIDO.");
                                }

                                //Segundo Apellido
                                if (datos.GetCell(4) != null && datos.GetCell(4).Value != null)
                                {
                                    registro.SegundoApellido = datos.GetCell(4).Value.ToString();
                                }
                            }

                            //Telefono principal
                            if (datos.GetCell(6) != null && datos.GetCell(6).Value != null)
                            {
                                registro.TelefonoPrincipal=datos.GetCell(6).Value.ToString();
                            }
                            //Teléfono Trabajo
                            if (datos.GetCell(7) != null && datos.GetCell(7).Value != null)
                            {
                                registro.TelefonoTrabajo = datos.GetCell(7).Value.ToString();
                            }
                            //Correo electrónico principal
                            if (datos.GetCell(8) != null && datos.GetCell(8).Value != null)
                            {
                                registro.CorreoElectronicoPrincipal = datos.GetCell(8).Value.ToString();
                            }
                            //Correo electrónico trabajo
                            if (datos.GetCell(9) != null && datos.GetCell(9).Value != null)
                            {
                                registro.CorreoElectronicoTrabajo = datos.GetCell(9).Value.ToString();
                            }
                            //Ciudad
                            if (datos.GetCell(10) != null && datos.GetCell(10).Value != null)
                            {
                                registro.Ciudad = datos.GetCell(10).Value.ToString();
                            }
                            //El Ejecutivo
                            if (datos.GetCell(11) == null || datos.GetCell(11).Value == null)
                            {
                                throw new Exception("Error en línea " + numfila + ": Cliente no existente. El registro debe poseer un valor en EJECUTIVO.");
                            }
                            else
                            {
                                registro.Ejecutivo = datos.GetCell(11).Value.ToString();
                            }
                        }
                        lista.Add(registro);
                    }
                    catch (Exception ex)
                    {
                        Exception ex2 = new Exception("Error en lectura de archivo, línea " + numfila + ": " + ex.Message, ex);
                        CuentaErrores++;
                        LogError(ex2, null);
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return lista;
        }

        private void ValidarIdentificacion(int TipoIdentificaicon, string Identificacion)
        {
            if (TipoIdentificaicon == 3)
            {
                return; //No validamos los pasaportes
            }

            if (TipoIdentificaicon == 1 && Identificacion.Length != 10)
            {
                throw new Exception("La identificación debe poseer 10 caracteres.");
            }
            if (TipoIdentificaicon == 2 && Identificacion.Length != 13)
            {
                throw new Exception("La identificación debe poseer 13 caracteres.");
            }
            //Validamos con el algoritmo
            int numero;
            foreach (char c in Identificacion)
            {
                if (!Int32.TryParse(c.ToString(),out numero))
                {
                    throw new Exception("La identificación debe poseer únicamente números.");
                }
            }
            //Preguntamos por los 2 primeros dígitos
            int valorProvincia = Int32.Parse(Identificacion.Substring(0, 2));
            if (valorProvincia < 1 || valorProvincia > 24)
            {
                throw new Exception("Número de identificación inválido.");
            }
            //Preguntamos por el tercer dígito
            int tercerDigito = Int32.Parse(Identificacion.Substring(2, 1));
            string coeficientes = "";
            int modulo;
            int posicion;
            if (tercerDigito != 6 && tercerDigito != 8 && tercerDigito != 9)
            {
                //Es una cedula, por lo tanto se aplica base 10
                coeficientes = "212121212";
                posicion = 9;
                modulo = 10;
            }
            else
            {
                modulo = 11;
                if (tercerDigito == 9)
                {
                    coeficientes = "432765432";
                    posicion = 9;
                }
                else
                {
                    coeficientes = "32765432";
                    posicion = 8;
                }
            }

            if (modulo == 10)
            {
                int suma = 0;
                int valor = 0;
                for (int i = 0; i < posicion; i++)
                {
                    valor = Int32.Parse(Identificacion.Substring(i, 1)) * Int32.Parse(coeficientes.Substring(i, 1));
                    if (valor > 9)
                        valor = valor - 9;
                    suma += valor;
                }
                int residuo = suma % modulo;
                int resultado = modulo - residuo;
                if (resultado == modulo)
                    resultado = 0;
                //validamos con el digito verificador
                if (resultado != Int32.Parse(Identificacion.Substring(posicion, 1)))
                {
                    throw new Exception("Número de identificación inválido.");
                }
            }
            else
            {
                int suma = 0;
                int valor = 0;
                for (int i = 0; i < posicion; i++)
                {
                    valor = Int32.Parse(Identificacion.Substring(i, 1)) * Int32.Parse(coeficientes.Substring(i, 1));
                    suma += valor;
                }
                int residuo = suma % modulo;
                int resultado = modulo - residuo;
                if (resultado == modulo)
                    resultado = 0;
                //validamos con el digito verificador
                if (resultado != Int32.Parse(Identificacion.Substring(posicion, 1)))
                {
                    throw new Exception("Número de identificación inválido.");
                }
            }
        }

        private void AnalizarDatosImportados(List<DatoListaMarketing> data, Entity ImportacionListaMarketing)
        {
            Guid idListaMarketingProspecto = Guid.Empty;
            Guid idListaMarketingNatural = Guid.Empty;
            Guid idListaMarketingJuridico = Guid.Empty;
            foreach (DatoListaMarketing item in data)
            {
                try
                {
                    
                    if (idListaMarketingJuridico == Guid.Empty)
                        idListaMarketingJuridico = CrearListaJuridicos(ImportacionListaMarketing);
                    if (item.IdRegistroCRM == Guid.Empty)
                        item.IdRegistroCRM=GuardarNuevoProspecto(item);
                    else
                        CuentaClienteJuridico++;
                    AgregarIntegranteALista(idListaMarketingJuridico, item.IdRegistroCRM);
                }
                catch (Exception ex)
                {
                    CuentaErrores++;
                    LogError(ex, item);
                }
            }
        }

        private Guid BuscarEnClientesJuridicos(DatoListaMarketing dato)
        {
            if (!string.IsNullOrEmpty(dato.Identificacion))
            {
                QueryExpression query = new QueryExpression();
                query.Distinct = true;
                query.EntityName = "account";
                query.NoLock = true;
                query.Criteria.AddCondition(new ConditionExpression("accountnumber", ConditionOperator.Equal, dato.Identificacion));
                query.Criteria.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, 0));
                EntityCollection resultado = ServicioCRM.RetrieveMultiple(query);

                if (resultado.Entities.Count > 0)
                {
                    return (Guid)resultado.Entities[0].Attributes["accountid"];
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(dato.Nombre))
                {
                    //buscamos por nombre
                    QueryExpression query = new QueryExpression();
                    query.Distinct = true;
                    query.EntityName = "account";
                    query.NoLock = true;
                    query.Criteria.AddCondition(new ConditionExpression("name", ConditionOperator.Equal, dato.Nombre));
                    query.Criteria.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, 0));
                    EntityCollection resultado = ServicioCRM.RetrieveMultiple(query);
                    if (resultado.Entities.Count > 0)
                    {
                        return (Guid)resultado.Entities[0].Attributes["accountid"];
                    }
                }
            }
            return Guid.Empty;
        }

        private Guid CrearListaJuridicos(Entity ImportacionListaMarketing)
        {
            Entity ListaMktClientesJuridicos;
            ListaMktClientesJuridicos = new Entity("list");
            ListaMktClientesJuridicos["listname"] = string.Format("{0}", ImportacionListaMarketing["efk_nombre"].ToString());
            ListaMktClientesJuridicos["efk_importacion_lista_marketingid"] = ImportacionListaMarketing["efk_importacion_listas_marketingid"].ToString();
            ListaMktClientesJuridicos["ownerid"] = ImportacionListaMarketing["ownerid"];
            ListaMktClientesJuridicos["efk_aprobada"] = new OptionSetValue(100000001);
            ListaMktClientesJuridicos["createdfromcode"] = new OptionSetValue(1);
            ListaMktClientesJuridicos["efk_importacion_lista_marketingid"] = new EntityReference("efk_importacion_listas_marketing", 
                (Guid)ImportacionListaMarketing["efk_importacion_listas_marketingid"]);
            return ServicioCRM.Create(ListaMktClientesJuridicos);
        }

        private Guid GuardarNuevoProspecto(DatoListaMarketing datoProspecto)
        {
            //Validamos que exista el usuario al cual se asignará el prospecto
            string usuarioId = ObtenerUsuarioPorNombre(datoProspecto.Ejecutivo);
            if (string.IsNullOrEmpty(usuarioId))
            {
                throw new Exception("Error en línea " + datoProspecto.numeroFila + ": No existe el usuario con login " + datoProspecto.Ejecutivo);
            }

            Entity prospecto = new Entity("account");
            if (datoProspecto.TipoIdentificacion != null)
                prospecto["efk_tipo_identificacion"] = new OptionSetValue(datoProspecto.TipoIdentificacion.Value);
            if (!string.IsNullOrWhiteSpace(datoProspecto.Identificacion))
                prospecto["accountnumber"] = datoProspecto.Identificacion;
            if (datoProspecto.TipoCliente == DatoListaMarketing.TIPO_CLIENTE_JURIDICO)
            {
                prospecto["efk_tipo_cliente"] = new OptionSetValue(DatoListaMarketing.OPCION_CLIENTE_JURIDICO);
                if (!string.IsNullOrWhiteSpace(datoProspecto.Nombre))
                    prospecto["name"] = datoProspecto.Nombre;
            }
            else if (datoProspecto.TipoCliente == DatoListaMarketing.TIPO_CLIENTE_NATURAL)
            {
                prospecto["efk_tipo_cliente"] = new OptionSetValue(DatoListaMarketing.OPCION_CLIENTE_NATURAL);
                string primerApellido = "";
                string segundoApellido = "";
                string nombre = "";
                if (!string.IsNullOrWhiteSpace(datoProspecto.PrimerApellido))
                {
                    prospecto["efk_primerapellido"] = datoProspecto.PrimerApellido;
                    primerApellido = datoProspecto.PrimerApellido;
                }
                if (!string.IsNullOrWhiteSpace(datoProspecto.PrimerApellido))
                {
                    prospecto["efk_segundoapellido"] = datoProspecto.SegundoApellido;
                    segundoApellido=datoProspecto.SegundoApellido;
                }
                if (!string.IsNullOrWhiteSpace(datoProspecto.Nombre))
                {
                    prospecto["efk_nombre_persona"] = datoProspecto.Nombre;
                    nombre = datoProspecto.Nombre;
                }

                string nombreCompleto = "";

                if (primerApellido != "")
                    nombreCompleto += primerApellido;
                if (segundoApellido != "")
                    nombreCompleto += " " + segundoApellido;
                if (nombre != "")
                {
                    if (nombreCompleto != "")
                        nombreCompleto += ", " + nombre;
                    else
                        nombreCompleto = nombre;
                }

                if (nombreCompleto != "")
                    prospecto["name"]= nombreCompleto;
            }

           if (!string.IsNullOrWhiteSpace(datoProspecto.TelefonoPrincipal))
                prospecto["telephone1"] = datoProspecto.TelefonoPrincipal;
           if (!string.IsNullOrWhiteSpace(datoProspecto.TelefonoTrabajo))
               prospecto["telephone3"] = datoProspecto.TelefonoTrabajo;
           if (!string.IsNullOrWhiteSpace(datoProspecto.CorreoElectronicoPrincipal))
               prospecto["emailaddress1"] = datoProspecto.CorreoElectronicoPrincipal;
           if (!string.IsNullOrWhiteSpace(datoProspecto.CorreoElectronicoTrabajo))
               prospecto["emailaddress3"] = datoProspecto.CorreoElectronicoTrabajo;
           if (!string.IsNullOrWhiteSpace(datoProspecto.Ciudad))
               prospecto["efk_ciudad"] = datoProspecto.Ciudad;

            if (!string.IsNullOrEmpty(usuarioId))
                prospecto["ownerid"] = new EntityReference("systemuser", new Guid(usuarioId));

            prospecto["efk_cliente_mis"] = false;

            Guid idProspecto = ServicioCRM.Create(prospecto);

            CuentaProspectoNuevo++;

            return idProspecto;
        }

        private void AgregarIntegranteALista(Guid idLista, Guid idIntegrante)
        {
            AddMemberListRequest req = new AddMemberListRequest();
            req.EntityId = idIntegrante;
            req.ListId = idLista;

            ServicioCRM.Execute(req);
        }

        private string ObtenerUsuarioPorNombre(string NombreUsuario)
        {
            QueryExpression query = new QueryExpression();
            query.Distinct = true;
            query.EntityName = "systemuser";
            query.NoLock = true;
            query.Criteria.AddCondition(new ConditionExpression("domainname", ConditionOperator.EndsWith, "\\" + NombreUsuario));
            EntityCollection resultado = ServicioCRM.RetrieveMultiple(query);
            return resultado.Entities.Count == 0 ? string.Empty : resultado.Entities[0].Id.ToString();
        }

        private string ObtenerNombreDominio()
        {
            return WindowsIdentity.GetCurrent().Name.Split('\\')[0].ToUpper();
        }

        private void ActualizarImportacionListaMarketing(Entity ImportacionListaMarketing)
        {
            ImportacionListaMarketing["efk_estado_carga"] = new OptionSetValue(100000001);
            ImportacionListaMarketing["efk_fecha_fin_ejecucion"] = DateTime.Now;
            ImportacionListaMarketing["efk_cant_registros_procesados"] = CuentaClienteJuridico + CuentaClienteNatural + CuentaErrores + CuentaProspectoExistente + CuentaProspectoNuevo;
            ImportacionListaMarketing["efk_cant_registros_cargados"] = CuentaClienteJuridico + CuentaClienteNatural + CuentaProspectoExistente + CuentaProspectoNuevo;
            ImportacionListaMarketing["efk_cant_clientes_juridicos_cargados"] = CuentaClienteJuridico;
            ImportacionListaMarketing["efk_cant_clientes_naturales_cargados"] = CuentaClienteNatural;
            ImportacionListaMarketing["efk_cant_errores"] = CuentaErrores;
            ImportacionListaMarketing["efk_cant_prospectos_existentes_cargados"] = CuentaProspectoExistente;
            ImportacionListaMarketing["efk_cant_prospectos_nuevos_cargados"] = CuentaProspectoNuevo;
            ImportacionListaMarketing["efk_detalle_errores"] = string.Format("Se encontraron {0} errores al procesar el archivo enviado. Para mayor detalle, revise el log adjunto.", CuentaErrores);
            ServicioCRM.Update(ImportacionListaMarketing);
        }

        private void LogError(Exception ex, DatoListaMarketing dato)
        {
            if (null == Errores) Errores = new StringBuilder();
            Errores.Append("-------------------------------------------");
            Errores.AppendFormat("Error {0} : \r\n Detalle del Error: {1} \r\n Información adicional : {2} \r\n Objeto procesado : {3} \r\n", ex.GetType(), ex.Message,
                string.IsNullOrEmpty(ex.StackTrace) ? "[Información no disponible]" : ex.StackTrace, dato == null ? "[Información no disponible]" : dato.ToString());
            Errores.Append("-------------------------------------------");
        }

        private void LogInformacion(string informacion)
        {
            if (null == Errores) Errores = new StringBuilder();
            Errores.AppendLine("-------------------------------------------");
            Errores.AppendLine("Información : "+informacion);
            Errores.AppendLine("-------------------------------------------");
        }

        private void GuardarLog()
        {
            if (string.IsNullOrEmpty(CarpetaLog))
            {
                CarpetaLog = @"C:\CRM\CRM_Archivos_Importacion\Log_Campania_ListaMarketing";
            }
            RutaArchivoLog = Path.Combine(CarpetaLog, GenerarCadenaAleatoria(10, true));
            File.WriteAllText(RutaArchivoLog, Errores.ToString());
        }

        private void EnviarCorreoElectronico(Entity ImportacionListaMarketing)
        {
            Errores.AppendLine("-------------------------------------------");
            Errores.AppendLine("-- Proceso de importación concluido. ");
            Entity correo = new Entity("email");
            Entity adjunto = new Entity("activitymimeattachment");
            ColumnSet columnas = new ColumnSet();
            columnas.AllColumns = true;
            Errores.AppendLine("-- Creando Correo Electrónico. ");

            Errores.AppendFormat("id de registro de importación: {0}\r\n", ImportacionListaMarketing.Id);
            Entity remitente = new Entity("activityparty");

            //Obtenemos el usuario dueño de la importación
            ColumnSet cols = new ColumnSet("ownerid");
            Entity registroImportacion = ServicioCRM.Retrieve("efk_importacion_listas_marketing", IdLista, cols);
            remitente.Attributes.Add("partyid", new EntityReference("systemuser", ((EntityReference)registroImportacion["ownerid"]).Id));

            correo["from"] = new EntityCollection(new List<Entity> { remitente });
            correo["to"] = new EntityCollection(new List<Entity> { remitente });
            correo["subject"] = string.Format("MS CRM: Se ha concluido la importación de la lista de Marketing '{0}'", ImportacionListaMarketing["efk_nombre"]);
            correo["description"] = string.Format(@"Estimado usuario,<br /><br /> Se ha concluido la importación de lista de Marketing '{0}'.<br />" +
                "Para mayores detalles revise el archivo adjunto y el correspondiente registro de importación en MS CRM. <br /><br /><br />" + 
                "Saludos,<br /><br />Microsoft Dynamics CRM 2011", ImportacionListaMarketing["efk_nombre"]);
            correo["directioncode"] = true;
            Errores.AppendLine("Obteniendo el archivo de Log ");

            Errores.AppendLine("Creando Correo");

            Guid idCorreo = ServicioCRM.Create(correo);

            Errores.AppendFormat("IdCorreo: {0}.", idCorreo);
            Errores.AppendLine("Configurando adjunto. ");
            Errores.AppendLine("Adjunto listo.");
            Errores.AppendLine("-- Proceso de correo terminado.");
           
            byte[] fileContent = UTF8Encoding.UTF8.GetBytes(Errores.ToString());
            string datosCodificados = Convert.ToBase64String(fileContent);

            ///* Creando el archivo adjunto */
            adjunto["objecttypecode"] = correo.LogicalName;
            adjunto["filename"] = "Log importación de listas - " + ImportacionListaMarketing["efk_nombre"] + ".txt";
            adjunto["body"] = datosCodificados;
            adjunto["mimetype"] = @"text/plain";
            adjunto["objectid"] = new EntityReference("email", idCorreo);

            ServicioCRM.Create(adjunto);

            //Adjunto para el registro de importación
            Entity adjunto2 = new Entity("annotation");
            adjunto2["subject"] = "Log de carga";
            adjunto2["filename"] = "Log importación de listas - " + ImportacionListaMarketing["efk_nombre"] + ".txt";
            adjunto2["documentbody"] = datosCodificados;
            adjunto2["mimetype"] = @"text/plain";
            adjunto2["objectid"] = new EntityReference("efk_importacion_listas_marketing", (Guid)ImportacionListaMarketing["efk_importacion_listas_marketingid"]);

            ServicioCRM.Create(adjunto2);

            SendEmailRequest req = new SendEmailRequest();
            req.EmailId = idCorreo;
            req.TrackingToken = "";
            ServicioCRM.Execute(req);
        }

        public static string GenerarCadenaAleatoria(int largoCadena, bool enMinuscula)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Log-");
            Random random = new Random();
            char ch;
            for (int i = 0; i < largoCadena; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            builder.Append(".txt");
            if (enMinuscula)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        public void EliminarArchivos()
        {
            try
            {
                File.Delete(RutaArchivoLog);
                File.Delete(ImportacionListaMarketing["efk_ruta_archivo_carga"].ToString());
            }
            catch { }
        }

        public Entity ObtenerMonedaDolar()
        {
            QueryExpression query = new QueryExpression();
            query.Distinct = true;
            query.EntityName = "transactioncurrency";
            query.NoLock = true;
            query.Criteria.AddCondition(new ConditionExpression("isocurrencycode", ConditionOperator.Equal, "USD"));
            EntityCollection resultado = ServicioCRM.RetrieveMultiple(query);
            if (resultado.Entities.Count > 0)
            {
                return resultado.Entities[0];
            }
            return null;
        }

        public void ObtenerCiudades()
        {
            RetrieveOptionSetRequest request = new RetrieveOptionSetRequest { Name = "efk_ciudad" };
            RetrieveOptionSetResponse respuesta = (RetrieveOptionSetResponse)ServicioCRM.Execute(request);
            Ciudades = new List<OptionMetadata>(((OptionSetMetadata)respuesta.OptionSetMetadata).Options.ToArray());
        }


        public OptionSetValue BuscarCiudad(string nombre)
        {
            foreach (var ciudad in Ciudades)
            {
                if (ciudad.Label.UserLocalizedLabel.Label == nombre)
                {
                    return new OptionSetValue(ciudad.Value.Value);
                }
            }
            return null;
        }
    }
}
