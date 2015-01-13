/****** Object:  UserDefinedFunction [dbo].[fn_sp_caractesp]    Script Date: 08/27/2013 11:36:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/***************************************
FUNCION: fn_sp_caractesp
DATE:    ENERO 17/2013
AUTOR:   IYCAZA
PROCEDIMIENTO: SE ENVIA UNA CADENA DE CARACTERES Y LA DEPURA ELIMINANDO LOS CARACTERES ESPECIALES
SE LA UTILIZA PARA LOS PROCESOS QUE REALIZAN CONSULTA A LA BASE DE DATOS EN EL CAMPO NOMBRE
***************************************/
CREATE function [dbo].[fn_sp_caractesp] (@cadena varchar(max)) 
returns varchar(max)  
begin
   -- declare @result varchar(max)
    declare @Caracteres VARCHAR(255)
    SET @Caracteres = '-;,./´()&\ñÑ¡!?#:$%[_*@áéíóú{}' + ''''

WHILE @cadena LIKE '%[' + @Caracteres + ']%'
BEGIN
		SELECT @cadena  = REPLACE(@cadena, SUBSTRING(@cadena, PATINDEX('%[' + @Caracteres + ']%', @cadena), 1),' ')
	    
end
return replace(@cadena,'  ','')
end
GO