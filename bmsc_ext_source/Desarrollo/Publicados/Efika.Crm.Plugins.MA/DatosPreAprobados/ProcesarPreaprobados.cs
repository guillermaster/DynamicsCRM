using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Client;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Activities;
using Microsoft.Xrm.Sdk.Query;
using ex = Net.SourceForge.Koogra.Excel;
using ex2 = Net.SourceForge.Koogra.Excel2007;
using Net.SourceForge.Koogra;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Configuration;


namespace Efika.Crm.Plugins.MA.DatosPreAprobados
{
    class ProcesarPreaprobados
    {
        #region Propiedades

        private int CuentaErrores { get; set; }
        private int CuentaCargados { get; set; }
        private StringBuilder Errores { get; set; }
        IOrganizationService ServicioCRM { get; set; }
        private string RutaArchivoLog { get; set; }
        private Guid IdLista { get; set; }

        public Entity ListaPreaprobados { get; set; }

        ITracingService tracingService = null;
        IWorkflowContext contexto = null;

        public string CarpetaLog { get; set; } 

        #endregion

        public ProcesarPreaprobados(IOrganizationService ServicioCRM,
            ITracingService tracingService, IWorkflowContext contexto, Guid idLista)
        {
            this.ServicioCRM = ServicioCRM;
            this.tracingService = tracingService;
            this.contexto = contexto;
            this.IdLista = idLista;
        }


        #region Eventos

        public void EjecutarImportacion()
        {
            try
            {
                Errores = new StringBuilder();
                tracingService.Trace("Creando enlace a servicio CRM.");
                tracingService.Trace("Enlace a servicio creado.");
                tracingService.Trace("Obteniendo parámetro.");
                tracingService.Trace("Id Importación actual : " + IdLista);
                ColumnSet columnas = new ColumnSet();
                columnas.AllColumns = true;
                ListaPreaprobados = ServicioCRM.Retrieve("efk_importacion_datos_preaprobados", IdLista, columnas);

                tracingService.Trace("Campaña actual : " + ((EntityReference)ListaPreaprobados["efk_campaignid"]).Id);
                tracingService.Trace("Procesando datos del archivo Excel.");
                List<DatoPreAprobado> datosLista = ProcesarArchivoExcel(ListaPreaprobados["efk_ruta_archivo_carga"].ToString());
                tracingService.Trace("Eliminando datos anteriores para esta campaña.");
                EliminarDatosAnterioresCampania();
                tracingService.Trace("Analizando los datos obtenido del archivo.");
                ProcesarDatosArchivo(datosLista);
                tracingService.Trace("Actualizando registro de importación de datos pre aprobados.");
                ActualizarImportacionPreaprobados();

                tracingService.Trace("Enviando correo electrónico.");
                EnviarCorreoElectronico(ListaPreaprobados);
                tracingService.Trace("Proceso finalizado correctamente.");
            }
            catch (Exception ex)
            {
                LogError(ex, null);
                tracingService.Trace(Errores.ToString());
                throw ex;
            }
            

        }

        #endregion

        #region Métodos

        private List<DatoPreAprobado> ProcesarArchivoExcel(string IdNota)
        {
            List<DatoPreAprobado> lista = new List<DatoPreAprobado>();
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
                    try
                    {
                        numfila = fila;
                        IRow datos = hoja.Rows.GetRow(fila);

                        if (datos.IsEmpty())
                            continue;

                        DatoPreAprobado registro = new DatoPreAprobado();
                        registro.NumeroFila = (int)numfila;

                        //Identificacion
                        if (datos.GetCell(0) == null || datos.GetCell(0).Value == null)
                        {
                            throw new Exception("Error en línea " + numfila + ": El registro debe poseer un valor en la columna IDENTIFCACION.");
                        }
                        registro.Identificacion = datos.GetCell(0).Value.ToString();
                        //Código producto
                        if (datos.GetCell(1) == null || datos.GetCell(1).Value == null)
                        {
                            throw new Exception("Error en línea " + numfila + ": El registro debe poseer un valor en la columna CODIGO_PRODUCTO.");
                        }
                        registro.CodigoProducto = datos.GetCell(1).Value.ToString();
                        //Nombre producto
                        if (!(datos.GetCell(2) == null || datos.GetCell(2).Value == null))
                        {
                            registro.NombreProducto = datos.GetCell(2).Value.ToString();
                        }

                        //antes de continuar validamos si existen datos del producto, caso contrario no se cargaria el registro
                        bool existeDato=false;
                        for(uint i=3;i<=12;i++){
                            if (datos.GetCell(i) != null && datos.GetCell(i).Value != null)
                                existeDato = true;
                        }
                        if (!existeDato)
                        {
                            throw new Exception("Error en línea " + numfila + ": El registro no posee datos del producto pre-aprobado.");
                        }
                        
                        //Cumpo_Monto
                         decimal d;
                        if (datos.GetCell(3) != null && datos.GetCell(3).Value != null)
                        {
                            if (!Decimal.TryParse(datos.GetCell(3).Value.ToString(), out d))
                            {
                                throw new Exception("Error en línea " + numfila + ": El registro no posee formato numérico en la columna CUPO_MONTO.");
                            }
                            if (d < 0)
                            {
                                throw new Exception("Error en línea " + numfila + ": El valor en CUPO_MONTO no debe ser negativo.");
                            }
                            registro.CupoMonto = d;
                        }

                        //Tasa
                        if (datos.GetCell(4) != null && datos.GetCell(4).Value != null){
                            if (!Decimal.TryParse(datos.GetCell(4).Value.ToString(), out d))
                            {
                                throw new Exception("Error en línea " + numfila + ": El registro no posee formato numérico en la columna TASA.");
                            }
                            else
                            {
                                registro.Tasa = d;
                            }
                        }

                        //Plazo
                        int plazo = 0;
                        if (datos.GetCell(5) != null && datos.GetCell(5).Value != null)
                        {
                            if (!Int32.TryParse(datos.GetCell(5).Value.ToString(), out plazo))
                            {
                                throw new Exception("Error en línea " + numfila + ": El registro no posee formato numérico en la columna PLAZO_DIAS.");
                            }
                            else
                            {
                                if (plazo < 0)
                                {
                                    throw new Exception("Error en línea " + numfila + ": El registro no puede poseer valores negativos en la columna PLAZO_DIAS.");
                                }
                                registro.Plazo = plazo;
                            }
                        }

                        //Comision
                        if (datos.GetCell(6) != null && datos.GetCell(6).Value != null)
                        {
                            if (!Decimal.TryParse(datos.GetCell(6).Value.ToString(), out d))
                            {
                                throw new Exception("Error en línea " + numfila + ": El registro no posee formato numérico en la columna COMISIÓN.");
                            }
                            else
                            {
                                registro.Comision = d;
                            }
                        }

                        //Atrubuto 1
                        if (datos.GetCell(7) != null && datos.GetCell(7).Value != null)
                        {
                            if (datos.GetCell(7).ToString().Length > 100)
                            {
                                throw new Exception("Error en línea " + numfila + ": El valor de la columna ATRIBUTO 1 posee mas de 100 caracteres.");
                            }
                            registro.Atributo_1 = datos.GetCell(7).Value.ToString();
                        }

                        //Valor 1
                        if (datos.GetCell(8) != null && datos.GetCell(8).Value != null)
                        {
                            if (datos.GetCell(8).ToString().Length > 100)
                            {
                                throw new Exception("Error en línea " + numfila + ": El valor de la columna VALOR 1 posee mas de 100 caracteres.");
                            }
                            registro.Valor_1 = datos.GetCell(8).Value.ToString();
                        }

                        //Atrubuto 2
                        if (datos.GetCell(9) != null && datos.GetCell(9).Value != null)
                        {
                            if (datos.GetCell(9).ToString().Length > 100)
                            {
                                throw new Exception("Error en línea " + numfila + ": El valor de la columna ATRIBUTO 2 posee mas de 100 caracteres.");
                            }
                            registro.Atributo_2 = datos.GetCell(9).Value.ToString();
                        }

                        //Valor 2
                        if (datos.GetCell(10) != null && datos.GetCell(10).Value != null)
                        {
                            if (datos.GetCell(10).ToString().Length > 100)
                            {
                                throw new Exception("Error en línea " + numfila + ": El valor de la columna VALOR 2 posee mas de 100 caracteres.");
                            }
                            registro.Valor_2 = datos.GetCell(10).Value.ToString();
                        }

                        //Atrubuto 3
                        if (datos.GetCell(11) != null && datos.GetCell(11).Value != null)
                        {
                            if (datos.GetCell(11).ToString().Length > 100)
                            {
                                throw new Exception("Error en línea " + numfila + ": El valor de la columna ATRIBUTO 3 posee mas de 100 caracteres.");
                            }
                            registro.Atributo_3 = datos.GetCell(11).Value.ToString();
                        }

                        //Valor 3
                        if (datos.GetCell(12) != null && datos.GetCell(12).Value != null)
                        {
                            if (datos.GetCell(12).ToString().Length > 100)
                            {
                                throw new Exception("Error en línea " + numfila + ": El valor de la columna VALOR 3 posee mas de 100 caracteres.");
                            }
                            registro.Valor_3 = datos.GetCell(12).Value.ToString();
                        }


                        //Validamos si ya existe un registro anterior del mismo cliente con el mismo producto
                        for(int i=0;i<lista.Count;i++)
                        {
                            DatoPreAprobado dp = lista[i];
                            if (dp.Identificacion == registro.Identificacion &&
                                dp.CodigoProducto == registro.CodigoProducto)
                            {
                                throw new Exception("Error en línea " + numfila + ": Ya se ha registrado anteriormente el mismo producto con el mismo cliente en el archivo de carga.");
                            }
                        }

                        lista.Add(registro);
                    }
                    catch (Exception ex)
                    {
                        LogError(ex, null);
                        CuentaErrores++;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lista;
        }

        private void ProcesarDatosArchivo(List<DatoPreAprobado> datos)
        {
            foreach (DatoPreAprobado item in datos)
            {
                try
                {
                    if (!ValidarProducto(item)) throw new ApplicationException("El producto especificado no existe.");
                    if (BuscarEnClientesJuridicos(item)) { GuardarDatoPreaprobado(item); continue; }
                    //Si llega hasta aqui es porque es un error
                    throw new ApplicationException("Error en línea " + item.NumeroFila + ": La identificación del cliente no existe en CRM.");
                }
                catch (Exception ex)
                {
                    LogError(ex, item);
                    CuentaErrores++;
                }
            }
        }


        private bool BuscarEnClientesNaturales(DatoPreAprobado dato)
        {
            bool res = false;
            QueryExpression query = new QueryExpression();
            query.Distinct = true;
            query.EntityName = "contact";
            query.NoLock = true;
            query.Criteria.AddCondition(new ConditionExpression("efk_numero_identificacion", ConditionOperator.Equal, dato.Identificacion));
            query.Criteria.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, 0));
            EntityCollection resultado = ServicioCRM.RetrieveMultiple(query);
            if (resultado.Entities.Count > 0)
            {
                dato.IdCliente = resultado.Entities[0].Id;
                dato.TipoCliente = 1;
                res = true;
            }
            return res;
        }

        private bool BuscarEnClientesJuridicos(DatoPreAprobado dato)
        {
            bool res = false;
            QueryExpression query = new QueryExpression();
            query.Distinct = true;
            query.EntityName = "account";
            query.NoLock = true;
            query.Criteria.AddCondition(new ConditionExpression("accountnumber", ConditionOperator.Equal, dato.Identificacion));
            query.Criteria.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, 0));
            EntityCollection resultado = ServicioCRM.RetrieveMultiple(query);
            if (resultado.Entities.Count > 0)
            {
                dato.IdCliente = resultado.Entities[0].Id;
                dato.TipoCliente = 2;
                res = true;
            }
            return res;
        }

        private bool BuscarEnProspectos(DatoPreAprobado dato)
        {
            bool res = false;
            QueryExpression query = new QueryExpression();
            query.Distinct = true;
            query.EntityName = "lead";
            query.NoLock = true;
            query.Criteria.AddCondition(new ConditionExpression("efk_numero_identificacion", ConditionOperator.Equal, dato.Identificacion));
            query.Criteria.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, 0));
            EntityCollection resultado = ServicioCRM.RetrieveMultiple(query);
            if (resultado.Entities.Count > 0)
            {
                dato.IdCliente = resultado.Entities[0].Id;
                dato.TipoCliente = 3;
                res = true;
            }
            return res;
        }

        private bool ValidarProducto(DatoPreAprobado dato)
        {
            QueryExpression query = new QueryExpression();
            query.Distinct = true;
            query.EntityName = "product";
            query.NoLock = true;
            query.Criteria.AddCondition(new ConditionExpression("productnumber", ConditionOperator.Equal, dato.CodigoProducto));
            EntityCollection resultado = ServicioCRM.RetrieveMultiple(query);
            if (resultado.Entities.Count > 0) { dato.Producto = resultado.Entities[0]; }
            return resultado.Entities.Count > 0;
        }

        private void GuardarDatoPreaprobado(DatoPreAprobado dato)
        {
            Entity datoPreAprobado = new Entity("efk_datos_importados_campana");
            datoPreAprobado["efk_campana_id"] = new EntityReference("campaign", ((EntityReference)ListaPreaprobados["efk_campaignid"]).Id);
            datoPreAprobado["efk_subtipo_producto_id"] = new EntityReference("product", dato.Producto.Id);
            datoPreAprobado["efk_importacion_datos_preaprobadosid"] = new EntityReference("efk_importacion_datos_preaprobados", ListaPreaprobados.Id);
            if (dato.CupoMonto != null)
                datoPreAprobado["efk_cupo_monto"] = new Money(dato.CupoMonto.Value);
            datoPreAprobado["efk_numero_identificacion"] = dato.Identificacion;
            if(dato.Tasa!=null)
                datoPreAprobado["efk_tasa"] = dato.Tasa.Value;
            if(dato.Plazo!=null)
                datoPreAprobado["efk_plazo"] = dato.Plazo.Value;
            if(dato.Comision!=null)
                datoPreAprobado["efk_comision"] = dato.Comision.Value;
            datoPreAprobado["efk_atributo_1"] = dato.Atributo_1;
            datoPreAprobado["efk_valor_1"] = dato.Valor_1;
            datoPreAprobado["efk_atributo_2"] = dato.Atributo_2;
            datoPreAprobado["efk_valor_2"] = dato.Valor_2;
            datoPreAprobado["efk_atributo_3"] = dato.Atributo_3;
            datoPreAprobado["efk_valor_3"] = dato.Valor_3;

            if (dato.TipoCliente == 1)
            {
                datoPreAprobado["efk_cliente_natural_id"] = new EntityReference("contact", dato.IdCliente);
            }
            if (dato.TipoCliente == 2)
            {
                datoPreAprobado["efk_cliente_juridico_id"] = new EntityReference("account", dato.IdCliente);
            }
            if (dato.TipoCliente == 3)
            {
                datoPreAprobado["efk_prospecto_id"] = new EntityReference("lead", dato.IdCliente);
            }
            ServicioCRM.Create(datoPreAprobado);
            CuentaCargados++;
        }


        private void LogError(Exception ex, DatoPreAprobado dato)
        {
            if (null == Errores) Errores = new StringBuilder();
            Errores.Append("-------\r\n");
            Errores.AppendFormat("Error {0} : \r\n Detalle del Error: {1} \r\n Información adicional : {2} \r\n Objeto procesado : {3} \r\n",
                ex.GetType(), ex.Message, ex.StackTrace, dato == null ? string.Empty : dato.ToString());
            Errores.Append("-------\r\n");
        }

        private void EliminarDatosAnterioresCampania()
        {
            QueryExpression query = new QueryExpression();
            query.Distinct = true;
            query.EntityName = "efk_datos_importados_campana";
            query.NoLock = true;
            query.Criteria.AddCondition(new ConditionExpression("efk_campana_id", ConditionOperator.Equal, ((EntityReference)ListaPreaprobados["efk_campaignid"]).Id));
            EntityCollection resultado = ServicioCRM.RetrieveMultiple(query);
            if (resultado == null || resultado.Entities.Count == 0) return;
            foreach (Entity item in resultado.Entities)
            {
                ServicioCRM.Delete("efk_datos_importados_campana", item.Id);
            }
        }

        private void ActualizarImportacionPreaprobados()
        {
            ListaPreaprobados["efk_cant_registros_procesados"] = CuentaCargados + CuentaErrores;
            ListaPreaprobados["efk_cant_registros_cargados"] = CuentaCargados;
            ListaPreaprobados["efk_cant_errores_encontrados"] = CuentaErrores;
            ListaPreaprobados["efk_estado_carga"] = new OptionSetValue(100000001);
            ListaPreaprobados["efk_fecha_fin_ejecucion"] = DateTime.Now;
            ListaPreaprobados["efk_detalle_errores"] = string.Format("Se encontraron {0} errores al procesar el archivo enviado. " + 
                "Para mayor detalle, revise el log adjunto que fue enviado por correo electrónico.", CuentaErrores);
            ServicioCRM.Update(ListaPreaprobados);
        }

        private void GuardarLog()
        {
            if (string.IsNullOrEmpty(CarpetaLog))
            {
                CarpetaLog = @"C:\CRM\CRM_Archivos_Importacion\Log_Campania_Preaprobados";
            }
            RutaArchivoLog = Path.Combine(CarpetaLog, GenerarCadenaAleatoria(10, true));
            File.WriteAllText(RutaArchivoLog, Errores.ToString());
        }

        

        private void EnviarCorreoElectronico(Entity ImportacionDatos)
        {
            Errores.AppendLine("-------------------------------------------");
            Errores.AppendLine("-- Proceso de importación concluido. ");
            Entity correo = new Entity("email");
            Entity adjunto = new Entity("activitymimeattachment");
            ColumnSet columnas = new ColumnSet();
            columnas.AllColumns = true;
            Errores.AppendLine("-- Creando Correo Electrónico. ");

            Errores.AppendFormat("id de registro de importación: {0}\r\n", ImportacionDatos.Id);
            Entity remitente = new Entity("activityparty");

            //Obtenemos el usuario dueño de la importación
            ColumnSet cols = new ColumnSet("ownerid");
            Entity registroImportacion = ServicioCRM.Retrieve("efk_importacion_datos_preaprobados", IdLista, cols);
            remitente.Attributes.Add("partyid", new EntityReference("systemuser", ((EntityReference)registroImportacion["ownerid"]).Id));

            
            correo["from"] = new EntityCollection(new List<Entity> { remitente });
            correo["to"] = new EntityCollection(new List<Entity> { remitente });
            correo["subject"] = string.Format("MS CRM: Se ha concluido la importación de la datos pre-aprobados '{0}'", ImportacionDatos["efk_name"]);
            correo["description"] = string.Format(@"Estimado usuario,<br /><br /> Se ha concluido la importación de datos pre-aprobados '{0}'.<br />  " + 
                "Para mayores detalles revise el archivo adjunto y el correspondiente registro de importación en MS CRM. <br /><br /><br />" + 
                "Saludos,<br /><br />Microsoft Dynamics CRM 2011", ImportacionDatos["efk_name"]);
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

            adjunto["objecttypecode"] = correo.LogicalName;
            adjunto["filename"] = "Log importación - " + ImportacionDatos["efk_name"] + ".txt";
            adjunto["body"] = datosCodificados;
            adjunto["mimetype"] = @"text/plain";
            adjunto["objectid"] = new EntityReference("email", idCorreo);

            ServicioCRM.Create(adjunto);

            //Adjunto para el registro de importación
            Entity adjunto2 = new Entity("annotation");
            adjunto2["subject"] = "Log de carga";
            adjunto2["filename"] = "Log importación - " + ImportacionDatos["efk_name"] + ".txt";
            adjunto2["documentbody"] = datosCodificados;
            adjunto2["mimetype"] = @"text/plain";
            adjunto2["objectid"] = new EntityReference("efk_importacion_datos_preaprobados", (Guid)ImportacionDatos["efk_importacion_datos_preaprobadosid"]);

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
                File.Delete(ListaPreaprobados["efk_ruta_archivo_carga"].ToString());
            }
            catch { }
        }

        #endregion
    }
}
