/****** Object:  UserDefinedFunction [dbo].[fn_sp_parametro_mony]    Script Date: 08/27/2013 11:36:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[fn_sp_parametro_mony](@clave varchar(max))
returns money 
begin
    declare @result varchar (max)
	select @result = efk_valor_decimal
	from BMSC_MSCRM..efk_paramtero_simulacion_crediticiaExtensionBase
	where efk_name = @clave 
	return  convert(money,@result,0) 
end
GO