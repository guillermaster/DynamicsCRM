using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Configuration;
using System.Web.Services;
using Efika.Crm.Entidades;
using Efika.Crm.Negocio;
using System.Collections;
using Efika.Crm.AccesoServicios;
using Efika.Crm.AccesoServicios.CRMSDK;
using Efika.Crm.Entidades.Common;
using System.Net;

namespace Efika.Crm.WebService
{
    /// <summary>
    /// Summary description for ProdPasivos
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ProdPasivos : System.Web.Services.WebService
    {
        [WebMethod]
        public bool ProductoPasivo_ActualizarSaldo(int codigoCliente, string accountId, string usuariocrm)
        {
            //Creamos una instancia de la clase de log
            Log cLog = null;
            string organizacion = "";
            string dominio = "";
            string rutaArchivoCredenciales = "";
            string cPathLog = "";
            string sTipoCuenta = "";
            BMSCTraerSaldos.TraerSaldosService servicio = null;
            ServicePointManager.ServerCertificateValidationCallback += MyCertificatePolicy.ValidateCertificate;
            //Obtenemos las credenciales--- ESTAS SON PARA COMUNICARNOS CON CRM
            CredencialesCRM credenciales;
            try
            {
                organizacion = ConfigurationManager.AppSettings["CRMorganizacion"];
                dominio = Environment.UserDomainName;
                rutaArchivoCredenciales = ConfigurationManager.AppSettings["CRMRutaArchivoCredenciales"];
                cPathLog = ConfigurationManager.AppSettings["CRMRutaLogs"];
                sTipoCuenta = ConfigurationManager.AppSettings["CRMTipoCuenta"];
                      
                servicio = new BMSCTraerSaldos.TraerSaldosService();

                credenciales = DPAPI.ObtenerUsuarioPasswordDPAPI(rutaArchivoCredenciales, organizacion, dominio);
                BMSCTraerSaldos.traerSaldos_request req = new BMSCTraerSaldos.traerSaldos_request();
                req.pCodCliente = (decimal)codigoCliente;           
            
                //El usuario y contraseña de los siguientes parámetros debería ser los mismos que el que utilizamod para conectarnos a CRM
                //se dejan quemados sólo para prueba.

                req.pContrasena = credenciales.Password;
                req.pUsuario = credenciales.Usuario;
                req.pDominio = credenciales.Dominio;                

                BMSCTraerSaldos.traerSaldos_response res = servicio.TraerSaldos(req);
                
                // Evaluamos lo que nos devuelve el Servicio Web
         
                if (res != null)
                {
                    if (res.pCuentas != null)
                    {
                        if (res.pCuentas.Length > 0)
                        {
                            ArrayList cuentas = new ArrayList();
                            for (int i = 0; i < res.pCuentas.Length; i++)
                            {
                                ProductosPasivo p = new ProductosPasivo();
                               
                                p.NumeroCuenta = res.pCuentas[i].cuenta;
                              
                                Utilidades obj = new Utilidades();
                                p.Moneda = obj.GetCodIsoMoneda(res.pMonedas[i].moneda);
                                
                                if (res.pTiposCuentas != null)
                                {
                                    if (res.pTiposCuentas.Length > 0)
                                        p.TipoCuenta = res.pTiposCuentas[i].tipoCuenta;
                                    else
                                        p.TipoCuenta = "";
                                }
                                else
                                    p.TipoCuenta = "";

                                

                                if (res.pSaldosContables != null)
                                {
                                    if (res.pSaldosContables.Length > 0)
                                        p.SaldoContable = res.pSaldosContables[i].saldoContable;
                                    else
                                        p.SaldoContable = 0;
                                }
                                else
                                    p.SaldoContable = 0;

                                if (res.pSaldosDisponibles != null)
                                {
                                    if (res.pSaldosDisponibles.Length > 0)
                                        p.SaldoDisponible = res.pSaldosDisponibles[i].saldoDisponible;
                                    else
                                        p.SaldoDisponible = 0;
                                }
                                else
                                    p.SaldoDisponible = 0;
                             
                                cuentas.Add(p);
                            }
                            Efika.Crm.Negocio.ProductosPasivoBMSC eProductoPasivo = new Efika.Crm.Negocio.ProductosPasivoBMSC();
                            return eProductoPasivo.ActualizarProductoCliente(credenciales, accountId, codigoCliente, cuentas, sTipoCuenta);
                        }
                        else
                            return false;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                cLog = new Log(cPathLog);
                cLog.Add(ex.Message, "ProdPasivos", "ProductoPasivo_ActualizarSaldo", usuariocrm, ex.StackTrace);
                throw new Exception(ex.Detail.InnerText);   
            }
            catch (Exception ex)
            {
                cLog = new Log(cPathLog);
                cLog.Add(ex.Message, "ProdPasivos", "ProductoPasivo_ActualizarSaldo", usuariocrm, ex.StackTrace);
                throw new Exception(ex.Message);
            }
            finally
            {
                servicio.Dispose();
            }
        }

        [WebMethod]
        public string ProductoPasivo_ActualizarSaldo_revisando(int codigoCliente, string accountId)
        {
            //Creamos una instancia de la clase de log
            Log cLog = null;
            //Obtenemos las credenciales--- ESTAS SON PARA COMUNICARNOS CON CRM
            CredencialesCRM credenciales;
            string organizacion = ConfigurationManager.AppSettings["CRMorganizacion"];
            string dominio = Environment.UserDomainName;
            string rutaArchivoCredenciales = ConfigurationManager.AppSettings["CRMRutaArchivoCredenciales"];
            string cPathLog = ConfigurationManager.AppSettings["CRMRutaLogs"];
            credenciales = DPAPI.ObtenerUsuarioPasswordDPAPI(rutaArchivoCredenciales, organizacion, dominio);

            ServicePointManager.ServerCertificateValidationCallback += MyCertificatePolicy.ValidateCertificate;

            BMSCTraerSaldos.TraerSaldosService servicio = new BMSCTraerSaldos.TraerSaldosService();

            try
            {
                BMSCTraerSaldos.traerSaldos_request req = new BMSCTraerSaldos.traerSaldos_request();
                req.pCodCliente = (decimal)codigoCliente;

                //El usuario y contraseña de los siguientes parámetros debería ser los mismos que el que utilizamod para conectarnos a CRM
                //se dejan quemados sólo para prueba.

                req.pContrasena = credenciales.Password;
                req.pUsuario = credenciales.Usuario;
                req.pDominio = credenciales.Dominio;           

                BMSCTraerSaldos.traerSaldos_response res = servicio.TraerSaldos(req);

                return "llegao aca OTRO" + credenciales.Dominio + " " + credenciales.Usuario + " " + credenciales.Password;
              
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                cLog = new Log(cPathLog);
                cLog.Add(ex.Message, "ProdPasivos", "ProductoPasivo_ActualizarSaldo", User.ToString(), ex.StackTrace);
                throw new Exception(ex.Detail.InnerText);
            }
            catch (Exception ex)
            {
                cLog = new Log(cPathLog);
                cLog.Add(ex.Message, "ProdPasivos", "ProductoPasivo_ActualizarSaldo", User.ToString(), ex.StackTrace);
                throw new Exception(ex.Message);
            }
            finally
            {
                servicio.Dispose();
            }
        }

        [WebMethod]
        // public ListasFinales ServicioWebUnicoOV(string guid_cliente, bool bandera)
        public List<BusinessEntity> ServicioCatalogoProductos()
        {
            //Creamos una instancia de la clase de log
            Log cLog = null;
            string organizacion = "";
            string dominio = "";
            string rutaArchivoCredenciales = "";
            string cPathLog = "";
            CrmService Servicio = null;
            //*******Obtenemos las credenciales--- ESTAS SON PARA COMUNICARNOS CON CRM
            CredencialesCRM credenciales;

            try
            {
                organizacion = ConfigurationManager.AppSettings["CRMorganizacion"];
                dominio = Environment.UserDomainName;
                rutaArchivoCredenciales = ConfigurationManager.AppSettings["CRMRutaArchivoCredenciales"];
                cPathLog = ConfigurationManager.AppSettings["CRMRutaLogs"];
                credenciales = DPAPI.ObtenerUsuarioPasswordDPAPI(rutaArchivoCredenciales, organizacion, dominio);
                //*******************************************************************//

                Servicio = ServicioCRM.ObtenerServicioCRM(credenciales);

                ColumnSet resultSetColumns = new ColumnSet();
                resultSetColumns.Attributes = new string[] { "name", "efk_tipo_productoid", "efk_familia_productosid", "statecode", "efk_habilitado_comercializar", "productid" };
                QueryExpression qryExpression = new QueryExpression();
                qryExpression.ColumnSet = resultSetColumns;

                qryExpression.EntityName = EntityName.product.ToString();
                qryExpression.Distinct = false;

                BusinessEntityCollection CatalogoResultSet = Servicio.RetrieveMultiple(qryExpression);
                return CatalogoResultSet.BusinessEntities.ToList();
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                cLog = new Log(cPathLog);
                cLog.Add(ex.Message, "ProdPasivos", "ServicioCatalogoProductos", User.ToString(), ex.StackTrace);
                throw new Exception(ex.Detail.InnerText);
            }
            catch (Exception ex)
            {
                cLog = new Log(cPathLog);
                cLog.Add(ex.Message, "ProdPasivos", "ServicioCatalogoProductos", User.ToString(), ex.StackTrace);
                throw new Exception(ex.Message);
            }
            finally
            {
                Servicio.Dispose();
            }

        }
    }
}
