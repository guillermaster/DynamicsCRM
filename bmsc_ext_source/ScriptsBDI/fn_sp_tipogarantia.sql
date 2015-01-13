/****** Object:  UserDefinedFunction [dbo].[fn_sp_tipogarantia]    Script Date: 08/27/2013 11:36:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/***************************************
FUNCION: [fn_sp_totalgarantia]
DATE:    MAYO 24/2013
PROCEDIMIENTO: SE ENVIA ELnumero de solicitud Y RETORNA el tipo DE garantias
***************************************/
CREATE function [dbo].[fn_sp_tipogarantia](@nrosolicitud varchar(max))
returns varchar(max) 
begin
    declare @result varchar(max) 
	
	select top 1 @result = case gc2.efk_garantia_existente_nueva when 221220001 then isnull(gc2.efk_garantia_producto_activoName,0) else isnull(tg.value,0) end 	
		     			        from BMSC_MSCRM..efk_garantia_credito gc2 
		     			        inner join BMSC_MSCRM..StringMap TG
		                          on TG.AttributeName = 'efk_codigo_tipo_garantia'
		                          and TG.AttributeValue = gc2.efk_codigo_tipo_garantia	
			          where gc2.efk_nrosolicitud  = @nrosolicitud and
			          gc2.efk_codigo_tipo_garantia is not null
			          order by gc2.efk_principal desc
	
	return @result 	
end
GO