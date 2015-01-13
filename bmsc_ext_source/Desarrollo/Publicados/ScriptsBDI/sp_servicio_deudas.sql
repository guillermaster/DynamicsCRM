/****** Object:  StoredProcedure [dbo].[sp_servicio_deudas]    Script Date: 08/27/2013 11:35:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[sp_servicio_deudas] (@c_cliente varchar(max), @c_serviciodeuda money  output ) as
begin

declare @c_SaldoCartera money =0
set nocount on

--- declaro mi variable table 
declare @tabproceso table
  		(
			c_tipo int,
			c_descripcion varchar(100),
			c_porcentaje decimal(3,2), 
		    c_regla1 money,
			c_regla2 money
		)

---- cargo los datos a la tabla 
declare @H0 money = 0		
declare @NON1 money = 0
declare @N0N1TC money = 0
declare @C0P money = 0
declare @M0M1 money = 0
SELECT @H0     = efk_valor_decimal FROM BMSC_MSCRM..efk_paramtero_simulacion_crediticia WHERE efk_name='H0'
SELECT @NON1   = efk_valor_decimal FROM BMSC_MSCRM..efk_paramtero_simulacion_crediticia WHERE efk_name='N0_N1'
SELECT @N0N1TC = efk_valor_decimal FROM BMSC_MSCRM..efk_paramtero_simulacion_crediticia WHERE efk_name='N0_N1(TDC)'
SELECT @C0P    = efk_valor_decimal FROM BMSC_MSCRM..efk_paramtero_simulacion_crediticia WHERE efk_name='C0_P'
SELECT @M0M1   = efk_valor_decimal FROM BMSC_MSCRM..efk_paramtero_simulacion_crediticia WHERE efk_name='M0_M1'
insert into @tabproceso values
(1,'HO',@H0,0,0)
insert into @tabproceso values
(2,'No o N1',@NON1,0,0)
insert into @tabproceso values
(3,'No o N1 (TDC)',@N0N1TC,0,0)
insert into @tabproceso values
(4,'Co o P',@C0P,0, 0)
insert into @tabproceso values
(5,'Mo o M1',@M0M1,0,0)


declare @ctipo int
declare @cdescripcion varchar(100) 
declare @cporcentaje decimal(3,2)=0
declare @cregla1 money =0
declare @cregla2 money =0
declare @cuotarespaldada money =0
declare @cuotaDDJJ  money =0 
declare @actualizar money =0
declare @saldocartera money = 0


declare c1 cursor for
    select c_tipo, c_descripcion, c_porcentaje , c_regla1 ,  c_regla2  from  @tabproceso;
open c1
	fetch next from c1
	into @ctipo,@cdescripcion, @cporcentaje, @cregla1,  @cregla2
     
    while @@FETCH_STATUS = 0
    begin
         
    		select 
    		@saldocartera =
    		case @ctipo 
    		when 1 then isnull(efk_hipotecario_cuotas_cartera ,0)
    		when 2 then isnull(efk_consumo_saldo_cartera,0)
    		when 3 then isnull(efk_consumotdc_saldo_cartera,0)
    		when 4 then isnull(efk_comercialPyme_saldo_cartera ,0)
    		when 5 then isnull(efk_microcredito_saldo_cartera,0)
    		end ,
    		@cuotarespaldada =
    		case @ctipo 
    		when 1 then isnull(efk_hipotecario_cuotas_respaldadas,0)
    		when 2 then isnull(efk_consumo_cuotas_respaldadas ,0)
    		when 3 then isnull(efk_consumotdc_cuotas_respaldadas ,0)
    		when 4 then isnull(efk_comercialPyme_cuotas_respaldadas ,0)
    		when 5 then isnull(efk_microcredito_cuotas_respaldadas,0)
    		end ,
    		@cuotaDDJJ = case @ctipo 
    		when 1 then isnull(efk_hipotecario_cuotas_DDJJ ,0)
    		when 2 then isnull(efk_consumo_cuotas_ddjj,0)
    		when 3 then isnull(efk_consumotdc_cuotas_ddjj,0)
    		when 4 then isnull(efk_comercialPyme_cuotas_DDJJ ,0)
    		when 5 then isnull(efk_microcredito_cuotas_ddjj,0)
    		end 
    		from BMSC_MSCRM..Account 
			where AccountId = @c_cliente 
			
			--- actualizo masivamente que regla2 sea igual a cuota respaldada
			--- si el campo de @cuotarespaldada esta lleno toma este valor en regla 2
			-- si no esta lleno  toma el max (regla1 vs cuoa ddjj)
			if @cuotarespaldada >  0 
			begin
		    	update  @tabproceso set c_regla1 = (@saldocartera * @cporcentaje)/100
		 	    ,c_regla2 = @cuotarespaldada
			    where c_tipo = @ctipo
            end
            else
            begin
            declare @aux_regla2 money 
              if ((@saldocartera * @cporcentaje)/100) > @cuotaDDJJ
               begin
            	update  @tabproceso set c_regla1 = (@saldocartera * @cporcentaje)/100
		 	    ,c_regla2 = ((@saldocartera * @cporcentaje)/100)
			    where c_tipo = @ctipo
               end
              else
               begin
               	update  @tabproceso set c_regla1 = (@saldocartera * @cporcentaje)/100
		 	    ,c_regla2 = @cuotaDDJJ
			    where c_tipo = @ctipo
               end   
            end
	     
    fetch next from c1
   	into @ctipo,@cdescripcion, @cporcentaje, @cregla1, @cregla2
    end
    
    close c1;
    deallocate c1;
    
     select @c_serviciodeuda = sum(isnull(ac.efk_cuotas_BMSC,0)) + sum(isnull(ac.efk_cuotas_BMSC_tramite,0)) + sum(isnull(ac.efk_deuda_empresa_empleadora,0))  +  SUM(tp.total)
    from 
    BMSC_MSCRM..Account  ac , 
    (select SUM(isnull(c_regla2,0)) total from @tabproceso) tp
    where ac.AccountId = @c_cliente 
    
    /*select @c_serviciodeuda =  sum(isnull(ac.efk_cuotas_BMSC,0)) + sum(isnull(ac.efk_cuotas_BMSC_tramite,0))  +  SUM(isnull(tp.c_regla2,0))
    from BMSC_MSCRM..Account  ac , @tabproceso tp
    where ac.AccountId = @c_cliente */

end;
GO