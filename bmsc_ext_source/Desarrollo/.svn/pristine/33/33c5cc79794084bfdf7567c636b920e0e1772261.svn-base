using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Configuration;
using System.Web.Services;
using Efika.Crm.Entidades;
using Efika.Crm.Negocio;
using Efika.Crm.AccesoServicios;
using Efika.Crm.AccesoServicios.CRMSDK;
using System.Data;
using Efika.Crm.Entidades.Common;
using System.Net;

namespace Efika.Crm.WebService
{
    /// <summary>
    /// Summary description for Clientes
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Clientes : System.Web.Services.WebService
    {

        [WebMethod]
        public bool RegistrarLogVisualizacionRegistros(string p_strIdCliente, string p_strIdUsuario, string p_strNombreOrganizacion,
                                                        string p_strCodigoMis, string p_strCodigoEvento)
        {
            //Creamos una instancia de la clase de log
            Log cLog = null;
            string organizacion = "";
            string dominio = "";
            string rutaArchivoCredenciales = "";
            string cPathLog = "";            
            CredencialesCRM credenciales;
            try
            {          
                organizacion=ConfigurationManager.AppSettings["CRMorganizacion"];
                dominio = Environment.UserDomainName;
                rutaArchivoCredenciales = ConfigurationManager.AppSettings["CRMRutaArchivoCredenciales"];
                cPathLog = ConfigurationManager.AppSettings["CRMRutaLogs"];

                //Obtenemos las credenciales
                credenciales = DPAPI.ObtenerUsuarioPasswordDPAPI(rutaArchivoCredenciales, organizacion, dominio);

                return LogVisualizacionRegistros.RegistrarLogVisualizacionRegistro(credenciales, p_strIdCliente, p_strIdUsuario, p_strNombreOrganizacion, p_strCodigoMis, p_strCodigoEvento);
            }
            catch (Exception ex)
            {                
                cLog = new Log(cPathLog);
                cLog.Add(ex.Message, "Cliente", "RegistrarLogVisualizacionRegistros", User.ToString(), ex.StackTrace);
                throw new Exception(ex.Message);
            }
        }

        [WebMethod]
        public bool SegmentarCliente(int codigoCliente, string accountid, decimal ingresos, decimal tipoIngreso, int edad, int numerohijos, int estadocivil, string usuariocrm, string divisaid)
        {
            //Creamos una instancia de la clase de log
            Log cLog = null;
            string cadenaconexion = "";
            string organizacion = "";
            string dominio = "";
            string rutaArchivoCredenciales = "";
            string rutaArchivoCredencialesBD = "";
            string cPathLog = "";
            string spCalculoOFV = "";
            BMSCSegmentacion.ObtenerSegmentoMService servicio = null;


            ServicePointManager.ServerCertificateValidationCallback += MyCertificatePolicy.ValidateCertificate;

            //Obtenemos las credenciales--- ESTAS SON PARA COMUNICARNOS CON CRM
            CredencialesCRM credenciales;
            CredencialesBD credencialesbd;
            try
            {          
                cadenaconexion = "Data Source={0};Initial Catalog={1};User Id={2};Password={3}";
                organizacion = ConfigurationManager.AppSettings["CRMorganizacion"];

                dominio = Environment.UserDomainName;
                rutaArchivoCredenciales = ConfigurationManager.AppSettings["CRMRutaArchivoCredenciales"];
                rutaArchivoCredencialesBD = ConfigurationManager.AppSettings["BDRutaArchivoCredenciales"];
                cPathLog = ConfigurationManager.AppSettings["CRMRutaLogs"];
                spCalculoOFV = ConfigurationManager.AppSettings["CRMNombreSProcedureOFV"];

                servicio = new BMSCSegmentacion.ObtenerSegmentoMService();

                credenciales = DPAPI.ObtenerUsuarioPasswordDPAPI(rutaArchivoCredenciales, organizacion, dominio);
                credencialesbd = DPAPI.ObtenerVariablesConexionBDDPAPI(rutaArchivoCredencialesBD);

                cadenaconexion = string.Format(cadenaconexion, credencialesbd.Servidor, credencialesbd.Catalogo, credencialesbd.Usuario, credencialesbd.Password);
                                                
                BMSCSegmentacion.InputParametersM input = new BMSCSegmentacion.InputParametersM();
                input.pTipoIngreso = tipoIngreso;                
                input.pIngreso = ingresos;
                input.pIngresoSpecified = true;
                input.pTipoIngresoSpecified = true;
                input.pMoneda = GetCodigoMonedaBMSC(new Guid(divisaid), credenciales); //2;
                
                //El usuario y contraseña de los siguientes parámetros debería ser los mismos que el que utilizamod para conectarnos a CRM
                //se dejan quemados sólo para prueba.

                input.pContrasena = credenciales.Password;
                input.pUsuario = credenciales.Usuario;
                input.pDominio = credenciales.Dominio;
              
              
                BMSCSegmentacion.OutputParametersM res = servicio.ObtenerSegmentoM(input);                
                
                
                //La respuesta de ObtenerSegmento es el código del sergmento, el cual esta en el campo efk_codigo de la entidad efk_segmento
                //por lo tanto en el cliente, en el campo "efk_segmento_ovid" hay que actualizarlo con el registro de dicho segmento.

                if (res != null)
                {
                    if (res.pSegmento != null)
                    {
                        if (numerohijos > 0)
                        {
                            Efika.Crm.Negocio.Segmentacion.ActualizarSegmentoCliente(credenciales, spCalculoOFV, cadenaconexion, accountid,
                                codigoCliente, ingresos, tipoIngreso, res.pSegmento.ToString(), edad, true, estadocivil);
                        }
                        else
                        {
                            Efika.Crm.Negocio.Segmentacion.ActualizarSegmentoCliente(credenciales, spCalculoOFV, cadenaconexion, accountid, 
                                codigoCliente, ingresos, tipoIngreso, res.pSegmento.ToString(), edad, false, estadocivil);
                        }

                    }
                }
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                cLog = new Log(cPathLog);
                cLog.Add(ex.Message, "Cliente", "SegmentarCliente", usuariocrm, ex.StackTrace);
                throw new Exception(ex.Detail.InnerText);
            }
            catch (Exception ex)
            {                
                cLog = new Log(cPathLog);
                cLog.Add(ex.Message, "Cliente", "SegmentarCliente", usuariocrm, ex.StackTrace);
                throw new Exception(ex.Message);
            }
            finally
            {
                servicio.Dispose();
            }

            return true;
        }

        [WebMethod]
        public string CadenaConexion()
        {
            Log cLog = null;
            CredencialesBD credencialesbd;
            string cadenaconexion = "";
            string organizacion = "";
            string dominio = "";
            string cPathLog = "";
            string rutaArchivoCredencialesBD = "";

            try
            {
                cadenaconexion = "Data Source={0};Initial Catalog={1};User Id={2};Password={3}";
                organizacion = ConfigurationManager.AppSettings["CRMorganizacion"];
                dominio = Environment.UserDomainName;                
                rutaArchivoCredencialesBD = ConfigurationManager.AppSettings["BDRutaArchivoCredenciales"];                
                credencialesbd = DPAPI.ObtenerVariablesConexionBDDPAPI(rutaArchivoCredencialesBD);
                cadenaconexion = string.Format(cadenaconexion, credencialesbd.Servidor, credencialesbd.Catalogo, credencialesbd.Usuario, credencialesbd.Password);
                return cadenaconexion;
            }
            catch (Exception ex)
            {
                cLog = new Log(cPathLog);
                cLog.Add(ex.Message, "Cliente", "CadenaConexion", User.ToString(), ex.StackTrace);
                throw new Exception(ex.Message);
            }

        }

        private int GetCodigoMonedaBMSC(Guid divisaId, CredencialesCRM credenciales)
        {
            Negocio.Divisa negDivisa = new Negocio.Divisa(credenciales);
            Entidades.Divisa divisa = negDivisa.ConsultaDivisa(divisaId);
            int codMonedaBMSC;
            codMonedaBMSC = int.Parse(Utilidades.GetCodBmscMoneda(divisa.isoCurrencyCode));
            return codMonedaBMSC;
        }

        public struct DatosFISACliente
        {
            public string COD_TIPO_DOC;
            public string TIPO_PERSONA;

            public void Puting(string COD_TIPO_DOC, string TIPO_PERSONA)
            {
                if (COD_TIPO_DOC != null) this.COD_TIPO_DOC = COD_TIPO_DOC;
                if (TIPO_PERSONA != null) this.TIPO_PERSONA = TIPO_PERSONA;
            }
        }

        public struct ListaResultado
        {
            public List<BusinessEntity> DatosCRMCliente;
            public DatosFISACliente DatosFISA_Cliente;
            public List<BusinessEntity> DatosCRMRepLegal;
            public List<BusinessEntity> DatosOfertaValor;

            public void Puting(List<BusinessEntity> DatosCRMCliente, DatosFISACliente DatosFISA_Cliente, List<BusinessEntity> DatosCRMRepLegal, List<BusinessEntity> DatosOfertaValor)
            {
                this.DatosCRMCliente = DatosCRMCliente;
                this.DatosFISA_Cliente = DatosFISA_Cliente;
                this.DatosCRMRepLegal = DatosCRMRepLegal;
                this.DatosOfertaValor = DatosOfertaValor;
            }

            public void PutingOne(List<BusinessEntity> DatosCRMCliente, DatosFISACliente DatosFISA_Cliente, List<BusinessEntity> DatosCRMRepLegal)
            {
                this.DatosCRMCliente = DatosCRMCliente;
                this.DatosFISA_Cliente = DatosFISA_Cliente;
                this.DatosCRMRepLegal = DatosCRMRepLegal;
            }
        }

        [WebMethod]
        public ListaResultado ServicioWebUnicoOV(string guid_cliente, bool bandera)
        {
            Guid cliente = new Guid(guid_cliente);
            //Creamos una instancia de la clase de log
            Log cLog = null;
            //*******Obtenemos las credenciales--- ESTAS SON PARA COMUNICARNOS CON CRM
            CredencialesCRM credenciales;
            CredencialesBD credencialesbd;
            string organizacion = "";
            string dominio = "";
            string rutaArchivoCredenciales = "";
            string cadenaconexion = "";
            string rutaArchivoCredencialesBD = "";
            string cPathLog = "";

            try
            {
                organizacion = ConfigurationManager.AppSettings["CRMorganizacion"];
                dominio = Environment.UserDomainName;
                rutaArchivoCredenciales = ConfigurationManager.AppSettings["CRMRutaArchivoCredenciales"];
            
                cadenaconexion = "Data Source={0};Initial Catalog={1};User Id={2};Password={3}";
                rutaArchivoCredencialesBD = ConfigurationManager.AppSettings["BDRutaArchivoCredenciales"];
                cPathLog = ConfigurationManager.AppSettings["CRMRutaLogs"];


                credenciales = DPAPI.ObtenerUsuarioPasswordDPAPI(rutaArchivoCredenciales, organizacion, dominio);

                string spDatosBDIClient = "spDatosAddServiUnikClient";
                credencialesbd = DPAPI.ObtenerVariablesConexionBDDPAPI(rutaArchivoCredencialesBD);

                cadenaconexion = string.Format(cadenaconexion, credencialesbd.Servidor, credencialesbd.Catalogo, credencialesbd.Usuario, credencialesbd.Password);
                //*******************************************************************//         
                CrmService Servicio = ServicioCRM.ObtenerServicioCRM(credenciales);
                //***********Datos CRM Cliente ***************************/
                ConditionExpression stateCondition = new ConditionExpression();
                stateCondition.AttributeName = "accountid";
                stateCondition.Operator = ConditionOperator.Equal;
                stateCondition.Values = new string[] { cliente.ToString() };

                FilterExpression outerFilter = new FilterExpression();
                outerFilter.FilterOperator = LogicalOperator.And;
                outerFilter.Conditions = new ConditionExpression[] { stateCondition };

                ColumnSet resultSetColumns = new ColumnSet();
                resultSetColumns.Attributes = new string[] { "name", "accountnumber","efk_cliente_mis","efk_tipo_identificacion","efk_segmento_ovid","efk_segmentoid"
                     ,"efk_nrodehijos_ov","efk_ingresos_ov","efk_fuente_ingresos_ov","efk_fechadenacimiento_ov","efk_estado_civil_ov","efk_codigo_cliente_texto"
                     ,"efk_tipo_cliente","efk_ingreso_mensual","revenue","efk_primerapellido","efk_segundoapellido","efk_nombre_persona"
                     ,"telephone1","emailaddress1","efk_codigo_cliente_texto","efk_codigo_tipo_cliente","efk_codigo_tipo_identificacion"};
                QueryExpression qryExpression = new QueryExpression();
                qryExpression.Criteria = outerFilter;
                qryExpression.ColumnSet = resultSetColumns;

                qryExpression.EntityName = EntityName.account.ToString();
                qryExpression.Distinct = false;

                BusinessEntityCollection clienteResultSet = Servicio.RetrieveMultiple(qryExpression);


                //***********Datos FISA Cliente ***************************/
                DataTable dtFisa = null;
                foreach (account eCliente in clienteResultSet.BusinessEntities)
                {

                    if (eCliente.efk_codigo_cliente_texto == null) eCliente.efk_codigo_cliente_texto = "0";
                    string codperson = null;
                    string codtdoc = null;
                    if (eCliente.efk_tipo_cliente != null) codperson = eCliente.efk_tipo_cliente.Value.ToString();
                    if (eCliente.efk_tipo_identificacion != null) codtdoc = eCliente.efk_tipo_identificacion.Value.ToString();

                    dtFisa = Efika.Crm.Negocio.Cliente.DatosBDI_ServicioUnik(cadenaconexion, spDatosBDIClient, codperson, codtdoc);                                      

                }

                //***********Datos CRM RepLegal ***************************/
                ConditionExpression stateCondition_rep = new ConditionExpression();
                stateCondition_rep.AttributeName = "efk_representantelegalid";
                stateCondition_rep.Operator = ConditionOperator.Equal;
                stateCondition_rep.Values = new string[] { cliente.ToString() };

                FilterExpression outerFilter_rep = new FilterExpression();
                outerFilter_rep.FilterOperator = LogicalOperator.And;
                outerFilter_rep.Conditions = new ConditionExpression[] { stateCondition_rep };

                ColumnSet resultSetColumns_rep = new ColumnSet();
                resultSetColumns_rep.Attributes = new string[] { "efk_representante_legalid", "efk_name", "efk_documento_identidad", "efk_cargo", "efk_name" };
                QueryExpression qryExpression_rep = new QueryExpression();
                qryExpression_rep.Criteria = outerFilter_rep;
                qryExpression_rep.ColumnSet = resultSetColumns_rep;

                qryExpression_rep.EntityName = EntityName.efk_representante_legal.ToString();
                qryExpression_rep.Distinct = false;

                BusinessEntityCollection RepLegalResultSet = Servicio.RetrieveMultiple(qryExpression_rep);


                //***********Datos CRM Oferta de Valor ***************************/
                ConditionExpression stateCondition_ov = new ConditionExpression();
                stateCondition_ov.AttributeName = "efk_cliente_juridico_id";
                stateCondition_ov.Operator = ConditionOperator.Equal;
                stateCondition_ov.Values = new string[] { cliente.ToString() };

                FilterExpression outerFilter_ov = new FilterExpression();
                outerFilter_ov.FilterOperator = LogicalOperator.And;
                outerFilter_ov.Conditions = new ConditionExpression[] { stateCondition_ov };

                ColumnSet resultSetColumns_ov = new ColumnSet();
                resultSetColumns_ov.Attributes = new string[] { "efk_tipo_productos_id","efk_product_id","efk_prioridad","efk_familia_productos_id"
                                   ,"efk_cliente_natural_id","efk_cliente_juridico_id","efk_oferta_valorid","efk_prioridad_portafolio"
                                   ,"efk_portafolio"};
                QueryExpression qryExpression_ov = new QueryExpression();
                qryExpression_ov.Criteria = outerFilter_ov;
                qryExpression_ov.ColumnSet = resultSetColumns_ov;

                qryExpression_ov.EntityName = EntityName.efk_oferta_valor.ToString();
                qryExpression_ov.Distinct = false;

                BusinessEntityCollection ofertaResultSet_ov = Servicio.RetrieveMultiple(qryExpression_ov);

                //*********** ***************************/

                ListaResultado reusltado = new ListaResultado();
                DatosFISACliente datosfisa_client = new DatosFISACliente();

                string COD_TIPO_DOC = null;
                string TIPO_PERSONA = null;
                if (dtFisa != null)
                {
                    for (int i = 0; i < dtFisa.Rows.Count; i++)
                    {
                        if (dtFisa.Rows[i][0].ToString() == "COD_TIPO_DOC") COD_TIPO_DOC = dtFisa.Rows[i][1].ToString();
                        else if (dtFisa.Rows[i][0].ToString() == "TIPO_PERSONA") TIPO_PERSONA = dtFisa.Rows[i][1].ToString();
                    }
                }

                datosfisa_client.Puting(COD_TIPO_DOC, TIPO_PERSONA);

                if (bandera)
                    reusltado.Puting(clienteResultSet.BusinessEntities.ToList(), datosfisa_client, RepLegalResultSet.BusinessEntities.ToList(), ofertaResultSet_ov.BusinessEntities.ToList());
                else
                    reusltado.PutingOne(clienteResultSet.BusinessEntities.ToList(), datosfisa_client, RepLegalResultSet.BusinessEntities.ToList());
 
                Servicio.Dispose();
                return reusltado;

            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                cLog = new Log(cPathLog);
                cLog.Add(ex.Message, "Clientes", "ServicioWebUnicoOV", User.ToString(), ex.StackTrace);
                throw new Exception(ex.Detail.InnerText);                
            }
            catch (Exception ex)
            {               
                cLog = new Log(cPathLog);
                cLog.Add(ex.Message, "Clientes", "ServicioWebUnicoOV", User.ToString(), ex.StackTrace);
                throw new Exception(ex.Message);
            }
            finally
            {
                
            }
        }

        [WebMethod]
        public bool SincronizarMonedaCliente(string clienteCodigo, string divisaCodigo)
        {
            //Creamos una instancia de la clase de log
            Log cLog = null;
            string cadenaconexion = "";
            string organizacion = "";
            string dominio = "";
            string rutaArchivoCredenciales = "";
            string rutaArchivoCredencialesBD = "";
            string cPathLog = "";
            BMSCSegmentacion.ObtenerSegmentoMService servicio = null;

            try
            {
                cadenaconexion = "Data Source={0};Initial Catalog={1};User Id={2};Password={3}";
                organizacion = ConfigurationManager.AppSettings["CRMorganizacion"];

                dominio = Environment.UserDomainName;
                rutaArchivoCredenciales = ConfigurationManager.AppSettings["CRMRutaArchivoCredenciales"];
                rutaArchivoCredencialesBD = ConfigurationManager.AppSettings["BDRutaArchivoCredenciales"];
                cPathLog = ConfigurationManager.AppSettings["CRMRutaLogs"];
                //spCalculoOFV = ConfigurationManager.AppSettings["CRMNombreSProcedureOFV"];

                servicio = new BMSCSegmentacion.ObtenerSegmentoMService();

                CredencialesCRM credenciales = DPAPI.ObtenerUsuarioPasswordDPAPI(rutaArchivoCredenciales, organizacion, dominio);
                CredencialesBD credencialesbd = DPAPI.ObtenerVariablesConexionBDDPAPI(rutaArchivoCredencialesBD);

                cadenaconexion = string.Format(cadenaconexion, credencialesbd.Servidor, credencialesbd.Catalogo, credencialesbd.Usuario, credencialesbd.Password);
                //cadenaconexion = string.Format(cadenaconexion, "192.168.1.192", credencialesbd.Catalogo, credencialesbd.Usuario, credencialesbd.Password);

                Efika.Crm.Negocio.Cliente.SincronizarMonedaCliente(cadenaconexion, clienteCodigo, divisaCodigo);
                

            } catch (Exception ex)
            {
                cLog = new Log(cPathLog);
                cLog.Add(ex.Message, "Clientes", "SincronizarMonedaCliente", User!=null? User.ToString():"", ex.StackTrace);
                throw new Exception(ex.Message);
                //return false;
            }

            return true;
        }

        [WebMethod]
        public bool GenerarOfertaValorMicro(string accountid)
        {
            try
            {
                string cadenaconexion = "Data Source={0};Initial Catalog={1};User Id={2};Password={3}";
                CredencialesBD credencialesbd = DPAPI.ObtenerVariablesConexionBDDPAPI(ConfigurationManager.AppSettings["BDRutaArchivoCredenciales"]);
                cadenaconexion = string.Format(cadenaconexion, credencialesbd.Servidor, credencialesbd.Catalogo, credencialesbd.Usuario, credencialesbd.Password);

                Negocio.OfertaValor negOfValor = new Negocio.OfertaValor(cadenaconexion);

                return negOfValor.GeneraOfertaValorMicro(new Guid(accountid));
            }
            catch (Exception ex)
            {
                Log cLog = new Log(ConfigurationManager.AppSettings["CRMRutaLogs"]);
                cLog.Add(ex.Message, "Clientes", "GenerarOfertaValorMicro", User != null ? User.ToString() : "", ex.StackTrace);
                throw new Exception(ex.Message);
            }
        }
    }

}
