using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services.Protocols;
using Efika.Crm.AccesoServicios.CRMSDK;
using Efika.Crm.Entidades;
using Efika.Crm.AccesoServicios;

namespace Efika.Crm.Negocio
{
    public class ParametroSimulacionCrediticia
    {
        private CrmService Servicio;
        private CredencialesCRM credenciales;

        #region Parametros
        public const string PARAMETRO_CANT_DIAS_ANTIGUEDAD_DATOS_SCORING = "cant_dias_antiguedad_datos_scoring";
        public const string PARAMETRO_CUOTA_MIN_SOBRANTE = "cuota_min_sobrante";
        public const string PARAMETRO_NUMERO_OFERTA_INICIAL = "numero_oferta_inicial";
        public const string PARAMETRO_NUMERO_SOLICITUD_INICIAL = "numero_solicitud_inicial";
        public const string PARAMETRO_TASA_CESANTIA = "tasa_cesantia";
        public const string PARAMETRO_TASA_DESGRAVAMEN = "tasa_desgravamen";
        public const string PARAMETRO_TASA_PAG_MIN = "tasa_pag_min";
        #endregion

        public ParametroSimulacionCrediticia(CredencialesCRM credenciales)
        {
            Servicio = ServicioCRM.ObtenerServicioCRM(credenciales);
            this.credenciales = credenciales;
        }

        public efk_paramtero_simulacion_crediticia GetParametroByName(string nameParameter)
        {
            try
            {
                ColumnSet resultSetColumns = new ColumnSet();

                resultSetColumns.Attributes = new string[] 
            { 
                "efk_name", 
                "efk_valor_decimal", 
                "efk_valor_entero" ,
                "efk_paramtero_simulacion_crediticiaid"
            };

                return GetParametroByName(nameParameter, resultSetColumns);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public efk_paramtero_simulacion_crediticia GetParametroByName(string nameParameter, ColumnSet columnSet)
        {
            try
            {
                efk_paramtero_simulacion_crediticia rvalue;

                Efika.Crm.AccesoServicios.CRMSDK.ConditionExpression ConditionExp = new Efika.Crm.AccesoServicios.CRMSDK.ConditionExpression();
                ConditionExp.AttributeName = "efk_name";
                ConditionExp.Operator = Efika.Crm.AccesoServicios.CRMSDK.ConditionOperator.Equal;
                ConditionExp.Values = new string[] { nameParameter };

                Efika.Crm.AccesoServicios.CRMSDK.QueryExpression queryExp = new Efika.Crm.AccesoServicios.CRMSDK.QueryExpression();
                queryExp.EntityName = EntityName.efk_paramtero_simulacion_crediticia.ToString();
                queryExp.Criteria = new Efika.Crm.AccesoServicios.CRMSDK.FilterExpression();
                queryExp.Criteria.Conditions = new Efika.Crm.AccesoServicios.CRMSDK.ConditionExpression[] { ConditionExp };
                queryExp.ColumnSet = columnSet;

                BusinessEntityCollection beParametroSolicitudCredito = Servicio.RetrieveMultiple(queryExp);

                rvalue = null;

                foreach (BusinessEntity entity in beParametroSolicitudCredito.BusinessEntities)
                {
                    rvalue = (efk_paramtero_simulacion_crediticia)entity;

                }

                return rvalue;
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



    }



}
