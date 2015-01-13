/****** Object:  UserDefinedFunction [dbo].[fn_sp_menorvalor]    Script Date: 08/27/2013 11:36:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*****************************************************************************
FUNCION: fn_sp_menorvalor 
DATE:    ENERO 17/2013
PROCEDIMIENTO: se envian dos valores a la funcion y retorna el menor valor
******************************************************************************/
CREATE function [dbo].[fn_sp_menorvalor](@valor1 MONEY, @valor2 MONEY)
returns MONEY  
begin
    declare @result money
    /* validacion de nulos */
    set @valor1 = ISNULL(@valor1,0)
    set @valor2 = ISNULL(@valor2,0)
    if @valor1 < @valor2
    begin
       set @result = @valor1 
    end
    if @valor2 < @valor1 
    begin
       set @result = @valor2 
    end 
    
    if @valor1 = @valor2 
    begin
      set @result = @valor1 
    end
	return @result 
end
GO