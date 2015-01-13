/****** Object:  StoredProcedure [dbo].[sp_simulador_pago]    Script Date: 08/27/2013 11:35:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*****************************************************************************
PROCEDIMIENTO: [sp_simulador_pago]
DATE:    ENERO 17/2013
AUTOR:   IYCAZA
PROCEDIMIENTO: Subproceso de Maximo endeudamiento y de la evaluacion crediticia
Si la variable de ingreso @c_bandera = 0 retorna como resultado el detalle que se imprime
en el reporte de plan de cuoptas
Si la variable de ingreso @c_bandera = 1 retorna como resultado el monto de cuota maxima
de la tabla del plan de cuotas
******************************************************************************/
			 
CREATE procedure [dbo].[sp_simulador_pago] 
(@c_mon decimal(18,6), -- 10204.08163
@c_nrocuo int, --60
@c_nrocuovar int, -- 54
@c_amocad int, --30
@c_tasfij decimal(18,6), -- 15
@c_tasvar decimal(18,6), -- 13
@c_tasvarpar decimal(18,6), -- 7
@c_tasdes decimal(18,6), ---0.78
@c_tasces decimal(18,6), ---0.68
@c_bandera int,
@resultado decimal(18,6) OUTPUT) --- si es 0 presenta como resultado el detalle, si es 1 presenta como resultado la cuota maxima   
as

declare @c_intfij decimal(18,6)
declare @c_intauxfij decimal(18,6)
declare @c_intvar decimal(18,6)
declare @c_intauxvar decimal(18,6)
declare @c_desgra decimal(18,6)
declare @c_cesant decimal(18,6)
declare @i int

--- VARIABLES CALCULADAS ANTES DE INGRESAR AL TABLE VARIABLE 
declare @aux_salcap decimal(18,6) 
declare @aux_valcuo decimal(18,6) 
declare @aux_valint decimal(18,6)
declare @aux_taspro decimal(18,6)
declare @aux_fecha datetime
set nocount on
declare @tabplancuotas table
(
c1_nrocuo int,
c1_fec datetime,
c1_salcap decimal(18,6) not null,
c1_amocap decimal(18,6)  not  null,
c1_valcuo decimal(18,6) not  null,
c1_valint decimal(18,6) not  null,
c1_segdes decimal(18,6) not  null,
c1_segces decimal(18,6) not  null,
c1_taspro decimal(18,6) not  null 
)


-- valido nulos
set @c_mon		= ISNULL(@c_mon,0)  -- monto
set @c_nrocuo	= ISNULL(@c_nrocuo,0) -- numero de cuotas
set @c_nrocuovar  = ISNULL(@c_nrocuovar,0) -- numero de cuota variable
set @c_amocad	  = ISNULL(@c_amocad,0)--- amortizacion cada
set @c_tasfij	  = ISNULL(@c_tasfij,0) -- tasa fija
set @c_tasvar	  = ISNULL(@c_tasvar,0) -- tasa variable
set @c_tasvarpar  = ISNULL(@c_tasvarpar,0) -- tasa variable a partir de 
set @c_tasdes	  = ISNULL(@c_tasdes,0) --- tasa desgravamen
set @c_tasces     = ISNULL(@c_tasces,0)--- tasa cesantia 

set @i = 1 -- indice inicio de 1 
set @aux_fecha = GETDATE()
begin
-- CALCULAR INTERES FIJO 
set @c_intfij=(@c_tasfij*@c_amocad)/(360*100)
set @c_intauxfij = power((1+@c_intfij),@c_nrocuo)
-- CALCULAR INTERES VARIABLE
set @c_intvar = (@c_tasvar*@c_amocad)/(360*100)
set @c_intauxvar = power((1+@c_intvar),@c_nrocuovar)
-- CALCULAR SEGURO DESGRAVAMEN
set @c_desgra= (@c_tasdes*@c_amocad)/(360*100)
-- CALCULAR SEGURO CESANTIA 
set @c_cesant=@c_tasces/1000
-- ITERAR PLAN CUOTAS
while @i <= @c_nrocuo 
begin
  if @i= 1
           --- obtener el saldo capital 
		   begin
			  set @aux_salcap = @c_mon
		   end
  else 
		   begin
			  select  @aux_salcap =  (isnull(c1_salcap,0)-isnull(c1_amocap,0))					  
				from @tabplancuotas where c1_nrocuo = (@i-1) 
		   end
   --- tasa variable 
   if (@i <  @c_tasvarpar) -- fijo
	   begin
		   if ( (@i=1) and (@c_intauxfij<>1)) 
			begin 
			 set @aux_valcuo = (@c_mon*@c_intfij*@c_intauxfij)/(@c_intauxfij-1) 
			end 
		   else
		    begin
		   	  select @aux_valcuo = c1_valcuo from @tabplancuotas where c1_nrocuo = (@i-1) 
			end
			 set @aux_valint    = @aux_salcap * @c_intfij
	   end
   else
       begin
           if (((@i-1) <  @c_tasvarpar) and (@c_intauxvar<>1))
			   begin
				 set @aux_valcuo =  (@aux_salcap*@c_intvar*@c_intauxvar)/(@c_intauxvar-1)
			   end 
		   else
			   begin
		   		  select @aux_valcuo = c1_valcuo from @tabplancuotas where c1_nrocuo = (@i-1) 
			   end
				
            set @aux_valint    = @aux_salcap * @c_intvar
            
       end
   if (@i = @c_nrocuo)
   begin
      set @aux_valint = @aux_valcuo - @aux_salcap
   end   
   
   --- calcula tasa promedio
 if  @aux_salcap = 0 
   begin
     set @aux_taspro = 0
   end 
 else
   begin
    if @aux_salcap > 0 
      set @aux_taspro =   @aux_valint/@aux_salcap
    end 
   
  if isnull(@aux_salcap,0) >  0 
	  begin
		insert into @tabplancuotas 
		(c1_nrocuo,c1_fec,c1_salcap,c1_amocap,c1_valcuo,c1_valint,c1_segdes,c1_segces,c1_taspro) values
		(@i, cast(@aux_fecha as varchar(11)) ,@aux_salcap,(isnull(@aux_valcuo,0)-isnull(@aux_valint,0)) , isnull(@aux_valcuo,0), @aux_valint ,(@aux_salcap*@c_desgra),(@aux_salcap*@c_cesant),@aux_taspro )
	  end
  else
    --- iterar a cero 
	  begin
		insert into @tabplancuotas 
		(c1_nrocuo,c1_fec,c1_salcap,c1_amocap,c1_valcuo,c1_valint,c1_segdes,c1_segces,c1_taspro) values
		(@i, null ,0,0,0,0,0,0,0)
	  end
  
	  set @aux_fecha = dateadd(month,1,@aux_fecha)
	  set @i = @i + 1
	  
  
end



if @c_bandera = 0  --- si se requiere el detalle del plan de pagos (reporte) 

	begin
	
		SELECT 
		isnull(c1_nrocuo,0) NroCuota,
		isnull(c1_fec,0) FechaActual,
		isnull(c1_salcap,0) SaldoCapital,
		isnull(c1_amocap,0) AmortizacionCapital,
		isnull(c1_valcuo,0) ValorCuota,
		isnull(c1_valint,0) ValorInteres,
		isnull(c1_segdes,0) SeguroDesgravamen,
		isnull(c1_segces,0) SeguroCesantia,
		isnull(c1_taspro,0) TasaPromedio,
		(isnull(c1_valcuo,0)+  isnull(c1_segdes,0) +   isnull(c1_segces,0))      ValCuotaTotal 
		 FROM @tabplancuotas
		 order by 1 ASC 
	end

if @c_bandera = 1  --- si se requiere retornar la cuota maxima 
	begin 
		select @resultado =  max ((c1_valcuo+  c1_segdes +   c1_segces)) from @tabplancuotas 
	end
end;
GO