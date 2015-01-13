/****** Object:  UserDefinedFunction [dbo].[fn_sp_capitaloperaciones]    Script Date: 02/10/2014 12:05:00 ******/
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
ALTER function [dbo].[fn_sp_capitaloperaciones](@clientId varchar, 
	@estOperacionVigente varchar = 'VIGENTE', @estOperacionPagada varchar = 'PAGADA')
returns int 
begin
    declare @respuesta int = 0;
	
select @respuesta = case 
		when COUNT(*) > 0 then 1
		else 0
		end 
	   --p.ProductId,p.Name as subproducto, pb.efk_nombre as productoBanco, 
	   --pa.efk_cliente_juridico_IdName as cliente, pa.efk_estado_operacion as estadoOperacion
		from BMSC_MSCRM..Product p 
			 join BMSC_MSCRM..efk_producto_core pb -- prodcuto banco
			 on p.ProductId = pb.efk_productid
			 join BMSC_MSCRM..efk_producto_activo pa -- producto del activo
			 on pb.efk_producto_coreId = pa.efk_producto_core_id
		where p.name = 'CAPITAL DE OPERACIONES'
			  and pa.efk_cliente_juridico_Id = @clientId
			  and (pa.efk_estado_operacion like '%' + @estOperacionVigente + '%' 
				or pa.efk_estado_operacion like '%'+ @estOperacionPagada + '%')
			  and p.StateCode = 0
			  and pb.statecode = 0
			  and pa.statecode = 0
		--select @respuesta
		
	return @respuesta 	
end