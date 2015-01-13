/****** Object:  StoredProcedure [dbo].[sp_porc_cuotas_ingreso_actual]    Script Date: 08/27/2013 11:35:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*****************************************************************************
FUNCION: [sp_porc_cuotas_ingreso_actual] 
DATE:    ENERO 19/2013
AUTOR:   IYCAZA
PROCEDIMIENTO: Función de porcentaje de cuotas ingreso actual
se la invoca desde el reporte de canasta
******************************************************************************/
/*
declare @resultado money
 exec sp_porc_cuotas_ingreso_actual 100052,@resultado output
 select @resultado
*/
CREATE  procedure  [dbo].[sp_porc_cuotas_ingreso_actual](@c_nrosolicitud int , @resultado money output, @resultado2 money output) as
begin


	declare @c_numeroferta int
	declare @aux_prodhom int
	declare @cursor1 cursor
	declare @tipo char(1) 
	declare @c_cliente varchar(max)
	declare @servdeudas money 
	declare @cuomaxsol money 
	
	

	select @c_numeroferta = efk_numero_oferta,
	@c_cliente = AccountId ,
	@cuomaxsol =  SUM(isnull(efk_cuota_maxima_solicitada,0))
	from BMSC_MSCRM..Opportunity 
	where StateCode = 0 and efk_numero_oferta in
	(select efk_numero_oferta from BMSC_MSCRM..Opportunity 
	 where efk_nrosolicitud = @c_nrosolicitud )
    group by  efk_numero_oferta, AccountId
    
    
    --select @c_numeroferta,@c_cliente,@cuomaxsol
    
    exec sp_servicio_deudas @c_cliente, @servdeudas output 
    
    --select @servdeudas
    --- aqui valido para que lovoy a dividir  
    --- si hay 1 vivienda se lo divide para ingreso de vivienda
    --- si no se lo divide para el ingreso de otros productos del cliente 
 set @tipo = 'P'
 SET @cursor1 = CURSOR FOR 
 select pr.efk_productohomologado 
	from BMSC_MSCRM..Opportunity op 
	--inner join  BMSC_MSCRM..efk_productosimulado ps
	--on op.efk_producto_simuladoid  = ps.efk_productosimuladoid
    left join  BMSC_MSCRM..OpportunityProduct ps
	on op.opportunityid  = ps.opportunityid
	left join BMSC_MSCRM..Product pr
	on pr.ProductId = ps.productid
	where op.efk_numero_oferta = @c_numeroferta
  OPEN @cursor1
  FETCH NEXT 
  FROM @cursor1 INTO @aux_prodhom
  WHILE @@FETCH_STATUS = 0
  BEGIN
     if  @aux_prodhom = '1' --- si es vivienda ?
      begin
        set @tipo = 'V'
        goto endcursor
      end  
  FETCH NEXT 
  FROM @cursor1 INTO @aux_prodhom
  END
  endcursor: 
  CLOSE @cursor1
  DEALLOCATE @cursor1 
  
  IF @tipo = 'V' 
  BEGIN 
    SELECT @resultado = ((@servdeudas + @cuomaxsol )/dbo.fn_sp_ingviv(@c_cliente) )*100 
    select  @resultado2 =  dbo.fn_sp_porcrelac(@aux_prodhom,dbo.fn_sp_ingviv(@c_cliente))
  END
  IF @tipo = 'C' 
  BEGIN 
    SELECT @resultado = ((@servdeudas + @cuomaxsol )/dbo.fn_sp_ingprod(@c_cliente))*100
    select  @resultado2 =  dbo.fn_sp_porcrelac(@aux_prodhom,bo.fn_sp_ingprod(@c_cliente))
  END
  
  
end;
GO
