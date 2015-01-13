/****** Object:  StoredProcedure [dbo].[spDatosTipoCliente]    Script Date: 08/27/2013 11:35:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[spDatosTipoCliente] (@codTipoClienteCRM    varchar(20) = null,                      
                                    @error                varchar(200) output)
As
begin

               select
               sCodigoValorBdi as 'valor'
               from BDI_CRM_MapeoPicklistValores
               where iCodigoMapeo = 10 and iCodigoValorCrm = @codTipoClienteCRM

end;
GO