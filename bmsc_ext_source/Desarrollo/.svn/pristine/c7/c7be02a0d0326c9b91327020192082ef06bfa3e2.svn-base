/****** Object:  UserDefinedFunction [dbo].[fn_sp_rangoing]    Script Date: 08/27/2013 11:36:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*****************************************************************************
FUNCION: [fn_sp_rangoing]
DATE:    ENERO 17/2013
AUTOR:   IYCAZA
PROCEDIMIENTO: se envía como parámetros el monto de ingreso y 
realiza una comparación de rangos retornando el valor de rango de ingreso al 
proceso pincipal  
******************************************************************************/

CREATE function [dbo].[fn_sp_rangoing](@ingresos decimal(18,4))
returns decimal(18,4) 
begin
    declare @result decimal(18,4) 
    if @ingresos < 1 
    begin
      set @result = 0 
    end 
    if @ingresos >= 1 and @ingresos<= 1000 
    begin
       set @result = 1
    end
    if @ingresos > 1000 and @ingresos<= 2000
    begin
       set @result = 1.5
    end
    if @ingresos > 2000 
    begin
       set @result = 2
    end
    
 
	return @result 
end
GO