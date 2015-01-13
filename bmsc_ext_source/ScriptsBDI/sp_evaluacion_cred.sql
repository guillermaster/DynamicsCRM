/****** Object:  StoredProcedure [dbo].[sp_evaluacion_cred]    Script Date: 08/27/2013 11:35:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_evaluacion_cred] (
											@c_productosimuladoId varchar(max), -- codigo del prodcuto simulado
											@c_OpportunityId varchar(max), -- codigo de oportunidad
											@c_SimulacionId varchar(max), -- codigo de simulacion
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
											@c_segdesgrav int ,--- SI=1, NO = 0 se calcula el seguro de desgravamen 
									        @c_opportunityname varchar(max) -- nombre de la oportunidad 
											 ) as
begin
declare @sqlserver varchar(max) 
declare @nrosolicitud int
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
declare @aux_minlinsol decimal (18,4)    	 -- minimo de linea solicitada		
declare @aux_prodanterior int -- producto anterior
-------------------- CARGA DE PARAMETROS --------------------------------- 
select @tasegdes  = ISNULL(dbo.fn_sp_parametro_mony('tasa_desgravamen'),0.78) 
select @tasegces  = ISNULL(dbo.fn_sp_parametro_mony('tasa_cesantia'),0.68)
select @taspagmin = ISNULL(dbo.fn_sp_parametro_mony('tasa_pag_min'),0.07)
select @cuominsob = ISNULL(dbo.fn_sp_parametro_mony('cuota_min_sobrante'),26.09)
------------------------------------------------------------------------------------
declare @aux_cuotabmsc_entramite DECIMAL(20,12)=0  -- cuotas bmsc en tramite

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


---- obtengo el numero de oferta -- 
declare @c_numeroferta int -- numero oferta
select @c_numeroferta = efk_numero_oferta,  @nrosolicitud = efk_nrosolicitud  
from BMSC_MSCRM..Opportunity where Opportunityid = @c_OpportunityId 



-- tipos de productos homologados 
-- 1 vivienda - 2 consumo - 3 tarjeta de credito 
         
        -- obtengo el producto homologado de la tabla de prudctos enviandole los 
        -- prametros de codigo de producto, tipo y familia
        select  @c_productohom = dbo.fn_sp_prodhom(@c_ProductId,@c_ProductoTipo,@c_ProductoFamilia)
        
        -- como no todos los productos tienen este campo lleno, se esta utilizando
        -- como default el de vivienda 
        set @c_productohom = ISNULL(@c_productohom,1) 
        
        --- UPDATE 
        --- _cuotabmsc_entramite_bmsc
			SELECT @aux_cuotabmsc_entramite = SUM(ISNULL(efk_cuota_maxima_solicitada,0)) from BMSC_MSCRM..OpportunityExtensionBase eb
			inner join BMSC_MSCRM..OpportunityBase b on b.OpportunityId = eb.OpportunityId
			WHERE b.CustomerId = @c_cliente
			and eb.efk_numero_oferta <> @c_numeroferta
			and b.StateCode=0
			Update BMSC_MSCRM..AccountExtensionBase
			set efk_cuotas_BMSC_tramite = isnull(@aux_cuotabmsc_entramite,0) 
			where AccountId = @c_cliente
        --- UPDATE 
        
         
	    exec sp_flujocaja @nrosolicitud ,@c_ingviv output , @c_ingotrpro  output 
	    
	    
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
		-- otros productos es lo mismo que consumo 
	    -- se obtiene el total de deudas del procedure de servicio de deudas  
	    exec sp_servicio_deudas @c_cliente, @aux_cuotaSF output 
	
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
	    
	    
--- desde aqui pueden haber cambios 	    
---- inicio del algoritmo tomando enconsideración el orden de los productos 	    
	  if @c_orden = 1 and @c_productohom = '3'  --- si el primer producto que se ingresa es tarjeta de crédito 
    	   begin
    			-- monto de linea segun rango de ingresos
    			select @aux_monlinrgoing =  @c_ingotrpro  * dbo.fn_sp_rangoing(@c_ingotrpro) 
    			--- auxiliar de relacion 
    			select @aux_rel = dbo.fn_sp_porcrelac(@c_productohom,@c_productohom)/100 
        	    -- monto de linea segun porcentaje de relacion deuda ingreso 
        	    --- esta validacion la dio sergio el dia Enero 17 del 2013 para que el
        	    -- monto maximo le retorne cero 
        	    
    		    select @aux_monlinporcreldeuing =  @aux_CEMajustadoprod/@taspagmin
    		    
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
        	-- solo cuando el orden es mayor a 1 se busca el producto anterior y se lo almacena
    		if @c_orden > 1 
            begin
            	select @aux_prodanterior= pr.efk_productohomologado   
				from BMSC_MSCRM..Opportunity op 
				inner join BMSC_MSCRM..efk_productosimulado ps
				on op.efk_producto_simuladoid  = ps.efk_productosimuladoid
				inner join BMSC_MSCRM..Product pr
				on pr.ProductId = ps.efk_producto_catalogoId
				where op.efk_orden = (@c_orden-1)
				and  op.efk_numero_oferta = @c_numeroferta 
			end --- EN ESTE BLOQUE SACA EL PRODUCTO ANTERIOR 
			
               -- el cem ajustado lo obtengo dependiendo el producto homologado
               -- si es vivienda mi cem ajustado = cem ajsutado de vivienda
               -- si es consumo mi cem ajustado = cem ajsuatdo de consumo 
               --- a partir de aqui tu cem ajustado es igual al cem ajustado del tipo de producto homologado 
               -- q le corresponde 
      		    declare @CEMajustado DECIMAL(20,12)
    			SELECT @CEMajustado = CASE @c_productohom WHEN 1 THEN round(@aux_CEMajustadoviv,2) ELSE ROUND(@aux_CEMajustadoprod,2) END
			    --select @CEMajustado--SAF
      ---- INICIO si el orden es >=1 y no es una tarjeta de credito 
       if @c_productohom <> '3' 
         begin
    		    -- cálculo del endeudamiento máximo referencia -- esto es por producto
    			if (@CEMajustado*@c_plazo) > 0 --- validacion de divisiones para cero 
				begin
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
				   set @aux_montodes = cast((@CEMajustado/@aux_tasdesctoreal) as float)
				end 
			--- cuota maxima de simulacion en sistema frances del monto maximo elegido por tasa de descuento 
		    --- se envia el monto de descuento al simulador para que retorne la cuota maxima de tasa de decuento
			    exec sp_simulador_pago @aux_montodes,@c_plazo,@aux_nrocuovar,@c_amocad,@c_tasfij ,@c_tasvar,@c_tasvarpar,@tasegdes,@tasegces,1, @cuomaxtasdes output 
            --- calculo del monto maximo real 
                if @cuomaxtasdes > 0-- and @c_orden > 1  -- validacion de division para cero 
			    begin
			        --set @aux_monmaxreal =  (@CEMajustado * @aux_montodes)/@cuomaxtasdes
			        -- MUFC
			        DECLARE @sobranteAnt DECIMAL(20,12)
			        
			        
    				select @sobranteAnt = isnull(efk_cuota_sobrante,0) --isnull(efk_cuota_sobrante,0)---,isnull(efk_cuota_maxima_solicitada,0))
    				from BMSC_MSCRM..Opportunity 
    				where    efk_numero_oferta  = @c_numeroferta  and efk_orden = (@c_orden-1) 
    				
					declare @nvoCEMcons0 DECIMAL(20,12)
					declare @nvoCEMviv0  DECIMAL(20,12)
					
					exec  sp_calcula_cem_actual_oportunidad @c_numeroferta,@c_orden,@aux_CEMajustadoprod,@aux_CEMajustadoviv,@nvoCEMcons0 output,@nvoCEMviv0 output
					
					/*set @nvoCEMcons0 = @aux_CEMajustadoprod - @sum_cuotas_anteriores_cons0
					set @nvoCEMviv0  = @aux_CEMajustadoviv  - @sum_cuotas_anteriores_viv0*/
					
					DECLARE @CuotaMaxVars0 TABLE ( VarNumber0 DECIMAL(20,12))    				
    				INSERT INTO @CuotaMaxVars0 ([VarNumber0]) VALUES(@nvoCEMviv0)
    				INSERT INTO @CuotaMaxVars0 ([VarNumber0]) VALUES(@sobranteAnt)
    				
    				SET @aux_CEMajustadoviv  = @nvoCEMviv0
    		        SET @aux_CEMajustadoprod = @nvoCEMcons0
    				    				
    				DECLARE @montoMax0 DECIMAL(20,12)
    				
    				if @c_productohom = '1' -- si es vivienda 
    		         begin	 
    		           SELECT @montoMax0 = MAX(NumTable.[VarNumber0]) FROM @CuotaMaxVars0 AS NumTable --CUOTA MAXIMA 
    		         END
    		        else
    		          begin
    		            INSERT INTO @CuotaMaxVars0 ([VarNumber0]) VALUES(@nvoCEMcons0)
    				    SELECT @montoMax0 = MIN(NumTable.[VarNumber0]) FROM @CuotaMaxVars0 AS NumTable --CUOTA MAXIMA
    				  end
    				
    				set @CEMajustado = @montoMax0
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
				  --SELECT @cuomaxsol AS '@cuomaxsol'
				  if @c_orden = 1 and @c_productohom = '2'
    				begin--SAF
    					SET @aux_cuosob = @aux_CEMajustadoviv - @cuomaxsol
    		        end
    			   ELSE
    		         begin
					   SET @aux_cuosob = @montoMax0 - @cuomaxsol --MUFC 				
					 end
				end
			--- la cuota sobrante es el cem ajustado de vivienda - la cuota maxima solicitada 	
			--- cuota sobrante  
			    
		end  
       ---- FIN si el orden es >=1 y no es una tarjeta de credito 
       ---- INICIO si el orden es >=2 y es una tarjeta de credito 
       else --- ES UNA TARJETA DE CREDITO
         begin    			
               declare @aux_ingcast DECIMAL(20,12) -- auxiliar para obtener el ingreso 
               if @aux_prodanterior = '1' -- si el producto anterior fue vivienda 
    		     begin
    		      --- obtengo solo la cuota sobrante del producto anterior 
    				select @aux_ingcast = isnull(efk_cuota_sobrante,0) --isnull(efk_cuota_sobrante,0)---,isnull(efk_cuota_maxima_solicitada,0))
    				from BMSC_MSCRM..Opportunity 
    				where    efk_numero_oferta  = @c_numeroferta  and efk_orden = (@c_orden-1) 
    				
    				--BEGIN MUFC
    				declare @nvoCEMcons DECIMAL(20,12)
					declare @nvoCEMviv  DECIMAL(20,12)
															
					exec  sp_calcula_cem_actual_oportunidad @c_numeroferta,@c_orden,@aux_CEMajustadoprod,@aux_CEMajustadoviv,@nvoCEMcons output,@nvoCEMviv output
					
					DECLARE @CuotaMaxVars TABLE ( VarNumber DECIMAL(20,12))
    				INSERT INTO @CuotaMaxVars ([VarNumber]) VALUES(@nvoCEMcons)
    				INSERT INTO @CuotaMaxVars ([VarNumber]) VALUES(@nvoCEMviv)
    				INSERT INTO @CuotaMaxVars ([VarNumber]) VALUES(@aux_ingcast)
					
					--MIN(@aux_CEMajustadoprod, efk_cuota_sobrante)
					
    				select @aux_ingcast = MIN(NumTable.[VarNumber]) FROM @CuotaMaxVars AS NumTable--CUOTA MAXIMA
    				--select @aux_ingcast as '@aux_ingcast'
    				SET @aux_CEMajustadoviv  = @nvoCEMviv
    		        SET @aux_CEMajustadoprod = @nvoCEMcons
    		        --END MUFC
    		     end
    		   else
    		      begin
    		      -- si el producto anterior es diferente de vivienda, utilizo el cem ajustado del producto  
    		        --set  @aux_ingcast = isnull(@aux_CEMajustadoprod,0)
                	declare @nvoCEMconsTarj DECIMAL(20,12)
					declare @nvoCEMvivTarj  DECIMAL(20,12)
					DECLARE @sobranteAntTarj DECIMAL(20,12)
					select @sobranteAntTarj = isnull(efk_cuota_sobrante,0) --isnull(efk_cuota_sobrante,0)---,isnull(efk_cuota_maxima_solicitada,0))
    				from BMSC_MSCRM..Opportunity 
    				where    efk_numero_oferta  = @c_numeroferta  and efk_orden = (@c_orden-1) 
					
					exec  sp_calcula_cem_actual_oportunidad @c_numeroferta,@c_orden,@aux_CEMajustadoprod,@aux_CEMajustadoviv,@nvoCEMconsTarj output,@nvoCEMvivTarj output
					
					DECLARE @CuotaMaxVarsTarj TABLE ( VarNumber0 DECIMAL(20,12))    				
    				INSERT INTO @CuotaMaxVarsTarj ([VarNumber0]) VALUES(@nvoCEMvivTarj)
    				INSERT INTO @CuotaMaxVarsTarj ([VarNumber0]) VALUES(@nvoCEMconsTarj)
    				INSERT INTO @CuotaMaxVarsTarj ([VarNumber0]) VALUES(@sobranteAntTarj)
					
    		        --set  @aux_ingcast = isnull(@aux_CEMajustadoprod,0)
    		        SELECT @aux_ingcast = MIN(NumTable.[VarNumber0]) FROM @CuotaMaxVarsTarj AS NumTable --CUOTA MAXIMA
    		        
    		        SET @aux_CEMajustadoviv  = @nvoCEMvivTarj
    		        SET @aux_CEMajustadoprod = @nvoCEMconsTarj
    			  end
    			                 
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
				--- cuota sobrante
				--- cem ajsutado de producto - minimo de linea solicitada 
				--set @aux_cuosob =  isnull(@aux_CEMajustadoprod,0) - isnull(@aux_minlinsol,0) --MUFC
				set @aux_cuosob =  isnull(@aux_ingcast,0) - isnull(@aux_minlinsol,0) --MUFC
			end
	  ---- FIN si el orden es >=2 y es una tarjeta de credito 
     end
     -- para este caso como necesito la cuota maxima solicita que se extrae de la simualcion.
     -- la cuota sobrante si el orden es = 1 y es tarjeta de credito 
     -- es el cem ajustado - la cuota maxima solicitada 
     if @c_orden = 1 and @c_productohom = '3'  --- lo coloco al final por q necesito el valor de la simulacion
	 begin
	  --select @aux_cuosob = @aux_CEMajustadoprod - @cuomaxsol 
	  SET @aux_cuosob = @aux_CEMajustadoviv - @cuomaxsol
	 end
--- valicacion previa al update la cuota sobrante no puede ser menor a 26.09
 if @aux_cuosob < @cuominsob
   begin
    set @aux_cuosob = @cuominsob
   end 


set @sqlserver = 'UPDATE BMSC_MSCRM..Opportunity set '
 set @sqlserver = @sqlserver + '   efk_cem_vivienda = ''' + cast(isnull(@aux_CEMviv,0) as varchar(max))  +  '''' 
 set @sqlserver = @sqlserver + '  ,efk_cem_ajustado_vivienda =  '''  + cast(isnull(@aux_CEMajustadoviv,0) as varchar(max)) +  ''''
 set @sqlserver = @sqlserver + '  ,efk_cem_ajustado_consumo = '''  + cast(isnull(@aux_CEMajustadoprod,0)  as varchar(max))+  ''''  
 set @sqlserver = @sqlserver + '  ,efk_cem_consumo = '''  + cast(isnull(@aux_CEMprod,0) as varchar(max)) +  '''' 
 set @sqlserver = @sqlserver + '  ,efk_ingreso_vivienda = '''  + cast(isnull(@c_ingviv,0) as varchar(max))  +  '''' 
 set @sqlserver = @sqlserver + '  ,efk_ingreso_otros_productos = ''' + cast(isnull(@c_ingotrpro,0)  as varchar(max))  +  ''''
if @c_productohom <> 3
begin
 set @sqlserver = @sqlserver + '  ,efk_monto_maximo =  '''  + cast(isnull(@aux_monmaxreal,0) as varchar(max))  +  ''''
 set @sqlserver = @sqlserver + '  ,efk_monto_maximo_credito_tasadescuento= '''  + cast(isnull(@aux_montodes,0) as varchar(max))  + '''' 
 set @sqlserver = @sqlserver + '  ,efk_monto_solicitado = '''  +  cast(isnull(@c_monsol,0) as varchar(max))  +  ''''
 set @sqlserver = @sqlserver + '  ,efk_cuota_maxima_solicitada = '''  +  cast(isnull(@cuomaxsol,0) as varchar(max))  + '''' 
 set @sqlserver = @sqlserver + '  ,efk_cuota_maxima_simulacion = '''  + cast(isnull(@CEMajustado,0) as varchar(max))  +  ''''
 set @sqlserver = @sqlserver + '  ,efk_cuota_sobrante= '''  + cast(isnull(@aux_cuosob,0) as varchar(max))  +  '''' 
 set @sqlserver = @sqlserver + '  ,efk_numero_cuotas= ''' + cast(isnull(@c_plazo,0) as varchar(max))  +  ''''
end   
if @c_productohom = 3
begin
  set @sqlserver = @sqlserver + '  ,efk_monto_maximo =  '''  + cast(isnull(@aux_monlinfin,0) as varchar(max))  +  ''''  
  set @sqlserver = @sqlserver + '  ,efk_cuota_maxima_simulacion = '''  +  cast(isnull(@aux_mintarj,0) as varchar(max))  + '''' 
  set @sqlserver = @sqlserver + '  ,efk_monto_solicitado = '''  +  cast(isnull(@c_monsol,0) as varchar(max))  +  ''''
  set @sqlserver = @sqlserver + '  ,efk_cuota_maxima_solicitada = '''  +  cast(isnull(@cuomaxsol,0) as varchar(max))  + '''' 
  set @sqlserver = @sqlserver + '  ,efk_cuota_sobrante= '''  + cast(isnull(@aux_cuosob,0) as varchar(max))  +  '''' 
end
 set @sqlserver = @sqlserver + '  ,efk_orden =  '''  + cast(@c_orden as varchar(max))  +  ''''
 set @sqlserver = @sqlserver + '  where  OpportunityId = ''' +  cast(@c_OpportunityId as varchar(max))  +  ''''
 exec (@sqlserver) --- ejecuta el query dimanico 
  			
     if @c_llamada = 0 --- si se llama del formulario 
	 begin
		select isnull(efk_monto_maximo,0) MontoMaximo from BMSC_MSCRM..Opportunity 
		where OpportunityId = @c_OpportunityId 
		/* Imprime errores */
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
end;
GO