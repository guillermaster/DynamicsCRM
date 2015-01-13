using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Efika.Crm.CargaDatos.crmSDK;

namespace Efika.Crm.CargaDatos
{
    public class CDServicioCanal
    {
        public Guid efk_servicios_canalesid;
        public int? iCodigoClienteBanco;
        public Guid efk_accountid;
        public string efk_codigo_unico_producto;
        public Guid efk_producto_core_id;
        public string efk_numero_operacion;
        public string efk_estado_operacion;
        public int? efk_clase_producto_banco;
        public int? efk_cant_transacciones_ult_mes;
        public decimal? efk_monto_total_transacciones_ult_mes;
        public float? efk_cant_prom_trim_trans_ult_mes; //
        public float? efk_porc_uso_canal_ult_mes;
        public decimal? efk_monto_prom_trim_total_trans_ult_mes;
        public decimal? efk_monto_prom_x_transaccion_ult_mes;
        public decimal? efk_monto_prom_comision_mensual_x_mes;
        public decimal? efk_comision_pactada;
        public float? efk_frecuencia_uso_diario_ult_mes; //
        public float? efk_frecuencia_uso_diario_ult_3_meses; //
        public string efk_codigo_ejecutivo_creacion;
        public string efk_codigo_agencia_creacion;
        public Guid efk_ejecutivo_creacionid;
        public Guid efk_agencia_creacionid;
        public DateTime? efk_fecha_ultimo_uso;
        public DateTime? efk_fecha_apertura_emision;
        public decimal? efk_monto_total_prom_trans_x_mes_ult_3_meses;

        public string sTipoReg;
        public Guid ownerid;
        public Guid ownerAnteriorid;
        public string efk_name;

        public int? efk_tipo_debito_automatico;
        public string efk_moneda;

        public DateTime? efk_dfechainactivo;
        public DateTime? efk_dfechaultimouso;
        public bool? efk_bnumeroconvenioprimeravez;

        public Guid efk_transcurrency;
        public decimal? efk_cambio;

        public void Cargar(CrmService servicio)
        {
            try
            {
                efk_servicios_canales obj = new efk_servicios_canales();              

                if (sTipoReg == "E")
                {
                    if (this.efk_servicios_canalesid != Guid.Empty)
                    {
                        if (ExisteRegistro(servicio, efk_servicios_canalesid, "efk_servicios_canales"))
                            servicio.Delete(EntityName.efk_servicios_canales.ToString(), this.efk_servicios_canalesid);
                    }
                }
                else
                {
                    if (efk_accountid != Guid.Empty)
                    {
                        obj.efk_accountid = new Lookup();
                        obj.efk_accountid.Value = efk_accountid;
                    }

                    //obj.transactioncurrencyid
                    //obj.exchangerate

                    if (efk_transcurrency != Guid.Empty)
                    {
                        obj.transactioncurrencyid = new Lookup();
                        obj.transactioncurrencyid.Value = efk_transcurrency;
                    }


                    if (efk_cambio != null)
                    {
                        obj.exchangerate = new CrmDecimal();
                        obj.exchangerate.Value = efk_cambio.Value;
                    }
                    
                  
                    
                    if (efk_codigo_unico_producto != null)
                    {
                        obj.efk_codigo_unico_producto = efk_codigo_unico_producto;
                    }
                    if (efk_name != null)
                    {
                        obj.efk_nombre = efk_name;
                    }
                    if (efk_moneda != null)
                    {
                        obj.efk_moneda = efk_moneda;
                    }     
                    if (efk_producto_core_id != Guid.Empty)
                    {
                        obj.efk_producto_coreid = new Lookup();
                        obj.efk_producto_coreid.Value = efk_producto_core_id;
                    }
                    else
                    {
                        throw new Exception("Registro sin producto banco: " + efk_codigo_unico_producto);
                    }

                    if (efk_numero_operacion != null)
                    {
                        obj.efk_numero_operacion = efk_numero_operacion;
                    }
                    if (efk_estado_operacion != null)
                    {
                        obj.efk_estado_operacion = efk_estado_operacion;
                    }

                    if (efk_cant_transacciones_ult_mes != null)
                    {
                        obj.efk_cantidad_transacciones_ultimo_mes = new CrmNumber();
                        obj.efk_cantidad_transacciones_ultimo_mes.Value = efk_cant_transacciones_ult_mes.Value;
                    }
                    if (efk_monto_total_transacciones_ult_mes != null)
                    {
                        obj.efk_monto_total_transacciones_ultimo_mes = new CrmMoney();
                        obj.efk_monto_total_transacciones_ultimo_mes.Value = efk_monto_total_transacciones_ult_mes.Value;
                    }
                    if (efk_cant_prom_trim_trans_ult_mes != null)
                    {
                        obj.efk_cant_prom_transacciones_x_mes_ult_3_meses = new CrmFloat();
                        obj.efk_cant_prom_transacciones_x_mes_ult_3_meses.Value = efk_cant_prom_trim_trans_ult_mes.Value;
                    }
                    if (efk_porc_uso_canal_ult_mes != null)
                    {
                        obj.efk_porc_uso_canal_x_mes = new CrmFloat();
                        obj.efk_porc_uso_canal_x_mes.Value = efk_porc_uso_canal_ult_mes.Value;
                    }
                    if (efk_frecuencia_uso_diario_ult_mes != null)
                    {
                        obj.efk_frecuencia_uso_diario_ult_mes = new CrmFloat();
                        obj.efk_frecuencia_uso_diario_ult_mes.Value = efk_frecuencia_uso_diario_ult_mes.Value;
                    }
                    if (efk_frecuencia_uso_diario_ult_3_meses != null)
                    {
                        obj.efk_frecuencia_uso_diario_ult_3_meses = new CrmFloat();
                        obj.efk_frecuencia_uso_diario_ult_3_meses.Value = efk_frecuencia_uso_diario_ult_3_meses.Value;
                    }
                    if (efk_fecha_ultimo_uso != null && efk_fecha_ultimo_uso.Value > new DateTime(1900, 1, 1))
                    {
                        obj.efk_ultima_fecha_uso = new CrmDateTime();
                        obj.efk_ultima_fecha_uso.Value = efk_fecha_ultimo_uso.Value.ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                    if (efk_monto_prom_trim_total_trans_ult_mes != null)
                    {
                        obj.efk_monto_total_prom_trans_x_mes_ult_3_meses = new CrmMoney();
                        obj.efk_monto_total_prom_trans_x_mes_ult_3_meses.Value = efk_monto_prom_trim_total_trans_ult_mes.Value;
                    }
                    if (efk_monto_prom_x_transaccion_ult_mes != null)
                    {
                        obj.efk_monto_promedio_transaccion_ult_3_meses = new CrmMoney();
                        obj.efk_monto_promedio_transaccion_ult_3_meses.Value = efk_monto_prom_x_transaccion_ult_mes.Value;
                    }
                    if (efk_monto_prom_comision_mensual_x_mes != null)
                    {
                        obj.efk_monto_total_comision_x_mes_ult_3_meses = new CrmMoney();
                        obj.efk_monto_total_comision_x_mes_ult_3_meses.Value = efk_monto_prom_comision_mensual_x_mes.Value;
                    }
                    if (efk_comision_pactada != null)
                    {
                        obj.efk_comision_pactada_servicio = new CrmMoney();
                        obj.efk_comision_pactada_servicio.Value = efk_comision_pactada.Value;
                    }

                    if (efk_fecha_apertura_emision != null && efk_fecha_apertura_emision.Value > new DateTime(1900, 1, 1))
                    {
                        obj.efk_fecha_apertura_emision = new CrmDateTime();
                        obj.efk_fecha_apertura_emision.Value = efk_fecha_ultimo_uso.Value.ToString("yyyy-MM-ddTHH:mm:ss");
                    }

                    if (efk_clase_producto_banco != null)
                    {
                        obj.efk_clase_producto_banco = new Picklist();
                        obj.efk_clase_producto_banco.Value = efk_clase_producto_banco.Value;
                    }

                    if (efk_codigo_ejecutivo_creacion != null)
                        obj.efk_codigo_ejecutivo_creacion = efk_codigo_ejecutivo_creacion;

                    if (efk_codigo_agencia_creacion != null)
                        obj.efk_codigo_agencia_creacion = efk_codigo_agencia_creacion;

                    if (efk_ejecutivo_creacionid != Guid.Empty)
                    {
                        obj.efk_ejecutivo_creacion_emision = new Lookup();
                        obj.efk_ejecutivo_creacion_emision.Value = efk_ejecutivo_creacionid;
                    }
                    if (efk_agencia_creacionid != Guid.Empty)
                    {
                        obj.efk_agencia_creacion_emision = new Lookup();
                        obj.efk_agencia_creacion_emision.Value = efk_agencia_creacionid;
                    }

                    if (efk_monto_total_prom_trans_x_mes_ult_3_meses != null)
                    {
                        obj.efk_monto_total_prom_trans_x_mes_ult_3_meses = new CrmMoney();
                        obj.efk_monto_total_prom_trans_x_mes_ult_3_meses.Value = efk_monto_total_prom_trans_x_mes_ult_3_meses.Value;
                    }
                    //***********************************************
                    if (ownerid != Guid.Empty)
                    {
                        obj.ownerid = new Owner();
                        obj.ownerid.type = "systemuser";
                        obj.ownerid.Value = ownerid;
                    }

                    //Prioridad 0
                    if (this.efk_tipo_debito_automatico != null)
                    {

                        obj.efk_tipo_debito_automatico = new Picklist();
                        obj.efk_tipo_debito_automatico.Value = this.efk_tipo_debito_automatico.Value;
                    }

                    if (this.efk_dfechainactivo != null)
                    {
                        obj.efk_fechainactivo = new CrmDateTime();
                        obj.efk_fechainactivo.Value = this.efk_dfechainactivo.Value.ToString("yyyy-MM-ddTHH:mm:ss"); ;
                    }

                    if (this.efk_dfechaultimouso != null)
                    {
                        obj.efk_fechaultimouso = new CrmDateTime();
                        obj.efk_fechaultimouso.Value = this.efk_dfechaultimouso.Value.ToString("yyyy-MM-ddTHH:mm:ss"); ;
                    }

                    if (this.efk_bnumeroconvenioprimeravez != null)
                    {
                        obj.efk_numeroconvenioprimeravez = new CrmBoolean();
                        obj.efk_numeroconvenioprimeravez.Value = this.efk_bnumeroconvenioprimeravez.Value;
                    }
                    //

                    if (this.efk_servicios_canalesid != Guid.Empty)
                    {
                        if (ExisteRegistro(servicio, efk_servicios_canalesid, "efk_servicios_canales"))
                        {
                            //Actualizamos
                            obj.efk_servicios_canalesid = new Key();
                            obj.efk_servicios_canalesid.Value = efk_servicios_canalesid;

                            servicio.Update(obj);

                            if (ownerid != Guid.Empty && ownerAnteriorid != ownerid)
                            {
                                //reasignamos el cliente
                                AssignRequest req = new AssignRequest();
                                TargetOwnedefk_servicios_canales pp = new TargetOwnedefk_servicios_canales();
                                pp.EntityId = obj.efk_servicios_canalesid.Value;
                                req.Target = pp;
                                SecurityPrincipal sp = new SecurityPrincipal();
                                sp.Type = SecurityPrincipalType.User;
                                sp.PrincipalId = ownerid;
                                req.Assignee = sp;

                                servicio.Execute(req);

                            }
                        }
                    }
                    else
                    {
                        this.efk_accountid = servicio.Create(obj);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="servicio"></param>
        /// <param name="id"></param>
        /// <param name="entidad"></param>
        /// <returns></returns>
        private bool ExisteRegistro(CrmService servicio, Guid id, string entidad)
        {
            bool existe = false;
            ColumnSet columna = new ColumnSet();
            columna.Attributes = new String[] { "efk_servicios_canalesid" };
            try
            {
                BusinessEntity ent = servicio.Retrieve(entidad, id, columna);
                if (ent != null)
                    existe = true;
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                if (ex.Detail.InnerText.Contains("0x80040217"))
                    existe = false;
            }

            return existe;
        }
    }
}
