/****** Object:  StoredProcedure [dbo].[spCalculaOfertaValorBMSC]    Script Date: 08/27/2013 11:35:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--***********************************************************************************
--******** Procedimiento para el cálculo de la Oferta de Valor **********************
--Parámetros:
--Fecha: 
CREATE Procedure [dbo].[spCalculaOfertaValorBMSC] (@idcliente     uniqueidentifier = null,
                                           @iEdad         int = null,
                                           @bTieneHijos   bit = null,
                                           @iEstadoCivil  int = null,
                                           @error         varchar(200) output)
As 
declare @valorMaximoProceso as int
declare @valorMinimoProceso as int
declare @cantidadRegistrosPorLote as int
declare @valorMaximoLote as int
declare @valorMinimoLote as int
declare @continuar as bit                                           
Begin
        Set @continuar = 0
        Set @cantidadRegistrosPorLote = 0
		--1: Creamos la tabla de subsegmento/subtipo-producto
		---------------------------------------------------------------------------------------------------
		--Query para armar la tabla de productos
		create table #TMP_SEGMENTO_SUBTIPO
		(segmento uniqueidentifier,
		subtipo uniqueidentifier)

		insert into #TMP_SEGMENTO_SUBTIPO
		select
		oferta.efk_Segmento Segmento,
		subtipo.efk_subtipo_producto_crm Subtipo_ProductoId
		from BMSC_MSCRM..efk_subtipo_producto_oferta_valor subtipo
			inner join BMSC_MSCRM..efk_tipo_producto_portafolio tipo
				on subtipo.efk_tipo_producto_portafolio=tipo.efk_tipo_producto_portafolioId
			inner join BMSC_MSCRM..efk_portafolio_segmento portafolio
				on tipo.efk_producto_segmento=portafolio.efk_portafolio_segmentoId
			inner join BMSC_MSCRM..efk_oferta_valor_banco oferta
				on portafolio.efk_PortafolioId=oferta.efk_oferta_valor_bancoId
			
		union all

		select
		oferta.efk_Segmento Segmento,
		subtipo.efk_subtipo_producto_crm
		from BMSC_MSCRM..efk_oferta_valor_banco oferta
			inner join BMSC_MSCRM..efk_subtipo_producto_oferta_valor subtipo
			on subtipo.efk_oferta_valorid=oferta.efk_oferta_valor_bancoId
		order by 1 asc

		--select * from #TMP_SEGMENTO_SUBTIPO
		
		---------------------------------------------------------------------------------------------------
		--2: Creamos la tabla Cliente/Subtipo en base al segmento del cliente
		---------------------------------------------------------------------------------------------------
		create table #TMP_CLIENTE_SUBTIPO_ORIGINAL
		(accountid uniqueidentifier,
		codigoCliente int,
		subtipoProducto uniqueidentifier
		)

		If (@idcliente is null)
		begin
			insert into #TMP_CLIENTE_SUBTIPO_ORIGINAL
			select
			cuenta.accountid,
			cuenta.efk_codigo_cliente,
			subtipo.subtipo
			from BMSC_MSCRM..Account cuenta inner join #TMP_SEGMENTO_SUBTIPO subtipo
			on cuenta.efk_segmento_ovid=subtipo.segmento
			
			Update  BMSC_MSCRM..AccountExtensionBase set efk_edad = Case when Not efk_FechadeNacimiento is null then (SELECT DATEDIFF (YY, efk_FechadeNacimiento, GETDATE()) -
                                                                              CASE WHEN (MONTH(efk_FechadeNacimiento)=MONTH(GETDATE()) AND DAY(efk_FechadeNacimiento) > DAY(GETDATE()) OR MONTH (efk_FechadeNacimiento) > MONTH (GETDATE()))
																	          THEN 1 
                                                                                 ELSE 0 
                                                                               END)
                                                                           else
                                                                              null
                                                                         end,
                                                           efk_tiene_hijos = Case when efk_NrodeHijos > 0 then 1
                                                                                    when efk_NrodeHijos is null then 0
                                                                                    else 0
                                                                               end 
                    
			
			-- Seteamos las variables para que se realice el insert de la oferta de valor masiva por bloques de registros
            set @cantidadRegistrosPorLote=10000
			select @valorMaximoProceso=max(efk_codigo_cliente) from BMSC_MSCRM..Account where efk_codigo_cliente < 1300000
			select @valorMinimoProceso=min(efk_codigo_cliente) from BMSC_MSCRM..Account
			set @continuar=1 
			set @valorMinimoLote=@valorMinimoProceso
			set @valorMaximoLote=@valorMinimoLote+@cantidadRegistrosPorLote                                                                               
                                                                             
		end
		else
		begin
		    Select * into #TMP_ACCOUNT from BMSC_MSCRM..Account 
		    where AccountId = @idcliente
		    
		    Select * into #TMP_ClientesServiciosCanales from BDI_ClientesServiciosCanales
		    where iCodigoClienteBanco = (Select efk_codigo_cliente from BMSC_MSCRM..Account where AccountId = @idcliente)
		    
			insert into #TMP_CLIENTE_SUBTIPO_ORIGINAL
			select
			cuenta.accountid,
			cuenta.efk_codigo_cliente,
			subtipo.subtipo
			from #TMP_ACCOUNT cuenta inner join #TMP_SEGMENTO_SUBTIPO subtipo
			on cuenta.efk_segmento_ovid=subtipo.segmento		
			where cuenta.AccountId = @idcliente
			
			-- Actualizo los campos para el cálculo de OFV
			Update BMSC_MSCRM..AccountExtensionBase   set efk_Edad = @iEdad,
			                                              efk_tiene_hijos = @bTieneHijos,
			                                              efk_estado_civil = @iEstadoCivil
			Where AccountId = @idcliente
		end	
			
		drop table #TMP_SEGMENTO_SUBTIPO
		
		--select * from #TMP_CLIENTE_SUBTIPO_ORIGINAL

		
		---------------------------------------------------------------------------------------------------
		--3: Eliminamos de la tabla #TMP_CLIENTE_SUBTIPO los productos que ya posee el cliente y la pasamos a 
		--una nueva tabla
		---------------------------------------------------------------------------------------------------
		
		--Creamos la tabla de subtipo y tipo para oferta de valor
		
		select
		oferta.efk_Segmento Segmento,
		tipo.efk_Tipodeproducto TipoProducto,
		subtipo.efk_subtipo_producto_crm SubtipoProducto
		into #TMP_SUBTIPO_TIPO_OFERTA_VALOR
		from BMSC_MSCRM..efk_subtipo_producto_oferta_valor subtipo
		inner join BMSC_MSCRM..efk_tipo_producto_portafolio tipo on subtipo.efk_tipo_producto_portafolio=tipo.efk_tipo_producto_portafolioId
		inner join BMSC_MSCRM..efk_portafolio_segmento portafolio on portafolio.efk_portafolio_segmentoId=tipo.efk_producto_segmento
		inner join BMSC_MSCRM..efk_oferta_valor_banco oferta on oferta.efk_oferta_valor_bancoId=portafolio.efk_portafolioid
		
		
		create table #TMP_CLIENTE_SUBTIPO_PRELIMINAR
		(accountid uniqueidentifier,
		codigoCliente int,
		subtipoProducto uniqueidentifier
		)

		--La siguiente comparación la hacemos para los productos que poseen Tipo de producto en la oferta de valor
		If (@idcliente is null)
		begin
			insert into #TMP_CLIENTE_SUBTIPO_PRELIMINAR
			select
			distinct
			subtipo.accountid,
			subtipo.codigoCliente,
			subtipo.subtipoProducto
			from #TMP_CLIENTE_SUBTIPO_ORIGINAL subtipo
			inner join BMSC_MSCRM..Account cuenta
				on cuenta.efk_codigo_cliente=subtipo.codigoCliente
			inner join #TMP_SUBTIPO_TIPO_OFERTA_VALOR producto
				on producto.SubtipoProducto=subtipo.subtipoProducto and cuenta.efk_segmento_ovid=producto.Segmento
			where not exists (select 1 from BDI_ClientesServiciosCanales productos 
										inner join BMSC_MSCRM..efk_producto_core Subcore on Subcore.efk_codigo_producto_core=productos.sCodigoUnicoProducto collate database_default
										inner join BMSC_MSCRM..Product prd on prd.ProductId=Subcore.efk_productid
										inner join BMSC_MSCRM..Account cuenta2
											on cuenta2.efk_codigo_cliente=productos.iCodigoClienteBanco
										inner join #TMP_SUBTIPO_TIPO_OFERTA_VALOR producto2
											on producto2.SubtipoProducto=prd.ProductId and cuenta2.efk_segmento_ovid=producto2.Segmento
										where subtipo.codigoCliente=productos.iCodigoClienteBanco
											and producto2.TipoProducto=producto.TipoProducto)
			
			--drop table #TMP_CLIENTE_SUBTIPO_ORIGINAL
			
			--select * from #TMP_CLIENTE_SUBTIPO_PRELIMINAR where accountid = 'DD0C3658-99F3-E111-B271-005056BC18E3'
			--Debemos comparar también con aquellos productos que no poseen tipo de producto
			select
			oferta.efk_Segmento Segmento,
			subtipo.efk_subtipo_producto_crm SubtipoProducto
			into #TMP_SUBTIPO_TIPO_OFERTA_VALOR_SIN_TIPO
			from BMSC_MSCRM..efk_subtipo_producto_oferta_valor subtipo
			inner join BMSC_MSCRM..efk_oferta_valor_banco oferta on subtipo.efk_oferta_valorid=oferta.efk_oferta_valor_bancoId
			where subtipo.efk_tipo_producto_portafolio is null
			
			insert into #TMP_CLIENTE_SUBTIPO_PRELIMINAR
			select
			distinct
			subtipo.accountid,
			subtipo.codigoCliente,
			subtipo.subtipoProducto
			from #TMP_CLIENTE_SUBTIPO_ORIGINAL subtipo
			inner join BMSC_MSCRM..Account cuenta
				on cuenta.efk_codigo_cliente=subtipo.codigoCliente
			inner join #TMP_SUBTIPO_TIPO_OFERTA_VALOR_SIN_TIPO producto
				on producto.SubtipoProducto=subtipo.subtipoProducto and cuenta.efk_segmento_ovid=producto.Segmento
			where not exists (select 1 from BDI_ClientesServiciosCanales productos 
										inner join BMSC_MSCRM..efk_producto_core Subcore on Subcore.efk_codigo_producto_core=productos.sCodigoUnicoProducto collate database_default
										inner join BMSC_MSCRM..Product prd on prd.ProductId=Subcore.efk_productid
										inner join BMSC_MSCRM..Account cuenta2
											on cuenta2.efk_codigo_cliente=productos.iCodigoClienteBanco
										inner join #TMP_SUBTIPO_TIPO_OFERTA_VALOR_SIN_TIPO producto2
											on producto2.SubtipoProducto=prd.ProductId and cuenta2.efk_segmento_ovid=producto2.Segmento
			
										where subtipo.codigoCliente=productos.iCodigoClienteBanco
											and producto2.SubtipoProducto=producto.SubtipoProducto)
			
			--select * from #TMP_CLIENTE_SUBTIPO_PRELIMINAR where codigoCliente=756145
			drop table #TMP_SUBTIPO_TIPO_OFERTA_VALOR_SIN_TIPO
		end
		else
		begin		    
			insert into #TMP_CLIENTE_SUBTIPO_PRELIMINAR
			select
			distinct
			subtipo.accountid,
			subtipo.codigoCliente,
			subtipo.subtipoProducto
			from #TMP_CLIENTE_SUBTIPO_ORIGINAL subtipo
			inner join #TMP_ACCOUNT cuenta
				on cuenta.AccountId=subtipo.accountid
			inner join #TMP_SUBTIPO_TIPO_OFERTA_VALOR producto
				on producto.SubtipoProducto=subtipo.subtipoProducto and cuenta.efk_segmento_ovid=producto.Segmento
			where not exists (select 1 from #TMP_ClientesServiciosCanales productos 
										inner join BMSC_MSCRM..efk_producto_core Subcore on Subcore.efk_codigo_producto_core=productos.sCodigoUnicoProducto collate database_default
										inner join BMSC_MSCRM..Product prd on prd.ProductId=Subcore.efk_productid
										inner join #TMP_ACCOUNT cuenta2
											on cuenta2.efk_codigo_cliente=productos.iCodigoClienteBanco
										inner join #TMP_SUBTIPO_TIPO_OFERTA_VALOR producto2
											on producto2.SubtipoProducto=prd.ProductId and cuenta2.efk_segmento_ovid=producto2.Segmento
										where subtipo.codigoCliente=productos.iCodigoClienteBanco
											and producto2.TipoProducto=producto.TipoProducto)
			
			--drop table #TMP_CLIENTE_SUBTIPO_ORIGINAL
			
			--select * from #TMP_CLIENTE_SUBTIPO_PRELIMINAR where accountid = 'DD0C3658-99F3-E111-B271-005056BC18E3'
			--Debemos comparar también con aquellos productos que no poseen tipo de producto
			select
			oferta.efk_Segmento Segmento,
			subtipo.efk_subtipo_producto_crm SubtipoProducto
			into #TMP_SUBTIPO_TIPO_OFERTA_VALOR_SIN_TIPO_C
			from BMSC_MSCRM..efk_subtipo_producto_oferta_valor subtipo
			inner join BMSC_MSCRM..efk_oferta_valor_banco oferta on subtipo.efk_oferta_valorid=oferta.efk_oferta_valor_bancoId
			where subtipo.efk_tipo_producto_portafolio is null
			
			insert into #TMP_CLIENTE_SUBTIPO_PRELIMINAR
			select
			distinct
			subtipo.accountid,
			subtipo.codigoCliente,
			subtipo.subtipoProducto
			from #TMP_CLIENTE_SUBTIPO_ORIGINAL subtipo
			inner join #TMP_ACCOUNT cuenta
				on cuenta.AccountId=subtipo.accountid
			inner join #TMP_SUBTIPO_TIPO_OFERTA_VALOR_SIN_TIPO_C producto
				on producto.SubtipoProducto=subtipo.subtipoProducto and cuenta.efk_segmento_ovid=producto.Segmento
			where not exists (select 1 from BDI_ClientesServiciosCanales productos 
										inner join BMSC_MSCRM..efk_producto_core Subcore on Subcore.efk_codigo_producto_core=productos.sCodigoUnicoProducto collate database_default
										inner join BMSC_MSCRM..Product prd on prd.ProductId=Subcore.efk_productid
										inner join #TMP_ACCOUNT cuenta2
											on cuenta2.efk_codigo_cliente=productos.iCodigoClienteBanco
										inner join #TMP_SUBTIPO_TIPO_OFERTA_VALOR_SIN_TIPO_C producto2
											on producto2.SubtipoProducto=prd.ProductId and cuenta2.efk_segmento_ovid=producto2.Segmento
			
										where subtipo.codigoCliente=productos.iCodigoClienteBanco
											and producto2.SubtipoProducto=producto.SubtipoProducto)
			
			--select * from #TMP_CLIENTE_SUBTIPO_PRELIMINAR where codigoCliente=756145
			
			--drop table #TMP_CLIENTE_SUBTIPO_ORIGINAL
			drop table #TMP_SUBTIPO_TIPO_OFERTA_VALOR_SIN_TIPO_C
		end
		drop table #TMP_SUBTIPO_TIPO_OFERTA_VALOR
		drop table #TMP_CLIENTE_SUBTIPO_ORIGINAL		
		---------------------------------------------------------------------------------------------------
		--4: Creamos la tabla de subtipo/criterios
		---------------------------------------------------------------------------------------------------
		create table #TMP_SUBTIPO_CRITERIO
		(subtipo uniqueidentifier,
		criterio uniqueidentifier)

		insert into #TMP_SUBTIPO_CRITERIO
		select
		subtipo.efk_subtipo_producto_crm Subtipo_Producto,
		criterio.efk_criterios_valorId
		from
		BMSC_MSCRM..efk_subtipo_producto_oferta_valor_criterio subtipo_criterio
		inner join BMSC_MSCRM..efk_criterios_valor criterio
			on subtipo_criterio.efk_criterios_valorid=criterio.efk_criterios_valorId
		inner join BMSC_MSCRM..efk_subtipo_producto_oferta_valor subtipo
			on subtipo_criterio.efk_subtipo_producto_oferta_valorid=subtipo.efk_subtipo_producto_oferta_valorId

		--select * from #TMP_SUBTIPO_CRITERIO
		
		
		---------------------------------------------------------------------------------------------------
		--5: Creamos la tabla de CRITERIOS la que incluye el "where"
		---------------------------------------------------------------------------------------------------
		create table #TMP_CRITERIOS
		(numero int identity,
		criterio uniqueidentifier,
		consulta nvarchar(1000))

		insert into #TMP_CRITERIOS
		select
		criterio.efk_criterios_valorId,
		--procedemos a calcular el query
		'('+
		case when efk_valor1 is not null then
			(select case 
						when parametro.efk_criterio_evaluacion=100000001 then ---rango
							parametro.efk_nombre_esquema+'>='+cast(valor.efk_valor_inicial as nvarchar) + ' and '+parametro.efk_nombre_esquema+'<='+cast(valor.efk_valor_final as nvarchar)
						when parametro.efk_criterio_evaluacion=100000002 then --dos opciones
							case when efk_valorsiono=1 then parametro.efk_nombre_esquema+'=1'
								else parametro.efk_nombre_esquema+'=0' end
						when parametro.efk_criterio_evaluacion=100000003 then --multiples opciones
							parametro.efk_nombre_esquema+' in (select efk_codigo_seleccionado from BMSC_MSCRM..efk_detalle_criterios_valor where efk_ValordeparmetroId=''{'+CAST(valor.efk_valorparmetroofertavalorId as nvarchar(50))+'}'')'
						end
				from BMSC_MSCRM..efk_valorparmetroofertavalor valor inner join BMSC_MSCRM..efk_parmetroofertavalor parametro
					on valor.efk_valorparmetroId=parametro.efk_parmetroofertavalorId
					where valor.efk_valorparmetroofertavalorId=criterio.efk_valor1) +')'
		else '' end 

		+ case when efk_valor2 is not null then ' and (' +
			(select case 
						when parametro.efk_criterio_evaluacion=100000001 then ---rango
							parametro.efk_nombre_esquema+'>='+cast(valor.efk_valor_inicial as nvarchar) + ' and '+parametro.efk_nombre_esquema+'<='+cast(valor.efk_valor_final as nvarchar)
						when parametro.efk_criterio_evaluacion=100000002 then --dos opciones
							case when efk_valorsiono=1 then parametro.efk_nombre_esquema+'=1'
								else parametro.efk_nombre_esquema+'=0' end
						when parametro.efk_criterio_evaluacion=100000003 then --multiples opciones
							parametro.efk_nombre_esquema+' in (select efk_codigo_seleccionado from BMSC_MSCRM..efk_detalle_criterios_valor where efk_ValordeparmetroId=''{'+CAST(valor.efk_valorparmetroofertavalorId as nvarchar(50))+'}'')'
						end
				from BMSC_MSCRM..efk_valorparmetroofertavalor valor inner join BMSC_MSCRM..efk_parmetroofertavalor parametro
					on valor.efk_valorparmetroId=parametro.efk_parmetroofertavalorId
					where valor.efk_valorparmetroofertavalorId=criterio.efk_valor2) +')'
		else '' end 

		+ case when efk_valor3 is not null then ' and (' +
			(select case 
						when parametro.efk_criterio_evaluacion=100000001 then ---rango
							parametro.efk_nombre_esquema+'>='+cast(valor.efk_valor_inicial as nvarchar) + ' and '+parametro.efk_nombre_esquema+'<='+cast(valor.efk_valor_final as nvarchar)
						when parametro.efk_criterio_evaluacion=100000002 then --dos opciones
							case when efk_valorsiono=1 then parametro.efk_nombre_esquema+'=1'
								else parametro.efk_nombre_esquema+'=0' end
						when parametro.efk_criterio_evaluacion=100000003 then --multiples opciones
							parametro.efk_nombre_esquema+' in (select efk_codigo_seleccionado from BMSC_MSCRM..efk_detalle_criterios_valor where efk_ValordeparmetroId=''{'+CAST(valor.efk_valorparmetroofertavalorId as nvarchar(50))+'}'')'
						end
				from BMSC_MSCRM..efk_valorparmetroofertavalor valor inner join BMSC_MSCRM..efk_parmetroofertavalor parametro
					on valor.efk_valorparmetroId=parametro.efk_parmetroofertavalorId
					where valor.efk_valorparmetroofertavalorId=criterio.efk_valor3) +')'
		else '' end 

		+ case when efk_valor4 is not null then ' and (' +
			(select case 
						when parametro.efk_criterio_evaluacion=100000001 then ---rango
							parametro.efk_nombre_esquema+'>='+cast(valor.efk_valor_inicial as nvarchar) + ' and '+parametro.efk_nombre_esquema+'<='+cast(valor.efk_valor_final as nvarchar)
						when parametro.efk_criterio_evaluacion=100000002 then --dos opciones
							case when efk_valorsiono=1 then parametro.efk_nombre_esquema+'=1'
								else parametro.efk_nombre_esquema+'=0' end
						when parametro.efk_criterio_evaluacion=100000003 then --multiples opciones
							parametro.efk_nombre_esquema+' in (select efk_codigo_seleccionado from BMSC_MSCRM..efk_detalle_criterios_valor where efk_ValordeparmetroId=''{'+CAST(valor.efk_valorparmetroofertavalorId as nvarchar(50))+'}'')'
						end
				from BMSC_MSCRM..efk_valorparmetroofertavalor valor inner join BMSC_MSCRM..efk_parmetroofertavalor parametro
					on valor.efk_valorparmetroId=parametro.efk_parmetroofertavalorId
					where valor.efk_valorparmetroofertavalorId=criterio.efk_valor4) +')'
		else '' end 


		+ case when efk_valor5 is not null then ' and (' +
			(select case 
						when parametro.efk_criterio_evaluacion=100000001 then ---rango
							parametro.efk_nombre_esquema+'>='+cast(valor.efk_valor_inicial as nvarchar) + ' and '+parametro.efk_nombre_esquema+'<='+cast(valor.efk_valor_final as nvarchar)
						when parametro.efk_criterio_evaluacion=100000002 then --dos opciones
							case when efk_valorsiono=1 then parametro.efk_nombre_esquema+'=1'
								else parametro.efk_nombre_esquema+'=0' end
						when parametro.efk_criterio_evaluacion=100000003 then --multiples opciones
							parametro.efk_nombre_esquema+' in (select efk_codigo_seleccionado from BMSC_MSCRM..efk_detalle_criterios_valor where efk_ValordeparmetroId=''{'+CAST(valor.efk_valorparmetroofertavalorId as nvarchar(50))+'}'')'
						end
				from BMSC_MSCRM..efk_valorparmetroofertavalor valor inner join BMSC_MSCRM..efk_parmetroofertavalor parametro
					on valor.efk_valorparmetroId=parametro.efk_parmetroofertavalorId
					where valor.efk_valorparmetroofertavalorId=criterio.efk_valor5) +')'
		else '' end 
		from BMSC_MSCRM..efk_criterios_valor criterio

		--select * from #TMP_CRITERIOS
		
		--------------------------------------------------------------------------------------------------
		--6: Creamos la tabla cliente/subtipo final y pasamos a ella los registros que no necesitan filtro
		--- y que son Jurídicos, pues a los jurídicos no se le aplica la oferta de valor por ciclo de vida
		---------------------------------------------------------------------------------------------------
		create table #TMP_CLIENTE_SUBTIPO_FINAL
		(accountid uniqueidentifier,
		codigoCliente int,
		subtipoProducto uniqueidentifier
		)

		If (@idcliente is null)
		begin
			insert into #TMP_CLIENTE_SUBTIPO_FINAL
			select pre.accountid, pre.codigoCliente, pre.subtipoProducto
			from #TMP_CLIENTE_SUBTIPO_PRELIMINAR pre
			inner join BMSC_MSCRM..Account cuenta on cuenta.AccountId=pre.accountid
			where 
			--pre.subtipoProducto not in (select subtipoProducto from #TMP_SUBTIPO_CRITERIO)
			not exists (select 1 from #TMP_SUBTIPO_CRITERIO tmp where tmp.subtipo=pre.subtipoProducto)
			or cuenta.efk_tipo_cliente=221220001 --los jurídicos no necesitan filtro
		end
		else
		begin
			insert into #TMP_CLIENTE_SUBTIPO_FINAL
			select pre.accountid, pre.codigoCliente, pre.subtipoProducto
			from #TMP_CLIENTE_SUBTIPO_PRELIMINAR pre
			inner join #TMP_ACCOUNT cuenta on cuenta.AccountId=pre.accountid
			where 
			--pre.subtipoProducto not in (select subtipoProducto from #TMP_SUBTIPO_CRITERIO)
			not exists (select 1 from #TMP_SUBTIPO_CRITERIO tmp where tmp.subtipo=pre.subtipoProducto)
			or cuenta.efk_tipo_cliente=221220001 --los jurídicos no necesitan filtro
		end		
		
		--select * from #TMP_SUBTIPO_CRITERIO

		--select distinct codigoCliente from #TMP_CLIENTE_SUBTIPO_PRELIMINAR
		--select * from #TMP_CLIENTE_SUBTIPO_PRELIMINAR
		
		--select distinct codigoCliente from #TMP_CLIENTE_SUBTIPO_FINAL
		--select * from #TMP_CLIENTE_SUBTIPO_FINAL
		
		--select * from #TMP_CLIENTE_SUBTIPO_FINAL
		--select distinct codigoCliente from #TMP_CLIENTE_SUBTIPO_FINAL  where subtipoProducto='C50212CF-EFF2-E111-9D8A-005056BC18E3'
		--order by 1 asc
		
		---------------------------------------------------------------------------------------------------
		--7: Aplicamos la evaluación de los clientes para aquellos que necesitan filtro
		---------------------------------------------------------------------------------------------------
		create table #TMP_CLIENTE_SUBTIPO_EVALUACION
		(accountid uniqueidentifier,
		codigoCliente int,
		subtipoProducto uniqueidentifier
		)


		declare @contador as int
		declare @cantidadRegistrosCriterios as int
		set @contador=1
		set @cantidadRegistrosCriterios=(select COUNT(1) from  #TMP_CRITERIOS)

		while @contador<=@cantidadRegistrosCriterios
		BEGIN
			declare @SQL as nvarchar(4000)
			declare @idCriterio as uniqueidentifier
			declare @filtroCriterio as nvarchar(1000)
			
			set @idCriterio=(select criterio from #TMP_CRITERIOS where numero=@contador)
			set @filtroCriterio=(select consulta from #TMP_CRITERIOS where numero=@contador)
			
			If (@idcliente is null)
			begin
				set @SQL= 'Insert into #TMP_CLIENTE_SUBTIPO_EVALUACION
				select cliente.accountid, cliente.codigoCliente, cliente.subtipoProducto
				from #TMP_CLIENTE_SUBTIPO_PRELIMINAR cliente
				inner join BMSC_MSCRM..Account cuenta on cuenta.AccountId=cliente.accountid
				inner join #TMP_SUBTIPO_CRITERIO subtipo_criterio on cliente.subtipoProducto=subtipo_criterio.subtipo 
				inner join #TMP_CRITERIOS criterio on subtipo_criterio.criterio=criterio.criterio
				where subtipo_criterio.criterio=''{'+cast (@idCriterio as nvarchar(50))+'}'' and ' + @filtroCriterio
			end
			else
			begin
				set @SQL= 'Insert into #TMP_CLIENTE_SUBTIPO_EVALUACION
				select cliente.accountid, cliente.codigoCliente, cliente.subtipoProducto
				from #TMP_CLIENTE_SUBTIPO_PRELIMINAR cliente
				inner join #TMP_ACCOUNT cuenta on cuenta.AccountId=cliente.accountid
				inner join #TMP_SUBTIPO_CRITERIO subtipo_criterio on cliente.subtipoProducto=subtipo_criterio.subtipo 
				inner join #TMP_CRITERIOS criterio on subtipo_criterio.criterio=criterio.criterio
				where subtipo_criterio.criterio=''{'+cast (@idCriterio as nvarchar(50))+'}'' and ' + @filtroCriterio			
			end
			--print @SQL
			
			exec( @SQL)
			
			set @contador=@contador+1
		END
		
		--select * from #TMP_CLIENTE_SUBTIPO_EVALUACION
		drop table #TMP_SUBTIPO_CRITERIO
		drop table #TMP_CRITERIOS
		---------------------------------------------------------------------------------------------------
		--7: Insertamos en la tabla cliente/subtipo final los registros que hayan cumplido con el filtro
		---------------------------------------------------------------------------------------------------
		insert into #TMP_CLIENTE_SUBTIPO_FINAL
		select cliente.accountid, cliente.codigoCliente, cliente.subtipoProducto
		from #TMP_CLIENTE_SUBTIPO_PRELIMINAR cliente
		where exists (select 1 from #TMP_CLIENTE_SUBTIPO_EVALUACION evaluacion where evaluacion.codigoCliente=cliente.codigoCliente and evaluacion.subtipoProducto=cliente.subtipoProducto)
				
		drop table #TMP_CLIENTE_SUBTIPO_PRELIMINAR
		drop table #TMP_CLIENTE_SUBTIPO_EVALUACION
		---------------------------------------------------------------------------------------------------
		--8: Insertamos la oferta de valor en MS CRM
		---------------------------------------------------------------------------------------------------
		
		If (@idcliente is null)
		begin
			----Truncamos la tabla de oferta de valor de MS CRM
			TRUNCATE TABLE BMSC_MSCRM..efk_oferta_valorextensionbase

			ALTER TABLE BMSC_MSCRM..efk_oferta_valorextensionbase DROP CONSTRAINT [FK_efk_oferta_valorExtensionBase_efk_oferta_valorBase]

			TRUNCATE TABLE BMSC_MSCRM..efk_oferta_valorbase

			ALTER TABLE BMSC_MSCRM..efk_oferta_valorextensionbase  WITH NOCHECK ADD  CONSTRAINT [FK_efk_oferta_valorExtensionBase_efk_oferta_valorBase] FOREIGN KEY([efk_oferta_valorId])
			REFERENCES [efk_oferta_valorBase] ([efk_oferta_valorId])
			NOT FOR REPLICATION

			ALTER TABLE BMSC_MSCRM..efk_oferta_valorextensionbase NOCHECK CONSTRAINT [FK_efk_oferta_valorExtensionBase_efk_oferta_valorBase]
		end
		else
		begin
			Select ofe.efk_oferta_valorId into #tempOfe from BMSC_MSCRM..efk_oferta_valorExtensionBase ofe
			where ofe.efk_cliente_juridico_Id = @idcliente
			
			Delete from BMSC_MSCRM..efk_oferta_valorExtensionBase where efk_cliente_juridico_Id = @idcliente
			
			Delete from BMSC_MSCRM..efk_oferta_valorBase where efk_oferta_valorId in (Select efk_oferta_valorId from #tempOfe)	
			
			drop table #tempOfe	
		end	

		------------------------------------------------------------------------------------------------------------
		------------------------------------------------------------------------------------------------------------
		
		create table #TMP_OFERTA_VALOR(
			efk_oferta_valorId uniqueidentifier not null default NEWSEQUENTIALID(),
			accountid uniqueidentifier,
			subtipoProducto uniqueidentifier
		)
		
		insert into #TMP_OFERTA_VALOR (accountid,subtipoProducto)
		select
		accountid,
		subtipoProducto
		from  #TMP_CLIENTE_SUBTIPO_FINAL
						
		DECLARE @nombreUsuario as nvarchar(100)
		DECLARE @IdUsrDefault AS uniqueidentifier
		
		set @nombreUsuario=(select SYSTEM_USER)
		
		SELECT @IdUsrDefault = SystemUserId FROM BMSC_MSCRM..SystemUser USR WHERE 
		USR.DomainName = @nombreUsuario
		
		If (Not @idcliente is Null)
		begin
			insert into 
			BMSC_MSCRM..efk_oferta_valorBase
			(efk_oferta_valorId,
			CreatedOn,CreatedBy,ModifiedOn,ModifiedBy,OwnerId,OwnerIdType,OwningBusinessUnit,statecode,statuscode,
			ImportSequenceNumber,TimeZoneRuleVersionNumber,UTCConversionTimeZoneCode
			)
			SELECT 
			ov.efk_oferta_valorId AS efk_oferta_valorId,
			GETDATE() AS CreatedOn,
			@IdUsrDefault AS CreatedBy,
			getdate() AS ModifiedOn,
			@IdUsrDefault AS ModifiedBy,
			c.OwnerId AS OwnerId,
			8 AS OwnerIdType,
			c.OwningBusinessUnit AS OwningBusinessUnit,
			0 AS statecode,
			1 AS statuscode,
			null AS ImportSequenceNumber,
			null AS TimeZoneRuleVersionNumber,
			null AS UTCConversionTimeZoneCode
			from #TMP_OFERTA_VALOR ov inner join 
			#TMP_ACCOUNT c on ov.AccountId=c.accountid 

					
			insert into BMSC_MSCRM..efk_oferta_valorExtensionBase
			(efk_oferta_valorId,efk_name,efk_new_name,efk_prioridad,efk_prioridad_tipo,efk_cliente_juridico_Id,efk_familia_productos_id,
			efk_product_id,efk_tipo_productos_id,efk_Portafolio, efk_prioridad_portafolio
			)
			SELECT 
			ov.efk_oferta_valorId AS efk_oferta_valorId,
			subtipo.efk_name AS efk_name, 
			subtipo.efk_name AS efk_new_name,
			subtipo.efk_prioridad AS efk_prioridad,
		    (tipo.efk_prioridad*10)+(subtipo.efk_prioridad) AS efk_prioridad_tipo,
			ov.AccountId AS efk_cliente_juridico_Id,
			null AS efk_familia_productos_id,
			ov.subtipoProducto AS efk_product_id,
			tipo.efk_Tipodeproducto AS efk_tipo_productos_id,
			portafolio.efk_name AS efk_Portafolio,
			portafolio.efk_prioridad
			from #TMP_OFERTA_VALOR ov inner join #TMP_ACCOUNT ac on ov.accountid = ac.AccountId
			inner join BMSC_MSCRM..efk_oferta_valor_banco oferta on oferta.efk_Segmento = ac.efk_segmento_ovid
			inner join BMSC_MSCRM..efk_subtipo_producto_oferta_valor subtipo on subtipo.efk_subtipo_producto_crm = ov.subtipoProducto and oferta.efk_oferta_valor_bancoId = subtipo.efk_oferta_valorid
			inner join BMSC_MSCRM..efk_portafolio_segmento portafolio on portafolio.efk_portafolio_segmentoId=subtipo.efk_portafolio_segmentoid
			left join BMSC_MSCRM..efk_tipo_producto_portafolio tipo on subtipo.efk_tipo_producto_portafolio=tipo.efk_tipo_producto_portafolioId
			
			drop table #TMP_ClientesServiciosCanales
		    drop table #TMP_ACCOUNT					    
       end
       else
       begin
			while @continuar=1
			BEGIN
					
				exec spCalculaOfertaValorBMSCLotes @valorMinimoLote,@valorMaximoLote,@IdUsrDefault
				
				set @valorMinimoLote=@valorMaximoLote + 1
				set @valorMaximoLote=@valorMinimoLote+@cantidadRegistrosPorLote
				if @valorMinimoLote>@valorMaximoProceso
				begin
					set @continuar=0
				end
			END	      
       end

	   drop table #TMP_CLIENTE_SUBTIPO_FINAL
	   drop table #TMP_OFERTA_VALOR	   
		
		if @@error <> 0
			Set @error = 'Error: ' + @error + LTRIM(str(@@ERROR))
			
End
GO
