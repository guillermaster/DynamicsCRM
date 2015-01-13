
USE [BDI_CRM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Jessenia Zavala>
-- Description:	Funcion que valida si un cliente 
--              Tiene o no boletas de Garantías
-- =============================================
CREATE FUNCTION [dbo].[fn_sp_boleta_garantia] 
(
	-- Add the parameters for the function here
	@u_accountID uniqueidentifier
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @i_boleta int,
	        @i_tiene_boleta int

	-- Senetencia para verificar que el cliente a consultar tiene boleta de garantía
	set @i_tiene_boleta =(select COUNT(*)
	from BMSC_MSCRM..efk_producto_activo t
	where t.efk_cliente_juridico_Id=@u_accountID
	and t.efk_clase_producto_banco = 221220004);--Código que identifica el producto de Boletas de Garantía--
	
	if @i_tiene_boleta >=1
		begin 
			set @i_boleta = 1; --Tiene boleta
		end
	else 
		begin
			set @i_boleta = 0; -- No Tiene boleta
		end

	-- Return the result of the function
	RETURN @i_boleta

END
GO

