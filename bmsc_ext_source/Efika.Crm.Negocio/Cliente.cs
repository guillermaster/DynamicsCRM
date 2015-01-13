using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Efika.Crm.AccesoServicios.CRMSDK;
using Efika.Crm.AccesoServicios;
using Efika.Crm.Entidades;
using System.Data.SqlClient;
using System.Data;
using System.Web.Services.Protocols;

namespace Efika.Crm.Negocio
{
    public class Cliente
    {
        private CrmService Servicio;
        private CredencialesCRM Credenciales;

        public Cliente(CredencialesCRM credenciales)
        {
            Credenciales = credenciales;
            Servicio = ServicioCRM.ObtenerServicioCRM(credenciales);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cadenaConexion"></param>
        /// <param name="codtipoper"></param>
        /// <param name="codtipodoc"></param>      
        /// <returns></returns>
        public static DataTable DatosBDI_ServicioUnik(string cadenaConexion, string sp, string codtipoper, string codtipodoc)
        {
            SqlConnection mysqlconnection = null;
            string error;
            try
            {
                using (mysqlconnection = new SqlConnection(cadenaConexion))
                {
                    mysqlconnection.Open();
                    if (mysqlconnection.State == System.Data.ConnectionState.Open)
                    {
                        SqlDataAdapter MyDataAdapter = new SqlDataAdapter(sp, mysqlconnection);
                        MyDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;


                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@codtipoper", SqlDbType.VarChar, 20));
                        MyDataAdapter.SelectCommand.Parameters["@codtipoper"].Value = codtipoper;

                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@codtipodoc", SqlDbType.VarChar, 20));
                        MyDataAdapter.SelectCommand.Parameters["@codtipodoc"].Value = codtipodoc;

                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@error", SqlDbType.VarChar, 200));
                        MyDataAdapter.SelectCommand.Parameters["@error"].Value = null;
                        MyDataAdapter.SelectCommand.Parameters["@error"].Direction = ParameterDirection.Output;

                        ///rellenar data set y dataadapter
                        DataSet DS = new DataSet();
                        MyDataAdapter.Fill(DS);
                        return DS.Tables[0];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                mysqlconnection.Close();
            }
        }


        public static string DatosBDI_TipoTrabajo(string cadenaConexion, string codTipoTrabajoCRM)
        {
            SqlConnection sqlconnection = null;
            string codTipoTrabajoDBI;

            try
            {
                using (sqlconnection = new SqlConnection(cadenaConexion))
                {
                    sqlconnection.Open();
                    if (sqlconnection.State == System.Data.ConnectionState.Open)
                    {
                        SqlDataAdapter da = new SqlDataAdapter("spDatosTipoTrabajoCliente", sqlconnection);
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;

                        da.SelectCommand.Parameters.Add(new SqlParameter("@codTipoTrabajoCRM", SqlDbType.VarChar, 20));
                        da.SelectCommand.Parameters["@codTipoTrabajoCRM"].Value = codTipoTrabajoCRM;

                        da.SelectCommand.Parameters.Add(new SqlParameter("@error", SqlDbType.VarChar, 200));
                        da.SelectCommand.Parameters["@error"].Value = null;
                        da.SelectCommand.Parameters["@error"].Direction = ParameterDirection.Output;

                        ///rellenar data set y dataadapter
                        DataSet ds = new DataSet();
                        da.Fill(ds);

                        codTipoTrabajoDBI = ds.Tables[0].Rows[0][0].ToString();
                    }
                    else
                    {
                        throw new Exception("No se pudo establecer conexión a la base de datos");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlconnection.Close();
            }

            return codTipoTrabajoDBI;
        }


        public static string DatosBDI_TipoIdentificacion(string cadenaConexion, string codTipoIdentCRM)
        {
            SqlConnection sqlconnection = null;
            string codTipoIdentificacionDBI;

            try
            {
                using (sqlconnection = new SqlConnection(cadenaConexion))
                {
                    sqlconnection.Open();
                    if (sqlconnection.State == System.Data.ConnectionState.Open)
                    {
                        SqlDataAdapter da = new SqlDataAdapter("spDatosTipoIdentificacionCliente", sqlconnection);
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;

                        da.SelectCommand.Parameters.Add(new SqlParameter("@codTipoIdentCRM", SqlDbType.VarChar, 20));
                        da.SelectCommand.Parameters["@codTipoIdentCRM"].Value = codTipoIdentCRM;

                        da.SelectCommand.Parameters.Add(new SqlParameter("@error", SqlDbType.VarChar, 200));
                        da.SelectCommand.Parameters["@error"].Value = null;
                        da.SelectCommand.Parameters["@error"].Direction = ParameterDirection.Output;

                        ///rellenar data set y dataadapter
                        DataSet ds = new DataSet();
                        da.Fill(ds);

                        codTipoIdentificacionDBI = ds.Tables[0].Rows[0][0].ToString();
                    }
                    else
                    {
                        throw new Exception("No se pudo establecer conexión a la base de datos");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlconnection.Close();
            }

            return codTipoIdentificacionDBI;
        }


        public static string DatosBDI_TipoCliente(string cadenaConexion, string codTipoClienteCRM)
        {
            SqlConnection sqlconnection = null;
            string codTipoClienteDBI;

            try
            {
                using (sqlconnection = new SqlConnection(cadenaConexion))
                {
                    sqlconnection.Open();
                    if (sqlconnection.State == System.Data.ConnectionState.Open)
                    {
                        SqlDataAdapter da = new SqlDataAdapter("spDatosTipoCliente", sqlconnection);
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;

                        da.SelectCommand.Parameters.Add(new SqlParameter("@codTipoClienteCRM", SqlDbType.VarChar, 20));
                        da.SelectCommand.Parameters["@codTipoClienteCRM"].Value = codTipoClienteCRM;

                        da.SelectCommand.Parameters.Add(new SqlParameter("@error", SqlDbType.VarChar, 200));
                        da.SelectCommand.Parameters["@error"].Value = null;
                        da.SelectCommand.Parameters["@error"].Direction = ParameterDirection.Output;

                        ///rellenar data set y dataadapter
                        DataSet ds = new DataSet();
                        da.Fill(ds);

                        codTipoClienteDBI = ds.Tables[0].Rows[0][0].ToString();
                    }
                    else
                    {
                        throw new Exception("No se pudo establecer conexión a la base de datos");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlconnection.Close();
            }

            return codTipoClienteDBI;
        }


        public Entidades.Cliente DatosCliente(Guid idCliente, ColumnSet campos)
        {
            Entidades.Cliente cliente;
            try
            {
                account clienteCrm = (account)Servicio.Retrieve(EntityName.account.ToString(), idCliente, campos);
                cliente = new Entidades.Cliente();

                if (clienteCrm.efk_tipo_cliente != null)
                {
                    cliente.TipoClienteDesc = clienteCrm.efk_tipo_cliente.name;
                    cliente.TipoClienteId = clienteCrm.efk_tipo_cliente.Value;
                    if (cliente.TipoClienteId == Entidades.Cliente.TipoCliente.Natural)//NATURAL
                    {
                        cliente.Nombre = clienteCrm.efk_nombre_persona;
                        cliente.Apellido1 = clienteCrm.efk_primerapellido;
                        cliente.Apellido2 = clienteCrm.efk_segundoapellido;
                    }
                    else if (cliente.TipoClienteId == Entidades.Cliente.TipoCliente.Juridico)//JURIDICO
                    {
                        cliente.Nombre = clienteCrm.name;
                        cliente.RazonSocial = clienteCrm.name;
                    }
                    else
                        cliente.Nombre = clienteCrm.name;
                }
                else
                    cliente.Nombre = clienteCrm.name;
                if (clienteCrm.efk_tipo_cliente != null)
                {
                    cliente.TipoClienteDesc = clienteCrm.efk_tipo_cliente.name;
                    cliente.TipoClienteId = clienteCrm.efk_tipo_cliente.Value;
                }
                if (clienteCrm.efk_fuente_ingresos_ov != null)
                {
                    cliente.FuenteIngresoOfValDesc = clienteCrm.efk_fuente_ingresos_ov.name;
                    cliente.FuenteIngresoOfValId = clienteCrm.efk_fuente_ingresos_ov.Value;
                }
                if (clienteCrm.efk_tipo_identificacion != null)
                {
                    cliente.TipoIdentificacion = clienteCrm.efk_tipo_identificacion.Value;
                    cliente.TipoClienteDesc = clienteCrm.efk_tipo_identificacion.name;
                }
                if (clienteCrm.efk_segmento_ovid != null)
                {
                    cliente.SegmentoDesc = clienteCrm.efk_segmento_ovid.name;
                }
                cliente.Telefono = clienteCrm.telephone1;
                cliente.Identificacion = clienteCrm.accountnumber;
                cliente.Email = clienteCrm.emailaddress1;
                cliente.Id = idCliente;
                return cliente;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetTelefonoCliente(Guid idCliente)
        {
            string telefono;
            try
            {
                ColumnSet cs = new ColumnSet();
                cs.Attributes = new string[] { "telephone1" };
                Entidades.Cliente cliente = DatosCliente(idCliente, cs);
                telefono = cliente.Telefono;
            }
            catch
            {
                telefono = string.Empty;
            }
            return telefono;
        }

        public account GetAccountById(Guid accountID, ColumnSet columnSet)
        {
            try
            {
                account clienteCrm = (account)Servicio.Retrieve(EntityName.account.ToString(), accountID, columnSet);
                return clienteCrm;
            }
            catch (SoapException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<Entidades.OfertaValor> GetOfertaValorCliente(Guid idCliente, ColumnSet cs, string orderBy)
        {
            try
            {
                List<Entidades.OfertaValor> ofertasValores = new List<Entidades.OfertaValor>();

                ConditionExpression condCliente = new ConditionExpression();
                condCliente.AttributeName = "efk_cliente_juridico_id";
                condCliente.Operator = ConditionOperator.Equal;
                condCliente.Values = new string[] { idCliente.ToString() };

                QueryExpression qryExp = new QueryExpression();
                qryExp.EntityName = EntityName.efk_oferta_valor.ToString();
                qryExp.Criteria = new FilterExpression();
                qryExp.Criteria.Conditions = new ConditionExpression[] { condCliente };
                qryExp.ColumnSet = cs;
                OrderExpression orderExp = new OrderExpression();
                orderExp.AttributeName = orderBy;
                orderExp.OrderType = OrderType.Ascending;
                qryExp.Orders = new OrderExpression[] { orderExp };

                BusinessEntityCollection becOf = Servicio.RetrieveMultiple(qryExp);

                foreach (BusinessEntity beOf in becOf.BusinessEntities)
                {
                    efk_oferta_valor ofValor = (efk_oferta_valor)beOf;
                    Entidades.OfertaValor ofertaValor = new Entidades.OfertaValor();

                    if (ofValor.efk_oferta_valorid != null)
                        ofertaValor.Id = ofValor.efk_oferta_valorid.Value;
                    if (ofValor.efk_prioridad != null)
                        ofertaValor.PrioridadProducto = ofValor.efk_prioridad.Value;
                    if (ofValor.efk_prioridad_portafolio != null)
                        ofertaValor.PrioridadPortafolio = ofValor.efk_prioridad_portafolio.Value;
                    if (ofValor.efk_product_id != null)
                        ofertaValor.ProductoId = ofValor.efk_product_id.Value;
                    if (ofValor.efk_portafolio != null)
                        ofertaValor.Portafolio = ofValor.efk_portafolio;

                    ofertasValores.Add(ofertaValor);
                }

                return ofertasValores;
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                throw new Exception(ex.Detail.InnerText);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        
        public bool ClienteDependiente(Guid idCliente)
        {
            try
            {
                ColumnSet cs = new ColumnSet();
                cs.Attributes = new string[] { "efk_cliente_mis","efk_tipo_cliente", "efk_tipo_banca"
                                               , "efk_fuente_ingresos", "efk_fuente_ingresos_ov", "efk_segmento_ovid" };
                account entCliente = GetAccountById(idCliente, cs);

                if (entCliente.efk_cliente_mis.Value)
                {
                    if (entCliente.efk_tipo_cliente != null && entCliente.efk_tipo_banca != null && entCliente.efk_fuente_ingresos != null)
                    {
                        if (entCliente.efk_tipo_cliente.Value == Entidades.Cliente.TipoCliente.Natural
                            && entCliente.efk_tipo_banca.Value != Entidades.Cliente.TipoBanca.BancaCorporativaEmpresas
                            && entCliente.efk_fuente_ingresos.Value == Entidades.Cliente.FuenteIngresos.Dependiente)
                        {
                            return true;
                        }
                        else
                            return false;
                    }
                    else
                        return false;
                }
                else
                {
                    if (entCliente.efk_tipo_cliente != null && entCliente.efk_fuente_ingresos_ov != null && entCliente.efk_segmento_ovid != null)
                    {
                        if (entCliente.efk_tipo_cliente.Value == Entidades.Cliente.TipoCliente.Natural
                            && entCliente.efk_fuente_ingresos_ov.Value == Entidades.Cliente.FuenteIngresos.Dependiente
                            && entCliente.efk_segmento_ovid.name.Contains("D-"))
                        {
                            return true;
                        }
                        else
                            return false;
                    }
                    else
                        return false;
                }
            }
            catch (SoapException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Guid GetMonedaOperacionCliente(Guid idCliente)
        {
            try
            {
                ColumnSet cs = new ColumnSet();
                cs.Attributes = new string[] { "transactioncurrencyid" };
                account entCliente = GetAccountById(idCliente, cs);
                return entCliente.transactioncurrencyid.Value;
            }
            catch (SoapException ex)
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(ex.Detail.InnerXml);
                throw new Exception(xmlDoc.FirstChild.ChildNodes[1].InnerText);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public decimal GetIngresosModeloEvaluador(Guid idCliente)
        {
            try
            {
                ColumnSet cs = new ColumnSet();
                cs.Attributes = new string[] { "efk_salario_liquido_titularmes1", "efk_salario_liquido_titularmes2", "efk_salario_liquido_titularmes3",
                    "efk_salario_liquido_conyuguemes1", "efk_salario_liquido_conyuguemes2", "efk_salario_liquido_conyuguemes3", "efk_ingresos_mensuales_alquileres_titular", 
                    "efk_ingresos_anuales_abonos_titular", "efk_ingresos_mensuales_alquileres_conyugue", "efk_ingresos_anuales_abonos_conyugue"};
                account entCliente = GetAccountById(idCliente, cs);

                decimal ingresos = 0;

                if (entCliente.efk_salario_liquido_titularmes1 != null)
                    ingresos += entCliente.efk_salario_liquido_titularmes1.Value;
                if(entCliente.efk_salario_liquido_titularmes2 != null)
                    ingresos += entCliente.efk_salario_liquido_titularmes2.Value;
                if(entCliente.efk_salario_liquido_titularmes3 != null)
                    ingresos += entCliente.efk_salario_liquido_titularmes3.Value;
                if(entCliente.efk_salario_liquido_conyuguemes1 != null)
                    ingresos += entCliente.efk_salario_liquido_conyuguemes1.Value;
                if(entCliente.efk_salario_liquido_conyuguemes2 != null)
                    ingresos += entCliente.efk_salario_liquido_conyuguemes2.Value;
                if(entCliente.efk_salario_liquido_conyuguemes3 != null)
                    ingresos += entCliente.efk_salario_liquido_conyuguemes3.Value;
                if (entCliente.efk_ingresos_mensuales_alquileres_titular != null)
                    ingresos += entCliente.efk_ingresos_mensuales_alquileres_titular.Value;
                if(entCliente.efk_ingresos_anuales_abonos_titular != null)
                    ingresos += entCliente.efk_ingresos_anuales_abonos_titular.Value;
                if(entCliente.efk_ingresos_mensuales_alquileres_conyugue != null)
                    ingresos += entCliente.efk_ingresos_mensuales_alquileres_conyugue.Value;
                if(entCliente.efk_ingresos_anuales_abonos_conyugue != null)
                    ingresos += entCliente.efk_ingresos_anuales_abonos_conyugue.Value;

                return ingresos;
            }
            catch (SoapException ex)
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(ex.Detail.InnerXml);
                throw new Exception(xmlDoc.FirstChild.ChildNodes[1].InnerText);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static decimal DatosMonedaCliente(string cadenaConexion, Guid transactioncurrencyid) 
        {
            string moneda;
            SqlConnection sqlconnection = null;
            

            try
            {
                using (sqlconnection = new SqlConnection(cadenaConexion))
                {
                    sqlconnection.Open();
                    if (sqlconnection.State == System.Data.ConnectionState.Open)
                    {
                        SqlDataAdapter da = new SqlDataAdapter("spDatoClieneMoneda", sqlconnection);
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;

                        da.SelectCommand.Parameters.Add(new SqlParameter("@transactioncurrencyid", SqlDbType.UniqueIdentifier));
                        da.SelectCommand.Parameters["@transactioncurrencyid"].Value = transactioncurrencyid;

                       
                        ///rellenar data set y dataadapter
                        DataSet ds = new DataSet();
                        da.Fill(ds);

                        moneda = ds.Tables[0].Rows[0][0].ToString();
                    }
                    else
                    {
                        throw new Exception("No se pudo establecer conexión a la base de datos");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlconnection.Close();
            }

        return Convert.ToDecimal(moneda);
        }

        public static bool SincronizarMonedaCliente(string cadenaConexion, string clienteCodigo, string monedaISOCodigo)
        {
            int clienteCodigoint = -1;
            if (int.TryParse(clienteCodigo, out clienteCodigoint))
            {
                SqlConnection sqlconnection = null;
                try
                {
                    using (sqlconnection = new SqlConnection(cadenaConexion))
                    {
                        sqlconnection.Open();
                        if (sqlconnection.State == System.Data.ConnectionState.Open)
                        {
                            SqlCommand sqlc = new SqlCommand("sp_sincronizar_moneda_cliente", sqlconnection);
                            sqlc.CommandType = CommandType.StoredProcedure;
                            sqlc.Parameters.AddWithValue("@iClienteCodigo", clienteCodigoint);
                            sqlc.Parameters.AddWithValue("@sCodigoMoneda", monedaISOCodigo);
                            sqlc.ExecuteNonQuery();
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (sqlconnection != null)
                        sqlconnection.Close();
                }
            }
            return false;
        }


        public bool TieneTipoProductoActivo(Guid idCliente, Guid idTipoProducto)
        {
            try
            {
                ConditionExpression condCliente = new ConditionExpression();
                condCliente.AttributeName = "efk_cliente_juridico_id";
                condCliente.Operator = ConditionOperator.Equal;
                condCliente.Values = new string[] { idCliente.ToString() };

                ColumnSet cs = new ColumnSet();
                cs.Attributes = new string[] { "efk_producto_core_id" };

                QueryExpression qryExp = new QueryExpression();
                qryExp.EntityName = EntityName.efk_producto_activo.ToString();
                qryExp.Criteria = new FilterExpression();
                qryExp.Criteria.Conditions = new ConditionExpression[] { condCliente };
                qryExp.ColumnSet = cs;
                
                BusinessEntityCollection becProdAct = Servicio.RetrieveMultiple(qryExp);

                if (becProdAct.BusinessEntities.Length > 0)
                {
                    Guid idProdCore;
                    Guid prodActIdTipoProd;
                    efk_producto_activo efkPAct;

                    Producto objProducto = new Producto(Credenciales);
                    bool founded = false;
                    int i = 0;

                    while(!founded && i < becProdAct.BusinessEntities.Length)
                    {
                        efkPAct = (efk_producto_activo)becProdAct.BusinessEntities[i];
                        if (efkPAct.efk_producto_core_id != null)
                        {
                            idProdCore = efkPAct.efk_producto_core_id.Value;
                            //obtener el id tipo producto correspondiente al producto banco
                            prodActIdTipoProd = objProducto.GetTipoProductoFromProductoBanco(idProdCore);
                            if (prodActIdTipoProd == idTipoProducto)
                            {
                                founded = true;
                            }
                        }
                        i++;
                    }
                    return founded;
                }
                else
                    return false;
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(ex.Detail.InnerXml);
                throw new Exception(xmlDoc.FirstChild.ChildNodes[1].InnerText);
            }
            catch
            {
                throw;
            }
        }


        public bool TieneTipoProductoPasivo(Guid idCliente, Guid idTipoProducto)
        {
            try
            {
                ConditionExpression condCliente = new ConditionExpression();
                condCliente.AttributeName = "efk_cliente_juridico_id";
                condCliente.Operator = ConditionOperator.Equal;
                condCliente.Values = new string[] { idCliente.ToString() };

                ColumnSet cs = new ColumnSet();
                cs.Attributes = new string[] { "efk_producto_core_id" };

                QueryExpression qryExp = new QueryExpression();
                qryExp.EntityName = EntityName.efk_producto_pasivo.ToString();
                qryExp.Criteria = new FilterExpression();
                qryExp.Criteria.Conditions = new ConditionExpression[] { condCliente };
                qryExp.ColumnSet = cs;

                BusinessEntityCollection becProdPas = Servicio.RetrieveMultiple(qryExp);

                if (becProdPas.BusinessEntities.Length > 0)
                {
                    Guid idProdCore;
                    Guid prodPasIdTipoProd;
                    efk_producto_pasivo efkPPas;

                    Producto objProducto = new Producto(Credenciales);
                    bool founded = false;
                    int i = 0;

                    while (!founded && i < becProdPas.BusinessEntities.Length)
                    {
                        efkPPas = (efk_producto_pasivo)becProdPas.BusinessEntities[i];
                        if (efkPPas.efk_producto_core_id != null)
                        {
                            idProdCore = efkPPas.efk_producto_core_id.Value;
                            //obtener el id tipo producto correspondiente al producto banco
                            prodPasIdTipoProd = objProducto.GetTipoProductoFromProductoBanco(idProdCore);
                            if (prodPasIdTipoProd == idTipoProducto)
                            {
                                founded = true;
                            }
                        }
                        i++;
                    }
                    return founded;
                }
                else
                    return false;
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(ex.Detail.InnerXml);
                throw new Exception(xmlDoc.FirstChild.ChildNodes[1].InnerText);
            }
            catch
            {
                throw;
            }
        }


        public bool TieneSubTipoProductoActivo(Guid idCliente, Guid idSubtipoProducto)
        {
            try
            {
                ConditionExpression condCliente = new ConditionExpression();
                condCliente.AttributeName = "efk_cliente_juridico_id";
                condCliente.Operator = ConditionOperator.Equal;
                condCliente.Values = new string[] { idCliente.ToString() };

                ColumnSet cs = new ColumnSet();
                cs.Attributes = new string[] { "efk_producto_core_id" };

                QueryExpression qryExp = new QueryExpression();
                qryExp.EntityName = EntityName.efk_producto_activo.ToString();
                qryExp.Criteria = new FilterExpression();
                qryExp.Criteria.Conditions = new ConditionExpression[] { condCliente };
                qryExp.ColumnSet = cs;

                BusinessEntityCollection becProdAct = Servicio.RetrieveMultiple(qryExp);

                if (becProdAct.BusinessEntities.Length > 0)
                {
                    Guid idProdCore;
                    Guid prodActIdSubtipoProd;
                    efk_producto_activo efkPAct;

                    Producto objProducto = new Producto(Credenciales);
                    bool founded = false;
                    int i = 0;

                    while (!founded && i < becProdAct.BusinessEntities.Length)
                    {
                        efkPAct = (efk_producto_activo)becProdAct.BusinessEntities[i];
                        if (efkPAct.efk_producto_core_id != null)
                        {
                            idProdCore = efkPAct.efk_producto_core_id.Value;
                            //obtener el id subtipo producto correspondiente al producto banco
                            prodActIdSubtipoProd = objProducto.GetSubtipoProductoFromProductoBanco(idProdCore);
                            if (prodActIdSubtipoProd == idSubtipoProducto)
                            {
                                founded = true;
                            }
                        }
                        i++;
                    }
                    return founded;
                }
                else
                    return false;
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(ex.Detail.InnerXml);
                throw new Exception(xmlDoc.FirstChild.ChildNodes[1].InnerText);
            }
            catch
            {
                throw;
            }
        }


        public bool TieneSubTipoProductoPasivo(Guid idCliente, Guid idSubtipoProducto)
        {
            try
            {
                ConditionExpression condCliente = new ConditionExpression();
                condCliente.AttributeName = "efk_cliente_juridico_id";
                condCliente.Operator = ConditionOperator.Equal;
                condCliente.Values = new string[] { idCliente.ToString() };

                ColumnSet cs = new ColumnSet();
                cs.Attributes = new string[] { "efk_producto_core_id" };

                QueryExpression qryExp = new QueryExpression();
                qryExp.EntityName = EntityName.efk_producto_pasivo.ToString();
                qryExp.Criteria = new FilterExpression();
                qryExp.Criteria.Conditions = new ConditionExpression[] { condCliente };
                qryExp.ColumnSet = cs;

                BusinessEntityCollection becProdPas = Servicio.RetrieveMultiple(qryExp);

                if (becProdPas.BusinessEntities.Length > 0)
                {
                    Guid idProdCore;
                    Guid prodPasIdSubtipoProd;
                    efk_producto_pasivo efkPPas;

                    Producto objProducto = new Producto(Credenciales);
                    bool founded = false;
                    int i = 0;

                    while (!founded && i < becProdPas.BusinessEntities.Length)
                    {
                        efkPPas = (efk_producto_pasivo)becProdPas.BusinessEntities[i];
                        if (efkPPas.efk_producto_core_id != null)
                        {
                            idProdCore = efkPPas.efk_producto_core_id.Value;
                            //obtener el id tipo producto correspondiente al producto banco
                            prodPasIdSubtipoProd = objProducto.GetSubtipoProductoFromProductoBanco(idProdCore);
                            if (prodPasIdSubtipoProd == idSubtipoProducto)
                            {
                                founded = true;
                            }
                        }
                        i++;
                    }
                    return founded;
                }
                else
                    return false;
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(ex.Detail.InnerXml);
                throw new Exception(xmlDoc.FirstChild.ChildNodes[1].InnerText);
            }
            catch
            {
                throw;
            }
        }
        
    }
}
