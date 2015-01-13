/****** Object:  StoredProcedure [dbo].[sp_calcula_cem_actual_oportunidad]    Script Date: 08/27/2013 11:35:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_calcula_cem_actual_oportunidad] (
												@c_numeroferta int,
												@c_orden int,
												@CemInicAjustadoCons money,
												@CemInicAjustadoViv  money,
												@NvoCemAjustadoCons  money OUTPUT,
												@NvoCemAjustadoViv   money OUTPUT
												)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @sum_cuotas_anteriores_cons money
	declare @sum_cuotas_anteriores_viv  money
	declare @nvoCEMcons money
	declare @nvoCEMviv  money
		
	--obtener sumatoria de las cuotas pactadas de productos anteriores (de consumo)
	select @sum_cuotas_anteriores_cons= isnull(SUM(op.efk_cuota_maxima_solicitada),0)
				from BMSC_MSCRM..Opportunity op 
				inner join BMSC_MSCRM..efk_productosimulado ps
				on op.efk_producto_simuladoid  = ps.efk_productosimuladoid
				where op.efk_numero_oferta = @c_numeroferta and op.efk_orden <= (@c_orden-1)  AND ps.efk_producto<>1
					
				
	--obtener sumatoria de las cuotas pactadas de productos anteriores (todos)
	select @sum_cuotas_anteriores_viv= isnull(SUM(op.efk_cuota_maxima_solicitada),0)
				from BMSC_MSCRM..Opportunity op 
				inner join BMSC_MSCRM..efk_productosimulado ps
				on op.efk_producto_simuladoid  = ps.efk_productosimuladoid
				where op.efk_numero_oferta = @c_numeroferta and op.efk_orden <= (@c_orden-1)
	
	
	SELECT @NvoCemAjustadoCons = @CemInicAjustadoCons - @sum_cuotas_anteriores_cons
	SELECT @NvoCemAjustadoViv  = @CemInicAjustadoViv  - @sum_cuotas_anteriores_viv
	
END;
GO