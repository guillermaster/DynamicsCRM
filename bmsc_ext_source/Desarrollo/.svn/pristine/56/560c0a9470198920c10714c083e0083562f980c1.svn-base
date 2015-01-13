/****** Object:  UserDefinedFunction [dbo].[CalculaContingenteLineasCredito]    Script Date: 08/27/2013 11:36:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[CalculaContingenteLineasCredito] (@accountid uniqueidentifier)
RETURNS MONEY
AS
BEGIN
    DECLARE @contingenteLC AS MONEY 
    Declare @efk_monto_aprobado as money
    Declare @efk_monto_utilizado as money
    
    Set @contingenteLC = 0

    Declare cur_lineas_credito Cursor
	For
	Select ac.efk_monto_aprobado, ac.efk_monto_utilizado
	from BMSC_MSCRM..efk_producto_activo ac 
	where ac.efk_cliente_juridico_Id = @accountid
	and ac.efk_clase_producto_banco = 221220009  --Indica sólo líneas de crédito
	and ac.efk_fecha_vencimiento_operacion > dateadd(second, (-1 * datediff(second, getutcdate(), getdate())), getdate())
	
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
END;
GO