USE
[BDI_CRM]
GO
/****** Object: StoredProcedure [dbo].[sp_consulta_formulario_propuesta] Script Date: 01/14/2014 10:51:07 ******/
SET
ANSI_NULLS ON
GO
SET
QUOTED_IDENTIFIER ON
GO
ALTER
procedure [dbo].[sp_consulta_formulario_propuesta] (@nro_solicitud as int) as
begin
select DBO.fn_sp_caractesp(ac.Name) FullName,
ac
.efk_codigo_cliente CodigoBanco,
ac
.AccountNumber NroIdentificacion,
ac
.efk_fechadenacimiento_ov FechaNacimiento,
isnull(ec.Value,'NO DEFINIDO') EstadoCivilFisa,
isnull(ac.efk_nrodehijos_ov,0) NroDependientes,
cast(datediff(dd,isnull(ac.efk_fechadenacimiento_ov,GETDATE()),GETDATE()) / 365.25 as Int) Edad,
camp
.Name Campania,
op
.efk_nrosolicitud NroSolicitud ,
op
.efk_numero_oferta NroOferta,
op
.efk_moneda_operacion MonedaOperac,
ac
.efk_segmento_ovidName Segmento ,
op
.efk_tipo_dependencianame TipoBanca,
op
.efk_bajo_comite_comercial BajoComiteComercial,
pro
.Name SubtipoPro,
isnull(ac.revenue,0) IngresosAnuales,
isnull(ac.efk_patrimonio_tecnico,0) Patrimonio,
isnull(ac.numberofemployees,0) NumeroEmpleados,
isnull(ac.efk_nroveces_calificacion_distintaa,0) NumeroVecesCalificacionDistintaA,
ac
.efk_peor_calificacion_12mesesname PeorCalificacion12mesesSF,
ae
.Value ActividadEconomica,
isnull(op.efk_monto_maximo,0) MontoMaximoSimulacion,
isnull(op.efk_monto_solicitado,0) MontoSolicitado,
isnull(op.efk_cuota_maxima_simulacion,0) CuotaMaximaSimulacion,
isnull(op.efk_cuota_maxima_solicitada,0) CuotaMaximaSolicitada,
isnull(op.efk_numero_cuotas,0) NroCuotas,
op
.efk_tipo_cuota TipoCuota,
op
.efk_unidad_tiempo_plazoname UnidadTiempoPlazo,
isnull(op.efk_tasa_fija,0) TasaFija,
isnull(op.efk_spread_fijo,0) SpreadFijo,
isnull(op.efk_tre_semana,0) TRE,
op
.TransactionCurrencyIdName Moneda,
op
.efk_tipo_poliza TipoPoliza,
op
.efk_amortizacion_cada AmortizacionCada,
CASE op.efk_con_seguro_desgravamen WHEN 0 THEN 'NO' WHEN 1 THEN 'SI' ELSE 'NO DEFINIDO' END ConSeguroDesgravamen,
CASE op.efk_con_seguro_cesantia WHEN 0 THEN 'NO' WHEN 1 THEN 'SI' ELSE 'NO DEFINIDO' END ConSeguroCesanta,
op
.efk_tasa_variable_apartirde TasaVarAPartir,
		op
.efk_tipo_aprobacionname TipoAprobacion,
		op
.efk_gracia_capital GraciaCapital,
--- puntaje del score
cast(cast(round(fs.efk_puntaje_score,2) as decimal(4,2)) as varchar(max)) + '%' efk_puntaje_score,
isnull( fs.efk_puntaje_score_recomendacion,'NO DEFINIDO') efk_puntaje_score_recomendacion
-----
from BMSC_MSCRM..FilteredAccount ac
left join BMSC_MSCRM..StringMap EC
on EC.AttributeName = 'efk_estado_civil'
and EC.AttributeValue = ac.efk_estado_civil
left join BMSC_MSCRM..FilteredOpportunity op
on op.AccountId = ac.AccountId
left join BMSC_MSCRM..StringMap AE
on AE.AttributeName = 'efk_actividad_economica'
and AE.AttributeValue = ac.efk_actividad_economica
		
left join BMSC_MSCRM..CampaignBase camp
					
on camp.CampaignId = op.campaignid
left join BMSC_MSCRM..FilteredOpportunityProduct PP
on pp.OpportunityId = op.OpportunityId
left join BMSC_MSCRM..FilteredProduct Pro
on pp.ProductId = pro.ProductId
left join BMSC_MSCRM..Filteredefk_foto_solicitud_credito fs on
fs
.efk_opportunityid = op.OpportunityId
where op.efk_nrosolicitud = @nro_solicitud
/*----Codeudores---*/
select isnull(CO.efk_codeudorname, 'SIN CODEUDOR') Codeudor,
ac
.AccountNumber DocIdentidad,
ac
.efk_codigo_cliente_texto CodigoBanco,
FI
.Value TipoIngreso,
ac
.efk_NumeroNIT NIT,
isnull(co.efk_tipo_relacion_titularname, 'SIN RELACION CODEUDOR') Relacion_Codeudor,
co
.efk_seguro_cesantianame Cesantia, --isnull(rl.efk_cargo,'NO DEFINIDO') CargoRepresentante
co
.efk_seguro_desgravamenname Desgravamen
from BMSC_MSCRM..Filteredefk_codeudor co
inner join BMSC_MSCRM..Account ac
			
on ac.AccountId = co.efk_codeudor
			
left join BMSC_MSCRM..StringMap FI
on FI.AttributeName = 'efk_fuente_ingresos_ov'
and FI.AttributeValue = ac.efk_fuente_ingresos_ov
inner join BMSC_MSCRM..FilteredOpportunity OP
on op.opportunityid = co.efk_opportunityid
and op.efk_nrosolicitud = @nro_solicitud
			
/*----------*/
/*---- Seguros y Codeudores -----*/
select * from (select count(*) SegDesgravamenIncluyeCodeudor from (select distinct
co
.efk_seguro_desgravamenname Desgravamen
from BMSC_MSCRM..Filteredefk_codeudor co
inner join BMSC_MSCRM..Account ac
			
on ac.AccountId = co.efk_codeudor
			
left join BMSC_MSCRM..StringMap FI
on FI.AttributeName = 'efk_fuente_ingresos_ov'
and FI.AttributeValue = ac.efk_fuente_ingresos_ov
inner join BMSC_MSCRM..FilteredOpportunity OP
on op.opportunityid = co.efk_opportunityid
and op.efk_nrosolicitud = 701316
where co.efk_seguro_desgravamen=1) SegDesgCod) SDIC, (select count(*) SegCesantiaIncluyeCodeudor from (select distinct
co
.efk_seguro_cesantianame Cesantia--, --isnull(rl.efk_cargo,'NO DEFINIDO') CargoRepresentante
--co.efk_seguro_desgravamenname Desgravamen
from BMSC_MSCRM..Filteredefk_codeudor co
inner join BMSC_MSCRM..Account ac
			
on ac.AccountId = co.efk_codeudor
			
left join BMSC_MSCRM..StringMap FI
on FI.AttributeName = 'efk_fuente_ingresos_ov'
and FI.AttributeValue = ac.efk_fuente_ingresos_ov
inner join BMSC_MSCRM..FilteredOpportunity OP
on op.opportunityid = co.efk_opportunityid
and op.efk_nrosolicitud = 701316
where co.efk_seguro_cesantia=1) SegCesCod) SCIC
/*----Lugares_Trabajo---*/
select isnull(tr.efk_empresaname, 'SIN Empresa') Empresa,
isnull(tr.efk_cargoname, 'SIN CARGO') Cargo,
tr
.efk_meses_experiencia Experiencia,
tr
.efk_lugar_trabajo_principal EmpresaPrincipal
from BMSC_MSCRM..FilteredAccount ac1
inner join BMSC_MSCRM..FilteredOpportunity OP1
on op1.accountid = ac1.accountid
and op1.efk_nrosolicitud = @nro_solicitud
inner join BMSC_MSCRM..Filteredefk_lugaresdetrabajo tr
on ac1.accountid = tr.efk_lugar_trabajoid
/*----------*/
end
;
/*--------------------------------------*/

