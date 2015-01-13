/****** Object:  StoredProcedure [dbo].[spCalculaOfertaValorBMSCLotes]    Script Date: 08/27/2013 11:35:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--***********************************************************************************
--******** Procedimiento para el cálculo de la Oferta de Valor en lotes **********************
--Parámetros:
--Fecha: 
CREATE Procedure [dbo].[spCalculaOfertaValorBMSCLotes] (@valorMinimoLote as int,
														@valorMaximoLote as int,
														@IdUsrDefault as uniqueidentifier)
As 
begin
		create table #TMP_OFERTA_VALOR_LOTE(
			efk_oferta_valorId uniqueidentifier not null default NEWSEQUENTIALID(),
			accountid uniqueidentifier,
			subtipoProducto uniqueidentifier
		)
		
		insert into #TMP_OFERTA_VALOR_LOTE (accountid,subtipoProducto)
		select
		accountid,
		subtipoProducto
		from  #TMP_CLIENTE_SUBTIPO_FINAL
		where codigoCliente>=@valorMinimoLote and codigoCliente<=@valorMaximoLote		
		
		insert into 
			BMSC_MSCRM..efk_oferta_valorBase
			(efk_oferta_valorId,
			CreatedOn,CreatedBy,ModifiedOn,ModifiedBy,OwnerId,OwnerIdType,OwningBusinessUnit,statecode,statuscode,
			ImportSequenceNumber,TimeZoneRuleVersionNumber,UTCConversionTimeZoneCode
		)
		SELECT 
			ov.efk_oferta_valorId AS efk_oferta_valorId,
			GETDATE() AS CreatedOn,
			@IdUsrDefault AS CreatedBy,
			getdate() AS ModifiedOn,
			@IdUsrDefault AS ModifiedBy,
			c.OwnerId AS OwnerId,
			8 AS OwnerIdType,
			c.OwningBusinessUnit AS OwningBusinessUnit,
			0 AS statecode,
			1 AS statuscode,
			null AS ImportSequenceNumber,
			null AS TimeZoneRuleVersionNumber,
			null AS UTCConversionTimeZoneCode
		from #TMP_OFERTA_VALOR_LOTE ov inner join 
		BMSC_MSCRM..Account c on ov.AccountId=c.accountid 
					
		insert into BMSC_MSCRM..efk_oferta_valorExtensionBase
			(efk_oferta_valorId,efk_name,efk_new_name,efk_prioridad,efk_prioridad_tipo,efk_cliente_juridico_Id,efk_familia_productos_id,
			efk_product_id,efk_tipo_productos_id,efk_Portafolio, efk_prioridad_portafolio
		)
		SELECT 
			ov.efk_oferta_valorId AS efk_oferta_valorId,
			subtipo.efk_name AS efk_name,
			subtipo.efk_name AS efk_new_name,
			subtipo.efk_prioridad AS efk_prioridad,
			(tipo.efk_prioridad*10)+(subtipo.efk_prioridad) AS efk_prioridad_tipo,
			ov.AccountId AS efk_cliente_juridico_Id,
			null AS efk_familia_productos_id,
			ov.subtipoProducto AS efk_product_id,
			tipo.efk_Tipodeproducto AS efk_tipo_productos_id,
			portafolio.efk_name AS efk_Portafolio,
			portafolio.efk_prioridad
		from #TMP_OFERTA_VALOR_LOTE ov inner join BMSC_MSCRM..Account ac on ov.accountid = ac.AccountId
		inner join BMSC_MSCRM..efk_oferta_valor_banco oferta on oferta.efk_Segmento = ac.efk_segmento_ovid
		inner join BMSC_MSCRM..efk_subtipo_producto_oferta_valor subtipo on subtipo.efk_subtipo_producto_crm = ov.subtipoProducto and oferta.efk_oferta_valor_bancoId = subtipo.efk_oferta_valorid
		inner join BMSC_MSCRM..efk_portafolio_segmento portafolio on portafolio.efk_portafolio_segmentoId=subtipo.efk_portafolio_segmentoid
		left join BMSC_MSCRM..efk_tipo_producto_portafolio tipo on subtipo.efk_tipo_producto_portafolio=tipo.efk_tipo_producto_portafolioId
		
		drop table #TMP_OFERTA_VALOR_LOTE
end
GO