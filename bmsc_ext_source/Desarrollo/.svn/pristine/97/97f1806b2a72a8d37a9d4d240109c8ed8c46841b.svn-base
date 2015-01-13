/****** Object:  UserDefinedFunction [dbo].[fn_sp_ingviv]    Script Date: 08/27/2013 11:36:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/***************************************
FUNCION: fn_sp_ingviv
DATE:    ENERO 17/2013
PROCEDIMIENTO: se envia el codigo del cliente y retorna el valor percibido de alquileres , bonos e
ingreso del conyugue mensual de la tabla de account 
***************************************/
CREATE function [dbo].[fn_sp_ingviv](@cliente varchar(max))
returns decimal(18,4) 
begin
    declare @result decimal(18,4)
	select @result = (isnull(efk_ingresos_ov,0)+  ISNULL(efk_valor_percibido_alquileres,0)) +   
	(ISNULL(efk_valor_percibido_bonos,0)/12) + isnull(efk_ingreso_conyugue_mensual,0) 
	from BMSC_MSCRM..AccountExtensionBase 
	where cast(accountid as varchar(max)) = @cliente 
	return @result 
end
GO