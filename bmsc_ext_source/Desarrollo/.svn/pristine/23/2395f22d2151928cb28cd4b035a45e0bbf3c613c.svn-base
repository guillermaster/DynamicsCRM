/****** Object:  UserDefinedFunction [dbo].[fn_sp_prodhom]    Script Date: 08/27/2013 11:36:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*****************************************************************************
FUNCION: fn_sp_prodhom
DATE:    ENERO 17/2013
AUTOR:   IYCAZA
PROCEDIMIENTO: Función donde se envia como parametros de ingreso el codigo
del producto, el codigo del tipo de producto y el codigo de la familia del producto
realiza un consulta a la tabla de productos y retorna el campo efk_producto_homologado

donde es: 1 - VIVIENDA
          2 - CONSUMO
          3 - TARJETA DE CREDITO 
******************************************************************************/
  
CREATE function [dbo].[fn_sp_prodhom](@ProductoSubTipo varchar(max), @ProductoTipo varchar(max), @ProductoFamilia varchar(max))  
returns int   
begin  
    declare @result int  
    select @result = efk_productohomologado from BMSC_MSCRM..Product  
    where efk_familia_productosid  = @ProductoFamilia and efk_tipo_productoid = @ProductoTipo   
    and productid = @ProductoSubTipo  
    
  return @result   
end
GO