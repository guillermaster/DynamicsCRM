/****** Object:  UserDefinedFunction [dbo].[fn_sp_totalgarantia]    Script Date: 08/27/2013 11:36:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/***************************************
FUNCION: [fn_sp_totalgarantia]
DATE:    MAYO 24/2013
PROCEDIMIENTO: SE ENVIA ELnumero de solicitud Y RETORNA LA SUMATORIA DE Las garantias
***************************************/
CREATE function [dbo].[fn_sp_totalgarantia](@nrosolicitud varchar(max))
returns decimal(18,4) 
begin
    declare @result decimal(18,4)
	select @result=sum(gc.efk_valor_liquidable_avaluo) from BMSC_MSCRM..efk_garantia_credito gc
	INNER JOIN 	BMSC_MSCRM..Opportunity op on  op.OpportunityId = gc.efk_garantia_oportunidadId
   where op.efk_nrosolicitud = @nrosolicitud 
   group by op.OpportunityId
	return @result 
end
GO