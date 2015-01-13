/****** Object:  StoredProcedure [dbo].[spCalculaMargenCredito]    Script Date: 08/27/2013 11:35:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--***********************************************************************************
--******** Procedimiento para el cálculo de Margen de Crédito **********************
--Parámetros:
--Fecha: 

ALTER Procedure [dbo].[spCalculaMargenCredito]
As 
Begin
	Declare @efk_margen_creditoId as uniqueidentifier 
	Declare @efk_monto_aprobado as money
	Declare @efk_saldo_fecha as money
	Declare @efk_accountId as uniqueidentifier 
	Declare @efk_monto as money
	Declare @efk_monto_disponible as money
	Declare @efk_monto_utilizado as money
	Declare @efk_porcentaje_uso as decimal(10,2)	
	Declare @efk_fecha_vencimiento as DateTime	
	Declare @contingentelineascredito as money
    Declare @contingentemargencredito as money
    Declare @transactioncurrencyid as uniqueidentifier
    Declare @efk_moneda as decimal(10,2)
    declare  @efk_monto_utilizado_base as money  
	declare  @efk_monto_disponible_base as money
	declare  @efk_monto_base as money  
	
	Set @efk_monto_aprobado = 0
	Set @efk_saldo_fecha = 0
	Set @efk_monto = 0
	Set @efk_monto_disponible = 0
	Set @efk_monto_utilizado = 0
	Set @efk_porcentaje_uso = 0
	Set @contingentelineascredito = 0
	Set @contingentemargencredito = 0
  	Set @efk_moneda = 0
  	Set @transactioncurrencyid = null
    set @efk_monto_utilizado_base = 0;
    set @efk_monto_disponible_base = 0;
    set @efk_monto_base = 0; 

	
	--Cancelar los márgenes de crédito que hayan vencido
	Declare cur_margenes_credito Cursor
	For
	Select mg.efk_margen_creditoId,
		   mg.efk_fecha_vencimiento
	from BMSC_MSCRM..efk_margen_credito mg 
	where mg.efk_estado <> 221220001
	
	Open cur_margenes_credito
	fetch cur_margenes_credito into @efk_margen_creditoId,@efk_fecha_vencimiento	
		
	WHILE (@@FETCH_STATUS = 0)
	Begin
	
		If (@efk_fecha_vencimiento < dateadd(second, (-1 * datediff(second, getutcdate(), getdate())), getdate()))
		begin
		    -- *************ACTUALIZO ESTADO A CANCELADO************** ------------------
			Update BMSC_MSCRM..efk_margen_creditoExtensionBase set efk_estado = 221220001
			Where efk_margen_creditoId = @efk_margen_creditoId
			-- ******************************************************* ------------------		
		end		
		fetch cur_margenes_credito into @efk_margen_creditoId,@efk_fecha_vencimiento
	End

	Close cur_margenes_credito
	Deallocate cur_margenes_credito	

	--Actuaizar márgenes de crédito
	
	--Set @efk_margen_creditoId = null
	
	Declare cur_operaciones Cursor
	For
	Select mg.efk_margen_creditoId,
		   mg.efk_accountId,
		   mg.efk_monto,
		   mg.transactioncurrencyid
	from BMSC_MSCRM..efk_margen_credito mg 
	where mg.efk_estado <> 221220001
	Open cur_operaciones
	fetch cur_operaciones into  @efk_margen_creditoId,@efk_accountId,@efk_monto,@transactioncurrencyid
	WHILE (@@FETCH_STATUS = 0)
	Begin
	    
	    --Modificado cris
	    set  @efk_monto_utilizado_base = 0;
	    set  @efk_monto_disponible_base = 0;
	    
	    set @efk_moneda = (select exchangerate from BMSC_MSCRM..transactioncurrency nolock where transactioncurrencyid= @transactioncurrencyid)
	    
		Select @efk_monto_utilizado_base = sum(efk_saldo_fecha_base),
		@efk_monto_utilizado= (sum(efk_saldo_fecha_base) * @efk_moneda)
		from BMSC_MSCRM..efk_producto_activo nolock
		where efk_margen_creditoId = @efk_margen_creditoId
		--
	  
		
		if (Not @efk_monto is  Null and Not @efk_monto_utilizado is Null)
		begin
			Set @efk_monto_disponible = (@efk_monto - @efk_monto_utilizado)
			
			if (@efk_moneda <> 1)
			begin
						Set @efk_monto = @efk_monto / @efk_moneda
						Set @efk_monto_disponible_base = (@efk_monto  - @efk_monto_utilizado_base)		
			end
			if (@efk_moneda = 1)
			begin
						Set @efk_monto_disponible_base = (@efk_monto  - @efk_monto_utilizado_base)		
			end
			
		end	   
		
		if (Not @efk_monto is  Null)
		begin
			if (@efk_monto > 0)
			begin
				Set @efk_porcentaje_uso = (@efk_monto_utilizado / @efk_monto) * 100
			end
		end
		
		--Actulizamos los saldos
		Update BMSC_MSCRM..efk_margen_creditoExtensionBase Set
			  efk_monto_disponible = @efk_monto_disponible,
			  efk_monto_utilizado = @efk_monto_utilizado,
			  efk_porcentaje_uso = @efk_porcentaje_uso,
			  efk_monto_disponible_Base = @efk_monto_disponible_base,
			  efk_monto_utilizado_base = @efk_monto_utilizado_base		  
		Where efk_margen_creditoId = @efk_margen_creditoId  
		-- *********************
   
	fetch cur_operaciones into @efk_margen_creditoId,@efk_accountId,@efk_monto,@transactioncurrencyid
	End

	Close cur_operaciones
	Deallocate cur_operaciones
	
    -- Actualizamos en la entidad de cuentas (clientes) el resumen del margen de crédito

	Set @efk_monto = 0
	Set @efk_monto_utilizado = 0
	Set @efk_porcentaje_uso = 0
	set @efk_moneda =0
	Set @transactioncurrencyid= null
	--Set @efk_accountId = null
	    
    Declare cur_clientes_margen_cred Cursor
	For
	Select mg.efk_accountId,
	       SUM(mg.efk_monto) monto_disponible,
		   SUM(mg.efk_monto_utilizado) monto_utilizado,
		   SUM(mg.efk_monto_base) monto_disponible_base,
		   SUM(mg.efk_monto_utilizado_base) monto_utilizado_base,
		   Case When (SUM(mg.efk_monto) > 0) then
					((SUM(mg.efk_monto_utilizado) / SUM(mg.efk_monto)) * 100)
				Else 0
		   End  porcentaje_uso,
		   ac.transactioncurrencyid
	from BMSC_MSCRM..efk_margen_credito mg inner join BMSC_MSCRM..Account ac
	on mg.efk_accountid = ac.AccountId and mg.efk_estado <> 221220001
	Group By mg.efk_accountId, ac.transactioncurrencyid	
	Open cur_clientes_margen_cred
	fetch cur_clientes_margen_cred into @efk_accountId,@efk_monto,@efk_monto_utilizado,@efk_monto_disponible_base ,@efk_monto_utilizado_base,@efk_porcentaje_uso,@transactioncurrencyid
	WHILE (@@FETCH_STATUS = 0)
	Begin	
	 set @efk_moneda = (select exchangerate from BMSC_MSCRM..transactioncurrency nolock where transactioncurrencyid= @transactioncurrencyid)
		--Actulizar Cliente
		Update BMSC_MSCRM..AccountExtensionBase Set
			   efk_monto_total_margenes_credito_vigentes = @efk_monto, 
			   efk_monto_total_margenes_credito_vigentes_Base = @efk_monto_disponible_base,
			   efk_monto_total_util_margenes_credito_vigente = @efk_monto_utilizado,
			   efk_monto_total_util_margenes_credito_vigente_base = @efk_monto_utilizado_base,		   
			   efk_porc_util_margenes_credito_vigentes = @efk_porcentaje_uso   
	    Where AccountId = @efk_accountId		   
		fetch cur_clientes_margen_cred into @efk_accountId,@efk_monto,@efk_monto_utilizado,@efk_monto_disponible_base ,@efk_monto_utilizado_base,@efk_porcentaje_uso,@transactioncurrencyid
	End

	Close cur_clientes_margen_cred
	Deallocate cur_clientes_margen_cred
	
	
	-- Cálculo del Monto Total del Riesgo Potencial
	--
	--Set @efk_accountId = null
	Set @transactioncurrencyid = null
	set @efk_moneda = 0
	Declare cur_clientes_corp Cursor
	For
	Select AccountId
	,transactioncurrencyid
	from BMSC_MSCRM..Account where efk_segmentoid in 
                             (Select efk_segmentoId from BMSC_MSCRM..efk_segmento
                              where efk_codigo in ('0011','0018','0044','0045','0046','0054','0055','0056'))  
                              --Filtro sólos clientes cuyo segmento sea Corporativo Empresarial
	
	Open cur_clientes_corp

	fetch cur_clientes_corp into @efk_accountId , @transactioncurrencyid

	WHILE (@@FETCH_STATUS = 0)
	Begin		
	     set @efk_moneda = (select exchangerate from BMSC_MSCRM..transactioncurrency nolock where transactioncurrencyid= @transactioncurrencyid)
	
	     Set @contingentelineascredito = dbo.fn_CalculaContingenteLineasCredito(@efk_accountId)
	     Set @contingentemargencredito = dbo.fn_CalculaContingenteMargenesCredito(@efk_accountId)
	     
	     if (@contingentelineascredito > 0 or @contingentemargencredito > 0)
	     begin	        
	         -- Actualizo Monto total riesgo contingente
			 Update BMSC_MSCRM..AccountExtensionBase 
			 set efk_monto_total_contingente_corp_bmsc = (COALESCE(efk_total_contingentes,0) + (@contingentelineascredito * @efk_moneda) + (@contingentemargencredito * @efk_moneda)),
			 
			  efk_monto_total_contingente_corp_bmsc_base = (COALESCE(efk_total_contingentes_base,0) + @contingentelineascredito + @contingentemargencredito)
			 where AccountId = @efk_accountId
			 
			 
		
			 -- Cálcular el Monto total del riesgo potencial
			 Update BMSC_MSCRM..AccountExtensionBase 
			 set efk_riesgo_potencial = (COALESCE((efk_monto_total_riesgo_cliente_corp_bmsc_base * @efk_moneda),0) + COALESCE((efk_monto_total_contingente_corp_bmsc_base * @efk_moneda),0)),
			     efk_riesgo_potencial_base = (COALESCE((efk_monto_total_riesgo_cliente_corp_bmsc_base),0) + COALESCE((efk_monto_total_contingente_corp_bmsc_base),0))
			 
			 
			 where AccountId = @efk_accountId			 
			 -- Recálcular el % de cobertura de garantías sobre el riesgo			
			 Update BMSC_MSCRM..AccountExtensionBase 
			 set efk_porc_cobertura_garantias_riesgo = ((COALESCE((efk_monto_total_valor_liquidable_garantia_base * @efk_moneda ),0) / (efk_riesgo_potencial_Base * @efk_moneda)) * 100)
			 where AccountId = @efk_accountId and efk_riesgo_potencial > 0		 
		 end	 
	     
	 
	     
	     --efk_riesgo_potencial = 
	     
	     fetch cur_clientes_corp into @efk_accountId, @transactioncurrencyid
	     
	End	

	Close cur_clientes_corp
	Deallocate cur_clientes_corp	
		
	If (@@ERROR > 0)
		Insert Into BDI_CRM_LogError values (GETDATE(),'CRM_CalculoMargenCredito','CRM_CalculoMargenCredito',SYSTEM_USER,'1','Error en ' + ISNULL(OBJECT_NAME(@@PROCID), 'unknown') + ' - no se puede actualizar los montos y saldos')

End
go
CREATE FUNCTION [dbo].[fn_CalculaContingenteLineasCredito] (@accountid uniqueidentifier)
RETURNS MONEY
AS
BEGIN
    DECLARE @contingenteLC AS MONEY 
    Declare @efk_monto_aprobado as money
    Declare @efk_monto_utilizado as money
    
    Set @contingenteLC = 0

    Declare cur_lineas_credito Cursor
	For
	--Modificado cris
	Select ac.efk_monto_aprobado_base, ac.efk_monto_utilizado_base
	from BMSC_MSCRM..efk_producto_activo ac 
	where ac.efk_cliente_juridico_Id = @accountid
	and ac.efk_clase_producto_banco = 221220009  --Indica sólo líneas de crédito
	and ac.efk_fecha_vencimiento_operacion > dateadd(second, (-1 * datediff(second, getutcdate(), getdate())), getdate())
	-------
	Open cur_lineas_credito
	fetch cur_lineas_credito into @efk_monto_aprobado,@efk_monto_utilizado
	
	WHILE (@@FETCH_STATUS = 0)
	Begin	
	    Set @contingenteLC = @contingenteLC + (@efk_monto_aprobado - @efk_monto_utilizado)	
	    fetch cur_lineas_credito into @efk_monto_aprobado,@efk_monto_utilizado
	End	
	
	Close cur_lineas_credito
	Deallocate cur_lineas_credito	

    RETURN Coalesce(@contingenteLC ,0)
END	
go

CREATE FUNCTION [dbo].[fn_CalculaContingenteMargenesCredito] (@accountid uniqueidentifier)
RETURNS MONEY
AS
BEGIN
    DECLARE @contingenteMC AS MONEY 
    Declare @efk_monto_aprobado as money
    Declare @efk_monto_utilizado as money
    
    Set @contingenteMC = 0

    Declare cur_margenes_credito Cursor
	For
	--Modificado cris
	Select mc.efk_monto_base, mc.efk_monto_utilizado_base
	from BMSC_MSCRM..efk_margen_credito mc 
	where mc.efk_accountId = @accountid
	and mc.efk_estado <> 221220001  --Indica sólo márgenes de crédito vigentes
	--------
	Open cur_margenes_credito
	fetch cur_margenes_credito into @efk_monto_aprobado,@efk_monto_utilizado
	
	WHILE (@@FETCH_STATUS = 0)
	Begin	
	    Set @contingenteMC = @contingenteMC + (@efk_monto_aprobado - @efk_monto_utilizado)	
	    fetch cur_margenes_credito into @efk_monto_aprobado,@efk_monto_utilizado
	End	
	
	Close cur_margenes_credito
	Deallocate cur_margenes_credito	

    RETURN Coalesce(@contingenteMC , 0)
END
go