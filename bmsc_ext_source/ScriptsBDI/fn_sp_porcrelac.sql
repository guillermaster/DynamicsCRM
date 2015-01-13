/****** Object:  UserDefinedFunction [dbo].[fn_sp_porcrelac]    Script Date: 08/27/2013 11:36:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[fn_sp_porcrelac](@producto int, @ingreso decimal(18,4)) 
returns decimal(18,4) 
begin
    declare @result decimal(18,4)
	select @result  = efk_RelacionDeudaIngreso  from BMSC_MSCRM..efk_porcentajerelacion
	where efk_producto = @producto
	and @ingreso  >= isnull(efk_IngresoDesde,0) 
	and case isnull(efk_IngresoHasta,0) 
	    when 0 then 0 else @ingreso end  <= isnull(efk_IngresoHasta,0)
	return @result 
end
GO