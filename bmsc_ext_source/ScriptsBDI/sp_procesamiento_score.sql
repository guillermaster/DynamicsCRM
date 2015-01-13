/****** Object:  StoredProcedure [dbo].[sp_procesamiento_score]    Script Date: 08/27/2013 11:35:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Text
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

/*****************************************************************************
PROCEDIMIENTO: [sp_procesamiento_score]
DATE:    ENERO 17/2013
AUTOR:   IYCAZA
PROCEDIMIENTO: Recibe como parametro de entrada el numero de solicitud
realiza consulta a la base de datos de la tabla de OpportunitY, Account y
Garantías 
******************************************************************************/

--- exec sp_procesamiento_score 100043

CREATE procedure [dbo].[sp_procesamiento_score] 
(
@c_nrosolicitud int  /* codigo de la solicitud */
)
as
   
begin
/* se obtienen los campos de cliente 360 */ 
	select op.efk_nrosolicitud NroSolicitud,  
	     op.opportunityid OportunidadId,
	     po.ProductId ProductoId,
	     ac.efk_fechadenacimiento_ov FechaNacimiento,
	     ISNULL(ac.efk_nrodehijos_ov,0) NroDependientes,
	     --gc.efk_garantia TipoGarantia,
         isnull(dbo.fn_sp_tipogarantia(op.efk_nrosolicitud),0) TipoGarantia,
	     isnull(gc11.GarantiasAnteriores,0) GarantiasAnteriores, 
		 Cast(isnull(map.sNombreValorCrm,'0') as varchar(50))   PeorCalificacionSistema,
		 isnull(ac.efk_nroveces_calificacion_distintaA,0)  NroVEcesCalificacionDistASistema,
		 isnull(ac.efk_nroveces_vencido_ejecucion,0)  NroVecesPorEstado,
		 isnull(ac.efk_nroveces_vigente2_14dias,0)  NroVecesVigenteDos ,
		 isnull(ac.efk_valor_liquidable_garantias_constituidas,0)  GarantiasConstituidas,
		 isnull(ac.efk_nroveces_vencido_ejecucion_titular,0)  RiesgoIndTitular,
		 isnull(ac.efk_nroveces_vencido_ejecucion_garante,0)  RiesgoIndGarante, 
		 isnull(ac.efk_total_cartera_garantia_hipotecaria,0)   TotalCarteraBajoGarantiaHp ,
		 isnull(ac.efk_total_cartera_garantia_personal,0)  TotalCarteraBajoGarantiaPersonal , 
		 isnull(ac.efk_total_cartera_asolaFirma,0)   TotalCarteraASolaFirma ,
		 isnull(op.efk_monto_maximo,0)  MontoMaximoOfrecido,
		 isnull(op.efk_monto_solicitado,0)  MontoSolicitado,
		 isnull(op.efk_cuota_maxima_simulacion,0)   CuotaMaxima , ---Simulacion Evaluacion Crediticia nuevo 
		 isnull(op.efk_numero_cuotas,0) NumeroCuotas,
		 ISNULL(op.efk_tasa_fija,0) TasaFija,
		 ISNULL(op.efk_spread_fijo, 0) SpreadFijo,
		 ISNULL(op.efk_amortizacion_cada, 30) AmortizacionCada,
		 ISNULL(op.efk_tasa_variable_apartirde, 0) TasaVariableDesde,
		 ISNULL(op.efk_con_seguro_cesantia, 0) ConSeguroCesantia,
		 ISNULL(op.efk_con_seguro_desgravamen, 0) ConSeguroDesgravamen,
		 ISNULL(dbo.fn_sp_parametro_mony('tasa_cesantia'),0.68) TasaCesantia,
		 ISNULL(dbo.fn_sp_parametro_mony('tasa_desgravamen'),0.78) TasaDesgravamen,
		 --- producto homologado si es 1 es vivienda si es (2,3) en otros productos o consumo
		 case po.efk_ProductoHomologado when 1 then isnull(op.efk_ingreso_vivienda,0) else isnull(op.efk_ingreso_otros_productos,0) end IngresoLiquido,
		 isnull(op.efk_serviciodeuda,0) ServicioDeuda, --- faltaria de crear este campo en Opportunity
		 isnull(ac.efk_gastos_personales,0) GastosPersonales 
		from BMSC_MSCRM..Account ac 
		     inner join BMSC_MSCRM..Opportunity op on 
			 ac.AccountId = op.AccountId
			 left join BMSC_MSCRM..OpportunityProduct opprod on
			 op.OpportunityId = opprod.OpportunityId
			 left join BDI_CRM_MapeoPicklist map on
			 ac.efk_peor_calificacion_12meses = map.iCodigoValorCrm and map.iCodigoMapeo = 19
			 left join BMSC_MSCRM..Product po on
			 po.ProductId = opprod.ProductId --- asumo que es el id del producto simulado de oportunidad 
		  --   left join (select top 1 gc2.efk_principal,gc2.efk_tipo_garantia,gc2.efk_garantia_existente_nueva,
		  --                     gc2.efk_codigo_tipo_garantia,gc2.efk_garantia_producto_activoName,
		  --                     gc2.efk_garantia_oportunidadId 
		  --   			        from BMSC_MSCRM..efk_garantia_credito gc2 
			 --         where gc2.efk_nrosolicitud  = @c_nrosolicitud  
			 --           /*order by gc2.efk_principal*/) gc
			 --on gc.efk_garantia_oportunidadId =  op.OpportunityId 		
			 left join 
			  (select gc1.efk_garantia_oportunidadId,SUM(efk_valor_liquidable_avaluo) GarantiasAnteriores 
			        from BMSC_MSCRM..efk_garantia_credito gc1 
			          where gc1.efk_nrosolicitud  = @c_nrosolicitud  
			            group by gc1.efk_garantia_oportunidadId) gc11
			 on gc11.efk_garantia_oportunidadId =  op.OpportunityId
		where op.efk_nrosolicitud  =  @c_nrosolicitud 

end;
GO