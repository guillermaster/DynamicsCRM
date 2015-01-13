/****** Object:  UserDefinedFunction [dbo].[fn_sp_tasdesreal]    Script Date: 08/27/2013 11:36:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*****************************************************************************
FUNCION: [fn_sp_tasdesreal]
DATE:    ENERO 17/2013
AUTOR:   IYCAZA
PROCEDIMIENTO: Recibe como parámetro el codigo del producto homologado
envia como resultado la tasa de descuento del producto inicial de la tabla
efk_tasadecuentoreal consultando por el producto homologado   
******************************************************************************/

CREATE function [dbo].[fn_sp_tasdesreal] (@producto int)
returns decimal(18,4) 
begin
    declare @result decimal(18,4)
	select @result=efk_tasadescuentoproductoinicial from BMSC_MSCRM..efk_tasadescuentoreal
	where efk_productohomologado = @producto 
	return @result 
end
GO