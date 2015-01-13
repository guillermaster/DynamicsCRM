/****** Object:  StoredProcedure [dbo].[sp_consulta_formulario_solicitud]    Script Date: 08/27/2013 11:35:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*****************************************************************************
PROCEDIMIENTO: [sp_consulta_formulario_solicitud]
DATE:    ENERO 17/2013
AUTOR:   IYCAZA
PROCEDIMIENTO: El proceso recibe como parámetro de entrada el numero de solicitud
realiza una consulta a la tabla de Account para obtener el codigo del tipo del cliente
En base al tipo de identificacion, si es cliente natural o jurídico realiza
consultas a la base de datos. 

Si es representante legal retorna 2 querys, fue definido de esta manera para
que la cabecera no se repita n veces. 
******************************************************************************/
--- exec sp_consulta_formulario_solicitud 100065
--- exec sp_consulta_formulario_solicitud 100065

CREATE procedure [dbo].[sp_consulta_formulario_solicitud] ( @c_nrosolicitud as int ) as
declare @c_tipidentif int = -1
begin

   --declare @c_nrosolicitud int = 00000
   --declare @c_tipidentif int

     select @c_tipidentif = ISNULL(A.efk_tipo_cliente,-1) from BMSC_MSCRM..account a
     inner join BMSC_MSCRM..opportunity o
     on a.AccountId = o.AccountId 
     and o.efk_nrosolicitud = @c_nrosolicitud

     --select @c_tipidentif

if  (@c_tipidentif= '221220000' or @c_tipidentif = -1)  -- cliente natural 
/*if  (@c_tipidentif <> '221220001')  -- cliente natural o cualquier otro*/
begin  
        select TC.Value TipodeCliente, 
        --isnull(AC.efk_ingresos_ov,0) FuentedeIngresos,
        FI.Value FuentedeIngresos, 			--isnull(FI.Value,'NO DEFINIDO') FuentedeIngresos,
        op.efk_nrosolicitud NroSolicitud, 
		ISNULL(OP.EstimatedValue,0) MontoSolicitado, 		
		--OP.TransactionCurrencyIdName   MonedaOperacion,OP.efk_moneda_operacion MonedaOperacion,
		ISNULL(MO.Value,0)   			MonedaOperacion, 				--isnull(MO.Value,'NO DEFINIDO')   MonedaOperacion, 		
        op.efk_modalidad	Codigo_Modalidad,
		ISNULL(MD.Value,0)   			Modalidad, 						--isnull(MD.Value,'NO DEFINIDO')   Modalidad, 		
		op.efk_condiciones 	Codigo_Condicion,
		ISNULL(TPO.Value,0)           Seguro_Todo_Riesgo,	
		op.OwnerIdname      ejecutivo,	
		CD.Value   			Condicion, 						--isnull(CD.Value,'NO DEFINIDO')   Condicion, 		
		op.efk_cambios 		Codigo_Cambio,
		CA.Value   			Cambio, 				 		--isnull(CA.Value,'NO DEFINIDO')   Cambio, 				 
		po.ProductId 		SubtipoPro, 
		su.efk_sucursalidName Nombre_Sucursal,
		su.efk_oficina_idName Nombre_Agencia,
		po.Name 			Descripcion_del_producto, 		--isnull(po.Name,'NO DEFINIDO') Descripcion_del_producto,
		UPPER(ac.efk_PrimerApellido)	PrimerApellido,  	--isnull(UPPER(ac.efk_PrimerApellido),'NO DEFINIDO') PrimerApellido, 
		UPPER(ac.efk_SegundoApellido) 	SegundoApellido, 	--isnull(UPPER(ac.efk_SegundoApellido),'NO DEFINIDO') SegundoApellido,
		UPPER(dbo.fn_sp_caractesp(AC.efk_nombre_persona)) Nombre, -- pqm 2013-02-20 --isnull(UPPER(dbo.fn_sp_caractesp(AC.efk_nombre_persona)),'NO DEFINIDO') Nombre, -- pqm 2013-02-20
		--AC.efk_fecha_nacimiento_ratificado FechaNacimiento, 
		AC.efk_fechadenacimiento_ov	FechaNacimiento, 
	    TI.Value TipoIdentificacion, 		--isnull(TI.Value,'NO DEFINIDO')   TipoIdentificacion, 
	    AC.accountnumber NumeroIdentificacion,
	    ac.efk_pais_origen Nacionalidad ,	--isnull(ac.efk_pais_origen,'NO DEFINIDO')  Nacionalidad ,
	    SP.Value Sexo, 						--isnull(SP.Value,'NO DEFINIDO')  Sexo,
	    ac.EMailAddress1 Email, 			--isnull(ac.EMailAddress1,'NO DEFINIDO') Email, 
	    ISNULL(ac.efk_nrodehijos_ov,0) NumerodeDependientes,
	    ISNULL(ac.Revenue,0) Ingreso, 
	    ISNULL(d.TotEgreso,0) TotalEgresos,
	    ISNULL(ac.revenue,0) IngresosAnuales, 
    	isnull(ac.efk_patrimonio_tecnico,0)  Patrimonio, 
	    EC.Value EstadoCivil, 				--isnull(EC.Value,'NO DEFINIDO')  EstadoCivil, 
	    AC.efk_Profesion Profesion, 		--isnull(AC.efk_Profesion,'NO DEFINIDO') Profesion, 
	    NE.VALUE NivelEstudios,				--isnull(NE.VALUE,'NO DEFINIDO') NivelEstudios,
	    zp.Value Zona_POSTAL,				--isnull(zp.Value,'NO DEFINIDO') Zona_POSTAL,
	    BA.Value Barrio_POSTAL, 			--isnull(BA.Value,'NO DEFINIDO') Barrio_POSTAL, 
	    DIR.efk_direccion Calle_POSTAL ,	--DIRECCION TIPO POSTAL--isnull(DIR.efk_direccion,'NO DEFINIDO') Calle_POSTAL , --- DIRECCION TIPO POSTAL
	    AC.efk_NumeroNIT NumeroNIT, 		--isnull(AC.efk_NumeroNIT,'NO DEFINIDO') NumeroNIT, 
	    zp.Value Zona_DOMIC,				--isnull(zp.Value,'NO DEFINIDO') Zona_DOMIC,
	    BAD.Value Barrio_DOMIC , 			--isnull(BAD.Value,'NO DEFINIDO') Barrio_DOMIC , 
	    DIRD.efk_direccion Calle_DOMIC , 	--DIRECCION TIPO DOMICILIO--isnull(DIRD.efk_direccion,'NO DEFINIDO') Calle_DOMIC , --- DIRECCION TIPO DOMICILIO
	    AC.efk_Telefonomovil TelefonoMovil,	--isnull(AC.efk_Telefonomovil,'NO DEFINIDO') TelefonoMovil , 
	    '' Mensaje,
		AC.efk_codigo_cliente CodigoCliente
	    from BMSC_MSCRM..Account  AC
		inner join BMSC_MSCRM..Opportunity OP
		on op.AccountId = ac.AccountId 
		and OP.efk_nrosolicitud = @c_nrosolicitud
		inner join BMSC_MSCRM..StringMap TC
		on TC.AttributeName = 'efk_tipo_cliente'
		and TC.AttributeValue = AC.efk_tipo_cliente
        and TC.AttributeValue = '221220000'        
        inner join BMSC_MSCRM..SystemUser su
        on su.SystemUserId = op.OwnerId        
        left join BMSC_MSCRM..StringMap TI
    	on TI.AttributeName = 'efk_tipo_identificacion'
		and TI.AttributeValue = AC.efk_tipo_identificacion
		--and TI.ObjectTypeCode = 10018
        and TI.ObjectTypeCode = (SELECT top 1 b.ObjectTypeCode  
								FROM BMSC_MSCRM..AttributeLogicalView a 
								inner join   BMSC_MSCRM..EntityLogicalView b on 
								a.Name  = 'efk_tipo_identificacion' and 
								a.EntityId =b.EntityId  and 
								upper( b.Name) = 'ACCOUNT') -- pqm 2013-02-20
		
        left join BMSC_MSCRM..StringMap SP
    	on SP.AttributeName = 'efk_sexo'
		and SP.AttributeValue = ac.efk_sexo
		left join BMSC_MSCRM..StringMap MO
		on MO.AttributeName = 'efk_moneda_operacion' and MO.ObjectTypeCode = 3 and MO.AttributeValue = OP.efk_moneda_operacion 		
		left join BMSC_MSCRM..StringMap TPO        
        on TPO.AttributeName = 'efk_tipo_poliza' and TPO.ObjectTypeCode = 3 and TPO.AttributeValue = op.efk_tipo_poliza 				
		left join BMSC_MSCRM..StringMap MD
		on MD.AttributeName = 'efk_modalidad' 
		and MD.AttributeValue = OP.efk_modalidad
		left join BMSC_MSCRM..StringMap CD
		on CD.AttributeName = 'efk_condiciones' 
		and CD.AttributeValue = OP.efk_condiciones
	    left join BMSC_MSCRM..StringMap CA
		on CA.AttributeName = 'efk_cambios' 
		and CA.AttributeValue = OP.efk_cambios
		left join BMSC_MSCRM..StringMap FI
		on FI.AttributeName = 'efk_fuente_ingresos_ov' 
		and FI.AttributeValue = AC.efk_fuente_ingresos_ov
        left join BMSC_MSCRM..StringMap EC
		on EC.AttributeName = 'efk_estado_civil_ov'
		and EC.AttributeValue = ac.efk_estado_civil
        left join BMSC_MSCRM..efk_direccion DIR ---- tipo de direccion postal 
        on DIR.efk_codigo_cliente  = AC.efk_codigo_cliente 
        and DIR.efk_tipo in (select AttributeValue  from BMSC_MSCRM..StringMap where Attributename = 'efk_tipo' and Value = 'Envío de correspondencia')
        left join BMSC_MSCRM..StringMap BA
		on BA.AttributeName = 'efk_barrio'
		and BA.AttributeValue = DIR.efk_barrio
        left join BMSC_MSCRM..efk_direccion DIRD ---- tipo de direccion postal 
        on DIRD.efk_codigo_cliente  = AC.efk_codigo_cliente 
        and DIRD.efk_tipo in (select AttributeValue  from BMSC_MSCRM..StringMap where Attributename = 'efk_tipo' and Value LIKE '%DOMICILIO%')
        left join BMSC_MSCRM..StringMap BAD
		on BAD.AttributeName = 'efk_barrio'
		and BAD.AttributeValue = DIRD.efk_barrio
        left join BMSC_MSCRM..StringMap NE
		on NE.AttributeName = 'efk_nivel_estudios'
		and NE.AttributeValue = AC.efk_nivel_estudios
		left join BMSC_MSCRM..StringMap ZP
		on zp.AttributeName = 'efk_zona'
		and zp.AttributeValue = dir.efk_zona 
		left join BMSC_MSCRM..OpportunityProduct opprod on
		 op.OpportunityId = opprod.OpportunityId
		left join BMSC_MSCRM..Product po on
		 po.ProductId = opprod.ProductId --- asumo que es el id del producto simulado de oportunidad 
		inner join
		( select ac1.AccountId ,  SUM(
	      isnull(ac1.efk_hipotecario_cuotas_respaldadas,0) 
	    + isnull(ac1.efk_hipotecario_cuotas_DDJJ,0) 
	    + isnull(ac1.efk_hipotecario_cuotas_cartera,0) 
	    + isnull(ac1.efk_consumo_cuotas_DDJJ,0)
        + isnull(ac1.efk_consumo_cuotas_respaldadas,0)
        + isnull(ac1.efk_consumo_saldo_cartera,0)
        + isnull(ac1.efk_consumoTDC_cuotas_DDJJ,0)
        + isnull(ac1.efk_consumoTDC_cuotas_respaldadas ,0)
        + isnull(ac1.efk_consumoTDC_saldo_cartera ,0)
        + isnull(ac1.efk_comercialPyme_cuotas_DDJJ,0)
        + isnull(ac1.efk_comercialPyme_saldo_cartera,0)
        + isnull(ac1.efk_comercialPyme_cuotas_respaldadas,0)
        + isnull(ac1.efk_microcredito_saldo_cartera,0)
        + isnull(ac1.efk_microcredito_cuotas_DDJJ,0)
        + isnull(ac1.efk_microcredito_cuotas_respaldadas ,0)
        + isnull(ac1.efk_cuotas_BMSC,0)
        + isnull(ac1.efk_cuotas_BMSC_tramite,0)
        + ISNULL(ac1.efk_gastos_personales,0)
        + isnull(ac1.efk_deuda_empresa_empleadora,0) 
        ) TotEgreso 
          from BMSC_MSCRM..Account ac1 inner join BMSC_MSCRM..Opportunity op1
          on ac1.AccountId = op1.AccountId 
          and op1.efk_nrosolicitud =  @c_nrosolicitud
          group by ac1.AccountId 
          ) as d
          on d.AccountId = ac.AccountId 
         
    
end         

if  (@c_tipidentif= '221220001')  -- cliente juridico
begin  

    	select  
		TC.Value TipodeCliente, 
		dbo.fn_sp_caractesp(AC.Name) Nombre,
    	AC.accountnumber NumeroIdentificacion,
		op.efk_nrosolicitud NroSolicitud,   
    	ISNULL(AC.numberofemployees,0) NumeroEmpleados,  	
    	'' CLI_BOMCORRESP, 	--- tiene pendiente el banco
    	'' SUJETOIMPONIBLE, --- tiene pendiente el banco 
    	ISNULL(ac.revenue,0) IngresosAnuales, 
    	isnull(ac.efk_patrimonio_tecnico,0)  Patrimonio, 
    	ac.efk_fecha_constitucion_empresa  FechaConstitucion,
    	AE.Value  ActividadEconomica,		--isnull(AE.Value,'NO DEFINIDO')  ActividadEconomica,
	    zp.value Zona_POSTAL,				--isnull(zp.value,'NO DEFINIDO') Zona_POSTAL,
	    BA.Value Barrio_POSTAL , 			--isnull(BA.Value,'NO DEFINIDO') Barrio_POSTAL , 
	    DIR.efk_direccion Calle_POSTAL , 	--DIRECCION TIPO POSTAL--isnull(DIR.efk_direccion,'NO DEFINIDO') Calle_POSTAL , --- DIRECCION TIPO POSTAL
        zo.Value Zona_OFIC,					--isnull(zo.Value,'NO DEFINIDO') Zona_OFIC,
        BAO.Value Barrio_OFIC , 			--isnull(BAO.Value,'NO DEFINIDO') Barrio_OFIC , 
        DIRO.efk_direccion Calle_OFIC , 	--DIRECCION TIPO OFICINA--isnull(DIRO.efk_direccion,'NO DEFINIDO') Calle_OFIC , --- DIRECCION TIPO OFICINA
	    AC.Telephone1  Telefono, 			--isnull(AC.Telephone1,'NO DEFINIDO')  Telefono, 
	    AC.efk_Telefonomovil  TelefonoMovil,	--isnull(AC.efk_Telefonomovil,'NO DEFINIDO')  TelefonoMovil
		AC.efk_codigo_cliente CodigoCliente,
		FI.Value FuentedeIngresos,
		ISNULL(OP.EstimatedValue,0) MontoSolicitado,
		MO.Value   			MonedaOperacion,
		op.OwnerIdName		ejecutivo,
		su.efk_sucursalidName Nombre_Sucursal,
		su.efk_oficina_idName Nombre_Agencia,
		po.ProductId 		SubtipoPro, 
		po.Name				Descripcion_del_producto,
		op.efk_modalidad	Codigo_Modalidad,
		ISNULL(MD.Value,0)   			Modalidad,
		op.efk_condiciones	Codigo_Condicion,
		ISNULL(CD.Value,0)   			Condicion,
		op.efk_cambios		Codigo_Cambio,
		ISNULL(CA.Value,0)			Cambio
	   /* --- REPRESENTANTE LEGAL 
	    isnull(RL.efk_name, 'SIN REPRESENTANTE')  NombreRepresentante,
	    isnull(rl.efk_documento_identidad, 'SIN REPRESENTANTE')  IdentificacionRepresentante,
	    rl.efk_cargo CargoRepresentante  
	   */ 
		from BMSC_MSCRM..Account  AC
		inner join BMSC_MSCRM..Opportunity OP
		on op.AccountId = ac.AccountId 
		and OP.efk_nrosolicitud = @c_nrosolicitud
		inner join BMSC_MSCRM..StringMap TC
		on TC.AttributeName = 'efk_tipo_cliente'
		and TC.AttributeValue = AC.efk_tipo_cliente
        and TC.AttributeValue = '221220001'
        left join BMSC_MSCRM..StringMap TI
    	on TI.AttributeName = 'efk_tipo_identificacion'
		and TI.AttributeValue = AC.efk_tipo_identificacion
        --and TI.ObjectTypeCode = 10017
		and TI.ObjectTypeCode = (SELECT top 1 b.ObjectTypeCode  
								FROM BMSC_MSCRM..AttributeLogicalView a 
								inner join   BMSC_MSCRM..EntityLogicalView b on 
								a.Name  = 'efk_tipo_identificacion' and 
								a.EntityId =b.EntityId  and 
								upper( b.Name) = 'ACCOUNT') -- pqm 2013-02-20
		
        left join BMSC_MSCRM..StringMap EC
		on EC.AttributeName = 'efk_estado_civil'
		and EC.AttributeValue = ac.efk_estado_civil
        left join BMSC_MSCRM..efk_direccion DIR ---- tipo de direccion postal 
        on DIR.efk_codigo_cliente  = AC.efk_codigo_cliente 
        and DIR.efk_tipo in (select AttributeValue  from BMSC_MSCRM..StringMap where Attributename = 'efk_tipo' and Value = 'Envío de correspondencia')
        left join BMSC_MSCRM..StringMap BA
		on BA.AttributeName = 'efk_barrio'
		and BA.AttributeValue = DIR.efk_barrio
        left join BMSC_MSCRM..efk_direccion DIRO ---- tipo de direccion postal 
        on DIRO.efk_codigo_cliente  = convert(int, convert(varchar, AC.efk_codigo_cliente)+'1')--obtener primera direccion que es igual a codigo_cliente + 1 (concat)
        and DIRO.efk_tipo in (select AttributeValue  from BMSC_MSCRM..StringMap where Attributename = 'efk_tipo' and ( Value LIKE '%OFICINA%' OR Value like '%TRABAJO%' ))
        left join BMSC_MSCRM..StringMap BAO
		on BAO.AttributeName = 'efk_barrio'
		and BAO.AttributeValue = DIRO.efk_barrio
		/*left join efk_representante_legal RL 
		on rl.efk_representante_legal_clienteid  = ac.AccountId */
		left join BMSC_MSCRM..StringMap AE
		on AE.AttributeName = 'efk_actividad_economica'
		and AE.AttributeValue = ac.efk_actividad_economica
		left join BMSC_MSCRM..StringMap ZP
		on zp.AttributeName = 'efk_zona'
		and zp.AttributeValue = dir.efk_zona 
		left join BMSC_MSCRM..StringMap ZO
		on zo.AttributeName = 'efk_zona'
		and zo.AttributeValue = diro.efk_zona 
		left join BMSC_MSCRM..StringMap FI
		on FI.AttributeName = 'efk_fuente_ingresos_ov' 
		and fi.AttributeValue = ac.efk_fuente_ingresos_ov
		left join BMSC_MSCRM..StringMap MO
		on MO.AttributeName = 'efk_moneda_operacion' and MO.ObjectTypeCode = 3 and MO.AttributeValue = OP.efk_moneda_operacion 	
		inner join BMSC_MSCRM..SystemUser su
        on su.SystemUserId = op.OwnerId  
        left join BMSC_MSCRM..OpportunityProduct opprod on
		 op.OpportunityId = opprod.OpportunityId
		left join BMSC_MSCRM..Product po on
		 po.ProductId = opprod.ProductId
		left join BMSC_MSCRM..StringMap MD
		on MD.AttributeName = 'efk_modalidad'
		and md.AttributeValue = op.efk_modalidad
		left join BMSC_MSCRM..StringMap CD
		on CD.AttributeName = 'efk_condiciones' 
		and CD.AttributeValue = op.efk_condiciones
		left join BMSC_MSCRM..StringMap CA
		on CA.AttributeName = 'efk_cambios'
		and CA.AttributeValue = op.efk_cambios
		 
		----REPRESENTANTE LEGAL 
		select  isnull(RL.efk_name, 'SIN REPRESENTANTE')  NombreRepresentante,
	    isnull(rl.efk_documento_identidad, 'SIN REPRESENTANTE')  IdentificacionRepresentante,
	    rl.efk_cargo CargoRepresentante,  --isnull(rl.efk_cargo,'NO DEFINIDO')  CargoRepresentante  
		AC.efk_codigo_cliente CodigoCliente
	    from BMSC_MSCRM..Account  AC
		inner join BMSC_MSCRM..Opportunity OP
		on op.AccountId = ac.AccountId 
		and OP.efk_nrosolicitud = @c_nrosolicitud
		left join BMSC_MSCRM..efk_representante_legal RL 
		on rl.efk_representante_legal_clienteid  = ac.AccountId 
end         

end;
GO