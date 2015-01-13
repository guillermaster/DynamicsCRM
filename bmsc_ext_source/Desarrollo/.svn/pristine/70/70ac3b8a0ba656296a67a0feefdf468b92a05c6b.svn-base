USE [BDI_CRM]
GO
/****** Object:  StoredProcedure [dbo].[sp_sincronizar_moneda_cliente]    Script Date: 02/19/2014 17:53:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_sincronizar_moneda_cliente] 
	(@iClienteCodigo int, @sCodigoMoneda nvarchar(4), @sCodigoMonedaDefault nvarchar(4) = 'USD')
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @codigoCliente INT,@codigoMoneda nvarchar(4);
	DECLARE @cursor CURSOR;
	DECLARE @existe int = 0;
	SET @cursor = CURSOR FOR
	SELECT iCodigoClienteBanco,sCodigoMoneda
	FROM BDI_InfoAdicionalClientes
	where iCodigoClienteBanco = @iClienteCodigo
	OPEN @cursor
	FETCH NEXT
	FROM @cursor INTO @codigoCliente,@codigoMoneda
	WHILE @@FETCH_STATUS = 0
	BEGIN
		if ( @sCodigoMoneda = @sCodigoMonedaDefault )
			delete from BDI_InfoAdicionalClientes
			where iCodigoClienteBanco =@iClienteCodigo;
		else 
			update BDI_InfoAdicionalClientes
			set sCodigoMoneda = @sCodigoMoneda
			where iCodigoClienteBanco =@iClienteCodigo;
		--PRINT @codigoCliente
		set @existe = 1;
		FETCH NEXT
		FROM @cursor INTO @codigoCliente,@codigoMoneda
	END
	CLOSE @cursor
	DEALLOCATE @cursor
	
	if (@existe = 0)
		insert into BDI_InfoAdicionalClientes(iCodigoClienteBanco,sCodigoMoneda)
		Values(@iClienteCodigo,@sCodigoMoneda);
	
END
