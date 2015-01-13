/****** Object:  UserDefinedFunction [dbo].[CalculaContingenteMargenesCredito]    Script Date: 08/27/2013 11:36:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[CalculaContingenteMargenesCredito] (@accountid uniqueidentifier)
RETURNS MONEY
AS
BEGIN
    DECLARE @contingenteMC AS MONEY 
    Declare @efk_monto_aprobado as money
    Declare @efk_monto_utilizado as money
    
    Set @contingenteMC = 0

    Declare cur_margenes_credito Cursor
	For
	Select mc.efk_monto, mc.efk_monto_utilizado
	from BMSC_MSCRM..efk_margen_credito mc 
	where mc.efk_accountId = @accountid
	and mc.efk_estado <> 221220001  --Indica sólo márgenes de crédito vigentes
	
	Open cur_margenes_credito
	fetch cur_margenes_credito into @efk_monto_aprobado,@efk_monto_utilizado
	
	WHILE (@@FETCH_STATUS = 0)
	Begin	
	    Set @contingenteMC = @contingenteMC + (@efk_monto_aprobado - @efk_monto_utilizado)	
	    fetch cur_margenes_credito into @efk_monto_aprobado,@efk_monto_utilizado
	End	
	
	Close cur_margenes_credito
	Deallocate cur_margenes_credito	

    RETURN Coalesce(@contingenteMC , 0)
END;
GO