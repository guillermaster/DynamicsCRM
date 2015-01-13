using System;
using System.Collections.Generic;
using System.Text;
using Efika.Crm.AccesoServicios.CRMSDK;
using Efika.Crm.AccesoServicios;
using Efika.Crm.Entidades;
using System.Collections;
using System.Data;

namespace Efika.Crm.Negocio
{
    public class ProductosPasivoBMSC
    {
        /// <summary>
        /// Obtiene los productos del pasivo asociados al cliente indicado
        /// </summary>
        /// <param name="credenciales"></param>
        /// <param name="p_strAccountId">Guid del cliente</param>
        /// <param name="p_intCodigoCliente">Código del cliente en el banco</param>
        /// <returns></returns>
        private BusinessEntityCollection ObtenerProductoPasivo(CredencialesCRM credenciales, string p_strAccountId, int p_intCodigoCliente)
        {
            CrmService Servicio = ServicioCRM.ObtenerServicioCRM(credenciales);

            try
            {
                ConditionExpression stateCondition = new ConditionExpression();
                stateCondition.AttributeName = "efk_cliente_juridico_id";
                stateCondition.Operator = ConditionOperator.Equal;
                stateCondition.Values = new string[] { p_strAccountId };

                FilterExpression outerFilter = new FilterExpression();
                outerFilter.FilterOperator = LogicalOperator.And;
                outerFilter.Conditions = new ConditionExpression[] { stateCondition };

                ColumnSet resultSetColumns = new ColumnSet();
                resultSetColumns.Attributes = new string[] { "efk_producto_pasivoid", "efk_numero_cuenta", "efk_codigo_producto" };
                QueryExpression qryExpression = new QueryExpression();
                qryExpression.Criteria = outerFilter;
                qryExpression.ColumnSet = resultSetColumns;

                qryExpression.EntityName = EntityName.efk_producto_pasivo.ToString();
                qryExpression.Distinct = false;

                BusinessEntityCollection ProductoPasivoResultSet = Servicio.RetrieveMultiple(qryExpression);

                return ProductoPasivoResultSet;
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                throw new Exception(ex.Detail.InnerText);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                Servicio.Dispose();
            }
        }

        /// <summary>
        /// Proceso que actualiza el saldo de las cuentas de pasivo del cliente indicado
        /// </summary>
        /// <param name="credenciales"></param>
        /// <param name="p_strAccountId"Guid del cliente></param>
        /// <param name="p_intCodigoCliente">Código del cliente en el banco</param>
        /// <param name="prodsnuevosaldo">Arreglo que contiene las cuentas con los nuevos saldos</param>
        /// <returns></returns>
        public bool ActualizarProductoCliente(CredencialesCRM credenciales, string p_strAccountId, int p_intCodigoCliente, ArrayList prodsnuevosaldo, string p_strTipoCuenta)
        {
            bool blRespuesta = false;
            try
            {
                if (p_strAccountId != string.Empty && p_intCodigoCliente != 0)
                {
                    BusinessEntityCollection ProductoPasivoResultSet = this.ObtenerProductoPasivo(credenciales, p_strAccountId, p_intCodigoCliente);
                    if (ProductoPasivoResultSet.BusinessEntities.Length > 0)
                    {
                        blRespuesta = this.ActualizarSaldoProducto(ProductoPasivoResultSet, credenciales, prodsnuevosaldo, p_strTipoCuenta);
                    }
                }
                blRespuesta = true;
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                blRespuesta = false;
                return blRespuesta;
                throw new Exception(ex.Detail.InnerText);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            return blRespuesta;
        }

        /// <summary>
        /// Función que actualiza los saldos de las cuentas en el CRM
        /// </summary>
        /// <param name="p_becProductoPasivo"></param>
        /// <param name="credenciales"></param>
        /// <param name="prodsnuevosaldo"></param>
        /// <returns></returns>
        private bool ActualizarSaldoProducto(BusinessEntityCollection p_becProductoPasivo, CredencialesCRM credenciales, ArrayList prodsnuevosaldo, string p_strTipoCuenta)
        {
            bool blRespuesta = false;

            CrmService Servicio = ServicioCRM.ObtenerServicioCRM(credenciales);
            try
            {
                //recorremos cada uno de los productos del cliente
                foreach (efk_producto_pasivo eProductoPasivo in p_becProductoPasivo.BusinessEntities)
                {
                    for (int i = 0; i < prodsnuevosaldo.Count; i++)
                    {
                        ProductosPasivo prod = (ProductosPasivo)prodsnuevosaldo[i];
                        if (eProductoPasivo.efk_numero_cuenta.Trim().Equals(prod.NumeroCuenta.Trim()))
                        {
                            //Evaluamos según el tipo de cuenta, sólo actualizamos Cuentas de Ahorro y Corrientes
                            if (prod.TipoCuenta.Substring(0, 4) == p_strTipoCuenta)
                            {
                                //Actualizamos el saldo contable y disponible
                                efk_producto_pasivo prod_pasivo = new efk_producto_pasivo();
                                prod_pasivo.efk_producto_pasivoid = new Key();
                                prod_pasivo.efk_producto_pasivoid.Value = eProductoPasivo.efk_producto_pasivoid.Value;

                                prod_pasivo.efk_saldo_contable_actual = new CrmMoney();
                                prod_pasivo.efk_saldo_contable_actual.Value = prod.SaldoContable;

                                prod_pasivo.efk_saldo_disponible = new CrmMoney();
                                prod_pasivo.efk_saldo_disponible.Value = prod.SaldoDisponible;

                                Efika.Crm.Negocio.Divisa eDivisa = new Efika.Crm.Negocio.Divisa(credenciales);
                                Entidades.Divisa objDivisa = eDivisa.ConsultaDivisa(prod.Moneda);                             
                                prod_pasivo.transactioncurrencyid = new Lookup();
                                prod_pasivo.transactioncurrencyid.Value = objDivisa.transactionCurrencyId;                                   
                                
                                Servicio.Update(prod_pasivo);
                            }
                            break;
                        }
                    }
                }
                blRespuesta = true;
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                blRespuesta = false;
                throw new Exception(ex.Detail.InnerText);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                Servicio.Dispose();
            }
            return blRespuesta;
        }
    }
}
