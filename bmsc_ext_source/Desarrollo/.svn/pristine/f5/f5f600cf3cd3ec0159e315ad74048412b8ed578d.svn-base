/****** Object:  StoredProcedure [dbo].[sp_maximo_endeudamiento]    Script Date: 08/27/2013 11:35:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_maximo_endeudamiento] (
											@c_productosimuladoId varchar(max), -- codigo del prodcuto simulado
											@c_simulacionId varchar(max), -- codigo de simulacion
											@c_cliente varchar(max), --- codigo del cliente 
											@c_ProductId varchar(max), --- subtipo del producto
											@c_ProductoTipo varchar(max), -- tipo del producto
											@c_ProductoFamilia varchar(max), -- familia del producto
											@c_plazo int,  -- plazo en meses 
											@c_tasfij DECIMAL(20,12), -- tasa fija
											@c_tre DECIMAL(20,12), --- TRE tasa variable 
											@c_sprfij DECIMAL(20,12), -- spread fijo
											@c_tasvarpar int, -- tasa variable a partir del mes
											@c_orden int, -- orden 
											@c_amocad int, --- amortizacion cada
											@c_tasvar DECIMAL(20,12),  -- tasa variable 
											@c_monsol DECIMAL(20,12) , -- cuota maxima solicitada 
											@c_llamada int, -- origen de llamada si es 0 es de la pantalla si es 1 es del reporte 
											@c_segcesantia int , --- SI=1, NO = 0 se calcula el seguro de cesantia
											@c_segdesgrav int --- SI=1, NO = 0 se calcula el seguro de desgravamen 
											 ) as
begin
declare @sqlserver varchar(max) 
----- es temporalmente x q tiene q ir en una entidad 
declare @tasegdes DECIMAL(20,12) = 0.78 -- tasa de seguro desgravamen
declare @tasegces DECIMAL(20,12) = 0.68 -- tasa de seguro de cesantia
declare @taspagmin DECIMAL(20,12) = 0.07 -- tasa de pagominimo
declare @cuominsob DECIMAL(20,12) = 26.09 -- cuota minima sobrante 
-------------------------------------------------
declare	@c_productohom int=0  --- producto homologado 
declare @aux_tasdesctoreal DECIMAL(20,12)  =0 -- tasa de descuento real
declare @c_ingviv DECIMAL(20,12) =0 --- ingreso de vivienda
declare @c_ingotrpro DECIMAL(20,12) = 0 -- ingreso de otros productos o ingreso de consumos
declare @cuomaxtasdes DECIMAL(20,12) = 0 -- cuoma maxima de tasa de descuento
declare @cuomaxsol DECIMAL(20,12) =0 -- cuota maxima de solicitud
declare @aux_prviv DECIMAL(20,12)=0 -- auxliliar porcentaje vivienda
declare @aux_proprod DECIMAL(20,12) =0 -- auxiliar porcentaje de otros productos
declare @aux_CEMajustadoviv DECIMAL(20,12) =0  -- cem ajustado de vivienda
declare @aux_CEMajustadoprod DECIMAL(20,12) =0 -- cem ajustado de otrod productos
declare @aux_CEMviv DECIMAL(20,12) = 0 --- cem de vivienda
declare @aux_CEMprod DECIMAL(20,12) = 0 -- cem de producto
declare @aux_calendmaxref DECIMAL(20,12)=0  -- calen
declare @aux_tasdesreal DECIMAL(20,12) =0 -- tasa de descuento real
declare @aux_montodes DECIMAL(20,12) =0 -- monto descuento
declare @aux_monmaxreal DECIMAL(20,12)=0  -- monto maximo real
declare @aux_cuosob DECIMAL(20,12)=0  -- cuota sobrante
declare @aux_nrocuovar int  -- numero de cuotas variables
declare @errorprint varchar(8000)='' -- error impresion
declare @aux_cuotaSF DECIMAL(20,12) --- total de deudas del sistema financiero 
-------------------variables TDC
declare @aux_monlinrgoing DECIMAL(20,12)	--- monto linea segun rango de ingresos
declare @aux_monlinporcreldeuing DECIMAL(20,12) -- monto de linea según porcentaje de realación deuda ingreso
declare @aux_rel DECIMAL(20,12)  -- auxiliar porcentaje de relacion
declare @aux_monlinfin DECIMAL(20,12)  -- monto de linea final
declare @aux_mintarj DECIMAL(20,12) -- minimo de tarjeta
declare @aux_minlinsol decimal (18,6)    	 -- minimo de linea solicitada		
declare @aux_prodanterior int -- producto anterior
-------------------- CARGA DE PARAMETROS --------------------------------- 
select @tasegdes  = ISNULL(dbo.fn_sp_parametro_mony('tasa_desgravamen'),0.78) 
select @tasegces  = ISNULL(dbo.fn_sp_parametro_mony('tasa_cesantia'),0.68)
select @taspagmin = ISNULL(dbo.fn_sp_parametro_mony('tasa_pag_min'),0.07)
select @cuominsob = ISNULL(dbo.fn_sp_parametro_mony('cuota_min_sobrante'),26.09)

--- Si el parametro de cesatía es cero, la tasa de cesantía queda en cero 
--- como las fomulas utilizan una multiplicación como queda en cero ya no es considerado 
if @c_segcesantia = 0 
BEGIN
   set  @tasegces = 0 
END
--- Si el parametro de desgravamen  es cero, la tasa de desgravamen queda en cero 
--- como las fomulas utilizan una multiplicación como queda en cero ya no es considerado 
if @c_segdesgrav = 0
BEGIN
   set @tasegdes = 0
END       


-- tipos de productos homologados 
-- 1 vivienda - 2 consumo - 3 tarjeta de credito 

        -- obtengo el producto homologado de la tabla de prudctos enviandole los 
        -- prametros de codigo de producto, tipo y familia
        select  @c_productohom = dbo.fn_sp_prodhom(@c_ProductId,@c_ProductoTipo,@c_ProductoFamilia)
        
        -- como no todos los productos tienen este campo lleno, se esta utilizando
        -- como default el de vivienda 
        set @c_productohom = ISNULL(@c_productohom,1) 
        -- se envia el codigo de cliente y retorna el ingreso de vivienda de cliente
        -- sumatoria de los campos ingresosov, valorpercibidoalquileres, valorpercibidosbonos/12,ingresoconyuguemensual
        select @c_ingviv = dbo.fn_sp_ingviv(@c_cliente ) 
    	-- se envia el codigo del cliente y retorna el ingreso de otros productos del cliente
		-- sumatoria de los campos ingresosov,valorpercibidoalquileres, ingresoconyuguemensual
		select @c_ingotrpro = dbo.fn_sp_ingprod(@c_cliente ) 
		-- numero de cuotas variables = plazo - tasa variable a partir del mes + 1
		set  @aux_nrocuovar  = @c_plazo - @c_tasvarpar + 1
		-- sumar a TRE el valor de variabilidad de la misma -- MUFC
		set @c_tre = @c_tre + ISNULL(dbo.fn_sp_variabilidadTRE(),0) 
		-- tasa variable = tasa tre + el spread fijo
		set @c_tasvar = @c_tre + @c_sprfij 
		--- Porcentaje de Relación de Vivienda: Se envía el ingreso de vivienda en la funcion
		--- con el producto homologado = vivienda (1) para q retorne el porcentaje de relacion
		select @aux_prviv = dbo.fn_sp_porcrelac(1, @c_ingviv )
    	--- Porcentaje de Relación de Otros Productos: Se envía el ingreso de otrosproductos en la funcion
		--- con el producto homologado = vivienda (2) para q retorne el porcentaje de relacion
		select @aux_proprod = dbo.fn_sp_porcrelac(2, @c_ingotrpro)
		--- calculo de capacidad de endeudamiento (CEM) de vivienda y de otros productos  
		set @aux_CEMviv = (@aux_prviv * @c_ingviv)/100 -- ( porcentaje de relacion vivienda * ingreso de vivienda ) /100
		set @aux_CEMprod =  (@aux_proprod * @c_ingotrpro)/100 -- ( porcentaje de relacion otros productos * ingreso de otros productos) /100
		
		--- se obtiene el total de deudas del sistema financiero 
		
	    select @aux_cuotaSF = isnull(efk_total_monto_cuotaSF,0) from BMSC_MSCRM..AccountExtensionBase  where cast(accountid as varchar(max)) = @c_cliente   --- vivienda
		-- calculo de CEM ajustado = CEM - total de deudas en el sistema financiero
		set @aux_CEMajustadoviv = isnull(@aux_CEMviv,0) - isnull(@aux_cuotaSF,0)
		set @aux_CEMajustadoprod = isnull(@aux_CEMprod,0) - isnull(@aux_cuotaSF,0)
		
	    /* validacion si da en negativo para q el monto sea cero  */
	    if @aux_CEMajustadoviv < 0 
	    begin
	      set @aux_CEMajustadoviv = 0  
	    end
	    if @aux_CEMajustadoprod < 0 
	    begin
	      set @aux_CEMajustadoprod = 0  
	    end
	  
---- inicio del algoritmo tomando enconsideración el orden de los productos 	    
	  if @c_orden = 1 and @c_productohom = '3'  --- si el primer producto que se ingresa es tarjeta de crédito 
    	   begin
    	        --print 'entra aqui 1'
    			-- monto de linea segun rango de ingresos
    			select @aux_monlinrgoing =  @c_ingotrpro  * dbo.fn_sp_rangoing(@c_ingotrpro) 
    			--- auxiliar de relacion 
    			select @aux_rel = dbo.fn_sp_porcrelac(@c_productohom,@c_productohom)/100 
    			-- monto de linea segun porcentaje de relacion deuda ingreso 
    			--- nueva validacion por Sergio Enero 17/2013 cambiar c_ingotro por c_cemajustado_prod
    		    -- si el cem de consumo es negativo cambia a cemprod si no queda ingotroprod
    		    
    		     select @aux_monlinporcreldeuing =  @aux_CEMajustadoprod/@taspagmin
    		     
    		    --- fin de nueva validacion enero 17/20013
    
    			-- monto de linea final
    			select @aux_monlinfin = dbo.fn_sp_menorvalor(@aux_monlinporcreldeuing,@aux_monlinrgoing)
    			--- minimo de la tarjeta de credito que sirve como cuota mensual		
                set @aux_mintarj = 	@aux_monlinfin * @taspagmin
    			--- minimo de linea solicitada
    			if (@c_monsol>0) --- solo aplica si el monto solicitado es mayor a cero 
			    begin
    				set @aux_minlinsol = @c_monsol *  @taspagmin
    				--- esto se lo considera como cuota maxima solicitada y se lo almacena en la tabla
    				set @cuomaxsol =  @aux_minlinsol
    			end
    			
			end -- fin de si el primer producto que se agrega es tarjeta de crédito
      else  
           begin	
           --print 'entra aqui 2'  
    		-- solo cuando el orden es mayor a 1 se busca el producto anterior y se lo almacena
    		if @c_orden > 1 
            begin
    			select @aux_prodanterior = efk_producto from BMSC_MSCRM..efk_productosimulado 
    			where efk_producto_simuladoId = @c_simulacionId and  efk_orden = (@c_orden -1)
	    	end
	              
               -- el cem ajustado lo obtengo dependiendo el producto homologado
               -- si es vivienda mi cem ajustado = cem ajsutado de vivienda
               -- si es consumo mi cem ajustado = cem ajsuatdo de consumo 
      		    declare @CEMajustado DECIMAL(20,12)
    			--SELECT @CEMajustado = CASE @c_productohom WHEN 1 THEN round(@aux_CEMajustadoviv,2) ELSE ROUND(@aux_CEMajustadoprod,2) END
                  SELECT @CEMajustado = CASE @c_productohom WHEN 1 THEN @aux_CEMajustadoviv ELSE @aux_CEMajustadoprod END    			    			    			
      ---- INICIO si el orden es >=1 y no es una tarjeta de credito 
       if @c_productohom <> '3' 
         begin
                --print 'entra aqui 3'
    		    -- cálculo del endeudamiento máximo referencia -- esto es por producto
    			if (@CEMajustado*@c_plazo) > 0 --- validacion de divisiones para cero 
				begin
				   --print 'entra aqui 3.12'
				  set @aux_calendmaxref  = (@CEMajustado*@c_plazo)-(@tasegdes/(12*@CEMajustado*@c_plazo))-((@tasegces/1000)*@CEMajustado*@c_plazo)
			    end 			    
			    -- calculo de tasa de descuento real 
			    -- se envia el producto homologado a la funcion y retorna un porcentaje en base a la tabla de parametros 
				select   @aux_tasdesreal = dbo.fn_sp_tasdesreal(@c_productohom) 
        	    -- tasa de descuento real por producto    
			    if (@aux_calendmaxref) > 0 -- validacion de division para cero 
				begin
				  set @aux_tasdesctoreal =   (@CEMajustado/@aux_calendmaxref)*@aux_tasdesreal-- guardar en la entidad
			    end
				-- monto maximo de credito elegido por tasa de descuento 
				if @aux_tasdesctoreal > 0 --- validacion de division para cero 
				begin
				   --print 'entra aqui 3.algo'
				   set @aux_montodes = cast((@CEMajustado/@aux_tasdesctoreal) as DECIMAL(20,12))
				end 
				
			--- cuota maxima de simulacion en sistema frances del monto maximo elegido por tasa de descuento 
		    --- se envia el monto de descuento al simulador para que retorne la cuota maxima de tasa de decuento
			    exec sp_simulador_pago @aux_montodes,@c_plazo,@aux_nrocuovar,@c_amocad,@c_tasfij ,@c_tasvar,@c_tasvarpar,@tasegdes,@tasegces,1, @cuomaxtasdes output 
			    
            --- calculo del monto maximo real 
                if @cuomaxtasdes > 0 -- validacion de division para cero 
			    begin -- MUFC
					DECLARE @sobranteAnt DECIMAL(20,12)
					
					select @sobranteAnt = isnull(efk_cuota_sobrante,0)
    				from BMSC_MSCRM..efk_productosimulado 
    				where   efk_producto_simuladoId = @c_simulacionId and efk_orden = (@c_orden-1) 
    				
					declare @nvoCEMcons0 DECIMAL(20,12)
					declare @nvoCEMviv0  DECIMAL(20,12)
					
					exec  sp_calcula_cem_actual @c_simulacionId,@c_orden,@aux_CEMajustadoprod,@aux_CEMajustadoviv,@nvoCEMcons0 output,@nvoCEMviv0 output
					
					DECLARE @CuotaMaxVars0 TABLE ( VarNumber0 DECIMAL(20,12))    				
    				INSERT INTO @CuotaMaxVars0 ([VarNumber0]) VALUES(@nvoCEMviv0)
    				INSERT INTO @CuotaMaxVars0 ([VarNumber0]) VALUES(@sobranteAnt)
    				
    				SET @aux_CEMajustadoviv  = @nvoCEMviv0
    		        SET @aux_CEMajustadoprod = @nvoCEMcons0
    		            				    				
    				DECLARE @montoMax0 DECIMAL(20,12)
    				
    				if @c_productohom = '1' -- si es vivienda 
    		         begin	 
    		           --print 'entra aqui 4'
    		           SELECT @montoMax0 = MAX(NumTable.[VarNumber0]) FROM @CuotaMaxVars0 AS NumTable --CUOTA MAXIMA 
    		         END
    		        else
    		          begin
    		            if @c_orden = 1
    		              begin
    		                 SELECT @montoMax0 = @aux_CEMajustadoviv
    		              end
    		              
    		            INSERT INTO @CuotaMaxVars0 ([VarNumber0]) VALUES(@nvoCEMcons0)
    		            --SELECT * FROM @CuotaMaxVars0
    				    SELECT @montoMax0 = MIN(NumTable.[VarNumber0]) FROM @CuotaMaxVars0 AS NumTable --CUOTA MAXIMA
    				  end			
    				
			        --set @aux_monmaxreal =  (@CEMajustado * @aux_montodes)/@cuomaxtasdes
			        set @aux_monmaxreal =  (@montoMax0 * @aux_montodes)/@cuomaxtasdes
			        
			        if(@aux_monmaxreal < 200)
						begin
							set @aux_monmaxreal = 0
						end			        
			    end
			--- cuota maxima solicitada 
			--- si el usuario ingresa el monto solicitado se ejecuta el simulador de pago
			--- retornando la cuota maxima solicitada 
		        if (@c_monsol>0)
	  			begin
				  exec  sp_simulador_pago @c_monsol,@c_plazo,@aux_nrocuovar,@c_amocad,@c_tasfij ,@c_tasvar,@c_tasvarpar,@tasegdes,@tasegces,1, @cuomaxsol output
				end
			--- la cuota sobrante es el cem ajustado de vivienda - la cuota maxima solicitada 	
			--- cuota sobrante    
				if @c_orden = 1 and @c_productohom = '2'
    				begin--SAF
    				    --print 'entra aqui 5'
    				    
    					SET @aux_cuosob = @aux_CEMajustadoviv - @cuomaxsol
    					
    		        end
    		    ELSE
    		       begin
    		        --print 'entra aqui 6'
					SET @aux_cuosob = @montoMax0 - @cuomaxsol
				   end				   
		end  
       ---- FIN si el orden es >=1 y no es una tarjeta de credito 
       ---- INICIO si el orden es >=2 y es una tarjeta de credito 
       else -- SI ORDEN ES MAYOR A 1 Y ES TARJETA
         begin    		
               --print 'entra aqui 7'	
               declare @aux_ingcast DECIMAL(20,12) -- auxiliar para obtener el ingreso 
               if @aux_prodanterior = '1' -- si el producto anterior fue vivienda 
    		     begin
    		     --print 'entra aqui 7.1'	
    		      --- obtengo solo la cuota sobrante del producto anterior 
    		        
    				select @aux_ingcast = isnull(efk_cuota_sobrante,0) --isnull(efk_cuota_sobrante,0)---,isnull(efk_cuota_maxima_solicitada,0))
    				from BMSC_MSCRM..efk_productosimulado 
    				where   efk_producto_simuladoId = @c_simulacionId and efk_orden = (@c_orden-1) 
    				
    				
					declare @nvoCEMcons DECIMAL(20,12)
					declare @nvoCEMviv  DECIMAL(20,12)
					/*set @nvoCEMcons = @aux_CEMajustadoprod - @sum_cuotas_anteriores_cons
					set @nvoCEMviv  = @aux_CEMajustadoviv  - @sum_cuotas_anteriores_viv*/
										
					exec  sp_calcula_cem_actual @c_simulacionId,@c_orden,@aux_CEMajustadoprod,@aux_CEMajustadoviv,@nvoCEMcons output,@nvoCEMviv output
					
					DECLARE @CuotaMaxVars TABLE ( VarNumber DECIMAL(20,12))
    				INSERT INTO @CuotaMaxVars ([VarNumber]) VALUES(@nvoCEMcons)
    				INSERT INTO @CuotaMaxVars ([VarNumber]) VALUES(@nvoCEMviv)
    				INSERT INTO @CuotaMaxVars ([VarNumber]) VALUES(@aux_ingcast)
					
					--MIN(@aux_CEMajustadoprod, efk_cuota_sobrante)
					
    				select @aux_ingcast = MIN(NumTable.[VarNumber]) FROM @CuotaMaxVars AS NumTable--CUOTA MAXIMA
    				
    				SET @aux_CEMajustadoviv  = @nvoCEMviv
    		        SET @aux_CEMajustadoprod = @nvoCEMcons
    				--END MUFC
    				
    		     end
    		   else
    		      begin
    		      --print 'entra aqui 8'
    		      -- si el producto anterior es diferente de vivienda, utilizo el cem ajustado del producto  
    		        declare @nvoCEMconsTarj DECIMAL(20,12)
					declare @nvoCEMvivTarj  DECIMAL(20,12)
					DECLARE @sobranteAntTarj DECIMAL(20,12)
					select @sobranteAntTarj = isnull(efk_cuota_sobrante,0)
    				from BMSC_MSCRM..efk_productosimulado 
    				where   efk_producto_simuladoId = @c_simulacionId and efk_orden = (@c_orden-1) 
					
					exec  sp_calcula_cem_actual @c_simulacionId,@c_orden,@aux_CEMajustadoprod,@aux_CEMajustadoviv,@nvoCEMconsTarj output,@nvoCEMvivTarj output
					
					DECLARE @CuotaMaxVarsTarj TABLE ( VarNumber0 DECIMAL(20,12))    				
    				INSERT INTO @CuotaMaxVarsTarj ([VarNumber0]) VALUES(@nvoCEMvivTarj)
    				INSERT INTO @CuotaMaxVarsTarj ([VarNumber0]) VALUES(@nvoCEMconsTarj)
    				INSERT INTO @CuotaMaxVarsTarj ([VarNumber0]) VALUES(@sobranteAntTarj)
					
    		        --set  @aux_ingcast = isnull(@aux_CEMajustadoprod,0)
    		        SELECT @aux_ingcast = MIN(NumTable.[VarNumber0]) FROM @CuotaMaxVarsTarj AS NumTable --CUOTA MAXIMA
    		        
    		        SET @aux_CEMajustadoviv  = @nvoCEMvivTarj
    		        SET @aux_CEMajustadoprod = @nvoCEMconsTarj
    			  end
               
               --print 'entra aqui 8.1'	    			                 
               -- monto de linea segun rango de ingresos
    			select @aux_monlinrgoing =  @c_ingotrpro  * dbo.fn_sp_rangoing(@c_ingotrpro) 
    			
    		   -- obtengo el porcentaje de relacion, en base a la funcion fn_sp_porcrel 
    			select @aux_rel = dbo.fn_sp_porcrelac(@c_productohom,@c_productohom)/100 
    			
        	   --- monto de linea segun porcentaje de relacion deuda ingreso 	
    			if @c_orden = 1
    			  select @aux_monlinporcreldeuing =  (@aux_ingcast * @aux_rel)/@taspagmin
    			else
    			  select @aux_monlinporcreldeuing =  (@aux_ingcast)/@taspagmin
		  
    			-- monto de linea final
    			-- obtengo el menor valor entre el monto de linea segun rango de ingresos y 
    			-- monto de linea segun porcentaje de relacion deuda ingreso 
    			  select @aux_monlinfin = dbo.fn_sp_menorvalor(@aux_monlinrgoing,@aux_monlinporcreldeuing)
    			  
            	--- minimo de la tarjeta de credito que sirve como cuota mensual
            	--- monto de linea final * el porcentaje de pago minimo (0.07) declarado en las variables 		
    			set @aux_mintarj = 	@aux_monlinfin * @taspagmin
    			
    			if (@c_monsol>0) -- si el monto solicitado es mayor a cero   
			    begin
    			--- minimo de linea solicitada
    				set @aux_minlinsol = @c_monsol *  @taspagmin
        			--- esto se lo considera como cuota maxima solicitada y se lo almacena en la tabla
    				set @cuomaxsol =  @aux_minlinsol
    			end
    			
    			 --print 'entra aqui 9'	
				--- cuota sobrante
				--- cem ajsutado de producto - minimo de linea solicitada 
				set @aux_cuosob =  isnull(@aux_ingcast,0) - isnull(@aux_minlinsol,0) -- CUOTA MAXIMA - MIN.DE LINEA SOLIC.

			end
	  ---- FIN si el orden es >=2 y es una tarjeta de credito 
     end
     -- para este caso como necesito la cuota maxima solicita que se extrae de la simualcion.
     -- la cuota sobrante si el orden es = 1 y es tarjeta de credito 
     -- es el cem ajustado - la cuota maxima solicitada 
     if @c_orden = 1 and @c_productohom = '3'  --- lo coloco al final por q necesito el valor de la simulacion
	 begin
	 --print 'entra aqui 10'
	  --select @aux_cuosob = @aux_CEMajustadoprod - @cuomaxsol 
	  SET @aux_cuosob = @aux_CEMajustadoviv - @cuomaxsol
	 end
--- valicacion previa al update la cuota sobrante no puede ser menor a 26.09
 if @aux_cuosob < @cuominsob
   begin
    set @aux_cuosob = @cuominsob
   end 

 set @sqlserver = 'UPDATE BMSC_MSCRM..efk_productosimulado set '
 set @sqlserver = @sqlserver + '   efk_cem_vivienda = ''' + cast(isnull(@aux_CEMviv,0) as varchar(max))  +  '''' 
 if @c_orden = 1  --- en orden 1 el cem ajustado de producto y vivienda se almacena en estos campos 
begin
 set @sqlserver = @sqlserver + '  ,efk_cem_ajustado_vivienda =  '''  + cast(isnull(@aux_CEMajustadoviv,0) as varchar(max)) +  ''''
 set @sqlserver = @sqlserver + '  ,efk_cem_ajustado_consumo = '''  + cast(isnull(@aux_CEMajustadoprod,0)  as varchar(max))+  ''''  
end
if @c_orden >= 2  -- si el orden es mayor = 2 el cem ajsutado de vivienda y producto se almacena en estos campos 
begin
 set @sqlserver = @sqlserver + '  ,efk_cem_ajustado_vivienda2 = '''  + cast(@aux_CEMajustadoviv as varchar(max))  +  ''''
 set @sqlserver = @sqlserver + '  ,efk_cem_ajustado_consumo2 =  '''  + cast(@aux_CEMajustadoprod as varchar(max))  +  ''''
end
 set @sqlserver = @sqlserver + '  ,efk_cem_consumo = '''  + cast(isnull(@aux_CEMprod,0) as varchar(max)) +  '''' 
 set @sqlserver = @sqlserver + '  ,efk_ingreso_vivienda = '''  + cast(isnull(@c_ingviv,0) as varchar(max))  +  '''' 
 set @sqlserver = @sqlserver + '  ,efk_ingreso_otros_productos = ''' + cast(isnull(@c_ingotrpro,0)  as varchar(max))  +  ''''
 set @sqlserver = @sqlserver + '  ,efk_amortizacion_cada = '''  + cast(isnull(@c_amocad,0) as varchar(max))  +  ''''
if @c_productohom <> 3 -- si el producto homologado es distinto de tarjeta de credito se almacenan estos campos 
begin
 set @sqlserver = @sqlserver + '  ,efk_monto_maximo =  '''  + cast(isnull(@aux_monmaxreal,0) as varchar(max))  +  ''''
 set @sqlserver = @sqlserver + '  ,efk_monto_maximo_credito_tasadescuento= '''  + cast(isnull(@aux_montodes,0) as varchar(max))  + '''' 
 set @sqlserver = @sqlserver + '  ,efk_monto_solicitado = '''  +  cast(isnull(@c_monsol,0) as varchar(max))  +  ''''
 set @sqlserver = @sqlserver + '  ,efk_cuota_maxima_solicitada = '''  +  cast(isnull(@cuomaxsol,0) as varchar(max))  + '''' 
 set @sqlserver = @sqlserver + '  ,efk_cuota_maxima_simulacion = '''  + cast(isnull(@montoMax0,0) as varchar(max))  +  ''''
 set @sqlserver = @sqlserver + '  ,efk_cuota_sobrante= '''  + cast(isnull(@aux_cuosob,0) as varchar(max))  +  '''' 
 set @sqlserver = @sqlserver + '  ,efk_numero_cuotas= ''' + cast(isnull(@c_plazo,0) as varchar(max))  +  ''''
end   
if @c_productohom = 3 -- si el producto homologado es tarjeta de credito de elmacena los montos de lineamiento 
begin
  set @sqlserver = @sqlserver + '  ,efk_monto_maximo =  '''  + cast(isnull(@aux_monlinfin,0) as varchar(max))  +  ''''  
  set @sqlserver = @sqlserver + '  ,efk_cuota_maxima_simulacion = '''  +  cast(isnull(@aux_mintarj,0) as varchar(max))  + '''' 
  set @sqlserver = @sqlserver + '  ,efk_monto_solicitado = '''  +  cast(isnull(@c_monsol,0) as varchar(max))  +  ''''
  set @sqlserver = @sqlserver + '  ,efk_cuota_maxima_solicitada = '''  +  cast(isnull(@cuomaxsol,0) as varchar(max))  + '''' 
  set @sqlserver = @sqlserver + '  ,efk_cuota_sobrante= '''  + cast(isnull(@aux_cuosob,0) as varchar(max))  +  '''' 
end
 set @sqlserver = @sqlserver + '  ,efk_producto =  '''  + cast(@c_productohom as varchar(max))  +  ''''
 set @sqlserver = @sqlserver + '  where cast(efk_productosimuladoId as varchar(max)) =  '''  + cast(@c_productosimuladoId   as varchar(max))  +  ''''
 set @sqlserver = @sqlserver + '  and  cast(efk_producto_simuladoId as varchar(max))=  '''  + cast(@c_simulacionId as varchar(max)) +  ''''
 set @sqlserver = @sqlserver + '  and  efk_orden =  '  + cast(@c_orden as varchar(max))
 exec (@sqlserver) --- ejecuta el query dimanico 
  			
     if @c_llamada = 0 --- si se llama del formulario 
	 begin
	    --print 'entra aqui 10.1'
		select isnull(efk_monto_maximo,0) MontoMaximo from BMSC_MSCRM..efk_productosimulado
		where efk_productosimuladoId = @c_productosimuladoId 
		and efk_producto_simuladoId = @c_simulacionId 
		and efk_orden = @c_orden
		
	-- validaciones para presentar como error	
		if (@c_ingviv = 0 and @c_ingotrpro=0)
	    begin
			set @errorprint = @errorprint + ' El usuario no registra ingresos '
		end
	    if 	@aux_CEMajustadoviv < 0
       	begin
       		set @errorprint = @errorprint + ' El Resultado de las Deudas Financieras es mayor al CEM de Vivienda'
       	end
       	if 	@aux_CEMajustadoprod < 0
       	begin
       		set @errorprint = @errorprint + ' El Resultado de las Deudas Financieras es mayor al CEM de Productos'
       	end
		select @errorprint ImprimeError
	 end 	
	 
 	 if @c_llamada = 1  --si se ejecuta desde el reporte 
		begin		
	     declare @result DECIMAL(20,12)
	     exec sp_simulador_pago @c_monsol,@c_plazo,@aux_nrocuovar,@c_amocad,@c_tasfij ,@c_tasvar,@c_tasvarpar,@tasegdes,@tasegces,0, @result output 
		end
	
end
GO
