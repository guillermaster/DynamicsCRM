/****** Object:  StoredProcedure [dbo].[sp_calcula_cem_actual]    Script Date: 08/27/2013 11:35:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*****************************************************************************
FUNCION: [sp_consulta_formulario_propuesta]
DATE:    ENERO 17/2013
AUTOR:   IYCAZA
PROCEDIMIENTO: Consulta campos de la base de datos , recibe como
parámetro de entrada el numero de la solicitud, realiza interrelación con las tablas
Cuenta, Opportunity, Product y OpportunityProduct
la tabla Stringmap es propia del CRM, donde se consulta para retornar el nombre
del estado, o actividad economica relacionándolos por el codigo.  
******************************************************************************/


CREATE PROCEDURE [dbo].[sp_calcula_cem_actual] (
												@c_simulacionId varchar(max),
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

	
	DECLARE @sobranteAnt MONEY
	declare @sum_cuotas_anteriores_cons money
	declare @sum_cuotas_anteriores_viv  money
	declare @nvoCEMcons money
	declare @nvoCEMviv  money
	
	SELECT @sobranteAnt = isnull(efk_cuota_sobrante,0)
	FROM   BMSC_MSCRM..efk_productosimulado 
	WHERE  efk_producto_simuladoId = @c_simulacionId and efk_orden = (@c_orden-1) 
	
	
	--obtener sumatoria de las cuotas pactadas de productos anteriores (de consumo)
	SELECT @sum_cuotas_anteriores_cons = ISNULL(SUM(efk_cuota_maxima_solicitada),0)
				FROM   BMSC_MSCRM..efk_productosimuladoExtensionBase
				WHERE efk_producto_simuladoId = @c_simulacionId 				
				AND efk_orden <= (@c_orden-1) AND efk_producto<>1
				
	--obtener sumatoria de las cuotas pactadas de productos anteriores (todos)
	SELECT @sum_cuotas_anteriores_viv = ISNULL(SUM(efk_cuota_maxima_solicitada),0)
				FROM BMSC_MSCRM..efk_productosimuladoExtensionBase
				WHERE efk_producto_simuladoId = @c_simulacionId 
				AND efk_orden <= (@c_orden-1) 
	
	
	SELECT @NvoCemAjustadoCons = @CemInicAjustadoCons - @sum_cuotas_anteriores_cons
	SELECT @NvoCemAjustadoViv  = @CemInicAjustadoViv  - @sum_cuotas_anteriores_viv
	
END;
GO