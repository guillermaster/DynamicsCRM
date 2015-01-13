using System;
using System.Collections.Generic;
using System.Text;
using Efika.Crm.AccesoServicios.CRMSDK;
using Efika.Crm.AccesoServicios;
using Efika.Crm.Entidades;
using System.Collections;
using System.Linq;
using System.Data.SqlClient;
using System.Data;


namespace Efika.Crm.Negocio
{
    public class Segmentacion
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="credenciales"></param>
        /// <param name="codigosegmento"></param>
        /// <returns></returns>
        private static Guid? ObtineGuidSegmento(CredencialesCRM credenciales, string codigosegmento)
        {
            CrmService Servicio = ServicioCRM.ObtenerServicioCRM(credenciales);
            Guid? segmentoid = null;
            try
            {
                ConditionExpression stateCondition = new ConditionExpression();
                stateCondition.AttributeName = "efk_codigo";
                stateCondition.Operator = ConditionOperator.Equal;
                stateCondition.Values = new string[] { codigosegmento.PadLeft(4,'0') };

                FilterExpression outerFilter = new FilterExpression();
                outerFilter.FilterOperator = LogicalOperator.And;
                outerFilter.Conditions = new ConditionExpression[] { stateCondition };

                ColumnSet resultSetColumns = new ColumnSet();
                resultSetColumns.Attributes = new string[] { "efk_segmentoid" };
                QueryExpression qryExpression = new QueryExpression();
                qryExpression.Criteria = outerFilter;
                qryExpression.ColumnSet = resultSetColumns;

                qryExpression.EntityName = EntityName.efk_segmento.ToString();
                qryExpression.Distinct = false;

                BusinessEntityCollection segmentoResultSet = Servicio.RetrieveMultiple(qryExpression);
                foreach (efk_segmento eSegmento in segmentoResultSet.BusinessEntities)           
                    segmentoid = eSegmento.efk_segmentoid.Value;
                
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
            return segmentoid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="credenciales"></param>
        /// <param name="p_strAccountId"></param>
        /// <param name="p_intCodigoCliente"></param>
        /// <param name="ingreso"></param>
        /// <param name="tipoingreso"></param>
        /// <param name="codigosegmento"></param>
        /// <returns></returns>
        public static bool ActualizarSegmentoCliente(CredencialesCRM credenciales, string sp, string cadenaconexion, string p_strAccountId, 
            int p_intCodigoCliente, decimal ingreso, decimal tipoingreso, string codigosegmento, int? edad, bool? tienehijos, int? estadocivil)
        {
            bool blRespuesta = false;
            CrmService Servicio = ServicioCRM.ObtenerServicioCRM(credenciales);
            try
            {
                if (codigosegmento != null)
                {
                    Guid? id = ObtineGuidSegmento(credenciales, codigosegmento);
                    if (id != null)
                    {
                        // Actualizar el campo código de segmento
                        account cliente = new account();
                        cliente.accountid = new Key();
                        cliente.accountid.Value = new Guid(p_strAccountId);
                        cliente.efk_segmento_ovid = new Lookup();
                        cliente.efk_segmento_ovid.Value = id.Value;
                        Servicio.Update(cliente);
                        //blRespuesta = CalculaOfertaValor(cadenaconexion, sp, new Guid(p_strAccountId), edad, tienehijos, estadocivil);                        
                        blRespuesta = CalculaOfertaValorMicro(cadenaconexion, sp, new Guid(p_strAccountId));
                    }
                }
                
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cadenaConexion"></param>
        /// <param name="idcliente"></param>
        /// <param name="edad"></param>
        /// <param name="tienehijos"></param>
        /// <param name="estadocivil"></param>
        /// <returns></returns>
        private static bool CalculaOfertaValor(string cadenaConexion, string sp, Guid? idcliente, int? edad, bool? tienehijos, int? estadocivil)
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


                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@idcliente", SqlDbType.UniqueIdentifier));
                        MyDataAdapter.SelectCommand.Parameters["@idcliente"].Value = idcliente;

                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@iEdad", SqlDbType.Int));
                        MyDataAdapter.SelectCommand.Parameters["@iEdad"].Value = edad;

                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@bTieneHijos", SqlDbType.Bit));
                        MyDataAdapter.SelectCommand.Parameters["@bTieneHijos"].Value = tienehijos;

                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@iEstadoCivil", SqlDbType.Int));
                        MyDataAdapter.SelectCommand.Parameters["@iEstadoCivil"].Value = estadocivil;

                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@error", SqlDbType.VarChar, 200));
                        MyDataAdapter.SelectCommand.Parameters["@error"].Value = null;
                        MyDataAdapter.SelectCommand.Parameters["@error"].Direction = ParameterDirection.Output;

                        ///rellenar data set y dataadapter
                        DataSet DS = new DataSet();
                        MyDataAdapter.Fill(DS, "Resultado");///                         
                        if (MyDataAdapter.SelectCommand.Parameters[4].Value is System.DBNull)
                             error = "";
                        else                             
                            error = (string)MyDataAdapter.SelectCommand.Parameters[4].Value;

                        if (error == "")
                            return true;
                        else
                            return false;
                    }
                    else
                    {
                        return false;
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


        private static bool CalculaOfertaValorMicro(string cadenaConexion, string sp, Guid? idcliente)
        {
            SqlConnection sqlconnection = null;
            string error;
            try
            {
                using (sqlconnection = new SqlConnection(cadenaConexion))
                {
                    sqlconnection.Open();
                    if (sqlconnection.State == System.Data.ConnectionState.Open)
                    {
                        SqlDataAdapter MyDataAdapter = new SqlDataAdapter(sp, sqlconnection);
                        MyDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        
                        MyDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@idcliente", SqlDbType.UniqueIdentifier));
                        MyDataAdapter.SelectCommand.Parameters["@idcliente"].Value = idcliente;

                        return true;
                    }
                    else
                    {
                        return false;
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
                sqlconnection.Close();
            }
        }
        
    }
}
