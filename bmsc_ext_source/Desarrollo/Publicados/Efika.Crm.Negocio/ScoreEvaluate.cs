
namespace Efika.Crm.Negocio
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Efika.Crm.Entidades;
    using Efika.Crm.Entidades.ScoreEvaluate;
    using Efika.Crm.AccesoServicios.CRMSDK;
    using Efika.Crm.AccesoServicios;
    using Efika.Crm.Negocio.Common;
    using System.Web.Services.Protocols;


    public class ScoreEvaluate
    {
        private Usuario usuario;
        private CrmService servicio;
        private CredencialesCRM mCredenciales;
        private MsgRequestScoreEvaluate msgReq = new MsgRequestScoreEvaluate();

        public ScoreEvaluate(CredencialesCRM credenciales)
        {
            this.mCredenciales = credenciales;
            servicio = ServicioCRM.ObtenerServicioCRM(credenciales);
            this.usuario = new Usuario(this.mCredenciales);
        }

        public MsgRequestScoreEvaluate GetMsgReqScoreEvaluate(Guid userID, Guid opportunityID)
        {
            try
            {
                ColumnSet resultSetColumns = new ColumnSet();
                systemuser crmUser;
                efk_oficina crmOficina;
                efk_sucursal crmSucursal;
                account crmAccount;
                efk_paramtero_simulacion_crediticia efk_psc;


                Cliente cliente = new Cliente(this.mCredenciales);
                Agencia oficina = new Agencia(this.mCredenciales);
                SucursalRepository sucursal = new SucursalRepository(this.mCredenciales);
                Oportunidad oportunidad = new Oportunidad(this.mCredenciales);
                opportunity e_opportunity;
                int diaAntiguedad = ScoreEvaluateResult.INFO_DIAS_ANTIGUEDAD;


                resultSetColumns.Attributes = new string[] { "fullname", "title", "efk_sucursalid", "efk_oficina_id" };
                crmUser = usuario.GetUserById(userID, resultSetColumns);

                if (crmUser == null) throw new Exception("No se logro obtener informacion sobre el usuario.");
                if (crmUser.efk_oficina_id == null) throw new Exception("No se logro obtener informacion de la oficina. El usuario debe tener registrada una oficina");
                if (crmUser.efk_sucursalid == null) throw new Exception("No se logro obtener informacion de la sucursal. El usuario debe tener registrada una sucursal");

                resultSetColumns.Attributes = new string[] { "efk_codigo" };
                crmOficina = oficina.getById(crmUser.efk_oficina_id.Value, resultSetColumns);

                resultSetColumns.Attributes = new string[] { "efk_codigo" };
                crmSucursal = sucursal.getById(crmUser.efk_sucursalid.Value, resultSetColumns);

                resultSetColumns.Attributes = new string[] { "customerid", "efk_fecha_ultima_actualizacion_cond_garantias", "efk_numero_oferta", "efk_orden" };
                e_opportunity = oportunidad.GetById(opportunityID, resultSetColumns);

                resultSetColumns.Attributes = new string[] { "efk_fecha_ultima_actualizacion_mod_evaluador" };
                crmAccount = cliente.GetAccountById(e_opportunity.customerid.Value, resultSetColumns);


                ParametroSimulacionCrediticia psc = new ParametroSimulacionCrediticia(this.mCredenciales);
                efk_psc = psc.GetParametroByName(ParametroSimulacionCrediticia.PARAMETRO_CANT_DIAS_ANTIGUEDAD_DATOS_SCORING);

                if (efk_psc != null)
                    diaAntiguedad = efk_psc.efk_valor_entero.Value;

                msgReq.NroSolicitud = 0;
                msgReq.NroOferta = e_opportunity.efk_numero_oferta == null ? 0 : e_opportunity.efk_numero_oferta.Value;
                msgReq.OpportunityOrden = e_opportunity.efk_orden == null ? 1 : e_opportunity.efk_orden.Value;
                msgReq.ClienteID = e_opportunity.customerid.Value.ToString();
                msgReq.LoginUsuario = string.Empty;
                msgReq.CodigoAgencia = crmOficina.efk_codigo;
                msgReq.CodigoSucursal = crmSucursal.efk_codigo;
                msgReq.FechaEjecucion = DateTime.Now;

                msgReq.OpportunityUpdateCondGarantias = Convert.ToDateTime(ValidateDateUpdate(e_opportunity.efk_fecha_ultima_actualizacion_cond_garantias));
                msgReq.ClienteUpdateModeloEvaluador = Convert.ToDateTime(ValidateDateUpdate(crmAccount.efk_fecha_ultima_actualizacion_mod_evaluador));
                msgReq.InfoDiaAntiguedad = diaAntiguedad;
                
               
                Divisa entDivisa = new Divisa(this.mCredenciales);
                Entidades.Divisa objDivisa = new Entidades.Divisa();
                objDivisa =  entDivisa.ConsultaDivisa(crmAccount.transactioncurrencyid.Value);
                msgReq.Moneda = GetCodigoMonedaBMSC(new Guid(objDivisa.isoCurrencyCode), mCredenciales); ;
                
                return msgReq;
            }
            catch (SoapException ex)
            {
                
                throw new Exception(ex.Detail.InnerText.Replace("\n", ""));
            }
            catch (Exception ex)
    {
                throw ex;
            }
        }


        private object ValidateDateUpdate(CrmDateTime value)
        {
            if (value == null)
            {
                int mes = DateTime.Now.Month;
                if (mes == 01)
                { mes = 12; }
                else
                { mes -= 1; }

                value = new CrmDateTime();
                value.Value = new DateTime(DateTime.Now.Year, mes, 1).ToString();
            }
            return value.Value;
        }


        public Guid GetCustomerID(Guid opportunityID)
        {
            Oportunidad oportunidad = new Oportunidad(this.mCredenciales);
            ColumnSet resultSetColumns = new ColumnSet();
            opportunity e_opportunity;

            resultSetColumns.Attributes = new string[] { "customerid" };
            e_opportunity = oportunidad.GetById(opportunityID, resultSetColumns);

            return e_opportunity.customerid.Value;
        }


        public bool ValidateAntiguedadDatos(Guid opportunityID, ref  List<ValidateField> validateField)
        {
            Cliente cliente = new Cliente(this.mCredenciales);
            Oportunidad oportunidad = new Oportunidad(this.mCredenciales);
            ColumnSet resultSetColumns = new ColumnSet();
            opportunity cmrOpportunity;
            account crmAccount;
            DateTime date1;
            DateTime date2;

            resultSetColumns.Attributes = new string[] { "customerid", "efk_fecha_ultima_actualizacion_cond_garantias" };
            cmrOpportunity = oportunidad.GetById(opportunityID, resultSetColumns);

            resultSetColumns.Attributes = new string[] { "efk_fecha_ultima_actualizacion_mod_evaluador" };
            crmAccount = cliente.GetAccountById(cmrOpportunity.customerid.Value, resultSetColumns);

            date1 = Convert.ToDateTime(ValidateDateUpdate(cmrOpportunity.efk_fecha_ultima_actualizacion_cond_garantias));
            date2 = Convert.ToDateTime(ValidateDateUpdate(crmAccount.efk_fecha_ultima_actualizacion_mod_evaluador));

            return ValidateAntiguedadDatos(this.mCredenciales, opportunityID, GetCustomerID(opportunityID), date1, date2, ScoreEvaluateResult.INFO_DIAS_ANTIGUEDAD, ref validateField);
        }


        public bool ValidateAntiguedadDatos(CredencialesCRM credenciales, Guid opportunityID, Guid customerID, 
            DateTime updateOpportunity, DateTime updateAccount, int diaAntiguedad, ref  List<ValidateField> validateField)
        {
            bool rvalue = true;
            try
            {
                long dayUpdate = 0;

                dayUpdate = Utility.DateDiff(DateInterval.Day, updateOpportunity, DateTime.Now);
                if (dayUpdate >= diaAntiguedad)
                {
                    validateField.Add(new ValidateField("Oportunidad", "Condiciones y Garantias", "*",
                        "La informacion de Condiciones y Garantias de la oportunidad esta desactualizada"));
                }

                dayUpdate = Utility.DateDiff(DateInterval.Day, updateAccount, DateTime.Now);
                if (dayUpdate >= diaAntiguedad)
                {
                    validateField.Add(new ValidateField("Cliente", "Datos Modelo Evaluador", "*",
                        "La informacion de Datos Modelo Evaluador del cliente esta desactualizada"));
                }
            }
            catch (Exception)
            {
                rvalue = false;
            }

            if (validateField.Count > 0) rvalue = false;

            return rvalue;
        }


        public bool ValidateOportunidad_CondicionesGarantia(Guid opportunityID, int nroOferta, ref  List<ValidateField> validateField)
        {
            return ValidateOportunidad_CondicionesGarantia(this.mCredenciales, opportunityID, nroOferta, ref validateField);
        }


        public bool ValidateOportunidad_CondicionesGarantia(CredencialesCRM credenciales, Guid opportunityID, int nroOferta, ref  List<ValidateField> listValidateField)
        {
            try
            {
                bool rValue;
                ColumnSet resultSetColumns = new ColumnSet();
                opportunity e_opportunity;
                BusinessEntityCollection listOpportunity;
                string opportunityName;

                rValue = true;

                resultSetColumns.Attributes = new string[]   {"opportunityid", "name", "efk_nrosolicitud", "efk_numero_oferta", "efk_numero_cuotas",
                                                    "efk_moneda_operacion", "efk_spread_fijo", "efk_tasa_fija", "efk_tre_semana", "efk_tasa_variable_apartirde",
                                                    "efk_tipo_poliza", "efk_con_seguro_cesantia", "efk_con_seguro_desgravamen", "efk_monto_maximo",
                                                    "efk_monto_solicitado"};

                Oportunidad oportunidad = new Oportunidad(credenciales);
                listOpportunity = oportunidad.GetByNroOferta(nroOferta, resultSetColumns);

                foreach (BusinessEntity item in listOpportunity.BusinessEntities)
                {
                    e_opportunity = (opportunity)item;

                    if (e_opportunity != null)
                    {
                        opportunityName = e_opportunity.name == null ? string.Empty : e_opportunity.name;

                        if (e_opportunity.efk_nrosolicitud == null)
                        {
                            listValidateField.Add(new ValidateField("Oportunidad", "Condiciones y Garantias", "Numero de Solicitud", 
                                "Se debe ingresar una valor para este campos", opportunityName, nroOferta.ToString()));
                        }
                        if (e_opportunity.efk_numero_oferta == null)
                        {
                            listValidateField.Add(new ValidateField("Oportunidad", "Condiciones y Garantias", "Numero de Oferta", 
                                "Se debe ingresar una valor para este campos", opportunityName, nroOferta.ToString()));
                        }
                        if (e_opportunity.efk_numero_cuotas == null)
                        {
                            listValidateField.Add(new ValidateField("Oportunidad", "Condiciones y Garantias", "Numero de Cuotas", 
                                "Se debe ingresar una valor para este campos", opportunityName, nroOferta.ToString()));
                        }
                        if (e_opportunity.efk_moneda_operacion == null)
                        {
                            listValidateField.Add(new ValidateField("Oportunidad", "Condiciones y Garantias", "Moneda de Operacion",
                                "Se debe ingresar una valor para este campos", opportunityName, nroOferta.ToString()));
                        }
                        if (e_opportunity.efk_spread_fijo == null)
                        {
                            listValidateField.Add(new ValidateField("Oportunidad", "Condiciones y Garantias", "SPREAD Fijo", 
                                "Se debe ingresar una valor para este campos", opportunityName, nroOferta.ToString()));
                        }
                        if (e_opportunity.efk_tasa_fija == null)
                        {
                            listValidateField.Add(new ValidateField("Oportunidad", "Condiciones y Garantias", "Tasa Fija", 
                                "Se debe ingresar una valor para este campos", opportunityName, nroOferta.ToString()));
                        }
                        if (e_opportunity.efk_tre_semana == null)
                        {
                            listValidateField.Add(new ValidateField("Oportunidad", "Condiciones y Garantias", "TRE Semana",
                                "Se debe ingresar una valor para este campos", opportunityName, nroOferta.ToString()));
                        }
                        if (e_opportunity.efk_tasa_variable_apartirde == null)
                        {
                            listValidateField.Add(new ValidateField("Oportunidad", "Condiciones y Garantias", "Tasa variable a partir de",
                                "Se debe ingresar una valor para este campos", opportunityName, nroOferta.ToString()));
                        }
                        if (e_opportunity.efk_tipo_poliza == null)
                        {
                            listValidateField.Add(new ValidateField("Oportunidad", "Condiciones y Garantias", "Tipo Poliza", 
                                "Se debe ingresar una valor para este campos", opportunityName, nroOferta.ToString()));
                        }
                        if (e_opportunity.efk_monto_solicitado == null)
                        {
                            listValidateField.Add(new ValidateField("Oportunidad", "Condiciones y Garantias", "Monto solicitado",
                                "Se debe ingresar una valor para este campos", opportunityName, nroOferta.ToString()));
                        }
                    }
                }

                if (listValidateField.Count > 0) rValue = false;

                return rValue;
            }
            catch (SoapException ex)
            {
                throw new Exception(ex.Detail.InnerText.Replace("\n", ""));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool ValidateCliente_ModeloEvaluador(CredencialesCRM credenciales, Guid customerID, ref  List<ValidateField> listValidateField)
        {
            bool rValue = true;
            ColumnSet resultSetColumns = new ColumnSet();
            account crmAccount;
            Cliente cliente = new Cliente(this.mCredenciales);


            try
            {
                resultSetColumns.Attributes = new string[] { 
                "efk_segmento_ovid",
                "efk_salario_liquido_titularmes1",
                "efk_salario_liquido_titularmes2",
                "efk_salario_liquido_titularmes3",
                "efk_salario_liquido_conyuguemes1",
                "efk_salario_liquido_conyuguemes2",
                "efk_salario_liquido_conyuguemes3",
                "efk_ingresos_mensuales_alquileres_titular",
                "efk_ingresos_mensuales_alquileres_conyugue",
                "efk_ingresos_anuales_abonos_titular",
                "efk_ingresos_anuales_abonos_conyugue",
                "efk_hipotecario_cuotas_respaldadas",
                "efk_hipotecario_cuotas_cartera",
                "efk_hipotecario_cuotas_ddjj",
                "efk_consumo_cuotas_respaldadas",
                "efk_consumo_saldo_cartera",
                "efk_consumo_cuotas_ddjj",
                "efk_consumotdc_cuotas_respaldadas",
                "efk_consumotdc_saldo_cartera",
                "efk_consumotdc_cuotas_ddjj",
                "efk_comercialpyme_cuotas_respaldadas",
                "efk_comercialpyme_saldo_cartera",
                "efk_comercialpyme_cuotas_ddjj",
                "efk_microcredito_cuotas_respaldadas",
                "efk_microcredito_saldo_cartera",
                "efk_microcredito_cuotas_ddjj",
                "efk_deuda_empresa_empleadora",
                "efk_cuotas_bmsc",
                "efk_cuotas_bmsc_tramite",
                "efk_cuotas_credito_compra",
                "efk_gastos_personales",
                "efk_peor_calificacion_12meses",
                "efk_nroveces_calificacion_distintaa",
                "efk_nroveces_vencido_ejecucion",
                "efk_nroveces_vigente2_14dias",
                "efk_valor_liquidable_garantias_constituidas",
                "efk_total_cartera_garantia_hipotecaria",
                "efk_total_cartera_garantia_personal",
                "efk_total_cartera_asolafirma",
                "efk_nroveces_vencido_ejecucion_titular",
                "efk_nroveces_vencido_ejecucion_garante"
                };

                crmAccount = cliente.GetAccountById(customerID, resultSetColumns);

                if (crmAccount.efk_segmento_ovid == null)
                {
                    crmAccount.efk_segmento_ovid = new Lookup();
                    crmAccount.efk_segmento_ovid.name = ".";
                }

                if (crmAccount.efk_segmento_ovid.name.Substring(0, 1).ToUpper() != "D")
                {
                    listValidateField.Add(new ValidateField("Cliente", "Segmento-D", "*", 
                        "El cliente no pertenece al segmento depenendiente, No es posible evaluar esta oportunidad."));
                }
                if (crmAccount.efk_salario_liquido_titularmes1 == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*", 
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_salario_liquido_titularmes2 == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*", 
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_salario_liquido_titularmes3 == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*", 
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_salario_liquido_conyuguemes1 == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*", 
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_salario_liquido_conyuguemes2 == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*", 
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_salario_liquido_conyuguemes3 == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*", 
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_ingresos_mensuales_alquileres_titular == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*", 
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_ingresos_mensuales_alquileres_conyugue == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*", 
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_ingresos_anuales_abonos_titular == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*", 
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_ingresos_anuales_abonos_conyugue == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*", 
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_hipotecario_cuotas_respaldadas == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*", 
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_hipotecario_cuotas_cartera == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*", 
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_hipotecario_cuotas_ddjj == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*", 
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_consumo_cuotas_respaldadas == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*", 
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_consumo_saldo_cartera == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*", 
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_consumo_cuotas_ddjj == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*",
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_consumotdc_cuotas_respaldadas == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*",
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_consumotdc_saldo_cartera == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*",
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_consumotdc_cuotas_ddjj == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*",
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_comercialpyme_cuotas_respaldadas == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*", 
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_comercialpyme_saldo_cartera == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*", 
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_comercialpyme_cuotas_ddjj == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*", 
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_microcredito_cuotas_respaldadas == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*", 
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_microcredito_saldo_cartera == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*", 
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_microcredito_cuotas_ddjj == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*", 
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_deuda_empresa_empleadora == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*", 
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_cuotas_bmsc == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*", 
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_cuotas_bmsc_tramite == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*", 
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_cuotas_credito_compra == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*", 
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_gastos_personales == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*", 
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_peor_calificacion_12meses == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*", 
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_nroveces_calificacion_distintaa == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*", 
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_nroveces_vencido_ejecucion == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*", 
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_nroveces_vigente2_14dias == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*", 
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_valor_liquidable_garantias_constituidas == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*", 
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_total_cartera_garantia_hipotecaria == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*", 
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_total_cartera_garantia_personal == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*", 
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_total_cartera_asolafirma == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*", 
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_nroveces_vencido_ejecucion_titular == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*", 
                        "Se debe ingresar una valor para este campos"));
                }
                if (crmAccount.efk_nroveces_vencido_ejecucion_garante == null)
                {
                    listValidateField.Add(new ValidateField("Cliente", "Modelo Evaluador", "*", 
                        "Se debe ingresar una valor para este campos"));
                }
            }
            catch (Exception)
            {
                rValue = false;
            }

            if (listValidateField.Count > 0) rValue = false;

            return rValue;
        }
        private int GetCodigoMonedaBMSC(Guid divisaId, CredencialesCRM credenciales)
        {
            Negocio.Divisa negDivisa = new Negocio.Divisa(credenciales);
            Entidades.Divisa divisa = negDivisa.ConsultaDivisa(divisaId);
            int codMonedaBMSC;
            codMonedaBMSC = int.Parse(Utilidades.GetCodBmscMoneda(divisa.isoCurrencyCode));
            return codMonedaBMSC;
        }

     
    }
}
