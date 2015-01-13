using System;
using System.Collections.Generic;
using System.Data;
using Efika.Crm.AccesoServicios.CRMSDK;
using Efika.Crm.AccesoServicios.CRMMETADATA;
using Efika.Crm.AccesoServicios;
using Efika.Crm.Entidades;
using Microsoft.Xrm.Sdk;

namespace Efika.Crm.Negocio
{
    public class Divisa
    {
        private CrmService Servicio;

        public Divisa(CredencialesCRM credenciales)
        {
            Servicio = ServicioCRM.ObtenerServicioCRM(credenciales);
        }

        public DataTable ConsultaDivisas()
        {
            try
            {
                DataTable dtDivisas;
                #region Inicializar datatable
                dtDivisas = new DataTable();
                dtDivisas.Columns.Add("transactioncurrencyid");
                dtDivisas.Columns.Add("Nombre de divisa");
                dtDivisas.Columns.Add("Código de divisa");
                dtDivisas.Columns.Add("Símbolo de moneda");
                #endregion

                /*Columnas a retornar*/
                ColumnSet cs = new ColumnSet();
                cs.Attributes = new string[] { "transactioncurrencyid", "isocurrencycode", "currencyname", "currencysymbol" };

                QueryExpression qryExp = new QueryExpression();
                qryExp.EntityName = EntityName.transactioncurrency.ToString();
                qryExp.ColumnSet = cs;

                BusinessEntityCollection beDivisas = Servicio.RetrieveMultiple(qryExp);

                foreach (BusinessEntity beDivisa in beDivisas.BusinessEntities)
                {
                    transactioncurrency entDivisa = (transactioncurrency)beDivisa;
                    DataRow drDivisa = dtDivisas.NewRow();
                    drDivisa[0] = entDivisa.transactioncurrencyid.Value;
                    drDivisa[1] = entDivisa.currencyname;
                    drDivisa[2] = entDivisa.isocurrencycode;
                    drDivisa[3] = entDivisa.currencysymbol;

                    dtDivisas.Rows.Add(drDivisa);
                }
                return dtDivisas;
            }
            catch (System.Web.Services.Protocols.SoapException ex)
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


        public Entidades.Divisa ConsultaDivisa(Guid idDivisa)
        {
            try
            {
                /*Columnas a retornar*/
                ColumnSet cs = new ColumnSet();
                cs.Attributes = new string[] { "transactioncurrencyid", "isocurrencycode", "currencyname", "currencysymbol", "exchangerate" };

                BusinessEntity beDivisa = Servicio.Retrieve(EntityName.transactioncurrency.ToString(), idDivisa, cs);
                transactioncurrency entDivisa = (transactioncurrency)beDivisa;

                Entidades.Divisa objDivisa = new Entidades.Divisa();
                objDivisa.transactionCurrencyId = entDivisa.transactioncurrencyid.Value;
                objDivisa.isoCurrencyCode = entDivisa.isocurrencycode;
                objDivisa.currencyName = entDivisa.currencyname;
                objDivisa.currencySymbol = entDivisa.currencysymbol;
                objDivisa.ExchangeRate = entDivisa.exchangerate.Value;

                return objDivisa;
            }
            catch (System.Web.Services.Protocols.SoapException ex)
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

        public Entidades.Divisa ConsultaDivisa(string codIsoDivisa)
        {
            try
            {
                Entidades.Divisa objDivisa = new Entidades.Divisa();
                /*Columnas a retornar*/
                ColumnSet cs = new ColumnSet();
                cs.Attributes = new string[] { "transactioncurrencyid", "isocurrencycode", "currencyname", "currencysymbol", "exchangerate" };

                ConditionExpression ConditionExp = new ConditionExpression();
                ConditionExp.AttributeName = "isocurrencycode";
                ConditionExp.Operator = Efika.Crm.AccesoServicios.CRMSDK.ConditionOperator.Equal;
                ConditionExp.Values = new string[] { codIsoDivisa };  

                QueryExpression qryExp = new QueryExpression();
                qryExp.EntityName = EntityName.transactioncurrency.ToString();
                qryExp.ColumnSet = cs;
                qryExp.Criteria = new FilterExpression();
                qryExp.Criteria.Conditions = new ConditionExpression[] { ConditionExp };

                BusinessEntityCollection beDivisas = Servicio.RetrieveMultiple(qryExp);

                if (beDivisas.BusinessEntities.Length > 0)
                {
                    transactioncurrency entDivisa = (transactioncurrency)beDivisas.BusinessEntities[0];                    
                    objDivisa.transactionCurrencyId = entDivisa.transactioncurrencyid.Value;
                    objDivisa.isoCurrencyCode = entDivisa.isocurrencycode;
                    objDivisa.currencyName = entDivisa.currencyname;
                    objDivisa.currencySymbol = entDivisa.currencysymbol;
                    objDivisa.ExchangeRate = entDivisa.exchangerate.Value;
                }
                
                return objDivisa;
            }
            catch (System.Web.Services.Protocols.SoapException ex)
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
    }
}
