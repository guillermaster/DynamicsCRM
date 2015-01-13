namespace Efika.Crm.Negocio
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Efika.Crm.AccesoServicios.CRMSDK;
    using Efika.Crm.AccesoServicios;
    using Efika.Crm.Entidades;
    using Efika.Crm.Entidades.ScoreEvaluate;
    using Efika.Crm.Negocio.Common;
    using System.Reflection;
    using System.Web.Services.Protocols;
    using Efika.Crm.Entidades.TipoCambio;

    public class FotoSolicitudCredito
    {
        private CrmService Servicio = new CrmService();
        private CredencialesCRM credenciales;

        private const int TIPO_RELACION_CONYUGUE = 221220000;
        private const int TIPO_RELACION_UNION_LIBRE = 221220001;
        private const int TIPO_RELACION_OTRO = 221220002;

        private const int TIPO_SCORE_REFERENCIAL = 221220000;
        private const int TIPO_SCORE_DEFINITIVO = 221220001;

        public FotoSolicitudCredito(CredencialesCRM credenciales)
        {
            Servicio = ServicioCRM.ObtenerServicioCRM(credenciales);
            this.credenciales = credenciales;
        }


        public efk_foto_solicitud_credito GetFotoSolicitudCreditoById(Guid fotoSolicitudID, ColumnSet columns)
        {
            efk_foto_solicitud_credito fotoSolicitud = new efk_foto_solicitud_credito();
            BusinessEntity beFotoSolicitud = Servicio.Retrieve(EntityName.efk_foto_solicitud_credito.ToString(), fotoSolicitudID, columns);

            fotoSolicitud = (efk_foto_solicitud_credito)beFotoSolicitud;

            return fotoSolicitud;
        }

        public efk_foto_solicitud_credito GetFotoSolicitudCreditoByNroSolicitud(string nroSolicitud, ColumnSet columns)
        {
            BusinessEntityCollection be_fotoSolicitud;
            efk_foto_solicitud_credito fotoSolicitud = new efk_foto_solicitud_credito();

            ConditionExpression condExpFS = new ConditionExpression();
            condExpFS.AttributeName = "efk_nrosolicitud";
            condExpFS.Operator = ConditionOperator.Equal;
            condExpFS.Values = new string[] { nroSolicitud };

            QueryExpression qryExp = new QueryExpression();
            qryExp.EntityName = EntityName.efk_foto_solicitud_credito.ToString();
            qryExp.Criteria = new FilterExpression();
            qryExp.Criteria.Conditions = new ConditionExpression[] { condExpFS };
            qryExp.ColumnSet = columns;

            try
            {
                be_fotoSolicitud = Servicio.RetrieveMultiple(qryExp);//retorna instancias de datos importados de campaña

                fotoSolicitud = (efk_foto_solicitud_credito)be_fotoSolicitud.BusinessEntities[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return fotoSolicitud;
        }

        public TipoCambioResult GetTipoCambioByNroSolicitud(string nroSolicitud)
        {
            TipoCambioResult tcr = new TipoCambioResult(1, 1, 1, 0, string.Empty);
            efk_foto_solicitud_credito fotoSolicitud = new efk_foto_solicitud_credito();
            ColumnSet columnas = new ColumnSet();
            columnas.Attributes = new string[] { "efk_tipo_cambio_compra", "efk_tipo_cambio_contable", "efk_tipo_cambio_venta" };

            fotoSolicitud = GetFotoSolicitudCreditoByNroSolicitud(nroSolicitud, columnas);

            if (fotoSolicitud != null)
            {
                tcr.TCCompra = (fotoSolicitud.efk_tipo_cambio_compra == null ? 1 : fotoSolicitud.efk_tipo_cambio_compra.Value);
                tcr.TCContable = (fotoSolicitud.efk_tipo_cambio_contable == null ? 1 : fotoSolicitud.efk_tipo_cambio_contable.Value);
                tcr.TCVenta = (fotoSolicitud.efk_tipo_cambio_venta == null ? 1 : fotoSolicitud.efk_tipo_cambio_venta.Value);
            }

            return tcr;
        }


        public string CreateUpdateFotoSolicitudCredito(MsgResponseScoreEvaluate msgWSResponse)
        {
            string resultado;
            string fotoSolicitudCreditoId;
            bool createFotoSolicitud;
            createFotoSolicitud = true;
            resultado = null;
            fotoSolicitudCreditoId = string.Empty;

            try
            {
                createFotoSolicitud = ExistsFotoSolicitudCredito(msgWSResponse.OpportunityID, ref fotoSolicitudCreditoId);
                efk_foto_solicitud_credito fotoSolicitud = new efk_foto_solicitud_credito();

                fotoSolicitud.efk_name = "Foto solicitud credito - Solicitud Nro. " + msgWSResponse.NroSolcicitud.ToString();

                fotoSolicitud.efk_clienteid = new Lookup();
                fotoSolicitud.efk_clienteid.Value = msgWSResponse.ClientID;

                fotoSolicitud.efk_opportunityid = new Lookup();
                fotoSolicitud.efk_opportunityid.Value = msgWSResponse.OpportunityID;


                fotoSolicitud.efk_nrosolicitud = new CrmNumber();
                fotoSolicitud.efk_nrosolicitud.Value = msgWSResponse.NroSolcicitud;

                fotoSolicitud.efk_clienteid = new Lookup();
                fotoSolicitud.efk_clienteid.Value = msgWSResponse.ClientID;

                fotoSolicitud.efk_orden_oportunidad = new CrmNumber();
                fotoSolicitud.efk_orden_oportunidad.Value = msgWSResponse.OpportunityOrden;

                fotoSolicitud.efk_numero_oferta = new CrmNumber();
                fotoSolicitud.efk_numero_oferta.Value = msgWSResponse.NumeroOferta;

                fotoSolicitud.efk_puntaje_score = new CrmDecimal();
                fotoSolicitud.efk_puntaje_score.Value = Utility.ConvertStringToDecimal(GetScoreEvaluateResult(
                    ScoreEvaluateResult.WSRESULT_SCORE, msgWSResponse.Result));
                fotoSolicitud.efk_puntaje_score_sugerencia = GetScoreEvaluateResult(
                    ScoreEvaluateResult.WSRESULT_SCORE_SUGERENCIA, msgWSResponse.Result);
                fotoSolicitud.efk_puntaje_score_recomendacion = GetScoreEvaluateResult(
                    ScoreEvaluateResult.WSRESULT_SCORE_RECOMENDACION, msgWSResponse.Result);

                fotoSolicitud.efk_color = GetScoreEvaluateResult(ScoreEvaluateResult.WSRESULT_COLOR, msgWSResponse.Result);

                fotoSolicitud.efk_limites_comerciales_garantia_hipotecaria = new CrmMoney();
                fotoSolicitud.efk_limites_comerciales_garantia_hipotecaria.Value = Utility.ConvertStringToDecimal(
                    GetScoreEvaluateResult(ScoreEvaluateResult.WSRESULT_LIMITE_GARANTIA_HIPOTECARIA, msgWSResponse.Result));
                fotoSolicitud.efk_limites_garantia_hipotecaria_sugerencia = GetScoreEvaluateResult(
                    ScoreEvaluateResult.WSRESULT_LIMITE_GARANTIA_HIPOTECARIA_SUGERENCIA, msgWSResponse.Result);

                fotoSolicitud.efk_limites_comerciales_garantiapersona = new CrmMoney();
                fotoSolicitud.efk_limites_comerciales_garantiapersona.Value = Utility.ConvertStringToDecimal(
                    GetScoreEvaluateResult(ScoreEvaluateResult.WSRESULT_LIMITE_GARANTIA_PERSONAL, msgWSResponse.Result));
                fotoSolicitud.efk_limites_garantia_persona_sugerencia = GetScoreEvaluateResult(
                    ScoreEvaluateResult.WSRESULT_LIMITE_GARANTIA_PERSONAL_SUGERENCIA, msgWSResponse.Result);

                fotoSolicitud.efk_limites_comerciales_garantia_asolafirma = new CrmMoney();
                fotoSolicitud.efk_limites_comerciales_garantia_asolafirma.Value = Utility.ConvertStringToDecimal(
                    GetScoreEvaluateResult(ScoreEvaluateResult.WSRESULT_LIMITE_A_SOLA_FIRMA, msgWSResponse.Result));
                fotoSolicitud.efk_limites_garantia_asolafirma_sugerencia = GetScoreEvaluateResult(
                    ScoreEvaluateResult.WSRESULT_LIMITE_A_SOLA_FIRMA_SUGERENCIA, msgWSResponse.Result);

                fotoSolicitud.efk_saldo_caja = new CrmMoney();
                fotoSolicitud.efk_saldo_caja.Value = Utility.ConvertStringToDecimal(GetScoreEvaluateResult(ScoreEvaluateResult.WSRESULT_SALDO_CAJA_LIBRE, msgWSResponse.Result));
                fotoSolicitud.efk_saldo_caja_sugerencia = GetScoreEvaluateResult(
                    ScoreEvaluateResult.WSRESULT_SALDO_CAJA_LIBRE_SUGERENCIA, msgWSResponse.Result);

                fotoSolicitud.efk_ptos_edad = new CrmDecimal();
                fotoSolicitud.efk_ptos_edad.Value = Utility.ConvertStringToDecimal(GetScoreEvaluateResult(
                    ScoreEvaluateResult.WSRESULT_CLIENTE, msgWSResponse.Result));

                fotoSolicitud.efk_ptos_cantidad_dependientes = new CrmDecimal();
                fotoSolicitud.efk_ptos_cantidad_dependientes.Value = Utility.ConvertStringToDecimal(
                    GetScoreEvaluateResult(ScoreEvaluateResult.WSRESULT_CANTIDAD_DE_DEPENDIENTES, msgWSResponse.Result));

                fotoSolicitud.efk_ptos_peor_calificacion_dependientes = new CrmDecimal();
                fotoSolicitud.efk_ptos_peor_calificacion_dependientes.Value = Utility.ConvertStringToDecimal(
                    GetScoreEvaluateResult(ScoreEvaluateResult.WSRESULT_PEOR_CALIFICACION_SISTEMA_12_MESES, msgWSResponse.Result));

                fotoSolicitud.efk_ptos_veces_calificacion_distinta_a12m_sf = new CrmDecimal();
                fotoSolicitud.efk_ptos_veces_calificacion_distinta_a12m_sf.Value = Utility.ConvertStringToDecimal(
                    GetScoreEvaluateResult(ScoreEvaluateResult.WSRESULT_NRO_VECES_CALIFICACION_DISTINTA_DE_A_EN12MESES, msgWSResponse.Result));

                fotoSolicitud.efk_ptos_veces_por_estado_12m_sf = new CrmDecimal();
                fotoSolicitud.efk_ptos_veces_por_estado_12m_sf.Value = Utility.ConvertStringToDecimal(
                    GetScoreEvaluateResult(ScoreEvaluateResult.WSRESULT_NRO_VECES_POR_ESTADO_EN_12_MESES_SISTEMA, msgWSResponse.Result));

                fotoSolicitud.efk_ptosvecesclienteestado14dmasvgnt2_12mbmsc = new CrmDecimal();
                fotoSolicitud.efk_ptosvecesclienteestado14dmasvgnt2_12mbmsc.Value = Utility.ConvertStringToDecimal(
                    GetScoreEvaluateResult(ScoreEvaluateResult.WSRESULT_NRO_VECES_CLIENTE_ESTA_MAS_DE_14_DIAS_EN_VIGENTE2_BMSC, msgWSResponse.Result));

                fotoSolicitud.efk_ptos_garantias_constituidas = new CrmDecimal();
                fotoSolicitud.efk_ptos_garantias_constituidas.Value = Utility.ConvertStringToDecimal(
                    GetScoreEvaluateResult(ScoreEvaluateResult.WSRESULT_GARANTIAS_CONTITUIDAS, msgWSResponse.Result));

                fotoSolicitud.efk_ptos_garantias_credito_solicitud = new CrmDecimal();
                fotoSolicitud.efk_ptos_garantias_credito_solicitud.Value = Utility.ConvertStringToDecimal(
                    GetScoreEvaluateResult(ScoreEvaluateResult.WSRESULT_GARANTIAS_CREDITO_SOLICITADO, msgWSResponse.Result));

                fotoSolicitud.efk_ptos_riesgo_indirecto_titular = new CrmDecimal();
                fotoSolicitud.efk_ptos_riesgo_indirecto_titular.Value = Utility.ConvertStringToDecimal(
                    GetScoreEvaluateResult(ScoreEvaluateResult.WSRESULT_RIESGO_INDIRECTO_TITULAR, msgWSResponse.Result));

                fotoSolicitud.efk_ptos_riesgo_indirecto_garante = new CrmDecimal();
                fotoSolicitud.efk_ptos_riesgo_indirecto_garante.Value = Utility.ConvertStringToDecimal(
                    GetScoreEvaluateResult(ScoreEvaluateResult.WSRESULT_RIESGO_INDIRECTO_GARANTE, msgWSResponse.Result));

                fotoSolicitud.efk_ptos_flujo_caja_negativo = new CrmDecimal();
                fotoSolicitud.efk_ptos_flujo_caja_negativo.Value = Utility.ConvertStringToDecimal(GetScoreEvaluateResult(
                    ScoreEvaluateResult.WSRESULT_FLUJO_DE_CAJA_NEGATIVO, msgWSResponse.Result));

                fotoSolicitud.efk_ptos_diferencia_cuota_maxima_negativa = new CrmDecimal();
                fotoSolicitud.efk_ptos_diferencia_cuota_maxima_negativa.Value = Utility.ConvertStringToDecimal(GetScoreEvaluateResult(
                    ScoreEvaluateResult.WSRESULT_DIFERENCIA_DE_CUOTA_MAXIMA_NEGATIVA, msgWSResponse.Result));

                fotoSolicitud.efk_ptos_subparametrob = new CrmDecimal();
                fotoSolicitud.efk_ptos_subparametrob.Value = Utility.ConvertStringToDecimal(GetScoreEvaluateResult(
                    ScoreEvaluateResult.WSRESULT_SUBPARAMETRO_B, msgWSResponse.Result));

                fotoSolicitud.efk_ptos_subparametroc = new CrmDecimal();
                fotoSolicitud.efk_ptos_subparametroc.Value = Utility.ConvertStringToDecimal(GetScoreEvaluateResult(
                    ScoreEvaluateResult.WSRESULT_SUBPARAMETRO_C, msgWSResponse.Result));

                fotoSolicitud.efk_ptos_factor_cliente = new CrmDecimal();
                fotoSolicitud.efk_ptos_factor_cliente.Value = Utility.ConvertStringToDecimal(GetScoreEvaluateResult(
                    ScoreEvaluateResult.WSRESULT_CLIENTE, msgWSResponse.Result));

                fotoSolicitud.efk_ptos_factor_comportamientosf = new CrmDecimal();
                fotoSolicitud.efk_ptos_factor_comportamientosf.Value = Utility.ConvertStringToDecimal(GetScoreEvaluateResult(
                    ScoreEvaluateResult.WSRESULT_COMPORTAMIENTO_SISTEMA, msgWSResponse.Result));

                fotoSolicitud.efk_ptos_factor_comportamientobmsc = new CrmDecimal();
                fotoSolicitud.efk_ptos_factor_comportamientobmsc.Value = Utility.ConvertStringToDecimal(GetScoreEvaluateResult(
                    ScoreEvaluateResult.WSRESULT_COMPORTAMIENTO_BMSC, msgWSResponse.Result));

                fotoSolicitud.efk_ptos_factor_garantias = new CrmDecimal();
                fotoSolicitud.efk_ptos_factor_garantias.Value = Utility.ConvertStringToDecimal(GetScoreEvaluateResult(
                    ScoreEvaluateResult.WSRESULT_GARANTIAS, msgWSResponse.Result));

                fotoSolicitud.efk_ptos_riesgo_indirecto = new CrmDecimal();
                fotoSolicitud.efk_ptos_riesgo_indirecto.Value = Utility.ConvertStringToDecimal(GetScoreEvaluateResult(
                    ScoreEvaluateResult.WSRESULT_RIESGO_INDIRECTO, msgWSResponse.Result));

                fotoSolicitud.efk_ptos_sobre_endeudamiento = new CrmDecimal();
                fotoSolicitud.efk_ptos_sobre_endeudamiento.Value = Utility.ConvertStringToDecimal(GetScoreEvaluateResult(
                    ScoreEvaluateResult.WSRESULT_SOBRE_ENDEUDAMIENTO, msgWSResponse.Result));

                fotoSolicitud.efk_puntuacion_global_final = new CrmDecimal();
                fotoSolicitud.efk_puntuacion_global_final.Value = Utility.ConvertStringToDecimal(GetScoreEvaluateResult(
                    ScoreEvaluateResult.WSRESULT_SOBRE_ENDEUDAMIENTO, msgWSResponse.Result));

                // tipo de cambio
                fotoSolicitud.efk_tipo_cambio_contable = new CrmMoney();
                fotoSolicitud.efk_tipo_cambio_contable.Value = msgWSResponse.TCContable;

                fotoSolicitud.efk_tipo_cambio_compra = new CrmMoney();
                fotoSolicitud.efk_tipo_cambio_compra.Value = msgWSResponse.TCCompra;

                fotoSolicitud.efk_tipo_cambio_venta = new CrmMoney();
                fotoSolicitud.efk_tipo_cambio_venta.Value = msgWSResponse.TCVenta;

                SetDatosModeloEvaluador(ref fotoSolicitud);


                if (createFotoSolicitud == false)
                {
                    resultado = Servicio.Create(fotoSolicitud).ToString();
                }
                else
                {
                    fotoSolicitud.efk_foto_solicitud_creditoid = new Key();
                    fotoSolicitud.efk_foto_solicitud_creditoid.Value = new Guid(fotoSolicitudCreditoId);
                    Servicio.Update(fotoSolicitud);
                }

                resultado = "0";
            }
            catch (SoapException ex)
            {
                resultado = null;
                throw new Exception(ex.Detail.InnerText.Replace("\n", ""));
            }

            catch (Exception ex)
            {
                resultado = null;
                throw ex;
            }

            return resultado;
        }


        private bool ExistsFotoSolicitudCredito(Guid opportunityID, ref string fotoSolicitudID)
        {
            bool exists;
            ColumnSet resultSetColumns = new ColumnSet();

            resultSetColumns.Attributes = new string[] { "efk_foto_solicitud_creditoid", "efk_name", "efk_opportunityid" };
            exists = false;

            Efika.Crm.AccesoServicios.CRMSDK.ConditionExpression ConditionExp = new Efika.Crm.AccesoServicios.CRMSDK.ConditionExpression();
            ConditionExp.AttributeName = "efk_opportunityid";
            ConditionExp.Operator = Efika.Crm.AccesoServicios.CRMSDK.ConditionOperator.Equal;
            ConditionExp.Values = new string[] { opportunityID.ToString() };

            Efika.Crm.AccesoServicios.CRMSDK.QueryExpression queryExp = new Efika.Crm.AccesoServicios.CRMSDK.QueryExpression();
            queryExp.EntityName = EntityName.efk_foto_solicitud_credito.ToString();
            queryExp.Criteria = new Efika.Crm.AccesoServicios.CRMSDK.FilterExpression();
            queryExp.Criteria.Conditions = new Efika.Crm.AccesoServicios.CRMSDK.ConditionExpression[] { ConditionExp };
            queryExp.ColumnSet = resultSetColumns;

            BusinessEntityCollection beFotoSolicitud = Servicio.RetrieveMultiple(queryExp);

            if (beFotoSolicitud.BusinessEntities.Count() > 0)
            {
                foreach (BusinessEntity entity in beFotoSolicitud.BusinessEntities)
                {
                    var fsc = (efk_foto_solicitud_credito)entity;
                    fotoSolicitudID = fsc.efk_foto_solicitud_creditoid.Value.ToString();
                    exists = true;
                }
            }
            return exists;
        }


        private string GetScoreEvaluateResult(string field, List<ScoreEvaluateResult> value, string defaultValue)
        {
            string rvalue;
            try
            {
                var result = (from o in value where o.Campo == field select o).Single<ScoreEvaluateResult>();
                rvalue = result.Valor;
            }
            catch (Exception)
            {
                rvalue = defaultValue;
            }
            return rvalue;
        }

        private string GetScoreEvaluateResult(string field, List<ScoreEvaluateResult> value)
        {
            return GetScoreEvaluateResult(field, value, string.Empty);
        }

        private bool SetDatosModeloEvaluador(ref efk_foto_solicitud_credito foto)
        {
            bool rvalue = true;
            int ordenOportunidad = 0;
            Cliente cliente;

            ordenOportunidad = (foto.efk_orden_oportunidad == null ? 0 : foto.efk_orden_oportunidad.Value);

            if (ordenOportunidad == 0)
                return false;

            cliente = new Cliente(this.credenciales);
            AccesoServicios.CRMSDK.account accountCrm = cliente.GetAccountById(foto.efk_clienteid.Value, GetColumnsModeloEvaluador());

            Divisa negDivisa = new Divisa(credenciales);
            Entidades.Divisa divisaFoto = negDivisa.ConsultaDivisa(foto.transactioncurrencyid.Value);


            if (accountCrm.efk_salario_liquido_titularmes1_base != null)
            {
                foto.efk_salario_liquido_titularmes1 = new CrmMoney();
                foto.efk_salario_liquido_titularmes1.Value = accountCrm.efk_salario_liquido_titularmes1_base.Value * divisaFoto.ExchangeRate;
            }

            if (accountCrm.efk_salario_liquido_titularmes2_base != null)
            {
                foto.efk_salario_liquido_titularmes2 = new CrmMoney();
                foto.efk_salario_liquido_titularmes2.Value = accountCrm.efk_salario_liquido_titularmes2_base.Value * divisaFoto.ExchangeRate;
            }

            if (accountCrm.efk_salario_liquido_titularmes3_base != null)
            {
                foto.efk_salario_liquido_titularmes3 = new CrmMoney();
                foto.efk_salario_liquido_titularmes3.Value = accountCrm.efk_salario_liquido_titularmes3_base.Value * divisaFoto.ExchangeRate;
            }

            if (accountCrm.efk_salario_liquido_conyuguemes1_base != null)
            {
                foto.efk_salario_liquido_conyuguemes1 = new CrmMoney();
                foto.efk_salario_liquido_conyuguemes1.Value = accountCrm.efk_salario_liquido_conyuguemes1_base.Value * divisaFoto.ExchangeRate;
            }

            if (accountCrm.efk_salario_liquido_conyuguemes2_base != null)
            {
                foto.efk_salario_liquido_conyuguemes2 = new CrmMoney();
                foto.efk_salario_liquido_conyuguemes2.Value = accountCrm.efk_salario_liquido_conyuguemes2_base.Value * divisaFoto.ExchangeRate;
            }

            if (accountCrm.efk_salario_liquido_conyuguemes3_base != null)
            {
                foto.efk_salario_liquido_conyuguemes3 = new CrmMoney();
                foto.efk_salario_liquido_conyuguemes3.Value = accountCrm.efk_salario_liquido_conyuguemes3_base.Value * divisaFoto.ExchangeRate;
            }

            if (accountCrm.efk_ingresos_mensuales_alquileres_titular_base != null)
            {
                foto.efk_ingresos_mensuales_alquileres_titular = new CrmMoney();
                foto.efk_ingresos_mensuales_alquileres_titular.Value = accountCrm.efk_ingresos_mensuales_alquileres_titular_base.Value * divisaFoto.ExchangeRate;
            }

            if (accountCrm.efk_ingresos_mensuales_alquileres_conyugue_base != null)
            {
                foto.efk_ingresos_mensuales_alquileres_conyugue = new CrmMoney();
                foto.efk_ingresos_mensuales_alquileres_conyugue.Value = accountCrm.efk_ingresos_mensuales_alquileres_conyugue_base.Value * divisaFoto.ExchangeRate;
            }

            if (accountCrm.efk_ingresos_anuales_abonos_titular_base != null)
            {
                foto.efk_ingresos_anuales_abonos_titular = new CrmMoney();
                foto.efk_ingresos_anuales_abonos_titular.Value = accountCrm.efk_ingresos_anuales_abonos_titular_base.Value * divisaFoto.ExchangeRate;
            }

            if (accountCrm.efk_ingresos_anuales_abonos_conyugue_base != null)
            {
                foto.efk_ingresos_anuales_abonos_conyugue = new CrmMoney();
                foto.efk_ingresos_anuales_abonos_conyugue.Value = accountCrm.efk_ingresos_anuales_abonos_conyugue_base.Value * divisaFoto.ExchangeRate;
            }

            if (accountCrm.efk_hipotecario_cuotas_respaldadas_base != null)
            {
                foto.efk_hipotecario_cuotas_respaldadas = new CrmMoney();
                foto.efk_hipotecario_cuotas_respaldadas.Value = accountCrm.efk_hipotecario_cuotas_respaldadas_base.Value * divisaFoto.ExchangeRate;
            }

            if (accountCrm.efk_hipotecario_cuotas_cartera_base != null)
            {
                foto.efk_hipotecario_cuotas_cartera = new CrmMoney();
                foto.efk_hipotecario_cuotas_cartera.Value = accountCrm.efk_hipotecario_cuotas_cartera_base.Value * divisaFoto.ExchangeRate;
            }

            if (accountCrm.efk_hipotecario_cuotas_ddjj_base != null)
            {
                foto.efk_hipotecario_cuotas_ddjj = new CrmMoney();
                foto.efk_hipotecario_cuotas_ddjj.Value = accountCrm.efk_hipotecario_cuotas_ddjj_base.Value * divisaFoto.ExchangeRate;
            }

            if (accountCrm.efk_consumo_cuotas_respaldadas_base != null)
            {
                foto.efk_consumo_cuotas_respaldadas = new CrmMoney();
                foto.efk_consumo_cuotas_respaldadas.Value = accountCrm.efk_consumo_cuotas_respaldadas_base.Value * divisaFoto.ExchangeRate;
            }

            if (accountCrm.efk_consumo_saldo_cartera_base != null)
            {
                foto.efk_consumo_saldo_cartera = new CrmMoney();
                foto.efk_consumo_saldo_cartera.Value = accountCrm.efk_consumo_saldo_cartera_base.Value * divisaFoto.ExchangeRate;
            }

            if (accountCrm.efk_consumo_cuotas_ddjj_base != null)
            {
                foto.efk_consumo_cuotas_ddjj = new CrmMoney();
                foto.efk_consumo_cuotas_ddjj.Value = accountCrm.efk_consumo_cuotas_ddjj_base.Value * divisaFoto.ExchangeRate;
            }

            if (accountCrm.efk_consumotdc_cuotas_respaldadas_base != null)
            {
                foto.efk_consumotdc_cuotas_respaldadas = new CrmMoney();
                foto.efk_consumotdc_cuotas_respaldadas.Value = accountCrm.efk_consumotdc_cuotas_respaldadas_base.Value * divisaFoto.ExchangeRate;
            }

            if (accountCrm.efk_consumotdc_saldo_cartera_base != null)
            {
                foto.efk_consumotdc_saldo_cartera = new CrmMoney();
                foto.efk_consumotdc_saldo_cartera.Value = accountCrm.efk_consumotdc_saldo_cartera_base.Value * divisaFoto.ExchangeRate;
            }

            if (accountCrm.efk_consumotdc_cuotas_ddjj_base != null)
            {
                foto.efk_consumotdc_cuotas_ddjj = new CrmMoney();
                foto.efk_consumotdc_cuotas_ddjj.Value = accountCrm.efk_consumotdc_cuotas_ddjj_base.Value * divisaFoto.ExchangeRate;
            }

            if (accountCrm.efk_comercialpyme_cuotas_respaldadas_base != null)
            {
                foto.efk_comercialpyme_cuotas_respaldadas = new CrmMoney();
                foto.efk_comercialpyme_cuotas_respaldadas.Value = accountCrm.efk_comercialpyme_cuotas_respaldadas_base.Value * divisaFoto.ExchangeRate;
            }

            if (accountCrm.efk_comercialpyme_saldo_cartera_base != null)
            {
                foto.efk_comercialpyme_saldo_cartera = new CrmMoney();
                foto.efk_comercialpyme_saldo_cartera.Value = accountCrm.efk_comercialpyme_saldo_cartera_base.Value * divisaFoto.ExchangeRate;
            }

            if (accountCrm.efk_comercialpyme_cuotas_ddjj_base != null)
            {
                foto.efk_comercialpyme_cuotas_ddjj = new CrmMoney();
                foto.efk_comercialpyme_cuotas_ddjj.Value = accountCrm.efk_comercialpyme_cuotas_ddjj_base.Value * divisaFoto.ExchangeRate;
            }

            if (accountCrm.efk_microcredito_cuotas_respaldadas_base != null)
            {
                foto.efk_microcredito_cuotas_respaldadas = new CrmMoney();
                foto.efk_microcredito_cuotas_respaldadas.Value = accountCrm.efk_microcredito_cuotas_respaldadas_base.Value * divisaFoto.ExchangeRate;
            }

            if (accountCrm.efk_microcredito_saldo_cartera_base != null)
            {
                foto.efk_microcredito_saldo_cartera = new CrmMoney();
                foto.efk_microcredito_saldo_cartera.Value = accountCrm.efk_microcredito_saldo_cartera_base.Value * divisaFoto.ExchangeRate;
            }

            if (accountCrm.efk_microcredito_cuotas_ddjj_base != null)
            {
                foto.efk_microcredito_cuotas_ddjj = new CrmMoney();
                foto.efk_microcredito_cuotas_ddjj.Value = accountCrm.efk_microcredito_cuotas_ddjj_base.Value * divisaFoto.ExchangeRate;
            }

            if (accountCrm.efk_deuda_empresa_empleadora_base != null)
            {
                foto.efk_deuda_empresa_empleadora = new CrmMoney();
                foto.efk_deuda_empresa_empleadora.Value = accountCrm.efk_deuda_empresa_empleadora_base.Value * divisaFoto.ExchangeRate;
            }

            if (accountCrm.efk_cuotas_bmsc_base != null)
            {
                foto.efk_cuotas_bmsc = new CrmMoney();
                foto.efk_cuotas_bmsc.Value = accountCrm.efk_cuotas_bmsc_base.Value * divisaFoto.ExchangeRate;
            }

            if (accountCrm.efk_cuotas_bmsc_tramite_base != null)
            {
                foto.efk_cuotas_bmsc_tramite = new CrmMoney();
                foto.efk_cuotas_bmsc_tramite.Value = accountCrm.efk_cuotas_bmsc_tramite_base.Value * divisaFoto.ExchangeRate;
            }

            if (accountCrm.efk_cuotas_credito_compra_base != null)
            {
                foto.efk_cuotas_credito_compra = new CrmMoney();
                foto.efk_cuotas_credito_compra.Value = accountCrm.efk_cuotas_credito_compra_base.Value * divisaFoto.ExchangeRate;
            }

            if (accountCrm.efk_gastos_personales_base != null)
            {
                foto.efk_gastos_personales = new CrmMoney();
                foto.efk_gastos_personales.Value = accountCrm.efk_gastos_personales_base.Value * divisaFoto.ExchangeRate;
            }

            if (accountCrm.efk_peor_calificacion_12meses != null)
            {
                foto.efk_peor_calificacion_12meses = new Picklist();
                foto.efk_peor_calificacion_12meses.Value = accountCrm.efk_peor_calificacion_12meses.Value;

            }

            if (accountCrm.efk_nroveces_calificacion_distintaa != null)
            {
                foto.efk_nroveces_calificacion_distintaa = new CrmNumber();
                foto.efk_nroveces_calificacion_distintaa.Value = accountCrm.efk_nroveces_calificacion_distintaa.Value;
            }

            if (accountCrm.efk_nroveces_vencido_ejecucion != null)
            {
                foto.efk_nroveces_vencido_ejecucion = new CrmNumber();
                foto.efk_nroveces_vencido_ejecucion.Value = accountCrm.efk_nroveces_vencido_ejecucion.Value;
            }

            if (accountCrm.efk_nroveces_vigente2_14dias != null)
            {
                foto.efk_nroveces_vigente2_14dias = new CrmNumber();
                foto.efk_nroveces_vigente2_14dias.Value = accountCrm.efk_nroveces_vigente2_14dias.Value;
            }

            if (accountCrm.efk_valor_liquidable_garantias_constituidas_base != null)
            {
                foto.efk_valor_liquidable_garantias_constituidas = new CrmMoney();
                foto.efk_valor_liquidable_garantias_constituidas.Value = accountCrm.efk_valor_liquidable_garantias_constituidas_base.Value * divisaFoto.ExchangeRate;
            }

            if (accountCrm.efk_total_cartera_garantia_hipotecaria_base != null)
            {
                foto.efk_total_cartera_garantia_hipotecaria = new CrmMoney();
                foto.efk_total_cartera_garantia_hipotecaria.Value = accountCrm.efk_total_cartera_garantia_hipotecaria_base.Value * divisaFoto.ExchangeRate;
            }

            if (accountCrm.efk_total_cartera_garantia_personal_base != null)
            {
                foto.efk_total_cartera_garantia_personal = new CrmMoney();
                foto.efk_total_cartera_garantia_personal.Value = accountCrm.efk_total_cartera_garantia_personal_base.Value * divisaFoto.ExchangeRate;
            }

            if (accountCrm.efk_total_cartera_asolafirma_base != null)
            {
                foto.efk_total_cartera_asolafirma = new CrmMoney();
                foto.efk_total_cartera_asolafirma.Value = accountCrm.efk_total_cartera_asolafirma_base.Value * divisaFoto.ExchangeRate;
            }

            if (accountCrm.efk_nroveces_vencido_ejecucion_titular != null)
            {
                foto.efk_nroveces_vencido_ejecucion_titular = new CrmNumber();
                foto.efk_nroveces_vencido_ejecucion_titular.Value = accountCrm.efk_nroveces_vencido_ejecucion_titular.Value;
            }

            if (accountCrm.efk_nroveces_vencido_ejecucion_garante != null)
            {
                foto.efk_nroveces_vencido_ejecucion_garante = new CrmNumber();
                foto.efk_nroveces_vencido_ejecucion_garante.Value = accountCrm.efk_nroveces_vencido_ejecucion_garante.Value;
            }

            return rvalue;
        }

        private ColumnSet GetColumnsModeloEvaluador()
        {
            ColumnSet resultSetColumns = new ColumnSet();

            resultSetColumns.Attributes = new string[] {
                "efk_salario_liquido_titularmes1_base",
               "efk_salario_liquido_titularmes2_base",
               "efk_salario_liquido_titularmes3_base", 
               "efk_salario_liquido_conyuguemes1_base", 
               "efk_salario_liquido_conyuguemes2_base", 
               "efk_salario_liquido_conyuguemes3_base",
               "efk_ingresos_mensuales_alquileres_titular_base", 
               "efk_ingresos_mensuales_alquileres_conyugue_base",
               "efk_ingresos_anuales_abonos_titular_base", 
               "efk_ingresos_anuales_abonos_conyugue_base",
               "efk_hipotecario_cuotas_respaldadas_base", 
               "efk_hipotecario_cuotas_cartera_base",
               "efk_hipotecario_cuotas_ddjj_base", 
               "efk_consumo_cuotas_respaldadas_base",
               "efk_consumo_saldo_cartera_base", 
               "efk_consumo_cuotas_ddjj_base",
               "efk_consumotdc_cuotas_respaldadas_base", 
               "efk_consumotdc_saldo_cartera_base",
               "efk_consumotdc_cuotas_ddjj_base", 
               "efk_comercialpyme_cuotas_respaldadas_base",
               "efk_comercialpyme_saldo_cartera_base", 
               "efk_comercialpyme_cuotas_ddjj_base",
               "efk_microcredito_cuotas_respaldadas_base", 
               "efk_microcredito_saldo_cartera_base",
               "efk_microcredito_cuotas_ddjj_base", 
               "efk_deuda_empresa_empleadora_base",
               "efk_cuotas_bmsc_base", 
               "efk_cuotas_bmsc_tramite_base",
               "efk_cuotas_credito_compra_base",
               "efk_gastos_personales_base",
               "efk_peor_calificacion_12meses",
               "efk_nroveces_calificacion_distintaa",
               "efk_nroveces_vencido_ejecucion",
               "efk_nroveces_vigente2_14dias",
               "efk_valor_liquidable_garantias_constituidas_base",
               "efk_total_cartera_garantia_hipotecaria_base",
               "efk_total_cartera_garantia_personal_base",
               "efk_total_cartera_asolafirma_base",
               "efk_nroveces_vencido_ejecucion_titular",
               "efk_nroveces_vencido_ejecucion_garante"
            };

            return resultSetColumns;

        }
    }

}
