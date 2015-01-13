/****** Object:  UserDefinedFunction [dbo].[fn_sp_variabilidadTRE]    Script Date: 08/27/2013 11:36:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*****************************************************************************
FUNCION: [fn_sp_variabilidadTRE]
DATE:    FEBRERO 20/2013
AUTOR:   GPINCAY
PROCEDIMIENTO: Retorna el valor de variabilidad de la tasa TRE (que se encuentra
               en la entidad de parámetros de simulación crediticia)
******************************************************************************/

CREATE function [dbo].[fn_sp_variabilidadTRE] ()
returns decimal(18,4) 
begin
    declare @result decimal(18,4)
	SELECT @result = efk_valor_decimal FROM BMSC_MSCRM..efk_paramtero_simulacion_crediticia WHERE efk_name='tasa_tre_variabilidad'
	return @result 
end
GO