using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Efika.Crm.CargaDatos.crmSDK;

namespace Efika.Crm.CargaDatos
{
    public class CDProductoPasivo
    {
        public Guid efk_producto_pasivoid;
        public int? iCodigoClienteBanco;
        public Guid efk_cliente_juridico_id;
        public string efk_codigo_modulo;
        public string efk_codigo_producto;
        public string efk_codigo_tipo;
        public string efk_codigo_producto_core;
        public string efk_codigo_unico_producto;
        public Guid efk_producto_core_id;
        public string efk_numero_cuenta;
        public string efk_estado;
        public Guid transactioncurrencyid;
        public string efk_moneda;
        public decimal? efk_monto_operacion;
        public DateTime? efk_fecha_vencimiento;
        public int? efk_plazo_dias;
        public decimal? efk_saldo_disponible;
        public decimal? efk_saldo_contable_actual;
        public decimal? efk_promedio_trimestral;
        public DateTime? efk_fecha_apertura;
        public int? efk_dias_sobregiro;
        public double? efk_tasa_actual;
        public bool? efk_bloqueada;
        public bool? efk_retencion_judicial;
        public string efk_manejo_cuenta;
        public string efk_tipo_pago_intereses;
        public decimal? efk_intereses_al_vencimiento;
        public string sTipoReg;
        public Guid ownerid;
        public Guid ownerAnteriorid;
        public string efk_name;
        public int? efk_clase_producto_banco;
        //*********** Fase 2 ******************
        public bool? efk_pignorado;
        public decimal? efk_monto_total_depositos_mensuales;
        public decimal? efk_monto_total_transf_ctas_propias;
        public decimal? efk_monto_total_transf_ctas_terceros_mismo_banco;
        public decimal? efk_monto_total_transf_ctas_terceros_otros_bancos;
        public decimal? efk_saldo_bloqueado;
        public int? efk_numero_renovacion;
        public bool? efk_posee_tarjeta_debito;
        public string efk_codigo_ejecutivo_creacion;
        public string efk_codigo_agencia_creacion;
        public Guid efk_ejecutivo_creacionid;
        public Guid efk_agencia_creacionid;

        //***   FASE - REQUERIMIENTOS PRIORIDAD 0 (F1) ****/
        public string efk_firma_autorizada;
        public decimal? efk_promedio_depositos_6m;
        public decimal? efk_promedio_retiros_6m;
        public bool? efk_debitos_automaticos;
        public string efk_estado_cuenta;
        public bool? efk_sobregiro_activo;
        public decimal? efk_monto_sobregiro;
        public int? efk_tipo_cuenta_corriente;

        public decimal? efk_saldo_mes_anterior;
        public void Cargar(CrmService servicio)
        {
            try
            {
                efk_producto_pasivo obj = new efk_producto_pasivo();

                if (sTipoReg == "E")
                {
                    if (this.efk_producto_pasivoid != Guid.Empty)
                        servicio.Delete(EntityName.efk_producto_pasivo.ToString(), this.efk_producto_pasivoid);
                }
                else
                {
                    if (efk_producto_core_id != Guid.Empty)
                    {
                        obj.efk_producto_core_id = new Lookup();
                        obj.efk_producto_core_id.Value = efk_producto_core_id;
                    }
                    else
                    {
                        throw new Exception("Registro sin producto banco2: " + efk_codigo_unico_producto);
                    }
                    
                    if (efk_cliente_juridico_id != Guid.Empty)
                    {
                        obj.efk_cliente_juridico_id = new Lookup();
                        obj.efk_cliente_juridico_id.Value = efk_cliente_juridico_id;
                    }
                    if (efk_codigo_modulo != null)
                    {
                        obj.efk_codigo_modulo = efk_codigo_modulo;
                    }
                    if (efk_name != null)
                    {
                        obj.efk_name = efk_name;
                    }
                    if (efk_codigo_producto != null)
                    {
                        obj.efk_codigo_producto = efk_codigo_producto;
                    }
                    if (efk_codigo_tipo != null)
                    {
                        obj.efk_codigo_tipo = efk_codigo_tipo;
                    }
                    if (efk_codigo_producto_core != null)
                    {
                        obj.efk_codigo_producto_core = efk_codigo_producto_core;
                    }
                    if (efk_codigo_unico_producto != null)
                    {
                        obj.efk_codigo_unico_producto = efk_codigo_unico_producto;
                    }
                    if (efk_numero_cuenta != null)
                    {
                        obj.efk_numero_cuenta = efk_numero_cuenta;
                    }
                    if (efk_estado != null)
                    {
                        obj.efk_estado = efk_estado;
                    }
                    if (transactioncurrencyid != Guid.Empty)
                    {
                        obj.transactioncurrencyid = new Lookup();
                        obj.transactioncurrencyid.Value = transactioncurrencyid;
                    }
                    if (efk_moneda != null)
                    {
                        obj.efk_moneda = efk_moneda;
                    }
                    if (efk_monto_operacion != null)
                    {
                        obj.efk_monto_operacion = new CrmMoney();
                        obj.efk_monto_operacion.Value = efk_monto_operacion.Value;
                    }
                    if (efk_fecha_vencimiento != null && efk_fecha_vencimiento.Value > new DateTime(1900, 1, 1))
                    {
                        obj.efk_fecha_vencimiento = new CrmDateTime();
                        obj.efk_fecha_vencimiento.Value = efk_fecha_vencimiento.Value.ToString("yyyy-MM-ddTHH:mm:ss");
                    }

                    if (efk_saldo_disponible != null)
                    {
                        obj.efk_saldo_disponible = new CrmMoney();
                        obj.efk_saldo_disponible.Value = efk_saldo_disponible.Value;
                    }
                    if (efk_saldo_contable_actual != null)
                    {
                        obj.efk_saldo_contable_actual = new CrmMoney();
                        obj.efk_saldo_contable_actual.Value = efk_saldo_contable_actual.Value;
                    }
                    if (efk_promedio_trimestral != null)
                    {
                        obj.efk_promedio_trimestral = new CrmMoney();
                        obj.efk_promedio_trimestral.Value = efk_promedio_trimestral.Value;
                    }
                    if (efk_fecha_apertura != null && efk_fecha_apertura.Value > new DateTime(1900, 1, 1))
                    {
                        obj.efk_fecha_apertura = new CrmDateTime();
                        obj.efk_fecha_apertura.Value = efk_fecha_apertura.Value.ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                    if (efk_dias_sobregiro != null)
                    {
                        obj.efk_dias_sobregiro = new CrmNumber();
                        obj.efk_dias_sobregiro.Value = efk_dias_sobregiro.Value;
                    }

                    if (efk_plazo_dias != null)
                    {
                        obj.efk_plazo_dias = new CrmNumber();
                        obj.efk_plazo_dias.Value = efk_plazo_dias.Value;
                    }
                    if (efk_tasa_actual != null)
                    {
                        obj.efk_tasa_actual = new CrmFloat();
                        obj.efk_tasa_actual.Value = efk_tasa_actual.Value;
                    }
                    if (efk_bloqueada != null)
                    {
                        obj.efk_bloqueado = new CrmBoolean();
                        obj.efk_bloqueado.Value = efk_bloqueada.Value;
                    }
                    if (efk_retencion_judicial != null)
                    {
                        obj.efk_retencion_judicial = new CrmBoolean();
                        obj.efk_retencion_judicial.Value = efk_retencion_judicial.Value;
                    }
                    if (efk_manejo_cuenta != null)
                    {
                        obj.efk_manejo_cuenta = efk_manejo_cuenta;
                    }
                    if (efk_tipo_pago_intereses != null)
                    {
                        obj.efk_tipo_pago_intereses = efk_tipo_pago_intereses;
                    }
                    if (efk_intereses_al_vencimiento != null)
                    {
                        obj.efk_interes_al_vencimiento = new CrmMoney();
                        obj.efk_interes_al_vencimiento.Value = efk_intereses_al_vencimiento.Value;
                    }
                    //**********************************************
                    if (efk_clase_producto_banco != null)
                    {
                        obj.efk_clase_producto_banco = new Picklist();
                        obj.efk_clase_producto_banco.Value = efk_clase_producto_banco.Value;
                    }
                    //**********************************************
                    // Fase 2
                    if (efk_pignorado != null)
                    {
                        obj.efk_pignorado = new CrmBoolean();
                        obj.efk_pignorado.Value = efk_pignorado.Value;
                    }
                    if (efk_monto_total_depositos_mensuales != null)
                    {
                        obj.efk_monto_total_depositos_mensuales = new CrmMoney();
                        obj.efk_monto_total_depositos_mensuales.Value = efk_monto_total_depositos_mensuales.Value;
                    }
                    if (efk_monto_total_transf_ctas_propias != null)
                    {
                        obj.efk_monto_total_transferencia_cuentas_propias = new CrmMoney();
                        obj.efk_monto_total_transferencia_cuentas_propias.Value = efk_monto_total_transf_ctas_propias.Value;
                    }
                    if (efk_monto_total_transf_ctas_terceros_mismo_banco != null)
                    {
                        obj.efk_monto_total_transferencia_cuentas_tercero = new CrmMoney();
                        obj.efk_monto_total_transferencia_cuentas_tercero.Value = efk_monto_total_transf_ctas_terceros_mismo_banco.Value;
                    }
                    if (efk_monto_total_transf_ctas_terceros_otros_bancos != null)
                    {
                        obj.efk_monto_total_trans_ctas_terceros_otros_ban = new CrmMoney();
                        obj.efk_monto_total_trans_ctas_terceros_otros_ban.Value = efk_monto_total_transf_ctas_terceros_otros_bancos.Value;
                    }
                    if (efk_saldo_bloqueado != null)
                    {
                        obj.efk_saldo_bloqueado = new CrmMoney();
                        obj.efk_saldo_bloqueado.Value = efk_saldo_bloqueado.Value;
                    }
                    if (efk_numero_renovacion != null)
                    {
                        obj.efk_numero_renovacion = new CrmNumber();
                        obj.efk_numero_renovacion.Value = efk_numero_renovacion.Value;
                    }

                    if (efk_posee_tarjeta_debito != null)
                    {
                        obj.efk_posee_tarjeta_debito = new CrmBoolean();
                        obj.efk_posee_tarjeta_debito.Value = efk_posee_tarjeta_debito.Value;
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
                    //***********************************************
                    if (ownerid != Guid.Empty)
                    {
                        obj.ownerid = new Owner();
                        obj.ownerid.type = "systemuser";
                        obj.ownerid.Value = ownerid;
                    }

                    //*********** FASE - Priodad 0 ******************

                    if (string.IsNullOrEmpty(this.efk_firma_autorizada) == false)
                    {
                        obj.efk_firma_autorizada = this.efk_firma_autorizada;
                    }

                    if (efk_promedio_depositos_6m != null)
                    {
                        obj.efk_promedio_depositos_6m = new CrmMoney();
                        obj.efk_promedio_depositos_6m.Value = efk_promedio_depositos_6m.Value;
                    }

                    if (efk_promedio_retiros_6m != null)
                    {
                        obj.efk_promedio_retiros_6m = new CrmMoney();
                        obj.efk_promedio_retiros_6m.Value = efk_promedio_retiros_6m.Value;
                    }

                    if (efk_debitos_automaticos != null)
                    {
                        obj.efk_debitos_automaticos = new CrmBoolean();
                        obj.efk_debitos_automaticos.Value = efk_debitos_automaticos.Value;
                    }

                    if (string.IsNullOrEmpty(this.efk_estado_cuenta) == false)
                    {
                        obj.efk_estado_cuenta = this.efk_estado_cuenta;
                    }
                    if (efk_sobregiro_activo != null)
                    {
                        obj.efk_sobregiro_activo = new CrmBoolean();
                        obj.efk_sobregiro_activo.Value = efk_sobregiro_activo.Value;
                    }
                    if (efk_monto_sobregiro != null)
                    {
                        obj.efk_monto_sobregiro = new CrmMoney();
                        obj.efk_monto_sobregiro.Value = efk_monto_sobregiro.Value;
                    }
                    if (efk_tipo_cuenta_corriente != null)
                    {
                          obj.efk_tipo_cuenta_corriente =  new Picklist();
                          obj.efk_tipo_cuenta_corriente.Value = efk_tipo_cuenta_corriente.Value;
                    }

                    if (efk_saldo_mes_anterior != null)
                    {
                        obj.efk_saldo_mes_anterior = new CrmMoney();
                        obj.efk_saldo_mes_anterior.Value = efk_saldo_mes_anterior.Value;
                    }
                    

                    if (this.efk_producto_pasivoid != Guid.Empty)
                    {
                        //Actualizamos
                        obj.efk_producto_pasivoid = new Key();
                        obj.efk_producto_pasivoid.Value = efk_producto_pasivoid;

                        servicio.Update(obj);

                        if (ownerid != Guid.Empty && ownerAnteriorid != ownerid)
                        {
                            //reasignamos el cliente
                            AssignRequest req = new AssignRequest();
                            TargetOwnedefk_producto_pasivo pp = new TargetOwnedefk_producto_pasivo();
                            pp.EntityId = obj.efk_producto_pasivoid.Value;
                            req.Target = pp;
                            SecurityPrincipal sp = new SecurityPrincipal();
                            sp.Type = SecurityPrincipalType.User;
                            sp.PrincipalId = ownerid;
                            req.Assignee = sp;

                            servicio.Execute(req);

                        }
                    }
                    else
                    {
                        this.efk_producto_pasivoid = servicio.Create(obj);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
