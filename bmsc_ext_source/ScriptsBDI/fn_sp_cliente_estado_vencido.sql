
GO
/****** Object:  UserDefinedFunction [dbo].[fn_sp_capitaloperaciones]    Script Date: 02/10/2014 15:29:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/***************************************
FUNCION: [fn_sp_capitaloperaciones]
DATE:    Feberero 10/2014
PROCEDIMIENTO: Funcion que valida si un cliente tiene o tuvo mas de 2 capitales de operaciones pagadas o vigentes
			   1 => Verdadero, 0 => Falso
***************************************/
CREATE function [dbo].[fn_sp_clienteEstadoVencido](@clientId nvarchar(72))
returns int 
begin
    declare @respuesta int = 0;
	
	select @respuesta = case 
			when c.efk_nroveces_vencido_ejecucion > 0 then 1
			else 0
			end
	from BMSC_MSCRM..Account c
	where c.AccountId = @clientId and
		  c.efk_nroveces_vencido_ejecucion > 0
	
		
	return @respuesta 	
end