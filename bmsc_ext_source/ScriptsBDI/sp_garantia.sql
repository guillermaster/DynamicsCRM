/****** Object:  StoredProcedure [dbo].[sp_garantia]    Script Date: 08/27/2013 11:35:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*****************************************************************************
PROCEDIMIENTO: [sp_garantia]
DATE:    ENERO 17/2013
AUTOR:   IYCAZA
PROCEDIMIENTO: Recibe como parametro de entrada el numero de solicitud de la 
oportunidad, retorna el total de garantias y la cobertura total de garantias 
******************************************************************************/

CREATE procedure [dbo].[sp_garantia] (@c_nrosolicitud int, @c_totgar money output /* total garantia*/, @c_cobtotgar money output /* cobertura total garantias */ ) as
begin
declare @garsol money /* garantia solicitada */
declare @garconant Money /* garantia consolidada anteriores */
declare @cartgarhip money /* cartera bajo garantía hipotecaria */
declare @cartgarper money /* cartera bajo garantía personal */
declare @cartsolfir money /* cartera sola firma */
declare @monsol money /* monto solicitado */
   
	select  @garconant = SUM(isnull(efk_valor_liquidable_avaluo,0)) 
		from BMSC_MSCRM..efk_garantia_credito gc1
		where gc1.efk_nrosolicitud = @c_nrosolicitud 
		group by gc1.efk_garantia_oportunidadId 
	select 
			@garsol		=isnull(gc.efk_valor_liquidable_avaluo,0),
			@monsol		=isnull(op.efk_monto_solicitado,0),
			@cartgarhip	=isnull(ac.efk_total_cartera_garantia_hipotecaria,0),
			@cartgarper	=isnull(ac.efk_total_cartera_garantia_personal,0),
			@cartsolfir	=isnull(ac.efk_total_cartera_asolafirma,0)
	from BMSC_MSCRM..Opportunity Op
		inner join BMSC_MSCRM..Account ac
		on ac.accountid = op.accountid 
		inner join BMSC_MSCRM..efk_garantia_credito gc
		on gc.efk_garantia_oportunidadId = op.OpportunityId 
    where op.efk_nrosolicitud = @c_nrosolicitud 
    
    set @c_totgar =  @garsol + @garconant
    set @c_cobtotgar = @c_totgar/(@cartgarhip+@cartgarper+@cartsolfir + @monsol)
    
end;
GO