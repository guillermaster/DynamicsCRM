using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Efika.Crm.CargaDatos.crmSDK;

namespace Efika.Crm.CargaDatos
{
    public class CDCliente
    {

        public string formatoFecha = "yyyy-MM-ddTHH:mm:ss";
        public Guid accountid;
        public int? efk_tipo_cliente;
        public int? efk_fuente_ingresos;
        public int? efk_tipo_banca;
        public int? efk_tipo_identificacion;
        public string accountnumber;
        public int? efk_codigo_cliente;
        public string efk_codigo_cliente_texto;
        public string name;
        public string efk_nombre_persona;
        public string efk_primerapellido;
        public string efk_segundoapellido;
        public int? efk_tipo_compania;
        public int? efk_tamano_actividad;
        public int? numberofemployees;
        public Guid efk_oficina_manejoid;
        public string efk_nombre_oficial;
        public Guid ownerid;
        public Guid ownerAnteriorid;
        public string efk_codigo_oficial;
        public int? efk_actividad_economica;
        public int? efk_detalle_actividad_economica;
        public Guid efk_oficina_creacionid;
        public Guid efk_segmentoid;
        public string telephone1;
        public string emailaddress1;
        public decimal? efk_ingreso_mensual;
        public decimal? revenue;
        public int? efk_sexo;
        public int? efk_estado_civil;
        public DateTime? efk_fechadenacimiento;
        public int? efk_nrodehijos;
        public int? efk_nivel_estudios;
        public int? efk_estado_cliente_banco;
        public int? Actual_efk_estado_civil;
        public int? Actual_efk_nrodehijos;
        public DateTime? Actual_efk_fechadenacimiento;
        public decimal? Actual_revenue;
        public decimal? Actual_efk_ingreso_mensual;
        public int? Actual_efk_fuente_ingresos;
        public Guid Actual_efk_segmentoid;

        //--------------------------------------------
        public string efk_numero_nit;
        public decimal? efk_patrimonio_tecnico;
        public DateTime? efk_fecha_creacion_cliente_banco;
        public DateTime? efk_fecha_constitucion_empresa;
        public string efk_calificacion_cliente_banco;
        public bool? efk_atencionpreferente;
        public string efk_descripciondelaatencionpreferente;
        public string efk_telefonomovil;
        public string efk_ciudad;
        public string efk_barrio;
        public string efk_zona;
        public int? efk_aniosdeexperienciaenelmedio;
        public string efk_pais_origen;
        public string efk_profesion;
        public string efk_cargo;
        public string efk_empresa;
        public DateTime? efk_fecha_ingreso_empresa;
        //--------------------------------------------
        //---- Fase 2 --------------------------------

        public Guid efk_sucursalid;

        //--------- Info Adicional -------------------
        public string efk_indicerelacionactual;
        public int? efk_cantidaddeproductosdelactivo;
        public int? efk_cantidaddeproductosdelpasivo;
        public int? efk_cantidaddeoperacionesdeactivoscanceladas;
        public bool? efk_retencionjudicial;
        public bool? efk_cuentacorrienteclausurada;
        public decimal? efk_total_credito_vivienda;
        public decimal? efk_total_credito_vehicular;
        public decimal? efk_total_credito_consumo;
        public decimal? efk_total_credito_comercial;
        public decimal? efk_total_productos_del_activo;
        public decimal? efk_monto_maximo_ultimos_24_meses_bmsc;
        public decimal? efk_monto_maximo_ultimos_24_meses_sf;
        public decimal? efk_volumen_total_negocio_actual;
        public decimal? efk_volumen_promedio_negocio_24_meses;
        public decimal? efk_maximo_volumen_total_negocio_24_meses;
        public decimal? efk_totalgarantias;
        public decimal? efk_total_tarjeta_credito;
        public decimal? efk_monto_total_cuentas_corrientes;
        public decimal? efk_monto_total_cuentas_ahorros;
        public decimal? efk_total_dpf;
        public decimal? efk_total_productos_pasivo;
        public decimal? efk_total_contingentes;
        public decimal? efk_total_central_riesgo_ult_mes;

        public decimal? efk_valor_liquidable_garantias;
        public decimal? efk_total_saldo_arraste_garantias;
        public float? efk_porc_cobertura_garantias_vigente;
        public decimal? efk_total_lineas_credito;
        public decimal? efk_total_cobertura_lineas_credito;
        public float? efk_porc_utilizacion_lineas_credito;
        public decimal? efk_total_riesgo_cliente_bmsc;
        public decimal? efk_total_riesgo_cliente_sf;
        public decimal? efk_total_riesgo_contingente_sf;
        public string efk_calificacion_sf;
        public float? efk_porc_participacion_riesgo_bmsc;
        public double? efk_indice_reciprocidad;
        public float? efk_indice_clv;
        public decimal? efk_total_com_garantias_hipotecarias;
        public decimal? efk_total_com_garantias_depositos;
        public decimal? efk_total_com_garantias_personales;
        public decimal? efk_total_com_garantias_prendarias;
        public decimal? efk_total_com_garantias_otros;
        public decimal? efk_total_riesgo_grupo_economico;
        public bool? efk_posee_creditos_mora;
        public float? efk_indice_liquidez_corto_plazo;
        public float? efk_indice_roe;
        public float? efk_indice_roa;
        public float? efk_indice_ros;
        public float? efk_apalancamiento_financiero;
        public float? efk_apalancamiento_total;
        public decimal? efk_total_depositos_ult_mes;
        public decimal? efk_total_transf_cta_propias_ult_mes;
        public decimal? efk_total_transf_terceros_ult_mes;
        public decimal? efk_total_transf_terceros_otros_ult_mes;
        public decimal? efk_total_depositos_ult_trimestre;
        public decimal? efk_total_transf_ctas_propias_ult_trim;
        public decimal? efk_total_transf_terceros_bmsc_ult_mes;
        
        public string efk_codigo_club_puntos;
        public int? efk_puntos_acumulados_mes1;
        public int? efk_puntos_acumulados_mes2;
        public int? efk_puntos_acumulados_mes3;
        public string efk_estado_club_puntos;
        public int? efk_total_puntos;
        public int? efk_cant_giros_comex_ult_mes;
        public decimal? efk_monto_total_giros_comex_ult_mes;
        public bool? efk_posee_creditos_vencidos;
        public bool? efk_posee_sueldos_salarios;
        public bool? efk_posee_pago_proveedores;
        public bool? efk_posee_pago_colegios;
        public bool? efk_posee_dep_numerados;
        public bool? efk_posee_pagos_automaticos;
        public bool? efk_posee_debitos_automaticos;
        public int? efk_peor_calificacion_sf_ult_12_meses;
        public int? efk_num_veces_calif_distinta_ult_12_meses;
        public int? efk_num_veces_en_ejecucion_ult_12_meses;
        public int? efk_num_veces_envigente;
        public int? efk_num_veces_vencido_insolvencia;
        public decimal? efk_total_cartera_garantia_hipotecaria;
        public decimal? efk_total_cartera_garantia_personal;
        public decimal? efk_total_cartera_a_sola_firma;
        public decimal? efk_total_cuotas_bmsc;
        public int? efk_num_veces_vencido_insolvencia_garante;
        //prioridad 0 fase 1
        public decimal? efk_total_activos;
        public decimal? efk_total_pasivos;
        public bool? efk_tiene_tc;
        public bool? efk_tiene_bxi;
        public bool? efk_tiene_movil;
		public string efk_cod_moneda;
		public int? efk_cantidad_servicio;
		public decimal? efk_volumen_total_negocio_actual_activos;
        public decimal? efk_volumen_total_negocio_actual_contingentes;
        public decimal? efk_volumen_total_negocio_actual_pasivos;
		public int? efk_ivc;
        public string efk_caedec_cod;
        public string efk_caedec_desc;

        // multimoneda

        public Guid transactioncurrencyid;
        public string efk_moneda;
        
        //prioridad 0 fase 2
        public float? efk_dm_capital_inversion;
		public float? efk_dm_capital_operaciones;
		public float? efk_dm_comerciante;
		public float? efk_dm_conyuge;
		public float? efk_dm_cred_consumo;
		public float? efk_dm_cred_vehicular;
		public float? efk_dm_cred_vivienda;
		public float? efk_dm_dependiente;
		public float? efk_dm_edad_022;
		public float? efk_dm_edad_027;
		public float? efk_dm_edad_029;
		public float? efk_dm_edad_039;
		public float? efk_dm_edad_041;
		public float? efk_dm_edad_2334;
		public float? efk_dm_edad_2833;
		public float? efk_dm_edad_3036;
		public float? efk_dm_edad_3057;
		public float? efk_dm_edad_3439;
		public float? efk_dm_edad_3748;
		public float? efk_dm_edad_4047;
		public float? efk_dm_edad_4059;
		public float? efk_dm_edad_4256;
		public float? efk_dm_edad_4859;
		public float? efk_dm_edad_4965;
		public float? efk_dm_edad_5765;
		public float? efk_dm_ingresos_0300;
		public float? efk_dm_ingresos_14562908;
		public float? efk_dm_ingresos_29095000;
		public float? efk_dm_ingresos_3011455;
		public float? efk_dm_juridico;
		public float? efk_dm_masculino;
		public float? efk_dm_productivo;
		public float? efk_dm_region_centro;
		public float? efk_dm_region_occidente;
		public float? efk_dm_region_oriente;
		public float? efk_dm_servicios;
		public float? efk_dm_tarjeta_credito;
		public float? efk_dm_utilizacion_025;
		public float? efk_dm_utilizacion_26335;
		public float? efk_dm_utilizacion_456755;
		public float? efk_dm_utilizacion_mas_755;
		public bool? efk_TieneAlarmaProbable;
		public DateTime? efk_fechacumpleanos;
		public float? efk_LNSaldoPromedioMensualCaptaciones;
		public float? efk_LNVariacionSaldoPromedioMensualCaptaciones;
		public float? efk_NroAdicionales;
		public float? efk_NroIniciales;
		public float? efk_NroReposiciones;
        public float? efk_NroSinSolicitud;
        public bool? efk_NroHijos;
		public float? efk_VecesVigente2TC;
		public float? efk_alarma_vivienda;
		public float? efk_anos_como_cliente;
		public float? efk_cantidad_tx;
		public float? efk_cantidad_tx_td;
        public float? efk_cantidad_tx_tc;
		public int? efk_plazo_transcurrido_credito;
		public int? efk_total_productos_activos;
		public float? efk_TasaCredito;
		public float? efk_LNIngresoMensual;
		public float? efk_LNSaldoCaptaciones;
		public float? efk_LNSaldoCartera;
		public float? efk_LNSaldoCarteraTC;
		public float? efk_LNSaldoCarteraVivienda;
        public float? efk_LNSaldoCarteraModComercial;
        public float? efk_LNSaldoCarteraModVivienda;
        public float? efk_LNSaldoCarteraModConsumo;
        public float? efk_LNSaldoCarteraModVehicular;
        public float? efk_LNSaldoCarteraModTC;
		public DateTime? efk_FechaAlarmaProbable;
		public float? efk_RatioAlarma;
		public int? efk_RangoSaldoInicial;
		public decimal? efk_SumatoriaCuotaVigente;
		public decimal? efk_SumatoriaCuotasMaximas;
        public int? efk_EdadCliente;
        public float? efk_educacion;
        public float? efk_cartera_en_sistema;
        public float? efk_capacidad_de_ahorro;
      

        public void Cargar(CrmService servicio)
        {
            try
            {
                //Creamos el objeto
                account obj = new account();
                if (efk_tipo_cliente != null)
                {
                    obj.efk_tipo_cliente = new Picklist();
                    obj.efk_tipo_cliente.Value = efk_tipo_cliente.Value;
                }
                if (efk_fuente_ingresos != null)
                {
                    obj.efk_fuente_ingresos = new Picklist();
                    obj.efk_fuente_ingresos.Value = this.efk_fuente_ingresos.Value;
                }
                if (efk_tipo_banca != null)
                {
                    obj.efk_tipo_banca = new Picklist();
                    obj.efk_tipo_banca.Value = this.efk_tipo_banca.Value;
                }


                if (efk_tipo_identificacion != null)
                {
                    obj.efk_tipo_identificacion = new Picklist();
                    obj.efk_tipo_identificacion.Value = this.efk_tipo_identificacion.Value;
                }

                if (accountnumber != null)
                    obj.accountnumber = this.accountnumber;
                if (efk_codigo_cliente != null)
                {
                    obj.efk_codigo_cliente = new CrmNumber();
                    obj.efk_codigo_cliente.Value = this.efk_codigo_cliente.Value;
                }
                if (numberofemployees != null)
                {
                    obj.numberofemployees = new CrmNumber();
                    obj.numberofemployees.Value = this.numberofemployees.Value;
                }

                obj.emailaddress1 = (this.emailaddress1 != null ? this.emailaddress1 : string.Empty);
                obj.telephone1 = (this.telephone1 != null ? this.telephone1 : string.Empty);
                obj.efk_telefonomovil = (this.efk_telefonomovil != null ? this.efk_telefonomovil : string.Empty);
                obj.efk_codigo_cliente_texto = (efk_codigo_cliente_texto != null ? efk_codigo_cliente_texto : string.Empty);
                obj.name = (name != null ? name : string.Empty);
                obj.efk_nombre_persona = (efk_nombre_persona != null ? efk_nombre_persona : string.Empty);
                obj.efk_primerapellido = (efk_primerapellido != null ? efk_primerapellido : string.Empty);
                obj.efk_segundoapellido = (efk_segundoapellido != null ? efk_segundoapellido : string.Empty);

                if (efk_tipo_compania != null)
                {
                    obj.efk_tipo_compania = new Picklist();
                    obj.efk_tipo_compania.Value = efk_tipo_compania.Value;
                }
                if (efk_tamano_actividad != null)
                {
                    obj.efk_tamano_actividad = new Picklist();
                    obj.efk_tamano_actividad.Value = efk_tamano_actividad.Value;
                }
                if (efk_oficina_manejoid != Guid.Empty)
                {
                    obj.efk_oficina_manejoid = new Lookup();
                    obj.efk_oficina_manejoid.Value = efk_oficina_manejoid;
                }
                if (efk_nombre_oficial != null)
                    obj.efk_nombre_oficial = efk_nombre_oficial;

                if (efk_codigo_oficial != null)
                    obj.efk_codigo_oficial = efk_codigo_oficial;
                if (efk_actividad_economica != null)
                {
                    obj.efk_actividad_economica = new Picklist();
                    obj.efk_actividad_economica.Value = efk_actividad_economica.Value;
                }
                if (efk_detalle_actividad_economica != null)
                {
                    obj.efk_detalle_actividad_economica = new Picklist();
                    obj.efk_detalle_actividad_economica.Value = efk_detalle_actividad_economica.Value;
                }
                if (efk_oficina_creacionid != Guid.Empty)
                {
                    obj.efk_oficina_creacionid = new Lookup();
                    obj.efk_oficina_creacionid.Value = efk_oficina_creacionid;
                }
                if (efk_segmentoid != Guid.Empty)
                {
                    obj.efk_segmentoid = new Lookup();
                    obj.efk_segmentoid.Value = efk_segmentoid;
                }
                if (this.efk_ingreso_mensual != null)
                {
                    obj.efk_ingreso_mensual = new CrmMoney();
                    obj.efk_ingreso_mensual.Value = efk_ingreso_mensual.Value;
                }
                if (this.revenue != null)
                {
                    obj.revenue = new CrmMoney();
                    obj.revenue.Value = revenue.Value;
                }
                if (efk_sexo != null)
                {
                    obj.efk_sexo = new Picklist();
                    obj.efk_sexo.Value = efk_sexo.Value;
                }
                if (efk_estado_civil != null)
                {
                    obj.efk_estado_civil = new Picklist();
                    obj.efk_estado_civil.Value = efk_estado_civil.Value;
                }
                if (efk_nivel_estudios != null)
                {
                    obj.efk_nivel_estudios = new Picklist();
                    obj.efk_nivel_estudios.Value = efk_nivel_estudios.Value;
                }
                if (efk_estado_cliente_banco != null)
                {
                    obj.efk_estado_cliente_banco = new Picklist();
                    obj.efk_estado_cliente_banco.Value = efk_estado_cliente_banco.Value;
                }
                if (efk_fechadenacimiento != null)
                {
                    obj.efk_fechadenacimiento = new CrmDateTime();
                    obj.efk_fechadenacimiento.Value = efk_fechadenacimiento.Value.ToString(formatoFecha);
                }
                if (efk_nrodehijos != null)
                {
                    obj.efk_nrodehijos = new CrmNumber();
                    obj.efk_nrodehijos.Value = efk_nrodehijos.Value;
                }
                //---Fase 2-----
                if (efk_sucursalid != Guid.Empty)
                {
                    obj.efk_sucursalid = new Lookup();
                    obj.efk_sucursalid.Value = efk_sucursalid;
                }

                

                obj.efk_numeronit = (efk_numero_nit != null ? efk_numero_nit : string.Empty);

                if (efk_patrimonio_tecnico != null)
                {
                    obj.efk_patrimonio_tecnico = new CrmMoney();
                    obj.efk_patrimonio_tecnico.Value = efk_patrimonio_tecnico.Value;
                }

                if (efk_fecha_creacion_cliente_banco != null)
                {
                    obj.efk_fecha_creacion_cliente_banco = new CrmDateTime();
                    obj.efk_fecha_creacion_cliente_banco.Value = efk_fecha_creacion_cliente_banco.Value.ToString(formatoFecha);
                }
                if (efk_fecha_constitucion_empresa != null)
                {
                    obj.efk_fecha_constitucion_empresa = new CrmDateTime();
                    obj.efk_fecha_constitucion_empresa.Value = efk_fecha_constitucion_empresa.Value.ToString(formatoFecha);
                }

                obj.efk_calificacion_cliente_banco = (efk_calificacion_cliente_banco != null ? efk_calificacion_cliente_banco : string.Empty);

                if (efk_atencionpreferente != null)
                {
                    obj.efk_atencionpreferente = new CrmBoolean();
                    obj.efk_atencionpreferente.Value = efk_atencionpreferente.Value;
                }

                obj.efk_descripciondelaatencionpreferente = (efk_descripciondelaatencionpreferente != null ? efk_descripciondelaatencionpreferente : string.Empty);
                obj.efk_telefonomovil = (efk_telefonomovil != null ? efk_telefonomovil : string.Empty);
                obj.efk_ciudad = (efk_ciudad != null ? efk_ciudad : string.Empty);
                obj.efk_barrio = (efk_barrio != null ? efk_barrio : string.Empty);
                obj.efk_zona = (efk_zona != null ? efk_zona : string.Empty);

                if (efk_aniosdeexperienciaenelmedio != null)
                {
                    obj.efk_aniosdeexperienciaenelmedio = new CrmNumber();
                    obj.efk_aniosdeexperienciaenelmedio.Value = efk_aniosdeexperienciaenelmedio.Value;
                }

                obj.efk_pais_origen = (efk_pais_origen != null ? efk_pais_origen : string.Empty);
                obj.efk_profesion = (efk_profesion != null ? efk_profesion : string.Empty);
                obj.efk_cargo = (efk_cargo != null ? efk_cargo : string.Empty);
                obj.efk_nombre_empresa = (efk_empresa != null ? efk_empresa : string.Empty);

                if (efk_fecha_ingreso_empresa != null)
                {
                    obj.efk_fecha_ingreso_empresa = new CrmDateTime();
                    obj.efk_fecha_ingreso_empresa.Value = efk_fecha_ingreso_empresa.Value.ToString(formatoFecha);
                }



                #region Campos ratificados
                ///Aplicamos la lógica para indicar si los valores de los campos de oferta de valor han sido ratificados o no
                if (this.efk_estado_civil != null && this.efk_estado_civil != this.Actual_efk_estado_civil)
                {
                    obj.efk_estado_civil_ov = new Picklist();
                    obj.efk_estado_civil_ov.Value = this.efk_estado_civil.Value;

                    obj.efk_estado_civil_ratificado = new CrmBoolean();
                    obj.efk_estado_civil_ratificado.Value = true;
                }
                if (this.efk_nrodehijos != null && this.efk_nrodehijos != this.Actual_efk_nrodehijos)
                {
                    obj.efk_nrodehijos_ov = new CrmNumber();
                    obj.efk_nrodehijos_ov.Value = this.efk_nrodehijos.Value;

                    obj.efk_nro_hijos_ratificado = new CrmBoolean();
                    obj.efk_nro_hijos_ratificado.Value = true;
                }
                if (this.efk_fechadenacimiento != null && this.efk_fechadenacimiento != this.Actual_efk_fechadenacimiento)
                {
                    obj.efk_fechadenacimiento_ov = new CrmDateTime();
                    obj.efk_fechadenacimiento_ov.Value = this.efk_fechadenacimiento.Value.ToString(formatoFecha);

                    obj.efk_fecha_nacimiento_ratificado = new CrmBoolean();
                    obj.efk_fecha_nacimiento_ratificado.Value = true;
                }
                if (this.efk_fuente_ingresos != null && this.efk_fuente_ingresos != this.Actual_efk_fuente_ingresos)
                {
                    obj.efk_fuente_ingresos_ov = new Picklist();
                    obj.efk_fuente_ingresos_ov.Value = this.efk_fuente_ingresos.Value;

                    obj.efk_fuente_ingresos_ratificado = new CrmBoolean();
                    obj.efk_fuente_ingresos_ratificado.Value = true;
                }
                if (this.efk_segmentoid != Guid.Empty && this.efk_segmentoid != this.Actual_efk_segmentoid)
                {
                    obj.efk_segmento_ovid = new Lookup();
                    obj.efk_segmento_ovid.Value = efk_segmentoid;
                }

                if (this.efk_ingreso_mensual != null && this.efk_ingreso_mensual != this.Actual_efk_ingreso_mensual)
                {
                    obj.efk_ingresos_ov = new CrmMoney();
                    obj.efk_ingresos_ov.Value = this.efk_ingreso_mensual.Value;

                    obj.efk_ingresos_ratificado = new CrmBoolean();
                    obj.efk_ingresos_ratificado.Value = true;
                }
                else
                {
                    if (this.revenue != null && this.revenue != this.Actual_revenue)
                    {
                        if (this.efk_tipo_cliente.HasValue && this.efk_tipo_banca.HasValue)
                        {
                            if (this.efk_tipo_cliente.Value == 221220001 || this.efk_tipo_banca.Value == 221220000)
                            {
                                obj.efk_ingresos_ov = new CrmMoney();
                                obj.efk_ingresos_ov.Value = this.revenue.Value;

                                obj.efk_ingresos_ratificado = new CrmBoolean();
                                obj.efk_ingresos_ratificado.Value = true;
                            }
                            else
                            {
                                if (this.efk_ingreso_mensual != null)
                                {
                                    obj.efk_ingresos_ov = new CrmMoney();
                                    obj.efk_ingresos_ov.Value = this.efk_ingreso_mensual.Value;
                                }
                            }
                        }
                        else
                        {
                            if (this.efk_ingreso_mensual != null)
                            {
                                obj.efk_ingresos_ov = new CrmMoney();
                                obj.efk_ingresos_ov.Value = this.efk_ingreso_mensual.Value;
                            }
                        }
                    }
                    else
                    {
                        if (this.revenue != null)
                        {
                            if (this.efk_tipo_cliente.HasValue && this.efk_tipo_banca.HasValue)
                            {
                                if (this.efk_tipo_cliente.Value == 221220001 || this.efk_tipo_banca.Value == 221220000)
                                {
                                    obj.efk_ingresos_ov = new CrmMoney();
                                    obj.efk_ingresos_ov.Value = this.revenue.Value;
                                }
                                else
                                {
                                    if (this.efk_ingreso_mensual != null)
                                    {
                                        obj.efk_ingresos_ov = new CrmMoney();
                                        obj.efk_ingresos_ov.Value = this.efk_ingreso_mensual.Value;
                                    }
                                }
                            }
                            else
                            {
                                if (this.efk_ingreso_mensual != null)
                                {
                                    obj.efk_ingresos_ov = new CrmMoney();
                                    obj.efk_ingresos_ov.Value = this.efk_ingreso_mensual.Value;
                                }
                            }
                        }
                        else
                        {
                            if (this.efk_ingreso_mensual != null)
                            {
                                obj.efk_ingresos_ov = new CrmMoney();
                                obj.efk_ingresos_ov.Value = this.efk_ingreso_mensual.Value;
                            }
                        }
                    }
                }



                #endregion

                if (ownerid != Guid.Empty)
                {
                    obj.ownerid = new Owner();
                    obj.ownerid.type = "systemuser";
                    obj.ownerid.Value = ownerid;
                }
                obj.efk_cliente_mis = new CrmBoolean();
                obj.efk_cliente_mis.Value = true;

                if (this.accountid != Guid.Empty)
                {
                    //Actualizamos
                    obj.accountid = new Key();
                    obj.accountid.Value = accountid;

                    servicio.Update(obj);

                    if (ownerid != Guid.Empty && ownerAnteriorid != ownerid)
                    {
                        //reasignamos el cliente
                        AssignRequest req = new AssignRequest();
                        TargetOwnedAccount acc = new TargetOwnedAccount();
                        acc.EntityId = obj.accountid.Value;
                        req.Target = acc;
                        SecurityPrincipal sp = new SecurityPrincipal();
                        sp.Type = SecurityPrincipalType.User;
                        sp.PrincipalId = ownerid;
                        req.Assignee = sp;

                        servicio.Execute(req);

                    }
                }
                else
                {
                    //Colocamos la divisa por defecto, por temas de rendimiento se utiliza la táctica de colocar el valor en 0 de un campo no utilizado en el modelo de CRM.
                    obj.marketcap = new CrmMoney();
                    obj.marketcap.Value = 0;

                    this.accountid = servicio.Create(obj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CargarInfoAdicional(CrmService servicio)
        {
            try
            {
                //Creamos el objeto
                account obj = new account();
                string usd_cod_iso = "USD";

                #region Info Adicional

                obj.efk_indice_relacion_actual = (efk_indicerelacionactual != null ? efk_indicerelacionactual : string.Empty);

                if (efk_cantidaddeproductosdelactivo != null)
                {
                    obj.efk_cantidaddeproductosdelactivo = new CrmNumber();
                    obj.efk_cantidaddeproductosdelactivo.Value = efk_cantidaddeproductosdelactivo.Value;
                }
                if (efk_cantidaddeproductosdelpasivo != null)
                {
                    obj.efk_cantidaddeproductosdelpasivo = new CrmNumber();
                    obj.efk_cantidaddeproductosdelpasivo.Value = efk_cantidaddeproductosdelpasivo.Value;
                }
                if (efk_cantidaddeoperacionesdeactivoscanceladas != null)
                {
                    obj.efk_cantidaddeoperacionesdeactivoscanceladas = new CrmNumber();
                    obj.efk_cantidaddeoperacionesdeactivoscanceladas.Value = efk_cantidaddeoperacionesdeactivoscanceladas.Value;
                }
                if (efk_retencionjudicial != null)
                {
                    obj.efk_retencionjudicial = new CrmBoolean();
                    obj.efk_retencionjudicial.Value = efk_retencionjudicial.Value;
                }
                if (efk_cuentacorrienteclausurada != null)
                {
                    obj.efk_cuentacorrienteclausurada = new CrmBoolean();
                    obj.efk_cuentacorrienteclausurada.Value = efk_cuentacorrienteclausurada.Value;
                }
                if (efk_total_credito_vivienda != null)
                {
                    obj.efk_total_credito_vivienda = new CrmMoney();
                    obj.efk_total_credito_vivienda.Value = efk_total_credito_vivienda.Value;
                }
                if (efk_total_credito_vehicular != null)
                {
                    obj.efk_total_credito_vehicular = new CrmMoney();
                    obj.efk_total_credito_vehicular.Value = efk_total_credito_vehicular.Value;
                }
                if (efk_total_credito_consumo != null)
                {
                    obj.efk_total_credito_consumo = new CrmMoney();
                    obj.efk_total_credito_consumo.Value = efk_total_credito_consumo.Value;
                }
                if (efk_total_credito_comercial != null)
                {
                    obj.efk_total_credito_comercial = new CrmMoney();
                    obj.efk_total_credito_comercial.Value = efk_total_credito_comercial.Value;
                }
                if (efk_total_productos_del_activo != null)
                {
                    obj.efk_total_productos_del_activo = new CrmMoney();
                    obj.efk_total_productos_del_activo.Value = efk_total_productos_del_activo.Value;
                }
                if (efk_monto_maximo_ultimos_24_meses_bmsc != null)
                {
                    obj.efk_monto_maximo_ultimos_24_meses_bmsc = new CrmMoney();
                    obj.efk_monto_maximo_ultimos_24_meses_bmsc.Value = efk_monto_maximo_ultimos_24_meses_bmsc.Value;
                }
                if (efk_monto_maximo_ultimos_24_meses_sf != null)
                {
                    obj.efk_monto_maximo_deuda_24_meses_sf = new CrmMoney();
                    obj.efk_monto_maximo_deuda_24_meses_sf.Value = efk_monto_maximo_ultimos_24_meses_sf.Value;
                }
                if (efk_volumen_total_negocio_actual != null)
                {
                    obj.efk_volumen_total_negocio_actual = new CrmMoney();
                    obj.efk_volumen_total_negocio_actual.Value = efk_volumen_total_negocio_actual.Value;
                }
                if (efk_volumen_promedio_negocio_24_meses != null)
                {
                    obj.efk_volumen_promedio_negocio_24_meses = new CrmMoney();
                    obj.efk_volumen_promedio_negocio_24_meses.Value = efk_volumen_promedio_negocio_24_meses.Value;
                }
                if (efk_maximo_volumen_total_negocio_24_meses != null)
                {
                    obj.efk_maximo_volumen_total_negocio_24_meses = new CrmMoney();
                    obj.efk_maximo_volumen_total_negocio_24_meses.Value = efk_maximo_volumen_total_negocio_24_meses.Value;
                }
                if (efk_totalgarantias != null)
                {
                    obj.efk_totalgarantas = new CrmMoney();
                    obj.efk_totalgarantas.Value = efk_totalgarantias.Value;
                }
                if (efk_total_tarjeta_credito != null)
                {
                    obj.efk_total_tarjeta_credito = new CrmMoney();
                    obj.efk_total_tarjeta_credito.Value = efk_total_tarjeta_credito.Value;
                }
                if (efk_monto_total_cuentas_corrientes != null)
                {
                    obj.efk_monto_total_cuentas_corrientes = new CrmMoney();
                    obj.efk_monto_total_cuentas_corrientes.Value = efk_monto_total_cuentas_corrientes.Value;
                }
                if (efk_monto_total_cuentas_ahorros != null)
                {
                    obj.efk_monto_total_cuentas_ahorros = new CrmMoney();
                    obj.efk_monto_total_cuentas_ahorros.Value = efk_monto_total_cuentas_ahorros.Value;
                }
                if (efk_total_dpf != null)
                {
                    obj.efk_total_dpf = new CrmMoney();
                    obj.efk_total_dpf.Value = efk_total_dpf.Value;
                }
                if (efk_total_productos_pasivo != null)
                {
                    obj.efk_total_productos_pasivo = new CrmMoney();
                    obj.efk_total_productos_pasivo.Value = efk_total_productos_pasivo.Value;
                }
                if (efk_total_contingentes != null)
                {
                    obj.efk_total_contingentes = new CrmMoney();
                    obj.efk_total_contingentes.Value = efk_total_contingentes.Value;
                }
                if (efk_total_central_riesgo_ult_mes != null)
                {
                    obj.efk_total_central_riesgo_ult_mes = new CrmMoney();
                    obj.efk_total_central_riesgo_ult_mes.Value = efk_total_central_riesgo_ult_mes.Value;
                }
                #endregion

                #region Info Adicional 2
                if (efk_valor_liquidable_garantias != null)
                {
                    obj.efk_monto_total_valor_liquidable_garantia = new CrmMoney();
                    obj.efk_monto_total_valor_liquidable_garantia.Value = efk_valor_liquidable_garantias.Value;
                }
                if (efk_total_saldo_arraste_garantias != null)
                {
                    obj.efk_monto_total_saldo_arrastre_garantias = new CrmMoney();
                    obj.efk_monto_total_saldo_arrastre_garantias.Value = efk_total_saldo_arraste_garantias.Value;
                }
                if (efk_porc_cobertura_garantias_vigente != null)
                {
                    obj.efk_porc_cobertura_garantias_riesgo = new CrmFloat();
                    obj.efk_porc_cobertura_garantias_riesgo.Value = efk_porc_cobertura_garantias_vigente.Value;
                }
                if (efk_total_lineas_credito != null)
                {
                    obj.efk_monto_total_linea_creditos_vigente = new CrmMoney();
                    obj.efk_monto_total_linea_creditos_vigente.Value = efk_total_lineas_credito.Value;
                }
                if (efk_total_cobertura_lineas_credito != null)
                {
                    obj.efk_monto_total_cobertura_lin_cred_vigentes = new CrmMoney();
                    obj.efk_monto_total_cobertura_lin_cred_vigentes.Value = efk_total_cobertura_lineas_credito.Value;
                }
                if (efk_porc_utilizacion_lineas_credito != null)
                {
                    obj.efk_porc_utilizacion_lineas_credito_vigentes = new CrmFloat();
                    obj.efk_porc_utilizacion_lineas_credito_vigentes.Value = efk_porc_utilizacion_lineas_credito.Value;
                }
                if (efk_total_riesgo_cliente_bmsc != null)
                {
                    obj.efk_monto_total_riesgo_cliente_bmsc = new CrmMoney();
                    obj.efk_monto_total_riesgo_cliente_bmsc.Value = efk_total_riesgo_cliente_bmsc.Value;
                }
                if (efk_total_riesgo_cliente_sf != null)
                {
                    obj.efk_monto_total_riesgo_cliente_corp_bmsc = new CrmMoney();
                    obj.efk_monto_total_riesgo_cliente_corp_bmsc.Value = efk_total_riesgo_cliente_sf.Value;
                }
                if (efk_total_riesgo_contingente_sf != null)
                {
                    obj.efk_monto_total_riesgo_contingente_sf = new CrmMoney();
                    obj.efk_monto_total_riesgo_contingente_sf.Value = efk_total_riesgo_contingente_sf.Value;
                }
                if (efk_calificacion_sf != null)
                {
                    obj.efk_calificacion_cliente_sf = efk_calificacion_sf;
                }
                if (efk_porc_participacion_riesgo_bmsc != null)
                {
                    obj.efk_porc_participacion_bmsc_riesgo_cliente = new CrmFloat();
                    obj.efk_porc_participacion_bmsc_riesgo_cliente.Value = efk_porc_participacion_riesgo_bmsc.Value;
                }
                if (efk_indice_reciprocidad != null)
                {
                    obj.efk_indice_reprocidad = new CrmFloat();
                    obj.efk_indice_reprocidad.Value = efk_indice_reciprocidad.Value;
                }
                if (efk_indice_clv != null)
                {
                    obj.efk_clv = new CrmFloat();
                    obj.efk_clv.Value = efk_indice_clv.Value;
                }
                if (efk_total_com_garantias_hipotecarias != null)
                {
                    obj.efk_total_valor_comercial_garantias_hipoteca = new CrmMoney();
                    obj.efk_total_valor_comercial_garantias_hipoteca.Value = efk_total_com_garantias_hipotecarias.Value;
                }
                if (efk_total_com_garantias_depositos != null)
                {
                    obj.efk_total_valor_comercial_garantias_depositos = new CrmMoney();
                    obj.efk_total_valor_comercial_garantias_depositos.Value = efk_total_com_garantias_depositos.Value;
                }
                if (efk_total_com_garantias_personales != null)
                {
                    obj.efk_total_valor_comercial_garantias_personal = new CrmMoney();
                    obj.efk_total_valor_comercial_garantias_personal.Value = efk_total_com_garantias_personales.Value;
                }
                if (efk_total_com_garantias_prendarias != null)
                {
                    obj.efk_total_valor_comercial_garantias_prendaria = new CrmMoney();
                    obj.efk_total_valor_comercial_garantias_prendaria.Value = efk_total_com_garantias_prendarias.Value;
                }
                if (efk_total_com_garantias_otros != null)
                {
                    obj.efk_total_valor_comercial_garantias_otros = new CrmMoney();
                    obj.efk_total_valor_comercial_garantias_otros.Value = efk_total_com_garantias_otros.Value;
                }
                if (efk_total_riesgo_grupo_economico != null)
                {
                    obj.efk_monto_total_riesgo_grupo_economico = new CrmMoney();
                    obj.efk_monto_total_riesgo_grupo_economico.Value = efk_total_riesgo_grupo_economico.Value;
                }
                if (efk_posee_creditos_mora != null)
                {
                    obj.efk_tiene_creditos_en_mora = new CrmBoolean();
                    obj.efk_tiene_creditos_en_mora.Value = efk_posee_creditos_mora.Value;
                }
                if (efk_indice_liquidez_corto_plazo != null)
                {
                    obj.efk_liquidez_corto_plazo = new CrmFloat();
                    obj.efk_liquidez_corto_plazo.Value = efk_indice_liquidez_corto_plazo.Value;
                }
                if (efk_indice_roe != null)
                {
                    obj.efk_roe = new CrmFloat();
                    obj.efk_roe.Value = efk_indice_roe.Value;
                }
                if (efk_indice_roa != null)
                {
                    obj.efk_roa = new CrmFloat();
                    obj.efk_roa.Value = efk_indice_roa.Value;
                }
                if (efk_indice_ros != null)
                {
                    obj.efk_ros = new CrmFloat();
                    obj.efk_ros.Value = efk_indice_ros.Value;
                }
                if (efk_apalancamiento_financiero != null)
                {
                    obj.efk_apalancamiento_financiero = new CrmFloat();
                    obj.efk_apalancamiento_financiero.Value = efk_apalancamiento_financiero.Value;
                }
                if (efk_apalancamiento_total != null)
                {
                    obj.efk_apalancamiento_total = new CrmFloat();
                    obj.efk_apalancamiento_total.Value = efk_apalancamiento_total.Value;
                }
                if (efk_total_depositos_ult_mes != null)
                {
                    obj.efk_monto_total_depositos_cuentas_ult_mes = new CrmMoney();
                    obj.efk_monto_total_depositos_cuentas_ult_mes.Value = efk_total_depositos_ult_mes.Value;
                }
                if (efk_total_transf_cta_propias_ult_mes != null)
                {
                    obj.efk_monto_total_transf_ctas_propias_ult_mes = new CrmMoney();
                    obj.efk_monto_total_transf_ctas_propias_ult_mes.Value = efk_total_transf_cta_propias_ult_mes.Value;
                }
                if (efk_total_transf_terceros_ult_mes != null)
                {
                    obj.efk_monto_total_transf_terceros_ult_mes = new CrmMoney();
                    obj.efk_monto_total_transf_terceros_ult_mes.Value = efk_total_transf_terceros_ult_mes.Value;
                }
                if (efk_total_transf_terceros_otros_ult_mes != null)
                {
                    obj.efk_monto_total_transf_terceros_otros_ult_mes = new CrmMoney();
                    obj.efk_monto_total_transf_terceros_otros_ult_mes.Value = efk_total_transf_terceros_otros_ult_mes.Value;
                }
                if (efk_total_depositos_ult_trimestre != null)
                {
                    obj.efk_monto_total_depositos_cuentas_ult_3_mes = new CrmMoney();
                    obj.efk_monto_total_depositos_cuentas_ult_3_mes.Value = efk_total_depositos_ult_trimestre.Value;
                }
                if (efk_total_transf_ctas_propias_ult_trim != null)
                {
                    obj.efk_monto_total_transf_ctas_propias_ult_3_mes = new CrmMoney();
                    obj.efk_monto_total_transf_ctas_propias_ult_3_mes.Value = efk_total_transf_ctas_propias_ult_trim.Value;
                }
                if (efk_total_transf_terceros_bmsc_ult_mes != null)
                {
                    obj.efk_monto_total_transf_terceros_ult_3_mes = new CrmMoney();
                    obj.efk_monto_total_transf_terceros_ult_3_mes.Value = efk_total_transf_terceros_bmsc_ult_mes.Value;
                }
                if (efk_total_transf_terceros_otros_ult_mes != null)
                {
                    obj.efk_monto_total_transf_terceros_otros_ult_3_m = new CrmMoney();
                    obj.efk_monto_total_transf_terceros_otros_ult_3_m.Value = efk_total_transf_terceros_otros_ult_mes.Value;
                }
                if (efk_codigo_club_puntos != null)
                    obj.efk_codigo_club_puntos = efk_codigo_club_puntos;

                if (efk_puntos_acumulados_mes1 != null)
                {
                    obj.efk_cant_puntos_acum_mes_uno = new CrmNumber();
                    obj.efk_cant_puntos_acum_mes_uno.Value = efk_puntos_acumulados_mes1.Value;
                }
                if (efk_puntos_acumulados_mes2 != null)
                {
                    obj.efk_cant_puntos_acum_mes_dos = new CrmNumber();
                    obj.efk_cant_puntos_acum_mes_dos.Value = efk_puntos_acumulados_mes2.Value;
                }
                if (efk_puntos_acumulados_mes3 != null)
                {
                    obj.efk_cant_puntos_acum_mes_tres = new CrmNumber();
                    obj.efk_cant_puntos_acum_mes_tres.Value = efk_puntos_acumulados_mes3.Value;
                }
                if (efk_estado_club_puntos != null)
                {
                    obj.efk_estado_puntos = efk_estado_club_puntos;
                }

                if (efk_total_puntos != null)
                {
                    obj.efk_total_puntos = new CrmNumber();
                    obj.efk_total_puntos.Value = efk_total_puntos.Value;
                }
                if (efk_cant_giros_comex_ult_mes != null)
                {
                    obj.efk_cant_trans_giros_comex_ult_mes = new CrmNumber();
                    obj.efk_cant_trans_giros_comex_ult_mes.Value = efk_cant_giros_comex_ult_mes.Value;
                }
                if (efk_monto_total_giros_comex_ult_mes != null)
                {
                    obj.efk_monto_total_giros_comex_ult_mes = new CrmMoney();
                    obj.efk_monto_total_giros_comex_ult_mes.Value = efk_monto_total_giros_comex_ult_mes.Value;
                }
                if (efk_posee_creditos_vencidos != null)
                {
                    obj.efk_posee_creditos_vencidos_o_ejec = new CrmBoolean();
                    obj.efk_posee_creditos_vencidos_o_ejec.Value = efk_posee_creditos_vencidos.Value;
                }
                if (efk_posee_sueldos_salarios != null)
                {
                    obj.efk_posee_pago_sueldos_salarios = new CrmBoolean();
                    obj.efk_posee_pago_sueldos_salarios.Value = efk_posee_sueldos_salarios.Value;
                }
                if (efk_posee_pago_proveedores != null)
                {
                    obj.efk_posee_pago_proveedores = new CrmBoolean();
                    obj.efk_posee_pago_proveedores.Value = efk_posee_pago_proveedores.Value;
                }
                if (efk_posee_pago_colegios != null)
                {
                    obj.efk_posee_pago_colegios_pensiones = new CrmBoolean();
                    obj.efk_posee_pago_colegios_pensiones.Value = efk_posee_pago_colegios.Value;
                }
                if (efk_posee_dep_numerados != null)
                {
                    obj.efk_posee_depositos_numerados = new CrmBoolean();
                    obj.efk_posee_depositos_numerados.Value = efk_posee_dep_numerados.Value;
                }
                if (efk_posee_pagos_automaticos != null)
                {
                    obj.efk_posee_pagos_automaticos = new CrmBoolean();
                    obj.efk_posee_pagos_automaticos.Value = efk_posee_pagos_automaticos.Value;
                }
                if (efk_posee_debitos_automaticos != null)
                {
                    obj.efk_posee_debitos_automaticos = new CrmBoolean();
                    obj.efk_posee_debitos_automaticos.Value = efk_posee_debitos_automaticos.Value;
                }
                if (efk_peor_calificacion_sf_ult_12_meses != null)
                {
                    obj.efk_peor_calificacion_12meses = new Picklist();
                    obj.efk_peor_calificacion_12meses.Value = efk_peor_calificacion_sf_ult_12_meses.Value;
                }
                if (efk_num_veces_calif_distinta_ult_12_meses != null)
                {
                    obj.efk_nroveces_calificacion_distintaa = new CrmNumber();
                    obj.efk_nroveces_calificacion_distintaa.Value = efk_num_veces_calif_distinta_ult_12_meses.Value;
                }
                if (efk_num_veces_en_ejecucion_ult_12_meses != null)
                {
                    obj.efk_nroveces_vencido_ejecucion = new CrmNumber();
                    obj.efk_nroveces_vencido_ejecucion.Value = efk_num_veces_en_ejecucion_ult_12_meses.Value;
                }
                if (efk_num_veces_envigente != null)
                {
                    obj.efk_nroveces_vigente2_14dias = new CrmNumber();
                    obj.efk_nroveces_vigente2_14dias.Value = efk_num_veces_envigente.Value;
                }
                if (efk_num_veces_vencido_insolvencia != null)
                {
                    obj.efk_nroveces_vencido_ejecucion_titular = new CrmNumber();
                    obj.efk_nroveces_vencido_ejecucion_titular.Value = efk_num_veces_vencido_insolvencia.Value;
                }
                if (efk_total_cartera_garantia_hipotecaria != null)
                {
                    obj.efk_total_cartera_garantia_hipotecaria = new CrmMoney();
                    obj.efk_total_cartera_garantia_hipotecaria.Value = efk_total_cartera_garantia_hipotecaria.Value;
                }
                if (efk_total_cartera_garantia_personal != null)
                {
                    obj.efk_total_cartera_garantia_personal = new CrmMoney();
                    obj.efk_total_cartera_garantia_personal.Value = efk_total_cartera_garantia_personal.Value;
                }
                if (efk_total_cartera_a_sola_firma != null)
                {
                    obj.efk_total_cartera_asolafirma = new CrmMoney();
                    obj.efk_total_cartera_asolafirma.Value = efk_total_cartera_a_sola_firma.Value;
                }
                if (efk_total_cuotas_bmsc != null)
                {
                    obj.efk_cuotas_bmsc = new CrmMoney();
                    obj.efk_cuotas_bmsc.Value = efk_total_cuotas_bmsc.Value;
                }

                if (efk_num_veces_vencido_insolvencia_garante != null)
                {
                    obj.efk_nroveces_vencido_ejecucion_garante = new CrmNumber();
                    obj.efk_nroveces_vencido_ejecucion_garante.Value = efk_num_veces_vencido_insolvencia_garante.Value;
                }
                #endregion

                #region "Prioridad 0 Fase 1"
                if (efk_total_activos != null)
                {
                    obj.efk_total_activos = new CrmMoney();
                    obj.efk_total_activos.Value = efk_total_activos.Value;
                }

                if (efk_total_pasivos != null)
                {
                    obj.efk_total_pasivos = new CrmMoney();
                    obj.efk_total_pasivos.Value = efk_total_pasivos.Value;
                }

                if (efk_tiene_tc != null)
                {
                    obj.efk_tiene_tc = new CrmBoolean();
                    obj.efk_tiene_tc.Value = efk_tiene_tc.Value;
                }

                if (efk_tiene_bxi != null)
                {
                    obj.efk_tiene_bxi = new CrmBoolean();
                    obj.efk_tiene_bxi.Value = efk_tiene_bxi.Value;
                }

                if (efk_tiene_movil != null)
                {
                    obj.efk_tiene_movil = new CrmBoolean();
                    obj.efk_tiene_movil.Value = efk_tiene_movil.Value;
                }

                if (efk_cantidad_servicio != null)
                {
                    obj.efk_cantidad_servicio = new CrmNumber();
                    obj.efk_cantidad_servicio.Value = efk_cantidad_servicio.Value;
                }

                if (efk_volumen_total_negocio_actual_activos != null)
                {
                    obj.efk_volumen_total_negocio_actual_activo = new CrmMoney();
                    obj.efk_volumen_total_negocio_actual_activo.Value = efk_volumen_total_negocio_actual_activos.Value;
                }

                if (efk_volumen_total_negocio_actual_contingentes != null)
                {
                    obj.efk_volumen_total_negocio_actual_contingencia = new CrmMoney();
                    obj.efk_volumen_total_negocio_actual_contingencia.Value = efk_volumen_total_negocio_actual_contingentes.Value;
                }

                if (efk_volumen_total_negocio_actual_pasivos != null)
                {
                    obj.efk_volumen_total_negocio_actual_pasivo = new CrmMoney();
                    obj.efk_volumen_total_negocio_actual_pasivo.Value = efk_volumen_total_negocio_actual_pasivos.Value;
                }
                
                if (efk_ivc != null)
                {
                    obj.efk_ivc = new CrmNumber();
                    obj.efk_ivc.Value = efk_ivc.Value;
                }

                obj.efk_caedec_cod = efk_caedec_cod;
                obj.efk_caedec_desc = efk_caedec_desc;

                // Multimoneda

                if (efk_moneda == null || string.IsNullOrEmpty(efk_moneda))
                {
                    efk_moneda = usd_cod_iso;
                }
                   
                try
                {
                    DivisaCRM divisa = new DivisaCRM(servicio);
                    obj.transactioncurrencyid = new Lookup();
                    obj.transactioncurrencyid.Value = divisa.GetIdDivisa(efk_moneda);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + " en Registro: " + (this.efk_codigo_cliente == null ? string.Empty : this.efk_codigo_cliente.Value.ToString()));
                }
                

                ////////
                #endregion


                #region "Prioridad 0 Fase 2"
                if (efk_dm_capital_inversion != null)
                {
                    obj.efk_dm_capital_inversion = new CrmFloat();
                    obj.efk_dm_capital_inversion.Value = efk_dm_capital_inversion.Value;
                }
		        
                if ( efk_dm_capital_operaciones != null)
                {
                    obj.efk_dm_capital_operaciones = new CrmFloat();
                    obj.efk_dm_capital_operaciones.Value = efk_dm_capital_operaciones.Value;
                }

		        if ( efk_dm_comerciante != null)
                {
                    obj.efk_dm_comerciante = new CrmFloat();
                    obj.efk_dm_comerciante.Value = efk_dm_comerciante.Value;
                }

                if (efk_dm_conyuge != null)
                {
                    obj.efk_dm_conyuge = new CrmFloat();
                    obj.efk_dm_conyuge.Value = efk_dm_conyuge.Value;
                }

                if (efk_dm_cred_consumo != null)
                {
                    obj.efk_dm_cred_consumo = new CrmFloat();
                    obj.efk_dm_cred_consumo.Value = efk_dm_cred_consumo.Value;
                }

                if (efk_dm_cred_vehicular != null)
                {
                    obj.efk_dm_cred_vehicular = new CrmFloat();
                    obj.efk_dm_cred_vehicular.Value = efk_dm_cred_vehicular.Value;
                }

                if (efk_dm_cred_vivienda != null)
                {
                    obj.efk_dm_cred_vivienda = new CrmFloat();
                    obj.efk_dm_cred_vivienda.Value = efk_dm_cred_vivienda.Value;
                }

                if (efk_dm_dependiente != null)
                {
                    obj.efk_dm_dependiente = new CrmFloat();
                    obj.efk_dm_dependiente.Value = efk_dm_dependiente.Value;
                }

                if (efk_dm_edad_022 != null)
                {
                    obj.efk_dm_edad_022 = new CrmFloat();
                    obj.efk_dm_edad_022.Value = efk_dm_edad_022.Value;
                }

                if (efk_dm_edad_027 != null)
                {
                    obj.efk_dm_edad_027 = new CrmFloat();
                    obj.efk_dm_edad_027.Value = efk_dm_edad_027.Value;
                }

                if (efk_dm_edad_029 != null)
                {
                    obj.efk_dm_edad_029 = new CrmFloat();
                    obj.efk_dm_edad_029.Value = efk_dm_edad_029.Value;
                }

                if (efk_dm_edad_039 != null)
                {
                    obj.efk_dm_edad_039 = new CrmFloat();
                    obj.efk_dm_edad_039.Value = efk_dm_edad_039.Value;
                }

                if (efk_dm_edad_041 != null)
                {
                    obj.efk_dm_edad_041 = new CrmFloat();
                    obj.efk_dm_edad_041.Value = efk_dm_edad_041.Value;
                }

                if (efk_dm_edad_2334 != null)
                {
                    obj.efk_dm_edad_2334 = new CrmFloat();
                    obj.efk_dm_edad_2334.Value = efk_dm_edad_2334.Value;
                }

                if (efk_dm_edad_2833 != null)
                {
                    obj.efk_dm_edad_2833 = new CrmFloat();
                    obj.efk_dm_edad_2833.Value = efk_dm_edad_2833.Value;
                }

                if (efk_dm_edad_3036 != null)
                {
                    obj.efk_dm_edad_3036 = new CrmFloat();
                    obj.efk_dm_edad_3036.Value = efk_dm_edad_3036.Value;
                }

                if (efk_dm_edad_3057 != null)
                {
                    obj.efk_dm_edad_3057 = new CrmFloat();
                    obj.efk_dm_edad_3057.Value = efk_dm_edad_3057.Value;
                }

                if (efk_dm_edad_3439 != null)
                {
                    obj.efk_dm_edad_3439 = new CrmFloat();
                    obj.efk_dm_edad_3439.Value = efk_dm_edad_3439.Value;
                }

                if (efk_dm_edad_3748 != null)
                {
                    obj.efk_dm_edad_3748 = new CrmFloat();
                    obj.efk_dm_edad_3748.Value = efk_dm_edad_3748.Value;
                }

                if (efk_dm_edad_4047 != null)
                {
                    obj.efk_dm_edad_4047 = new CrmFloat();
                    obj.efk_dm_edad_4047.Value = efk_dm_edad_4047.Value;
                }

                if (efk_dm_edad_4059 != null)
                {
                    obj.efk_dm_edad_4059 = new CrmFloat();
                    obj.efk_dm_edad_4059.Value = efk_dm_edad_4059.Value;
                }

                if (efk_dm_edad_4256 != null)
                {
                    obj.efk_dm_edad_4256 = new CrmFloat();
                    obj.efk_dm_edad_4256.Value = efk_dm_edad_4256.Value;
                }

                if (efk_dm_edad_4859 != null)
                {
                    obj.efk_dm_edad_4859 = new CrmFloat();
                    obj.efk_dm_edad_4859.Value = efk_dm_edad_4859.Value;
                }

                if (efk_dm_edad_4965 != null)
                {
                    obj.efk_dm_edad_4965 = new CrmFloat();
                    obj.efk_dm_edad_4965.Value = efk_dm_edad_4965.Value;
                }

                if (efk_dm_edad_5765 != null)
                {
                    obj.efk_dm_edad_5765 = new CrmFloat();
                    obj.efk_dm_edad_5765.Value = efk_dm_edad_5765.Value;
                }

                if (efk_dm_ingresos_0300 != null)
                {
                    obj.efk_dm_ingresos_0300 = new CrmFloat();
                    obj.efk_dm_ingresos_0300.Value = efk_dm_ingresos_0300.Value;
                }

                if (efk_dm_ingresos_14562908 != null)
                {
                    obj.efk_dm_ingresos_14562908 = new CrmFloat();
                    obj.efk_dm_ingresos_14562908.Value = efk_dm_ingresos_14562908.Value;
                }

                if (efk_dm_ingresos_29095000 != null)
                {
                    obj.efk_dm_ingresos_29095000 = new CrmFloat();
                    obj.efk_dm_ingresos_29095000.Value = efk_dm_ingresos_29095000.Value;
                }

                if (efk_dm_ingresos_3011455 != null)
                {
                    obj.efk_dm_ingresos_3011455 = new CrmFloat();
                    obj.efk_dm_ingresos_3011455.Value = efk_dm_ingresos_3011455.Value;
                }

                if (efk_dm_juridico != null)
                {
                    obj.efk_dm_juridico = new CrmFloat();
                    obj.efk_dm_juridico.Value = efk_dm_juridico.Value;
                }

                if (efk_dm_masculino != null)
                {
                    obj.efk_dm_masculino = new CrmFloat();
                    obj.efk_dm_masculino.Value = efk_dm_masculino.Value;
                }

                if (efk_dm_productivo != null)
                {
                    obj.efk_dm_productivo = new CrmFloat();
                    obj.efk_dm_productivo.Value = efk_dm_productivo.Value;
                }

                if (efk_dm_region_centro != null)
                {
                    obj.efk_dm_region_centro = new CrmFloat();
                    obj.efk_dm_region_centro.Value = efk_dm_region_centro.Value;
                }

                if (efk_dm_region_occidente != null)
                {
                    obj.efk_dm_region_occidente = new CrmFloat();
                    obj.efk_dm_region_occidente.Value = efk_dm_region_occidente.Value;
                }

                if (efk_dm_region_oriente != null)
                {
                    obj.efk_dm_region_oriente = new CrmFloat();
                    obj.efk_dm_region_oriente.Value = efk_dm_region_oriente.Value;
                }

                if (efk_dm_servicios != null)
                {
                    obj.efk_dm_servicios = new CrmFloat();
                    obj.efk_dm_servicios.Value = efk_dm_servicios.Value;
                }

                if (efk_dm_tarjeta_credito != null)
                {
                    obj.efk_dm_tarjeta_credito = new CrmFloat();
                    obj.efk_dm_tarjeta_credito.Value = efk_dm_tarjeta_credito.Value;
                }

                if (efk_dm_utilizacion_025 != null)
                {
                    obj.efk_dm_utilizacion_025 = new CrmFloat();
                    obj.efk_dm_utilizacion_025.Value = efk_dm_utilizacion_025.Value;
                }

                if (efk_dm_utilizacion_26335 != null)
                {
                    obj.efk_dm_utilizacion_26335 = new CrmFloat();
                    obj.efk_dm_utilizacion_26335.Value = efk_dm_utilizacion_26335.Value;
                }

                if (efk_dm_utilizacion_456755 != null)
                {
                    obj.efk_dm_utilizacion_456755 = new CrmFloat();
                    obj.efk_dm_utilizacion_456755.Value = efk_dm_utilizacion_456755.Value;
                }

                if (efk_dm_utilizacion_mas_755 != null)
                {
                    obj.efk_dm_utilizacion_mas_755 = new CrmFloat();
                    obj.efk_dm_utilizacion_mas_755.Value = efk_dm_utilizacion_456755.Value;
                }

                if (efk_TieneAlarmaProbable != null)
                {
                    obj.efk_tiene_alarma_probable = new CrmBoolean();
                    obj.efk_tiene_alarma_probable.Value = efk_TieneAlarmaProbable.Value;
                }

                if (efk_fechacumpleanos != null)
                {
                    obj.efk_fecha_cumpleanos = new CrmDateTime();
                    obj.efk_fecha_cumpleanos.Value = efk_fechacumpleanos.Value.ToString(formatoFecha);
                }

                if (efk_LNSaldoPromedioMensualCaptaciones != null)
                {
                    obj.efk_ln_saldo_promedio_mensual_captaciones = new CrmFloat();
                    obj.efk_ln_saldo_promedio_mensual_captaciones.Value = efk_LNSaldoPromedioMensualCaptaciones.Value;
                }

                if (efk_LNVariacionSaldoPromedioMensualCaptaciones != null)
                {
                    obj.efk_lnvariacionsaldopromediomensualcaptacione = new CrmFloat();
                    obj.efk_lnvariacionsaldopromediomensualcaptacione.Value = efk_LNVariacionSaldoPromedioMensualCaptaciones.Value;
                }

                if (efk_NroAdicionales != null)
                {
                    obj.efk_nro_adicionales = new CrmFloat();
                    obj.efk_nro_adicionales.Value = efk_NroAdicionales.Value;
                }

                if (efk_NroIniciales != null)
                {
                    obj.efk_nro_iniciales = new CrmFloat();
                    obj.efk_nro_iniciales.Value = efk_NroIniciales.Value;
                }

                if (efk_NroReposiciones != null)
                {
                    obj.efk_nro_reposiciones = new CrmFloat();
                    obj.efk_nro_reposiciones.Value = efk_NroReposiciones.Value;
                }

                if (efk_NroSinSolicitud != null)
                {
                    obj.efk_nro_sin_solicitud = new CrmFloat();
                    obj.efk_nro_sin_solicitud.Value = efk_NroSinSolicitud.Value;
                }

                if (efk_NroHijos != null)
                {
                    obj.efk_nro_hijos_ratificado = new CrmBoolean();
                    obj.efk_nro_hijos_ratificado.Value = efk_NroHijos.Value;
                }

                if (efk_VecesVigente2TC != null)
                {
                    obj.efk_veces_vigente_2tc = new CrmFloat();
                    obj.efk_veces_vigente_2tc.Value = efk_VecesVigente2TC.Value;
                }

                if (efk_alarma_vivienda != null)
                {
                    obj.efk_alarma_de_vivienda = new CrmFloat();
                    obj.efk_alarma_de_vivienda.Value = efk_alarma_vivienda.Value;
                }

                if (efk_anos_como_cliente != null)
                {
                    obj.efk_anos_como_cliente = new CrmFloat();
                    obj.efk_anos_como_cliente.Value = efk_anos_como_cliente.Value;
                }

                if (efk_cantidad_tx != null)
                {
                    obj.efk_cantidad_de_tx = new CrmFloat();
                    obj.efk_cantidad_de_tx.Value = efk_cantidad_tx.Value;
                }

                if (efk_cantidad_tx_td != null)
                {
                    obj.efk_cantidad_tx_td = new CrmFloat();
                    obj.efk_cantidad_tx_td.Value = efk_cantidad_tx_td.Value;
                }

                if (efk_cantidad_tx_tc != null)
                {
                    obj.efk_cantidad_tx_tc = new CrmFloat();
                    obj.efk_cantidad_tx_tc.Value = efk_cantidad_tx_tc.Value;
                }

                if (efk_plazo_transcurrido_credito != null)
                {
                    obj.efk_plazo_transcurrido_del_credito = new CrmFloat();
                    obj.efk_plazo_transcurrido_del_credito.Value = efk_plazo_transcurrido_credito.Value;
                }

                if (efk_total_productos_activos != null)
                {
                    obj.efk_total_de_productos_activos = new CrmFloat();
                    obj.efk_total_de_productos_activos.Value = efk_total_productos_activos.Value;
                }

                if (efk_TasaCredito != null)
                {
                    obj.efk_tasa_credito = new CrmFloat();
                    obj.efk_tasa_credito.Value = efk_TasaCredito.Value;
                }

                if (efk_LNIngresoMensual != null)
                {
                    obj.efk_ln_ingreso_mensual = new CrmFloat();
                    obj.efk_ln_ingreso_mensual.Value = efk_LNIngresoMensual.Value;
                }

                if (efk_LNSaldoCaptaciones != null)
                {
                    obj.efk_ln_saldo_captaciones = new CrmFloat();
                    obj.efk_ln_saldo_captaciones.Value = efk_LNSaldoCaptaciones.Value;
                }

                if (efk_LNSaldoCartera != null)
                {
                    obj.efk_ln_saldo_cartera = new CrmFloat();
                    obj.efk_ln_saldo_cartera.Value = efk_LNSaldoCartera.Value;
                }

                if (efk_LNSaldoCarteraTC != null)
                {
                    obj.efk_ln_saldo_cartera_tc = new CrmFloat();
                    obj.efk_ln_saldo_cartera_tc.Value = efk_LNSaldoCarteraTC.Value;
                }

                if (efk_LNSaldoCarteraVivienda != null)
                {
                    obj.efk_ln_saldo_cartera_vivienda = new CrmFloat();
                    obj.efk_ln_saldo_cartera_vivienda.Value = efk_LNSaldoCarteraVivienda.Value;
                }

                if (efk_LNSaldoCarteraModComercial != null)
                {
                    obj.efk_ln_saldo_cartera_mod_comercial = new CrmFloat();
                    obj.efk_ln_saldo_cartera_mod_comercial.Value = efk_LNSaldoCarteraModComercial.Value;
                }

                if (efk_LNSaldoCarteraModConsumo != null)
                {
                    obj.efk_ln_saldo_cartera_mod_consumo = new CrmFloat();
                    obj.efk_ln_saldo_cartera_mod_consumo.Value = efk_LNSaldoCarteraModConsumo.Value;
                }

                if (efk_LNSaldoCarteraModTC != null)
                {
                    obj.efk_ln_saldo_cartera_mod_tc = new CrmFloat();
                    obj.efk_ln_saldo_cartera_mod_tc.Value = efk_LNSaldoCarteraModTC.Value;
                }

                if (efk_LNSaldoCarteraModVehicular != null)
                {
                    obj.efk_ln_saldo_cartera_mod_vehicular = new CrmFloat();
                    obj.efk_ln_saldo_cartera_mod_vehicular.Value = efk_LNSaldoCarteraModVehicular.Value;
                }

                if (efk_LNSaldoCarteraModVivienda != null)
                {
                    obj.efk_ln_saldo_cartera_mod_vivienda = new CrmFloat();
                    obj.efk_ln_saldo_cartera_mod_vivienda.Value = efk_LNSaldoCarteraModVivienda.Value;
                }

                if (efk_FechaAlarmaProbable != null)
                {
                    obj.efk_fecha_alarma_probable = new CrmDateTime();
                    obj.efk_fecha_alarma_probable.Value = efk_FechaAlarmaProbable.Value.ToString(formatoFecha);
                }

                if (efk_RatioAlarma != null)
                {
                    obj.efk_ratio_alarma = new CrmFloat();
                    obj.efk_ratio_alarma.Value = efk_RatioAlarma.Value;
                }

                if (efk_RangoSaldoInicial != null)
                {
                    obj.efk_rango_saldo_inicial = new Picklist();
                    obj.efk_rango_saldo_inicial.Value = efk_RangoSaldoInicial.Value;
                }

                if (efk_SumatoriaCuotaVigente != null)
                {
                    obj.efk_sumatoria_cuota_vigente = new CrmMoney();
                    obj.efk_sumatoria_cuota_vigente.Value = efk_SumatoriaCuotaVigente.Value;
                }

                if (efk_SumatoriaCuotasMaximas != null)
                {
                    obj.efk_sumatoria_cuotas_maximas = new CrmMoney();
                    obj.efk_sumatoria_cuotas_maximas.Value = efk_SumatoriaCuotasMaximas.Value;
                }

                if (efk_EdadCliente != null)
                {
                    obj.efk_edad = new CrmNumber();
                    obj.efk_edad.Value = efk_EdadCliente.Value;
                    obj.efk_edad_cliente = new CrmFloat();
                    obj.efk_edad_cliente.Value = efk_EdadCliente.Value;
                }

                if (efk_educacion != null)
                {
                    obj.efk_educacion = new CrmFloat();
                    obj.efk_educacion.Value = efk_educacion.Value;
                }

                if (efk_cartera_en_sistema != null)
                {
                    obj.efk_cartera_en_sistema = new CrmFloat();
                    obj.efk_cartera_en_sistema.Value = efk_cartera_en_sistema.Value;
                }

                if (efk_capacidad_de_ahorro != null)
                {
                    obj.efk_capacidad_de_ahorro = new CrmFloat();
                    obj.efk_capacidad_de_ahorro.Value = efk_capacidad_de_ahorro.Value;
                }

                #endregion

                if (this.accountid != Guid.Empty)
                {
                    //Actualizamos
                    obj.accountid = new Key();
                    obj.accountid.Value = accountid;
                    servicio.Update(obj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ActualizarDatosAdicionales(CrmService servicio)
        {
            try
            {
                //Creamos el objeto
                account obj = new account();
                obj.efk_numeronit = (efk_numero_nit != null ? efk_numero_nit : string.Empty);

                if (efk_patrimonio_tecnico != null)
                {
                    obj.efk_patrimonio_tecnico = new CrmMoney();
                    obj.efk_patrimonio_tecnico.Value = efk_patrimonio_tecnico.Value;
                }

                if (efk_fecha_creacion_cliente_banco != null)
                {
                    obj.efk_fecha_creacion_cliente_banco = new CrmDateTime();
                    obj.efk_fecha_creacion_cliente_banco.Value = efk_fecha_creacion_cliente_banco.Value.ToString(formatoFecha);
                }
                if (efk_fecha_constitucion_empresa != null)
                {
                    obj.efk_fecha_constitucion_empresa = new CrmDateTime();
                    obj.efk_fecha_constitucion_empresa.Value = efk_fecha_constitucion_empresa.Value.ToString(formatoFecha);
                }

                obj.efk_calificacion_cliente_banco = (efk_calificacion_cliente_banco != null ? efk_calificacion_cliente_banco : string.Empty);

                if (efk_atencionpreferente != null)
                {
                    obj.efk_atencionpreferente = new CrmBoolean();
                    obj.efk_atencionpreferente.Value = efk_atencionpreferente.Value;
                }

                obj.efk_descripciondelaatencionpreferente = (efk_descripciondelaatencionpreferente != null ? efk_descripciondelaatencionpreferente : string.Empty);
                obj.efk_telefonomovil = (efk_telefonomovil != null ? efk_telefonomovil : string.Empty);
                obj.efk_ciudad = (efk_ciudad != null ? efk_ciudad : string.Empty);
                obj.efk_barrio = (efk_barrio != null ? efk_barrio : string.Empty);
                obj.efk_zona = (efk_zona != null ? efk_zona : string.Empty);

                if (efk_aniosdeexperienciaenelmedio != null)
                {
                    obj.efk_aniosdeexperienciaenelmedio = new CrmNumber();
                    obj.efk_aniosdeexperienciaenelmedio.Value = efk_aniosdeexperienciaenelmedio.Value;
                }

                obj.efk_pais_origen = (efk_pais_origen != null ? efk_pais_origen : string.Empty);
                obj.efk_profesion = (efk_profesion != null ? efk_profesion : string.Empty);
                obj.efk_cargo = (efk_cargo != null ? efk_cargo : string.Empty);
                obj.efk_nombre_empresa = (efk_empresa != null ? efk_empresa : string.Empty);

                if (efk_fecha_ingreso_empresa != null)
                {
                    obj.efk_fecha_ingreso_empresa = new CrmDateTime();
                    obj.efk_fecha_ingreso_empresa.Value = efk_fecha_ingreso_empresa.Value.ToString(formatoFecha);
                }
                                

                if (this.accountid != Guid.Empty)
                {
                    //Actualizamos
                    obj.accountid = new Key();
                    obj.accountid.Value = accountid;
                    servicio.Update(obj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ActualizarCodigoCliente(CrmService servicio)
        {
            try
            {
                //Creamos el objeto
                account obj = new account();

                if (efk_codigo_cliente != null)
                {
                    obj.efk_codigo_cliente = new CrmNumber();
                    obj.efk_codigo_cliente.Value = this.efk_codigo_cliente.Value;
                }

                if (this.accountid != Guid.Empty)
                {
                    //Actualizamos
                    obj.accountid = new Key();
                    obj.accountid.Value = accountid;
                    servicio.Update(obj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ActualizarDatosOfertaValor(CrmService servicio)
        {
            try
            {
                //Creamos el objeto
                account obj = new account();

                if (efk_fechadenacimiento != null)
                {
                    int edad = DateTime.Now.Year - efk_fechadenacimiento.Value.Year;
                    //Obtengo la fecha de cumpleaños de este año.
                    DateTime nacimientoAhora = efk_fechadenacimiento.Value.AddYears(edad);
                    //Le resto un año si la fecha actual es anterior al día de nacimiento.
                    if (DateTime.Now.CompareTo(nacimientoAhora) > 0)
                    {
                        edad--;
                    }
                    obj.efk_edad = new CrmNumber();
                    obj.efk_edad.Value = edad;
                }

                if (efk_nrodehijos != null)
                {
                    obj.efk_tiene_hijos = new CrmBoolean();
                    if (efk_nrodehijos > 0)
                        obj.efk_tiene_hijos.Value = true;
                    else
                        obj.efk_tiene_hijos.Value = false;
                }

                if (efk_estado_civil != null)
                {
                    obj.efk_estado_civil = new Picklist();
                    obj.efk_estado_civil.Value = efk_estado_civil.Value;
                }

                if (this.accountid != Guid.Empty)
                {
                    //Actualizamos
                    obj.accountid = new Key();
                    obj.accountid.Value = accountid;
                    servicio.Update(obj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
