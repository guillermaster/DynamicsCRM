/****** Object:  StoredProcedure [dbo].[sp_flujocaja]    Script Date: 08/27/2013 11:35:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*****************************************************************************
PROCEDIMIENTO: [sp_flujocaja]
DATE:    ENERO 17/2013
AUTOR:   IYCAZA
PROCEDIMIENTO: Recibe como parametro de entrada el numero de solicitud de la 
oportunidad, y retorna el ingreso de vivienda y el ingreso de otros productos
realiza una consulta a la tabla de Account.

******************************************************************************/



CREATE procedure [dbo].[sp_flujocaja] (  @nrosolicitud int /* nro de solicitud */,
                                        @ingviv money output /* ingresos de vivienda */,
										@ingoprod money output /* ingresos de otros productos */
										) as
begin 

/* titular */
declare @ingdectit money
declare @monalqtit money
declare @monanubontit money

/* conyugue */
declare @ingdeccony money
declare @monalqcony money
declare @monanuboncony money


declare @ingtitviv money  -- ingreso tittular vivienda
declare @ingtitcon  money -- ingreso titular conyugue
declare @ingconyviv money --- ingreso conyugue vivienda
declare @ingconycon money --- ingreso conyugue consumo 

   select               @ingdectit    =  ((isnull(efk_salario_liquido_titularMes1,0) + isnull(efk_salario_liquido_titularMes2,0) + isnull(efk_salario_liquido_titularMes3,0))/ 3) ,
                        @monalqtit    =   isnull(efk_ingresos_mensuales_alquileres_titular,0) , 
                        @monanubontit =   isnull(efk_ingresos_anuales_abonos_titular,0)/12  ,
                        @ingdeccony    =  ((isnull(efk_salario_liquido_conyugueMes1,0) + isnull(efk_salario_liquido_conyugueMes2,0) + isnull(efk_salario_liquido_conyugueMes3,0))/ 3) ,
                        @monalqcony    =   isnull(efk_ingresos_mensuales_alquileres_conyugue,0) , 
                        @monanuboncony =   isnull(efk_ingresos_anuales_abonos_conyugue,0)/12                          
      from BMSC_MSCRM..Account 
   where AccountId in
   ( select AccountId CodigoCliente from BMSC_MSCRM..Opportunity where efk_nrosolicitud = @nrosolicitud  )

--- ingreso de titular vivienda	
    set @ingtitviv =  @ingdectit +  @monalqtit + @monanubontit
--- ingreso de titular consumo
    set @ingtitcon =   @ingdectit +  @monalqtit 
--- ingreso de conyugue vivienda
    set @ingconyviv  =  @ingdeccony +  @monalqcony + @monanuboncony
--- ingreso de conyugue consumo 
    set @ingconycon =   @ingdeccony +  @monalqcony

--- ingreso de vivienda
    set @ingviv = @ingtitviv + @ingconyviv
--- ingreso de otros productos 
    set @ingoprod = @ingtitcon + @ingconycon
    
 
end;
GO