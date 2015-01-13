/****** Object:  StoredProcedure [dbo].[spDatosTipoTrabajoCliente]    Script Date: 08/27/2013 11:35:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[spDatosTipoTrabajoCliente] (@codTipoTrabajoCRM    varchar(20) = null,                      
                                           @error         varchar(200) output)
As
begin

               select
               sCodigoValorBdi as 'valor'
               from BDI_CRM_MapeoPicklistValores
               where iCodigoMapeo = 6 and iCodigoValorCrm = @codTipoTrabajoCRM

end;
GO
