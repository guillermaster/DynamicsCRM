using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Efika.Crm.CargaDatos.crmSDK;

namespace Efika.Crm.CargaDatos
{
    public class CDProductoActivo
    {
        public Guid efk_producto_activoid;
        public int? iCodigoClienteBanco;
        public Guid efk_cliente_juridico_id;
        public string efk_codigo_modulo_producto;
        public string efk_codigo_producto;
        public string efk_codigo_tipo;
        public string efk_codigo_producto_core;
        public string efk_codigo_unico_producto;
        public Guid efk_producto_core_id;
        public string efk_numero;
        public string efk_estado_operacion;
        public Guid transactioncurrencyid;
        public string efk_moneda;
        //---------************************Fase 2************************
        public Guid efk_producto_activoid_linea_credito;
        public string efk_numero_linea_credito;
        public Guid efk_ejecutivo_creacionid;
        public string efk_codigo_ejecutivo_creacion;
        public Guid efk_agencia_creacionid;
        public string efk_codigo_agencia_creacion;

        //********************** credito ********************************
        public decimal? efk_monto_desembolsado;
        public decimal? efk_monto_pagado;
        public decimal? efk_monto_proxima_cuota;
        public decimal? efk_monto_vencido;
        public decimal? efk_monto_vencido_a_pagar;
        public DateTime? efk_fecha_vencimiento_operacion;
        public DateTime? efk_fecha_inicio_operacion;
        public DateTime? efk_fecha_pago_proxima_cuota;
        public int? efk_plazo;
        public int? efk_cantidad_dividendos_vencidos;
        public decimal? efk_saldo_fecha;
        public double? efk_tasa_actual;
        public double? efk_tasa_tre;
        public bool? efk_debito_automatico;
        //Fase 2
        public bool? efk_reprogramado;
        public int? efk_numero_cuotas;

        //**************************************************************
        public int? efk_anio_vehiculo;
        public string efk_beneficiario;
        public string efk_calificacion_interna;
        public string efk_compania_seguro_todo_riesgo;
        public Picklist efk_condicion; //conjunto de opciones
        public decimal? efk_cupo_autorizado;
        public decimal? efk_cupo_utilizado;
        public decimal? efk_deuda_capital;
        public string efk_estado_cuenta;
        public decimal? efk_monto_operacion_capital;
        //****************garantias************************
        public decimal? efk_valor_liquidacion;
        public decimal? efk_valor_comercial;
        public string efk_descripcion_garantia;
        public string efk_operaciones_garantizadas;
        public string efk_numero_poliza_todo_riesgo;
        public string efk_codigo_linea_credito;
        public Guid efk_cliente_garante_personalid;
        //Fase 2
        public decimal? efk_saldo_disponible;
        public DateTime? efk_fecha_ultimo_avaluo;

        public int? efk_clase_producto_banco;
        //********************** credeito vehiculo **************************        
        public decimal? efk_valor_comercial_vivienda;
        public decimal? efk_valor_vehiculo;
        //**************** TDC **************************************
        public string efk_numero_tarjeta;
        public string efk_estado_tarjeta;
        public string efk_estado_cuenta_atc;
        public decimal? efk_monto_utilizado;
        public decimal? efk_limite_compra;
        public decimal? efk_limite_financiamiento;
        public decimal? efk_gastos_generados;
        public decimal? efk_pago_minimo;
        public bool? efk_realiza_compras_exterior;
        public bool? efk_tarjetas_adicionales;

        public string efk_tipo_pago_debito_automatico;
        public DateTime? efk_fecha_vencimiento_plastico;
        public string efk_tipo_tdc;
        public string efk_nombre_tarjetahabiente;

        //****************** COMEX *********************************
        //fase 2
        public decimal? efk_monto_utilizacion_carta_credito;
        public bool? efk_permite_embarques_parciales;
        public float? efk_tolerancia_maxima;
        public float? efk_tolerancia_minima;
        public string efk_tipo_carta_credito;
        public string efk_tipo_garantia;//comex
        public string efk_objeto_garantia;
        public string efk_ordenante;

        //****************** Líneas de Crédito *********************
        public string efk_tipo_habilitar_para_ingreso;
        public decimal? efk_monto_aprobado;
        public decimal? efk_monto_disponible;
        public float? efk_porc_uso;
        public string efk_sublimite_linea_credito;
        public string efk_condiciones_particulares;
        public decimal? efk_coverance;

        //************************************************************
        public string sTipoReg;
        public Guid ownerid;
        public Guid ownerAnteriorid;
        public string efk_name;



        //*********** FASE - Priodad 0 ******************
        public int efk_dias_mora = 0;
        public decimal? efk_monto_cuota_maxima;
        //TC
        public string efk_sEstadoPlasticodeCredito;
        // GARANTÍAS
        public bool? efk_legalizacion;
        public string efk_numero_folio;
        public DateTime? efk_fecha_avaluo;
        public string efk_avaluador;
        public string efk_numero_protocolo;
        public string efk_notaria;
        public decimal? efk_valor_prestamos;
        public bool? efk_polizas_vencidas;
        public DateTime? efk_fecha_legalizacion_garantias;
        public decimal? efk_monto_no_utilizado;
        public int? efk_ultima_cuota_pagada;
        public string efk_tipo_de_garantia;
        //****************************************

        public void Cargar(CrmService servicio)
        {
            try
            {
                efk_producto_activo obj = new efk_producto_activo();

                if (sTipoReg == "E")
                {
                    if (this.efk_producto_activoid != Guid.Empty)
                    {
                        int c = 0;
                        bool exito = false;
                        do
                        {
                            try
                            {
                                if (ExisteRegistro(servicio, efk_producto_activoid, "efk_producto_activo"))
                                    servicio.Delete("efk_producto_activo", this.efk_producto_activoid);

                                exito = true;
                            }
                            catch (System.Web.Services.Protocols.SoapException ex)
                            {
                                if (ex.Detail.InnerText.Contains("0x80044150"))
                                {
                                    c++;
                                    System.Threading.Thread.Sleep(500);
                                }
                                else throw ex;
                            }
                        } while (!exito & c < 5);

                    }
                }
                else
                {

                    if (efk_cliente_juridico_id != Guid.Empty)
                    {
                        obj.efk_cliente_juridico_id = new Lookup();
                        obj.efk_cliente_juridico_id.Value = efk_cliente_juridico_id;
                    }
                    if (efk_codigo_modulo_producto != null)
                    {
                        obj.efk_codigo_modulo_producto = efk_codigo_modulo_producto;
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
                    if (efk_producto_core_id != Guid.Empty)
                    {
                        obj.efk_producto_core_id = new Lookup();
                        obj.efk_producto_core_id.Value = efk_producto_core_id;
                    }
                    else
                    {
                        throw new Exception("Registro sin producto banco: " + efk_codigo_unico_producto);
                    }
                    if (efk_numero != null)
                    {
                        obj.efk_numero = efk_numero;
                    }
                    if (efk_estado_operacion != null)
                    {
                        obj.efk_estado_operacion = efk_estado_operacion;
                        obj.efk_tipo_grado_garantia = efk_estado_operacion;
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
                    if (efk_monto_desembolsado != null)
                    {
                        obj.efk_monto_desembolsado = new CrmMoney();
                        obj.efk_monto_desembolsado.Value = efk_monto_desembolsado.Value;
                    }
                    if (efk_monto_pagado != null)
                    {
                        obj.efk_monto_pagado = new CrmMoney();
                        obj.efk_monto_pagado.Value = efk_monto_pagado.Value;
                    }
                    if (efk_monto_proxima_cuota != null)
                    {
                        obj.efk_monto_proxima_cuota = new CrmMoney();
                        obj.efk_monto_proxima_cuota.Value = efk_monto_proxima_cuota.Value;
                    }
                    if (efk_monto_vencido != null)
                    {
                        obj.efk_monto_vencido = new CrmMoney();
                        obj.efk_monto_vencido.Value = efk_monto_vencido.Value;
                    }
                    if (efk_monto_vencido_a_pagar != null)
                    {
                        obj.efk_monto_vencido_a_pagar = new CrmMoney();
                        obj.efk_monto_vencido_a_pagar.Value = efk_monto_vencido_a_pagar.Value;
                    }
                    if (efk_fecha_vencimiento_operacion != null && efk_fecha_vencimiento_operacion.Value > new DateTime(1900, 1, 1))
                    {
                        obj.efk_fecha_vencimiento_operacion = new CrmDateTime();
                        obj.efk_fecha_vencimiento_operacion.Value = efk_fecha_vencimiento_operacion.Value.ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                    if (efk_fecha_inicio_operacion != null && efk_fecha_inicio_operacion.Value > new DateTime(1900, 1, 1))
                    {
                        obj.efk_fecha_inicio_operacion = new CrmDateTime();
                        obj.efk_fecha_inicio_operacion.Value = efk_fecha_inicio_operacion.Value.ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                    if (efk_fecha_pago_proxima_cuota != null && efk_fecha_pago_proxima_cuota.Value > new DateTime(1900, 1, 1))
                    {
                        obj.efk_fecha_pago_proxima_cuota = new CrmDateTime();
                        obj.efk_fecha_pago_proxima_cuota.Value = efk_fecha_pago_proxima_cuota.Value.ToString("yyyy-MM-ddTHH:mm:ss");
                    }

                    if (efk_saldo_fecha != null)
                    {
                        obj.efk_saldo_fecha = new CrmMoney();
                        obj.efk_saldo_fecha.Value = efk_saldo_fecha.Value;
                    }

                    if (efk_plazo != null)
                    {
                        obj.efk_plazo = new CrmNumber();
                        obj.efk_plazo.Value = efk_plazo.Value;
                    }
                    if (efk_cantidad_dividendos_vencidos != null)
                    {
                        obj.efk_cantidad_dividendos_vencidos = new CrmNumber();
                        obj.efk_cantidad_dividendos_vencidos.Value = efk_cantidad_dividendos_vencidos.Value;
                    }

                    if (efk_tasa_actual != null)
                    {
                        obj.efk_tasa_actual = new CrmFloat();
                        obj.efk_tasa_actual.Value = efk_tasa_actual.Value;
                    }
                    if (efk_tasa_tre != null)
                    {
                        obj.efk_tasa_tre = new CrmFloat();
                        obj.efk_tasa_tre.Value = efk_tasa_tre.Value;
                    }

                    if (efk_debito_automatico != null)
                    {
                        obj.efk_debito_automatico = new CrmBoolean();
                        obj.efk_debito_automatico.Value = efk_debito_automatico.Value;
                    }
                    if (efk_anio_vehiculo != null)
                    {
                        obj.efk_anio_vehiculo = new CrmNumber();
                        obj.efk_anio_vehiculo.Value = efk_anio_vehiculo.Value;
                    }
                    if (efk_beneficiario != null)
                    {
                        obj.efk_beneficiario = efk_beneficiario;
                    }
                    if (efk_calificacion_interna != null)
                    {
                        obj.efk_calificacion_interna = efk_calificacion_interna;
                    }
                    if (efk_compania_seguro_todo_riesgo != null)
                    {
                        obj.efk_compania_seguro_todo_riesgo = efk_compania_seguro_todo_riesgo;
                    }
                    if (efk_cupo_autorizado != null)
                    {
                        obj.efk_cupo_autorizado = new CrmMoney();
                        obj.efk_cupo_autorizado.Value = efk_cupo_autorizado.Value;
                    }
                    if (efk_cupo_utilizado != null)
                    {
                        obj.efk_cupo_utilizado = new CrmMoney();
                        obj.efk_cupo_utilizado.Value = efk_cupo_utilizado.Value;
                    }
                    if (efk_descripcion_garantia != null)
                    {
                        obj.efk_descripcion_garantia = efk_descripcion_garantia;
                    }
                    if (efk_deuda_capital != null)
                    {
                        obj.efk_deuda_capital = new CrmMoney();
                        obj.efk_deuda_capital.Value = efk_deuda_capital.Value;
                    }
                    if (efk_estado_cuenta != null)
                    {
                        obj.efk_estado_cuenta = efk_estado_cuenta;
                    }
                    if (efk_monto_operacion_capital != null)
                    {
                        obj.efk_monto_operacion_capital = new CrmMoney();
                        obj.efk_monto_operacion_capital.Value = efk_monto_operacion_capital.Value;
                    }

                    if (efk_valor_comercial_vivienda != null)
                    {
                        obj.efk_valor_comercial_vivienda = new CrmMoney();
                        obj.efk_valor_comercial_vivienda.Value = efk_valor_comercial_vivienda.Value;
                    }
                    if (efk_valor_vehiculo != null)
                    {
                        obj.efk_valor_vehiculo = new CrmMoney();
                        obj.efk_valor_vehiculo.Value = efk_valor_vehiculo.Value;
                    }
                    //************************************************************************//
                    if (efk_numero_tarjeta != null)
                    {
                        obj.efk_numero_tarjeta = efk_numero_tarjeta;
                    }
                    if (efk_estado_tarjeta != null)
                    {
                        obj.efk_estado_tarjeta = efk_estado_tarjeta;
                    }
                    if (efk_estado_cuenta_atc != null)
                    {
                        obj.efk_estado_cuenta_atc = efk_estado_cuenta_atc;
                    }
                    if (efk_monto_utilizado != null)
                    {
                        obj.efk_monto_utilizado = new CrmMoney();
                        obj.efk_monto_utilizado.Value = efk_monto_utilizado.Value;
                    }
                    if (efk_limite_compra != null)
                    {
                        obj.efk_limite_compra = new CrmMoney();
                        obj.efk_limite_compra.Value = efk_limite_compra.Value;
                    }
                    if (efk_limite_financiamiento != null)
                    {
                        obj.efk_limite_financiamiento = new CrmMoney();
                        obj.efk_limite_financiamiento.Value = efk_limite_financiamiento.Value;
                    }
                    if (efk_gastos_generados != null)
                    {
                        obj.efk_gastos_generados = new CrmMoney();
                        obj.efk_gastos_generados.Value = efk_gastos_generados.Value;
                    }
                    if (efk_pago_minimo != null)
                    {
                        obj.efk_pago_minimo = new CrmMoney();
                        obj.efk_pago_minimo.Value = efk_pago_minimo.Value;
                    }
                    if (efk_realiza_compras_exterior != null)
                    {
                        obj.efk_realiza_compras_exterior = new CrmBoolean();
                        obj.efk_realiza_compras_exterior.Value = efk_realiza_compras_exterior.Value;
                    }
                    if (efk_tarjetas_adicionales != null)
                    {
                        obj.efk_tarjetas_adicionales = new CrmBoolean();
                        obj.efk_tarjetas_adicionales.Value = efk_tarjetas_adicionales.Value;
                    }
                    if (efk_tipo_pago_debito_automatico != null)
                    {
                        obj.efk_tipo_pago_debito_autom = efk_tipo_pago_debito_automatico;
                    }
                    if (efk_fecha_vencimiento_plastico != null)
                    {
                        obj.efk_fecha_vencimiento_plastico = new CrmDateTime();
                        obj.efk_fecha_vencimiento_plastico.Value = efk_fecha_vencimiento_plastico.Value.ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                    if (efk_tipo_tdc != null)
                    {
                        obj.efk_tipo_tarjeta = efk_tipo_tdc;
                    }
                    if (efk_nombre_tarjetahabiente != null)
                    {
                        obj.efk_nombre_tarjetahabiente = efk_nombre_tarjetahabiente;
                    }

                    if (efk_valor_liquidacion != null)
                    {
                        obj.efk_valor_liquidacion = new CrmMoney();
                        obj.efk_valor_liquidacion.Value = efk_valor_liquidacion.Value;
                    }
                    if (efk_valor_comercial != null)
                    {
                        obj.efk_valor_comercial = new CrmMoney();
                        obj.efk_valor_comercial.Value = efk_valor_comercial.Value;
                    }
                    if (efk_operaciones_garantizadas != null)
                    {
                        obj.efk_operaciones_garantizadas = efk_operaciones_garantizadas;
                    }
                    if (efk_numero_poliza_todo_riesgo != null)
                    {
                        obj.efk_numero_poliza_todo_riesgo = efk_numero_poliza_todo_riesgo;
                    }
                    if (efk_cliente_garante_personalid != Guid.Empty)
                    {
                        obj.efk_cliente_garante_personalid = new Lookup();
                        obj.efk_cliente_garante_personalid.Value = efk_cliente_garante_personalid;
                    }

                    if (efk_clase_producto_banco != null)
                    {
                        obj.efk_clase_producto_banco = new Picklist();
                        obj.efk_clase_producto_banco.Value = efk_clase_producto_banco.Value;

                        if (efk_fecha_vencimiento_operacion != null && efk_fecha_vencimiento_operacion.Value > new DateTime(1900, 1, 1))
                        {
                            if (efk_clase_producto_banco.Value == 221220002)
                            {
                                obj.efk_fecha_inicio_operacion = new CrmDateTime();
                                obj.efk_fecha_inicio_operacion.Value = efk_fecha_vencimiento_operacion.Value.ToString("yyyy-MM-ddTHH:mm:ss");
                            }
                        }
                    }
                    //************************************************************************//
                    //************* Fase 2 ***************************************************//
                    if (efk_producto_activoid_linea_credito != Guid.Empty)
                    {
                        obj.efk_linea_creditoid = new Lookup();
                        obj.efk_linea_creditoid.Value = efk_producto_activoid_linea_credito;
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

                    if (efk_reprogramado != null)
                    {
                        obj.efk_reprogramado = new CrmBoolean();
                        obj.efk_reprogramado.Value = efk_reprogramado.Value;
                    }
                    if (efk_numero_cuotas != null)
                    {
                        obj.efk_numero_cuotas = new CrmNumber();
                        obj.efk_numero_cuotas.Value = efk_numero_cuotas.Value;
                    }
                    if (efk_saldo_disponible != null)
                    {
                        obj.efk_saldo_disponible_para_garantia = new CrmMoney();
                        obj.efk_saldo_disponible_para_garantia.Value = efk_saldo_disponible.Value;
                    }
                    if (efk_fecha_ultimo_avaluo != null && efk_fecha_ultimo_avaluo.Value > new DateTime(1900, 1, 1))
                    {
                        obj.efk_fecha_ultimo_avaluo = new CrmDateTime();
                        obj.efk_fecha_ultimo_avaluo.Value = efk_fecha_ultimo_avaluo.Value.ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                    if (efk_monto_utilizacion_carta_credito != null)
                    {
                        obj.efk_monto_utilizacion_carta_credito = new CrmMoney();
                        obj.efk_monto_utilizacion_carta_credito.Value = efk_monto_utilizacion_carta_credito.Value;
                    }
                    if (efk_permite_embarques_parciales != null)
                    {
                        obj.efk_permite_embarques_parciales = new CrmBoolean();
                        obj.efk_permite_embarques_parciales.Value = efk_permite_embarques_parciales.Value;
                    }
                    if (efk_tolerancia_maxima != null)
                    {
                        obj.efk_porc_tolerancia_maxima = new CrmFloat();
                        obj.efk_porc_tolerancia_maxima.Value = efk_tolerancia_maxima.Value;
                    }
                    if (efk_tolerancia_minima != null)
                    {
                        obj.efk_porc_tolerancia_minima = new CrmFloat();
                        obj.efk_porc_tolerancia_minima.Value = efk_tolerancia_minima.Value;
                    }
                    if (efk_tipo_carta_credito != null)
                        obj.efk_tipo_carta_credito = efk_tipo_carta_credito;
                    //----------------------------------------------------------------------------------
                    //--boleta de garantía
                    if (efk_estado_operacion != null)
                        obj.efk_tipo_de_garantia = efk_estado_operacion;
                    
                    //---garantía
                    if (efk_tipo_de_garantia != null)
                        obj.efk_tipo_grado_garantia = efk_tipo_de_garantia;
                    
                    //---comex
                    if (efk_tipo_garantia != null)
                        obj.efk_tipo_grado_garantia = efk_tipo_garantia;
                    //----------------------------------------------------------------------------------
                    if (efk_objeto_garantia != null)
                        obj.efk_objeto_de_garantia = efk_objeto_garantia;

                    if (efk_ordenante != null)
                        obj.efk_ordenante = efk_ordenante;

                    if (efk_tipo_habilitar_para_ingreso != null)
                        obj.efk_tipo_habilitar_para_ingreso = efk_tipo_habilitar_para_ingreso;

                    if (efk_monto_aprobado != null)
                    {
                        obj.efk_monto_aprobado = new CrmMoney();
                        obj.efk_monto_aprobado.Value = efk_monto_aprobado.Value;
                    }
                    if (efk_monto_disponible != null)
                    {
                        obj.efk_monto_disponible = new CrmMoney();
                        obj.efk_monto_disponible.Value = efk_monto_disponible.Value;
                    }
                    if (efk_porc_uso != null)
                    {
                        obj.efk_porc_uso = new CrmFloat();
                        obj.efk_porc_uso.Value = efk_porc_uso.Value;
                    }
                    if (efk_sublimite_linea_credito != null)
                        obj.efk_sublimite_linea_credito = efk_sublimite_linea_credito;

                    if (efk_condiciones_particulares != null)
                        obj.efk_condiciones_particulares = efk_condiciones_particulares;

                    if (efk_coverance != null)
                    {
                        obj.efk_coverance = new CrmMoney();
                        obj.efk_coverance.Value = efk_coverance.Value;
                    }

                    //************************************************************************//
                    if (ownerid != Guid.Empty)
                    {
                        obj.ownerid = new Owner();
                        obj.ownerid.type = "systemuser";
                        obj.ownerid.Value = ownerid;
                    }

                    //*********** FASE - Priodad 0 ******************
                    obj.efk_dias_mora = new CrmNumber();
                    obj.efk_dias_mora.Value = this.efk_dias_mora;

                    if (efk_monto_cuota_maxima != null)
                    {
                        obj.efk_monto_cuota_maxima = new CrmMoney();
                        obj.efk_monto_cuota_maxima.Value = efk_monto_cuota_maxima.Value;
                    }

                    // GARANTÍAS

                    if (efk_legalizacion != null)
                    {
                        obj.efk_legalizacion = new CrmBoolean();
                        obj.efk_legalizacion.Value = efk_legalizacion.Value;
                    }

                    obj.efk_numero_folio = efk_numero_folio;

                    if (efk_fecha_avaluo != null)
                    {
                        obj.efk_fecha_avaluo = new CrmDateTime();
                        obj.efk_fecha_avaluo.Value = efk_fecha_avaluo.Value.ToString("yyyy-MM-ddTHH:mm:ss");
                    }

                    obj.efk_avaluador = efk_avaluador;
                    obj.efk_numero_protocolo = efk_numero_protocolo;
                    obj.efk_notaria = efk_notaria;
                    if (efk_valor_prestamos != null)
                    {
                        obj.efk_valor_prestamos = new CrmMoney();
                        obj.efk_valor_prestamos.Value = efk_valor_prestamos.Value;
                    }
                    if (efk_polizas_vencidas != null)
                    {
                        obj.efk_polizas_vencidas = new CrmBoolean();
                        obj.efk_polizas_vencidas.Value = efk_polizas_vencidas.Value;
                    }
                    if (efk_fecha_legalizacion_garantias != null)
                    {
                        obj.efk_fecha_legalizacion_garantias = new CrmDateTime();
                        obj.efk_fecha_legalizacion_garantias.Value = efk_fecha_legalizacion_garantias.Value.ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                    if (efk_monto_no_utilizado != null)
                    {
                        obj.efk_monto_no_utilizado = new CrmMoney();
                        obj.efk_monto_no_utilizado.Value = efk_monto_no_utilizado.Value;
                    }

                    //TC
                    obj.efk_estadoplasticocodecredito = efk_sEstadoPlasticodeCredito;

                    if (efk_ultima_cuota_pagada != null)
                    {
                        obj.efk_ultima_cuota_pagada = new CrmNumber();
                        obj.efk_ultima_cuota_pagada.Value = efk_ultima_cuota_pagada.Value;
                    }

                    //************************************************************************//

                    if (this.efk_producto_activoid != Guid.Empty)
                    {
                        if (ExisteRegistro(servicio, efk_producto_activoid, "efk_producto_activo"))
                        {
                            //Actualizamos
                            obj.efk_producto_activoid = new Key();
                            obj.efk_producto_activoid.Value = efk_producto_activoid;

                            servicio.Update(obj);

                            if (ownerid != Guid.Empty && ownerAnteriorid != ownerid)
                            {
                                //reasignamos el cliente
                                AssignRequest req = new AssignRequest();
                                TargetOwnedefk_producto_activo pp = new TargetOwnedefk_producto_activo();
                                pp.EntityId = obj.efk_producto_activoid.Value;
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
                        this.efk_producto_activoid = servicio.Create(obj);
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
            columna.Attributes = new String[] { "efk_producto_activoid" };
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
